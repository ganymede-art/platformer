using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateRigidBody : MonoBehaviour
{
    // Private fields.
    public Vector3 cachedVelocity;
    public Vector3 cachedAngularVelocity;

    // Public fields.
    public Rigidbody stateRigidBody;

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
        if(StateHighLogic.G.ActiveState == HighLogicStateId.Play
            && StateHighLogic.G.PreviousState != HighLogicStateId.Play)
        {
            stateRigidBody.isKinematic = false;

            stateRigidBody.velocity = cachedVelocity;
            stateRigidBody.angularVelocity = cachedAngularVelocity;
        }
        else if(StateHighLogic.G.ActiveState != HighLogicStateId.Play
            && StateHighLogic.G.PreviousState == HighLogicStateId.Play)
        {
            cachedVelocity = stateRigidBody.velocity;
            cachedAngularVelocity = stateRigidBody.angularVelocity;

            stateRigidBody.isKinematic = true;
        }
    }
}
