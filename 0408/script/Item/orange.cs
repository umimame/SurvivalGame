using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Orange : Item
{
    public Orange()
    {
        score = 1;
        onHitPlayer += OnHitOrange;
    }
    // Update is called once per frame

    public void OnHitOrange(Chara_Player player)
    {
        player.AddScore(score);
        resultParam.AddOrange();
    }
}
