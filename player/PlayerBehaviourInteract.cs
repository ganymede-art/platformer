
using Assets.script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.script.GameConstants;

public class PlayerBehaviourInteract : MonoBehaviour, IPlayerBehaviour
{
    private bool isInteractableInRange = false;
    private IInteractable interactableInRange = null;

    public GameObject interactPromptObject;

    public string GetBehaviourType()
    {
        return GameConstants.PLAYER_BEHAVIOUR_INTERACT;
    }

    void Start()
    {
        StartCoroutine(UpdateStatus());
    }

    void Update()
    {
        if (GameMasterController.Global.gameState != GAME_STATE_GAME)
            return;

        // position interaction prompt.

        if (isInteractableInRange)
            interactPromptObject.transform.position
                = interactableInRange.GetInteractableTransform().position
                + interactableInRange.GetInteractablePromptOffset();

        // handle interaction on key press.

        if(GameInputController.Global.isInputNorth
            && !GameInputController.Global.wasInputNorth)
        {
            if (!isInteractableInRange)
                return;

            if (interactableInRange == null)
                return;

            if (!GameMasterController.GlobalPlayerController.isSpherecastGrounded
                && !interactableInRange.GetIsInteractableWhenNotGrounded())
                return;

            interactableInRange.OnInteract();
        }
    }

    IEnumerator UpdateStatus()
    {
        while (true)
        {
            isInteractableInRange = false;
            interactableInRange = null;

            if(GameMasterController.Global.gameState != GAME_STATE_GAME)
                yield return new WaitForSeconds(0.1F);

            foreach (var interactable in GameSceneController.Global.interactables)
            {
                float distance = Vector3.Distance
                    (this.transform.position, interactable.GetInteractableTransform().position);

                if (distance < interactable.GetInteractableRange())
                {
                    isInteractableInRange = true;
                    interactableInRange = interactable;
                    yield return new WaitForSeconds(0.1F);
                }
            }

            interactPromptObject.SetActive(isInteractableInRange);

            yield return new WaitForSeconds(0.1F);
        }
    }
}
