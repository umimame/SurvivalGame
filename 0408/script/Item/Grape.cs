using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape : Item
{
    public Grape()
    {
        score = 3;
    }
    // Update is called once per frame
    public override void HitThePlayer(player other)
    {
        if (other.CompareTag("Player"))
        {
            //Chara player = other.GetComponent<Chara>();
            //if (player != null)
            //{
                //player.AddScore(score);
                //player.ApplyPowerup("Grape");
                Debug.Log(score);
            //}
            Destroy(gameObject);
        }
    }
}
