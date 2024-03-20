using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItemUsePlayerBehaviour : MonoBehaviour, IBehaviour<Player, PlayerBehaviourId>
{
    // Consts.
    private const float REFRESH_INTERVAL = 0.1F;
    private const float REFRESH_DEFAULT_LOWEST_DISTANCE = 1000.0F;

    // Private fields.
    private float refreshTimer;
    private bool isKeyItemUsableInRange;
    private IKeyItemUsable keyItemUsableInRange;

    // Public properties.
    public PlayerBehaviourId BehaviourId => PlayerBehaviourId.KeyItemUse;
    public bool IsKeyItemUsableInRange => isKeyItemUsableInRange;
    public IKeyItemUsable KeyItemUsableInRange => keyItemUsableInRange;

    public void BeginBehaviour(Player c, Dictionary<string, object> args = null) { }
    public void FixedUpdateBehaviour(Player c) { }

    public void UpdateBehaviour(Player c)
    {
        // Handle refresh.
        refreshTimer += Time.deltaTime;
        if(refreshTimer >= REFRESH_INTERVAL)
        {
            refreshTimer = 0.0F;
            Refresh(c);
        }

        // Handle Prompt.
        c.keyItemUsePromptObject.SetActive(isKeyItemUsableInRange);

        if (!isKeyItemUsableInRange)
            return;

        c.keyItemUsePromptObject.transform.position = keyItemUsableInRange
            .KeyItemUsableTransform
            .TransformPoint(keyItemUsableInRange.KeyItemUsablePromptOffset);
    }

    public void EndBehaviours(Player c) { }

    public void KeyItemUse(Player c, string keyItemId)
    {
        if (!isKeyItemUsableInRange)
            return;

        if (keyItemUsableInRange == null)
            return;

        keyItemUsableInRange.OnKeyItemUse(keyItemId);
    }

    public void ClearKeyItemUsable()
    {
        isKeyItemUsableInRange = false;
        keyItemUsableInRange = null;
    }

    private void Refresh(Player c)
    {
        isKeyItemUsableInRange = false;
        keyItemUsableInRange = null;

        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play)
            return;

        float lowestDistance = REFRESH_DEFAULT_LOWEST_DISTANCE;

        foreach(var keyItemUsable in ActiveSceneHighLogic.G.KeyItemUsables.Values)
        {
            if (!keyItemUsable.IsKeyItemUsable)
                continue;

            if (keyItemUsable.KeyItemUsableTransform == null)
                continue;

            float distance = Vector3.Distance
                (c.transform.position
                , keyItemUsable.KeyItemUsableTransform.position);

            if (distance > keyItemUsable.KeyItemUsableRange)
                continue;

            if (distance > lowestDistance)
                continue;

            lowestDistance = distance;
            isKeyItemUsableInRange = true;
            keyItemUsableInRange = keyItemUsable;
        }
    }
}
