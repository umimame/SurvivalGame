using AddClass;
using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GenericChara
{
    public class Chara : MonoBehaviour
    {
        public enum CharaState
        {
            Spawn,
            Alive,
            Death,
            None,
        }
        [field: SerializeField, NonEditable] public CharaState charaState { get; private set; }
        protected Action spawnAction;
        protected Action aliveAction;
        protected Action deathAction;
        [field: SerializeField] public Parameter hp;
        [field: SerializeField] public Parameter speed;
        protected float assignSpeed;
        [field: SerializeField] public Parameter pow;
        public Engine engine { get; set; }
        [field: SerializeField, NonEditable] public bool alive { get; protected set; }  //  生存
        [SerializeField] private Interval respawnInterval;
        [SerializeField] protected EntityAndPlan<Vector2> moveVelocity;
        protected Action<UnderAttackType> underAttackAction;
        [SerializeField] private Interval spawnInvincible;
        [SerializeField] protected Interval invincible;
        [field: SerializeField] public Chara lastAttacker { get; private set; }
        protected virtual void Start()
        {
            Initialize();
            engine = GetComponent<Engine>();
            engine.velocityPlanAction += AddVelocityPlan;
            alive = true;
            spawnAction += Spawn;

            deathAction += () => respawnInterval.Update();
            respawnInterval.reachAction += () => StateChange(CharaState.Spawn);
        }

        protected virtual void Spawn()
        {

            respawnInterval.Initialize(false);
            spawnInvincible.Reset();
        }

        protected virtual void Update()
        {
            hp.Update();
            speed.Update();
            pow.Update();
            spawnInvincible.Update();
            switch (charaState)
            {
                case CharaState.Spawn:
                    spawnAction?.Invoke();
                    StateChange(CharaState.Alive);

                    break;
                case CharaState.Alive:
                    aliveAction?.Invoke();
                    break;

                case CharaState.Death:
                    deathAction?.Invoke();
                    break;
            }
        }
        public void Initialize()
        {
            hp.Initialize();
            speed.Initialize();
            pow.Initialize();
        }

        protected virtual void Reset()
        {
            lastAttacker = null;
        }

        public void AddVelocityPlan()
        {
            Vector3 assign = Vector3.zero;
            assign.x = moveVelocity.plan.x * assignSpeed;
            assign.z = moveVelocity.plan.y * assignSpeed;
            engine.velocityPlan += assign;
        }

        /// <summary>
        /// 引数はダメージ量と被弾モーションを行うかどうか
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="damageMotion"></param>
        public bool UnderAttack(float damage, UnderAttackType type = UnderAttackType.None, Chara attacker = null)
        {
            if (alive == false) { return false; }
            else if (spawnInvincible.active == false) { return false; }
            else if (invincible.active == false) { return false; }

            hp.entity -= damage;

            underAttackAction?.Invoke(type);

            if (attacker != null) { lastAttacker = attacker; }

            return true;
        }

        public void StateChange(CharaState state)
        {
            charaState = state;
        }

    }


}