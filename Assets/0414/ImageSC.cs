using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class ImageSC:MonoBehaviour
{
    [SerializeField] RectTransform animal1;
    [SerializeField] RectTransform animal2;
    [SerializeField] RectTransform animal3;
    [SerializeField] RectTransform animal4;
    [SerializeField] RectTransform animal5;
    [SerializeField] private int counter;
    [SerializeField] private float move;
    // Start is called before the first frame update
     void Start()
    {
        counter = 0;
        move = 0.1f;
    }
   
    // Update is called once per frame
     void Update()
    {
        animal1.position += new Vector3(move, 0, 0);
        animal2.position += new Vector3(move, 0, 0);
        animal3.position += new Vector3(0, move, 0);
        animal4.position += new Vector3(0, move, 0);
        animal5.position += new Vector3(-move, -move, 0);
        counter++;
        if (counter == 1000)
        {
            counter = 0;
            move *= -1;
        }
    }
}
