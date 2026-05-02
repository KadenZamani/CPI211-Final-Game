using UnityEngine;

public class MiniBossScript : MonoBehaviour

{
    [SerializeField] int bossNumber;


    //public GameControllerScript GCScript;
   public EnemyStateController ESController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  

    [SerializeField] private string enemyID; // Give each enemy a unique name in the Inspector

    void Start()
    {
        // Check with the Controller if I should exist
        if (GameControllerScript.Instance != null && GameControllerScript.Instance.IsEnemyDead(enemyID))
        {
            Destroy(gameObject);
        }
    }

    public void Die()
    {
        // Tell the Controller to remember this death
        if (GameControllerScript.Instance != null)
        {
            GameControllerScript.Instance.MarkEnemyAsDead(enemyID);
        }

        // Normal death logic
       // Destroy(gameObject);
    }

    private void Update()
    {
        if(ESController.HP <= 0)
        {
            
            Die();
        }
    }



    // Update is called once per frame


}
