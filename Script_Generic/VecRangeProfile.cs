using AddClass;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PosLimitInRange", menuName = "ScriptableObject/PosLimitInRange")]
public class VecRangeProfile : ScriptableObject
{

    [field: SerializeField] public MinMax x = new MinMax();
    [field: SerializeField] public MinMax y = new MinMax();
    [field: SerializeField] public MinMax z = new MinMax();
    private VecT<MinMax> range = new VecT<MinMax>();

    public VecT<MinMax> GetVecT()
    {
        range.x = x;
        range.y = y;
        range.z = z;
        return range;
    }
}

[Serializable] public class VecRangeOperator
{
    [field: SerializeField] public VecRangeProfile profile { get; private set; }
    [field: SerializeField] public Vec3Bool enableAxis { get; set; }
    [field: SerializeField] public VecT<ValueInRange> valueInRange { get; private set; } = new VecT<AddClass.ValueInRange>();

    public void AssignProfile(VecRangeProfile assignProfile = null)
    {
        if(assignProfile != null) { this.profile = assignProfile; }
        if(this.profile == null) { return; }
        for(int i = 0; i < valueInRange.List.Count; ++i)
        {
            valueInRange.IndexToEntity(i).range = this.profile.GetVecT().IndexToEntity(i);
        }
    }

    /// <summary>
    /// 中心点と動かすVector3を指定する
    /// </summary>
    /// <param name="centerPos"></param>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public Vector3 Update(Vector3 centerPos, Vector3 targetPos)
    {

        VecT<float> vecTCenter = new VecT<float>();
        VecT<float> vecTTarget = new VecT<float>();

        AddFunction.VecTFloatConvert(vecTCenter, centerPos);
        AddFunction.VecTFloatConvert(vecTTarget, targetPos);

        for (int i = 0; i < VecT<float>.count; ++i)
        {
            if (enableAxis.ConvertToVecT().List[i] == true)
            {
                valueInRange.List[i].Update(AddFunction.IndexToVec3(i, vecTTarget));
                vecTTarget.Assign(i, Mathf.Clamp(vecTTarget.IndexToEntity(i), valueInRange.List[i].min + vecTCenter.List[i], valueInRange.List[i].max + vecTCenter.List[i]));
            }
        }

        Vector3 returnPos = AddFunction.VecTFloatConvert(vecTTarget);

        return returnPos;
    }

    /// <summary>
    /// 制限するVector3を指定する
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public Vector3 Update(Vector3 targetPos)
    {

        VecT<float> vecTTarget = new VecT<float>();

        AddFunction.VecTFloatConvert(vecTTarget, targetPos);

        for (int i = 0; i < VecT<float>.count; ++i)
        {
            valueInRange.List[i].Update(AddFunction.IndexToVec3(i, vecTTarget));
            if (enableAxis.ConvertToVecT().List[i] == true)
            {
                vecTTarget.Assign(i, Mathf.Clamp(vecTTarget.IndexToEntity(i), valueInRange.List[i].min, valueInRange.List[i].max));
            }
        }

        Vector3 returnPos = AddFunction.VecTFloatConvert(vecTTarget);

        return returnPos;
    }
    public Vector3 Update(Transform centerTra, Transform targetTra)
    {
        VecT<float> vecTCenter = new VecT<float>();
        VecT<float> vecTTarget = new VecT<float>();

        AddFunction.VecTFloatConvert(vecTCenter, centerTra.position);
        AddFunction.VecTFloatConvert(vecTTarget, targetTra.position);

        for (int i = 0; i < VecT<float>.count; ++i)
        {
            if (enableAxis.ConvertToVecT().List[i] == true)
            {

                valueInRange.List[i].Update(AddFunction.IndexToVec3(i, vecTTarget));
                vecTTarget.Assign(i, Mathf.Clamp(vecTTarget.IndexToEntity(i), valueInRange.List[i].min + vecTCenter.List[i], valueInRange.List[i].max + vecTCenter.List[i]));
            }
        }

        Vector3 returnPos = AddFunction.VecTFloatConvert(vecTTarget);

        return returnPos;
    }

}