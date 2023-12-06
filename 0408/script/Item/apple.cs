using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple: Item
{
    public Apple()
    {
        score = 5;
        onHitPlayer += OnHitApple;
    }
    // Update is called once per frame

    public void OnHitApple(Chara_Player player)
    {
        player.AddScore(score);
    }
}