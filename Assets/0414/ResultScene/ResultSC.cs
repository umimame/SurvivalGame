using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using Unity.VisualScripting;

public class ResultSC : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Winner;
    [SerializeField] private TextMeshProUGUI PlayerName;
    [SerializeField] private TextMeshProUGUI Kill;
    [SerializeField] private TextMeshProUGUI KillAmounts;
    [SerializeField] private TextMeshProUGUI Score;
    [SerializeField] private TextMeshProUGUI ScoreAmounts;
    [SerializeField] private TextMeshProUGUI Apple;
    [SerializeField] private TextMeshProUGUI AppleAmounts;
    [SerializeField] private TextMeshProUGUI Orange;
    [SerializeField] private TextMeshProUGUI OrangeAmounts;
    [SerializeField] private TextMeshProUGUI Grape;
    [SerializeField] private TextMeshProUGUI GrapeAmounts;
    [SerializeField] private TextMeshProUGUI PushAnyKey;
    [SerializeField] private float r, g, b, addR,addG,addB;
    bool rAdd, gAdd, bAdd;
    // Start is called before the first frame update
    void Start()
    {
        r = 1f;
        g = 0.5f;
        b = 0f;
        addR = Random.Range(0.001f,0.0001f);
        addG = Random.Range(0.001f, 0.0001f);
        addB = Random.Range(0.001f, 0.0001f);

        rAdd = true;
        gAdd = true;
        bAdd = true;
        TMProTransparent();
        StartCoroutine(TextControl());
    }

    // Update is called once per frame
    void Update()
    {
        //rAdd
        if (rAdd)
        {
            r += addR;
            if (r>=1f)
            {
                rAdd = false;
            }
        }
        if (!rAdd)
        {
            r -= addR;
            if(r<=0f)
            {
                rAdd = true;
            }
        }
        //gAdd
        if (gAdd)
        {
            g += addG;
            if (g >= 1f)
            {
                gAdd = false;
            }
        }
        if (!gAdd)
        {
            g -= addG;
            if (g <= 0f)
            {
                gAdd = true;
            }
        }
        //bAdd
        if (bAdd)
        {
            b += addB;
            if (b >= 1f)
            {
                bAdd = false;
            }
        }
        if (!bAdd)
        {
            b -= addB;
            if (b <= 0f)
            {
                bAdd = true;
            }
        }
        //�F�ς�
        PushAnyKey.color = new Color(r, g, b);

        SceneChange();
    }
    void SceneChange()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
    void TMProTransparent()
    {
        Winner.alpha = 0f;
        //�v���C���[�̖��O
        PlayerName.color = Color.red;
        PlayerName.alpha = 0f;
        //�L��
        Kill.alpha = 0f;
        KillAmounts.alpha = 0f;
        //�X�R�A
        Score.alpha = 0f;
        ScoreAmounts.alpha = 0f;
        //�����S
        Apple.color = Color.red;
        Apple.alpha = 0f;
        AppleAmounts.alpha = 0f;
        //�݂���
        Orange.alpha = 0f;
        OrangeAmounts.alpha = 0f;
        //�u�h�E
        Grape.color = new Color(1, 0, 1, 0);
        Grape.alpha = 0f;
        GrapeAmounts.alpha = 0f;
        //PushAnyKey
        PushAnyKey.alpha = 0f;
    }

    private IEnumerator TextControl()
    {
        yield return new WaitForSeconds(1f);
        Winner.alpha = 1f;
        yield return new WaitForSeconds(1f);
        PlayerName.alpha = 1f;
        yield return new WaitForSeconds(1f);
        Kill.alpha = 1f;
        yield return new WaitForSeconds(1f);
        KillAmounts.alpha = 1f;
        yield return new WaitForSeconds(1f);
        Score.alpha = 1f;
        yield return new WaitForSeconds(1f);
        ScoreAmounts.alpha = 1f;
        yield return new WaitForSeconds(1f);
        Apple.alpha = 1f;
        yield return new WaitForSeconds(1f);
        AppleAmounts.alpha = 1f;
        yield return new WaitForSeconds(1f);
        Orange.alpha = 1f;
        yield return new WaitForSeconds(1f);
        OrangeAmounts.alpha = 1f;
        yield return new WaitForSeconds(1f);
        Grape.alpha = 1f;
        yield return new WaitForSeconds(1f);
        GrapeAmounts.alpha = 1f;
        yield return new WaitForSeconds(1f);
        while (true)
        {

          
            // �e�L�X�g��_�ł�����

            PushAnyKey.alpha = 1f; // �A���t�@�l��1�ɓ�������
            yield return new WaitForSeconds(0.5f); // 0.5�b�҂�
            PushAnyKey.alpha = 0f; // �A���t�@�l��0�ɓ����ɂȂ�
            yield return new WaitForSeconds(0.5f); // 0.5�b�҂�


        }
    }
}
