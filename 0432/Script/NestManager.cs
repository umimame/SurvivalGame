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
    /// �a�����Ă���Score��D��
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
    /// �X�R�A��a����֐�<br/>
    /// �����͗a����X�R�A<br/>
    /// �߂�l�͗a����ꂽ���ǂ���
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public bool ControleNest(Chara_Player player, bool active)
    {
        if (player == leavedPlayer) { return false; }   // �a�������v���C���[���G��Ă���Ȃ�

        int i = targets.GetIndex(player);
        if (active == false)                            // ���͂��Ȃ��Ȃ�
        {
            intervals[i].Reset();
            player.UI.ControlNestGuage(intervals[i].ratio);

            return false;
        }
        if (i == -1) {  return false; }                 // targets�ɓo�^����Ă��Ȃ��Ȃ�(��O����)


        intervals[i].Update();

        player.UI.ControlNestGuage(intervals[i].ratio);
        if(intervals[i].active == true)                 // �Ώۂ�interval��active�Ȃ�
        {
            if (leaved == false)    // �a�����Ă��Ȃ���ԂȂ�
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
                if (leavedPlayer != player) // �a�����Ă����Ԋ����̃v���C���[���G���Ă����ԂȂ�
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
            targets.Update(other.transform.root.GetChild(0).GetComponent<Chara_Player>());  // �G��Ă���v���C���[��o�^����
        }
    }

}
