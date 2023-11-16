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
    [SerializeField] private Vector2 screenPos;
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
                thisRect.anchoredPosition = outPos;
                break;
        }
        
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
        if (target == null) { return; }


        switch (adulationType)
        {
            case AdulationType.World:
                adulation = gameObject.transform.position + (target.transform.position + adjustPos - gameObject.transform.position) * adulationPer;
                gameObject.transform.position = adulation;
                break;

            case AdulationType.Screen:

                wToS = targetCamera.WorldToScreenPoint(target.transform.position);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, wToS, targetCamera, out Vector2 outPos);
                screenPos  =  outPos;
                adulation = thisRect.anchoredPosition + (screenPos + (new Vector2 (adjustPos.x, adjustPos.y)) - thisRect.anchoredPosition) * adulationPer;
                gameObject.transform.localPosition = outPos;
                //thisRect.anchoredPosition = adulation;
                break;
        }


        
    }
}
