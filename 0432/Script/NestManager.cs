using AddClass;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class NestManager : MonoBehaviour
{
    [field: SerializeField, NonEditable] public bool leaved { get; set; }
    [SerializeField] private Chara_Player leavedPlayer;
    [SerializeField] private TargetColliders<Chara_Player> targets = new TargetColliders<Chara_Player>();
    [SerializeField] private List<Interval> intervals = new List<Interval>();
    [SerializeField] private float leavedScore;
    public Action clearAction { get; set; }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        leaved = false;
        leavedPlayer = null;
        leavedScore = 0;
        transform.tag = SurvivalGameTags.Nest;
        targets.firstTimeAction += TargetsFirstTimeAction;
    }

    public void Clear()
    {
        clearAction?.Invoke();
        for(int i = 0; i < intervals.Count; i++)
        {
            intervals[i].Reset();

        }
    }

    public void TargetsFirstTimeAction()
    {
        Interval newInterval = new Interval();
        newInterval.Initialize(false, true, 1.0f);
        intervals.Add(newInterval);
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
        if (player == leavedPlayer) { return false; }   // —a‚©‚Á‚½ƒvƒŒƒCƒ„[‚ªG‚ê‚Ä‚¢‚é‚È‚ç

        int i = targets.GetIndex(player);
        if (active == false)                            // “ü—Í‚ª‚È‚¢‚È‚ç
        {
            intervals[i].Reset();
            player.UI.ControlNestGuage(intervals[i].ratio);

            return false;
        }
        if (i == -1) {  return false; }                 // targets‚É“o˜^‚³‚ê‚Ä‚¢‚È‚¢‚È‚ç(—áŠOˆ—)


        intervals[i].Update();

        player.UI.ControlNestGuage(intervals[i].ratio);
        if(intervals[i].active == true)                 // ‘ÎÛ‚Ìinterval‚ªactive‚È‚ç
        {
            if (leaved == false)    // —a‚©‚Á‚Ä‚¢‚È‚¢ó‘Ô‚È‚ç
            {
                leavedScore = player.ChangeScoreByLeave();
                Debug.Log(leavedScore);
                leaved = true;
                leavedPlayer = player;
                Clear();

                Debug.Log("Leave");
                return true;
            }

            for (int j = 0; j < targets.Count; ++j)
            {
                if (leavedPlayer != player) // —a‚©‚Á‚Ä‚¢‚éó‘ÔŠ‚Â‘¼‚ÌƒvƒŒƒCƒ„[‚ªG‚Á‚Ä‚¢‚éó‘Ô‚È‚ç
                {
                    player.AddScore((int)StealLeavedScore());
                    Clear();
                    leaved = false;
                    leavedPlayer = null;
                    return true;
                }
            }

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
