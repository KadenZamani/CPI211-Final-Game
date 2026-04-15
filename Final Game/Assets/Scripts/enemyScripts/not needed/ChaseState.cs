using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    private float attackCooldown = 0f;
    NavMeshAgent agent;
    Transform player;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player= GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = 3.5f;
        attackCooldown = 0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
   override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackCooldown += Time.deltaTime;
        agent.SetDestination(player.position);
        float distance = Vector3.Distance(player.position, animator.transform.position);

        if (distance < 1.5f)
        {
           agent.SetDestination(animator.transform.position);
        }

        if (distance > 15)
        {
            animator.SetBool("isChasing", false);
        }
        if (distance < 2.5f && attackCooldown >= 5f)
        {
           
            animator.SetTrigger("attack");
            attackCooldown = 0f;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
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
