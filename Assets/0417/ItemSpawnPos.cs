using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemSpawnPos : MonoBehaviour
{
    [SerializeField]
    private GameObject createPrefab;
    [SerializeField]
    private Transform range1A;
    [SerializeField]
    private Transform range1B;
    [SerializeField]
    private Transform range2A;
    [SerializeField]
    private Transform range2B;
    [SerializeField]
    private Transform range3A;
    [SerializeField]
    private Transform range3B;
    [SerializeField]
    private Transform range4A;
    [SerializeField]
    private Transform range4B;
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

            //ItemRespawn1�ɏo��
            if (RandomArea <= 6)
            {
                // range1A��range1B��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float x1 = Random.Range(range1A.position.x, range1B.position.x);
                // range1A��rang1eB��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float y1 = Random.Range(range1A.position.y, range1B.position.y);
                // range1A��range1B��z���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float z1 = Random.Range(range1A.position.z, range1B.position.z);
                // GameObject����L�Ō��܂��������_���ȏꏊ�ɐ���
                Instantiate(createPrefab, new Vector3(x1, y1, z1), createPrefab.transform.rotation);
            }
            //ItemRespawn1�ɏo��
            else if (RandomArea == 7)
            {
                // range2A��range2B��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float x2 = Random.Range(range2A.position.x, range2B.position.x);
                // range2A��range2B��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float y2 = Random.Range(range2A.position.y, range2B.position.y);
                // range2A��range2B��z���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float z2 = Random.Range(range2A.position.z, range2B.position.z);
                Instantiate(createPrefab, new Vector3(x2, y2, z2), createPrefab.transform.rotation);
            }
            //ItemRespawn3�ɏo��
            else if (RandomArea == 8)
            {
                // range3A��range3B��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float x3 = Random.Range(range3A.position.x, range3B.position.x);
                // range3A��range3B��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float y3 = Random.Range(range3A.position.y, range3B.position.y);
                // range3A��range3B��z���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float z3 = Random.Range(range3A.position.z, range3B.position.z);
                Instantiate(createPrefab, new Vector3(x3, y3, z3), createPrefab.transform.rotation);
            }
            //ItemRespawn4�ɏo��
            else
            {
                // range4A��range4B��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float x4 = Random.Range(range4A.position.x, range4B.position.x);
                // range4A��range4B��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float y4 = Random.Range(range4A.position.y, range4B.position.y);
                // range4A��range4B��z���W�͈͓̔��Ń����_���Ȑ��l���쐬
                float z4 = Random.Range(range4A.position.z, range4B.position.z);
                Instantiate(createPrefab, new Vector3(x4, y4, z4), createPrefab.transform.rotation);
            }

            // �o�ߎ��ԃ��Z�b�g
            time = 0f;
        }
    }
}
