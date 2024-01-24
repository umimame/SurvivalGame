using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// 別のオブジェクトにアタッチされたコライダーで関数を実行できる
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
