using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnPos : MonoBehaviour
{
    //プレイヤーObject
    [SerializeField] private GameObject[] createPlayer;
    //リスポーン位置の設定
    [SerializeField] private Transform[] RespawnPos;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //Zキーでリスポーン
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayerRespawn(0);
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            PlayerRespawn(1);
        }
    }

    void PlayerRespawn(int player)
    {
        //リスポーンさせる位置をランダムで設定
        int RandomRespawn = Random.Range(0, RespawnPos.Length);
        float x1 = RespawnPos[RandomRespawn].position.x;
        float y1 = RespawnPos[RandomRespawn].position.y;
        float z1 = RespawnPos[RandomRespawn].position.z;
        //プレイヤーのインスタンスを生成     
         Instantiate(createPlayer[player], new Vector3(x1, y1, z1), createPlayer[player].transform.rotation);
    }
}