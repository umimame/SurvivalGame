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
        PlayerController player = other.GetComponent<PlayerController>();
        player.AddScore(score);
        Debug.Log(score);
        Debug.Log("orange");
    }
}
