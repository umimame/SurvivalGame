using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultParamOperator : MonoBehaviour
{
    private static ResultSC resultSC;
    private static Winner superiority;    // óDê®Ç»ÉvÉåÉCÉÑÅ[
    private static int maxScore;
    private static int blows;
    private static int orange;
    private static int apple;
    private static int grape;

    [SerializeField] private string resultSceneName;
    private void Start()
    {
    }

    private void Update()
    {
        
    }


    public static void SetResultSC(ResultSC _resultSC)
    {
        resultSC = _resultSC;
        resultSC.SetResult(superiority, maxScore, blows, apple, orange, grape);
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
