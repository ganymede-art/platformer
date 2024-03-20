using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
    , IStateMachine<Mob,MobStateId>
    , IBehaviourMachine<Mob,MobBehaviourId>
{
    // Private fields.
    private float stateTimer;
    private Dictionary<MobStateId, IState<Mob, MobStateId>> states;
    private Dictionary<MobBehaviourId, IBehaviour<Mob, MobBehaviourId>> behaviours;
    private List<IStateAction<Mob, MobStateId>> stateActions;
    private MobStateId activeState;
    private MobStateId previousState;

    private PhysicMaterialCombine cachedFrictionCombine;
    private float cachedStaticFriction;
    private float cachedDynamicFriction;

    private Vector3 cachedVelocity;
    private Vector3 cachedAngularVelocity;

    // Public properties.
    public float StateTimer => stateTimer;
    public Dictionary<MobStateId, IState<Mob, MobStateId>> States => states;
    public Dictionary<MobBehaviourId, IBehaviour<Mob, MobBehaviourId>> Behaviours => behaviours;
    public List<IStateAction<Mob, MobStateId>> StateActions => stateActions;
    public MobStateId ActiveState => activeState;
    public MobStateId PreviousState => previousState;

    // Public fields.
    [Header("Mob Attributes")]
    public int health;
    public int maxHealth;
    public int special;
    public int maxSpecial;

    [Header("State Attributes")]
    public GameObject statesObject;
    public GameObject stateActionsObject;
    public GameObject behavioursObject;
    [Space]
    public MobStateIdConstant defaultStateId;

    [Header("Component Attributes")]
    public Collider mobCollider;
    public Rigidbody mobRigidBody;
    [Space]
    public GameObject mobRendererObject;
    public GameObject mobDirectionObject;
    public Renderer mobRenderer;
    public Animator mobAnimator;
    [Space]
    public Collider[] passiveHitboxes;
    [Space]
    public DamageActor damageActor;

    private void Awake()
    {
        stateTimer = 0.0F;

        states = new Dictionary<MobStateId, IState<Mob, MobStateId>>();
        behaviours = new Dictionary<MobBehaviourId, IBehaviour<Mob, MobBehaviourId>>();
        stateActions = new List<IStateAction<Mob, MobStateId>>();

        activeState = defaultStateId.mobStateId;
        previousState = defaultStateId.mobStateId;

        var statesToAdd = statesObject
            .GetComponentsInChildren<IState<Mob, MobStateId>>();
        foreach (var stateToAdd in statesToAdd)
            states.Add(stateToAdd.StateId, stateToAdd);

        var behavioursToAdd = behavioursObject
            .GetComponentsInChildren<IBehaviour<Mob, MobBehaviourId>>();
        foreach (var behaviourToAdd in behavioursToAdd)
            behaviours.Add(behaviourToAdd.BehaviourId, behaviourToAdd);

        var stateActionsToAdd = stateActionsObject
            .GetComponentsInChildren<IStateAction<Mob, MobStateId>>();
        foreach (var stateActionToAdd in stateActionsToAdd)
            stateActions.Add(stateActionToAdd);

        StateHighLogic.G.HighLogicStateChanged += OnHighLogicStateChanged;

        cachedFrictionCombine = mobCollider.material.frictionCombine;
        cachedDynamicFriction = mobCollider.material.dynamicFriction;
        cachedStaticFriction = mobCollider.material.staticFriction;
    }

    private void OnDestroy()
    {
        if (StateHighLogic.G != null)
            StateHighLogic.G.HighLogicStateChanged -= OnHighLogicStateChanged;
    }

    private void Start()
    {
        foreach (var behaviour in behaviours.Values)
            behaviour.BeginBehaviour(this);

        states[activeState].BeginState(this);

        for (int i = 0; i < stateActions.Count; i++)
            if (stateActions[i].StateId == activeState)
                stateActions[i].BeginStateAction(this, null);
    }

    private void FixedUpdate()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play)
            return;
        foreach (var behaviour in behaviours.Values)
            behaviour.FixedUpdateBehaviour(this);
        states[activeState].FixedUpdateState(this);
        for (int i = 0; i < stateActions.Count; i++)
            if (stateActions[i].StateId == activeState)
                stateActions[i].FixedUpdateStateAction(this);
    }

    private void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play)
            return;
        foreach (var behaviour in behaviours.Values)
            behaviour.UpdateBehaviour(this);
        states[activeState].UpdateState(this);
        for (int i = 0; i < stateActions.Count; i++)
            if (stateActions[i].StateId == activeState)
                stateActions[i].UpdateStateAction(this);
        stateTimer += Time.deltaTime;
    }

    public void ChangeState(MobStateId stateId, Dictionary<string, object> args = null)
    {
        for (int i = 0; i < stateActions.Count; i++)
            if (stateActions[i].StateId == activeState)
                stateActions[i].EndStateAction(this);
        states[activeState].EndState(this);
        foreach (var behaviour in behaviours.Values)
            behaviour.EndBehaviours(this);
        previousState = activeState;
        activeState = stateId;
        stateTimer = 0.0F;
        foreach (var behaviour in behaviours.Values)
            behaviour.BeginBehaviour(this);
        states[activeState].BeginState(this, args);
        for (int i = 0; i < stateActions.Count; i++)
            if (stateActions[i].StateId == activeState)
                stateActions[i].BeginStateAction(this, args);
    }

    public void ChangeToRandomState(MobStateIdConstant[] stateIds, Dictionary<string,object> args = null)
    {
        int index = UnityEngine.Random.Range(0, stateIds.Length);
        ChangeState(stateIds[index].mobStateId, args);
    }

    public void OnHighLogicStateChanged(object sender, EventArgs e)
    {
        // Rigid body.
        if (StateHighLogic.G.ActiveState == HighLogicStateId.Play
            && StateHighLogic.G.PreviousState != HighLogicStateId.Play)
        {
            mobRigidBody.isKinematic = false;
            mobRigidBody.velocity = cachedVelocity;
            mobRigidBody.angularVelocity = cachedAngularVelocity;
        }

        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play
            && StateHighLogic.G.PreviousState == HighLogicStateId.Play)
        {
            cachedVelocity = mobRigidBody.velocity;
            cachedAngularVelocity = mobRigidBody.angularVelocity;
            mobRigidBody.isKinematic = true;
        }


        // Animator.
        if (mobAnimator.isActiveAndEnabled)
        {
            if (StateHighLogic.G.ActiveState != HighLogicStateId.Play
                && StateHighLogic.G.ActiveState != HighLogicStateId.Film)
                mobAnimator.enabled = false;
        }
        else
        {
            if (StateHighLogic.G.ActiveState == HighLogicStateId.Play
                || StateHighLogic.G.ActiveState == HighLogicStateId.Film)
                mobAnimator.enabled = true;
        }
    }

    public void RestoreCachedFriction()
    {
        mobCollider.material.frictionCombine = cachedFrictionCombine;
        mobCollider.material.dynamicFriction = cachedDynamicFriction;
        mobCollider.material.staticFriction = cachedStaticFriction;
    }
}
