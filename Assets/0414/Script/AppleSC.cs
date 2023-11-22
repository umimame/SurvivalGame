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

        pos.y -= speed;    // yÀ•W‚ğspeed•ªˆÚ“®
        myTransform.position = pos;  // À•W‚ğİ’è
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
