using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemSpawnPos : MonoBehaviour
{
    [SerializeField] private GameObject[] createItem;
    [SerializeField] private Transform[] ranges;
    private GameObject[] Item;
    
    // �o�ߎ���
    private float time;
    //���X�|�[���̃^�C�~���O
    [SerializeField]
    private float RespawnTime = 1.0f;
    [SerializeField] private int ItemValue;

    //�}�b�v�ɑ��݂ł���A�C�e���̌���1000(��)�܂łɂ�����
    //���łɃ}�b�v�ɐ�������Ă���A�C�e�����l������
    //��@�}�b�v�ɂ�1000�̃A�C�e������������10�v���C�����擾��990�ɂȂ���
    //���̏ꍇ�̓A�C�e����10��������

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
            int RandomItem = Random.Range(0, createItem.Length);

            if (Check("Item") < ItemValue)
            {
                //ItemRespawn1�ɏo��
                if (RandomArea <= 6)
                {
                    // range[0]��range[1]��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
                    float x1 = Random.Range(ranges[0].position.x, ranges[1].position.x);
                    // range[0]��range[1]��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
                    float y1 = Random.Range(ranges[0].position.y, ranges[1].position.y);
                    // range[0]��range[1]��z���W�͈͓̔��Ń����_���Ȑ��l���쐬
                    float z1 = Random.Range(ranges[0].position.z, ranges[1].position.z);
                    // GameObject����L�Ō��܂��������_���ȏꏊ�ɐ���
                    Instantiate(createItem[RandomItem], new Vector3(x1, y1, z1), createItem[RandomItem].transform.rotation);
                }
                //ItemRespawn1�ɏo��
                else if (RandomArea == 7)
                {
                    // range[2]��range[3]��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
                    float x2 = Random.Range(ranges[2].position.x, ranges[3].position.x);
                    // range[2]��range[3]��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
                    float y2 = Random.Range(ranges[2].position.y, ranges[3].position.y);
                    // range[2]��range[3]��z���W�͈͓̔��Ń����_���Ȑ��l���쐬
                    float z2 = Random.Range(ranges[2].position.z, ranges[3].position.z);
                    Instantiate(createItem[RandomItem], new Vector3(x2, y2, z2), createItem[RandomItem].transform.rotation);
                }
                //ItemRespawn3�ɏo��
                else if (RandomArea == 8)
                {
                    // range[4]��range[5]��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
                    float x3 = Random.Range(ranges[4].position.x, ranges[5].position.x);
                    // range[4]��range[5]��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
                    float y3 = Random.Range(ranges[4].position.y, ranges[5].position.y);
                    // range[4]��range[5]��z���W�͈͓̔��Ń����_���Ȑ��l���쐬
                    float z3 = Random.Range(ranges[4].position.z, ranges[5].position.z);
                    Instantiate(createItem[RandomItem], new Vector3(x3, y3, z3), createItem[RandomItem].transform.rotation);
                }
                //ItemRespawn4�ɏo��
                else
                {
                    // range[6]��range[7]��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
                    float x4 = Random.Range(ranges[6].position.x, ranges[7].position.x);
                    // range[6]��range[7]��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
                    float y4 = Random.Range(ranges[6].position.y, ranges[7].position.y);
                    // range[6]��range[7]��z���W�͈͓̔��Ń����_���Ȑ��l���쐬
                    float z4 = Random.Range(ranges[6].position.z, ranges[7].position.z);
                    Instantiate(createItem[RandomItem], new Vector3(x4, y4, z4), createItem[RandomItem].transform.rotation);
                }
            }

            ////ItemRespawn1�ɏo��
            //if (RandomArea <= 6)
            //{
            //    // range[0]��range[1]��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
            //    float x1 = Random.Range(ranges[0].position.x, ranges[1].position.x);
            //    // range[0]��range[1]��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
            //    float y1 = Random.Range(ranges[0].position.y, ranges[1].position.y);
            //    // range[0]��range[1]��z���W�͈͓̔��Ń����_���Ȑ��l���쐬
            //    float z1 = Random.Range(ranges[0].position.z, ranges[1].position.z);
            //    // GameObject����L�Ō��܂��������_���ȏꏊ�ɐ���
            //    Instantiate(createItem[RandomItem], new Vector3(x1, y1, z1), createItem[RandomItem].transform.rotation);
            //}
            ////ItemRespawn1�ɏo��
            //else if (RandomArea == 7)
            //{
            //    // range[2]��range[3]��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
            //    float x2 = Random.Range(ranges[2].position.x, ranges[3].position.x);
            //    // range[2]��range[3]��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
            //    float y2 = Random.Range(ranges[2].position.y, ranges[3].position.y);
            //    // range[2]��range[3]��z���W�͈͓̔��Ń����_���Ȑ��l���쐬
            //    float z2 = Random.Range(ranges[2].position.z, ranges[3].position.z);
            //    Instantiate(createItem[RandomItem], new Vector3(x2, y2, z2), createItem[RandomItem].transform.rotation);
            //}
            ////ItemRespawn3�ɏo��
            //else if (RandomArea == 8)
            //{
            //    // range[4]��range[5]��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
            //    float x3 = Random.Range(ranges[4].position.x, ranges[5].position.x);
            //    // range[4]��range[5]��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
            //    float y3 = Random.Range(ranges[4].position.y, ranges[5].position.y);
            //    // range[4]��range[5]��z���W�͈͓̔��Ń����_���Ȑ��l���쐬
            //    float z3 = Random.Range(ranges[4].position.z, ranges[5].position.z);
            //    Instantiate(createItem[RandomItem], new Vector3(x3, y3, z3), createItem[RandomItem].transform.rotation);
            //}
            ////ItemRespawn4�ɏo��
            //else
            //{
            //    // range[6]��range[7]��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
            //    float x4 = Random.Range(ranges[6].position.x, ranges[7].position.x);
            //    // range[6]��range[7]��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
            //    float y4 = Random.Range(ranges[6].position.y, ranges[7].position.y);
            //    // range[6]��range[7]��z���W�͈͓̔��Ń����_���Ȑ��l���쐬
            //    float z4 = Random.Range(ranges[6].position.z, ranges[7].position.z);
            //    Instantiate(createItem[RandomItem], new Vector3(x4, y4, z4), createItem[RandomItem].transform.rotation);
            //}
            // �o�ߎ��ԃ��Z�b�g
            time = 0f;
        }
    }

    //�}�b�v�ɂ���I�u�W�F�N�g�̌��𐔂���
    int Check(string item)
    {
        int value = 0;
        Item = GameObject.FindGameObjectsWithTag(item);
        value = Item.Length;
        return value;
    }
}
