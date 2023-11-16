using My;
using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Chara : MonoBehaviour
{
    [field: SerializeField] public Parameter hp;
    [field: SerializeField] public Parameter speed;
    protected float assignSpeed;
    [field: SerializeField] public Parameter pow;
    protected Engine engine;
    [field: SerializeField, NonEditable] public bool alive { get; protected set; }  //  ����
    [SerializeField] protected EntityAndPlan<Vector2> inputMoveVelocity;
    protected Action<UnderAttackType> underAttackAction;

    [SerializeField] protected Interval invincible;
    [field: SerializeField] public Chara_Player lastAttacker { get; private set; }
    protected virtual void Start()
    {
        Initialize();
        engine = GetComponent<Engine>();
        engine.velocityPlanAction += AddVelocityPlan;
        alive = true;

    }
    protected virtual void Update()
    {
        hp.Update();
        speed.Update();
        pow.Update();
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
        assign.x = inputMoveVelocity.plan.x * assignSpeed;
        assign.z = inputMoveVelocity.plan.y * assignSpeed;
        engine.velocityPlan += assign;
    }

    /// <summary>
    /// �����̓_���[�W�ʂƔ�e���[�V�������s�����ǂ���
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="damageMotion"></param>
    public bool UnderAttack(float damage, UnderAttackType type = UnderAttackType.None, Chara_Player attacker = null)
    {
        if (alive == false) { return false; }
        if (invincible.active == false) { return false; }

        hp.entity -= damage;

        underAttackAction?.Invoke(type);
        
        if(attacker != null) { lastAttacker = attacker; }

        return true;
    }

}

/// <summary>
/// ���l�̒��g�ƍő�l���܂�<br/>
/// �C���X�^���X���s�v
/// </summary>
[Serializable] public class Parameter
{
    public float entity;
    public float max;
    public float autoRecaverValue;
    public void Initialize()
    {
        entity = max;
    }

    public void Update()
    {
        entity += autoRecaverValue;
        if(entity > max) { entity = max; }
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Parameter))]
public class ParameterDrawer : MyPropertyDrawer
{
    string entity = nameof(entity);
    string max = nameof(max);
    protected override void Update(Rect pos, SerializedProperty property, GUIContent label)
    {

        pos = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);

        HorizontalRect entityLabel = new HorizontalRect(pos);
        HorizontalRect entityRect = new HorizontalRect(pos);
        HorizontalRect maxLabel = new HorizontalRect(pos);
        HorizontalRect maxRect = new HorizontalRect(pos);

        entityLabel.Set(pos.x, 40);
        entityRect.Set(AddFunction.Neighbor(entityLabel) + 5, 30);
        maxLabel.Set(AddFunction.Neighbor(entityRect) + 5, 30);
        maxRect.Set(AddFunction.Neighbor(maxLabel) + 5, 30);


        EditorGUI.LabelField(entityLabel.entity, "Entity");

        EditorGUI.BeginDisabledGroup(true);
        {
            EditorGUI.PropertyField(entityRect.entity, property.FindPropertyRelative(entity), GUIContent.none);

        }
        EditorGUI.LabelField(maxLabel.entity, "Max");
        EditorGUI.EndDisabledGroup();
        EditorGUI.PropertyField(maxRect.entity, property.FindPropertyRelative(max), GUIContent.none);
        
    }
}

#endif

public enum IngredientUnit { Spoon, Cup, Bowl, Piece }

// �J�X�^���� Serializable �N���X
[Serializable]
public class Ingredient
{
    public string name;
    public int amount = 1;
    public IngredientUnit unit;
}

public class Recipe : MonoBehaviour
{
    public Ingredient potionResult;
    public Ingredient[] potionIngredients;
}
#if UNITY_EDITOR
// IngredientDrawer
[CustomPropertyDrawer(typeof(Ingredient))]
public class IngredientDrawer : MyPropertyDrawer
{
    // �w�肳�ꂽ��`���̃v���p�e�B��`��
    protected override void Update(Rect position, SerializedProperty property, GUIContent label)
    {
        // ���x����`��
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // ��`���v�Z
        var amountRect = new Rect(position.x, position.y, 30, position.height);
        var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
        var nameRect = new Rect(position.x + 90, position.y, RightEnd(position), position.height);

        // �t�B�[���h��`�� - GUIContent.none �����ꂼ��ɓn���ƁA���x���Ȃ��ɕ`�悳��܂�
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"), GUIContent.none);
        EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("unit"), GUIContent.none);
        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);

    }
}
#endif