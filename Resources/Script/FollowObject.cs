using UnityEngine;
using UnityEngine.UI;

public class FollowObject : MonoBehaviour
{
    public Transform targetObject; // 3DオブジェクトのTransform
    public Image uiImage; // 追従するUIのImage
    public Camera uiCamera; // UI用のカメラ

    void Update()
    {
        // 3Dオブジェクトの位置をスクリーン座標に変換
        Vector3 screenPos = uiCamera.WorldToScreenPoint(targetObject.position);

        // UIの位置を更新
        uiImage.rectTransform.anchoredPosition = screenPos;
    }
}