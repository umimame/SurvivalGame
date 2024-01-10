using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    SceneBlackOut SceneBlackOut;
    // Start is called before the first frame update
    void Start()
    {
        SceneBlackOut = FindObjectOfType<SceneBlackOut>();
        //�F�X�ȕ��̏�����
        TMProTransparent();
        //���ԂŃe�L�X�g���̓�������
        StartCoroutine(ShowTextByTime());
        //PushKey�̓�������
        StartCoroutine(PushKeyBlinking());
        //�V�[���J�ڂ����邩�ǂ���
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
    bool GetButtonDown()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Fire0�������ꂽ��");
            return true;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Fire1�������ꂽ��");
            return true;
        }
        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("Fire2�������ꂽ��");
            return true;
        }
        if (Input.GetButtonDown("Fire3"))
        {
            Debug.Log("Fire3�������ꂽ��");
            return true;
        }
        //float horizontalInput = Input.GetAxis("Horizontal");
        //if (horizontalInput>0)
        //{
        //    Debug.Log("�E�����̓��͂���\��");
        //    return true;
        //}
        return false;
    }
    void SceneChange()//�V�[���J�ڗp
    {
        if (SceneChangeFlag)
        {
            if (Input.GetKey(KeyCode.Escape))//Escape�������ꂽ��
            {
                //Escape�������ꂽ�Ƃ������������Ȃ�
                Debug.Log("Escape�����Ă������Ȃ����`�`�`");
            }
            else
            {
                if (Input.anyKey)//�ꕔ�������Ȃ��L�[������
                {
                    //charaPlayer.SetScore();//�V�[���J�ڑO�ɃX�R�A���̏�����
                    Debug.Log("ResultSC�ŃV���O���g���j��");
                    SceneBlackOut.BlackOutSceneChangeForResult();
                    //SceneManager.LoadScene("TitleScene");
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
        r = 0f;
        g = 0f;
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
        if (Input.GetKeyDown(KeyCode.Space) || GetButtonDown())
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
    private IEnumerator PushKeyBlinking()//PushKey�̓�������
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Alpha = 1f;//����������
            yield return new WaitForSeconds(1f);
            Alpha = 0f;//������
        }
    }
    //�v���C���[���Ăԗp�̊֐�
    // ���R�ǋL
    // �����ɏ��҂�ݒ�o����悤�ǋL
    public void SetResult(Winner winner, int score, int kill, int apple, int orange, int grape)
    {
        playerScore = score;
        playerKill = kill;
        playerApple = apple;
        playerOrange = orange;
        playerGrape = grape;

        // ��L�ɑ�������ϐ����g�p����Ă��Ȃ����ߒ��ڑ��
        PlayerName.text = winner.ToString();
        ScoreAmounts.text = playerScore.ToString();
        KillAmounts.text = playerKill.ToString();
        AppleAmounts.text = playerApple.ToString();
        OrangeAmounts.text = playerOrange.ToString();
        GrapeAmounts.text = playerGrape.ToString();
    }
}

/// <summary>
/// ���҂�enum
/// </summary>
public enum Winner
{
    Player1,
    Player2,
}