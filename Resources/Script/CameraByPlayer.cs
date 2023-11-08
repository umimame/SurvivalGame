using UnityEngine;

/// <summary>
/// CameraByTargetと同時にアタッチする
/// </summary>
public class CameraByPlayer : MonoBehaviour
{
    [SerializeField] CameraByTarget camByTg;
    [SerializeField] GameObject body;
    [SerializeField] Chara_Player player;
    private Camera cam;
    private void Start()
    {
        player = body.GetComponent<Chara_Player>();
        if (camByTg == null) { camByTg = gameObject.AddComponent<CameraByTarget>(); }
        if (camByTg.target == null) { camByTg.target = body; }
        cam = GetComponent<Camera>();
        if(body.tag == Tags.Player01)
        {
            gameObject.tag = "MainCamera";
            cam.rect = FrontCanvas.instance.presets.cameraRectPre[0];


        }
        else if(body.tag == Tags.Player02)
        {
            gameObject.tag = "Untagged";
            cam.rect = FrontCanvas.instance.presets.cameraRectPre[1];
        }
    }
}
