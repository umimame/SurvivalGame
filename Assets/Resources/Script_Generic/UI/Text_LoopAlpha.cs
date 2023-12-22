using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Text_LoopAlpha : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI text { get; private set; }
    [SerializeField] private float speed;
    [SerializeField] private float initialAlpha;
    private bool folding = false;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.alpha = initialAlpha;
    }

    void Update()
    {
        if(folding == false)
        {
            text.alpha -= speed;
            if(text.alpha <= 0)
            {
                folding = true;
            }
        }
        else if(folding == true)
        {
            text.alpha += speed;
            if(text.alpha >= 1)
            {
                folding = false;
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Text_LoopAlpha))]
public class Text_LoopAlphaEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Text_LoopAlpha tg = (Text_LoopAlpha)target;


        EditorGUI.BeginDisabledGroup(true);

        Color currentColor = Color.black;
        if (tg.text != null) { 
            currentColor = tg.text.color;
            EditorGUILayout.ColorField("CurrentCorlor", currentColor);
        }
        EditorGUI.EndDisabledGroup();


        if (GUI.changed)
        {
            EditorUtility.SetDirty(tg);
        }
    }
}
#endif