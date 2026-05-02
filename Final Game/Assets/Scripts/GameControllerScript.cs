using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameControllerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] public bool key1 = false;
    [SerializeField] public bool key2 = false;
    [SerializeField] public bool key3 = false;
   // [SerializeField] private EnemyStateController miniboss1;
  //  [SerializeField] private EnemyStateController miniboss2;
  //  [SerializeField] private EnemyStateController miniboss3;
   // [SerializeField] private GameObject minibossObject1;
   // [SerializeField] private GameObject minibossObject2;
  //  [SerializeField] private GameObject minibossObject3;

    public static GameControllerScript Instance;

    // This stores our "keys" (EnemyID, isDead)
    private HashSet<string> deadEnemies = new HashSet<string>();

    void Awake()
    {
        // Singleton pattern: ensures only one Controller exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MarkEnemyAsDead(string id)
    {
        if (!deadEnemies.Contains(id))
        {
            deadEnemies.Add(id);
        }
    }

    public bool IsEnemyDead(string id)
    {
        return deadEnemies.Contains(id);
    }

    void Start()
    {
        
    }

  

    // Update is called once per frame
    void Update()
    {
      if(deadEnemies.Contains("miniboss1"))
        {
            key1 = true;
        }
      if (deadEnemies.Contains("miniboss2"))
        {
            key2 = true;
        }
      if (deadEnemies.Contains("miniboss3"))
        {
            key3 = true;
        }



    }

   
}
