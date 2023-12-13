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
    //Chara_Playerオブジェクトを取得
    ResultPlayer charaPlayer;
    // Start is called before the first frame update
    void Start()
    {
        charaPlayer = FindObjectOfType<ResultPlayer>();

        //色々な物の初期化
        TMProTransparent();
        //時間でテキスト等の透明解除
        StartCoroutine(ShowTextByTime());

        StartCoroutine(PushKeyBlinking());

        SceneChangeFlag = false;

        GetPlayerScore();
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
    void SceneChange()//シーン遷移用
    {
        if (SceneChangeFlag)
        {
            if (Input.GetKey(KeyCode.Escape))//Escapeが押されたら
            {
                //Escapeが押されたときだけ何もしない
            }
            else
            {
                if (Input.anyKey)//一部反応しないキーがある
                {
                    charaPlayer.SetScore();//シーン遷移前にスコア等の初期化

                    SceneManager.LoadScene("TitleScene");
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
        r = 1f;
        g = 0.5f;
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
    private IEnumerator PushKeyBlinking()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Alpha = 1f;//透明化解除
            yield return new WaitForSeconds(1f);
            Alpha = 0f;//透明化
        }
    }
    void GetPlayerScore()
    {
        if (charaPlayer != null)
        {
            Debug.Log("Chara_Playerが見つかりましたわよ～～お～～ほっほっほ～～～");
            //Chara_PlayerのGet~を呼び出す
            playerScore = charaPlayer.GetScore();
            playerKill = charaPlayer.GetKill();
            playerApple = charaPlayer.GetApple();
            playerOrange = charaPlayer.GetOrange();
            playerGrape = charaPlayer.GetGrape();
            //スコアを文字に変換
            ScoreAmounts.text = playerScore.ToString();
            KillAmounts.text = playerKill.ToString();
            AppleAmounts.text = playerApple.ToString();
            OrangeAmounts.text = playerOrange.ToString();
            GrapeAmounts.text = playerGrape.ToString();
            //ScoreやKill等を0にする(SceneChange前に実装済み)
            //charaPlayer.SetScore();
        }
        else
        {
            Debug.Log("Chara_Playerが見つからないよ");
        }
    }    
//会
//い
//た
//く
//て
//「
//I
//M
//i
//S
//S
//Y
//o
//u
//」
//
}
