using AddClass;
using UnityEngine;

/// <summary>
/// CameraByTargetと同時にアタッチする
/// </summary>
public class CameraByPlayer : MonoBehaviour
{
    [SerializeField] AdulationTarget camByTg;
    [SerializeField] GameObject body;
    [SerializeField] Chara_Player player;
    [SerializeField] Transform viewPoint;
    [SerializeField] SmoothRotate smooth;
    [SerializeField] private Vector3 world;
    [SerializeField] private float angle;
    [field: SerializeField] public Camera cam { get; private set; }
    private void Start()
    {

        if (camByTg == null) { camByTg = gameObject.AddComponent<AdulationTarget>(); }
        if (body != null)
        {
            player = body.GetComponent<Chara_Player>();
            cam = GetComponent<Camera>();
            if (body.tag == Tags.Player01)
            {
                //gameObject.tag = "MainCamera";
                cam.rect = FrontCanvas.instance.presets.cameraRectPre[0];


            }
            else if (body.tag == Tags.Player02)
            {
                gameObject.tag = "Untagged";
                cam.rect = FrontCanvas.instance.presets.cameraRectPre[1];
            }
        }
        smooth.Initialize(gameObject);
    }
    private void Update()
    {

        //world = camByTg.target.transform.TransformPoint(camByTg.target.transform.position);
        //angle = AddFunction.GetAngleByVec3(camByTg.target.transform.TransformPoint(camByTg.target.transform.position), viewPoint.position);
        //gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, AddFunction.GetAngleByVec3(camByTg.target.transform.TransformPoint(camByTg.target.transform.position), viewPoint.position));
        //transform.LookAt(viewPoint);
    }
}
