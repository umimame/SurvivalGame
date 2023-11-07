using System;
using System.Collections.Generic;
using UnityEngine;
using My;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "PresetByPlayerType",menuName = "ScriptableObject/PresetByPlayerType")]
[Serializable] public class PresetsByPlayerType : ScriptableObject
{
    public int playerType { get; set; }
    [field: SerializeField] public List<Color> colorPre { get; set; } = new List<Color>();
    [field: SerializeField] public List<Color> playerColorPre { get; set; } = new List<Color>();
    [field: SerializeField] public List<Color> playerColorSets { get; set; } = new List<Color>();
    [field: SerializeField] public List<Rect> cameraRectPre { get; set; } = new List<Rect>();
    [field: SerializeField] public List<Vector2> playerPos { get; set; } = new List<Vector2>();
    public void Initialize()
    {
        colorPre = new List<Color>(playerType);
        cameraRectPre = new List<Rect>(playerType);
        for(int i = 0; i < playerType; ++i)
        {
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PresetsByPlayerType))]
public class PresetsByPlayerTypeEditor : MyEditor <PresetsByPlayerType>
{
    
    private void OnEnable()
    {
        Initialize();
    }
    public override void OnInspectorGUI()
    {
        tg.playerType = 2;
        EditorGUILayout.LabelField("PlayerType = ", tg.playerType.ToString());
        DrawDefaultInspector();
        base.OnInspectorGUI();
    }
}
#endif