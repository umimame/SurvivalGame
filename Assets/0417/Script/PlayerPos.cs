using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    //プレイヤーObject
    [SerializeField] private GameObject[] PlayerObject;
    //PlayerRespawnPosにアクセス
    public PlayerRespawnPos playerRespawnPos;
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Zキーでプレイヤーの位置にPlayerRespawnを代入
        if (Input.GetKeyDown(KeyCode.Z))
        {
            pos = playerRespawnPos.PlayerRespawn();
            PlayerObject[0].transform.position = pos;
            Debug.Log(pos);
        }
       
        if (Input.GetKeyDown(KeyCode.X))
        {
            pos = playerRespawnPos.PlayerRespawn();
            PlayerObject[1].transform.position = pos;
            Debug.Log(pos);
        }
    }
}
