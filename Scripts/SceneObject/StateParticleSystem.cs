using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateParticleSystem : MonoBehaviour
{
    // Public fields.
    public ParticleSystem fx;

    void Start()
    {
        StateHighLogic.G.HighLogicStateChanged += OnHighLogicStateChanged;
    }

    private void OnDestroy()
    {
        if (StateHighLogic.G != null)
            StateHighLogic.G.HighLogicStateChanged -= OnHighLogicStateChanged;
    }

    private void OnEnable()
    {
        StateHighLogic.G.HighLogicStateChanged += OnHighLogicStateChanged;
    }

    private void OnDisable()
    {
        if (StateHighLogic.G != null)
            StateHighLogic.G.HighLogicStateChanged -= OnHighLogicStateChanged;
    }

    public void OnHighLogicStateChanged(object sender, EventArgs e)
    {
        if(StateHighLogic.G.ActiveState == HighLogicStateId.Play
            || StateHighLogic.G.ActiveState == HighLogicStateId.Film)
        {
            if (fx.isPaused)
                fx.Play();
        }
        else
        {
            if (fx.isPlaying)
                fx.Pause();
        }
    }
}
