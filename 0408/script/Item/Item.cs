using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] protected int score;
    public int Score { get { return score; } }

    public Action<Chara_Player> onHitPlayer;
    // Start is called before the first frame update

    

    void OnTriggerStay(Collider other)
    {

        if (other.tag == Tags.Player01 || other.tag == Tags.Player02)
        {
            HitThePlayer(other.gameObject);
            GetComponent<ParticleSystem>().Play();
        }
    }

    private void OnCollisionEnter(Collision other)
    {


        if (other.gameObject.tag == Tags.Player01 || other.gameObject.tag == Tags.Player02)
        {
            HitThePlayer(other.gameObject);
        }
    }
    public virtual void HitThePlayer(GameObject other)
    {
        if(other.tag == Tags.Player01 || other.tag == Tags.Player02)
        {
            Chara_Player player = other.transform.root.GetComponentInChildren<Chara_Player>();
            if(player != null)
            {
                onHitPlayer?.Invoke(player);
            }
            Destroy(gameObject);
        }
    }
}