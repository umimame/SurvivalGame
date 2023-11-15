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

    public override void HitThePlayer(GameObject other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.AddScore(score);
                Debug.Log(score);
                Debug.Log("orange");
            }
            Destroy(gameObject);
        }
    }
}
