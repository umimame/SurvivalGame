using UnityEngine;
using UnityEngine.UI;

public class FollowObject : MonoBehaviour
{
    public Transform targetObject; // 3D�I�u�W�F�N�g��Transform
    public Image uiImage; // �Ǐ]����UI��Image
    public Camera uiCamera; // UI�p�̃J����

    void Update()
    {
        // 3D�I�u�W�F�N�g�̈ʒu���X�N���[�����W�ɕϊ�
        Vector3 screenPos = uiCamera.WorldToScreenPoint(targetObject.position);

        // UI�̈ʒu���X�V
        uiImage.rectTransform.anchoredPosition = screenPos;
    }
}