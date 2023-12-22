using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraByUI : MonoBehaviour
{
    [SerializeField] private Camera playerCam;
    [SerializeField] private Camera thisCam;
    private void Start()
    {
        thisCam = GetComponent<Camera>();
        thisCam.rect = playerCam.rect;
    }
}
