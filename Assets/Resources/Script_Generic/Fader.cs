using AddClass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    enum FadeType
    {
        None,
        FadeIn,
        FadeOut,
        FadeInOut,
    }
    [SerializeField] private Image image;
    private Color imageColor;
    [SerializeField] private FadeType fadeType;
    private bool turning;
    [SerializeField] private Interval fadeTime = new Interval();            // �w�肵�����Ԃ�Fade������������
    private Interval fadeInOutInterval = new Interval();                    // �w�肵�����Ԃ�Fade������������
    [SerializeField] private Interval betweenDisplayTime = new Interval();  // FadeInOut�ŕ\�����鎞��
    [field: SerializeField] public FadeState fadeState { get; private set; }
    private void Start()
    {
        imageColor = image.color;
        imageColor.a = 0.0f;

        image.color = imageColor;

        fadeTime.Initialize(true, true);
        fadeTime.reachAction += () => turning = true;
        fadeInOutInterval.Initialize(false, true, fadeTime.interval * 2);
        betweenDisplayTime.Initialize(false);
        betweenDisplayTime.lowAction += fadeTime.Reset;
    }

    private void Update()
    {
        ChangeFadeState();
        if(fadeState != FadeState.Fading)
        {
            fadeTime.Reset();
        }

        switch (fadeType)
        {

            case FadeType.FadeIn:
                fadeTime.Update();
                imageColor.a = fadeTime.ratio;
                break;

            case FadeType.FadeOut:
                fadeTime.Update();
                imageColor.a = 1.0f - fadeTime.ratio;
                break;

            case FadeType.FadeInOut:
                fadeTime.Update();
                fadeInOutInterval.Update();
                if (turning == false)
                {
                    imageColor.a = fadeTime.ratio;
                }
                else
                {
                    imageColor.a = 1.0f - fadeTime.ratio;
                }
                break;
        }
        image.color = imageColor;

    }

    private void ChangeFadeState()
    {
        if (imageColor.a <= 0.0f)
        {
            fadeState = FadeState.FinishFadeOut;
        }
        else if (imageColor.a >= 1.0f)
        {
            fadeState = FadeState.FinishFadeIn;
        }
        else
        {
            fadeState = FadeState.Fading;
        }

        Debug.Log(imageColor.a);
    }

    public void FadeIn()
    {
        if(fadeState == FadeState.Fading) 
        {
            Debug.Log("Fade��");
            return; 
        }

        fadeTime.Reset();
        fadeType = FadeType.FadeIn;
        turning = false;
    }

    public void FadeOut()
    {
        if (fadeState == FadeState.Fading)
        {
            Debug.Log("Fade��");
            return;
        }

        fadeTime.Reset();
        fadeType = FadeType.FadeOut;
        turning = false;
    }
    public void FadeInOut()
    {


        if (fadeState == FadeState.Fading)
        {
            Debug.Log("Fade��");
            return;
        }

        fadeTime.Reset();
        fadeInOutInterval.Reset();
        fadeType = FadeType.FadeInOut;
        turning = false;
    }

    private void ActiveChanger()
    {
        if(turning == true)
        {

        }
    }
}

public enum FadeState
{
    FinishFadeIn,   // �摜��\���������Ă�����
    FinishFadeOut,  // �摜���\���ɂ������Ă�����
    Fading          // �摜��\���܂��͔�\�����̏��
}