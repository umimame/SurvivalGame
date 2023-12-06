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
    [SerializeField] private TextMeshProUGUI[] AllText = new TextMeshProUGUI[12];



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
    [SerializeField] private Image AppleImage;
    [SerializeField] private Image OrangeImage;
    [SerializeField] private Image GrapeImage;
    [SerializeField] private float r, g, b, addR, addG, addB;
    bool rAdd, gAdd, bAdd;
    //[SerializeField] bool WinnerAlpha,//���҂́H
    //     PlayerNameAlpha,//�v���C���[
    //     KillAlpha, KillAmountsAlpha,//�L��
    //     ScoreAlpha, ScoreAmountsAlpha,//�X�R�A
    //     AppleAlpha, AppleAmountsAlpha, AppleImageAlpha,//�����S
    //     OrangeAlpha, OrangeAmountsAlpha, OrangeImageAlpha,//�~�J��
    //     GrapeAlpha, GrapeAmountsAlpha, GrapeImageAlpha,//�u�h�E
    //     PushAnyKeyAlpha;//PushtoTitle
    [SerializeField] bool[] ShowText = new bool[12];
    int NowIndex;


    // Start is called before the first frame update
    void Start()
    {
        //ShowText�����ׂ�false��
        for (int i = 0; i < ShowText.Length; i++)
        {
            ShowText[i] = false;
            AllText[i].alpha = 0f;
        }


        //WinnerAlpha = false;
        //PlayerNameAlpha = false;
        //KillAlpha = false;
        //KillAmountsAlpha = false;
        //ScoreAlpha = false;
        //ScoreAmountsAlpha = false;
        //AppleAlpha = false;
        //AppleAmountsAlpha = false;
        //AppleImageAlpha = false;
        //OrangeAlpha = false;
        //OrangeAmountsAlpha = false;
        //OrangeImageAlpha = false;
        //GrapeAlpha = false;
        //GrapeAmountsAlpha = false;
        //GrapeImageAlpha = false;
        //PushAnyKeyAlpha = false;

        //PlayerName�ɕ\�����镶����ݒ�,�ǂ��������������ŕς����悤�ɂ���B������
        PlayerName.text = "Player1";
        //�L�����擾(�ϐ��Ŏ��)������
        KillAmounts.text = "12";
        //�X�R�A�擾(�ϐ��łƂ�)������
        ScoreAmounts.text = "123";
        //�����S�擾���A������
        AppleAmounts.text = "12";
        //�~�J���擾���A������
        OrangeAmounts.text = "123";
        //�u�h�E�擾���A������
        GrapeAmounts.text = "12";
        //RGB���ꂼ��̏����l
        r = 1f;
        g = 0.5f;
        b = 0f;
        //RGB�ɉ��Z����l�������_���Ŏ擾
        addR = Random.Range(0.001f, 0.0001f);
        addG = Random.Range(0.001f, 0.0001f);
        addB = Random.Range(0.001f, 0.0001f);
        //RGB���ꂼ��ɉ��Z���邩���Z���邩�̔���
        rAdd = true;
        gAdd = true;
        bAdd = true;
        //�ŏ���TMPro�𓧖���
        TMProTransparent();
        //���ԂŃe�L�X�g���̓�������
        //StartCoroutine(TextControl());

        //StartCoroutine(ShowTextByTime());


        StartCoroutine(Test());
    }

    // Update is called once per frame
    void Update()
    {
        //PushtoTitle�̐F�ς�
        PushKeyColorChange();

        PushtoBoolInversion();

        //�{�^���������ăV�[���J��
        //SceneChange();

        ShowAll();
    }
    void SceneChange()//�V�[���J�ڗp
    {
        if (Input.GetKey(KeyCode.Escape))//Escape�������ꂽ��
        {
            //Escape�������ꂽ�Ƃ������������Ȃ�
        }
        else
        {
            if (Input.anyKey)
            {
                SceneManager.LoadScene("TitleScene");
            }
        }
    }
    void TMProTransparent()//�e�L�X�g�Ȃǂ𓧖���(���l��0��)
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
        AppleImage.color = new Color(1f, 1f, 1f, 0f);
        //�݂���
        Orange.alpha = 0f;
        OrangeAmounts.alpha = 0f;
        OrangeImage.color = new Color(1f, 1f, 1f, 0f);
        //�u�h�E
        Grape.color = new Color(1f, 0f, 1f, 0f);
        GrapeAmounts.alpha = 0f;
        GrapeImage.color = new Color(1f, 1f, 1f, 0f);
        //PushAnyKey
        PushAnyKey.alpha = 0f;
    }
    void PushtoBoolInversion()//Space��������e�L�X�g�\��(������)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < ShowText.Length; i++)
            {

                Debug.Log("NowIndex" + NowIndex);

                if (!ShowText[i])
                {
                    ShowText[i] = true;
                    NowIndex = i;
                    //Debug.Log("NowIndex" + NowIndex);
                    break;
                }
            }
        }
    }
    void ShowAll()
    {
        for (int i = 0; i < ShowText.Length; i++)
        {
            if (ShowText[i])
            {
                AllText[i].alpha = 1f;
                if (ShowText[7])
                {
                    AppleImage.color = new Color(1f, 0.759434f, 0.759434f, 1f);
                }
                if (ShowText[9])
                {
                    OrangeImage.color = new Color(0.9433962f, 0.8610221f, 0.7253471f, 1f);
                }
                if (ShowText[11])
                {
                    GrapeImage.color = new Color(0.745283f, 0.7277056f, 0.7388983f, 1f);
                }
            }
        }
    }

    private IEnumerator Test()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < ShowText.Length; i++)
            {
                if (!ShowText[i])
                {
                    ShowText[i] = true;
                    NowIndex = i;
                    //Debug.Log("NowIndex" + NowIndex);
                    break;
                }
                if (ShowText[i])
                {
                    yield return new WaitForSeconds(1f);
                    NowIndex++;
                }
            }
        }
       
    }



    private IEnumerator ShowTextByTime()
    {
        for (int i = NowIndex; i < ShowText.Length; i++)
        {
            yield return new WaitForSeconds(1f);
            ShowText[i] = true;
            Debug.Log("NowIndex==" + NowIndex);
            if (ShowText[i])
            {
                AllText[i].alpha = 1f;
            }
        }
    }

    private IEnumerator TextControl()//���ԂŃe�L�X�g�Ȃǂ�\��(���l��1��)
    {
        yield return new WaitForSeconds(1f);
        ShowText[0] = true;
        if (ShowText[0])
        {
            Winner.alpha = 1f;
        }
        yield return new WaitForSeconds(1f);
        ShowText[1] = true;
        if (ShowText[1])
        {
            PlayerName.alpha = 1f;
        }
        //Kill
        yield return new WaitForSeconds(1f);
        ShowText[2] = true;
        if (ShowText[2])
        {
            Kill.alpha = 1f;
        }
        yield return new WaitForSeconds(1f);
        ShowText[3] = true;
        if (ShowText[3])
        {
            KillAmounts.alpha = 1f;
        }
        //Score
        yield return new WaitForSeconds(1f);
        ShowText[4] = true;
        if (ShowText[4])
        {
            Score.alpha = 1f;
        }
        yield return new WaitForSeconds(1f);
        ShowText[5] = true;
        if (ShowText[5])
        {
            ScoreAmounts.alpha = 1f;
        }
        //�����S
        yield return new WaitForSeconds(1f);
        ShowText[6] = true;
        if (ShowText[6])
        {
            Apple.alpha = 1f;
            AppleImage.color = new Color(1f, 0.759434f, 0.759434f, 1f);
        }
        yield return new WaitForSeconds(1f);
        ShowText[7] = true;
        if (ShowText[7])
        {
            AppleAmounts.alpha = 1f;
        }
        //�~�J��
        yield return new WaitForSeconds(1f);
        ShowText[8] = true;
        if (ShowText[8])
        {
            Orange.alpha = 1f;
            OrangeImage.color = new Color(0.9433962f, 0.8610221f, 0.7253471f, 1f);
        }
        yield return new WaitForSeconds(1f);
        ShowText[9] = true;
        if (ShowText[9])
        {
            OrangeAmounts.alpha = 1f;
        }
        //�u�h�E
        yield return new WaitForSeconds(1f);
        ShowText[10] = true;
        if (ShowText[10])
        {
            Grape.alpha = 1f;
            GrapeImage.color = new Color(0.745283f, 0.7277056f, 0.7388983f, 1f);
        }
        yield return new WaitForSeconds(1f);
        ShowText[11] = true;
        if (ShowText[11])
        {
            GrapeAmounts.alpha = 1f;
        }
        yield return new WaitForSeconds(1f);

        //while (true)//��Ɏ��s
        //{
        //    // �e�L�X�g��_�ł�����
        //    PushAnyKey.alpha = 1f; // �A���t�@�l��1�ɓ�������
        //    yield return new WaitForSeconds(0.5f); // 0.5�b�҂�
        //    PushAnyKey.alpha = 0f; // �A���t�@�l��0�ɓ����ɂȂ�
        //    yield return new WaitForSeconds(0.5f); // 0.5�b�҂�
        //}
    }
    void PushKeyColorChange()//PushAnyKey�̐F�ς�
    {
        //R�ɉ��Z
        if (rAdd)
        {
            r += addR;
            if (r >= 1f) { rAdd = false; }
        }
        if (!rAdd)//R�Ɍ��Z
        {
            r -= addR;
            if (r <= 0f) { rAdd = true; }
        }
        //G�ɉ��Z
        if (gAdd)
        {
            g += addG;
            if (g >= 1f) { gAdd = false; }
        }
        if (!gAdd)//G�Ɍ��Z
        {
            g -= addG;
            if (g <= 0f) { gAdd = true; }
        }
        //B�ɉ��Z
        if (bAdd)
        {
            b += addB;
            if (b >= 1f) { bAdd = false; }
        }
        if (!bAdd)//B�Ɍ��Z
        {
            b -= addB;
            if (b <= 0f) { bAdd = true; }
        }
        //�F�ς�
        PushAnyKey.color = new Color(r, g, b);
    }
}
