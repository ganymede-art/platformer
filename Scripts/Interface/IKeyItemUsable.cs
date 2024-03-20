using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKeyItemUsable
{
    bool IsKeyItemUsable { get; }
    float KeyItemUsableRange { get; }
    GameObject KeyItemUsableGameObject { get; }
    Transform KeyItemUsableTransform { get; }
    Vector3 KeyItemUsablePromptOffset { get; }
    void OnKeyItemUse(string keyItemId);
}