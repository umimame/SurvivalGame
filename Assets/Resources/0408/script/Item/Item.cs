using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] protected int score;
    public int Score { get { return score; } }

    public Action<Chara_Player> onHitPlayer;

    [SerializeField]
    [Tooltip("発生させるエフェクト(パーティクル)")]
    private ParticleSystem particle;
    
    public AudioClip ItemSE;

    // 畠山による追記
    [SerializeField] protected ResultParamOperator resultParam;   // ResultSceneに渡すために必要
    private void Start()
    {
        resultParam = GameObject.FindWithTag(Tags.SceneOperator).GetComponent<ResultParamOperator>();
    }
    // 追記終了
    // 継承先の関数でResultParamOperatorの関数を実行する処理を追記

    void OnTriggerStay(Collider other)
    {
       // 畠山による変更
       // Stayが実行される前にEnterによりオブジェクトが消滅するため、
       // Stayに書かれた処理をすべてEnterに移した
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == Tags.Player01 || other.gameObject.tag == Tags.Player02)
        {
            ParticleSystem newParticle = Instantiate(particle);
            newParticle.transform.position = this.transform.position;
            newParticle.Play();
            AudioSource.PlayClipAtPoint(ItemSE, transform.position);
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