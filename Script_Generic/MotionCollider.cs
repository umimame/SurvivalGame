using System.Collections.Generic;
using UnityEngine;
using AddClass;
using GenericChara;
using System;

public class MotionCollider : MonoBehaviour
{
    [field: SerializeField] public Chara parent { get; set; }
    [field: SerializeField, NonEditable] public bool enable { get; private set; } = false;
    [SerializeField] private Collider thisCollider;
    [field: SerializeField, NonEditable] public int hitCount { get; private set; }
    [SerializeField, NonEditable] private float damage;
    [SerializeField] private List<int> hitCountEntitys = new List<int>();
    [SerializeField] private List<Chara> targets = new List<Chara>();
    [SerializeField] private TargetColliders<Chara> targetColliders = new TargetColliders<Chara>();
    public Action hitAction { get; set; }
    public Func<bool, Collider, bool> passJudgeFunc { get; set; }
    public void Initialize()
    {
        if (thisCollider == null) { thisCollider = GetComponent<Collider>(); }
        Spawn();
        targetColliders.firstTimeAction = null;
        targetColliders.firstTimeAction += () => hitCountEntitys.Add(0);
        targetColliders.firstTimeAction += () => Debug.Log(hitCountEntitys[0]);
    }
    public void Spawn()
    {
        hitCount = 0;
        damage = 0.0f;
        hitCountEntitys.Clear();
        targetColliders.Clear();
        enable = false;
    }

    /// <summary>
    /// 引数:<br/>
    /// ・ダメージ<br/>
    /// ・ヒット回数
    /// </summary>
    public void Launch(float damage, int hitCount = 1)
    {
        this.damage = damage;
        this.hitCount = hitCount;
        enable = true;
    }

    private void OnTriggerStay(Collider you)
    {
        OnTriggerAction(you);

    }

    public void OnTriggerAction(Collider you)
    {
        bool passing = true;
        if (enable == false) { passing = false; }
        if (passJudgeFunc != null) { passing = passJudgeFunc.Invoke(passing, you); }
        if (passing == false) { return; }

        targetColliders.Update(you.transform.root.GetChild(0).GetComponent<Chara>());

        bool attacked = false;

        for (int i = 0; i < targetColliders.targets.Count; ++i)
        {
            if (hitCountEntitys[i] != hitCount)     // 対象が攻撃回数分攻撃されていないなら
            {
                attacked = targetColliders.targets[i].UnderAttack(damage, UnderAttackType.Normal, parent);    // 攻撃出来たら
                if (attacked == true)
                {
                    if (hitCount >= 1)
                    {
                        Debug.Log("Hit");
                        hitCountEntitys[i]++;   // ヒットさせる
                        hitAction?.Invoke();
                    }
                }
            }
        }

    }
}

public enum UnderAttackType
{
    None,
    Normal,
}