using UnityEngine;
using UnityEngine.Events;

public class ColliderChecker : MonoBehaviour
{
    public enum OnColliderType
    {
        Enter,
        Stay,
        Exit,
    }
    [SerializeField] private OnColliderType onColliderType;
    [field: SerializeField] public UnityEvent<Collider> colliderEvent;
    private void OnTriggerEnter(Collider other)
    {
        if(onColliderType == OnColliderType.Enter) { colliderEvent.Invoke(other); }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(onColliderType == OnColliderType.Stay) { colliderEvent.Invoke(other); }
        
    }

    private void OnTriggerExit(Collider other)
    {

        if (onColliderType == OnColliderType.Exit) { colliderEvent.Invoke(other); }
    }
}
