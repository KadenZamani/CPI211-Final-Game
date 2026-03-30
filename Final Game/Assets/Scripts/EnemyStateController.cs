using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    [SerializeField] public int enemyHealth = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Enemy health: " + enemyHealth);
    }
}
