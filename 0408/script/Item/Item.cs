using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] protected int score;
    public int Score { get { return score; } }

    public Action<GameObject> onHitPlayer;
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
            Chara_Player player = other.GetComponent<Chara_Player>();
            if(player != null)
            {
                onHitPlayer?.Invoke(other);
            }
            Destroy(gameObject);
        }
    }
}