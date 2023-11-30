using UnityEngine;

public class GameScene_Operator : SceneOperator
{
    [SerializeField] private Instancer playerInstancer;
    [SerializeField] private int NumberOfPlayer;
    [field: SerializeField] public GravityManager gravity { get; set; }
    protected override void Start()
    {
        base.Start();
        PresetsByPlayerType preset = FrontCanvas.instance.presets;
        for(int i = 0; i < NumberOfPlayer; i++)
        {
            playerInstancer.Instance();
            playerInstancer.lastObj.tag = TagAndArray.ArrayToTag(i);
            playerInstancer.lastObj.transform.GetChild(0).position = preset.playerPos[i];

            Engine playerEngine = playerInstancer.lastObj.GetComponentInChildren<Engine>();
            playerEngine.SetGravity(gravity);

        }
    }

    protected override void Update()
    {
        base.Update();
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