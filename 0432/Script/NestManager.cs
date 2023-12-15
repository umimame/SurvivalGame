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
    /// —a‚©‚Á‚Ä‚¢‚éScore‚ğ’D‚¤
    /// </summary>
    /// <returns></returns>
    public float StealLeavedScore()
    {
        float returnScore = leavedScore;
        leavedScore = 0.0f;
        Debug.Log("Steal");
        return returnScore;
    }

    /// <summary>
    /// ƒXƒRƒA‚ğ—a‚¯‚éŠÖ”<br/>
    /// ˆø”‚Í—a‚¯‚éƒXƒRƒA<br/>
    /// –ß‚è’l‚Í—a‚¯‚ç‚ê‚½‚©‚Ç‚¤‚©
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public bool ControleNest(Chara_Player player, bool active)
    {
        int i = targets.GetIndex(player);
        enable = active;
        if (player == leavedPlayer) { return false; }   // —a‚©‚Á‚½ƒvƒŒƒCƒ„[‚ªG‚ê‚Ä‚¢‚é‚È‚ç

        if (active == false)                            // “ü—Í‚ª‚È‚¢‚È‚ç
        {
            leaveIntervals[i].Reset();
            stealIntervals[i].Reset();
            player.UI.ControlNestGuage(0, true);

            return false;
        }

        if (i == -1) {  return false; }                 // targets‚É“o˜^‚³‚ê‚Ä‚¢‚È‚¢‚È‚ç(—áŠOˆ—)



        if (leavedPlayer == null)    // —a‚©‚Á‚Ä‚¢‚È‚¢ó‘Ô‚È‚ç
        {
            stealIntervals[i].Reset();
            leaveIntervals[i].Update();
            player.UI.ControlNestGuage(leaveIntervals[i].ratio, true);
            if (leaveIntervals[i].active == true)                 // ‘ÎÛ‚Ìinterval‚ªactive‚È‚ç
            {
                leavedScore = player.ChangeScoreByLeave();
                player.leavedScore += (int)leavedScore;
                leavedPlayer = player;
                player.UI.NestTextLaunch(true);

                Clear();

                Debug.Log("Leave");
                return true;
            }
        }
        else if (leavedPlayer != player) // —a‚©‚Á‚Ä‚¢‚éó‘ÔŠ‚Â‘¼‚ÌƒvƒŒƒCƒ„[‚ªG‚Á‚Ä‚¢‚éó‘Ô‚È‚ç
        {
            leaveIntervals[i].Reset();
            stealIntervals[i].Update();
            player.UI.ControlNestGuage(stealIntervals[i].ratio, false);
            if (stealIntervals[i].active == true)
            {
                Debug.Log(leavedPlayer.leavedScore);
                leavedPlayer.leavedScore -= (int)leavedScore;
                player.AddScore((int)StealLeavedScore());
                Debug.Log(leavedPlayer.leavedScore);

                Clear();
                leavedPlayer = null;
            }
            return true;
        }


        return false;
    }




    public void OnTriggerStay(Collider other)
    {
        if(other.CompareTag(Tags.Player01) || other.CompareTag(Tags.Player02))
        {
            targets.Update(other.transform.root.GetChild(0).GetComponent<Chara_Player>());  // G‚ê‚Ä‚¢‚éƒvƒŒƒCƒ„[‚ğ“o˜^‚·‚é
        }
    }

}
