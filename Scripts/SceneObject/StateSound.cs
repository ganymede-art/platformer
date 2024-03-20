using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateSound : MonoBehaviour
{
    // Public fields.
    public AudioSource audioSource;

    void Start()
    {
        StateHighLogic.G.HighLogicStateChanged += OnHighLogicStateChanged;
    }

    private void OnDestroy()
    {
        if (StateHighLogic.G != null)
            StateHighLogic.G.HighLogicStateChanged -= OnHighLogicStateChanged;
    }

    public void OnHighLogicStateChanged(object sender, EventArgs e)
    {
        if (StateHighLogic.G.ActiveState == HighLogicStateId.Play
            || StateHighLogic.G.ActiveState == HighLogicStateId.Film)
        {
            audioSource.UnPause();
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Pause();
        }
    }
}
