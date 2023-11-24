using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class AppleSC : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private float speed;
    private RectTransform canvasRect;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.05f, 0.1f);
        
        // CanvasのRectTransformを取得
        canvasRect = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveApple();
        DestroySelf();
    }
    void MoveApple()
    {
        // transformを取得
        Transform myTransform = this.transform;

        // 座標を取得
        Vector3 pos = myTransform.position;
        myTransform.transform.SetParent(canvasRect, false);

        pos.y -= speed;    // y座標をspeed分移動
        myTransform.position = pos;  // 座標を設定
        Debug.Log("posY:" + pos.y);
        if (pos.y <= 100)
        {
            speed = 0f;
            time = time + Time.deltaTime;

        }
    }
    void DestroySelf()
    {
        if (time > 3f)
        {
            Destroy(this.gameObject);
        }
    }
}
