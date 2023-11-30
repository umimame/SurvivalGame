using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemSpawnPos : MonoBehaviour
{
    [SerializeField] private GameObject[] createItem;
    [SerializeField] private Transform[] ranges;
    // 経過時間
    private float time;
    //リスポーンのタイミング
    [SerializeField]
    private float RespawnTime = 1.0f;

    // Update is called once per frame
    void Update()
    {
        
        // 前フレームからの時間を加算していく
        time = time + Time.deltaTime;

        // 約1秒置きにランダムに生成されるようにする。
        if (time > RespawnTime)
        {
            //アイテム出現のエリアを抽選
            int RandomArea = Random.Range(0, 10);
            //アイテムをランダムで選択
            int RandomItem = Random.Range(0, 3);

            //ItemRespawn1に出現
            if (RandomArea <= 6)
            {
                // range1Aとrange1Bのx座標の範囲内でランダムな数値を作成
                float x1 = Random.Range(ranges[0].position.x, ranges[1].position.x);
                // range1Aとrang1eBのy座標の範囲内でランダムな数値を作成
                float y1 = Random.Range(ranges[0].position.y, ranges[1].position.y);
                // range1Aとrange1Bのz座標の範囲内でランダムな数値を作成
                float z1 = Random.Range(ranges[0].position.z, ranges[1].position.z);
                // GameObjectを上記で決まったランダムな場所に生成
                Instantiate(createItem[RandomItem], new Vector3(x1, y1, z1), createItem[RandomItem].transform.rotation);
            }
            //ItemRespawn1に出現
            else if (RandomArea == 7)
            {
                // range2Aとrange2Bのx座標の範囲内でランダムな数値を作成
                float x2 = Random.Range(ranges[2].position.x, ranges[3].position.x);
                // range2Aとrange2Bのy座標の範囲内でランダムな数値を作成
                float y2 = Random.Range(ranges[2].position.y, ranges[3].position.y);
                // range2Aとrange2Bのz座標の範囲内でランダムな数値を作成
                float z2 = Random.Range(ranges[2].position.z, ranges[3].position.z);
                Instantiate(createItem[RandomItem], new Vector3(x2, y2, z2), createItem[RandomItem].transform.rotation);
            }
            //ItemRespawn3に出現
            else if (RandomArea == 8)
            {
                // range3Aとrange3Bのx座標の範囲内でランダムな数値を作成
                float x3 = Random.Range(ranges[4].position.x, ranges[5].position.x);
                // range3Aとrange3Bのy座標の範囲内でランダムな数値を作成
                float y3 = Random.Range(ranges[4].position.y, ranges[5].position.y);
                // range3Aとrange3Bのz座標の範囲内でランダムな数値を作成
                float z3 = Random.Range(ranges[4].position.z, ranges[5].position.z);
                Instantiate(createItem[RandomItem], new Vector3(x3, y3, z3), createItem[RandomItem].transform.rotation);
            }
            //ItemRespawn4に出現
            else
            {
                // range4Aとrange4Bのx座標の範囲内でランダムな数値を作成
                float x4 = Random.Range(ranges[6].position.x, ranges[7].position.x);
                // range4Aとrange4Bのy座標の範囲内でランダムな数値を作成
                float y4 = Random.Range(ranges[6].position.y, ranges[7].position.y);
                // range4Aとrange4Bのz座標の範囲内でランダムな数値を作成
                float z4 = Random.Range(ranges[6].position.z, ranges[7].position.z);
                Instantiate(createItem[0], new Vector3(x4, y4, z4), createItem[0].transform.rotation);
            }

            // 経過時間リセット
            time = 0f;
        }
    }
}
