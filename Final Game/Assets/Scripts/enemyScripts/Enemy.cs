using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public int HP = 100;
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public void takeDamage(int damageAmount)
    {
               HP -= damageAmount;
        if (HP <= 0)
        {
           animator.SetTrigger("die");
            GetComponent<Collider>().enabled = false; // Disable collider to prevent further interactions
             Destroy(gameObject, 10f); // Destroy the enemy after a delay to allow death animation to play
        }
        else
        {
            animator.SetTrigger("damage");
        }
    }
}
