using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;

public class EnemyStateController : MonoBehaviour
{
    public enum State
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead,
        readyAttack
    }

    public State currentState;

    [Header("References")]
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;
    public Slider healthBar;
    public GameObject attack;

    [Header("Stats")]
    public int HP = 100;

    [Header("Ranges")]
    public float chaseRange = 8f;
    public float attackRange = 0.5f;
    public float attackCooldownMax = 4f;
    float idleTimer;
    float patrolTimer;
    float walkTimer;
    float attackCooldown;
    public bool isPerformingAction = false;


    Vector3 patrolDestination;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        
        attackCooldown = 0f; 
        attack.GetComponent<Collider>().enabled = false;
        currentState = State.Idle;
    }

    void Update()
    {
        attackCooldown += Time.deltaTime;
       
        if (currentState == State.Dead) return;

        float distance = Vector3.Distance(player.position, transform.position);

        switch (currentState)
        {
            case State.Idle:
                Idle(distance);
                break;

            case State.Patrol:
                Patrol(distance);
                break;

            case State.Chase:
                Chase(distance);
                break;

            case State.Attack:
                //Attack(distance);
                break;

            case State.readyAttack:
                ReadyAttack(distance);
                break;
        }
    }



    //Attack Collider functions
    public void EnableCollider()
    {
        attack.GetComponent<Collider>().enabled = true;
    }

    public void DisableCollider()
    {
        attack.GetComponent<Collider>().enabled = false;
    }





    // ---------------- STATES ----------------

    void Idle(float distance)
    {
        idleTimer += Time.deltaTime;
        animator.SetBool("isPatrolling", false);
        animator.SetBool("isChasing", false);
        agent.SetDestination(transform.position);
        if (distance < chaseRange)
        {
            ChangeState(State.Chase);
        }
        else if (idleTimer > 5f)
        {
            ChangeState(State.Patrol);
        }
    }

    void Patrol(float distance)
    {
        animator.SetBool("isPatrolling", true);
        animator.SetBool("isChasing", false);
        agent.speed = 1.5f;
        patrolTimer += Time.deltaTime;
        walkTimer -= Time.deltaTime;

        if (walkTimer <= 0)
        {
            Vector2 random = Random.insideUnitCircle * 10f;
            patrolDestination = transform.position + new Vector3(random.x, 0, random.y);
            agent.SetDestination(patrolDestination);
            walkTimer = 3f;
        }

        if (distance < chaseRange)
        {
            ChangeState(State.Chase);
        }
        else if (patrolTimer > 10f)
        {
            ChangeState(State.Idle);
        }
    }

    void Chase(float distance)
    {
        

        agent.speed = 3.5f;
        agent.SetDestination(player.position);
        animator.SetBool("isChasing", true);
       
        if (distance <= attackRange )
        {
            ChangeState(State.readyAttack);
        }
        else if (distance > 15f)
        {
            ChangeState(State.Idle);
        }

        
    }

    void Attack(float distance)
    {

        


        agent.SetDestination(transform.position);
        transform.LookAt(player);



        // Enable hitbox during attack window

       




        attackCooldown = 0f;
           
            ChangeState(State.readyAttack);
        
        
    }

    IEnumerator PerformAttack(float duration, float distance)
    {
        isPerformingAction = true;
        ChangeState(State.Attack);
        Attack(distance);
        // Wait for the animation to finish
        yield return new WaitForSeconds(duration);


        ChangeState(State.readyAttack);
        isPerformingAction = false;

    }
    void ReadyAttack(float distance)
    {
        agent.SetDestination(transform.position);
       animator.SetBool("readyAttack", true);

        if (distance <= attackRange && attackCooldown >= attackCooldownMax && !isPerformingAction)
        {
            StartCoroutine(PerformAttack(1f, distance));

        }
        else if (distance >= attackRange)
        {
            ChangeState(State.Chase);
            animator.SetBool("readyAttack", false);
        }

        if (distance <= attackRange)
        {
            agent.SetDestination(transform.position);
            transform.LookAt(player);
        }
    }

    // ---------------- DAMAGE ----------------

    public void takeDamage(int damageAmount)
    {
        //if (currentState == State.Dead) return;

        HP -= damageAmount;
        healthBar.value = HP;

        if (HP <= 0)
        {
            currentState = State.Dead;
            DisableCollider();
            agent.enabled = false;
            animator.SetTrigger("die");
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 10f);
        }
        else
        {
            DisableCollider();
            animator.SetTrigger("damage");
            

        }
    }

    // ---------------- STATE SWITCH ----------------

    void ChangeState(State newState)
    {
        // Reset timers when entering states
        idleTimer = 0;
        patrolTimer = 0;
       

        if (newState == State.Attack)
        {
            animator.SetTrigger("attack");
        }

        currentState = newState;
    }
}