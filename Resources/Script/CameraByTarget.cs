using UnityEngine;

public class CameraByTarget : MonoBehaviour
{
    [field: SerializeField] public GameObject target { get; set; }
    [SerializeField] private Vector3 adjustPos;
    [SerializeField, NonEditable] private Vector3 adulation;
    [SerializeField] private float adulationPer;
    private void Start()
    {
        gameObject.transform.position = target.transform.position + adjustPos;
    }

    private void Update()
    {
    }
    private void FixedUpdate()
    {
        PosAdulation();
        
    }
    void PosAdulation()
    {
        adulation = gameObject.transform.position + (target.transform.position + adjustPos - gameObject.transform.position) * adulationPer;
        gameObject.transform.position = adulation;
    }
}
