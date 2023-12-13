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

    public Action hitAction { get; set; }
    public Func<bool, Collider, bool> passJudgeFunc { get; set; }
    public void Initialize()
    {
        if (thisCollider == null) { thisCollider = GetComponent<Collider>(); }
        Spawn();
    }
    public void Spawn()
    {
        hitCount = 0;
        damage = 0.0f;
        hitCountEntitys.Clear();
        targets.Clear();
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


        bool firstTime = false;
        bool attacked = false;
        if (targets.Count == 0)
        {
            firstTime = true;
        }
        else
        {
            foreach (Chara c in targets)  // targetsをループして
            {
                if (c == you)
                {
                    firstTime = false;
                }
            }

        }

        if (firstTime == true)          // 同一個体でなければ
        {                               // targetsに追加する
            targets.Add(you.transform.root.GetChild(0).GetComponent<Chara>());
            hitCountEntitys.Add(0);
        }

        for (int i = 0; i < targets.Count; ++i)
        {
            if (hitCountEntitys[i] != hitCount)     // 対象が攻撃回数分攻撃されていないなら
            {
                attacked = targets[i].UnderAttack(damage, UnderAttackType.Normal, parent);    // 攻撃出来たら
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