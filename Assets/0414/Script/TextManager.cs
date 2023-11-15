using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class TextManager:MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PushAnyKey;
    public void Start()
    {
        StartCoroutine(BlinkText());
    }
    private void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene("TestScene");
        }
    }
   
IEnumerator BlinkText()//�e�L�X�g�_�ŗp
    {
        while (true)
        { 
            // �e�L�X�g��_�ł�����

           PushAnyKey.alpha = 1f; // �A���t�@�l��1�ɓ�������
            yield return new WaitForSeconds(0.5f); // 0.5�b�҂�
            PushAnyKey.alpha = 0f; // �A���t�@�l��0�ɓ����ɂȂ�
            yield return new WaitForSeconds(0.5f); // 0.5�b�҂�

            
        }
    }
}