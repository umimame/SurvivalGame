using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnPos : MonoBehaviour
{
    //���X�|�[���ʒu�̐ݒ�
    [SerializeField] private Transform[] RespawnPos;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 PlayerRespawn()
    {
        //���X�|�[��������ʒu�������_���Őݒ�
        int RandomRespawn = Random.Range(0, RespawnPos.Length);
        Vector3 newPos = RespawnPos[RandomRespawn].position;
        return newPos;
    }
}