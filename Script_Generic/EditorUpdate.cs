using System;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
/// <summary>
/// Editor上でも任意のメソッドを実行できる
/// </summary>
[ExecuteAlways]
public class EditorUpdate : MonoBehaviour
{
    static double waitTime = 0;
    bool updateOnSelect = true;     // 対象のゲームオブジェクトを選択中のみUpdateする
    Action editorUpdateAction;


    /// <summary>
    /// OnEnableに記述
    /// </summary>
    /// <param name="editorUpdateAction"></param>
    public void EnableAction(Action editorUpdateAction)
    {
        this.editorUpdateAction += editorUpdateAction;
        EditorApplication.update += ExeEditorUpdate;
    }

    /// <summary>
    /// OnDisableに記述
    /// </summary>
    public void DisableAction()
    {
        this.editorUpdateAction -= editorUpdateAction;
        EditorApplication.update -= ExeEditorUpdate;
    }
    private void ExeEditorUpdate()
    {
        //Debug.Log("Update");
        if (EditorApplication.isPlaying == true) { return; }    // Play中は重複する恐れがあるためUpdateしない

        if (updateOnSelect == true)
        {
            foreach (var go in Selection.gameObjects)
            {
                // 選択中のオブジェクトのみ更新
                if (go == this.gameObject)
                {
                    //１／６０秒に１回更新
                    if ((EditorApplication.timeSinceStartup - waitTime) >= 0.01666f)
                    {
                        EditorUpdateAction();
                        Debug.Log("Update");
                        SceneView.RepaintAll(); // シーンビュー更新
                        waitTime = EditorApplication.timeSinceStartup;
                        break;
                    }
                }
            }
        }
        else
        {
            //１／６０秒に１回更新
            if ((EditorApplication.timeSinceStartup - waitTime) >= 0.01666f)
            {
                EditorUpdateAction();
                Debug.Log("Update");
                SceneView.RepaintAll(); // シーンビュー更新
                waitTime = EditorApplication.timeSinceStartup;
            }

        }
    }

    public virtual void EditorUpdateAction()
    {
        editorUpdateAction?.Invoke();
    }
}
#endif