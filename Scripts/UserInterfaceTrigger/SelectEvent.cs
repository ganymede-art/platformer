using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectEvent : MonoBehaviour
    , ISelectHandler
    , IDeselectHandler
{
    // Public fields.
    public event EventHandler OnSelection;
    public event EventHandler OnDeselection;

    public void OnDeselect(BaseEventData eventData)
    {
        OnDeselection?.Invoke(this, null);
    }

    public void OnSelect(BaseEventData eventData)
    {
        OnSelection?.Invoke(this, null);
    }
}  
