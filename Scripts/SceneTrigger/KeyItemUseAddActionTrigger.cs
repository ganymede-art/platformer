using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItemUseAddActionTrigger : MonoBehaviour, IKeyItemUsable
{
    public bool IsKeyItemUsable => gameObject.activeSelf;
    public float KeyItemUsableRange => usableRange;
    public GameObject KeyItemUsableGameObject => gameObject;
    public Transform KeyItemUsableTransform => transform;
    public Vector3 KeyItemUsablePromptOffset => usablePromptOffset;

    // Public fields.
    public float usableRange;
    public Vector3 usablePromptOffset;
    [Space]
    public Pair<KeyItemIdConstant,AddActionHighLogicTrigger>[] interactionPairs;
    public AddActionHighLogicTrigger defaultAddActionTrigger;

    public void OnKeyItemUse(string keyItemId)
    {
        foreach(var interactionPair in interactionPairs)
        {
            if (keyItemId != interactionPair.pairKey.KeyItemId)
                continue;

            interactionPair.pairValue.AddAction();
            return;
        }

        if (defaultAddActionTrigger != null)
            defaultAddActionTrigger.AddAction();
    }

    private void Start()
    {
        ActiveSceneHighLogic.G.KeyItemUsables[gameObject] = this;
    }

    private void OnDestroy()
    {
        if (ActiveSceneHighLogic.G == null)
            return;
        ActiveSceneHighLogic.G.KeyItemUsables.Remove(gameObject);
    }
}
