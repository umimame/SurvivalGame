using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultScene_Operator : SceneOperator
{
    [field: SerializeField] public ResultSC result { get; set; }

    public void SetResultParam(int score, int kill, int apple, int orange, int grape)
    {
        result.SetResult(Winner.Player1, score, kill, apple, orange, grape);
    }
}
