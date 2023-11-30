using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemSpawnPos : MonoBehaviour
{
    [SerializeField]
    private GameObject createPrefab;
    [SerializeField]
    private Transform range1A;
    [SerializeField]
    private Transform range1B;
    [SerializeField]
    private Transform range2A;
    [SerializeField]
    private Transform range2B;
    [SerializeField]
    private Transform range3A;
    [SerializeField]
    private Transform range3B;
    [SerializeField]
    private Transform range4A;
    [SerializeField]
    private Transform range4B;
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

            //ItemRespawn1に出現
            if (RandomArea <= 6)
            {
                // range1Aとrange1Bのx座標の範囲内でランダムな数値を作成
                float x1 = Random.Range(range1A.position.x, range1B.position.x);
                // range1Aとrang1eBのy座標の範囲内でランダムな数値を作成
                float y1 = Random.Range(range1A.position.y, range1B.position.y);
                // range1Aとrange1Bのz座標の範囲内でランダムな数値を作成
                float z1 = Random.Range(range1A.position.z, range1B.position.z);
                // GameObjectを上記で決まったランダムな場所に生成
                Instantiate(createPrefab, new Vector3(x1, y1, z1), createPrefab.transform.rotation);
            }
            //ItemRespawn1に出現
            else if (RandomArea == 7)
            {
                // range2Aとrange2Bのx座標の範囲内でランダムな数値を作成
                float x2 = Random.Range(range2A.position.x, range2B.position.x);
                // range2Aとrange2Bのy座標の範囲内でランダムな数値を作成
                float y2 = Random.Range(range2A.position.y, range2B.position.y);
                // range2Aとrange2Bのz座標の範囲内でランダムな数値を作成
                float z2 = Random.Range(range2A.position.z, range2B.position.z);
                Instantiate(createPrefab, new Vector3(x2, y2, z2), createPrefab.transform.rotation);
            }
            //ItemRespawn3に出現
            else if (RandomArea == 8)
            {
                // range3Aとrange3Bのx座標の範囲内でランダムな数値を作成
                float x3 = Random.Range(range3A.position.x, range3B.position.x);
                // range3Aとrange3Bのy座標の範囲内でランダムな数値を作成
                float y3 = Random.Range(range3A.position.y, range3B.position.y);
                // range3Aとrange3Bのz座標の範囲内でランダムな数値を作成
                float z3 = Random.Range(range3A.position.z, range3B.position.z);
                Instantiate(createPrefab, new Vector3(x3, y3, z3), createPrefab.transform.rotation);
            }
            //ItemRespawn4に出現
            else
            {
                // range4Aとrange4Bのx座標の範囲内でランダムな数値を作成
                float x4 = Random.Range(range4A.position.x, range4B.position.x);
                // range4Aとrange4Bのy座標の範囲内でランダムな数値を作成
                float y4 = Random.Range(range4A.position.y, range4B.position.y);
                // range4Aとrange4Bのz座標の範囲内でランダムな数値を作成
                float z4 = Random.Range(range4A.position.z, range4B.position.z);
                Instantiate(createPrefab, new Vector3(x4, y4, z4), createPrefab.transform.rotation);
            }

            // 経過時間リセット
            time = 0f;
        }
    }
}
