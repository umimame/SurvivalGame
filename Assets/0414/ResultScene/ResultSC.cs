using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.Build;

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
    [SerializeField] private AudioSource DramRollEnd;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerScript playerScript = FindAnyObjectByType<PlayerScript>();

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

        //�F�X�ȕ��̏�����
        TMProTransparent();
        //���ԂŃe�L�X�g���̓�������
        StartCoroutine(ShowTextByTime());

        StartCoroutine(PushKeyBlinking());

        SceneChangeFlag = false;
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
                PlayDramRollEnd();
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
                PlayDramRollEnd2();
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
    void PlayDramRollEnd()
    {
        DramRollEnd.PlayOneShot(DramRollEnd.clip);
        Debug.Log("�����Ă܂����`�I�I�I");
    }
    void PlayDramRollEnd2()
    {
        DramRollEnd.PlayOneShot(DramRollEnd.clip);
        Debug.Log("�����Ă܂����2222");
    }

    //�v���C���[�X�R�A�擾�p�̃X�N���v�g�̃e�X�g------------------------------------------
    //void GetScore()
    //{
    //    int playerScore = playerScript.GetPlayerScore();
    //    ScoreAmounts.text = playerScore.ToSafeString();
    //}

    //void GetScore2(Chara_Player player)
    //{
    //    ScoreAmounts.text = player.Score.ToSafeString();
    //}

    //void GetScore3()
    //{
    //    //Chara_Player�I�u�W�F�N�g���擾
    //    Chara_Player charaPlayer = FindObjectOfType<Chara_Player>();
    //    if (charaPlayer != null)
    //    {
    //        int playerScore = charaPlayer.GetScore();
    //        ScoreAmounts.text = playerScore.ToString();
    //    }
    //    else
    //    {
    //        Debug.Log("Chara_Player��������Ȃ���");
    //    }
    //}

    //void GetPlayerFruits()
    //{
    //    Chara_Player�I�u�W�F�N�g���擾
    //    Chara_Player charaPlayer = FindObjectOfType<Chara_Player>();
    //    if (charaPlayer != null)
    //    {
    //        //�����S�擾
    //        int playerApple = charaPlayer.GetFruits();
    //        AppleAmounts.text = playerGrape.ToString();
    //        //�~�J���擾
    //        int playerOrange= charaPlayer.GetFruits();
    //        OrangeAmounts.text = playerGrape.ToString();
    //        //�u�h�E�擾
    //        int playerGrape = charaPlayer.GetFruits();
    //        GrapeAmounts.text=playerGrape.ToString();
    //        
    //
    //
    //
    //
    //
    //
    //        
    //    }
    //}

}
