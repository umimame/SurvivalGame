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
        //���X�|�[��������ʒu�������_���Őݒ�
        int RandomRespawn = Random.Range(0, RespawnPos.Length);
        float x1 = RespawnPos[RandomRespawn].position.x;
        float y1 = RespawnPos[RandomRespawn].position.y;
        float z1 = RespawnPos[RandomRespawn].position.z;
        //Z�L�[�Ń��X�|�[��
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Instantiate(createPlayer[0], new Vector3(x1, y1, z1), createPlayer[0].transform.rotation);
        }
    }
}
