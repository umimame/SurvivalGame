using System;
using UnityEngine;


[CreateAssetMenu(fileName = "GravityManager", menuName = "ScriptableObject/GravityManager")]
[Serializable] public class GravityManager : ScriptableObject
{
    [field: SerializeField] public Vector3 gravityScale { get; private set; }
    [field: SerializeField] public AnimationCurve easing { get; private set; }
}

