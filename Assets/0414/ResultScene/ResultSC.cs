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
    //[SerializeField] bool WinnerAlpha,//勝者は？
    //     PlayerNameAlpha,//プレイヤー
    //     KillAlpha, KillAmountsAlpha,//キル
    //     ScoreAlpha, ScoreAmountsAlpha,//スコア
    //     AppleAlpha, AppleAmountsAlpha, AppleImageAlpha,//リンゴ
    //     OrangeAlpha, OrangeAmountsAlpha, OrangeImageAlpha,//ミカン
    //     GrapeAlpha, GrapeAmountsAlpha, GrapeImageAlpha,//ブドウ
    //     PushAnyKeyAlpha;//PushtoTitle
    [SerializeField] bool[] ShowText = new bool[12];
    int NowIndex;


    // Start is called before the first frame update
    void Start()
    {
        //ShowTextをすべてfalseに
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

        //PlayerNameに表示する文字を設定,どっちが勝ったかで変えれるようにする。未実装
        PlayerName.text = "Player1";
        //キル数取得(変数で取る)未実装
        KillAmounts.text = "12";
        //スコア取得(変数でとる)未実装
        ScoreAmounts.text = "123";
        //リンゴ取得数、未実装
        AppleAmounts.text = "12";
        //ミカン取得数、未実装
        OrangeAmounts.text = "123";
        //ブドウ取得数、未実装
        GrapeAmounts.text = "12";
        //RGBそれぞれの初期値
        r = 1f;
        g = 0.5f;
        b = 0f;
        //RGBに加算する値をランダムで取得
        addR = Random.Range(0.001f, 0.0001f);
        addG = Random.Range(0.001f, 0.0001f);
        addB = Random.Range(0.001f, 0.0001f);
        //RGBそれぞれに加算するか減算するかの判定
        rAdd = true;
        gAdd = true;
        bAdd = true;
        //最初にTMProを透明に
        TMProTransparent();
        //時間でテキスト等の透明解除
        //StartCoroutine(TextControl());

        //StartCoroutine(ShowTextByTime());


        StartCoroutine(Test());
    }

    // Update is called once per frame
    void Update()
    {
        //PushtoTitleの色変え
        PushKeyColorChange();

        PushtoBoolInversion();

        //ボタンを押してシーン遷移
        //SceneChange();

        ShowAll();
    }
    void SceneChange()//シーン遷移用
    {
        if (Input.GetKey(KeyCode.Escape))//Escapeが押されたら
        {
            //Escapeが押されたときだけ何もしない
        }
        else
        {
            if (Input.anyKey)
            {
                SceneManager.LoadScene("TitleScene");
            }
        }
    }
    void TMProTransparent()//テキストなどを透明に(α値を0に)
    {
        Winner.alpha = 0f;
        //プレイヤーの名前
        PlayerName.color = Color.red;
        PlayerName.alpha = 0f;
        //キル
        Kill.alpha = 0f;
        KillAmounts.alpha = 0f;
        //スコア
        Score.alpha = 0f;
        ScoreAmounts.alpha = 0f;
        //リンゴ
        Apple.color = Color.red;
        Apple.alpha = 0f;
        AppleAmounts.alpha = 0f;
        AppleImage.color = new Color(1f, 1f, 1f, 0f);
        //みかん
        Orange.alpha = 0f;
        OrangeAmounts.alpha = 0f;
        OrangeImage.color = new Color(1f, 1f, 1f, 0f);
        //ブドウ
        Grape.color = new Color(1f, 0f, 1f, 0f);
        GrapeAmounts.alpha = 0f;
        GrapeImage.color = new Color(1f, 1f, 1f, 0f);
        //PushAnyKey
        PushAnyKey.alpha = 0f;
    }
    void PushtoBoolInversion()//Space押したらテキスト表示(未完成)
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

    private IEnumerator TextControl()//時間でテキストなどを表示(α値を1に)
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
        //リンゴ
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
        //ミカン
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
        //ブドウ
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

        //while (true)//常に実行
        //{
        //    // テキストを点滅させる
        //    PushAnyKey.alpha = 1f; // アルファ値を1に透明解除
        //    yield return new WaitForSeconds(0.5f); // 0.5秒待つ
        //    PushAnyKey.alpha = 0f; // アルファ値を0に透明になる
        //    yield return new WaitForSeconds(0.5f); // 0.5秒待つ
        //}
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
        //色変え
        PushAnyKey.color = new Color(r, g, b);
    }
}
