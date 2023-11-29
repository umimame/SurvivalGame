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

    public void OnHitOrange(GameObject other)
    {
        Chara_Player player = other.GetComponent<Chara_Player>();
        player.AddScore(score);
        Debug.Log(score);
        Debug.Log("Orange");
    }
}
