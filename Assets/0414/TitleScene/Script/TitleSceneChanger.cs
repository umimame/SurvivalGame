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
            //�������Ȃ�
        }
        else
        {
            if (Input.anyKey)
            {
                Debug.Log("�^�C�g����ʂŉ����L�[�������ꂽ��");
                SceneBlackOut.BlackOutSceneChangeForTitle();
                //SceneManager.LoadScene("TestScene");
            }
        }
        //if (Input.GetKey(KeyCode.Escape))
        //{
        //    //�������Ȃ�
        //}
        //else
        //{
        //    if (Input.anyKey)
        //    {
        //        Debug.Log("�^�C�g����ʂŉ����L�[�������ꂽ��");
        //        SceneManager.LoadScene(changeScene);
        //    }
        //}
    }
}