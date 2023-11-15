using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    float speed = 10.0f;

    void Update()
    {
        // Wキー（前方移動）
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += speed * transform.forward * Time.deltaTime;
        }

        // Sキー（後方移動）
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= speed * transform.forward * Time.deltaTime;
        }

        // Dキー（右移動）
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += speed * transform.right * Time.deltaTime;
        }

        // Aキー（左移動）
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= speed * transform.right * Time.deltaTime;
        }
    }
}
