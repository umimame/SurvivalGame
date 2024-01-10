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
    //Chara_Playerオブジェクトを取得
    SceneBlackOut SceneBlackOut;
    // Start is called before the first frame update
    void Start()
    {
        SceneBlackOut = FindObjectOfType<SceneBlackOut>();
        //色々な物の初期化
        TMProTransparent();
        //時間でテキスト等の透明解除
        StartCoroutine(ShowTextByTime());
        //PushKeyの透明操作
        StartCoroutine(PushKeyBlinking());
        //シーン遷移をするかどうか
        SceneChangeFlag = false;
    }
    // Update is called once per frame
    void Update()
    {
        //PushtoTitleの色変え
        PushKeyColorChange();
        //Space押したらテキスト表示用のboolを反転
        PushtoBoolInversion();
        //ボタンを押してシーン遷移
        SceneChange();
        //画像とテキストの表示
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
            Debug.Log("Fire0が押されたよ");
            return true;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Fire1が押されたよ");
            return true;
        }
        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("Fire2が押されたよ");
            return true;
        }
        if (Input.GetButtonDown("Fire3"))
        {
            Debug.Log("Fire3が押されたよ");
            return true;
        }
        //float horizontalInput = Input.GetAxis("Horizontal");
        //if (horizontalInput>0)
        //{
        //    Debug.Log("右方向の入力あり申す");
        //    return true;
        //}
        return false;
    }
    void SceneChange()//シーン遷移用
    {
        if (SceneChangeFlag)
        {
            if (Input.GetKey(KeyCode.Escape))//Escapeが押されたら
            {
                //Escapeが押されたときだけ何もしない
                Debug.Log("Escape押しても何もないわよ～～～");
            }
            else
            {
                if (Input.anyKey)//一部反応しないキーがある
                {
                    //charaPlayer.SetScore();//シーン遷移前にスコア等の初期化
                    Debug.Log("ResultSCでシングルトン破棄");
                    SceneBlackOut.BlackOutSceneChangeForResult();
                    //SceneManager.LoadScene("TitleScene");
                }
            }
        }
    }
    void TMProTransparent()//色々な物の初期化
    {
        //ShowTextをすべてfalseに,全部透明に
        for (int i = 0; i < ShowText.Length; i++)
        {
            ShowText[i] = false;
            AllText[i].alpha = 0f;
        }
        //リンゴの画像
        AppleImage.color = new Color(1f, 1f, 1f, 0f);
        //ミカンの画像
        OrangeImage.color = new Color(1f, 1f, 1f, 0f);
        //ブドウの画像
        GrapeImage.color = new Color(1f, 1f, 1f, 0f);
        //PushAnyKeyの初期化
        Alpha = 0f;
        r = 0f;
        g = 0f;
        b = 0f;
        PushAnyKey.color = new Color(r, g, b, Alpha);//最初は透明
        //RGBに加算する値をランダムで取得
        addR = Random.Range(0.001f, 0.00001f);
        addG = Random.Range(0.001f, 0.00001f);
        addB = Random.Range(0.001f, 0.00001f);
        //RGBそれぞれに加算するか減算するかの判定
        rAdd = true;
        gAdd = true;
        bAdd = true;
        playerScore = 0;
    }
    void PushtoBoolInversion()//Space押したらテキスト表示フラグをtrueに変更
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
    void ShowAll()//テキスト等の表示処理
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
    private IEnumerator ShowTextByTime()//時間経過でテキスト等の表示
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
    void PushKeyColorChange()//PushAnyKeyの色変え
    {
        //Rに加算
        if (rAdd)
        {
            r += addR;
            if (r >= 1f) { rAdd = false; }
        }
        if (!rAdd)//Rに減算
        {
            r -= addR;
            if (r <= 0f) { rAdd = true; }
        }
        //Gに加算
        if (gAdd)
        {
            g += addG;
            if (g >= 1f) { gAdd = false; }
        }
        if (!gAdd)//Gに減算
        {
            g -= addG;
            if (g <= 0f) { gAdd = true; }
        }
        //Bに加算
        if (bAdd)
        {
            b += addB;
            if (b >= 1f) { bAdd = false; }
        }
        if (!bAdd)//Bに減算
        {
            b -= addB;
            if (b <= 0f) { bAdd = true; }
        }
        if (SceneChangeFlag)
        {
            //色変えと透明化
            PushAnyKey.color = new Color(r, g, b, Alpha);
        }
    }
    private IEnumerator PushKeyBlinking()//PushKeyの透明操作
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Alpha = 1f;//透明化解除
            yield return new WaitForSeconds(1f);
            Alpha = 0f;//透明化
        }
    }
    //プレイヤーが呼ぶ用の関数
    // 畠山追記
    // 引数に勝者を設定出来るよう追記
    public void SetResult(Winner winner, int score, int kill, int apple, int orange, int grape)
    {
        playerScore = score;
        playerKill = kill;
        playerApple = apple;
        playerOrange = orange;
        playerGrape = grape;

        // 上記に代入した変数が使用されていないため直接代入
        PlayerName.text = winner.ToString();
        ScoreAmounts.text = playerScore.ToString();
        KillAmounts.text = playerKill.ToString();
        AppleAmounts.text = playerApple.ToString();
        OrangeAmounts.text = playerOrange.ToString();
        GrapeAmounts.text = playerGrape.ToString();
    }
}

/// <summary>
/// 勝者のenum
/// </summary>
public enum Winner
{
    Player1,
    Player2,
}