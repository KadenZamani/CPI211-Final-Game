using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

public enum MoveState { Idle, Walking, Sprinting }
public enum ActionState { None, Blocking, Attacking, GettingHit, Dying }

public class CharacterStateController : MonoBehaviour
{
    
    public MoveState currentMove = MoveState.Idle;
    public ActionState currentAction = ActionState.None;
    [SerializeField] public FadeScript fader;
    [SerializeField] public CameraController cam;
    public ThirdPersonController ScriptReference;
    private bool isPerformingAction = false;
    private bool isGettingHit = false;
    private bool isDying = false;
    public bool attackHitboxActive = false;
    public Slider healthBar;
    private int playerHealth = 100;
    public Slider staminaBar;
    private float stamina = 100;
    private float maxStamina = 100;
    private float regenDelay = 1f;
    private float regenRate = 20f;
    private float lastUsedTime;
    private Coroutine regenCoroutine;
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

    private void RegisterStaminaUsage()
    {
        lastUsedTime = Time.time; // Mark the exact second we used stamina

        // If we are currently regenerating, stop it. We need to wait for the delay again.
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
        }

        // Start a watcher that waits for the delay to pass
        regenCoroutine = StartCoroutine(RegenRoutine());
    }

    private IEnumerator RegenRoutine()
    {
       
        while (Time.time < lastUsedTime + regenDelay)
        {
            yield return null;
        }

        // Start Refilling
        while (stamina < maxStamina)
        {
            stamina += regenRate * Time.deltaTime;
            stamina = Mathf.Min(stamina, maxStamina);
            yield return null;
        }

        regenCoroutine = null;
    }

    void HandleInput()
    {
        // 1. Determine Action First
        if (isDying) return;
        if (isGettingHit && !isDying) return;
        if (isPerformingAction && !isDying) return;
        if (Input.GetMouseButton(1) && stamina >= 1)
        {
            currentAction = ActionState.Blocking;
           
        }
        else if (Input.GetMouseButtonDown(0) && stamina >= 20)
        {
            StartCoroutine(PerformAttack(1.1f)); // 1.7s is the length of attack
        }
        else currentAction = ActionState.None;

        float moveInput1 = Input.GetAxis("Vertical"); // W/S keys
        float moveInput2 = Input.GetAxis("Horizontal"); // A/D keys
        bool isMoving = Mathf.Abs(moveInput1) > 0.1f || Mathf.Abs(moveInput2) > 0.1f;
        

        if (!isMoving)
        {
            currentMove = MoveState.Idle;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && currentAction == ActionState.None && stamina > 1)
        {
            // Only sprint if not attacking or blocking
            currentMove = MoveState.Sprinting;
        }
        else
        {
            currentMove = MoveState.Walking;
        }
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
        if(isDying)
        {
            ScriptReference.velocity = 0f;
        }
        //UpdateAnimator();
        // Debug.Log("Current Move State: " + currentMove.ToString());
        Debug.Log("Current Action State: " + currentAction.ToString());
        Debug.Log("Health: " + playerHealth);
    }

    IEnumerator PerformAttack(float duration)
    {
        isPerformingAction = true;
        currentAction = ActionState.Attacking;
        stamina -= 15;
        RegisterStaminaUsage();
        
        anim.SetTrigger("attack");

        // Wait for the animation to finish
        yield return new WaitForSeconds(duration);

        
        currentAction = ActionState.None;
        isPerformingAction = false;
        
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
        
        anim.SetTrigger("isDead");
        if (cam != null)
        {
            cam.isLocked = true;
        }


        // Wait for the animation to finish
        yield return new WaitForSeconds(duration);


        //you died pop up and reset scene
        StartCoroutine(fader.FadeToBlack());
        yield return new WaitForSeconds(2);
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex);
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
                StartCoroutine(Die(6f));
            }
            else
            {
                StartCoroutine(GetHit(1.2f));
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
                RegisterStaminaUsage();
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
                anim.SetBool("isBlocking", false);
                break;
            case ActionState.Blocking:
                //make player block
                ScriptReference.velocity = 1f;
                attackHitboxActive = false;
                anim.SetBool("isBlocking", true);
                stamina -= 20 * Time.deltaTime;
                RegisterStaminaUsage();
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
