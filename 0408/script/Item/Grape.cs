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

    public void OnHitGrape(GameObject other)
    {
        Chara_Player player = other.GetComponent<Chara_Player>();
        player.AddScore(score);
        Debug.Log(score);
        Debug.Log("Grape");
    }
}
