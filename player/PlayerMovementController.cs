using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using System;
using static Assets.script.PlayerEnums;
using static Assets.script.PlayerConstants;
using static Assets.script.AttributeDataClasses;

public class PlayerMovementController : MonoBehaviour
    , IActorFootstepManager
    , IActorDataManager
{
    // state variables.

    private Vector3 stored_velocity = Vector3.zero;

    [System.NonSerialized] public PlayerEnums.PlayerState player_state = PlayerState.player_default;
    [System.NonSerialized] public PlayerEnums.PlayerState player_state_previous = PlayerState.player_default;
    [System.NonSerialized] public Dictionary<PlayerEnums.PlayerState, IPlayerStateController> player_state_controllers;

    [System.NonSerialized] public PlayerStateDefaultController state_default;
    [System.NonSerialized] public PlayerStateJumpController state_jump;
    [System.NonSerialized] public PlayerStateWaterDefaultController state_water_default;
    [System.NonSerialized] public PlayerStateWaterJumpController state_water_jump;
    [System.NonSerialized] public PlayerStateSlideController state_slide;
    [System.NonSerialized] public PlayerStateDiveController state_dive;
    [System.NonSerialized] public PlayerStateWaterDiveController state_water_dive;
    [System.NonSerialized] public PlayerStateAttackController state_attack;
    [System.NonSerialized] public PlayerStateDamageController state_damage;
    [System.NonSerialized] public PlayerStateRepelController state_repel;
    [System.NonSerialized] public PlayerStateCrouchController state_crouch;
    [System.NonSerialized] public PlayerStateCrouchJumpController state_crouch_jump;

    [System.NonSerialized] public int state_update_count = 0;

    // input variables.

    [System.NonSerialized] public Vector3 input_directional = Vector3.zero;
    [System.NonSerialized] public bool is_input_directional = false;
    [System.NonSerialized] public bool was_input_directional = false;

    [System.NonSerialized] public bool is_raised_positive = false;
    [System.NonSerialized] public bool is_raised_interact = false;
    [System.NonSerialized] public bool is_raised_positive_2 = false;

    // component variables.

    [System.NonSerialized] public GameMasterController master;
    [System.NonSerialized] public Rigidbody rigid_body;
    [System.NonSerialized] public SphereCollider player_sphere_collider;
    [System.NonSerialized] public Animator player_animator;
    [System.NonSerialized] public GameObject camera_object;
    [System.NonSerialized] public GameObject player_renderer_object;
    [System.NonSerialized] public Renderer player_renderer;
    [System.NonSerialized] public GameObject player_direction_object;
    [System.NonSerialized] public GameObject player_attack_forward_object;
    [System.NonSerialized] public Collider player_attack_forward_collider;
    [System.NonSerialized] public ActorDamageEffectController damageEffectController;

    // physical variables.

    [System.NonSerialized] public bool is_under_gravity = true;

    // grounded variables.

    float grounded_raycast_max_distance = 0f;
    float grounded_spherecast_max_distance = 0f;

    [System.NonSerialized] public RaycastHit raycast_hit_info;
    [System.NonSerialized] public bool is_raycast_hit = false;
    [System.NonSerialized] public bool is_raycast_grounded = false;

    [System.NonSerialized] public RaycastHit spherecast_hit_info;
    [System.NonSerialized] public bool is_spherecast_hit = false;
    [System.NonSerialized] public bool is_spherecast_grounded = false;
    [System.NonSerialized] public bool is_spherecast_grounded_since_state_begin = false;

    [System.NonSerialized] public float raycast_grounded_slope_angle = 0.0f;
    [System.NonSerialized] public Vector3 raycast_grounded_slope_normal = Vector3.up;
    [System.NonSerialized] public Vector3 raycast_grounded_slope_direction = Vector3.up;

    [System.NonSerialized] public float spherecast_grounded_slope_angle = 0.0f;
    [System.NonSerialized] public Vector3 spherecast_grounded_slope_normal = Vector3.up;

    [System.NonSerialized] public GameConstants.GroundType ground_type;

    // movement variables.

    [System.NonSerialized] public RaycastHit movement_hit;
    [System.NonSerialized] public RaycastHit step_movement_hit;

    [System.NonSerialized] public bool is_movement_hit = false;
    [System.NonSerialized] public bool is_step_movement_hit = false;

    // jump variables.

    [System.NonSerialized] public int jump_persist_energy = 0;

    // slide variables.

    [System.NonSerialized] public float slide_resistance = 1.0f;
    [System.NonSerialized] public float slide_force = 1.0f;
    [System.NonSerialized] public Vector3 slide_direction = Vector3.zero;

    [System.NonSerialized] public bool is_slide_hit = false;

    // dive variables.

    [System.NonSerialized] public Vector3 dive_direction = Vector3.zero;

    // damage variables.

    [System.NonSerialized] public GameObject damage_source = null;
    [System.NonSerialized] public AttributeDamageData damage_type = null;

    [System.NonSerialized] public bool isDamaged = false;
    [System.NonSerialized] public float damageTimer = 0.0f;
    [System.NonSerialized] public float damageInterval = DAMAGE_INTERVAL;

    // repel variables.

    [System.NonSerialized] public GameObject repel_source = null;
    [System.NonSerialized] public AttributeDamageData repel_type = null;

    // moving object variables.

    [System.NonSerialized] public List<GameObject> moving_object_collision_list = new List<GameObject>();
    bool is_colliding_moving_object = false;

    // water variables.

    [System.NonSerialized] public List<GameObject> water_object_collision_list = new List<GameObject>();
    [System.NonSerialized] public bool is_colliding_water_object = false;
    [System.NonSerialized] public bool is_partial_submerged = false;
    [System.NonSerialized] public bool is_full_submerged = false;
    [System.NonSerialized] public float water_y_level = 0;

    // animator variables.

    [System.NonSerialized] public Vector3 facing_direction = Vector3.zero;
    [System.NonSerialized] public Vector3 facing_direction_delta = Vector3.zero;

    public RuntimeAnimatorController animator_game;
    public RuntimeAnimatorController animator_game_over;
    public RuntimeAnimatorController animator_cutscene;

    // audio variables.

    [System.NonSerialized] public AudioSource audio_source;

    public AudioClip sfx_player_dive;
    public AudioClip sfx_player_hurt_default;
    public AudioClip sfx_player_jump;
    public AudioClip sfx_player_crouch_jump;
    public AudioClip sfx_player_slide;
    public AudioClip sfx_player_water_jump;

    // interface variables.

    ActorFootstepManager footstep_manager;
    ActorDataManager adm;

    private void Start()
    {
        master = GameMasterController.GetMasterController();

        // initialise state controllers.

        state_default = new PlayerStateDefaultController();
        state_jump = new PlayerStateJumpController();
        state_water_default = new PlayerStateWaterDefaultController();
        state_water_jump = new PlayerStateWaterJumpController();
        state_slide = new PlayerStateSlideController();
        state_dive = new PlayerStateDiveController();
        state_water_dive = new PlayerStateWaterDiveController();
        state_attack = new PlayerStateAttackController();
        state_damage = new PlayerStateDamageController();
        state_repel = new PlayerStateRepelController();
        state_crouch = new PlayerStateCrouchController();
        state_crouch_jump = new PlayerStateCrouchJumpController();

        player_state_controllers = new Dictionary<PlayerState, IPlayerStateController>();

        player_state_controllers.Add(PlayerState.player_default, state_default);
        player_state_controllers.Add(PlayerState.player_jump, state_jump);
        player_state_controllers.Add(PlayerState.player_water_default, state_water_default);
        player_state_controllers.Add(PlayerState.player_water_jump, state_water_jump);
        player_state_controllers.Add(PlayerState.player_slide, state_slide);
        player_state_controllers.Add(PlayerState.player_dive, state_dive);
        player_state_controllers.Add(PlayerState.player_water_dive, state_water_dive);
        player_state_controllers.Add(PlayerState.player_attack, state_attack);
        player_state_controllers.Add(PlayerState.player_damage, state_damage);
        player_state_controllers.Add(PlayerState.player_repel, state_repel);
        player_state_controllers.Add(PlayerState.player_crouch, state_crouch);
        player_state_controllers.Add(PlayerState.player_crouch_jump, state_crouch_jump);

        // initialise componenets.

        rigid_body = this.GetComponent<Rigidbody>();
        player_sphere_collider = GameObject.Find
            (MAIN_COLLIDER_GAME_OBJECT_NAME).GetComponent<SphereCollider>();
        player_animator = this.GetComponent<Animator>();
        camera_object = GameObject.FindGameObjectWithTag(GameConstants.TAG_MAIN_CAMERA);
        player_renderer_object = GameObject.Find(PLAYER_RENDER_GAME_OBJECT_NAME);
        player_renderer = this.GetComponentInChildren<SkinnedMeshRenderer>();
        player_direction_object = GameObject.Find(PLAYER_DIRECTION_GAME_OBJECT_NAME);

        player_attack_forward_object = GameObject.Find(PLAYER_ATTACK_FORWARD_OBJECT_NAME);
        player_attack_forward_collider = player_attack_forward_object.GetComponent<Collider>();
        player_attack_forward_collider.enabled = false;

        damageEffectController = this.gameObject.GetComponentInChildren<ActorDamageEffectController>();

        // add listeners.

        master.GameStateChange += ChangeGameState;

        // add components.

        audio_source = this.gameObject.AddComponent<AudioSource>();

        // initialise actor attributes.

        damage_type = new AttributeDamageData();
        repel_type = new AttributeDamageData();

        // initialise interface.

        footstep_manager = new ActorFootstepManager();

        adm = new ActorDataManager();
        adm = ActorDataManager.GetDefault();

        // setup.

        InitialisePhysicalParameters();
    }

    void OnDestroy()
    {
        // remove listeners.

        master.GameStateChange -= ChangeGameState;
    }

    private void InitialisePhysicalParameters()
    {
        // rigid body inits.

        rigid_body.mass = RIGID_BODY_MASS;
        rigid_body.drag = RIGID_BODY_DRAG;
        rigid_body.angularDrag = RIGID_BODY_ANGULAR_DRAG;

        rigid_body.constraints = RigidbodyConstraints.FreezeRotation;

        // collider inits.

        player_sphere_collider.material.bounciness = 0.0f;
        player_sphere_collider.material.bounceCombine = PhysicMaterialCombine.Minimum;

        grounded_raycast_max_distance = player_sphere_collider.radius + GROUNDED_RAYCAST_ADDITIONAL_DISTANCE;
        grounded_spherecast_max_distance = GROUNDED_SPHERECAST_ADDITIONAL_DISTANCE;
    }

    private void Update()
    {
        if (master.gameState == GameState.Game 
            || master.gameState == GameState.GameCutscene)
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
        if (master.gameState == GameState.Game 
            || master.gameState == GameState.GameCutscene)
        {
            // run update if in game state.

            UpdateWaterStatus();
            UpdateMovingObjectStatus();

            UpdateGroundedRay();
            UpdateGroundedSphere();
            UpdateGravity();
            player_state_controllers[player_state].UpdateStateDragAndFriction(this);

            // update the player state.

            state_update_count++;
            player_state_controllers[player_state].CheckState(this);

            // do state specific actions.

            player_state_controllers[player_state].UpdateState(this);
            player_state_controllers[player_state].UpdateStateSlide(this);
            player_state_controllers[player_state].UpdateStateSpeed(this);

            // update animator.

            UpdateAnimatorVariables();
            player_state_controllers[player_state].UpdateStateAnimator(this);

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
            player_animator.enabled = false;
        }
    }

    private void UpdatePlayerInput()
    {

        // ignore all input for a short
        // time when entering game state
        // from a non-game state.

        if (master.gameStatePrevious != GameState.Game
            && master.gameStatePrevious != GameState.GameCutscene
            && master.GameStateTime <= INPUT_GAME_STATE_DELAY)
            return;

        // get previous inputs.

        was_input_directional = is_input_directional;

        // get input from input mapper.

        float input_horizontal = master.input_controller.actionHorizontal.ReadValue<float>();
        float input_vertical = master.input_controller.actionVertical.ReadValue<float>();

        Vector3 input = new Vector3(input_horizontal, 0.0f, input_vertical);

        if (input.magnitude > 1)
            input = input.normalized;

        input_directional = input;
        is_input_directional = input_directional.magnitude > INPUT_DIRECTIONAL_THRESHOLD;
        
        // raise events for fixed update
        // edge detection.

        if (!master.input_controller.wasInputPositive && master.input_controller.isInputPositive)
            is_raised_positive = true;

        if (!master.input_controller.wasInputInteract && master.input_controller.isInputInteract)
            is_raised_interact = true;

        if (!master.input_controller.wasInputPositive2 
            && master.input_controller.isInputPositive2)
            is_raised_positive_2 = true;
    }

    private void UpdateWaterStatus()
    {
        if (!is_colliding_water_object)
            return;

        is_partial_submerged = (this.transform.position + WATER_PARTIAL_SUBMERGED_OFFSET).y <= water_y_level;
        is_full_submerged = (this.transform.position + WATER_FULL_SUBMERGED_OFFSET).y <= water_y_level;
    }

    private void UpdateMovingObjectStatus()
    {
        if (!is_colliding_moving_object)
            return;
    }

    private void UpdateGroundedRay()
    {
        // check grounding by ray.

        is_raycast_hit = Physics.Raycast(this.transform.position, Vector3.down, out raycast_hit_info, GROUNDED_RAYCAST_DISTANCE);

        if (is_raycast_hit)
        {
            // set the angle of the surface.
            raycast_grounded_slope_angle = Vector3.Angle(raycast_hit_info.normal, Vector3.up);

            // set the slope normal.
            raycast_grounded_slope_normal = raycast_hit_info.normal;

            // set the ground slope direction.
            var temp = Vector3.Cross(raycast_hit_info.normal, Vector3.down);
            raycast_grounded_slope_direction = Vector3.Cross(temp, raycast_hit_info.normal);

            // set grounded, if under max distance.
            is_raycast_grounded = raycast_hit_info.distance <= grounded_raycast_max_distance 
                && raycast_grounded_slope_angle <= SLIDE_ANGLE_MAX;

            // if grounded, and in the regular state, set the position above the floor.

            if (is_raycast_grounded && player_state == PlayerState.player_default)
            {
                rigid_body.MovePosition(new Vector3(
                    rigid_body.position.x,
                    raycast_hit_info.point.y + raycast_hit_info.distance,
                    rigid_body.position.z));
                Debug.DrawRay(transform.position, Vector3.down, Color.magenta);
            }
            
        }
    }

    private void UpdateGroundedSphere()
    {
        // check grounding by sphere.

        is_spherecast_hit = Physics.SphereCast(transform.position, GROUNDED_SPHERECAST_RADIUS, Vector3.down, out spherecast_hit_info, GROUNDED_RAYCAST_DISTANCE);

        if (is_spherecast_hit)
        {
            if (!is_spherecast_grounded_since_state_begin)
                is_spherecast_grounded_since_state_begin = is_spherecast_grounded;

            // if also ray grounded
            // set the slope normal.
            spherecast_grounded_slope_normal = (is_raycast_grounded)
                ? spherecast_hit_info.normal
                : raycast_hit_info.normal;

            // if also ray grounded
            // set the angle of the surface.
            spherecast_grounded_slope_angle = (is_raycast_grounded)
                ? Vector3.Angle(raycast_hit_info.normal, Vector3.up)
                : raycast_grounded_slope_angle;

            is_spherecast_grounded = spherecast_hit_info.distance <= grounded_spherecast_max_distance
                && spherecast_grounded_slope_angle <= SLIDE_ANGLE_MAX;

            if (is_spherecast_grounded)
            {
                if (spherecast_hit_info.collider.gameObject
                    .GetComponent<MapAttributeGroundType>() == null)
                    ground_type = GameConstants.GroundType.ground_default;
                else
                    ground_type = spherecast_hit_info.collider.gameObject
                        .GetComponent<MapAttributeGroundType>().ground_type;
            }
        }
    }

    private void UpdateGravity()
    {
        // apply rising or falling gravity.

        if (!is_under_gravity)
            return;

        if (rigid_body.velocity.y > 0)
            rigid_body.AddForce(Physics.gravity, ForceMode.Acceleration);
        else
            rigid_body.AddForce(Physics.gravity * GRAVITY_MULTIPLIER, ForceMode.Acceleration);
    }

    private void UpdateAnimatorVariables()
    {
        player_animator.SetInteger("anim_game_state", (int)master.gameState);
        player_animator.SetInteger("anim_player_state", (int)player_state);
        player_animator.SetInteger("anim_player_state_update_count", state_update_count);
        player_animator.SetBool("anim_is_grounded", is_spherecast_grounded);
        player_animator.SetBool("anim_is_moving", rigid_body.velocity.magnitude > 0.2f);
        player_animator.SetFloat("anim_horizontal_speed", is_input_directional ? rigid_body.velocity.magnitude : 0.0f);
        player_animator.SetFloat("anim_vertical_speed", rigid_body.velocity.y);
        player_animator.SetFloat("anim_speed_water_dive", Mathf.Clamp(rigid_body.velocity.magnitude,0.25f,1.0f));
        player_animator.SetBool("anim_is_input_right", input_directional.x > 0.5);
        player_animator.SetBool("anim_is_input_left", input_directional.x < -0.5);
    }

    private void UpdateAnimatorGameOver()
    {

    }

    private void UpdateAnimatorCutscene()
    {

        if (master.cutscene_controller.currentEventSource == null)
            return;

        facing_direction_delta = Vector3.RotateTowards(player_renderer_object.transform.forward,
            master.cutscene_controller.currentEventSource.transform.position - player_renderer_object.transform.position,
            PlayerConstants.ANIMATION_TURNING_SPEED_MULTIPLIER,
            0.0f);

        facing_direction_delta.y = 0.0f;

        // Move our position a step closer to the target.
        player_renderer_object.transform.rotation = Quaternion.LookRotation(facing_direction_delta);
    }

    // clear raised inputs.

    private void UpdateClearRaisedInputs()
    {
        is_raised_positive = false;
        is_raised_interact = false;
        is_raised_positive_2 = false;
    }

    // state change.

    private void ChangeGameState(object sender, EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;

        if (args.gameState == GameState.Game || master.gameState == GameState.GameCutscene)
            player_animator.runtimeAnimatorController = animator_game;
        else if (args.gameState == GameState.GameOver)
            player_animator.runtimeAnimatorController = animator_game_over;
        else if (args.gameState == GameState.Cutscene)
            player_animator.runtimeAnimatorController = animator_cutscene;
        else
            player_animator.runtimeAnimatorController = animator_game;

        // store, or restore, velocity when changing state.

        if (args.gameState == GameState.Game
            || args.gameState == GameState.GameCutscene)
        {
            if(args.game_state_previous != GameState.Game 
                && args.game_state_previous != GameState.GameCutscene)
                ResumeController(e);
        }
        else
        {
            if (args.game_state_previous == GameState.Game
                || args.game_state_previous == GameState.GameCutscene)
                PauseController(e);
        }

        // if not in a game state, clear the directional input.

        if (args.gameState != GameState.Game && master.gameState != GameState.GameCutscene)
            input_directional = Vector3.zero;
    }

    private void PauseController(EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;

        stored_velocity = rigid_body.velocity;

        rigid_body.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        rigid_body.isKinematic = true;
        UpdateAnimatorVariables();
    }

    private void ResumeController(EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;

        rigid_body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigid_body.isKinematic = false;
        player_animator.enabled = true;

        // restore velocity if coming from the pause meny.

        if(args.game_state_previous == GameState.Menu)
            rigid_body.velocity = stored_velocity;
    }

    public void ChangePlayerState(PlayerState new_state)
    {
        player_state_controllers[player_state].FinishState(this);

        player_state_previous = player_state;
        player_state = new_state;

        player_state_controllers[player_state].BeginState(this);

        // set general state variables.

        state_update_count = 0;
        is_spherecast_grounded_since_state_begin = false;
    }

    public void SimpleRepel(GameObject repelSource, float repelForceMultiplier)
    {
        rigid_body.velocity = Vector3.zero;
        var repelVector = (this.transform.position - repelSource.transform.position).normalized;
        rigid_body.AddForce(repelVector * repelForceMultiplier, ForceMode.VelocityChange);
    }


    // collision.

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == GameConstants.TAG_MOVING_OBJECT)
        {
            moving_object_collision_list.Add(collision.gameObject);

            is_colliding_moving_object = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (moving_object_collision_list.Contains(collision.gameObject))
        {
            moving_object_collision_list.Remove(collision.gameObject);

            if (moving_object_collision_list.Count == 0)
            {
                is_colliding_moving_object = false;
            }
        }
    }

    // trigger.

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GameConstants.TAG_WATER)
        {
            water_object_collision_list.Add(other.gameObject);

            is_colliding_water_object = true;
            water_y_level = other.bounds.center.y + (other.bounds.size.y / 2);
            
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
        if (water_object_collision_list.Contains(other.gameObject))
        {
            water_object_collision_list.Remove(other.gameObject);

            if (water_object_collision_list.Count == 0)
            {
                is_colliding_water_object = false;
                water_y_level = 0.0f;
                is_full_submerged = false;
                is_partial_submerged = false;
            }
        }
    }

    // set and unset.

    public void SetDamaged(AttributeDamageData damageData)
    {
        isDamaged = true;

        damageTimer = 0.0f;
        master.player_controller.player_health -= damageData.damageAmount;
        ChangePlayerState(PlayerState.player_damage);

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
        adm.is_in_water = is_colliding_water_object;
        adm.is_submerged = is_full_submerged;
        adm.water_y_level = water_y_level;
    }

    public ActorFootstepManager UpdateFootstepController()
    {
        if (master.gameState != GameState.Game
            && master.gameState != GameState.GameCutscene)
            return null;

        if(player_state == PlayerState.player_water_dive)
        {
            footstep_manager.is_grounded = true;
            footstep_manager.ground_type = ground_type;
            footstep_manager.velocity = rigid_body.velocity.magnitude;
            footstep_manager.is_in_water = true;
            footstep_manager.is_submerged = true;
            return footstep_manager;
        }

        footstep_manager.is_grounded = is_spherecast_grounded;
        footstep_manager.ground_type = ground_type;
        footstep_manager.velocity = rigid_body.velocity.magnitude;
        footstep_manager.is_in_water = is_colliding_water_object;
        footstep_manager.is_submerged = is_full_submerged;
        return footstep_manager;
    }

    public ActorDataManager? UpdateActorController()
    {
        return adm;
    }

    private void OnGUI()
    {
        //GUI.color = Color.black;
        //GUI.Label(new Rect(64, Screen.height - 600, 600, 600),
        //    "player_state: " + player_state
        //    + "\nupdate count: " + state_update_count
        //    + "\npos " + rigid_body.position.x.ToString("0.00")
        //    + "|" + rigid_body.position.y.ToString("0.00")
        //    + "|" + rigid_body.position.z.ToString("0.00")
        //    + "\nvel " + rigid_body.velocity.x.ToString("0.00")
        //    + "|" + rigid_body.velocity.y.ToString("0.00")
        //    + "|" + rigid_body.velocity.z.ToString("0.00")
        //    + "\nmovement hit: " + is_movement_hit
        //    + "\nray distance: " + raycast_hit_info.distance
        //    + "\nray grounded: " + is_raycast_grounded
        //    + "\nray angle   : " + raycast_grounded_slope_angle
        //    + "\nsphere distance            : " + spherecast_hit_info.distance
        //    + "\nsphere grounded            : " + is_spherecast_grounded
        //    + "\nsphere angle               : " + spherecast_grounded_slope_angle
        //    + "\nmoving object collisions   : " + moving_object_collision_list.Count
        //    + "\ncolliding moving object    : " + is_colliding_moving_object
        //    + "\nwater trigger collisions   : " + water_object_collision_list.Count
        //    + "\ncolliding water trigger    : " + is_colliding_water_object
        //    + "\nwater y level              : " + water_y_level
        //    + "\nis partial submerged       : " + is_partial_submerged
        //    + "\nis full submerged          : " + is_full_submerged
        //    + "\nslide resistance           : " + slide_resistance);
    }

    
}
