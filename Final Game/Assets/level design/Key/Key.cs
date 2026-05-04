using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CharacterStateController player =
            other.GetComponent<CharacterStateController>();

        if (player != null)
        {
            player.addKey();
            Destroy(gameObject);
        }
    }
}
