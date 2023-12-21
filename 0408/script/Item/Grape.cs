using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape : Item
{
    public Grape()
    {
        score = 3;
        onHitPlayer += OnHitGrape;
    }
    // Update is called once per frame

    public void OnHitGrape(Chara_Player player)
    {
        player.AddScore(score);
        resultParam.AddGrape();
    }
}
