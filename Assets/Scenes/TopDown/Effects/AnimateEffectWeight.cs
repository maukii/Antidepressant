using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Volume))]
public class AnimateEffectWeight : MonoBehaviour
{
    [SerializeField] float animateVisibleDuration = 1f;
    [SerializeField] float animateInvisibleDuration = 1f;
    [SerializeField] LeanTweenType animateVisibleType = LeanTweenType.easeInBack;
    [SerializeField] LeanTweenType animateInvisibleType = LeanTweenType.easeInBack;
    Volume volume;
    bool visible = false;


    void Awake()
    {
        volume = GetComponent<Volume>();
        visible = volume.weight > 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            AnimateWeight();
    }

    void AnimateWeight()
    {
        if (LeanTween.isTweening(gameObject))
            LeanTween.cancel(gameObject);

        float start = visible ? 1 : 0;
        float end = visible ? 0 : 1;
        LeanTweenType easing = visible ? animateInvisibleType : animateVisibleType;
        float duration = visible ? animateInvisibleDuration : animateVisibleDuration;
        LeanTween.value(gameObject, UpdateWeight, start, end, duration).setEase(easing);
        visible = !visible;
    }

    void UpdateWeight(float weight) => volume.weight = weight;
}
