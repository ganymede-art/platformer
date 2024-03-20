using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonReplacer : MonoBehaviour, IReplacer
{
    public string ReplacementValue => GetButtonString();
    public object RawReplacementValue => ReplacementValue;

    // Public fields.
    public ButtonTypeConstant buttonType;

    private string GetButtonString()
    {
        string replacementValue = buttonType.ButtonType switch
        {
            ButtonType.North => $"(Y)/[R]",
            ButtonType.East => $"(B)/[G]",
            ButtonType.South => "(A)/[E]",
            ButtonType.West => $"(X)/[F]",
            _ => string.Empty,
        };

        return replacementValue;
    }
}
