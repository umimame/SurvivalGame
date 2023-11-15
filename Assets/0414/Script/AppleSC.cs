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
        // Canvas‚ÌRectTransform‚ğæ“¾
        canvasRect = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        time = time + Time.deltaTime;
        MoveApple();
        DestroySelf();
    }
    void MoveApple()
    {
        // transform‚ğæ“¾
        Transform myTransform = this.transform;

        // À•W‚ğæ“¾
        Vector3 pos = myTransform.position;
        myTransform.transform.SetParent(canvasRect, false);

        pos.y -= speed;    // yÀ•W‚Ö0.01‰ÁZ
        myTransform.position = pos;  // À•W‚ğİ’è
        Debug.Log("posY:" + pos.y);
        if (pos.y <= 0)
        {
            speed = 0f;
        }
    }
    void DestroySelf()
    {
        if (time > 500f)
        {
            Destroy(this.gameObject);
        }
    }
}
