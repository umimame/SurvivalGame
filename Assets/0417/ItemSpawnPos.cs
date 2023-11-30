using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemSpawnPos : MonoBehaviour
{
    [SerializeField] private GameObject[] createItem;
    [SerializeField] private Transform[] ranges;
    // �o�ߎ���
    private float time;
    //���X�|�[���̃^�C�~���O
    [SerializeField]
    private float RespawnTime = 1.0f;

    // Update is called once per frame
    void Update()
    {
        
        // �O�t���[������̎��Ԃ����Z���Ă���
        time = time + Time.deltaTime;

        // ��1�b�u���Ƀ����_���ɐ��������悤�ɂ���B
        if (time > RespawnTime)
        {
            //�A�C�e���o���̃G���A�𒊑I
            int RandomArea = Random.Range(0, 10);
            //�A�C�e���������_���őI��
            int RandomItem = Random.Range(0, 3);

            //ItemRespawn1�ɏo��
            if (RandomArea <= 6)
            {
                // range1A��range1B��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float x1 = Random.Range(ranges[0].position.x, ranges[1].position.x);
                // range1A��rang1eB��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float y1 = Random.Range(ranges[0].position.y, ranges[1].position.y);
                // range1A��range1B��z���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float z1 = Random.Range(ranges[0].position.z, ranges[1].position.z);
                // GameObject����L�Ō��܂��������_���ȏꏊ�ɐ���
                Instantiate(createItem[RandomItem], new Vector3(x1, y1, z1), createItem[RandomItem].transform.rotation);
            }
            //ItemRespawn1�ɏo��
            else if (RandomArea == 7)
            {
                // range2A��range2B��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float x2 = Random.Range(ranges[2].position.x, ranges[3].position.x);
                // range2A��range2B��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float y2 = Random.Range(ranges[2].position.y, ranges[3].position.y);
                // range2A��range2B��z���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float z2 = Random.Range(ranges[2].position.z, ranges[3].position.z);
                Instantiate(createItem[RandomItem], new Vector3(x2, y2, z2), createItem[RandomItem].transform.rotation);
            }
            //ItemRespawn3�ɏo��
            else if (RandomArea == 8)
            {
                // range3A��range3B��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float x3 = Random.Range(ranges[4].position.x, ranges[5].position.x);
                // range3A��range3B��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float y3 = Random.Range(ranges[4].position.y, ranges[5].position.y);
                // range3A��range3B��z���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float z3 = Random.Range(ranges[4].position.z, ranges[5].position.z);
                Instantiate(createItem[RandomItem], new Vector3(x3, y3, z3), createItem[RandomItem].transform.rotation);
            }
            //ItemRespawn4�ɏo��
            else
            {
                // range4A��range4B��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float x4 = Random.Range(ranges[6].position.x, ranges[7].position.x);
                // range4A��range4B��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float y4 = Random.Range(ranges[6].position.y, ranges[7].position.y);
                // range4A��range4B��z���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float z4 = Random.Range(ranges[6].position.z, ranges[7].position.z);
                Instantiate(createItem[0], new Vector3(x4, y4, z4), createItem[0].transform.rotation);
            }

            // �o�ߎ��ԃ��Z�b�g
            time = 0f;
        }
    }
}
