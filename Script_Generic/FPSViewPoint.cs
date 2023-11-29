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
    [field: SerializeField, NonEditable] public EntityAndPlan<Vector3> viewPointPosPlan { get; set; }


    [SerializeField] private Transform viewPointObject;
    [SerializeField] private MoveCircleSurface viewCircleHorizontal;
    [SerializeField] private MoveCircleSurface viewCircleVertical;
    [SerializeField] private ThresholdRatio verticalLimitter;

    [SerializeField] private CircleClamp norHorizontalCircle;
    [SerializeField] private Transform norHorizontalCircleOffset;

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
        viewPointPosPlan.Reset(viewPointObject.transform.position);
        Vector3 newPos = Vector3.zero;
        newPos.x = inputViewPoint.plan.x;
        newPos.y = inputViewPoint.plan.y;

        viewPointPosPlan.plan += newPos;

        //viewPointObject.position = viewPointPosPlan.plan;
    }
    public void DirrectionManager()
    {
        // 向きを制御


        viewCircleHorizontal.Update(inputViewPoint.plan.x);

        viewCircleVertical.axis = transform.right;
        viewCircleVertical.Update(-viewPointPosPlan.plan.y);

        // Y軸の視点制限
        verticalLimitter.Update(viewCircleVertical.angleFromCenter.z);
        if (verticalLimitter.reaching == false) { viewCircleVertical.Update(inputViewPoint.plan.y); }  // 範囲外ならなかったことにする


        //norCircle.moveObject.transform.position = new Vector3(norCircle.moveObject.transform.position.x, gameObject.transform.transform.position.y, norCircle.moveObject.transform.position.z);
        //norCircleOffset.Update(norCircle.moveObject);

        norHorizontalCircle.Limit();
    }

    /// <summary>
    /// Y軸を追従する
    /// </summary>
    /// <param name="t1"></param>
    public void VerticalOffset(Transform t1)
    {
        Vector3 newViewPointPos = viewPointPosPlan.entity;
        newViewPointPos.y = t1.gameObject.transform.position.y;

        viewPointPosPlan.plan += newViewPointPos;
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
