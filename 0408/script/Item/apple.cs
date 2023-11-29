using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple: Item
{
    public Apple()
    {
        score = 1;
        onHitPlayer += OnHitApple;
    }
    // Update is called once per frame

    public void OnHitApple(GameObject other)
    {
        Chara_Player player = other.GetComponent<Chara_Player>();
        //player.AddScore(score);
        Debug.Log(score);
        Debug.Log("Apple");
    }
}