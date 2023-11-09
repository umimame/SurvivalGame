using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orange : Item
{
    // Start is called before the first frame update
    public orange()
    {
        score = 1;
    }
    // Update is called once per frame
    public override void HitThePlayer(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            //Chara player = other.GetComponent<Chara>();
            //if (player != null)
            //{
                //player.AddScore(score);
                //player.ApplyPowerup("orange");
                Debug.Log(score);
            //}
            Destroy(gameObject);
        }
    }
}
