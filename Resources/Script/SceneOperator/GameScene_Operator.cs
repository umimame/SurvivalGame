using AddClass;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene_Operator : SceneOperator
{
    [SerializeField] private Instancer playerInstancer;
    [SerializeField] private List<Chara_Player> players = new List<Chara_Player>();
    [SerializeField] private int NumberOfPlayer;
    [SerializeField] private string nextScene;
    [SerializeField] private ResultScene_Operator nextSceneOperator;
    [SerializeField] private PlayerRespawnPos respownPos;

    [SerializeField] private Interval timeLimit;
    [SerializeField] private TextMeshProUGUI timeLimitText;
    [SerializeField] private int initialScore;
    [SerializeField] private List<float> scoreList = new List<float>();
    [field: SerializeField] public GravityProfile gravity { get; set; }
    protected override void Start()
    {
        SceneManager.sceneLoaded += GetResultSceneOperator;

        base.Start();
        PresetsByPlayerType preset = FrontCanvas.instance.presets;
        for(int i = 0; i < NumberOfPlayer; i++)
        {
            playerInstancer.Instance(); 
            playerInstancer.lastObj.tag = TagAndArray.ArrayToTag(i);
            
            players.Add(playerInstancer.lastObj.GetComponentInChildren<Chara_Player>());
            players[i].AddScore(initialScore);
            players[i].sceneOperator = this;
            players[i].playerRespawnPos = respownPos;


            Engine playerEngine = playerInstancer.lastObj.GetComponentInChildren<Engine>();
            playerEngine.SetGravity(gravity);

            Collider[] colliders = playerInstancer.lastObj.GetComponentsInChildren<Collider>();
            foreach(Collider collider in colliders)
            {
                collider.gameObject.tag = TagAndArray.ArrayToTag(i);
                collider.gameObject.layer = 6;      // LayerをPlayerに変更
            }
            playerInstancer.lastObj.transform.GetChild(0).position = preset.playerPos[i];

            scoreList.Add(players[i].sumScore);
        }

        timeLimit.Initialize(false, false);
        timeLimit.reachAction += TimeOver;
    }

    protected override void Update()
    {
        base.Update();


        for (int i = 0; i < players.Count; i++)
        {
            scoreList[i] = players[i].score.plan + players[i].score.entity;


        }
        scoreList = AddFunction.SortInDescending(scoreList);
        timeLimit.Update();
        TimeUpdate();
    }

    private void TimeOver()
    {
        GoToResultScene();
    }

    private void GoToResultScene()
    {
        SceneManager.LoadScene(nextScene);

    }

    private void GetResultSceneOperator(Scene scene, LoadSceneMode mode)
    {
        nextSceneOperator = GameObject.FindWithTag(Tags.SceneOperator).GetComponent<ResultScene_Operator>();
        Debug.Log(nextSceneOperator);
    }

    private void TimeUpdate()
    {
        float newTime = 0.0f;
        if(timeLimit.active == true)
        {
            newTime = 0.000f;
        }
        else 
        {
            newTime = Mathf.Abs(timeLimit.difference);
        }

        
        timeLimitText.SetText("Time:" + newTime.ToString("f3"));

    }

    /// <summary>
    /// トッププレイヤーとのスコア差を返す
    /// </summary>
    /// <param name="currentScore"></param>
    /// <returns></returns>
    public float DifferenceOfTopScore(float currentScore)
    {
        return scoreList[0] - currentScore;
    }

    public bool timeOver
    {
        get { return timeLimit.active; }
    }

}

public class TagAndArray
{
    public static int TagToArray(string tag)
    {
        switch(tag)
        {
            case "Player01":
                return 0;
            case "Player02":
                return 1;
        }

        return -1;
    }

    public static string ArrayToTag(int array)
    {
        switch (array)
        {
            case 0:
                return Tags.Player01;
            case 1:
                return Tags.Player02;
        }

        return "null";
    }
}