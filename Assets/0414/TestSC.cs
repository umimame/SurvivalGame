using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSC : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

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
                SceneManager.LoadScene("ResultScene");
            }
        }
    }
}
