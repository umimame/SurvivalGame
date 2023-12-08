using AddClass;
using UnityEngine;

public class TPSViewPoint : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform camPos;
    [SerializeField] private Engine engine;
    [SerializeField] private float sensitivity;
    [field: SerializeField, NonEditable] public EntityAndPlan<Vector2> inputViewPoint { get; set; }
    [field: SerializeField, NonEditable] public Vector3 viewPointPosPlan { get; set; }
    [SerializeField, NonEditable] private Vector3 beforePlan;

    [SerializeField] private Transform viewPointObject;
    [SerializeField] private MoveCircleSurface viewCircleHorizontal;
    [SerializeField] private MoveCircleSurface viewCircleVertical;
    [SerializeField] private ThresholdRatio verticalLimitter;

    [SerializeField] private CircleClamp norHorizontalCircle;
    [SerializeField] private Transform centerPos;

    private void Start()
    {
        Initialize();
        Reset();
    }
    public void Initialize()
    {
        Debug.Log(transform.position);
        viewCircleHorizontal.SetDistance(centerPos.forward);
        viewCircleVertical.SetDistance(viewCircleHorizontal.moveObject.forward);
        //verticalLimitter.Initialize();


        Debug.Log(transform.position);
        //norHorizontalCircle.Initialize(centerPos.gameObject, viewPointObject.gameObject);

    }

    public void Reset()
    {
        cam.transform.eulerAngles = transform.eulerAngles;
    }

    public void Update()
    {
        cam.transform.LookAt(viewPointObject);
        //CameraContorller();
    }

    public void CameraContorller()
    {

        viewCircleHorizontal.Update();
        viewCircleVertical.axis = viewCircleHorizontal.moveObject.transform.right;
        viewCircleVertical.Update();
        //// CameraのRotationを変更
        //Vector3 newCamEuler = cam.transform.eulerAngles;
        //newCamEuler.y += inputViewPoint.plan.x * sensitivity;
        //newCamEuler.x += -inputViewPoint.plan.y * sensitivity;
        //cam.transform.eulerAngles = newCamEuler;

        //// Y軸の視点制限
        //if (cam.transform.eulerAngles.x != 0.0f)
        //{
        //    verticalLimitter.Update(cam.transform.eulerAngles.x);
        //    if (verticalLimitter.reaching == true)  // 視点の角度が範囲外なら
        //    {
        //        newCamEuler.x -= -inputViewPoint.plan.y * sensitivity;    // なかったことにする
        //    }
        //}
        //cam.transform.eulerAngles = newCamEuler;



        //// PlayerのRotationを変更(Y軸のみ)
        //Vector3 newPlayerEuler = transform.eulerAngles;
        //newPlayerEuler.y = newCamEuler.y;
        //transform.eulerAngles = newPlayerEuler;



    }

    /// <summary>
    /// 
    /// </summary>
    public void InputZeroAssign()
    {
        inputViewPoint.PlanDefault();
    }

    /// <summary>
    /// Y軸を追従する
    /// </summary>
    /// <param name="t1"></param>
    public void VerticalOffset(Transform t1)
    {
        Vector3 newViewPointPos = Vector3.zero;
        newViewPointPos.y = t1.gameObject.transform.position.y;

        viewPointPosPlan += newViewPointPos;
    }

    /// <summary>
    /// Cameraの方を向かせる<br/>
    /// 引数は向かせるオブジェクト、向かせる方向
    /// </summary>
    /// <param name="objTransform"></param>
    public void AssignCamAngle(Transform objTransform, bool x = true, bool y = true, bool z = true)
    {
        Vector3 newRotation = objTransform.eulerAngles;

        if (x == true) { newRotation.x = cam.transform.eulerAngles.x; }
        if (y == true) { newRotation.y = cam.transform.eulerAngles.y; }
        if (z == true) { newRotation.z = cam.transform.eulerAngles.z; }

        objTransform.eulerAngles = newRotation;
        Debug.Log(cam.transform.eulerAngles);
    }
}
