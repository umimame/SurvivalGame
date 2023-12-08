using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene_Operator : SceneOperator
{
    [SerializeField] private Instancer playerInstancer;
    [SerializeField] private List<Chara_Player> players = new List<Chara_Player>();
    [SerializeField] private int NumberOfPlayer;
    [SerializeField] private string nextScene;
    [SerializeField] private Transform[] respownPos;
    [field: SerializeField] public GravityProfile gravity { get; set; }
    protected override void Start()
    {
        base.Start();
        PresetsByPlayerType preset = FrontCanvas.instance.presets;
        for(int i = 0; i < NumberOfPlayer; i++)
        {
            playerInstancer.Instance(); 
            playerInstancer.lastObj.tag = TagAndArray.ArrayToTag(i);
            players.Add(playerInstancer.lastObj.GetComponentInChildren<Chara_Player>());

            Engine playerEngine = playerInstancer.lastObj.GetComponentInChildren<Engine>();
            playerEngine.SetGravity(gravity);

            Collider[] colliders = playerInstancer.lastObj.GetComponentsInChildren<Collider>();
            foreach(Collider collider in colliders)
            {
                collider.gameObject.tag = TagAndArray.ArrayToTag(i);
                collider.gameObject.layer = 6;      // LayerÇPlayerÇ…ïœçX
            }
            playerInstancer.lastObj.transform.GetChild(0).position = preset.playerPos[i];


        }
    }

    protected override void Update()
    {
        base.Update();

        for(int i = 0; i < players.Count; i++)
        {
            if (players[i].motionState == Chara_Player.MotionState.Death)
            {
                Debug.Log("éÄ");
            }
        }
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