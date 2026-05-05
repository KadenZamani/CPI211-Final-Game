using UnityEngine;

public class DeleteGates : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject GameController;
    public GameControllerScript controller;
    public GameObject gate1;
    public GameObject gate2;
    public GameObject gate3;
    public GameObject gate4;
    void Start()
    {
        controller = GameControllerScript.Instance;
        gate3.SetActive(false);
        gate4.SetActive(false);
    }

    private void Awake()
    {

       
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (controller.key1 && controller.key2 && controller.key3)
        {
            if (gate1 != null && gate2 != null)
            {
                gate1.SetActive(false);
                gate2.SetActive(false);
            }
                
            gate3.SetActive(true);
            gate4.SetActive(true);
        }
    }
}
