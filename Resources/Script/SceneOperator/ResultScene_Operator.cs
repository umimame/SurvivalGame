using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultScene_Operator : SceneOperator
{
    [SerializeField] private ResultSC result;

    public void SetResultParam(int score, int kill, int apple, int orange, int grape)
    {
        result.SetResult(score, kill, apple, orange, grape);
    }
}
