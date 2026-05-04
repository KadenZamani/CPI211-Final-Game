using UnityEngine;
using UnityEngine.SceneManagement;
public class EnterPortal : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Portal"))
        {
            SceneManager.LoadScene("BossFight");
        }
    }
        // Update is called once per frame
        void Update()
    {
        
    }
}
