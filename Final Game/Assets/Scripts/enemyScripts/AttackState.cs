using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    HitboxAccessor attack;
    Transform player;
    [SerializeField] float timer;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        attack = animator.GetComponent<HitboxAccessor>();
        attack.hitbox.GetComponent<Collider>().enabled = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer > 3f)
        {
            timer = 0;
            animator.SetBool("isAttacking", false);
            
        }

        if (timer > 1.5f)
        {
            attack.hitbox.GetComponent<Collider>().enabled = false;
        }

            animator.transform.LookAt(player);
        float distance = Vector3.Distance(player.position, animator.transform.position);
        //if (distance > 3.5f)
        //{
       //     animator.SetBool("isAttacking", false);
      // }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attack.hitbox.GetComponent<Collider>().enabled = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
