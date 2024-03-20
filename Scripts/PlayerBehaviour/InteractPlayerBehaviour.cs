using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPlayerBehaviour : MonoBehaviour, IBehaviour<Player, PlayerBehaviourId>
{
    // Consts.
    private const float REFRESH_INTERVAL = 0.1F;
    private const float REFRESH_DEFAULT_LOWEST_DISTANCE = 1000.0F;

    // Private fields.
    private float refreshTimer;
    private bool isInteractableInRange;
    private IInteractable interactableInRange;

    // Public properties.
    public PlayerBehaviourId BehaviourId => PlayerBehaviourId.Interact;

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
        c.interactPromptObject.SetActive(isInteractableInRange);

        if (!isInteractableInRange)
            return;

        c.interactPromptObject.transform.position = interactableInRange
            .InteractableTransform
            .TransformPoint(interactableInRange.InteractablePromptOffset);
    }

    public void EndBehaviours(Player c) { }

    public void Interact(Player c)
    {
        if (!isInteractableInRange)
            return;

        if (interactableInRange == null)
            return;

        interactableInRange.OnInteract();
    }

    public void ClearInteractable()
    {
        isInteractableInRange = false;
        interactableInRange = null;
    }

    private void Refresh(Player c)
    {
        isInteractableInRange = false;
        interactableInRange = null;

        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play)
            return;

        float lowestDistance = REFRESH_DEFAULT_LOWEST_DISTANCE;

        foreach(var interactable in ActiveSceneHighLogic.G.Interactables.Values)
        {
            if (!interactable.IsInteractable)
                continue;

            if (interactable.InteractableTransform == null)
                continue;

            float distance = Vector3.Distance
                (c.transform.position
                , interactable.InteractableTransform.position);

            if (distance > interactable.InteractableRange)
                continue;

            if (distance > lowestDistance)
                continue;

            lowestDistance = distance;
            isInteractableInRange = true;
            interactableInRange = interactable;
        }
    }
}
