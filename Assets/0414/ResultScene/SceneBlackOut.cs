using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneBlackOut : MonoBehaviour
{
    [SerializeField] private Image _PanelImage;
    [SerializeField] private float _speed;
    [SerializeField] private string nextSceneName;
    private bool isSceneChange;
    private Color PanelColor;
    RectTransform rectTransform;
    float width, height, ScaleX, ScaleY;
    // Start is called before the first frame update
    private void Awake()
    {
        isSceneChange = false;
        PanelColor = _PanelImage.color;
        Debug.Log("PanelColorInAwake" + PanelColor);
        rectTransform = _PanelImage.rectTransform;
        width = 300;
        height = 300;
        ScaleX = 0f;
        ScaleY = 0f;
        Debug.Log("リザルト画面の暗転用画像のSpeed" + _speed);
    }
    public void BlackOutSceneChangeForResult()
    {
        //StartCoroutine(BlackOutForResultScene());
        //StartCoroutine(BigWolfSceneChange());
        StartCoroutine(BigPanelCircleSceneChange());
    }
    public void BlackOutSceneChangeForTitle()
    {
        StartCoroutine(BlackOutForTitleScene());
    }
    private IEnumerator BlackOutForResultScene()
    {
        while (!isSceneChange)
        {
            PanelColor.a += 0.0005f;
            _PanelImage.color = PanelColor;
            if (PanelColor.a >= 1)
                isSceneChange = true;
            yield return new WaitForSeconds(_speed);
        }
        SceneManager.LoadScene(nextSceneName);
    }
    private IEnumerator BlackOutForTitleScene()
    {
        while (!isSceneChange)
        {
            PanelColor.a += 0.08f;
            _PanelImage.color = PanelColor;
            if (PanelColor.a >= 1)
                isSceneChange = true;
            yield return new WaitForSeconds(_speed);
        }
        SceneManager.LoadScene(nextSceneName);
    }
   
    private IEnumerator BigPanelCircleSceneChange()
    {
        while (!isSceneChange)
        {
            rectTransform.localScale = new Vector3(ScaleX += 0.2f, ScaleY += 0.2f);
            PanelColor.a += 0.07f;
            _PanelImage.color = PanelColor;
            //Debug.Log("Speed" + _speed);
            if (rectTransform.localScale.x >= 3f && PanelColor.a >= 1)
                isSceneChange = true;
            yield return new WaitForSeconds(_speed);
        }
        Debug.Log("リザルト画面の暗転用画像のSpeed2  " + _speed);
        SceneManager.LoadScene(nextSceneName);
    }
}