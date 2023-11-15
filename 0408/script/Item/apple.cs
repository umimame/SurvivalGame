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
    // Update is called once per frame
    public override void HitThePlayer(GameObject other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.AddScore(score);
                Debug.Log(score);
                Debug.Log("apple");
            }
            Destroy(gameObject);
        }
    }
}
