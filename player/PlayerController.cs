using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using System;

using static Assets.script.PlayerConstants;
using static Assets.script.AttributeDataClasses;
using UnityEngine.Serialization;
using Assets.Script;

public class PlayerController : MonoBehaviour
    , IActorDataManager
{
    // state variables.

    private Vector3 storedVelocity = Vector3.zero;

    [NonSerialized] public PlayerStateType currentStateType = PlayerStateType.playerDefault;
    [NonSerialized] public PlayerStateType previousStateType = PlayerStateType.playerDefault;
    [NonSerialized] public Dictionary<PlayerStateType, IPlayerStateController> stateControllers;

    [NonSerialized] public int stateUpdateCount = 0;

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

    float groundedRaycastMaxDistance = 0f;
    float groundedSpherecastMaxDistance = 0f;

    [NonSerialized] public RaycastHit raycastHitInfo;
    [NonSerialized] public bool isRaycastHit = false;
    [NonSerialized] public bool wasRaycastGrounded = false;
    [NonSerialized] public bool isRaycastGrounded = false;

    [NonSerialized] public RaycastHit spherecastHitInfo;
    [NonSerialized] public bool isSpherecastHit = false;
    [NonSerialized] public bool wasSpherecastGrounded = false;
    [NonSerialized] public bool isSpherecastGrounded = false;
    [NonSerialized] public bool isSpherecastGroundedSinceStateBegin = false;

    [NonSerialized] public float raycastGroundedSlopeAngle = 0.0f;
    [NonSerialized] public Vector3 raycastGroundedSlopeNormal = Vector3.up;
    [NonSerialized] public Vector3 raycastGroundedSlopeDirection = Vector3.up;

    [NonSerialized] public float spherecastGroundedSlopeAngle = 0.0f;
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

    [NonSerialized] public float slideResistance = 1.0f;
    [NonSerialized] public float slideForce = 1.0f;
    [NonSerialized] public Vector3 slideDirection = Vector3.zero;

    [NonSerialized] public bool isSlideHit = false;

    // dive variables.

    [NonSerialized] public Vector3 diveDirection = Vector3.zero;

    // damage variables.

    [NonSerialized] public GameObject damageSourceObject = null;
    [NonSerialized] public AttributeDamageData damageData = null;

    [NonSerialized] public bool isDamaged = false;
    [NonSerialized] public float damageTimer = 0.0f;
    [NonSerialized] public float damageInterval = DAMAGE_INTERVAL;

    // repel variables.

    [NonSerialized] public GameObject repelSourceObject = null;
    [NonSerialized] public AttributeDamageData repelData = null;

    // moving object variables.

    [NonSerialized] public List<GameObject> collidingMovingObjects = new List<GameObject>();
    bool isCollidingMovingObject = false;

    // water variables.

    [NonSerialized] public List<GameObject> collidingWaterObjects = new List<GameObject>();
    [NonSerialized] public bool isCollidingWaterObject = false;
    [NonSerialized] public bool isPartialSubmerged = false;
    [NonSerialized] public bool isFullSubmerged = false;
    [NonSerialized] public float waterYLevel = 0;

    // animator variables.

    [NonSerialized] public Vector3 facingDirection = Vector3.zero;
    [NonSerialized] public Vector3 facingDirectionDelta = Vector3.zero;

    public RuntimeAnimatorController animatorGame;
    public RuntimeAnimatorController animatorGameOver;
    public RuntimeAnimatorController animatorCutscene;

    // cutscene variables.

    [NonSerialized] public bool isCutsceneFaceDirection;
    [NonSerialized] public GameObject cutsceneFaceDirectionTargetObject;

    // audio variables.

    [NonSerialized] public AudioSource audioSource;

    public AudioClip soundAttack;
    public AudioClip soundDive;
    public AudioClip soundHurt;
    public AudioClip soundJump;
    public AudioClip soundCrouchJump;
    public AudioClip soundSlide;
    public AudioClip soundWaterJump;
    public AudioClip soundDoubleJump;

    // interface variables.

    ActorData adm;

    private void Start()
    {
        master = GameMasterController.Global;

        // initialise state controllers.

        stateControllers = new Dictionary<PlayerStateType, IPlayerStateController>();

        var states_to_add = gameObject.GetComponentsInChildren<IPlayerStateController>();

        foreach (var state_to_add in states_to_add)
        {
            stateControllers.Add(state_to_add.GetStateType(), state_to_add);
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

        master.GameStateChange += ChangeGameState;

        // add components.

        audioSource = this.gameObject.AddComponent<AudioSource>();

        // initialise actor attributes.

        damageData = new AttributeDamageData();
        repelData = new AttributeDamageData();

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

        master.GameStateChange -= ChangeGameState;
    }

    private void InitialisePhysicalParameters()
    {
        // rigid body inits.

        rigidBody.mass = RIGID_BODY_MASS;
        rigidBody.drag = RIGID_BODY_DRAG;
        rigidBody.angularDrag = RIGID_BODY_ANGULAR_DRAG;

        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

        // collider inits.

        rbCollider.material.bounciness = 0.0f;
        rbCollider.material.bounceCombine = PhysicMaterialCombine.Minimum;

        groundedRaycastMaxDistance = rbCollider.radius + GROUNDED_RAYCAST_ADDITIONAL_DISTANCE;
        groundedSpherecastMaxDistance = GROUNDED_SPHERECAST_ADDITIONAL_DISTANCE;
    }

    private void Update()
    {
        if (master.gameState == GameState.Game)
        {
            UpdatePlayerInput();
            UpdateActorDataManager();

            if(isDamaged)
            {
                damageTimer += Time.deltaTime;

                if (damageTimer >= DAMAGE_INTERVAL)
                    UnsetDamaged();
            }
        }

        
    }

    private void FixedUpdate()
    {
        if (master.gameState == GameState.Game)
        {
            // run update if in game state.

            UpdateWaterStatus();
            UpdateMovingObjectStatus();

            UpdateGroundedRay();
            UpdateGroundedSphere();
            UpdateGravity();
            stateControllers[currentStateType].UpdateStateDragAndFriction(this);

            // update the player state.

            stateUpdateCount++;
            stateControllers[currentStateType].CheckState(this);

            // do state specific actions.

            stateControllers[currentStateType].UpdateState(this);
            stateControllers[currentStateType].UpdateStateSlide(this);
            stateControllers[currentStateType].UpdateStateSpeed(this);

            // update animator.

            UpdateAnimatorVariables();
            stateControllers[currentStateType].UpdateStateAnimator(this);

            // clear any raised inputs.

            UpdateClearRaisedInputs();
        }
        else if (master.gameState == GameState.GameOver)
        {
            
        }
        else if (master.gameState == GameState.Cutscene)
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

        if (master.gameStatePrevious != GameState.Game
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

    private void UpdateWaterStatus()
    {
        if (!isCollidingWaterObject)
            return;

        isPartialSubmerged = (this.transform.position + WATER_PARTIAL_SUBMERGED_OFFSET).y <= waterYLevel;
        isFullSubmerged = (this.transform.position + WATER_FULL_SUBMERGED_OFFSET).y <= waterYLevel;
    }

    private void UpdateMovingObjectStatus()
    {
        if (!isCollidingMovingObject)
            return;
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

            // if grounded, and in the regular state, set the position above the floor.

            if (isRaycastGrounded && currentStateType == PlayerStateType.playerDefault)
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

    private void UpdateAnimatorVariables()
    {
        playerAnimator.SetInteger("anim_game_state", (int)master.gameState);
        playerAnimator.SetInteger("anim_player_state", (int)currentStateType);
        playerAnimator.SetInteger("anim_player_state_update_count", stateUpdateCount);
        playerAnimator.SetBool("anim_is_grounded", isSpherecastGrounded);
        playerAnimator.SetBool("anim_is_moving", rigidBody.velocity.magnitude > 0.2f);
        playerAnimator.SetFloat("anim_horizontal_speed", isInputDirectional ? rigidBody.velocity.magnitude : 0.0f);
        playerAnimator.SetFloat("anim_vertical_speed", rigidBody.velocity.y);
        playerAnimator.SetFloat("anim_speed_water_dive", Mathf.Clamp(rigidBody.velocity.magnitude,0.25f,1.0f));
        playerAnimator.SetBool("anim_is_input_right", inputDirectional.x > 0.5);
        playerAnimator.SetBool("anim_is_input_left", inputDirectional.x < -0.5);
    }

    private void UpdateAnimatorGameOver()
    {

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

        facingDirectionDelta.y = 0.0f;

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

    private void ChangeGameState(object sender, EventArgs e)
    {
        Debug.Log("Game State Changed");

        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;

        if (master.gameState == GameState.Game)
            playerAnimator.runtimeAnimatorController = animatorGame;
        else if (master.gameState == GameState.GameOver)
            playerAnimator.runtimeAnimatorController = animatorGameOver;
        else if (master.gameState == GameState.Cutscene)
            playerAnimator.runtimeAnimatorController = animatorCutscene;
        else
            playerAnimator.runtimeAnimatorController = animatorGame;

        // store, or restore, velocity when changing state.

        if (master.gameState == GameState.Game)
        {
            if(master.gameStatePrevious != GameState.Game)
                ResumeController(e);
        }
        else
        {
            if (master.gameStatePrevious == GameState.Game)
                PauseController(e);
        }

        // if not in a game state, clear the directional input.

        if (args.gameState != GameState.Game)
            inputDirectional = Vector3.zero;
    }

    private void PauseController(EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;

        storedVelocity = rigidBody.velocity;

        rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        rigidBody.isKinematic = true;
        UpdateAnimatorVariables();
    }

    private void ResumeController(EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;

        rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigidBody.isKinematic = false;
        playerAnimator.enabled = true;

        // restore velocity if coming from the pause meny.

        if(args.game_state_previous == GameState.Menu)
            rigidBody.velocity = storedVelocity;
    }

    public void ChangePlayerState(PlayerStateType new_state)
    {
        stateControllers[currentStateType].FinishState(this);

        previousStateType = currentStateType;
        currentStateType = new_state;

        stateControllers[currentStateType].BeginState(this);

        // set general state variables.

        stateUpdateCount = 0;
        isSpherecastGroundedSinceStateBegin = false;
    }

    public void SimpleRepel(GameObject repelSource, float repelForceMultiplier)
    {
        rigidBody.velocity = Vector3.zero;
        var repelVector = (this.transform.position - repelSource.transform.position).normalized;
        rigidBody.AddForce(repelVector * repelForceMultiplier, ForceMode.VelocityChange);
    }


    // collision.

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == GameConstants.TAG_MOVING_OBJECT)
        {
            collidingMovingObjects.Add(collision.gameObject);

            isCollidingMovingObject = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collidingMovingObjects.Contains(collision.gameObject))
        {
            collidingMovingObjects.Remove(collision.gameObject);

            if (collidingMovingObjects.Count == 0)
            {
                isCollidingMovingObject = false;
            }
        }
    }

    // trigger.

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GameConstants.TAG_WATER)
        {
            collidingWaterObjects.Add(other.gameObject);

            isCollidingWaterObject = true;
            waterYLevel = other.bounds.center.y + (other.bounds.size.y / 2);
            
        }

        if (other.gameObject.tag == GameConstants.TAG_DAMAGE_OBJECT)
        {
            PlayerStaticMethods.HandleDamageObject(this, other.gameObject, false);
        }

        if (other.gameObject.tag == GameConstants.TAG_REPEL_OBJECT)
        {
            PlayerStaticMethods.HandleRepelObject(this, other.gameObject);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == GameConstants.TAG_DAMAGE_OBJECT)
        {
            PlayerStaticMethods.HandleDamageObject(this, other.gameObject, true);
        }

        if (other.gameObject.tag == GameConstants.TAG_REPEL_OBJECT)
        {
            PlayerStaticMethods.HandleRepelObject(this, other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (collidingWaterObjects.Contains(other.gameObject))
        {
            collidingWaterObjects.Remove(other.gameObject);

            if (collidingWaterObjects.Count == 0)
            {
                isCollidingWaterObject = false;
                waterYLevel = 0.0f;
                isFullSubmerged = false;
                isPartialSubmerged = false;
            }
        }
    }

    // set and unset.

    public void SetDamaged(AttributeDamageData damageData)
    {
        isDamaged = true;

        damageTimer = 0.0f;
        master.playerController.health -= damageData.damageAmount;
        ChangePlayerState(PlayerStateType.playerDamage);

        audioSource.clip = soundHurt;
        audioSource.Play();

        damageEffectController.SetDamageEffect();
    }

    public void UnsetDamaged()
    {
        isDamaged = false;

        damageTimer = 0.0f;
        damageEffectController.UnsetDamageEffect();
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

    }

    
}
