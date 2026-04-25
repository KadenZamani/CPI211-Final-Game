using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;

public class BossStateController : MonoBehaviour
{
    public RxAttackVariable RxAttack;
    public EnemyStateController enemyController;

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
    }
}