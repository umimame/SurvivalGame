using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnPos : MonoBehaviour
{
    //�v���C���[Object
    [SerializeField] private GameObject[] createPlayer;
    //���X�|�[���ʒu�̐ݒ�
    [SerializeField] private Transform[] RespawnPos;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //Z�L�[�Ń��X�|�[��
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayerRespawn(0);
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            PlayerRespawn(1);
        }
    }

    void PlayerRespawn(int player)
    {
        //���X�|�[��������ʒu�������_���Őݒ�
        int RandomRespawn = Random.Range(0, RespawnPos.Length);
        float x1 = RespawnPos[RandomRespawn].position.x;
        float y1 = RespawnPos[RandomRespawn].position.y;
        float z1 = RespawnPos[RandomRespawn].position.z;
        //�v���C���[�̃C���X�^���X�𐶐�     
         Instantiate(createPlayer[player], new Vector3(x1, y1, z1), createPlayer[player].transform.rotation);
    }
}