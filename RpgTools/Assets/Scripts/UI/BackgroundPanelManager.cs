using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundPanelManager : MonoBehaviour
{
    [SerializeField]
    private Image fadeImage;
    [SerializeField]
    private RectTransform rectTransform;
    private const float fadeDuration = 1f;

    public static BackgroundPanelManager Instance = null;

    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void Start()
    {
        HideImage();
    }

    public void DisplayImage(string name)
    {
        Color c = Color.white;
        c.a = 1;
        fadeImage.color = c;
        fadeImage.sprite = Resources.Load<Sprite>($"Images/{name}");
        rectTransform.anchorMin = new Vector2( rectTransform.anchorMin.x, 0.3f);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }

    public void HideImage()
    {
        Color c = Color.black;
        c.a = 0;
        fadeImage.color = c;
        fadeImage.sprite = null;
        rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, 0f);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }


    public IEnumerator Transition(Action onFadeIn, Action onFadeOut)
    {
        float t = 0f;
        Color c = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration); ;
            fadeImage.color = c;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        c.a = 1;
        fadeImage.color = c;

        onFadeIn();


        t = 0f;
        c = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadeImage.color = c;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        c.a = 0;
        fadeImage.color = c;

        yield return new WaitForSeconds(fadeDuration);

        onFadeOut();
    }
}
