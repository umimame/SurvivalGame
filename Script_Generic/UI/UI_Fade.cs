using My;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 空のCanvasにつけてPrefab化して使用
/// </summary>
public class UI_Fade : SingletonDontDestroy<UI_Fade>
{
    [field: SerializeField] public FadeParameter inObj;
    [field: SerializeField] public FadeParameter outObj;
    private List<FadeParameter> faders = new List<FadeParameter>();
    [field: SerializeField] public string scene { get; set; }

    public void Start()
    {
        TypeFinder finder = gameObject.AddComponent<TypeFinder>();
        faders = finder.GetAndInList<FadeParameter>(GetType());
        Destroy(finder);

        inObj.Initialize(gameObject, 0.0f, outObj, scene);
        outObj.Initialize(scene, gameObject, 1.0f);
        
        if (inObj.obj.obj != null)  // inObjから優先的に実行
        {
            inObj.Instance();
            return;
        }

        outObj.Instance();  // inObjが無ければ実行

    }

    public void Update()
    {
        foreach(FadeParameter fade in faders)
        {
            fade.Update();
            if(fade.obj.state != Instancer.DisplayState.Death) { return; }
        }

        Destroy(gameObject);    // 全て出現したなら

    }
}


[Serializable] public class FadeParameter
{
    public enum FadeType
    {
        In,
        Connect,
        Out,
        End,
    }
    [field: SerializeField] public Instancer obj { get; set; }
    [field: SerializeField] public SpriteOrImage img { get; set; } = new SpriteOrImage();
    [field: SerializeField] public float speed { get; set; }
    [field: SerializeField] public FadeType type{ get; set; }
    [field: SerializeField] public GameObject parent { get; set; }
    private FadeParameter afterFader;
    private string afterScene;
    private string concurrentScene;
    private float initialAlpha;

    public void Initialize(GameObject parent, float alpha)
    {
        this.parent = parent;
        obj.Initialize();
        initialAlpha = alpha;

    }
    public void Initialize(string concurrentScene, GameObject parent, float alpha)
    {
        this.concurrentScene = concurrentScene;
        this.parent = parent;
        obj.Initialize();
        initialAlpha = alpha;

    }
    public void Initialize(GameObject parent, float alpha, FadeParameter after = null, string afterScene = null)
    {
        this.afterScene = afterScene;
        this.parent = parent;
        obj.Initialize();
        initialAlpha = alpha;

        if (after != null) { this.afterFader = after; }
    }

    public void Instance()
    {
        if (concurrentScene != null) {

            SceneManager.LoadScene(concurrentScene); }
        if(obj.obj == null) { return; }
        parent.transform.SetAsLastSibling();
        obj.Instance(parent);
        img.Initialize(obj.lastObj);
        img.Alpha = initialAlpha;
    }

    /// <summary>
    /// Instanceが実行されているなら処理される
    /// </summary>
    public void Update()
    {
        obj.Update();
        if (obj.state != Instancer.DisplayState.NotDisplayYet) { Fade(); }
    }

    public void Fade()
    {
        switch(type)
        {
            case FadeType.In:
                img.Alpha += speed;
                if(img.Alpha >= 1.0f) {
                    GameObject.Destroy(obj.lastObj);
                    Next();
                    type = FadeType.End;
                }
                break;
            case FadeType.Connect:
                break;
                case FadeType.Out:
                img.Alpha -= speed;
                if (img.Alpha <= 0.0f) {

                    GameObject.Destroy(obj.lastObj);
                    Next();
                    type = FadeType.End;
                }
                break;
        }
    }

    public void Next()
    {
        if(afterFader != null)
        {
            afterFader.Instance();
        }

        if (afterScene != null) 
        { 
            SceneManager.LoadScene(afterScene); 
        }
    }


}

[Serializable] public class FadeInstancer : Instancer
{
    [field: SerializeField] public UI_Fade fadeUI;
    [field: SerializeField] public string scene;

    public void Initialize()
    {
        fadeUI = obj.GetComponent<UI_Fade>();
        fadeUI.scene = scene;
    }

}
