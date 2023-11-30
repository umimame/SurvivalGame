using AddClass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ViewPointを主軸にしたFPS視点操作<br/>
/// 操作するオブジェクトにアタッチする
/// </summary>
public class FPSViewPoint : MonoBehaviour
{
    [field: SerializeField, NonEditable] public EntityAndPlan<Vector2> inputViewPoint { get; set; }
    [field: SerializeField, NonEditable] public Vector3 viewPointPosPlan { get; set; }


    [SerializeField] private Transform viewPointObject;
    [SerializeField] private MoveCircleSurface viewCircleHorizontal;
    [SerializeField] private MoveCircleSurface viewCircleVertical;
    [SerializeField] private ThresholdRatio verticalLimitter;

    [SerializeField] private CircleClamp norHorizontalCircle;
    [SerializeField] private Transform centerPos;

    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        viewCircleHorizontal.Initialize(viewPointObject);
        viewCircleVertical.Initialize(viewPointObject);
        verticalLimitter.Initialize();

        norHorizontalCircle.Initialize(gameObject, viewPointObject.gameObject);

    }
    
    private void Update()
    {
        DirrectionManager();
    }
    public void DirrectionManager()
    {
        // 向きを制御
        inputViewPoint.Assign();

        if (inputViewPoint.entity == Vector2.zero)
        {
            //viewPointPosPlan = viewPointObject.position;
        }
        else
        {
        }
        viewPointPosPlan = Vector3.zero;
        Vector3 newPlan = Vector3.zero;
        newPlan = Vector3.Scale(centerPos.forward, new Vector3(5, 5, 5));
        VerticalOffset(centerPos);

        //newPlan.x = centerPos.position.x;
        //newPlan.z = centerPos.position.z;

        //newPlan.x += viewCircleHorizontal.NewPosUpdate(inputViewPoint.plan.x).x;
        //newPlan.y += viewCircleHorizontal.NewPosUpdate(inputViewPoint.plan.x).y;
        

        viewCircleVertical.axis = centerPos.right; 
        //viewPointPosPlan += viewCircleVertical.NewPosUpdate(-inputViewPoint.plan.y);
        //viewCircleVertical.Update(-inputViewPoint.plan.y);

        // Y軸の視点制限
        verticalLimitter.Update(viewCircleVertical.angleFromCenter.z);
        //if (verticalLimitter.reaching == false) { viewPointPosPlan.plan -= viewCircleVertical.NewPosUpdate(-inputViewPoint.plan.y); }  // 範囲外ならなかったことにする


        viewPointPosPlan += newPlan;

        viewPointObject.position = viewPointPosPlan;
        //norHorizontalCircle.Limit();
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
    /// viewPointObjectの方を向かせる<br/>
    /// 引数は向かせるオブジェクト、向かせる方向
    /// </summary>
    /// <param name="objTransform"></param>
    public void LookAtViewPoint(Transform objTransform, bool x = true, bool y = true, bool z = true)
    {
        Vector3 newRotation = objTransform.position;

        if(x == true) { newRotation.x = viewPointObject.position.x; }
        if(y == true) { newRotation.y = viewPointObject.position.y; }
        if(z == true) { newRotation.z = viewPointObject.position.z; }

        objTransform.LookAt(newRotation);
    }
}
