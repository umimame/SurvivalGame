using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    //プレイヤーObject
    [SerializeField] private GameObject[] PlayerObject;
    //PlayerRespawnPosにアクセス
    public PlayerRespawnPos playerRespawnPos;
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
            PlayerObject[0].transform.position = playerRespawnPos.PlayerRespawn();
            Debug.Log("Z");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerObject[1].transform.position = playerRespawnPos.PlayerRespawn();
            Debug.Log("X");
        }
    }
}
