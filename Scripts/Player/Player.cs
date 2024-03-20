using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
    ,IStateMachine<Player, PlayerStateId>
    ,IBehaviourMachine<Player,PlayerBehaviourId>
{
    // Private fields.
    private float stateTimer;
    private Dictionary<PlayerStateId, IState<Player, PlayerStateId>> states;
    private Dictionary<PlayerBehaviourId, IBehaviour<Player, PlayerBehaviourId>> behaviours;
    private PlayerStateId activeState;
    private PlayerStateId previousState;

    private GravityPlayerBehaviour gravity;
    private GroundCheckPlayerBehaviour groundCheck;
    private WaterPlayerBehaviour water;
    private InteractPlayerBehaviour interact;
    private KeyItemUsePlayerBehaviour keyItemUse;
    private DamagePlayerBehaviour damage;
    private BoundsPlayerBehaviour bounds;

    private Vector3 cachedVelocity;
    private Vector3 cachedAngularVelocity;

    // Public fields.
    [Header("State Attributes")]
    public GameObject playerStatesObject;
    public GameObject playerBehavioursObject;

    [Header("Components")]
    public Collider playerCollider;
    public Rigidbody playerRigidBody;
    [Space]
    public GameObject playerRendererObject;
    public GameObject playerDirectionObject;
    public Renderer playerRenderer;
    public Animator playerAnimator;
    [Space]
    public GameObject interactPromptObject;
    public GameObject keyItemUsePromptObject;
    public GameObject useKeyItemCardObject;
    public SpriteRenderer useKeyItemCardSpriteRenderer;
    [Space]
    public PlayerFilm playerFilm;
    [Space]
    public EmoteActor playerEmoteActor;
    public DamageActor playerDamageActor;
    [Space]
    public Transform camcorderTarget;

    [Header("Prefab Attributes")]
    public GameObject projectilePrefab;

    [Header("Sound Attributes")]
    public AudioSource footstepSound;
    public AudioSource jumpSound;
    public AudioSource doubleJumpSound;
    public AudioSource highJumpSound;
    public AudioSource diveUnderwaterSound;
    public AudioSource attackSound;
    public AudioSource attackUnderwaterSound;
    public AudioSource damageAudioSource;
    public AudioSource hurtSound;
    public AudioSource splashSound;
    public AudioSource lungeSound;
    public AudioSource slamEndSound;
    public AudioSource useKeyItemAudioSource;

    [Header("Damage Source Attributes")]
    public Collider attackHitbox;
    public Collider attackUnderwaterHitbox;
    public Collider slamHitbox;
    public Collider lungeHitbox;

    [Header("Effect Attributes")]
    public ParticleSystem doubleJumpBeginFx;
    public ParticleSystem hurtBeginFx;
    public ParticleSystem attackUnderwaterBeginFx;
    public ParticleSystem waterMoveFx;
    public ParticleSystem waterInOutFx;
    public ParticleSystem slamEndFx;

    // Public Properties.
    public float StateTimer => stateTimer;
    public Dictionary<PlayerStateId, IState<Player, PlayerStateId>> States => states;
    public Dictionary<PlayerBehaviourId, IBehaviour<Player, PlayerBehaviourId>> Behaviours => behaviours;
    public PlayerStateId ActiveState => activeState;
    public PlayerStateId PreviousState => previousState;

    public GravityPlayerBehaviour Gravity
    {
        get
        {
            if (gravity == null)
                gravity = behaviours[PlayerBehaviourId.Gravity] as GravityPlayerBehaviour;
            return gravity;
        }
    }

    public GroundCheckPlayerBehaviour GroundCheck
    {
        get
        {
            if (groundCheck == null)
                groundCheck = behaviours[PlayerBehaviourId.GroundCheck] as GroundCheckPlayerBehaviour;
            return groundCheck;
        }
    }

    public WaterPlayerBehaviour Water
    {
        get
        {
            if (water == null)
                water = behaviours[PlayerBehaviourId.Water] as WaterPlayerBehaviour;
            return water;
        }
    }

    public BoundsPlayerBehaviour Bounds
    {
        get
        {
            if (bounds == null)
                bounds = behaviours[PlayerBehaviourId.Bounds] as BoundsPlayerBehaviour;
            return bounds;
        }
    }

    public InteractPlayerBehaviour Interact => interact;
    public KeyItemUsePlayerBehaviour KeyItemUse => keyItemUse;
    public DamagePlayerBehaviour Damage => damage;

    private void Awake()
    {
        stateTimer = 0.0F;
        states = new Dictionary<PlayerStateId, IState<Player, PlayerStateId>>();
        behaviours = new Dictionary<PlayerBehaviourId, IBehaviour<Player, PlayerBehaviourId>>();
        activeState = PlayerStateId.Default;
        previousState = PlayerStateId.Default;

        var statesToAdd = playerStatesObject
            .GetComponentsInChildren<IState<Player, PlayerStateId>>();
        foreach (var stateToAdd in statesToAdd)
            states.Add(stateToAdd.StateId, stateToAdd);

        var behavioursToAdd = playerBehavioursObject
            .GetComponentsInChildren<IBehaviour<Player, PlayerBehaviourId>>();
        foreach (var behaviourToAdd in behavioursToAdd)
            behaviours.Add(behaviourToAdd.BehaviourId, behaviourToAdd);

        interact = behaviours[PlayerBehaviourId.Interact] as InteractPlayerBehaviour;
        keyItemUse = behaviours[PlayerBehaviourId.KeyItemUse] as KeyItemUsePlayerBehaviour;
        damage = behaviours[PlayerBehaviourId.Damage] as DamagePlayerBehaviour;

        StateHighLogic.G.HighLogicStateChanged += OnHighLogicStateChanged;
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

        playerCollider.material.frictionCombine = PhysicMaterialCombine.Minimum;
        playerCollider.material.bounceCombine = PhysicMaterialCombine.Minimum;
    }

    private void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play
            && StateHighLogic.G.ActiveState != HighLogicStateId.Dead)
            return;

        foreach (var behaviour in behaviours.Values)
            behaviour.UpdateBehaviour(this);

        states[activeState].UpdateState(this);

        stateTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play
            && StateHighLogic.G.ActiveState != HighLogicStateId.Dead)
            return;

        foreach (var behaviour in behaviours.Values)
            behaviour.FixedUpdateBehaviour(this);

        states[activeState].FixedUpdateState(this);
    }

    public void ChangeState(PlayerStateId stateId, Dictionary<string, object> args = null)
    {
        states[activeState].EndState(this);
        foreach (var behaviour in behaviours.Values)
            behaviour.EndBehaviours(this);
        previousState = activeState;
        activeState = stateId;
        stateTimer = 0.0F;
        foreach (var behaviour in behaviours.Values)
            behaviour.BeginBehaviour(this);
        states[activeState].BeginState(this, args);  
    }

    public void OnHighLogicStateChanged(object sender, EventArgs e)
    {
        // Rigid body.
        if(StateHighLogic.G.ActiveState == HighLogicStateId.Play
            && StateHighLogic.G.PreviousState != HighLogicStateId.Play)
        {
            playerRigidBody.isKinematic = false;
            playerRigidBody.velocity = cachedVelocity;
            playerRigidBody.angularVelocity = cachedAngularVelocity;
        }
        
        if(StateHighLogic.G.ActiveState != HighLogicStateId.Play 
            && StateHighLogic.G.PreviousState == HighLogicStateId.Play)
        {
            cachedVelocity = playerRigidBody.velocity;
            cachedAngularVelocity = playerRigidBody.angularVelocity;
            playerRigidBody.isKinematic = true; 
        }


        // Animator.
        if(playerAnimator.isActiveAndEnabled)
        {
            if (StateHighLogic.G.ActiveState != HighLogicStateId.Play
                && StateHighLogic.G.ActiveState != HighLogicStateId.Film)
                playerAnimator.enabled = false;
        }
        else
        {
            if (StateHighLogic.G.ActiveState == HighLogicStateId.Play
                || StateHighLogic.G.ActiveState == HighLogicStateId.Film)
                playerAnimator.enabled = true;
        }

        // Prompts and cards.
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play
            && StateHighLogic.G.PreviousState == HighLogicStateId.Play)
        {
            interactPromptObject.SetActive(false);
            Interact.ClearInteractable();
            keyItemUsePromptObject.SetActive(false);
            KeyItemUse.ClearKeyItemUsable();
            useKeyItemCardObject.SetActive(false);
        }
    }

    public void ClearCachedVelocity()
    {
        cachedVelocity = Vector3.zero;
        cachedAngularVelocity = Vector3.zero;
    }

    private void OnGUI()
    {
        string debug
            = $"\nActive State: {activeState}"
            + $"\nRigid Body: {playerRigidBody.position}|{playerRigidBody.velocity}"
            + $"\nChecksphere (IH, IG):"
            + $"\n    |{GroundCheck.IsCheckSphereHit}"
            + $"\n    |{GroundCheck.IsCheckSphereGrounded}"
            + $"\nSpherecast: {GroundCheck.SpherecastGroundAngle}° ({GroundCheck.SpherecastGroundNormal})"
            + $"\nWater: (IPS, IFS)"
            + $"\n    |{Water.IsPartialSubmerged}"
            + $"\n    |{Water.IsFullSubmerged}"
            + $"\nWater Height: {Water.WaterHeight}";
        GUI.Label(new Rect(0, 0, 500, 500), debug);
    }
}