using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;
using BigRookGames.Weapons;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class BossStateController : MonoBehaviour
{
    public RxAttackVariable RxAttack;
    public bool dead = false;
    public EnemyStateController enemyController;
    public GunfireController gunController1;
    public GunfireController gunController2;
    public GunfireController gunController3;
    public NavMeshAgent agent;
    [SerializeField] public FadeScript fader;
    public GameObject explode;
    public GameObject boss;
    public GameObject rpg1;
    public GameObject rpg2;
    public GameObject rpg3;
    public float duration = 1.0f; // How many seconds the rotation takes
    private bool isRotating = false;
    private bool phaseChange = false;
    public Animator rexAnim;
    private bool missileMode = false;

    private void Update()
    {
        if(RxAttack.a)
        {
            enemyController.EnableCollider();
        }

        if(RxAttack.a == false)
        {
            enemyController.DisableCollider();
        }

        if (enemyController.HP <= 200 && !phaseChange)
        {
            phaseChange = true;
            StartCoroutine(stateTransition());

        }

        if (enemyController.HP < 130 && !missileMode)
        {
            missileMode = true;
            gunController3.FireWeapon();
            gunController3.autoFire = true;
        }
        if (enemyController.HP <= 0 && !dead)
        {
            dead = true;
            gunController1.autoFire = false;
            gunController2.autoFire = false;
            gunController3.autoFire = false;
            StartCoroutine(Die());
        }


    }

    IEnumerator stateTransition()
    {
       
        enemyController.enabled = false;
        RxAttack.b = true;
        
        StartCoroutine(Barrage(0.5f));
        
        yield return new WaitForSeconds(6f);
        enemyController.enabled = true;
        gunController1.shotDelay = 8;
        gunController2.shotDelay = 8;
        agent.speed = 2.5f;
        enemyController.currentState = EnemyStateController.State.Chase;


    }

    IEnumerator Die()
    {
        enemyController.enabled = false;
        StartRotation();
        yield return new WaitForSeconds(2f);
        Instantiate(explode, transform.position, transform.rotation);
        boss.SetActive(false);
        rpg1.SetActive(false);
        rpg2.SetActive(false);
        rpg3.SetActive(false);
        yield return new WaitForSeconds(2f);
        StartCoroutine(fader.FadeToBlack());
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("WinScreen");


    }

    public void StartRotation()
    {
        if (!isRotating)
        {
            StartCoroutine(RotateRoutine());
        }
    }

    IEnumerator Barrage(float delay)
    {
        yield return new WaitForSeconds(3f);
        gunController1.FireWeapon();
        yield return new WaitForSeconds(delay);
        gunController2.FireWeapon();
        yield return new WaitForSeconds(delay);
        gunController1.FireWeapon();
        yield return new WaitForSeconds(delay);
        gunController2.FireWeapon();
        yield return new WaitForSeconds(delay);
        gunController1.FireWeapon();
        yield return new WaitForSeconds(delay);
        gunController2.FireWeapon();
        yield return new WaitForSeconds(delay);

       

    }

   

    IEnumerator RotateRoutine()
    {
        isRotating = true;
        Debug.Log("Rotation Started!");

        Quaternion startRotation = transform.rotation;
        // Calculate the target: current rotation + 90 degrees on Z
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, 90);

        float elapsed = 0;

        while (elapsed < duration)
        {
            // Calculate how far we are through the animation (0.0 to 1.0)
            float t = elapsed / duration;

            // Apply the rotation
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);

            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure we land exactly on the target rotation at the end
        transform.rotation = endRotation;
        isRotating = false;
    }
}