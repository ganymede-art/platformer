using Assets.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script
{
    public class PlayerConstants
    {
        // debug constants.

        public static readonly Rect DEBUG_RECTANGLE_1 = new Rect(0, 0, 1000, 1000);
        public static readonly Rect DEBUG_RECTANGLE_2 = new Rect(2, 2, 1000, 1000);

        // input constants.

        public const float INPUT_GAME_STATE_DELAY = 0.09F;
        public const float INPUT_DIRECTIONAL_THRESHOLD = 0.01F;
        public const float INPUT_BUTTON_THRESHOLD = 0.5F;

        // component constants.

        public const string COLLIDER_OBJECT = "player_collider";
        public const string RENDERER_OBJECT = "player_render";
        public const string DIRECTION_OBJECT = "player_direction";
        public const string EYES_OBJECT = "player_eyes";
        public const string ATTACK_FORWARD_1_OBJECT = "attack_forward_1";
        public const string ATTACK_FORWARD_2_OBJECT = "attack_forward_2";
        public const string ATTACK_DOWN_1_OBJECT = "attack_down_1";

        // physical constants.

        public const float RIGID_BODY_MASS = 1F;
        public const float RIGID_BODY_DRAG = 1F;
        public const float RIGID_BODY_ANGULAR_DRAG = 0.05F;

        public const float GRAVITY_MULTIPLIER = 2.0F;

        public const float DRAG_GROUNDED = 5F;
        public const float DRAG_AIR = 1F;

        public const float ACCELERATION_GROUNDED = 0.5F;
        public const float ACCELERATION_AIR = 0.2F;
        public const float ACCELERATION_CROUCH_JUMP = 0.05F;

        public const float MAX_SPEED_GROUNDED = 3.0F;
        public const float MAX_SPEED_WATER = 2.0F;
        public const float MAX_SPEED_WATER_DIVE = 5.0F;
        public const float MAX_SPEED_WATER_SINK = 1.25F;
        public const float MAX_SPEED_DIVE = 7.0F;

        // grounded constants.

        public const float GROUNDED_SPHERECAST_RADIUS = 0.1625F;

        public const float GROUNDED_RAYCAST_DISTANCE = 100F;
        public const float GROUNDED_SPHERECAST_DISTANCE = 100F;

        public const float MAX_GROUNDED_RAYCAST_DISTANCE = 0.2875F;
        public const float MAX_GROUNDED_SPHERECAST_DISTANCE = 0.05F;

        public static float GROUNDED_CHECKSPHERE_RADIUS = 0.1625F;      // 
        public static readonly Vector3 GROUNDED_CHECKSPHERE_OFFSET      // 
            = new Vector3(0.0F, -0.1F, 0.0F);                           //
        public static readonly Vector3 NEAR_GROUNDED_CHECKSPHERE_OFFSET // 
            = new Vector3(0.0F, -0.2F, 0.0F);                           //

        public const float MAX_GROUNDED_ANGLE = 49.9F;

        // movement constants.

        public const float STEP_MAX_VELOCITY = 1F;

        public const float MOVEMENT_SPHERECAST_DISTANCE = 0.1F;
        public const float MOVEMENT_SPHERECAST_RADIUS = 0.1625F;

        // jump constants.

        public const float JUMP_FORCE_MULTIPLIER = 3.0F;
        public const float JUMP_PERSIST_FORCE_MULTIPLIER = 0.4F;
        public const int JUMP_PERSIST_ENERGY_MAX = 10;

        public const float FORCE_MULTIPLIER_HIGH_JUMP = 5.0F;
        public const float FORCE_MULTIPLIER_HIGH_JUMP_PERSIST = 0.4F;

        public const float WATER_JUMP_FORCE_MULTIPLIER = 2.5F;
        public const float MINIMUM_WATER_JUMP_Y_SPEED = -1F;

        // dive constants.

        public const float DIVE_MIN_INPUT_DIRECTIONAL_MAGNITUDE = 1.0F;

        // animation constants.

        public const string TRIGGER_SPEED_MULTIPLIER = "speed_multiplier";
        public const string TRIGGER_ATTACK = "attack";
        public const string TRIGGER_HIGH_JUMP_UP = "high_jump_up";
        public const string TRIGGER_HURT_UP = "hurt_up";
        public const string TRIGGER_HURT_DOWN = "hurt_down";
        public const string TRIGGER_CROUCH = "crouch";
        public const string TRIGGER_MOVE = "move";
        public const string TRIGGER_IDLE = "idle";
        public const string TRIGGER_JUMP_UP = "jump_up";
        public const string TRIGGER_JUMP_DOWN = "jump_down";
        public const string TRIGGER_EMOTE_DIE = "emote_die";
        public const string TRIGGER_DIVE_BEGIN = "dive_begin";
        public const string TRIGGER_FLUTTER_UP = "flutter_up";
        public const string TRIGGER_SWIM_BEGIN = "swim_begin";
        public const string TRIGGER_SWIM = "swim";
        public const string TRIGGER_WATER_JUMP_UP = "water_jump_up";
        public const string TRIGGER_WATER_JUMP_DOWN = "water_jump_down";
        public const string TRIGGER_SLAM_UP = "slam_up";
        public const string TRIGGER_SLAM_DOWN = "slam_down";


        public const float ANIMATION_TURNING_SPEED_MULTIPLIER = 0.3F;
        public const float ANIMATION_TURNING_SPEED_WATER_DIVE_MULTIPLIER = 0.1F;

        // fixed update count constants.

        public const int UPDATE_COUNT_JUMP_RECOVERY_MIN = 10;
        public const int UPDATE_COUNT_DIVE_RECOVERY_MIN = 30;
        public const int UPDATE_COUNT_WATER_DIVE_RESTART_MIN = 10;
        public const int UPDATE_COUNT_WATER_DIVE_RECOVERY_MIN = 30;
    }
}
