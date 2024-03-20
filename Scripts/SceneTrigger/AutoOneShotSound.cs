using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AutoOneShotSound : MonoBehaviour
{
    [FormerlySerializedAs("sound")]
    public AudioSource audioSource;
    public SoundTypeConstant soundType;
    public float minPitch;
    public float maxPitch;

    private void Start()
    {
        float volumeScale
            = soundType.soundType == SoundType.Player ? SettingsHighLogic.G.PlayerVolume
            : soundType.soundType == SoundType.Music ? SettingsHighLogic.G.MusicVolume
            : soundType.soundType == SoundType.Environment ? SettingsHighLogic.G.EnvironmentVolume
            : soundType.soundType == SoundType.Mob ? SettingsHighLogic.G.MobVolume
            : soundType.soundType == SoundType.Prop ? SettingsHighLogic.G.PropVolume
            : soundType.soundType == SoundType.UserInterface ? SettingsHighLogic.G.UserInterfaceVolume
            : SettingsHighLogic.G.MasterVolume;

        audioSource.PlayPitchedOneShot(audioSource.clip, volumeScale, minPitch, maxPitch);
    }
}
