using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] public int damage = 20;
    public CharacterStateController ScriptReference;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // duration is player attack cooldown
            StartCoroutine(DisableHitbox(0.8f));
        }

        if (other.CompareTag("Enemy"))
        {
            if (ScriptReference.attackHitboxActive)
            {
                other.GetComponent<Enemy>().takeDamage(damage);
                ScriptReference.attackHitboxActive = false;
            }
        }
    }

    IEnumerator DisableHitbox(float duration)
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(duration);
        GetComponent<Collider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
