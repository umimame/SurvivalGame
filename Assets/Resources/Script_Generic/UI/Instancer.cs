using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �����ō폜�����Object�ɂ̂ݎg�p
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
    /// ���ǒ��ɂ��g�p���Ȃ���<br/>
    /// Initialize���Ɏw�肵���ꍇ�A�e�q�֌W�������
    /// </summary>
    [field: SerializeField] public Instancer afterObj;

    /// <summary>
    /// ����2�ɐe�I�u�W�F�N�g���w�肷�邱�Ƃ��o����B
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
    /// List�̍X�V
    /// </summary>
    public virtual void Update()
    {

        for(int i = clones.Count - 1; i >= 0; i--)
        {
            if (clones[i] != null) { state = DisplayState.Displaying; } // ��ł��\�����Ȃ�
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
    /// state��NotDisplayYet�̏ꍇ�̂�Instance���s��
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
    /// clones�̍Ō��n��
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