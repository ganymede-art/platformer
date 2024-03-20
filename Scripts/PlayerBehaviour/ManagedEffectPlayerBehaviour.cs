using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public class ManagedEffectPlayerBehaviour : MonoBehaviour, IBehaviour<Player, PlayerBehaviourId>
{
    // Consts.
    private float WATER_MOVE_EFFECT_MIN_SPEED = 0.125F;
    private float WATER_MOVE_EFFECT_MAX_SPEED = 3.0F;
    private float WATER_MOVE_EFFECT_MIN_GRAVITY = 0.125F;
    private float WATER_MOVE_EFFECT_MAX_GRAVITY = 3.0F;

    // Private fields.
    private bool wasWaterMoveEffectActive;
    private bool isWaterMoveEffectActive;
    private Vector3 waterMoveEffectPosition;
    private ParticleSystem.MainModule waterMoveEffectMain;

    // Public properties.
    public PlayerBehaviourId BehaviourId => PlayerBehaviourId.ManagedEffect;

    public void BeginBehaviour(Player c, Dictionary<string, object> args = null) 
    {
        waterMoveEffectPosition = Vector3.zero;
        waterMoveEffectMain = c.waterMoveFx.main;
    }

    public void FixedUpdateBehaviour(Player c) { }

    public void UpdateBehaviour(Player c) 
    {
        wasWaterMoveEffectActive = isWaterMoveEffectActive;
        isWaterMoveEffectActive = c.Water.IsWaterCollision && !c.Water.IsFullSubmerged;

        if (!wasWaterMoveEffectActive
            && isWaterMoveEffectActive)
            c.waterMoveFx.Play();

        if (wasWaterMoveEffectActive
            && !isWaterMoveEffectActive)
            c.waterMoveFx.Stop();

        if(isWaterMoveEffectActive)
        {
            waterMoveEffectMain.startSpeed = Mathf.Clamp
                ( c.playerRigidBody.velocity.magnitude
                , WATER_MOVE_EFFECT_MIN_SPEED
                , WATER_MOVE_EFFECT_MAX_SPEED);
            waterMoveEffectMain.gravityModifier = Mathf.Clamp
                ( c.playerRigidBody.velocity.magnitude
                , WATER_MOVE_EFFECT_MIN_GRAVITY
                , WATER_MOVE_EFFECT_MAX_GRAVITY);

            waterMoveEffectPosition.x = c.transform.position.x;
            waterMoveEffectPosition.z = c.transform.position.z;
            waterMoveEffectPosition.y = c.Water.WaterHeight;

            c.waterMoveFx.transform.position = waterMoveEffectPosition;
        }

        if (!c.Water.WasPartialSubmerged && c.Water.IsPartialSubmerged)
            PlayWaterInOutFx(c);

        if (c.Water.WasPartialSubmerged && !c.Water.IsPartialSubmerged)
            PlayWaterInOutFx(c);
    }

    public void EndBehaviours(Player c) { }

    private void PlayWaterInOutFx(Player c)
    {
        if (c.waterInOutFx.isPlaying)
            return;

        c.waterInOutFx.Play();
        c.splashSound.PlayPitchedOneShot
            ( c.splashSound.clip
            , SettingsHighLogic.G.PlayerVolume
            , SFX_MIN_PT
            , SFX_MAX_PT);
    }
}
