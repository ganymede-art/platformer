using Assets.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script
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
        public const float MAX_SPEED_SLIDE = 6.0F;
        public const float MAX_SPEED_DIVE = 7.0F;

        // grounded constants.

        public const float GROUNDED_RAYCAST_ADDITIONAL_DISTANCE = 0.1F;
        public const float GROUNDED_SPHERECAST_ADDITIONAL_DISTANCE = 0.025F;

        public const float GROUNDED_RAYCAST_DISTANCE = 100F;
        public const float GROUNDED_SPHERECAST_DISTANCE = 100F;

        public const float MAX_GROUNDED_RAYCAST_DISTANCE = 0.2875F;
        public const float MAX_GROUNDED_SPHERECAST_DISTANC = 0.025F;

        public const float MAX_NEAR_GROUNDED_RAYCAST_DISTANCE = 0.4875F;
        public const float MAX_NEAR_GROUNDED_SPHERECAST_DISTANCE = 0.225F;

        public const float GROUNDED_SPHERECAST_RADIUS = 0.187F;

        public static readonly AttributeGroundData DEFAULT_GROUND_DATA = AttributeGroundData.GetDefault();

        // movement constants.

        public static readonly Vector3 STEP_MOVEMENT_OFFSET = new Vector3(0, 0.15f, 0);
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

        // slide constants.

        public const float SLIDE_FORCE_ANGLE_MIN = 3F;                         // minimum angle to adjust slide direction to slope.
        public const float SLIDE_ANGLE_RECOVERY_MAX = 30F;                     // maximum angle to recover from slide.
        public const float SLIDE_SPEED_RECOVERY_MAX = 0.25F;                   // maximum speed to recover from slide.
        public const float SLIDE_ANGLE_MIN = 50F;                              // minimum angle to start sliding
        public const float SLIDE_ANGLE_MAX = 65F;                              // maximum angle to start sliding.
        public const float SLIDE_RESISTANCE_GROUND_ANGLE_MULTIPLIER = 0.001F;  // multiplier for ground angle to subtract from resistance.
        public const float SLIDE_RESISTANCE_MAX = 1.0F;                        // maximum slide resistance.
        public const float SLIDE_RESISTANCE_RECOVERY = 0.05F;                  // slide resistance recovery amount
        public const float SLIDE_FORCE_MULIPLIER = 1F;                         // multiplier to slide vector.
        public const float SLIDE_DIRECTION_ROTATION_MULTIPLIER = 0.5F;         // how fast the slide direction matches current slope.

        // dive constants.

        public const float DIVE_MIN_INPUT_DIRECTIONAL_MAGNITUDE = 1.0F;    

        // animation constants.

        public const float ANIMATION_TURNING_SPEED_MULTIPLIER = 0.3F;
        public const float ANIMATION_TURNING_SPEED_WATER_DIVE_MULTIPLIER = 0.1F;

        // fixed update count constants.

        public const int UPDATE_COUNT_JUMP_RECOVERY_MIN = 10;
        public const int UPDATE_COUNT_DIVE_RECOVERY_MIN = 30;
        public const int UPDATE_COUNT_WATER_DIVE_RESTART_MIN = 10;
        public const int UPDATE_COUNT_WATER_DIVE_RECOVERY_MIN = 30;
    }
}
