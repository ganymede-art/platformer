using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public class SlamPlayerState : MonoBehaviour, IState<Player, PlayerStateId>
{
    // Consts.
    const int PHASE_SLAM_UP = 0;
    const int PHASE_SLAM_DOWN = 1;

    // Private fields.
    private IRemoteTrigger remoteTrigger;
    private int phase;
    private bool hasSlamColliderHit = false;

    // Public properties.
    public PlayerStateId StateId => PlayerStateId.Slam;

    // Public fields.
    public GameObject remoteTriggerObject;

    private void Awake()
    {
        remoteTrigger = remoteTriggerObject.GetComponent<IRemoteTrigger>();
        remoteTrigger.RemoteTriggerEntered += OnRemoteTriggerEnter;
    }

    public void BeginState(Player c, Dictionary<string, object> args = null)
    {
        phase = PHASE_SLAM_UP;
        hasSlamColliderHit = false;

        c.playerAnimator.ResetAllAnimatorTriggers();
        c.playerAnimator.SetTrigger(ANIMATION_TRIGGER_SLAM_UP);

        c.playerRigidBody.velocity = new Vector3
                (c.playerRigidBody.velocity.x, 0, c.playerRigidBody.velocity.z);
        c.playerRigidBody.AddForce(Vector3.up * SLAM_UP_FORCE_MULT, ForceMode.VelocityChange);

        c.jumpSound.PlayPitchedOneShot
            (c.jumpSound.clip
            , SettingsHighLogic.G.PlayerVolume
            , SFX_MIN_PT
            , SFX_MAX_PT);

        c.slamHitbox.gameObject.SetActive(true);
    }

    public void FixedUpdateState(Player c)
    {
        var direction = PlayerStatics.GetFlatDirectionForMovement(c);
        var force = PlayerStatics.GetForceForMovement(c, direction);
        PlayerStatics.FixedUpdateDynamicFriction(c, DYNAMIC_FRICTION, STATIC_FRICTION);
        PlayerStatics.FixedUpdateMovement(c, direction, force);
        PlayerStatics.FixedUpdateLimitVelocityTwoAxis(c, DEFAULT_MAX_SPEED);
    }

    public void UpdateState(Player c)
    {
        // States.
        if (c.StateTimer > SLAM_UP_MIN_INTERVAL
            && phase == PHASE_SLAM_UP
            && c.GroundCheck.IsCheckSphereHit)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }

        if (c.GroundCheck.IsCheckSphereHit)
            hasSlamColliderHit = true;

        if (c.StateTimer > SLAM_UP_MIN_INTERVAL
            && phase == PHASE_SLAM_DOWN
            && hasSlamColliderHit)
        {
            c.playerRigidBody.velocity = new Vector3
                (c.playerRigidBody.velocity.x, 0, c.playerRigidBody.velocity.z);
            c.playerRigidBody.AddForce(Vector3.up * SLAM_BOUNCE_UP_FORCE, ForceMode.VelocityChange);

            c.slamEndSound.PlayPitchedOneShot
            (c.slamEndSound.clip
            , SettingsHighLogic.G.PlayerVolume
            , SFX_MIN_PT
            , SFX_MAX_PT);

            c.slamEndFx.Play();

            c.ChangeState(PlayerStateId.Default);
            return;
        }



        if (c.StateTimer > SLAM_DOWN_MAX_INTERVAL)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }

        // Phases.
        if (phase == PHASE_SLAM_UP 
            && c.StateTimer > SLAM_UP_MIN_INTERVAL
            && c.playerRigidBody.velocity.y < 0.0F)
        {
            phase = PHASE_SLAM_DOWN;

            c.playerAnimator.ResetAllAnimatorTriggers();
            c.playerAnimator.SetTrigger(ANIMATION_TRIGGER_SLAM_DOWN);

            c.playerRigidBody.velocity = new Vector3
                (c.playerRigidBody.velocity.x, 0, c.playerRigidBody.velocity.z);
            c.playerRigidBody.AddForce(Vector3.down * SLAM_DOWN_FORCE_MULT, ForceMode.VelocityChange);

            c.attackSound.PlayPitchedOneShot
            (c.attackSound.clip
            , SettingsHighLogic.G.PlayerVolume
            , SFX_MIN_PT
            , SFX_MAX_PT);
        }
    }

    public void EndState(Player c)
    {
        c.slamHitbox.gameObject.SetActive(false);
    }

    public void OnRemoteTriggerEnter(object sender, RemoteTriggerArgs args)
    {
        if (args.remoteTriggerObject.name != TRANSFORM_NAME_PLAYER_SLAM_HITBOX)
            return;
        if (args.other.isTrigger)
            return;

        hasSlamColliderHit = true;
    }
}
