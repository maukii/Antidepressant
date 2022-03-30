using System;
using UnityEngine;

public class Colorable : MonoBehaviour
{
    public event Action<Colorable> ColoringFinished;

    [Range(0f, 1f)]
    [SerializeField] float grayscaleAmount = 0f;

    public bool Colorized { get; private set; }
    float hoverOverTime = 0f;
    [SerializeField] SpriteRenderer rend;
    [SerializeField] float colorizingDuration = 1f;

    public MaterialPropertyBlock Mbp
    {
        get
        {
            if (mpb == null)
                mpb = new MaterialPropertyBlock();

            return mpb;
        }
    }
    MaterialPropertyBlock mpb;



    void OnValidate()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.GetPropertyBlock(Mbp);
        Mbp.SetFloat("_GrayscaleAmount", grayscaleAmount);
        rend.SetPropertyBlock(Mbp);
    }

    void Awake()
    {
        grayscaleAmount = 1f;

        rend = GetComponent<SpriteRenderer>();
        rend.GetPropertyBlock(Mbp);
        Mbp.SetFloat("_GrayscaleAmount", grayscaleAmount);
        rend.SetPropertyBlock(Mbp);
    }


    //TODO::
    //Call from main camera script which checks: 
    //Object which got clicked(this)
    //Vertex position of click(so effect can start there)
    void OnMouseDown()
    {
        if (!Colorized)
        {
            Colorize(Vector2.zero);
            Colorized = true;
        }
    }
    public void Colorize(Vector2 hitPosition)
    {
        rend.GetPropertyBlock(Mbp);
        LeanTween.value(grayscaleAmount, 0f, colorizingDuration).setOnUpdate((value) =>
        {
            Mbp.SetFloat("_GrayscaleAmount", value);
            rend.SetPropertyBlock(Mbp);
        }).setOnComplete(() => 
        {
            Colorized = true;
            grayscaleAmount = 0f;
            Mbp.SetFloat("_GrayscaleAmount", grayscaleAmount);
            rend.SetPropertyBlock(Mbp);

            LeanTween.scale(gameObject, Vector3.one * 1.1f, 0.25f).setEasePunch();
            ColoringFinished?.Invoke(this);
        });
    }

    // Adds color when hovering 
    void OnMouseOver() => AddColor();
    public void AddColor()
    {
        Debug.LogError(grayscaleAmount);

        if (grayscaleAmount > 0)
        {
            hoverOverTime += Time.deltaTime;
            grayscaleAmount = Mathf.Clamp01(1 - (hoverOverTime / colorizingDuration));
            Mbp.SetFloat("_GrayscaleAmount", grayscaleAmount);
            rend.SetPropertyBlock(Mbp);
            return;
        }
        else
        {
            if (!Colorized)
            {
                Colorized = true;
                grayscaleAmount = 0f;
                Mbp.SetFloat("_GrayscaleAmount", grayscaleAmount);
                rend.SetPropertyBlock(Mbp);
                Colorized = true;

                LeanTween.scale(gameObject, Vector3.one * 1.1f, 0.25f).setEasePunch();
                ColoringFinished?.Invoke(this);
            }
        }
    }
}
