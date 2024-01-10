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
    [Tooltip("����������G�t�F�N�g(�p�[�e�B�N��)")]
    private ParticleSystem particle;
    
    public AudioClip ItemSE;

    // ���R�ɂ��ǋL
    [SerializeField] protected ResultParamOperator resultParam;   // ResultScene�ɓn�����߂ɕK�v
    private void Start()
    {
        resultParam = GameObject.FindWithTag(Tags.SceneOperator).GetComponent<ResultParamOperator>();
    }
    // �ǋL�I��
    // �p����̊֐���ResultParamOperator�̊֐������s���鏈����ǋL

    void OnTriggerStay(Collider other)
    {
       // ���R�ɂ��ύX
       // Stay�����s�����O��Enter�ɂ��I�u�W�F�N�g�����ł��邽�߁A
       // Stay�ɏ����ꂽ���������ׂ�Enter�Ɉڂ���
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