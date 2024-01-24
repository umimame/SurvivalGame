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
    /// �a�����Ă���Score��D��
    /// </summary>
    /// <returns></returns>
    public float StealLeavedScore()
    {
        float returnScore = leavedScore;
        leavedScore = 0.0f;
        return returnScore;
    }

    /// <summary>
    /// �X�R�A��a����֐�<br/>
    /// �����͗a����X�R�A<br/>
    /// �߂�l�͗a����ꂽ���ǂ���
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public bool ControleNest(Chara_Player player, bool active)
    {
        int i = targets.GetIndex(player);
        if (player == leavedPlayer) { return false; }   // �a�������v���C���[���G��Ă���Ȃ�

        if (active == false)                            // ���͂��Ȃ��Ȃ�
        {
            leaveIntervals[i].Reset();
            stealIntervals[i].Reset();
            player.UI.ControlNestGuage(0, true);

            return false;
        }

        if (i == -1)    // targets�ɓo�^����Ă��Ȃ��Ȃ�(��O����)
        {  
            Debug.LogError("Nest�̗�O����");
            return false; 
        }                 



        if (leavedPlayer == null)    // �a�����Ă��Ȃ���ԂȂ�
        {
            leaveIntervals[i].Update();
            stealIntervals[i].Reset();
            player.UI.ControlNestGuage(leaveIntervals[i].ratio, true);
            return true;
        }
        else if (leavedPlayer != player) // �a�����Ă����Ԋ����̃v���C���[���G���Ă����ԂȂ�
        {
            leaveIntervals[i].Reset();
            stealIntervals[i].Update();
            player.UI.ControlNestGuage(stealIntervals[i].ratio, false);
            return true;
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

    private void StealActiveAction(Chara_Player player)
    {
        Debug.Log(leavedPlayer);
        leavedPlayer.leavedScore -= (int)leavedScore;
        player.AddScore((int)StealLeavedScore());

        Clear();
        leavedPlayer = null;
    }


    public void OnTriggerStay(Collider other)
    {
        if(other.CompareTag(Tags.Player01) || other.CompareTag(Tags.Player02))
        {
            targets.Update(other.transform.root.GetChild(0).GetComponent<Chara_Player>());  // �G��Ă���v���C���[��o�^����

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player01) || other.CompareTag(Tags.Player02))
        {
            Chara_Player player = other.transform.parent.GetComponent<Chara_Player>();
            if (player != null)
            {
                targets.Update(player);  // �G��Ă���v���C���[��o�^����
                int i = targets.GetIndex(player);
                leaveIntervals[i].activeAction += () => LeaveActiveAction(player);
                stealIntervals[i].activeAction += () => StealActiveAction(player);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Player01) || other.CompareTag(Tags.Player02))
        {
            Chara_Player player = other.transform.parent.GetComponent<Chara_Player>();
            if (player != null)
            {

                int i = targets.GetIndex(player);

                leaveIntervals[i].activeAction = null;
                stealIntervals[i].activeAction = null;
            }
        }

    }

}
