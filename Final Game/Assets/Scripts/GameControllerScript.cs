using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameControllerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] public bool key1 = false;
    [SerializeField] public bool key2 = false;
    [SerializeField] public bool key3 = false;
    [SerializeField] private EnemyStateController miniboss1;
    [SerializeField] private EnemyStateController miniboss2;
    [SerializeField] private EnemyStateController miniboss3;
    [SerializeField] private GameObject minibossObject1;
    [SerializeField] private GameObject minibossObject2;
    [SerializeField] private GameObject minibossObject3;



    void Start()
    {
        
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
      

       

    }

   
}
