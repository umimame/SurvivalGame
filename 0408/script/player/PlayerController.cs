using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private int playerScore = 0;

    // �X�R�A�����Z���郁�\�b�h
    public void AddScore(int score)
    {
        playerScore += score;
        Debug.Log("Score: " + playerScore);
    }
}
