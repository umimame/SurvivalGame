using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// �ʂ̃I�u�W�F�N�g�ɃA�^�b�`���ꂽ�R���C�_�[�Ŋ֐������s�ł���
/// </summary>
public class ColliderChecker : MonoBehaviour
{
    [field: SerializeField] public UnityEvent<Collider> colliderEnterEvent;
    [field: SerializeField] public UnityEvent<Collider> colliderStayEvent;
    [field: SerializeField] public UnityEvent<Collider> colliderExitEvent;
    private void OnTriggerEnter(Collider other)
    {
         colliderEnterEvent?.Invoke(other);
        
    }

    private void OnTriggerStay(Collider other)
    {
        colliderStayEvent?.Invoke(other);
        //Debug.Log("Stay");
    }

    private void OnTriggerExit(Collider other)
    {
        colliderExitEvent?.Invoke(other);
        //Debug.Log("Exit");
    }

}
