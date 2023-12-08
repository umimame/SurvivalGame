using AddClass;
using UnityEngine;

[CreateAssetMenu(fileName = "PosLimitInRange", menuName = "ScriptableObject/PosLimitInRange")]
public class PosLimitInRange : ScriptableObject
{
    [field: SerializeField] public VecRangeClamp range { get; set; }
}
