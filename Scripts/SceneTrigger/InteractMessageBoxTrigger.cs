using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMessageBoxTrigger : MonoBehaviour, IInteractable
{
    // Private fields.
    private GameObject addActionObject;
    private MessageBoxAction messageBoxAction;
    private GameObject messageBoxActionObject;
    private AddActionHighLogicTrigger addActionHighLogicTrigger;
    private int textIdIndex;

    // Public properties.
    public bool IsInteractable => gameObject.activeInHierarchy;
    public float InteractableRange => interactableRange;
    public GameObject InteractableGameObject => gameObject;
    public Transform InteractableTransform => transform;
    public Vector3 InteractablePromptOffset => interactablePromptOffset;

    // Public fields.
    [Header("Interaction Attributes")]
    public float interactableRange;
    public Vector3 interactablePromptOffset;
    [Header("Text Attributes")]
    public string[] textIds;
    public VoxData voxData;
    public Animator messageBoxAnimator;
    public AnimatorTriggerIdConstant[] messageBoxAnimatorTriggerIds;

    private void Start()
    {
        ActiveSceneHighLogic.G.Interactables[gameObject] = this;

        // Create the add action.
        addActionObject = new GameObject($"AddActionSave");
        addActionObject.transform.SetPositionAndRotation
            ( transform.position
            , transform.rotation);

        // Create the message box action.
        messageBoxActionObject = new GameObject($"MessageBox");
        messageBoxActionObject.transform.SetParent(addActionObject.transform, false);
        messageBoxAction = messageBoxActionObject.AddComponent<MessageBoxAction>();
        messageBoxAction.voxData = voxData;
        messageBoxAction.textId = string.Empty;

        // Configure the add action.
        var stateIdConstant = ScriptableObject.CreateInstance<HighLogicStateIdConstant>();
        stateIdConstant.name = HighLogicStateId.Film.ToString();

        addActionHighLogicTrigger = addActionObject.AddComponent<AddActionHighLogicTrigger>();
        addActionHighLogicTrigger.actionHighLogicStateId = stateIdConstant;
        addActionHighLogicTrigger.actionId = $"{Guid.NewGuid()}";
        addActionHighLogicTrigger.isSequenced = true;
        addActionHighLogicTrigger.isOneShot = false;
        addActionHighLogicTrigger.activeActionObject = messageBoxActionObject;
    }

    private void OnDestroy()
    {
        if (ActiveSceneHighLogic.G != null)
            ActiveSceneHighLogic.G.Interactables.Remove(gameObject);
    }

    public void OnInteract()
    {
        if(messageBoxAnimator != null 
            && messageBoxAnimatorTriggerIds != null 
            && messageBoxAnimatorTriggerIds.Length > 0)
        {
            int triggerIdIndex = UnityEngine.Random.Range(0, messageBoxAnimatorTriggerIds.Length);
            messageBoxAnimator.ResetAllAnimatorTriggers();
            messageBoxAnimator.SetTrigger(messageBoxAnimatorTriggerIds[triggerIdIndex].AnimatorTriggerId);
        }
        messageBoxAction.textId = textIds[textIdIndex];
        addActionHighLogicTrigger.AddAction();

        textIdIndex++;
        if (textIdIndex > textIds.Length - 1)
            textIdIndex = 0;
    }
}
