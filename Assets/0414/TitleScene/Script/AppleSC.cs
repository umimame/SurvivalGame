using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class AppleSC : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private float speed;
    private RectTransform canvasRect;

    // Start is called before the first frame update
    void Start()
    {
        //�����X�s�[�h�������_���Ŏ擾
        speed = Random.Range(0.05f, 0.1f);
        
        // Canvas��RectTransform���擾
        canvasRect = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveApple();
        DestroySelf();
    }
    void MoveApple()//�����S���~�点��
    {
        // transform���擾
        Transform myTransform = this.transform;

        // ���W���擾
        Vector3 pos = myTransform.position;
        myTransform.transform.SetParent(canvasRect, false);

        pos.y -= speed;    // y���W��speed���ړ�
        myTransform.position = pos;  // ���W��ݒ�
        Debug.Log("posY:" + pos.y);
        if (pos.y <= 100)//�����S���n�ʂɂ�����
        {
            speed = 0f;
            time = time + Time.deltaTime;
        }
    }
    void DestroySelf()//�����S���n�ʂɗ����Ă��玩�g���E��
    {
        if (time > 3f)
        {
            Destroy(this.gameObject);
        }
    }
}
