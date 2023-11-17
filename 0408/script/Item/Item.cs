using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] protected int score;
    public int Score { get { return score; } }

    // Start is called before the first frame update

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter: " + other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            Item item = GetComponent<Item>();
            if (item != null)
            {
                item.HitThePlayer(other.gameObject);
            }
        }
    }
    public virtual void HitThePlayer(GameObject other)
    {
        if(other.gameObject.CompareTag ("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if(player != null)
            {
                player.AddScore(score);
                Debug.Log(score);
                
            }
            Destroy(gameObject);
        }
    }
}