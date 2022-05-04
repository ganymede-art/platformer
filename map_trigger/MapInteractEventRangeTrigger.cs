using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MapInteractEventRangeTrigger : MonoBehaviour, IInteractable
{
    static readonly Vector3 INTERACTABLE_PROMPT_OFFSET_DEFAULT = new Vector3(0.0F, 0.75F, 0.0F);

    [Header("Interation Attributes")]
    [FormerlySerializedAs("interact_range")]
    public float interactRange = 1.0F;
    public Vector3 interactablePromptOffset;
    public bool isInteractableWhenNotGrounded;

    [Header("Event Attributes")]
    public GameObject gameEventTriggerObject;


    void Start()
    {
        if (interactablePromptOffset == Vector3.zero)
            interactablePromptOffset = INTERACTABLE_PROMPT_OFFSET_DEFAULT;

        GameSceneController.Global.interactableObjects.Add(this);
    }

    private void OnDestroy()
    {
        GameSceneController.Global.interactableObjects.Remove(this);
    }

    void Update()
    {
    }

    public float GetInteractableRange()
    {
        return interactRange;
    }

    public GameObject GetInteractableObject()
    {
        return this.gameObject;
    }

    public Transform GetInteractableTransform()
    {
        return this.transform;
    }

    public void OnInteract()
    {
        if (gameEventTriggerObject == null)
        {
            Debug.LogError("Missing event trigger object.");
            return;
        }

        var triggerComponent = gameEventTriggerObject
            .GetComponent<GameEventTrigger>();

        if (triggerComponent == null)
        {
            Debug.LogError("Missing event trigger component.");
            return;
        }

        triggerComponent.StartGameEvent();

        if (triggerComponent.isOneShot)
            gameObject.SetActive(false);
    }

    public Vector3 GetInteractablePromptOffset()
    {
        return interactablePromptOffset;
    }

    public bool GetIsInteractableWhenNotGrounded()
    {
        return isInteractableWhenNotGrounded;
    }
}
