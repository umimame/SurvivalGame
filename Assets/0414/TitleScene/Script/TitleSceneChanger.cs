using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneChanger : MonoBehaviour
{
    [SerializeField] private string changeScene;
    SceneBlackOut SceneBlackOut;
    // Start is called before the first frame update
    void Start()
    {
        SceneBlackOut = FindObjectOfType<SceneBlackOut>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            //何もしない
        }
        else
        {
            if (Input.anyKey)
            {
                Debug.Log("タイトル画面で何かキーが押されたよ");
                SceneBlackOut.BlackOutSceneChangeForTitle();
                //SceneManager.LoadScene("TestScene");
            }
        }
        //if (Input.GetKey(KeyCode.Escape))
        //{
        //    //何もしない
        //}
        //else
        //{
        //    if (Input.anyKey)
        //    {
        //        Debug.Log("タイトル画面で何かキーが押されたよ");
        //        SceneManager.LoadScene(changeScene);
        //    }
        //}
    }
}