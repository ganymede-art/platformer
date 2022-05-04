using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using System;
using static Assets.Script.GameConstants;
using static Assets.Script.PlayerConstants;
using UnityEngine.Serialization;
using Assets.Script;

public class PlayerController : MonoBehaviour
    , IActorDataManager
{
    // state variables.

    private Vector3 storedVelocity = Vector3.zero;

    [NonSerialized] public string currentStateType = PLAYER_STATE_DEFAULT;
    [NonSerialized] public string previousStateType = PLAYER_STATE_DEFAULT;

    [NonSerialized] public Dictionary<string, IPlayerState> states;
    [NonSerialized] public int stateFixedUpdateCount = 0;

    // behaviour variables.

    [NonSerialized] public Dictionary<string, IPlayerBehaviour> behaviours;
    [NonSerialized] public PlayerBehaviourWater behaviourWater;
    [NonSerialized] public PlayerBehaviourDamage behaviourDamage;
    [NonSerialized] public PlayerBehaviourRepel behaviourRepel;

    // input variables.

    [NonSerialized] public Vector3 inputDirectional = Vector3.zero;
    [NonSerialized] public bool isInputDirectional = false;
    [NonSerialized] public bool wasInputDirectional = false;

    [NonSerialized] public bool isRaisedSouth = false;
    [NonSerialized] public bool isRaisedWest = false;
    [NonSerialized] public bool isRaisedEast = false;
    [NonSerialized] public bool isRaisedEastExtra = false;

    // component variables.

    [NonSerialized] public GameMasterController master;
    [NonSerialized] public Rigidbody rigidBody;
    [NonSerialized] public GameObject rbColliderObject;
    [NonSerialized] public SphereCollider rbCollider;
    [NonSerialized] public Animator playerAnimator;
    [NonSerialized] public GameObject cameraObject;
    [NonSerialized] public GameObject rendererObject;
    [NonSerialized] public Renderer playerRenderer;
    [NonSerialized] public GameObject directionObject;
    [NonSerialized] public GameObject eyesObject;
    [NonSerialized] public ActorEyeController eyeController;

    [NonSerialized] public GameObject attackForward1Object;
    [NonSerialized] public Collider attackForward1Collider;

    [NonSerialized] public GameObject attackForward2Object;
    [NonSerialized] public Collider attackForward2Collider;

    [NonSerialized] public GameObject attackDown1Object;
    [NonSerialized] public Collider attackDown1Collider;

    [NonSerialized] public ActorDamageEffectController damageEffectController;

    // physical variables.

    [NonSerialized] public bool isUnderGravity = true;

    // grounded variables.

    [NonSerialized] public RaycastHit raycastHitInfo;
    [NonSerialized] public bool isRaycastHit = false;
    [NonSerialized] public bool wasRaycastGrounded = false;
    [NonSerialized] public bool isRaycastGrounded = false;

    [NonSerialized] public RaycastHit spherecastHitInfo;
    [NonSerialized] public bool isSpherecastHit = false;
    [NonSerialized] public bool wasSpherecastGrounded = false;
    [NonSerialized] public bool isSpherecastGrounded = false;
    [NonSerialized] public bool isSpherecastGroundedSinceStateBegin = false;

    [NonSerialized] public bool isChecksphereCollision = false;
    [NonSerialized] public bool isNearCheckSphereCollision = false;
    [NonSerialized] public bool isChecksphereGrounded = false;
    [NonSerialized] public bool isNearChecksphereGrounded = false;

    [NonSerialized] public float raycastGroundAngle = 0.0F;
    [NonSerialized] public Vector3 raycastGroundNormal = Vector3.up;
    [NonSerialized] public Vector3 raycastGroundedSlopeDirection = Vector3.up;

    [NonSerialized] public float spherecastGroundAngle = 0.0F;
    [NonSerialized] public Vector3 spherecastGroundNormal = Vector3.up;

    [NonSerialized] public GroundData groundData;

    // movement variables.

    [NonSerialized] public RaycastHit movementHit;
    [NonSerialized] public RaycastHit stepMovementHit;

    [NonSerialized] public bool isMovementHit = false;
    [NonSerialized] public bool isStepMovementHit = false;

    // jump variables.

    [NonSerialized] public int jumpPersistEnergy = 0;

    // dive variables.

    [NonSerialized] public Vector3 diveDirection = Vector3.zero;

    // moving object variables.

    [NonSerialized] public List<GameObject> collidingMovingObjects = new List<GameObject>();
    [NonSerialized] public bool isCollidingMovingObject = false;

    // animator variables.

    [NonSerialized] public Vector3 facingDirection = Vector3.zero;
    [NonSerialized] public Vector3 facingDirectionDelta = Vector3.zero;

    // cutscene variables.

    [NonSerialized] public bool isCutsceneFaceDirection;
    [NonSerialized] public GameObject cutsceneFaceDirectionTargetObject;

    // audio variables.

    [NonSerialized] public AudioSource audioSource;
    [NonSerialized] public ActorStepEffectController stepEffectController;

    // fx systems.

    [NonSerialized] public ParticleSystem impactDownFx;

    // interface variables.

    ActorData adm;

    // public fields.

    [Header("State and Behaviour Objects")]
    public GameObject statesObject;
    public GameObject behavioursObject;

    [Header("Actor Effect Objects")]
    public GameObject stepEffectControllerObject;

    [Header("Sounds")]
    public AudioClip attackSound;
    public AudioClip diveSound;
    public AudioClip jumpSound;
    [FormerlySerializedAs("crouchJumpSound")]
    public AudioClip highJumpSound;
    public AudioClip slideSound;
    public AudioClip waterJumpSound;
    [FormerlySerializedAs("doubleJumpSound")]
    public AudioClip flutterSound;
    public AudioClip shootSound;
    [FormerlySerializedAs("slamSound")]
    public AudioClip slamUpSound;
    public AudioClip slamDownSound;
    public AudioClip slamImpactSound;

    [Header("FX")]
    [FormerlySerializedAs("slamImpactFxObject")]
    public GameObject impactDownFxObject;

    private void Start()
    {
        master = GameMasterController.Global;

        // initialise state controllers.

        states = new Dictionary<string, IPlayerState>();
        var statesToAdd = statesObject.GetComponents<IPlayerState>();
        foreach (var stateToAdd in statesToAdd)
        {
            states.Add(stateToAdd.GetStateType(), stateToAdd);
        }

        // initialise behaviour controllers.

        behaviours = new Dictionary<string, IPlayerBehaviour>();
        var behavioursToAdd = behavioursObject.GetComponents<IPlayerBehaviour>();
        foreach(var behaviourToAdd in behavioursToAdd)
        {
            behaviours.Add(behaviourToAdd.GetBehaviourType(), behaviourToAdd);
        }
        behaviourWater = behaviours[PLAYER_BEHAVIOUR_WATER] as PlayerBehaviourWater;
        behaviourDamage = behaviours[PLAYER_BEHAVIOUR_DAMAGE] as PlayerBehaviourDamage;
        behaviourRepel = behaviours[PLAYER_BEHAVIOUR_REPEL] as PlayerBehaviourRepel;

        // initialise actor effect controllers.

        stepEffectController = stepEffectControllerObject.GetComponent<ActorStepEffectController>();

        // initialise componenets.

        rigidBody = GetComponent<Rigidbody>();

        directionObject = transform.Find(DIRECTION_OBJECT).gameObject;
        rbColliderObject = transform.Find(COLLIDER_OBJECT).gameObject;
        rendererObject = transform.Find(RENDERER_OBJECT).gameObject;
        eyesObject = transform.Find(EYES_OBJECT).gameObject;

        rbCollider = GameObject.Find(COLLIDER_OBJECT).GetComponent<SphereCollider>();
        playerRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        eyeController = eyesObject.GetComponent<ActorEyeController>();

        playerAnimator = GetComponent<Animator>();
        cameraObject = GameMasterController.GlobalCameraObject;

        attackForward1Object = GameObject.Find(ATTACK_FORWARD_1_OBJECT);
        attackForward1Collider = attackForward1Object.GetComponent<Collider>();
        attackForward1Collider.enabled = false;

        attackForward2Object = GameObject.Find(ATTACK_FORWARD_2_OBJECT);
        attackForward2Collider = attackForward2Object.GetComponent<Collider>();
        attackForward2Collider.enabled = false;

        attackDown1Object = GameObject.Find(ATTACK_DOWN_1_OBJECT);
        attackDown1Collider = attackDown1Object.GetComponent<Collider>();
        attackDown1Collider.enabled = false;

        // initialise fx.

        impactDownFx = impactDownFxObject.GetComponent<ParticleSystem>();

        damageEffectController = this.gameObject.GetComponentInChildren<ActorDamageEffectController>();

        // add listeners.

        master.GameStateChange += OnGameStateChange;

        // add components.

        audioSource = this.gameObject.AddComponent<AudioSource>();

        // initialise interface.

        adm = new ActorData();
        adm = ActorData.GetDefault();

        // setup.

        InitialiseRigidBody();

        // defaultGroundData

        groundData = GameDefaultsController.Global.defaultGroundData;
    }

    void OnDestroy()
    {
        // remove listeners.

        master.GameStateChange -= OnGameStateChange;
    }

    private void InitialiseRigidBody()
    {
        // rigid body inits.

        rigidBody.mass = RIGID_BODY_MASS;
        rigidBody.drag = RIGID_BODY_DRAG;
        rigidBody.angularDrag = RIGID_BODY_ANGULAR_DRAG;

        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

        // collider inits.

        rbCollider.material.bounciness = 0.0F;
        rbCollider.material.bounceCombine = PhysicMaterialCombine.Minimum;
    }

    private void Update()
    {
        if (master.gameState == GAME_STATE_GAME)
        {
            UpdatePlayerInput();
            UpdateActorDataManager();
        }
    }

    private void FixedUpdate()
    {
        if (master.gameState == GAME_STATE_GAME)
        {
            UpdateGroundedRaycast();
            UpdateGroundedSpherecast();
            UpdateGroundedCheckSphere();
            UpdateGravity();

            // update the player state.
            stateFixedUpdateCount++;
            states[currentStateType].CheckState(this);

            // do state specific actions.
            states[currentStateType].FixedUpdateState(this);

            // update animator.
            states[currentStateType].UpdateState(this);
        }
        else if (master.gameState == GAME_STATE_CUTSCENE)
        {
            UpdateAnimatorCutscene();
        }
        else if(master.gameState == GAME_STATE_GAME_OVER)
        {
            // limited version of the normal game update.

            UpdateGroundedRaycast();
            UpdateGroundedSpherecast();
            UpdateGravity();
        }
        else
        {
            playerAnimator.enabled = false;
        }

        // clear any raised inputs.
        UpdateClearRaisedInputs();
    }

    private void UpdatePlayerInput()
    {

        // ignore all input for a short
        // time when entering game state
        // from a non-game state.

        if (master.gameStatePrevious != GAME_STATE_GAME
            && master.GameStateTime <= INPUT_GAME_STATE_DELAY)
            return;

        // get previous inputs.

        wasInputDirectional = isInputDirectional;

        // get input from input mapper.

        float input_horizontal = master.inputController.axisMoveHorizontal.ReadValue<float>();
        float input_vertical = master.inputController.axisMoveVertical.ReadValue<float>();

        Vector3 input = new Vector3(input_horizontal, 0.0f, input_vertical);

        if (input.magnitude > 1)
            input = input.normalized;

        inputDirectional = input;
        isInputDirectional = inputDirectional.magnitude > INPUT_DIRECTIONAL_THRESHOLD;
        
        // raise events for fixed update
        // edge detection.

        if (!master.inputController.wasInputSouth && master.inputController.isInputSouth)
            isRaisedSouth = true;

        if (!master.inputController.wasInputWest && master.inputController.isInputWest)
            isRaisedWest = true;

        if (!master.inputController.wasInputEast && master.inputController.isInputEast)
            isRaisedEast = true;

        if (!master.inputController.wasInputEastExtra 
            && master.inputController.isInputEastExtra)
            isRaisedEastExtra = true;
    }

    private void UpdateGroundedRaycast()
    {
        // check grounding by ray.

        isRaycastHit = Physics.Raycast(this.transform.position, Vector3.down, out raycastHitInfo, GROUNDED_RAYCAST_DISTANCE);

        if (isRaycastHit)
        {
            // set the angle of the surface.
            raycastGroundAngle = Vector3.Angle(raycastHitInfo.normal, Vector3.up);

            // set the slope normal.
            raycastGroundNormal = raycastHitInfo.normal;

            // set the ground slope direction.
            var temp = Vector3.Cross(raycastHitInfo.normal, Vector3.down);
            raycastGroundedSlopeDirection = Vector3.Cross(temp, raycastHitInfo.normal);

            // set grounded, if under max distance.
            wasRaycastGrounded = isRaycastGrounded;
            isRaycastGrounded = raycastHitInfo.distance <= MAX_GROUNDED_RAYCAST_DISTANCE
                && raycastGroundAngle <= MAX_GROUNDED_ANGLE;

            // if grounded, and in the regular state, set the position above the floor.

            if (isRaycastGrounded && currentStateType == GameConstants.PLAYER_STATE_DEFAULT)
            {
                rigidBody.MovePosition(new Vector3(
                    rigidBody.position.x,
                    raycastHitInfo.point.y + raycastHitInfo.distance,
                    rigidBody.position.z));
                Debug.DrawRay(transform.position, Vector3.down, Color.magenta);
            }
            
        }
    }

    private void UpdateGroundedSpherecast()
    {
        // check grounding by sphere.

        isSpherecastHit = Physics.SphereCast(transform.position, GROUNDED_SPHERECAST_RADIUS, Vector3.down, out spherecastHitInfo, GROUNDED_SPHERECAST_DISTANCE, MASK_PLAYER_IGNORES);

        if (isSpherecastHit)
        {
            if (!isSpherecastGroundedSinceStateBegin)
                isSpherecastGroundedSinceStateBegin = isSpherecastGrounded;

            // if also ray grounded
            // set the slope normal.

            spherecastGroundNormal = spherecastHitInfo.normal;

            // if also ray grounded
            // set the angle of the surface.
            spherecastGroundAngle = Vector3.Angle(spherecastHitInfo.normal, Vector3.up);

            wasSpherecastGrounded = isSpherecastGrounded;
            isSpherecastGrounded = spherecastHitInfo.distance <= MAX_GROUNDED_SPHERECAST_DISTANCE
                && spherecastGroundAngle <= MAX_GROUNDED_ANGLE;

            if (isSpherecastGrounded)
            {
                // set the ground data.

                if (GameSceneController.Global.groundDataObjects.ContainsKey(spherecastHitInfo.collider.gameObject))
                    groundData = GameSceneController.Global.groundDataObjects[spherecastHitInfo.collider.gameObject];
                else
                    groundData = GameDefaultsController.Global.defaultGroundData;
            }
        }
    }

    private void UpdateGroundedCheckSphere()
    {
        isChecksphereCollision = Physics.CheckSphere
            (this.transform.position + GROUNDED_CHECKSPHERE_OFFSET
            , GROUNDED_CHECKSPHERE_RADIUS
            , MASK_PLAYER_IGNORES);

        isChecksphereGrounded 
            = isChecksphereCollision 
            && spherecastGroundAngle <= MAX_GROUNDED_ANGLE;

        isNearCheckSphereCollision = Physics.CheckSphere
            (this.transform.position + NEAR_GROUNDED_CHECKSPHERE_OFFSET
            , GROUNDED_CHECKSPHERE_RADIUS
            , MASK_PLAYER_IGNORES);

        isNearChecksphereGrounded
            = isNearCheckSphereCollision
            && spherecastGroundAngle <= MAX_GROUNDED_ANGLE;
    }

    private void UpdateGravity()
    {
        // apply rising or falling gravity.

        if (!isUnderGravity)
            return;

        if (rigidBody.velocity.y > 0)
            rigidBody.AddForce(Physics.gravity, ForceMode.Acceleration);
        else
            rigidBody.AddForce(Physics.gravity * GRAVITY_MULTIPLIER, ForceMode.Acceleration);
    }

    private void UpdateAnimatorCutscene()
    {

        if (master.cutsceneController.orderedEvents.Count == 0 && !isCutsceneFaceDirection)
            return;

        if(isCutsceneFaceDirection)
        {
            if(cutsceneFaceDirectionTargetObject == null)
            {
                isCutsceneFaceDirection = false;
                return;
            }

            facingDirectionDelta = Vector3.RotateTowards(rendererObject.transform.forward,
            cutsceneFaceDirectionTargetObject.transform.position - rendererObject.transform.position,
            PlayerConstants.ANIMATION_TURNING_SPEED_MULTIPLIER,
            0.0f);
        }
        else
        {
            if (master.cutsceneController.orderedEvents.Count == 0)
                return;

            facingDirectionDelta = Vector3.RotateTowards(rendererObject.transform.forward,
            master.cutsceneController.orderedEvents[0].controllerSource.transform.position - rendererObject.transform.position,
            PlayerConstants.ANIMATION_TURNING_SPEED_MULTIPLIER,
            0.0f);
        }

        facingDirectionDelta.y = 0.0F;

        // Move our position a step closer to the target.
        rendererObject.transform.rotation = Quaternion.LookRotation(facingDirectionDelta);

    }

    // clear raised inputs.

    private void UpdateClearRaisedInputs()
    {
        isRaisedSouth = false;
        isRaisedWest = false;
        isRaisedEast = false;
        isRaisedEastExtra = false;
    }

    // state change.

    private void OnGameStateChange(object sender, EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;

        // store, or restore, velocity when changing state.

        if (master.gameState == GAME_STATE_GAME || master.gameState == GAME_STATE_GAME_OVER)
        {
            if(master.gameStatePrevious != GAME_STATE_GAME)
                ResumeController(e);
        }
        else
        {
            if (master.gameState == GAME_STATE_CUTSCENE)
            {
                playerAnimator.ResetAllAnimatorTriggers();
                playerAnimator.SetTrigger("idle");
            }

            if (master.gameStatePrevious == GAME_STATE_GAME)
                PauseController(e);
        }

        // if not in a game state, clear the directional input.

        if (args.gameState != GAME_STATE_GAME)
            inputDirectional = Vector3.zero;
    }

    private void PauseController(EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;

        storedVelocity = rigidBody.velocity;

        rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        rigidBody.isKinematic = true;
    }

    private void ResumeController(EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;

        rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigidBody.isKinematic = false;
        playerAnimator.enabled = true;

        // restore velocity if coming from the pause menu.

        if(args.game_state_previous == GAME_STATE_MENU_PAUSE)
            rigidBody.velocity = storedVelocity;
    }

    public void ChangePlayerState(string newState, params object[] parameters)
    {
        // finish current state.

        states[currentStateType].FinishState(this);

        previousStateType = currentStateType;
        currentStateType = newState;
        
        // begin new state, pass params if provided.

        if(parameters == null || parameters.Length == 0)
            states[currentStateType].BeginState(this);
        else
            states[currentStateType].BeginState(this, parameters);

        // set general state variables.

        stateFixedUpdateCount = 0;
        isSpherecastGroundedSinceStateBegin = false;
    }

    public void SimpleRepel(GameObject repelSource, float repelForceMultiplier)
    {
        rigidBody.velocity = Vector3.zero;
        var repelVector = (this.transform.position - repelSource.transform.position).normalized;
        rigidBody.AddForce(repelVector * repelForceMultiplier, ForceMode.VelocityChange);
    }

    // interface managers.

    private void UpdateActorDataManager()
    {
        adm.isInWater = behaviourWater.isCollidingWaterObject;
        adm.isSubmerged = behaviourWater.isFullSubmerged;
        adm.waterYLevel = behaviourWater.waterYLevel;
        adm.groundData = groundData;
    }

    public ActorData GetActorData()
    {
        return adm;
    }

    private void OnGUI()
    {
        string output = "[DEBUG]"
            + "\nplayerState: " + currentStateType
            + "\nisRaycastGrounded: " + isRaycastGrounded
            + "\nisSpherecastGrounded: " + isSpherecastGrounded
            + "\nSpherecastAngle: " + spherecastGroundAngle
            + "\nisSphereCheckGrounded: " + isChecksphereGrounded
            + "\nisNearSphereCheckGrounded: " + isNearChecksphereGrounded
            ;
            

        GUI.color = Color.black;
        GUI.Label(DEBUG_RECTANGLE_2, output);
        GUI.color = Color.white;
        GUI.Label(DEBUG_RECTANGLE_1, output);
    }
}
