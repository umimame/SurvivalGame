using My;
using Unity.VisualScripting;
using UnityEngine;

public class AdulationTarget : MonoBehaviour
{
    enum AdulationType
    {
        World,
        Screen,
    }
    [SerializeField] private AdulationType adulationType;
    [field: SerializeField] public GameObject target { get; set; }
    [SerializeField] private Camera targetCamera;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private Vector3 adjustPos;
    [SerializeField, NonEditable] private Vector3 adulation;
    [SerializeField] private float adulationPer;
    [SerializeField] private Vector3 wToS;
    [SerializeField] private RectTransform thisRect;
    private void Start()
    {
        if (target == null) { return; }

        thisRect = GetComponent<RectTransform>();
        switch (adulationType)
        {
            case AdulationType.World:

                gameObject.transform.position = target.transform.position + adjustPos;                
                break;

            case AdulationType.Screen:
                wToS = targetCamera.WorldToScreenPoint(target.transform.position);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, wToS, targetCamera, out Vector2 outPos);
                //gameObject.transform.position = new Vector3(outPos.x, outPos.y, 0.0f);
                Debug.Log(outPos);
                thisRect.anchoredPosition = Vector3.zero;
                break;
        }
        
    }

    private void Update()
    {
    }
    private void FixedUpdate()
    {
        //PosAdulation();

    }
    void PosAdulation()
    {
        if (target == null) { return; }


        switch (adulationType)
        {
            case AdulationType.World:
                adulation = gameObject.transform.position + (target.transform.position + adjustPos - gameObject.transform.position) * adulationPer;
                break;

            case AdulationType.Screen:

                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, wToS, targetCamera, out Vector2 outPos);
                Vector3 screenPos = Vector3.zero;
                screenPos = outPos;
                adulation = gameObject.transform.position + (screenPos + adjustPos - gameObject.transform.position) * adulationPer;
                adulation = outPos;
                Debug.Log(target.tag + screenPos);
                break;
        }

        gameObject.transform.position = adulation;

        
    }
}
