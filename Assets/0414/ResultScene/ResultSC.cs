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
    [SerializeField] private float r, g, b, addR, addG, addB;
    [SerializeField] bool[] ShowText = new bool[12];
    bool rAdd, gAdd, bAdd, SceneChangeFlag;
    int NowIndex;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerScript playerScript = FindAnyObjectByType<PlayerScript>();

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

        //色々な物の初期化
        TMProTransparent();
        //時間でテキスト等の透明解除
        StartCoroutine(ShowTextByTime());
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
                    SceneManager.LoadScene("TitleScene");
                }
            }
        }
    }
    void TMProTransparent()//テキストなどを透明に(α値を0に)
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
        //色変え
        PushAnyKey.color = new Color(r, g, b);
    }
    //void GetScore()
    //{
    //    int playerScore = playerScript.GetPlayerScore();
    //    ScoreAmounts.text = playerScore.ToSafeString();
    //}
    
    //void GetScore2(Chara_Player player)
    //{
    //    ScoreAmounts.text = player.Score.ToSafeString();
    //}
}
