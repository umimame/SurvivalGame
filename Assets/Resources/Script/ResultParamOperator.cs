using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultParamOperator : MonoBehaviour
{
    [SerializeField] private Winner superiority;  // �D���ȃv���C���[
    [SerializeField] private int maxScore;        // �D���v���C���[�̃X�R�A
    [SerializeField] private int blows;           // �������j��
    [SerializeField] private int orange;          // �����l����
    [SerializeField] private int apple;
    [SerializeField] private int grape;

    private void Start()
    {
    }

    private void Update()
    {
        
    }

    public void SetSuperiority(Winner _superiority)
    {
        superiority = _superiority;
    }

    public void SetResultSC(ResultSC _resultSC)
    {
        _resultSC.SetResult(superiority, maxScore, blows, apple, orange, grape);
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
