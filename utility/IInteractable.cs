using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script
{
    public interface IInteractable
    {
        float GetInteractableRange();
        GameObject GetInteractableObject();
        Transform GetInteractableTransform();
        Vector3 GetInteractablePromptOffset();
        bool GetIsInteractableWhenNotGrounded();
        void OnInteract();
    }
}
