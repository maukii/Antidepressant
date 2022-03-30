using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] CanvasGroup group;
    [SerializeField] TextMeshProUGUI scalingLabel;
    [SerializeField] TextMeshProUGUI titleLabel;

    [Header("Appear animation")]
    [SerializeField] float fadeDuration = 0.25f;
    [SerializeField] float rotationAmount = 14f;
    [SerializeField] float rotationDuration = 1f;

    [Header("Text animation")]
    [SerializeField] float textScaleDuration = 0.5f;

    [SerializeField] AudioSource sighAudio;
    [SerializeField] AudioSource gulpAudio;
    bool clicked = false;


    void Awake()
    {
        titleLabel.transform.localPosition = Vector3.zero;
        scalingLabel.transform.localScale = Vector3.zero;
        group.alpha = 0f;
    }

    void Start()
    {
        LeanTween.value(0f, 1f, fadeDuration).setOnUpdate((value) => { group.alpha = value; });

        group.transform.Rotate(Vector3.forward * rotationAmount);
        LeanTween.rotate(group.gameObject, Vector3.forward * -rotationAmount * 2f, rotationDuration).setLoopPingPong().setEaseInOutSine();
    }

    public void OnHoverEnter()
    {
        if (LeanTween.isTweening(scalingLabel.gameObject))
            LeanTween.cancel(scalingLabel.gameObject);

        if(LeanTween.isTweening(titleLabel.gameObject))
            LeanTween.cancel(titleLabel.gameObject);

        LeanTween.scale(scalingLabel.gameObject, Vector3.one, textScaleDuration).setEaseOutBack();
        LeanTween.moveLocalX(titleLabel.gameObject, 100f, textScaleDuration).setEaseOutBack();
    }

    public void OnHoverExit()
    {
        if (LeanTween.isTweening(scalingLabel.gameObject))
            LeanTween.cancel(scalingLabel.gameObject);

        if (LeanTween.isTweening(titleLabel.gameObject))
            LeanTween.cancel(titleLabel.gameObject);

        LeanTween.scale(scalingLabel.gameObject, Vector3.zero, textScaleDuration).setEaseInBack();
        LeanTween.moveLocalX(titleLabel.gameObject, 0f, textScaleDuration).setEaseInBack();
    }

    public void OnClick()
    {
        if (clicked)
            return;

        clicked = true;
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        sighAudio.Play();
        yield return new WaitForSeconds(sighAudio.clip.length);
        gulpAudio.Play();
        yield return new WaitForSeconds(gulpAudio.clip.length);

        Fader.Instance.OnFadeCompleted += FadeComplete;
        Fader.Instance.FadeToColor(Color.black);
    }

    void FadeComplete() => SceneManager.LoadScene(1);
}
