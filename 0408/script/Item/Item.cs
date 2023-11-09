using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int score;
    public int Score { get { return score; } }
    // Start is called before the first frame update
   
    public void HitThePlayer(GameObject other)
    {
        if(other.CompareTag ("Player"))
        {
            Chara player = other.GetComponent<Chara>();
            if(player != null)
            {
                //player.AddScore(score);
            }
            Destroy(gameObject);
        }
    }
}