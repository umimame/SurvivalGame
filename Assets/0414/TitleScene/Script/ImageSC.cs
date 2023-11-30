using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSC : MonoBehaviour
{
    [SerializeField] RectTransform animal1;
    [SerializeField] RectTransform animal2;
    [SerializeField] RectTransform animal3;
    [SerializeField] RectTransform animal4;
    [SerializeField] RectTransform animal5;
    [SerializeField] RectTransform animal6;

    [SerializeField] private GameObject ApplePrefab;

    [SerializeField] private int counter;
    [SerializeField] private float move;
    [SerializeField] private float time;

    private RectTransform canvasRect;
    private float X;
    private float Y;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        move = 0.2f;
        SetRandomApplePosition(); // 初期ランダムな位置を設定

        // CanvasのRectTransformを取得
        canvasRect = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        // 動物を移動させるためのコード
        MoveAnimalImage();
        counter++;
        if (counter == 1000)
        {
            counter = 0;
            move *= -1;
            //Debug.Log("Move反転");
        }

        GenerateApple();
    }
    void MoveAnimalImage()
    {
        // 動物を移動させるためのコード
        animal1.position += new Vector3(move, 0, 0);
        animal2.position += new Vector3(move, 0, 0);
        animal3.position += new Vector3(0, move, 0);
        animal4.position += new Vector3(0, move, 0);
        animal5.position += new Vector3(-move, -move, 0);
        animal6.sizeDelta += new Vector2(move, move);
        animal6.position += new Vector3(move, -move, 0);
    }
    void GenerateApple()
    {
        if (time > 5f)
        {
            // Canvas内のランダムな位置にApplePrefabを生成
            GameObject apple = Instantiate(ApplePrefab, new Vector3(X, Y, 0), ApplePrefab.transform.rotation);
            //Canvasを親に設定
            apple.transform.SetParent(canvasRect, false);
            // 次のリンゴのために新しいランダムな位置を設定
            SetRandomApplePosition();

            time = 0f;
        }
    }

    void SetRandomApplePosition()
    {
        // Canvas境界内のランダムな位置を設定
        X = Random.Range(40f, 765f);
        Y = Random.Range(-70f, 70f);
    }
}
