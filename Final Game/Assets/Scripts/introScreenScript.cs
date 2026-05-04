using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroScreenScript : MonoBehaviour
{
    [Header("Scene")]
    public string gameSceneName = "SampleScene";

    [Header("Credits Text")]
    public TextMeshProUGUI creditsText;     // Opening crawl text
    public float scrollDistance = 400f;
    public float scrollDuration = 10f;
    public float fadeInTime = 2f;
    public float fadeOutTime = 2f;

    [Header("Continue Prompt")]
    public TextMeshProUGUI continueText;    // "Press J to continue"
    public float promptFadeInTime = 2f;

    private Vector3 creditsStartPos;
    private Vector3 creditsEndPos;

    private float timer = 0f;
    private bool creditsFinished = false;

    void Start()
    {
        // Save starting position
        creditsStartPos = creditsText.rectTransform.localPosition;
        creditsEndPos = creditsStartPos + Vector3.up * scrollDistance;

        // Start invisible
        SetAlpha(creditsText, 0f);
        SetAlpha(continueText, 0f);
    }

    void Update()
    {
        RunCredits();

        if (creditsFinished && Input.GetKeyDown(KeyCode.J))
        {
            GoToGameScene();
        }
    }

    void RunCredits()
    {
        timer += Time.deltaTime;

        // Fade in continue prompt after credits
        if (creditsFinished)
        {
            float promptTimer = timer - scrollDuration;
            float promptAlpha = Mathf.Clamp01(promptTimer / promptFadeInTime);
            SetAlpha(continueText, promptAlpha);
            return;
        }

        float progress = Mathf.Clamp01(timer / scrollDuration);

        // Move upward
        creditsText.rectTransform.localPosition =
            Vector3.Lerp(creditsStartPos, creditsEndPos, progress);

        // Fade credits text
        float alpha = 1f;

        if (timer < fadeInTime)
            alpha = timer / fadeInTime;
        else if (timer > scrollDuration - fadeOutTime)
            alpha = (scrollDuration - timer) / fadeOutTime;

        SetAlpha(creditsText, Mathf.Clamp01(alpha));

        // Finish sequence
        if (timer >= scrollDuration)
        {
            creditsFinished = true;
        }
    }

    void SetAlpha(TextMeshProUGUI textObj, float alpha)
    {
        Color c = textObj.color;
        c.a = alpha;
        textObj.color = c;
    }

    void GoToGameScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
