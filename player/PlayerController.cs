using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using System;
using static Assets.script.GameConstants;
using static Assets.script.PlayerConstants;
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

    // input variables.

    [NonSerialized] public Vector3 inputDirectional = Vector3.zero;
    [NonSerialized] public bool isInputDirectional = false;
    [NonSerialized] public bool wasInputDirectional = false;

    [NonSerialized] public bool isRaisedSouth = false;
    [NonSerialized] public bool isRaisedWest = false;
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

    float groundedRaycastMaxDistance = 0F;
    float groundedSpherecastMaxDistance = 0F;

    [NonSerialized] public RaycastHit raycastHitInfo;
    [NonSerialized] public bool isRaycastHit = false;
    [NonSerialized] public bool wasRaycastGrounded = false;
    [NonSerialized] public bool isRaycastGrounded = false;
    [NonSerialized] public bool isNearRaycastGrounded = false;

    [NonSerialized] public RaycastHit spherecastHitInfo;
    [NonSerialized] public bool isSpherecastHit = false;
    [NonSerialized] public bool wasSpherecastGrounded = false;
    [NonSerialized] public bool isSpherecastGrounded = false;
    [NonSerialized] public bool isNearSpherecaseGrounded = false;
    [NonSerialized] public bool isSpherecastGroundedSinceStateBegin = false;



    [NonSerialized] public float raycastGroundedSlopeAngle = 0.0F;
    [NonSerialized] public Vector3 raycastGroundedSlopeNormal = Vector3.up;
    [NonSerialized] public Vector3 raycastGroundedSlopeDirection = Vector3.up;

    [NonSerialized] public float spherecastGroundedSlopeAngle = 0.0F;
    [NonSerialized] public Vector3 spherecastGroundedSlopeNormal = Vector3.up;

    [NonSerialized] public AttributeGroundData groundData;

    // movement variables.

    [NonSerialized] public RaycastHit movementHit;
    [NonSerialized] public RaycastHit stepMovementHit;

    [NonSerialized] public bool isMovementHit = false;
    [NonSerialized] public bool isStepMovementHit = false;

    // jump variables.

    [NonSerialized] public int jumpPersistEnergy = 0;

    // slide variables.

    [NonSerialized] public float slideResistance = 1.0F;
    [NonSerialized] public float slideForce = 1.0F;
    [NonSerialized] public Vector3 slideDirection = Vector3.zero;

    [NonSerialized] public bool isSlideHit = false;

    // dive variables.

    [NonSerialized] public Vector3 diveDirection = Vector3.zero;

    // moving object variables.

    [NonSerialized] public List<GameObject> collidingMovingObjects = new List<GameObject>();
    [NonSerialized] public bool isCollidingMovingObject = false;

    // water variables.

    [NonSerialized] public List<GameObject> collidingWaterObjects = new List<GameObject>();
    [NonSerialized] public bool isCollidingWaterObject = false;
    [NonSerialized] public bool isPartialSubmerged = false;
    [NonSerialized] public bool isFullSubmerged = false;
    [NonSerialized] public float waterYLevel = 0;

    // animator variables.

    [NonSerialized] public Vector3 facingDirection = Vector3.zero;
    [NonSerialized] public Vector3 facingDirectionDelta = Vector3.zero;

    // cutscene variables.

    [NonSerialized] public bool isCutsceneFaceDirection;
    [NonSerialized] public GameObject cutsceneFaceDirectionTargetObject;

    // audio variables.

    [NonSerialized] public AudioSource audioSource;

    // interface variables.

    ActorData adm;

    // public fields.

    [Header("State and Behaviour Objects")]
    public GameObject statesObject;
    public GameObject behavioursObject;

    [Header("Sounds")]
    public AudioClip attackSound;
    public AudioClip diveSound;
    public AudioClip jumpSound;
    [FormerlySerializedAs("crouchJumpSound")]
    public AudioClip highJumpSound;
    public AudioClip slideSound;
    public AudioClip waterJumpSound;
    public AudioClip doubleJumpSound;
    public AudioClip shootSound;

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

        // initialise componenets.

        rigidBody = this.GetComponent<Rigidbody>();

        directionObject = transform.Find(DIRECTION_OBJECT).gameObject;
        rbColliderObject = transform.Find(COLLIDER_OBJECT).gameObject;
        rendererObject = transform.Find(RENDERER_OBJECT).gameObject;
        eyesObject = transform.Find(EYES_OBJECT).gameObject;

        rbCollider = GameObject.Find(COLLIDER_OBJECT).GetComponent<SphereCollider>();
        playerRenderer = this.GetComponentInChildren<SkinnedMeshRenderer>();
        eyeController = eyesObject.GetComponent<ActorEyeController>();

        playerAnimator = this.GetComponent<Animator>();
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

        damageEffectController = this.gameObject.GetComponentInChildren<ActorDamageEffectController>();

        // add listeners.

        master.GameStateChange += OnGameStateChange;

        // add components.

        audioSource = this.gameObject.AddComponent<AudioSource>();

        // initialise interface.

        adm = new ActorData();
        adm = ActorData.GetDefault();

        // setup.

        InitialisePhysicalParameters();

        // defaultGroundData

        groundData = DEFAULT_GROUND_DATA;
    }

    void OnDestroy()
    {
        // remove listeners.

        master.GameStateChange -= OnGameStateChange;
    }

    private void InitialisePhysicalParameters()
    {
        // rigid body inits.

        rigidBody.mass = RIGID_BODY_MASS;
        rigidBody.drag = RIGID_BODY_DRAG;
        rigidBody.angularDrag = RIGID_BODY_ANGULAR_DRAG;

        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

        // collider inits.

        rbCollider.material.bounciness = 0.0F;
        rbCollider.material.bounceCombine = PhysicMaterialCombine.Minimum;

        groundedRaycastMaxDistance = rbCollider.radius + GROUNDED_RAYCAST_ADDITIONAL_DISTANCE;
        groundedSpherecastMaxDistance = GROUNDED_SPHERECAST_ADDITIONAL_DISTANCE;
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
            UpdateGroundedRay();
            UpdateGroundedSphere();
            UpdateGravity();

            // update the player state.
            stateFixedUpdateCount++;
            states[currentStateType].CheckState(this);

            // do state specific actions.
            states[currentStateType].FixedUpdateState(this);

            // update animator.
            states[currentStateType].UpdateState(this);

            // clear any raised inputs.
            UpdateClearRaisedInputs();
        }
        else if (master.gameState == GAME_STATE_CUTSCENE)
        {
            UpdateAnimatorCutscene();
        }
        else
        {
            playerAnimator.enabled = false;
        }
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

        if (!master.inputController.wasInputWest && master.inputController.inInputWest)
            isRaisedWest = true;

        if (!master.inputController.wasInputEastExtra 
            && master.inputController.isInputEastExtra)
            isRaisedEastExtra = true;
    }

    private void UpdateGroundedRay()
    {
        // check grounding by ray.

        isRaycastHit = Physics.Raycast(this.transform.position, Vector3.down, out raycastHitInfo, GROUNDED_RAYCAST_DISTANCE);

        if (isRaycastHit)
        {
            // set the angle of the surface.
            raycastGroundedSlopeAngle = Vector3.Angle(raycastHitInfo.normal, Vector3.up);

            // set the slope normal.
            raycastGroundedSlopeNormal = raycastHitInfo.normal;

            // set the ground slope direction.
            var temp = Vector3.Cross(raycastHitInfo.normal, Vector3.down);
            raycastGroundedSlopeDirection = Vector3.Cross(temp, raycastHitInfo.normal);

            // set grounded, if under max distance.
            wasRaycastGrounded = isRaycastGrounded;
            isRaycastGrounded = raycastHitInfo.distance <= groundedRaycastMaxDistance 
                && raycastGroundedSlopeAngle <= SLIDE_ANGLE_MAX;
            isNearRaycastGrounded = raycastHitInfo.distance <= MAX_NEAR_GROUNDED_RAYCAST_DISTANCE
                && raycastGroundedSlopeAngle <= SLIDE_ANGLE_MAX;

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

    private void UpdateGroundedSphere()
    {
        // check grounding by sphere.

        isSpherecastHit = Physics.SphereCast(transform.position, GROUNDED_SPHERECAST_RADIUS, Vector3.down, out spherecastHitInfo, GROUNDED_RAYCAST_DISTANCE);

        if (isSpherecastHit)
        {
            if (!isSpherecastGroundedSinceStateBegin)
                isSpherecastGroundedSinceStateBegin = isSpherecastGrounded;

            // if also ray grounded
            // set the slope normal.
            spherecastGroundedSlopeNormal = (isRaycastGrounded)
                ? spherecastHitInfo.normal
                : raycastHitInfo.normal;

            // if also ray grounded
            // set the angle of the surface.
            spherecastGroundedSlopeAngle = (isRaycastGrounded)
                ? Vector3.Angle(raycastHitInfo.normal, Vector3.up)
                : raycastGroundedSlopeAngle;

            wasSpherecastGrounded = isSpherecastGrounded;
            isSpherecastGrounded = spherecastHitInfo.distance <= groundedSpherecastMaxDistance
                && spherecastGroundedSlopeAngle <= SLIDE_ANGLE_MAX;
            isNearSpherecaseGrounded = spherecastHitInfo.distance <= MAX_NEAR_GROUNDED_SPHERECAST_DISTANCE
                && spherecastGroundedSlopeAngle <= SLIDE_ANGLE_MAX;

            if (isSpherecastGrounded)
            {
                // set the ground data.

                if (spherecastHitInfo.collider.gameObject
                    .GetComponent<AttributeGround>() == null)
                    groundData = DEFAULT_GROUND_DATA;
                else
                    groundData = spherecastHitInfo.collider.gameObject
                        .GetComponent<AttributeGround>().groundData ?? DEFAULT_GROUND_DATA;
            }
        }
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
        isRaisedEastExtra = false;
    }

    // state change.

    private void OnGameStateChange(object sender, EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;

        // store, or restore, velocity when changing state.

        if (master.gameState == GAME_STATE_GAME)
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
            else if(master.gameState == GAME_STATE_GAME_OVER)
            {
                playerAnimator.ResetAllAnimatorTriggers();
                playerAnimator.SetTrigger("emote_die");
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
        adm.isInWater = isCollidingWaterObject;
        adm.isSubmerged = isFullSubmerged;
        adm.waterYLevel = waterYLevel;
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
            + "\nisNearRaycastGrounded: " + isNearRaycastGrounded
            + "\nisNearSpherecastGrounded: " + isNearSpherecaseGrounded;

        GUI.color = Color.black;
        GUI.Label(DEBUG_RECTANGLE_2, output);
        GUI.color = Color.white;
        GUI.Label(DEBUG_RECTANGLE_1, output);
    }
}
