using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPlayer : MonoBehaviour
{
    [SerializeField] int kill, score, apple, orange, grape;
    private static ResultPlayer instance; // シングルトンインスタンス
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("シングルトンにしたよ");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("ResultPlayerでシングルトンを破棄したよ");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.X))
        {
            Debug.Log("AddNownOwnoW");
            kill += 5;
            score += 100;
            apple += 1;
            orange += 2;
            grape += 3;
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            ResetScore();
        }
    }
    //public int GetScore() { return score; }
    //public int GetApple() { return apple; }
    //public int GetOrange() { return orange; }
    //public int GetGrape() { return grape; }
    //public int GetKill() { return kill; }
    void ResetScore()
    {
        score = 0;
        kill = 0;
        apple = 0;
        orange = 0;
        grape = 0;
    }
    // シングルトンの解除（破棄）メソッド
    public void ReleaseSingleton()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = null;
        }
    }      
}
