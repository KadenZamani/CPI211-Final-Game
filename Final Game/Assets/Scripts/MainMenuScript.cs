using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class MainMenuScript : MonoBehaviour
{
    [Header("Text Buttons")]
    public TextMeshProUGUI playText;
    public TextMeshProUGUI controlsText;

    [Header("Scenes")]
    public string playSceneName = "IntroScreen";
    public string controlsSceneName = "ControlsScreen";

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckClick();
        }
    }

    void CheckClick()
    {
        Vector2 mousePos = Input.mousePosition;

        if (RectTransformUtility.RectangleContainsScreenPoint(
            playText.rectTransform, mousePos, null))
        {
            SceneManager.LoadScene(playSceneName);
            return;
        }

        if (RectTransformUtility.RectangleContainsScreenPoint(
            controlsText.rectTransform, mousePos, null))
        {
            SceneManager.LoadScene(controlsSceneName);
        }
    }
}