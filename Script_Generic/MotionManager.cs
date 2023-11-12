using My;
using System;
using UnityEngine;

public class MotionManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}


[Serializable]　public class Motion
{
    public Action cutIn { get; set; }
    [SerializeField, NonEditable] private float motionTime;
    [SerializeField] private float adjustMotionTime;
    [field: SerializeField] public Interval interval { get; set; }
    [field: SerializeField] public ThresholdRatio motionThreshold { get; set; }
    [field: SerializeField] public Exist exist { get; set; }
    [field: SerializeField] public EasingAnimator easAnim { get; private set; }
    public void Initialize(Animator animator, string clipName)
    {
        motionTime = AddFunction.GetAnimationClipLength(animator, clipName);
        motionTime += adjustMotionTime;


        exist.start += () => easAnim.Initialize(motionTime, animator);
        exist.start += easAnim.Reset;
        exist.start += () => interval.Initialize(false, true, motionTime);
        exist.start += motionThreshold.Reset;

        exist.enable += easAnim.Update;
        exist.enable += () => interval.Update();
        exist.enable += () => motionThreshold.Update(easAnim.nowRatio);

        exist.toEnd += Reset;

        interval.activeAction += exist.Finish;
        interval.activeAction += exist.Reset;
    }


    public void Reset()
    {
        easAnim.Reset();
        exist.Initialize();
        interval.Initialize(false, true, motionTime);
        motionThreshold.Reset();
        Debug.Log("Reset");

    }

    public void Update()
    {
        exist.Update();
    }

    public void Start()
    {
        exist.Start();
    }
    public void StartOneShot()
    {
        exist.StartOneShot();
    }

    public bool active
    {
        get
        {
            return !interval.active;
        }
    }
    public float nowMotionRatio
    {
        get { return easAnim.nowRatio; }
    }

    #region Existプロパティ
    public Action initializeAction
    {
        get { return exist.initialize; }
        set { exist.initialize = value; }
    }
    public Action startAction
    {
        get { return exist.start; }
        set { exist.start = value; }
    }

    public Action enableAction
    {
        get { return exist.enable; }
        set { exist.enable = value; }
    }
    public Action endAction
    {
        get { return interval.activeAction; }
        set { interval.activeAction = value; }
    }
    #endregion

    #region Thresholdプロパティ
    public Action withinThreshold
    {
        get { return motionThreshold.withinRangeAction.action; }
        set { motionThreshold.withinRangeAction.action = value; }
    }

    public Action inThreshold
    {
        get { return motionThreshold.inRangeAction; }
        set { motionThreshold.inRangeAction = value; }
    }

    public Action exitThreshold
    {
        get { return motionThreshold.exitRangeAction.action; }
        set { motionThreshold.exitRangeAction.action = value; }
    }

    public Action outThreshold
    {
        get { return motionThreshold.outOfRangeAction; }
        set { motionThreshold.outOfRangeAction = value; }
    }
    #endregion

}

[Serializable] public class MotionAndCollider : Motion
{

}