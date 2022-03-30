using System;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public static Fader Instance;
    public event Action OnFadeCompleted;

    [SerializeField] Image fade;
    [SerializeField] CanvasGroup group;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            OnFadeCompleted += FadeOut;
        }
        else
            Destroy(gameObject);
    }

    public void FadeToColor(Color fadeColor)
    {
        fade.color = fadeColor;
        LeanTween.value(gameObject, UpdateAlpha, 0f, 1f, 1f).setOnComplete(OnFadeCompleted.Invoke);
    }

    public void FadeOut() => LeanTween.value(gameObject, UpdateAlpha, 1f, 0f, 1f);

    void UpdateAlpha(float value) => group.alpha = Mathf.Clamp01(value);
}
