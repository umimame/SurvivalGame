using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.Build;
//using static Chara_Player;

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
    [SerializeField] private float Alpha, r, g, b, addR, addG, addB;
    [SerializeField] bool[] ShowText = new bool[12];
    bool rAdd, gAdd, bAdd, SceneChangeFlag;
    int NowIndex;
    int playerScore, playerKill, playerApple, playerOrange, playerGrape;
    //Chara_Player�I�u�W�F�N�g���擾
    ResultPlayer charaPlayer;
    // Start is called before the first frame update
    void Start()
    {
        charaPlayer = FindObjectOfType<ResultPlayer>();

        //�F�X�ȕ��̏�����
        TMProTransparent();
        //���ԂŃe�L�X�g���̓�������
        StartCoroutine(ShowTextByTime());

        StartCoroutine(PushKeyBlinking());

        SceneChangeFlag = false;

        GetPlayerScore();
    }
    // Update is called once per frame
    void Update()
    {
        //PushtoTitle�̐F�ς�
        PushKeyColorChange();
        //Space��������e�L�X�g�\���p��bool�𔽓]
        PushtoBoolInversion();
        //�{�^���������ăV�[���J��
        SceneChange();
        //�摜�ƃe�L�X�g�̕\��
        ShowAll();
        if (NowIndex == ShowText.Length)
        {
            SceneChangeFlag = true;
        }
    }
    void SceneChange()//�V�[���J�ڗp
    {
        if (SceneChangeFlag)
        {
            if (Input.GetKey(KeyCode.Escape))//Escape�������ꂽ��
            {
                //Escape�������ꂽ�Ƃ������������Ȃ�
            }
            else
            {
                if (Input.anyKey)//�ꕔ�������Ȃ��L�[������
                {
                    charaPlayer.SetScore();//�V�[���J�ڑO�ɃX�R�A���̏�����

                    SceneManager.LoadScene("TitleScene");
                }
            }
        }
    }
    void TMProTransparent()//�F�X�ȕ��̏�����
    {
        //ShowText�����ׂ�false��,�S��������
        for (int i = 0; i < ShowText.Length; i++)
        {
            ShowText[i] = false;
            AllText[i].alpha = 0f;
        }
        //�����S�̉摜
        AppleImage.color = new Color(1f, 1f, 1f, 0f);
        //�~�J���̉摜
        OrangeImage.color = new Color(1f, 1f, 1f, 0f);
        //�u�h�E�̉摜
        GrapeImage.color = new Color(1f, 1f, 1f, 0f);
        //PushAnyKey�̏�����
        Alpha = 0f;
        r = 1f;
        g = 0.5f;
        b = 0f;
        PushAnyKey.color = new Color(r, g, b, Alpha);//�ŏ��͓���
        //RGB�ɉ��Z����l�������_���Ŏ擾
        addR = Random.Range(0.001f, 0.00001f);
        addG = Random.Range(0.001f, 0.00001f);
        addB = Random.Range(0.001f, 0.00001f);
        //RGB���ꂼ��ɉ��Z���邩���Z���邩�̔���
        rAdd = true;
        gAdd = true;
        bAdd = true;
        playerScore = 0;
    }
    void PushtoBoolInversion()//Space��������e�L�X�g�\���t���O��true�ɕύX
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < ShowText.Length; i++)
            {
                if (!ShowText[i])
                {
                    ShowText[i] = true;
                    NowIndex = i;
                    break;
                }
            }
        }
    }
    void ShowAll()//�e�L�X�g���̕\������
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
    private IEnumerator ShowTextByTime()//���Ԍo�߂Ńe�L�X�g���̕\��
    {
        for (; NowIndex < ShowText.Length; NowIndex++)
        {
            yield return new WaitForSeconds(1f);
            if (!ShowText[NowIndex])
            {
                ShowText[NowIndex] = true;
                AllText[NowIndex].alpha = 1f;
            }
        }
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
        if (SceneChangeFlag)
        {
            //�F�ς��Ɠ�����
            PushAnyKey.color = new Color(r, g, b, Alpha);
        }
    }
    private IEnumerator PushKeyBlinking()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Alpha = 1f;//����������
            yield return new WaitForSeconds(1f);
            Alpha = 0f;//������
        }
    }
    void GetPlayerScore()
    {
        if (charaPlayer != null)
        {
            Debug.Log("Chara_Player��������܂������`�`���`�`�ق��ق��ف`�`�`");
            //Chara_Player��Get~���Ăяo��
            playerScore = charaPlayer.GetScore();
            playerKill = charaPlayer.GetKill();
            playerApple = charaPlayer.GetApple();
            playerOrange = charaPlayer.GetOrange();
            playerGrape = charaPlayer.GetGrape();
            //�X�R�A�𕶎��ɕϊ�
            ScoreAmounts.text = playerScore.ToString();
            KillAmounts.text = playerKill.ToString();
            AppleAmounts.text = playerApple.ToString();
            OrangeAmounts.text = playerOrange.ToString();
            GrapeAmounts.text = playerGrape.ToString();
            //Score��Kill����0�ɂ���(SceneChange�O�Ɏ����ς�)
            //charaPlayer.SetScore();
        }
        else
        {
            Debug.Log("Chara_Player��������Ȃ���");
        }
    }    
//��
//��
//��
//��
//��
//�u
//I
//M
//i
//S
//S
//Y
//o
//u
//�v
//
}
