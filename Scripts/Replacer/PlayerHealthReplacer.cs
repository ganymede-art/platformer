using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthReplacer : MonoBehaviour, IReplacer
{
    public string ReplacementValue => PlayerHighLogic.G.Health.ToString();
    public object RawReplacementValue => PlayerHighLogic.G.Health;
}
