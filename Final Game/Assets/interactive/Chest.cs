using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Chest : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float openRadius = 3f;
    [SerializeField] private LayerMask detectionLayer;

    [Header("Animation")]
    [SerializeField] private string animatorParameter = "open";
    [SerializeField] private float useDelay;

    [Header("Treasure")]
    public int healAmount = 25;
    public bool useOnlyOnce = true;

    private Animator animator;
    private bool isOpen = false;
    private bool isOpening = false;
    private bool beenUsed = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, openRadius, detectionLayer);
        
        bool shouldOpen = hits.Length > 0;

        if (shouldOpen != isOpen)
        {
            isOpen = shouldOpen;
            animator.SetBool(animatorParameter, isOpen);
        }

        if (shouldOpen && !isOpening && (!useOnlyOnce || !beenUsed))
        {
            foreach (var hit in hits)
            {
                var controller = hit.GetComponent<CharacterStateController>();
                if (controller != null)
                {
                    StartCoroutine(UseAfterDelay(controller));
                    beenUsed = true;
                    break; // stop after healing first valid target
                }
            }
        }
    }

    IEnumerator UseAfterDelay(CharacterStateController controller)
    {
        isOpening = true;

        yield return new WaitForSeconds(useDelay);

        if (controller != null)
        {
            controller.GainHealth(healAmount);
            beenUsed = true;
        }

        isOpening = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, openRadius);
    }
}
