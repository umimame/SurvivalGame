using AddClass;
using UnityEngine;

[CreateAssetMenu(fileName = "PosLimitInRange", menuName = "ScriptableObject/PosLimitInRange")]
public class PosLimitInRange : ScriptableObject
{
    [field: SerializeField] public PosRange range { get; set; }
}
