using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultParamOperator : MonoBehaviour
{
    [SerializeField] private ResultSC resultSC;
    [SerializeField] private Winner superiority;    // óDê®Ç»ÉvÉåÉCÉÑÅ[
    [SerializeField] private int maxScore;
    [SerializeField] private int blows;
    [SerializeField] private int orange;
    [SerializeField] private int apple;
    [SerializeField] private int grape;

    [SerializeField] private string resultSceneName;
    private void Start()
    {
        SceneManager.sceneLoaded += GetResultSC;
        SceneManager.LoadScene(resultSceneName);
    }

    private void Update()
    {
        
    }

    private void GetResultSC(Scene scene, LoadSceneMode mode)
    {
        if(scene.name ==  resultSceneName)
        {
            resultSC = GameObject.FindWithTag("ResultSC").GetComponent<ResultSC>();
            SetResultSC(resultSC);

        }
    }

    public void SetResultSC(ResultSC resultSC)
    {
        this.resultSC = resultSC;
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
