using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSound : MonoBehaviour
{
    public AudioSource audioSource;
    public SoundTypeConstant soundType;

    void Start()
    {
        AutoPlaySound();
    }

    private void OnEnable()
    {
        AutoPlaySound();
    }

    private void AutoPlaySound()
    {
        float volumeScale
            = soundType.soundType == SoundType.Player ? SettingsHighLogic.G.PlayerVolume
            : soundType.soundType == SoundType.Music ? SettingsHighLogic.G.MusicVolume
            : soundType.soundType == SoundType.Environment ? SettingsHighLogic.G.EnvironmentVolume
            : soundType.soundType == SoundType.Mob ? SettingsHighLogic.G.MobVolume
            : soundType.soundType == SoundType.Prop ? SettingsHighLogic.G.PropVolume
            : soundType.soundType == SoundType.UserInterface ? SettingsHighLogic.G.UserInterfaceVolume
            : SettingsHighLogic.G.MasterVolume;

        audioSource.volume = volumeScale;
        audioSource.Play();
    }
}
