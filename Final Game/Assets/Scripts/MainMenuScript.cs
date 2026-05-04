using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    [Header("Text Buttons")]
    public TextMeshProUGUI playText;
    public TextMeshProUGUI controlsText;

    [Header("Scenes")]
    public string playSceneName = "IntroScreen";
    public string controlsSceneName = "ControlsScreen";

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;

    void Update()
    {
        CheckHover();

        if (Input.GetMouseButtonDown(0))
        {
            CheckClick();
        }
    }

    void CheckHover()
    {
        Vector2 mousePos = Input.mousePosition;

        // Reset colors
        playText.color = normalColor;
        controlsText.color = normalColor;

        // Highlight hovered option
        if (RectTransformUtility.RectangleContainsScreenPoint(
            playText.rectTransform, mousePos, null))
        {
            playText.color = hoverColor;
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(
            controlsText.rectTransform, mousePos, null))
        {
            controlsText.color = hoverColor;
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