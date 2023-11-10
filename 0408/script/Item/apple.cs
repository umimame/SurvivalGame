using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class apple : Item
{
    // Start is called before the first frame update
    public apple()
    {
        score = 5;
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
                //player.ApplyPowerup("apple");
                Debug.Log(score);
            //}
            Destroy(gameObject);
        }
    }
}
