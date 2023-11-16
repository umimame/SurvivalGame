using UnityEngine;

public class ObjectMarker : MonoBehaviour
{
    // オブジェクトを映すカメラ
    [SerializeField] private Camera targetCamera;
    
    // UIを表示させる対象オブジェクト
    [SerializeField] private Transform target;

    [SerializeField] private RectTransform parentUI;

    // オブジェクト位置のオフセット
    [SerializeField] private Vector3 worldOffset;


    // 初期化メソッド（Prefabから生成する時などに使う）
    public void Initialize(Transform target, Camera targetCamera = null)
    {
        this.target = target;
        this.targetCamera = targetCamera != null ? targetCamera : Camera.main;

        OnUpdatePosition();
    }

    private void Awake()
    {
        // カメラが指定されていなければメインカメラにする
        if (targetCamera == null)
            targetCamera = Camera.main;

    }

    // UIの位置を毎フレーム更新
    private void Update()
    {
        OnUpdatePosition();
    }

    // UIの位置を更新する
    private void OnUpdatePosition()
    {
        // カメラの向きベクトル
        var cameraDir = targetCamera.transform.forward;
        // オブジェクトの位置
        var targetWorldPos = target.position + worldOffset;
        // カメラからターゲットへのベクトル
        var targetDir = targetWorldPos - targetCamera.transform.position;

        // 内積を使ってカメラ前方かどうかを判定
        var isFront = Vector3.Dot(cameraDir, targetDir) > 0;

        // カメラ前方ならUI表示、後方なら非表示
        gameObject.SetActive(isFront);
        if (!isFront) return;

        // オブジェクトのワールド座標→スクリーン座標変換
        var targetScreenPos = targetCamera.WorldToScreenPoint(targetWorldPos);

        // スクリーン座標変換→UIローカル座標変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentUI,
            targetScreenPos,
            null,
            out var uiLocalPos
        );

        // RectTransformのローカル座標を更新
        gameObject.transform.localPosition = uiLocalPos;
    }
}