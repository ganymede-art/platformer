using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextIdReplacer : MonoBehaviour, IReplacer
{
    // Public properties.
    public string ReplacementValue => TextsHighLogic.G.GetText(textId);
    public object RawReplacementValue => TextsHighLogic.G.GetText(textId);

    // Public fields.
    public string textId;
}
