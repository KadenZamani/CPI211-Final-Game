using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] public int damage = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("Hea");
            StartCoroutine(DisableHitbox(0.8f));
        }
    }
    //duration  could be what ever we want to balance the attack
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
