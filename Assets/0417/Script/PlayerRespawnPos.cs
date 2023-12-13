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
        //Zキーでvector3を返す
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayerRespawn();
            //Debug用
            Debug.Log(PlayerRespawn());
        }
    }

    Vector3 PlayerRespawn()
    {
        //リスポーンさせる位置をランダムで設定
        int RandomRespawn = Random.Range(0, RespawnPos.Length);
        Vector3 newPos = RespawnPos[RandomRespawn].position;
        return newPos;
    }
}