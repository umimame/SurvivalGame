using UnityEngine;

public class player : MonoBehaviour
{
    //�ړ����x
    [SerializeField]private float _speed = 3.0f;
    [SerializeField]private float getpoint = 0;
    [SerializeField] private GameObject Item;
    //x�������̓��͂�ۑ�
    private float _input_x;
    //z�������̓��͂�ۑ�
    private float _input_z;

    void Update()
    {
        //x�������Az�������̓��͂��擾
        //Horizontal�A�����A�������̃C���[�W
        _input_x = Input.GetAxis("Horizontal");
        //Vertical�A�����A�c�����̃C���[�W
        _input_z = Input.GetAxis("Vertical");

        //�ړ��̌����ȂǍ��W�֘A��Vector3�ň���
        Vector3 velocity = new Vector3(_input_x, 0, _input_z);
        //�x�N�g���̌������擾
        Vector3 direction = velocity.normalized;

        //�ړ��������v�Z
        float distance = _speed * Time.deltaTime;
        //�ړ�����v�Z
        Vector3 destination = transform.position + direction * distance;

        //�ړ���Ɍ����ĉ�]
        transform.LookAt(destination);
        //�ړ���̍��W��ݒ�
        transform.position = destination;
    }
    public void TagJudge(GameObject Item, float point)
    {
        if (Item.CompareTag("Item"))
        {
            transform.GetComponent<Item>();
            getpoint += point;
        }
    }

}