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
    public float healDelay = 1.5f;
    public bool healOnlyOnce = true;

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
        Collider[] hits =
            Physics.OverlapSphere(transform.position, openRadius, detectionLayer);

        bool shouldOpen = false;
        CharacterStateController validPlayer = null;

        foreach (Collider hit in hits)
        {
            CharacterStateController controller =
                hit.GetComponent<CharacterStateController>();

            if (controller != null && controller.hasKey())
            {
                shouldOpen = true;
                validPlayer = controller;

                // Open only if player with key is nearby
                if (shouldOpen != isOpen)
                {
                    isOpen = shouldOpen;
                    animator.SetBool(openParameter, isOpen);
                }

                // Heal after opening
                if (shouldOpen &&
                    validPlayer != null &&
                    !isHealing &&
                    (!healOnlyOnce || !hasHealed))
                {
                    StartCoroutine(HealAfterDelay(validPlayer));
                    isHealing = true;
                    controller.useKey();
                }
                break;
            }
        }
    }

    IEnumerator HealAfterDelay(CharacterStateController player)
    {
        yield return new WaitForSeconds(healDelay);

        if (player != null)
        {
            float dist = Vector3.Distance(
                transform.position,
                player.transform.position
            );

            if (dist <= openRadius)
            {
                player.GainHealth(healAmount);
                hasHealed = true;
            }
        }

        isHealing = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, openRadius);
    }
}