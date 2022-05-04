using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using static Assets.Script.GameConstants;
using static Assets.Script.PlayerConstants;

public class PlayerStateDie : MonoBehaviour, IPlayerState
{
    public void BeginState(PlayerController mc, params object[] parameters)
    {
        mc.playerAnimator.ResetAllAnimatorTriggers();
        mc.playerAnimator.SetTrigger(TRIGGER_EMOTE_DIE);

        // play attack sound.

        mc.audioSource.clip = mc.jumpSound;
        mc.audioSource.Play();

        // zero out vertical velocity and add diving force.

        mc.rigidBody.velocity = new Vector3
            (mc.rigidBody.velocity.x, 0, mc.rigidBody.velocity.z);
        mc.rigidBody.velocity = new Vector3
            (0, 0, 0);
    }

    public void CheckState(PlayerController mc)
    {

    }

    public void FinishState(PlayerController mc)
    {

    }

    public void FixedUpdateState(PlayerController mc)
    {
        PlayerStaticMethods.LimitSpeedTwoAxis(mc, MAX_SPEED_GROUNDED);
    }

    public void UpdateState(PlayerController mc)
    {

    }

    public string GetStateType()
    {
        return GameConstants.PLAYER_STATE_DIE;
    }
}
