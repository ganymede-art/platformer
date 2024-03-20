using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHighLogic : MonoBehaviour
{
    // Consts.
    private const float FADE_SPEED_MULT = 2.0F;

    // Private fields.
    private MusicStatus activeStatus;
    private MusicStatus previousStatus;
    private AudioSource musicAudioSource;
    private MusicData activeMusicData;
    private MusicData previousMusicData;
    private float staticVolume;
    private float dynamicVolume;
    private float targetDynamicVolume;

    // Public properties.
    public static MusicHighLogic G => GameHighLogic.G?.MusicHighLogic;

    private void Awake()
    {
        staticVolume = 1.0F;
        dynamicVolume = 1.0F;
        targetDynamicVolume = 1.0F;
        activeStatus = MusicStatus.Stopped;
        previousStatus = MusicStatus.Stopped;
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.loop = true;
    }

    private void Update()
    {
        if(activeStatus == MusicStatus.FadeIn)
        {
            staticVolume = Mathf.MoveTowards(staticVolume, 1.0F, Time.deltaTime * FADE_SPEED_MULT);
            UpdateAudioSourceVolume();
            if (staticVolume == 1.0F)
                ChangeStatus(MusicStatus.Playing);
        }
        else if(activeStatus == MusicStatus.FadeOut)
        {
            staticVolume = Mathf.MoveTowards(staticVolume, 0.0F, Time.deltaTime * FADE_SPEED_MULT);
            UpdateAudioSourceVolume();
            if (staticVolume == 0.0F)
                if (activeMusicData == null || activeMusicData.musicAudioClip == null)
                    ChangeStatus(MusicStatus.Stopped);
                else
                    ChangeStatus(MusicStatus.SwitchClips);
        }

        if(dynamicVolume != targetDynamicVolume)
        {
            dynamicVolume = Mathf.MoveTowards(dynamicVolume, targetDynamicVolume, Time.deltaTime * FADE_SPEED_MULT);
            UpdateAudioSourceVolume();
        }
    }

    private void UpdateAudioSourceVolume()
    {
        musicAudioSource.volume = staticVolume * dynamicVolume * SettingsHighLogic.G.MusicVolume;
    }

    private void ChangeStatus(MusicStatus newStatus)
    {
        previousStatus = activeStatus;
        activeStatus = newStatus;
        if (activeStatus == MusicStatus.SwitchClips)
        {
            staticVolume = 0.0F;
            musicAudioSource.Stop();
            musicAudioSource.clip = activeMusicData.musicAudioClip;
            musicAudioSource.Play();
            ChangeStatus(MusicStatus.FadeIn);
            return;
        }
        else if(activeStatus == MusicStatus.Stopped)
        {
            staticVolume = 0.0F;
            musicAudioSource.Stop();
            musicAudioSource.clip = null;
        }
        else if(activeStatus == MusicStatus.Playing)
        {
            staticVolume = 1.0F;
            if(!musicAudioSource.isPlaying)
                musicAudioSource.Play();
        }
        else if(activeStatus == MusicStatus.FadeIn)
        {
            staticVolume = 0.0F;
            if (!musicAudioSource.isPlaying)
                musicAudioSource.Play();
        }
        else if(activeStatus == MusicStatus.FadeOut)
        {
            staticVolume = 1.0F;
            if (!musicAudioSource.isPlaying)
                musicAudioSource.Play();
        }
    }

    public void BeginMusic(MusicData newMusicData)
    {
        // Don't restart the same music data.
        if (newMusicData?.name == activeMusicData?.name)
            return;

        previousMusicData = activeMusicData;
        activeMusicData = newMusicData;

        if (activeStatus == MusicStatus.Playing)
            ChangeStatus(MusicStatus.FadeOut);
        else
            ChangeStatus(MusicStatus.SwitchClips);
        
    }

    public void EndMusic()
    {
        previousMusicData = activeMusicData;
        activeMusicData = null;
        ChangeStatus(MusicStatus.FadeOut);
    }

    public void SetTargetDynamicVolume(float newTargetVolume)
    {
        targetDynamicVolume = newTargetVolume;
    }
}
