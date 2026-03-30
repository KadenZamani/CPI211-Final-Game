using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public CharacterStateController ScriptReference;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Enemy"))
        {
            if (ScriptReference.attackHitboxActive)
            {
                //do damage
                ScriptReference.attackHitboxActive = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
