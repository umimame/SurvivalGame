using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private int playerScore = 0;

    // スコアを加算するメソッド
    public void AddScore(int score)
    {
        playerScore += score;
        Debug.Log("Score: " + playerScore);
    }
}
