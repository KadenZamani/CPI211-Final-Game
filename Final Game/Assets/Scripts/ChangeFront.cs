using UnityEngine;

public class ChangeFront : MonoBehaviour
{
    Vector3 newDirection = new Vector3(0f, 0f, -1f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.forward = -transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
