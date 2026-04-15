using UnityEngine;

public class DeleteGates : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameControllerScript controller;
    public GameObject gate1;
    public GameObject gate2;
    public GameObject gate3;
    public GameObject gate4;
    void Start()
    {
        gate3.SetActive(false);
        gate4.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           if(controller.key1 && controller.key2 && controller.key3)
            {
                Destroy(gate1);
                Destroy(gate2);
                gate3.SetActive(true);
                gate4.SetActive(true);
            }
        }
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}
