using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerConstants
{
    // Behaviour Constants.
    public static readonly Vector3 GROUND_CHECK_CHECK_SPHERE_OFFSET = new Vector3(0.0F, -0.05F, 0.0F);

    public static readonly Vector3 WATER_PARTIAL_SUBMERGED_OFFSET = new Vector3(0, 0.0F, 0);
    public static readonly Vector3 WATER_FULL_SUBMERGED_OFFSET = new Vector3(0, 0.1625f, 0);
    public static readonly Vector3 WATER_HEIGHT_RAY_OFFSET = new Vector3(0.0F, 50.0F, 0.0F);
    public const float WATER_HEIGHT_RAY_DISTANCE = 100.0F;

    public const float GROUND_CHECK_CHECK_SPHERE_RADIUS = 0.175F;
    public const float GROUND_CHECK_SPHERECAST_RADIUS = 0.175F;
    public const float GROUND_CHECK_SPHERECAST_DISTANCE = 100.0F;

    public const float DAMAGE_INTERVAL = 2.0F;

    // Physical constants.
    public const float MOVEMENT_SPHERECAST_RADIUS = 0.175F;
    public const float MOVEMENT_SPHERECAST_DISTANCE = 0.05F;

    public const float GROUND_CHECK_MAX_GROUNDED_ANGLE = 50.0F;

    public const float ACCELERATION_GROUNDED = 0.25F;
    public const float ACCELERATION_AIR = 0.15F;

    public const float GRAVITY_MULT = 2.0F;

    public const float DEFAULT_MAX_SPEED = 3.0F;
    public const float WATER_MAX_SPEED = 2.0F;
    public const float HURT_MAX_SPEED = 5.0F;
    public const float LUNGE_MAX_SPEED = 7.0F;
    public const float DIE_MAX_SPEED = 5.0F;

    public const float STATIC_FRICTION = 1.0F;
    public const float DYNAMIC_FRICTION = 0.2F;

    // Animation constants.
    public const float ANIMATION_TURNING_SPEED_MULT = 20.0F;

    // Sound constants.
    public const float SFX_MIN_PT = 0.8F;
    public const float SFX_MAX_PT = 1.2F;

    // Projectile constants.
    public const float PROJECTILE_FORCE_MULT = 10.0F;
    public const float PROJECTILE_MAX_INTERVAL = 3.0F;

    // State args.
    public const string STATE_ARG_HITBOX_OBJECT = "damage_object";
    public const string STATE_ARG_HITBOX_DATA = "damage_data";

    // State specific constants.
    public const float DIE_MAX_INTERVAL = 4.0F;
    public const float DIE_UP_FORCE_MULT = 1.5F;

    public const float ATTACK_MAX_INTERVAL = 0.3F;
    public const float ATTACK_UP_FORCE_MULT = 1.5F;
    public const float ATTACK_FORE_FORCE_MULT = 2.5F;

    public const float ATTACK_RECOIL_INTERVAL = 0.2F;
    public const float ATTACK_RECOIL_UP_FORCE_MULT = 2.5F;
    public const float ATTACK_RECOIL_REAR_FORCE_MULT = 3.5F;

    public const float ATTACK_UNDERWATER_INTERVAL = 0.4F;
    public const float ATTACK_UNDERWATER_FORE_FORCE_MULT = 2.5F;
    public const float ATTACK_UNDERWATER_MAX_VEL = 5.0F;

    public const float JUMP_MIN_INTERVAL = 0.2F;
    public const float JUMP_MAX_INTERVAL = 1.0F;
    public const float JUMP_PERSIST_MAX_INTERVAL = 0.15F;
    public const float JUMP_FORCE_MULT = 3.0F;
    public const float JUMP_PERSIST_FORCE_MULT = 0.375F;

    public const float LUNGE_MIN_INTERVAL = 0.5F;
    public const float LUNGE_MAX_INTERVAL = 3.0F;
    public const float LUNGE_MIN_VEL = 0.5F;
    public const float LUNGE_UP_FORCE_MULT = 3.0F;
    public const float LUNGE_FORE_FORCE_MULT = 7.0F;

    public const float HIGH_JUMP_MIN_INTERVAL = 0.2F;
    public const float HIGH_JUMP_MAX_INTERVAL = 2.0F;
    public const float HIGH_JUMP_FORCE_MULT = 6.0F;

    public const float SLAM_UP_MIN_INTERVAL = 0.1F;
    public const float SLAM_DOWN_MAX_INTERVAL = 2.0F;
    public const float SLAM_UP_FORCE_MULT = 3.0F;
    public const float SLAM_DOWN_FORCE_MULT = 3.0F;
    public const float SLAM_BOUNCE_UP_FORCE = 3.0F;

    public const float HURT_MIN_INTERVAL = 1.0F;
    public const float HURT_MAX_INTERVAL = 3.0F;
    public const float HURT_UP_FORCE_MULT = 3.0F;
    public const float HURT_AWAY_FORCE_MULT = 7.0F;
    public const float HURT_FALL_TRIGGER_MIN_VELOCITY = 0.1F;
    public const float HURT_FALL_TRIGGER_MIN_INTERVAL = 0.3F;

    public const float WD_SURFACE_CLAMP_HEIGHT_OFFSET = 0.05F;
    public const float WD_WEAK_BUOYANT_FORCE = 0.375F;
    public const float WD_MIN_STRONG_BUOYANT_FORCE = 0.0F;
    public const float WD_MIN_SUBMERGED_BUOYANT_FORCE = 1.0F;
    public const float WD_MAX_STRONG_BUOYANT_FORCE = 5.0F;
    public const float WD_SINK_VEL_LIMIT = -1.25F;
    public const float WD_RISE_VEL_LIMIT = 50.0F;

    public const float DIVE_UNDERWATER_FORCE_MULT = 2.25F;
    public const float DIVE_UNDERWATER_MIN_INTERVAL = 0.25F;
    public const float DIVE_UNDERWATER_MAX_INTERVAL = 0.5F;

    public const float USE_KEY_ITEM_INTERVAL = 1.0F;
    public const float USE_KEY_ITEM_CARD_INTERVAL = 0.5F;
}
