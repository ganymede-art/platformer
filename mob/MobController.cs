using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System;
using static Assets.Script.GameConstants;

public class MobController : MonoBehaviour
{
    private float cachedRigidBodyDrag;

    private PhysicMaterialCombine cachedColliderBounceCombine;
    private float cachedColliderBounciness;
    private PhysicMaterialCombine cachedColliderFrictionCombine;
    private float cachedColliderDynamicFriction;
    private float cachedColliderStaticFriction;

    // states vars.
    [NonSerialized] public string currentState;
    [NonSerialized] public string previousState;
    [NonSerialized] public Dictionary<string, IMobState> states;
    [NonSerialized] public float stateTimer;

    // behaviour vars.
    [NonSerialized] public Dictionary<string, IMobBehaviour> behaviours;

    // physical vars.
    [NonSerialized] public Rigidbody mobRigidBody;
    [NonSerialized] public Collider mobCollider;
    [NonSerialized] public Renderer mobRenderer;
    [NonSerialized] public Animator mobAnimator;

    [Header("State Attributes")]
    public string defaultState;
    public GameObject statesContainerObject;

    [Header("Behaviour Attributes")]
    public GameObject behavioursContainerObject;

    [Header("Physical Attributes")]
    public GameObject rigidBodyObject;
    public GameObject colliderObject;
    public GameObject rendererObject;
    public GameObject animatorObject;
    public Transform directionTransform;
    public GameObject[] hitboxObjects;

    void Start()
    {
        // initialise.

        states = new Dictionary<string, IMobState>();
        behaviours = new Dictionary<string, IMobBehaviour>();

        // get all the states.
        var statesToAdd = statesContainerObject.GetComponents<IMobState>();

        foreach (var stateToAdd in statesToAdd)
        {
            Debug.Log("[MobController] adding state " + stateToAdd.GetStateId());
            states.Add(stateToAdd.GetStateId(), stateToAdd);
        }

        // get all the behaviours.
        var behavioursToAdd = behavioursContainerObject.GetComponents<IMobBehaviour>();

        foreach (var behaviourToAdd in behavioursToAdd)
        {
            Debug.Log("[MobController] adding behaviour " + behaviourToAdd.GetBehaviourType());
            behaviours.Add(behaviourToAdd.GetBehaviourType(), behaviourToAdd);
        }

        //set the state.
        currentState = defaultState;
        previousState = defaultState;
        stateTimer = 0.0F;

        // setup physical attributes.
        mobRigidBody = rigidBodyObject.GetComponent<Rigidbody>();
        mobCollider = colliderObject.GetComponent<Collider>();
        mobRenderer = rendererObject.GetComponent<Renderer>();
        mobAnimator = animatorObject.GetComponent<Animator>();

        // cache the default physics material, from the collider.
        cachedColliderBounceCombine = mobCollider.material.bounceCombine;
        cachedColliderBounciness = mobCollider.material.bounciness;
        cachedColliderFrictionCombine = mobCollider.material.frictionCombine;
        cachedColliderDynamicFriction = mobCollider.material.dynamicFriction;
        cachedColliderStaticFriction = mobCollider.material.staticFriction;

        cachedRigidBodyDrag = mobRigidBody.drag;

        //begin the state.
        states[currentState].BeginState(this);
    }

    void Update()
    {
        if (GameMasterController.Global.gameState == GAME_STATE_GAME)
        {
            stateTimer += Time.deltaTime;
            states[currentState].UpdateState(this);
        }
    }

    private void FixedUpdate()
    {
        if (GameMasterController.Global.gameState == GAME_STATE_GAME)
            states[currentState].FixedUpdateState(this);
    }

    public void ChangeState(string newState, params object[] parameters)
    {
        Debug.Log("[MobController] moving to state " + newState);

        states[currentState].FinishState(this);

        previousState = currentState;
        currentState = newState;

        stateTimer = 0.0F;

        states[currentState].BeginState(this, parameters);
    }

    public void RestoreOriginalColliderMaterial()
    {
        mobCollider.material.bounceCombine = cachedColliderBounceCombine;
        mobCollider.material.bounciness = cachedColliderBounciness;
        mobCollider.material.frictionCombine = cachedColliderFrictionCombine;
        mobCollider.material.dynamicFriction = cachedColliderDynamicFriction;
        mobCollider.material.staticFriction = cachedColliderStaticFriction;
    }

    public void RestoreOriginalRigidBodyProperties()
    {
        mobRigidBody.drag = cachedRigidBodyDrag;
    }
}
