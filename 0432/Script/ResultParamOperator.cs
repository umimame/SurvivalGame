using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultParamOperator : MonoBehaviour
{
    [SerializeField] private ResultSC resultSC;
    [SerializeField] private int maxScore;
    [SerializeField] private int blows;
    [SerializeField] private int orange;
    [SerializeField] private int apple;
    [SerializeField] private int grape;
    private void Start()
    {
        maxScore = 0;
        blows = 0;
        orange = 0;
        grape = 0;
        apple = 0;
    }

    public void SetResultSC(ResultSC resultSC)
    {
        this.resultSC = resultSC;
        resultSC.SetResult(maxScore, blows, apple, orange, grape);
    }

    public void SetScore(int score)
    {
        maxScore = score;
    }

    public void AddBlows()
    {
        blows++;
    }
    public void AddApple()
    {
        apple++;
    }
    public void AddOrange()
    {
        orange++;
    }
    public void AddGrape()
    {
        grape++;
    }
}
