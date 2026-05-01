using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTimer : MonoBehaviour
{

    [SerializeField] private float damageDuration = 0.2f; 
    private Collider myCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myCollider = GetComponent<Collider>();
        // Start the countdown to disable the collider
        Invoke("DisableCollider", damageDuration);
    }

    void DisableCollider()
    {
        if (myCollider != null)
        {
            myCollider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
}
