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

        if (enemyController.HP < 200 && !phaseChange)
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


    }

    IEnumerator stateTransition()
    {
       
        enemyController.enabled = false;
        RxAttack.b = true;
        
        StartCoroutine(Barrage(0.5f));
        
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