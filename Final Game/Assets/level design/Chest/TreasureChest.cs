using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class TreasureChest : MonoBehaviour
{
    [Header("Detection Settings")]
    public float openRadius = 1.5f;
    public LayerMask detectionLayer;

    [Header("Animation")]
    public string openParameter = "isOpen";

    [Header("Healing")]
    public float healAmount = 25f;
    public bool healOnlyOnce = true;
    public float healDelay = 1.5f; // delay in seconds

    private Animator animator;
    private bool isOpen = false;
    private bool hasHealed = false;
    private bool isHealing = false;

    void Awake()
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
            animator.SetBool(openParameter, isOpen);
        }

        // Start delayed heal
        if (shouldOpen && !isHealing && (!healOnlyOnce || !hasHealed))
        {
            foreach (var hit in hits)
            {
                var controller = hit.GetComponent<CharacterStateController>();
                if (controller != null)
                {
                    StartCoroutine(HealAfterDelay(controller));
                    isHealing = true;
                    break;
                }
            }
        }
    }

    IEnumerator HealAfterDelay(CharacterStateController controller)
    {
        yield return new WaitForSeconds(healDelay);

        if (controller != null)
        {
            controller.GainHealth(healAmount);
            hasHealed = true;
        }

        isHealing = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, openRadius);
    }
}