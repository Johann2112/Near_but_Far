using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float speed = 2f;
    [SerializeField] private string enterKeySceneName;

    private bool isFadingOut = false;
    private bool isFadingIn = false;
    private string sceneName = "";

    private void Awake()
    {
        SetAlpha(0f);
    }

    private void Start()
    {
        SetAlpha(1f);
        TriggerFadeIn();
    }
    private void Update()
    {
        if (!isFadingOut && !isFadingIn && !string.IsNullOrEmpty(enterKeySceneName) && Input.GetKeyDown(KeyCode.Return))
        {
            TriggerFadeOut(enterKeySceneName);
        }

        if (isFadingIn)
        {
            float newAlpha = fadeImage.color.a - Time.deltaTime * speed;
            if (newAlpha <= 0f)
            {
                newAlpha = 0f;
                isFadingIn = false;
            }
            SetAlpha(newAlpha);
        }

        if (isFadingOut)
        {
            float newAlpha = fadeImage.color.a + Time.deltaTime * speed;
            if (newAlpha >= 1f)
            {
                newAlpha = 1f;
                isFadingOut = false;
                SceneManager.LoadScene(sceneName);
            }
            SetAlpha(newAlpha);
        }
    }

    public void TriggerFadeOut(string targetScene)
    {
        sceneName = targetScene;
        isFadingOut = true;
    }

    public void TriggerFadeIn()
    {
        isFadingIn = true;
    }

    private void SetAlpha(float a)
    {
        Color c = fadeImage.color;
        c.a = a;
        fadeImage.color = c;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OnButtonFadeOut(string targetScene)
    {
        TriggerFadeOut(targetScene);
    }
}