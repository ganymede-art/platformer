using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MobConstants;

public class PlaySoundMobStateAction : MonoBehaviour, IStateAction<Mob, MobStateId>
{
    // Public properties.
    public MobStateId StateId => stateId.mobStateId;

    // Public fields.
    public MobStateIdConstant stateId;
    [Space]
    public AudioSource actionAudioSource;

    public void BeginStateAction(Mob controller, Dictionary<string, object> args = null)
    {
        if(actionAudioSource!=null)
            actionAudioSource.PlayPitchedOneShot
                ( actionAudioSource.clip
                , SettingsHighLogic.G.MobVolume
                , MOB_SFX_MIN_PITCH
                , MOB_SFX_MAX_PITCH);
    }
    public void EndStateAction(Mob controller) { }
    public void FixedUpdateStateAction(Mob controller) { }
    public void UpdateStateAction(Mob controller) { }
}
