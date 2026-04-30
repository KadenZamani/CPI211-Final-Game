using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;
using BigRookGames.Weapons;

public class BossStateController : MonoBehaviour
{
    public RxAttackVariable RxAttack;
    public EnemyStateController enemyController;
    public GunfireController gunController1;
    public GunfireController gunController2;
    public GunfireController gunController3;
    private bool phaseChange = false;
    public Animator rexAnim;

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

        if (enemyController.HP < 50 && !phaseChange)
        {
            phaseChange = true;
            StartCoroutine(stateTransition());

        }


    }

    IEnumerator stateTransition()
    {
       
        enemyController.enabled = false;
        RxAttack.b = true;
        //rexAnim.Play("Roar");
        StartCoroutine(Barrage(0.3f));
        
        yield return new WaitForSeconds(6f);
        enemyController.enabled = true;
        enemyController.currentState = EnemyStateController.State.Chase;


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
}