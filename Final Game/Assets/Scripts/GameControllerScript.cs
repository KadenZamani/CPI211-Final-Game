using System;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] public bool key1 = false;
    [SerializeField] public bool key2 = false;
    [SerializeField] public bool key3 = false;
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
