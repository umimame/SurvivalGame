using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TextManager:MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PushAnyKey;
    public void Start()
    {
        StartCoroutine(BlinkText());    
    }
    IEnumerator BlinkText()//テキスト点滅用
    {
        while (true)
        {
            // テキストを点滅させる
            PushAnyKey.alpha = 0f; // アルファ値を0に透明になる
            yield return new WaitForSeconds(0.5f); // 0.5秒待つ

            PushAnyKey.alpha = 1f; // アルファ値を1に透明解除
            yield return new WaitForSeconds(0.5f); // 0.5秒待つ
        }
    }
}
