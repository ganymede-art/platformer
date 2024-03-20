using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAddActionTrigger : MonoBehaviour, IInteractable
{
    // Public fields.
    public AddActionHighLogicTrigger highLogicTrigger;
    public float interactableRange;
    public Vector3 interactablePromptOffset;

    // Public properties.
    public bool IsInteractable => gameObject.activeSelf;
    public float InteractableRange => interactableRange;
    public GameObject InteractableGameObject => gameObject;
    public Transform InteractableTransform => transform;
    public Vector3 InteractablePromptOffset => interactablePromptOffset;

    private void Start()
    {
        ActiveSceneHighLogic.G.Interactables[gameObject] = this;
    }

    private void OnDestroy()
    {
        if (ActiveSceneHighLogic.G == null)
            return;
        ActiveSceneHighLogic.G.Interactables.Remove(gameObject);
    }

    private void OnEnable()
    {
        ActiveSceneHighLogic.G.Interactables[gameObject] = this;
    }

    private void OnDisable()
    {
        if (ActiveSceneHighLogic.G == null)
            return;
        ActiveSceneHighLogic.G.Interactables.Remove(gameObject);
    }

    public void OnInteract()
    {
        if (highLogicTrigger == null)
        {
            Debug.LogWarning($"[{GetType()}] high logic trigger is missing.");
            return;
        }

        highLogicTrigger.AddAction();
    }
}
