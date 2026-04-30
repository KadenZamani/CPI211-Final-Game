using UnityEngine;
using UnityEngine.SceneManagement;
public class ControlsScreenScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void GoToGameScene()
    {
        SceneManager.LoadScene("IntroScreen");
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            GoToGameScene();
        }
    }
}
