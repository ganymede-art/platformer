using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/HitboxData")]
public class HitboxData : ScriptableObject
{
    public DamageTypeConstant damageType;
    public int damageAmount;
    public float damageForceMult;
}
