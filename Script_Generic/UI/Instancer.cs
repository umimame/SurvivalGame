using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 自動で削除されるObjectにのみ使用
/// </summary>
[Serializable] public class Instancer
{
    public enum DisplayState
    {
        NotDisplayYet,
        Displaying,
        Death,
    }
    [field: SerializeField] public GameObject obj { get; set; }
    [field: SerializeField, NonEditable] public List<GameObject> clones { get; set; } = new List<GameObject>();
    [field: SerializeField, NonEditable] public DisplayState state { get; private set;}
    [SerializeField] private AudioClip instanceSound;
    [SerializeField] private GameObject parent;


    /// <summary>
    /// 改良中につき使用しない事<br/>
    /// Initialize時に指定した場合、親子関係が作られる
    /// </summary>
    [field: SerializeField] public Instancer afterObj;

    /// <summary>
    /// 引数2に親オブジェクトを指定することが出来る。
    /// </summary>
    /// <param name="afterObj"></param>
    /// <param name="parent"></param>
    public virtual void Initialize(Instancer afterObj = null, GameObject parent = null)
    {
        if(parent != null) { this.parent = parent; }
        state = DisplayState.NotDisplayYet;
        if (afterObj != null) { this.afterObj = afterObj; }
    }


    /// <summary>
    /// Listの更新
    /// </summary>
    public virtual void Update()
    {

        for(int i = clones.Count - 1; i >= 0; i--)
        {
            if (clones[i] != null) { state = DisplayState.Displaying; }// 一つでも表示中なら
            else { clones.RemoveAt(i); }

        }

        switch (state)
        {
            case DisplayState.NotDisplayYet:
                break;

            case DisplayState.Displaying:
                if (clones.Count == 0) { state = DisplayState.Death; }
                break;

            case DisplayState.Death:
                break;
        }
    }
    public virtual void Instance()
    {
        if (instanceSound != null) { FrontCanvas.instance.source.PlayOneShot(instanceSound); }
        clones.Add(GameObject.Instantiate(obj));
    }
    public virtual void Instance(GameObject parent)
    {
        if (instanceSound != null) { FrontCanvas.instance.source.PlayOneShot(instanceSound); }
        clones.Add(GameObject.Instantiate(obj, parent.transform));
    }
    public virtual void Instance(Transform instancePos)
    {
        if (instanceSound != null) { FrontCanvas.instance.source.PlayOneShot(instanceSound); }
        GameObject clone =  GameObject.Instantiate(obj);
        clone.transform.position = instancePos.position;
        clones.Add(clone);
    }



    /// <summary>
    /// stateがNotDisplayYetの場合のみInstanceを行う
    /// </summary>
    /// <param name="parent"></param>
    public virtual void InstanceOnlyOnce(GameObject parent = null)
    {
        if (state == DisplayState.NotDisplayYet)
        {
            if (parent == null) { Instance(); }
            else { Instance(parent); }
        }
    }

    /// <summary>
    /// clonesの最後を渡す
    /// </summary>
    public GameObject lastObj
    {
        get { return clones[clones.Count - 1]; }
        private set { clones[clones.Count -1] = value; }
    }

    public bool displaying
    {
        get
        {
            switch (state)
            {
                case DisplayState.NotDisplayYet:
                    return false;

                case DisplayState.Displaying:
                    return true;

                case DisplayState.Death:
                    return false;
            }

            return false;
        }
    }

}
