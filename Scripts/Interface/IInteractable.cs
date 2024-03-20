using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool IsInteractable { get; }
    public float InteractableRange { get; }
    public GameObject InteractableGameObject { get; }
    public Transform InteractableTransform { get; }
    public Vector3 InteractablePromptOffset { get; }
    public void OnInteract();
}
