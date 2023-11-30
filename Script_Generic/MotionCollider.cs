using System.Collections.Generic;
using UnityEngine;
using AddClass;
using GenericChara;
public class MotionCollider : MonoBehaviour
{
    [field: SerializeField] public Chara parent { get; set; }
    [field: SerializeField, NonEditable] public bool enable { get; private set; }
    [SerializeField] private Collider thisCollider;
    [field: SerializeField, NonEditable] public int hitCount { get; private set; }
    [SerializeField, NonEditable] private float damage;
    [SerializeField] private List<int> hitCountEntitys = new List<int>();
    [SerializeField] private List<Chara> targets = new List<Chara>();

    [field: SerializeField] public MeshRenderer mesh;
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        if (thisCollider == null) { thisCollider = GetComponent<Collider>(); }
        mesh = GetComponent<MeshRenderer>();
        Reset();
    }
    private void Update()
    {
        if(FrontCanvas.instance.debugMode == true)
        {

            mesh.enabled = enable;
        }
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
    public void Launch(float damage, int hitCount = 1)
    {
        this.damage = damage;
        this.hitCount = hitCount;
        enable = true;
    }

    private void OnTriggerStay(Collider you)
    {
        if(enable == false) { return; }
        if(!(you.tag == Tags.Player01 || you.tag == Tags.Player02)) { return; }

        bool firstTime = false;
        bool attacked = false;
        if(targets.Count == 0 ) 
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

        for(int i = 0; i < targets.Count; ++i)
        {
            if (hitCountEntitys[i] != hitCount)
            {
                attacked = targets[i].UnderAttack(damage, UnderAttackType.Normal, parent);    // 攻撃出来たら
                if(attacked == true) { 
                    hitCountEntitys[i]++;
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