using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    // Transform names.
    public const string TRANSFORM_NAME_CAMCORDER = "Camcorder";
    public const string TRANSFORM_NAME_PLAYER = "Player";
    public const string TRANSFORM_NAME_DEFAULT_LOAD_TRANSFORM = "FromDefault";
    public const string TRANSFORM_NAME_PLAYER_COLLIDER = "Player";
    public const string TRANSFORM_NAME_PLAYER_ATTACK_HITBOX = "AttackHitbox";
    public const string TRANSFORM_NAME_PLAYER_SLAM_HITBOX = "SlamHitbox";
    public const string TRANSFORM_NAME_PLAYER_LUNGE_HITBOX = "LungeHitbox";

    // Layers.
    public const int LAYER_WATER = 4;
    public const int LAYER_PLAYER = 6;
    public const int LAYER_PLAYER_ONLY = 7;
    public const int LAYER_MOB = 8;
    public const int LAYER_MOB_ONLY = 9;
    public const int LAYER_ITEM = 10;
    public const int LAYER_ITEM_ONLY = 11;

    public const int LAYER_HITBOX = 16;

    public const int SCENE_BOUNDARY_IGNORE_CAMERA = 24;
    public const int SCENE_BOUNDARY = 25;
    public const int SCENE_RIGID_IGNORE_CAMERA = 26;
    public const int SCENE_RIGID = 27;
    public const int SCENE_DYNAMIC_IGNORE_CAMERA = 28;
    public const int SCENE_DYNAMIC = 29;
    public const int SCENE_STATIC_IGNORE_CAMERA = 30;
    public const int SCENE_STATIC = 31;

    public const int LAYER_MASK_PLAYER_IGNORES =
        ~(
            (1 << LAYER_PLAYER) |
            (1 << LAYER_MOB) |
            (1 << LAYER_MOB_ONLY) |
            (1 << LAYER_ITEM) |
            (1 << LAYER_ITEM_ONLY)
        );

    public const int LAYER_MASK_PLAYER_CHECKSPHERE =
        ~(
            (1 << LAYER_PLAYER) |
            (1 << LAYER_MOB) |
            (1 << LAYER_MOB_ONLY) |
            (1 << LAYER_ITEM) |
            (1 << LAYER_ITEM_ONLY) |
            (1 << SCENE_BOUNDARY_IGNORE_CAMERA) |
            (1 << SCENE_BOUNDARY)
        );

    public const int LAYER_MASK_ALL_BUT_PLAYER = ~(1 << LAYER_PLAYER);

    public const int LAYER_MASK_CAMCORDER_IGNORES =
        ~(
            (1 << LAYER_PLAYER) |
            (1 << LAYER_PLAYER_ONLY) |
            (1 << LAYER_MOB) |
            (1 << LAYER_MOB_ONLY) |
            (1 << LAYER_ITEM) |
            (1 << LAYER_ITEM_ONLY) |
            (1 << SCENE_BOUNDARY_IGNORE_CAMERA) |
            (1 << SCENE_RIGID_IGNORE_CAMERA) |
            (1 << SCENE_DYNAMIC_IGNORE_CAMERA) |
            (1 << SCENE_STATIC_IGNORE_CAMERA) 
        );

    public const int LAYER_MASK_MOB_IGNORES =
        ~(
            (1 << LAYER_PLAYER) |
            (1 << LAYER_PLAYER_ONLY) |
            (1 << LAYER_ITEM) |
            (1 << LAYER_ITEM_ONLY)
        );

    public const int LAYER_MASK_MOB_CHECKSPHERE =
        ~(
            (1 << LAYER_PLAYER) |
            (1 << LAYER_MOB_ONLY) |
            (1 << LAYER_ITEM) |
            (1 << LAYER_ITEM_ONLY) |
            (1 << SCENE_BOUNDARY_IGNORE_CAMERA) |
            (1 << SCENE_BOUNDARY)
        );

    // Resource paths.
    public const string RESOURCE_PATH_GAME_HIGH_LOGIC_PREFAB = @"HighLogic/HighLogic";
    public const string RESOURCE_PATH_INPUT_ACTION_ASSET = @"Input/Input";

    public const string RESOURCE_PATH_PLAYER_PREFAB = @"Prefabs/Player";
    public const string RESOURCE_PATH_CAMCORDER_PREFAB = @"Prefabs/Camcorder";
    public const string RESOURCE_PATH_PLAY_USER_INTERFACE_PREFAB = @"Prefabs/PlayUserInterface";
    public const string RESOURCE_PATH_STAT_USER_INTERFACE_PREFAB = @"Prefabs/StatUserInterface";
    public const string RESOURCE_PATH_FILM_USER_INTERFACE_PREFAB = @"Prefabs/FilmUserInterface";
    public const string RESOURCE_PATH_LOAD_USER_INTERFACE_PREFAB = @"Prefabs/LoadUserInterface";
    public const string RESOURCE_PATH_MENU_USER_INTERFACE_PREFAB = @"Prefabs/MenuUserInterface";
    public const string RESOURCE_PATH_KEY_ITEM_SPRITES = @"KeyItemSprites";

    public const string RESOURCE_FOLDER_TEXTS = @"Texts";

    // Shader properties.
    public const string SHADER_PROPERTY_NAME_STATE_SPEED = "_StateSpeed";

    // High logic settings.
    public const string STARTUP_SCENE_NAME = @"MenuStartup1";
    public const string NEW_GAME_SCENE_NAME = @"Test1";

    public static readonly Vector3 LOADING_SCENE_STARTING_POSITION_OFFSET = new Vector3(0.0F, 0.1875F, 0.0F);
    public const float STARTUP_SCENE_INTERVAL = 2.0F;
    public const float LOADING_SCENE_INTERVAL = 0.25F;
    public const float INPUT_STATE_BLANK_INTERVAL = 0.01F;
    public const float INPUT_THRESHOLD_AXIS = 0.01F;
    public const string PERSISTENCE_TEMP_VARIABLE_PREFIX = "temp_";

    public const int PLAYER_DEFAULT_HEALTH = 5;
    public const int PLAYER_DEFAULT_MAX_HEALTH = 5;
    public const int PLAYER_DEFAULT_OXYGEN = 5;
    public const int PLAYER_DEFAULT_MAX_OXYGEN = 5;
    public const int PLAYER_DEFAULT_AMMO = 5;
    public const int PLAYER_DEFAULT_MAX_AMMO = 5;
    public const int PLAYER_DEFAULT_MONEY = 0;
    public const int PLAYER_DEFAULT_MAX_MONEY = 5;

    public const int TIME_INITIAL_DAY_OFFSET = 1;
    public const float TIME_INITIAL_HOUR_OFFSET = 4.0F;
    public const float TIME_HOUR_INCREMENT_ON_SCENE_CHANGE = 0.25F;

    public const float MUS_TGT_DYN_VOL_MIN = 0.2F;
    public const float MUS_TGT_DYN_VOL_MAX = 1.0F;

    public const float MIN_SFX_PITCH = 0.9F;
    public const float MAX_SFX_PITCH = 1.1F;

    // Text constants.
    public const string LOC_MISSING_KEY_ITEM_NAME = "KeyItemNoneName";
    public const string LOC_MISSING_KEY_ITEM_DESCRIPTION = "KeyItemNoneDescription";
    public const string LOC_MISSING_KEY_ITEM_ICON = "KeyItemNoneIcon";

    // Load scene args.
    public const string LOAD_NEW_SCENE_ARG_STARTING_OBJECT_NAME = "starting_object_name";

    // User interface.
    public const float WIDGET_ENABLE_INTERVAL = 0.25F;

    public const string WIDGET_ID_BLACK_OVERLAY = "black_overlay";
    public const string WIDGET_ID_MESSAGE_BOX = "message_box";
    public const string WIDGET_ID_CHOICES = "choices";
    public const string WIDGET_ID_MESSAGE_BOX_TRANSLATE_LERP = "message_box_translate_lerp";

    public const float UI_BEGIN_DISABLE_INTERVAL = 1.0F;

    // Widget args.
    public const string WIDGET_ARG_MESSAGE_BOX_TEXT = "message_box_text";
    public const string WIDGET_ARG_MESSAGE_BOX_VOX_SPRITE = "message_box_vox_image";
    public const string WIDGET_ARG_MESSAGE_BOX_IS_CONTINUE_PROMPT_ENABLED = "message_box_is_continue_prompt_enabled";

    public const string WIDGET_ARG_CHOICES_CHOICE_TEXT = "choices_choice_text";
    public const string WIDGET_ARG_CHOICES_CHOICES_TEXT = "choices_choices_text";

    // Animation event names.
    public const string ANIMATION_EVENT_NAME_STEP = "Step";

    // Animation triggers.
    public const string ANIMATION_TRIGGER_SPEED_MULTIPLIER = "Speed Multiplier";
    public const string ANIMATION_TRIGGER_IDLE = "Idle";
    public const string ANIMATION_TRIGGER_MOVE = "Move";
    public const string ANIMATION_TRIGGER_FLY = "Fly";
    public const string ANIMATION_TRIGGER_JUMP_UP = "Jump Up";
    public const string ANIMATION_TRIGGER_JUMP_DOWN = "Jump Down";
    public const string ANIMATION_TRIGGER_DOUBLE_JUMP_UP = "Double Jump Up";
    public const string ANIMATION_TRIGGER_DOUBLE_JUMP_UP_ALTERNATE = "Double Jump Up Alternate";
    public const string ANIMATION_TRIGGER_ATTACK = "Attack";
    public const string ANIMATION_TRIGGER_ATTACK_ALTERNATE = "Attack Alternate";
    public const string ANIMATION_TRIGGER_SWIM = "Swim";
    public const string ANIMATION_TRIGGER_DIVE_UNDERWATER_DOWN = "Dive Underwater Down";
    public const string ANIMATION_TRIGGER_ATTACK_UNDERWATER = "Attack Underwater";
    public const string ANIMATION_TRIGGER_HURT_UP = "Hurt Up";
    public const string ANIMATION_TRIGGER_HURT_DOWN = "Hurt Down";
    public const string ANIMATION_TRIGGER_HIGH_JUMP_UP = "High Jump Up";
    public const string ANIMATION_TRIGGER_LUNGE_UP = "Lunge Up";
    public const string ANIMATION_TRIGGER_LUNGE_DOWN = "Lunge Down";
    public const string ANIMATION_TRIGGER_SLAM_UP = "Slam Up";
    public const string ANIMATION_TRIGGER_SLAM_DOWN = "Slam Down";
    public const string ANIMATION_TRIGGER_CROUCH = "Crouch";
    public const string ANIMATION_TRIGGER_SIDLE = "Sidle";
    public const string ANIMATION_TRIGGER_CHARGE = "Charge";

    public const string ANIMATION_TRIGGER_EMOTE_REACT_KEY_ITEM = "Emote React Key Item";
    public const string ANIMATION_TRIGGER_EMOTE_REACT_ALERT = "Emote React Alert";
    public const string ANIMATION_TRIGGER_EMOTE_DIE = "Emote React Die";

    public const string ANIMATION_TRIGGER_EMOTE_DAZE = "Emote Daze";
    public const string ANIMATION_TRIGGER_EMOTE_DEAD = "Emote Dead";

    // Gizmos.
    public static readonly Color GIZMO_COLOUR = new Color(1.0F, 0.375F, 0.125F, 0.5F);

    // Camcorder state arg.
    public const string CAMCORDER_STATE_ARG_FIXED_POSITION_OBJECT = "fixed_position_object";
    public const string CAMCORDER_STATE_ARG_FIXED_TRANSITION_INTERVAL = "fixed_transition_interval";
}
