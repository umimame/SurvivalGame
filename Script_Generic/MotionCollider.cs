using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionCollider : MonoBehaviour
{
    [field: SerializeField, NonEditable] public bool enable { get; private set; }
    [SerializeField] private Collider thisCollider;
    [field: SerializeField, NonEditable] public int hitCount { get; private set; }
    [SerializeField, NonEditable] private float damage;
    [SerializeField] private List<int> hitCountEntitys = new List<int>();
    [SerializeField] private List<Chara_Player> targets = new List<Chara_Player>();

    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        thisCollider = GetComponent<Collider>();
        Reset();
    }

    public void Reset()
    {
        enable = false;
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
    public void Launch(float damage, int hitCout = 1)
    {
        this.damage = damage;
        this.hitCount = hitCout;
        enable = true;
    }

    private void OnTriggerStay(Collider you)
    {
        if(enable == false) { return; }

        bool firstTime = false;
        bool attacked = false;
        Debug.Log("ActiveFang");
        if(targets.Count == 0 ) 
        {
            firstTime = true;
        }
        else
        {
            foreach (Chara_Player c in targets)  // targetsをループして
            {
                if (c == you)
                {
                    firstTime = false;
                }
            }

        }

        if (firstTime == true)          // 同一個体でなければ
        {                               // targetsに追加する
            targets.Add(you.transform.root.GetChild(0).GetComponent<Chara_Player>());
            hitCountEntitys.Add(0);
        }

        for(int i = 0; i < targets.Count; ++i)
        {
            if (hitCountEntitys[i] != hitCount)
            {
                attacked = targets[i].UnderAttack(damage, UnderAttackType.Normal);    // 攻撃出来たら
                if(attacked == true) { 
                    hitCountEntitys[i]++;
                    Debug.Log("Hit!!");
                }      // ヒットさせる
            }
        }


    }
}

public enum UnderAttackType
{
    None,
    Normal,
}