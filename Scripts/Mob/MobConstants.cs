using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MobConstants
{
    public const float ANIMATION_TURNING_SPEED_MULT_SLOW = 5.0F;
    public const float ANIMATION_TURNING_SPEED_MULT_FAST = 20.0F;
    public const float ANIMATION_MAGNITUDE_SPEED_MULT = 0.5F;

    public const float MOB_SFX_MIN_PITCH = 0.8F;
    public const float MOB_SFX_MAX_PITCH = 1.2F;

    public const float WALL_CHECK_WALL_ANGLE = 50.0F;

    public const float CHASE_TURN_AROUND_ANGLE = 30.0F;

    public const float WANDER_TURN_AROUND_ANGLE = 30.0F;

    public const float HURT_MIN_RISING_INTERVAL = 0.1F;
    public const float HURT_MIN_RISING_VEL = 0.1F;
    public const float HURT_FRICTION = 0.3F;

    public const string STATE_ARG_HITBOX_OBJECT = "damage_object";
    public const string STATE_ARG_HITBOX_DATA = "damage_data";

    // Behaviours.
    public const float GROUND_CHECK_MAX_GROUNDED_ANGLE = 50.0F;
}
