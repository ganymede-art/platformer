using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundAction : MonoBehaviour, IAction
{
    // Private fields.
    private float volumeScale;

    // Public fields.
    [Header("Action Attributes")]
    public GameObject nextActionObject;

    [Header("Sound Attributes")]
    public AudioSource audioSource;
    public SoundTypeConstant soundType;
    public float minPitch;
    public float maxPitch;

    // Public properties.
    public GameObject NextActionObject => nextActionObject;
    public ActionType ActionType => ActionType.PlaySound;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => true;
    public bool IsActionUpdateComplete => true;

    public void BeginAction(ActionSource actionSource)
    {
        volumeScale = soundType.soundType switch
        {
            SoundType.Player => SettingsHighLogic.G.PlayerVolume,
            SoundType.Music => SettingsHighLogic.G.MusicVolume,
            SoundType.Environment => SettingsHighLogic.G.EnvironmentVolume,
            SoundType.Mob => SettingsHighLogic.G.MobVolume,
            SoundType.Prop => SettingsHighLogic.G.PropVolume,
            _ => 1.0F,
        };

        audioSource.PlayPitchedOneShot
            ( audioSource.clip
            , volumeScale
            , minPitch
            , maxPitch);
    }

    public void UpdateAction(ActionSource actionSource) { }
    public void EndAction(ActionSource actionSource) { }
}
