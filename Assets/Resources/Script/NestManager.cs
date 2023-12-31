using AddClass;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class NestManager : MonoBehaviour
{
    [SerializeField] private Chara_Player leavedPlayer;
    [SerializeField] private TargetColliders<Chara_Player> targets = new TargetColliders<Chara_Player>();
    [SerializeField] private List<Interval> leaveIntervals = new List<Interval>();
    [SerializeField] private List<Interval> stealIntervals = new List<Interval>();
    [SerializeField] private float leavedScore;
    private bool enable;
    public Action clearAction { get; set; }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        leavedPlayer = null;
        leavedScore = 0;
        transform.tag = SurvivalGameTags.Nest;
        targets.firstTimeAction += TargetsFirstTimeAction;
    }

    public void Clear()
    {
        clearAction?.Invoke();
        for(int i = 0; i < leaveIntervals.Count; i++)
        {
            leaveIntervals[i].Reset();
            stealIntervals[i].Reset();
        }
    }

    public void TargetsFirstTimeAction()
    {
        Interval newLeaveInterval = new Interval();
        newLeaveInterval.Initialize(false, true, 1.0f);
        leaveIntervals.Add(newLeaveInterval);
        
        Interval newStealInterval = new Interval();
        newStealInterval.Initialize(false, true, 3.0f);
        stealIntervals.Add(newStealInterval);


    }

    /// <summary>
    /// 預かっているScoreを奪う
    /// </summary>
    /// <returns></returns>
    public float StealLeavedScore()
    {
        float returnScore = leavedScore;
        leavedScore = 0.0f;
        return returnScore;
    }

    /// <summary>
    /// スコアを預ける関数<br/>
    /// 引数は預けるスコア<br/>
    /// 戻り値は預けられたかどうか
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public bool ControleNest(Chara_Player player, bool active)
    {
        int i = targets.GetIndex(player);
        enable = active;
        if (player == leavedPlayer) { return false; }   // 預かったプレイヤーが触れているなら

        if (active == false)                            // 入力がないなら
        {
            leaveIntervals[i].Reset();
            stealIntervals[i].Reset();
            player.UI.ControlNestGuage(0, true);

            return false;
        }

        if (i == -1) {  return false; }                 // targetsに登録されていないなら(例外処理)



        if (leavedPlayer == null)    // 預かっていない状態なら
        {
            leaveIntervals[i].Update();
            stealIntervals[i].Reset();
            player.UI.ControlNestGuage(leaveIntervals[i].ratio, true);
        }
        else if (leavedPlayer != player) // 預かっている状態且つ他のプレイヤーが触っている状態なら
        {
            leaveIntervals[i].Reset();
            stealIntervals[i].Update();
            player.UI.ControlNestGuage(stealIntervals[i].ratio, false);
        }


        return false;
    }

    private void LeaveActiveAction(Chara_Player player)
    {
        leavedScore = player.ChangeScoreByLeave();
        player.leavedScore += (int)leavedScore;
        leavedPlayer = player;
        player.UI.NestTextLaunch(true);

        Clear();


    }

    private void SteakActiveAction(Chara_Player player)
    {
        leavedPlayer.leavedScore -= (int)leavedScore;
        player.AddScore((int)StealLeavedScore());

        Clear();
        leavedPlayer = null;
    }


    public void OnTriggerStay(Collider other)
    {
        if(other.CompareTag(Tags.Player01) || other.CompareTag(Tags.Player02))
        {
            targets.Update(other.transform.root.GetChild(0).GetComponent<Chara_Player>());  // 触れているプレイヤーを登録する
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player01) || other.CompareTag(Tags.Player02))
        {
            Chara_Player player = other.transform.root.GetChild(0).GetComponent<Chara_Player>();
            targets.Update(player);  // 触れているプレイヤーを登録する
            int i = targets.GetIndex(player);
            Debug.Log(i);
            leaveIntervals[i].activeAction += ()=> LeaveActiveAction(player);
            stealIntervals[i].activeAction += () => SteakActiveAction(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Player01) || other.CompareTag(Tags.Player02))
        {
            Chara_Player player = other.transform.root.GetChild(0).GetComponent<Chara_Player>();
            int i = targets.GetIndex(player);

            leaveIntervals[i].activeAction -= () => LeaveActiveAction(player);
            stealIntervals[i].activeAction += () => SteakActiveAction(player);
        }

    }

}
