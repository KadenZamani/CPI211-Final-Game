using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

public enum MoveState { Idle, Walking, Sprinting }
public enum ActionState { None, Blocking, Attacking, GettingHit }

public class CharacterStateController : MonoBehaviour
{
    
    public MoveState currentMove = MoveState.Idle;
    public ActionState currentAction = ActionState.None;

    public ThirdPersonController ScriptReference;
    private bool isPerformingAction = false;
    private bool isGettingHit = false;
    public bool attackHitboxActive = false;
    private int playerHealth = 100;
    private int damageDealtToPlayer = 0;
     public Animator anim;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void HandleInput()
    {
        // 1. Determine Action First
        if (isGettingHit) return;
        if (isPerformingAction) return;
        if (Input.GetMouseButton(1)) currentAction = ActionState.Blocking;
        else if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(PerformAttack(0.8f)); // 0.8s is the length of attack
        }
        else currentAction = ActionState.None;

        float moveInput1 = Input.GetAxis("Vertical"); // W/S keys
        float moveInput2 = Input.GetAxis("Horizontal"); // A/D keys
        bool isMoving = Mathf.Abs(moveInput1) > 0.1f || Mathf.Abs(moveInput2) > 0.1f;
        

        if (!isMoving)
        {
            currentMove = MoveState.Idle;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && currentAction == ActionState.None)
        {
            // Only sprint if not attacking or blocking
            currentMove = MoveState.Sprinting;
        }
        else
        {
            currentMove = MoveState.Walking;
        }
    }

    void UpdateAnimator()
    {
       // anim.SetInteger("MoveState", (int)currentMove);
       // anim.SetInteger("ActionState", (int)currentAction);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        ExecuteMovementLogic();
        ExecuteActionLogic();
        //UpdateAnimator();
       // Debug.Log("Current Move State: " + currentMove.ToString());
        Debug.Log("Current Action State: " + currentAction.ToString());
        Debug.Log("Health: " + playerHealth);
    }

    IEnumerator PerformAttack(float duration)
    {
        isPerformingAction = true;
        currentAction = ActionState.Attacking;

        
        //anim.SetInteger("ActionState", (int)currentAction);

        // Wait for the animation to finish
        yield return new WaitForSeconds(duration);

        
        currentAction = ActionState.None;
        isPerformingAction = false;
        //anim.SetInteger("ActionState", (int)currentAction);
    }

    IEnumerator GetHit(float duration)
    {
        isGettingHit = true;
        currentAction = ActionState.GettingHit;

        anim.SetTrigger("isHit");


        // Wait for the animation to finish
        yield return new WaitForSeconds(duration);


        currentAction = ActionState.None;
        isGettingHit = false;
        
        //anim.SetInteger("ActionState", (int)currentAction);
    }

    //detecting enemy attack hitbox
    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("EnemyAttack") && !isGettingHit && currentAction != ActionState.Blocking) 
        {
            EnemyAttack enemyScript = other.gameObject.GetComponent<EnemyAttack>();
            damageDealtToPlayer = enemyScript.damage;
            playerHealth = playerHealth - damageDealtToPlayer;
            StartCoroutine(GetHit(0.3f));
        }
    }

    void ExecuteMovementLogic()
    {
        switch (currentMove)
        {
            case MoveState.Idle:
                //Do nothing
                break;
            case MoveState.Walking:
                //walking code is already done in ThirdPersonController
                break;
            case MoveState.Sprinting:
                //sprint code is already done in ThirdPersonController
                break;
        }
    }

    void ExecuteActionLogic()
    {
        switch (currentAction)
        {
            case ActionState.None:
                //do nothing
                ScriptReference.velocity = 5f;
                attackHitboxActive = false;
                break;
            case ActionState.Blocking:
                //make player block
                ScriptReference.velocity = 1f;
                attackHitboxActive = false;
                break;
            case ActionState.Attacking:
                //make player attack
                ScriptReference.velocity = 1f;
                attackHitboxActive = true;
                break;
            case ActionState.GettingHit:
                ScriptReference.velocity = 1f;
               

                break;

        }
    }

}
