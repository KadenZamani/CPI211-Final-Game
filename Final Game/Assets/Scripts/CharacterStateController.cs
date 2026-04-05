using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public enum MoveState { Idle, Walking, Sprinting }
public enum ActionState { None, Blocking, Attacking, GettingHit, Dying }

public class CharacterStateController : MonoBehaviour
{
    
    public MoveState currentMove = MoveState.Idle;
    public ActionState currentAction = ActionState.None;

    public ThirdPersonController ScriptReference;
    private bool isPerformingAction = false;
    private bool isGettingHit = false;
    private bool isDying = false;
    public bool attackHitboxActive = false;
    public Slider healthBar;
    private int playerHealth = 100;
    public Slider staminaBar;
    private float stamina = 100;
    private int damageDealtToPlayer = 0;
     public Animator anim;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        staminaBar.maxValue = 100;
        staminaBar.value = stamina;
        healthBar.maxValue = 100;
        healthBar.value = playerHealth;
    }

    void HandleInput()
    {
        // 1. Determine Action First
        if (isDying) return;
        if (isGettingHit && !isDying) return;
        if (isPerformingAction && !isDying) return;
        if (Input.GetMouseButton(1) && stamina >= 1) currentAction = ActionState.Blocking;
        else if (Input.GetMouseButtonDown(0) && stamina >= 20)
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
        else if (Input.GetKey(KeyCode.LeftShift) && currentAction == ActionState.None && stamina > 0)
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

    void UpdateStamina()
    {
        if (stamina < 0)
        {
            stamina = 0;
        }
        if (stamina > 100)
        {
            stamina = 100;
        }
        if(currentMove != MoveState.Sprinting && currentAction == ActionState.None)
        {
            stamina += 20 * Time.deltaTime;
        }
        staminaBar.value = stamina;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        ExecuteMovementLogic();
        ExecuteActionLogic();
        UpdateStamina();
        healthBar.value = playerHealth;
        //UpdateAnimator();
        // Debug.Log("Current Move State: " + currentMove.ToString());
        Debug.Log("Current Action State: " + currentAction.ToString());
        Debug.Log("Health: " + playerHealth);
    }

    IEnumerator PerformAttack(float duration)
    {
        isPerformingAction = true;
        currentAction = ActionState.Attacking;
        stamina -= 20;
        
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

    IEnumerator Die(float duration)
    {
        isDying = true;
        currentAction = ActionState.Dying;
        //lock camera
        anim.SetTrigger("isDead");


        // Wait for the animation to finish
        yield return new WaitForSeconds(duration);


        //uou died pop up and reset scene
        

       
    }

    //detecting enemy attack hitbox
    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("EnemyAttack") && !isGettingHit && !isDying && currentAction != ActionState.Blocking) 
        {
            EnemyAttack enemyScript = other.gameObject.GetComponent<EnemyAttack>();
            damageDealtToPlayer = enemyScript.damage;
            playerHealth = playerHealth - damageDealtToPlayer;
            if (playerHealth < 1)
            {
                StartCoroutine(Die(12f));
            }
            else
            {
                StartCoroutine(GetHit(0.3f));
            }
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
                stamina -= 20 * Time.deltaTime;
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
                stamina -= 20 * Time.deltaTime;
                break;
            case ActionState.Attacking:
                //make player attack
                ScriptReference.velocity = 1f;
                attackHitboxActive = true;
                break;
            case ActionState.GettingHit:
                ScriptReference.velocity = 1f;
                break;
            case ActionState.Dying:
                ScriptReference.velocity = 0f;
                break;

        }
    }

}
