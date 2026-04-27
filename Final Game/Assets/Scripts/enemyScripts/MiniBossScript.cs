using UnityEngine;

public class MiniBossScript : MonoBehaviour

{
    [SerializeField] int bossNumber;
    public GameControllerScript GCScript;
    public EnemyStateController ESController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (ESController.HP <= 0 && bossNumber == 1)
        {
            GCScript.key1 = true;
        }
        if (ESController.HP <= 0 && bossNumber == 2)
        {
            GCScript.key2 = true;
        }
        if (ESController.HP <= 0 && bossNumber == 3)
        {
            GCScript.key3 = true;
        }



        if (GCScript.key1 == true && bossNumber == 1)
        {
            Destroy(gameObject);
        }
        if (GCScript.key2 == true && bossNumber == 2)
        {
            Destroy(gameObject);
        }
        if (GCScript.key3 == true && bossNumber == 3)
        {
            Destroy(gameObject);
        }
    }
}
