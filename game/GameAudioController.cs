using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameAudioController : MonoBehaviour
{
    private static GameAudioController global;
    public static GameAudioController Global
    {
        get
        {
            if(global == null)
            {
                global = GameMasterController.Global.audioController;
            }
            return global;
        }
    }

    [System.NonSerialized] public float volumeMusic = 0.4F;
    [System.NonSerialized] public float volumeFootstep = 0.4F;
    [System.NonSerialized] public float volumeObject = 1.0F;
    [System.NonSerialized] public float volumeItem = 1.0F;

    // music.

    AudioSource musicAudioSource;
    MusicData currentMusicData;

    private void Awake()
    {
        musicAudioSource = this.gameObject.AddComponent<AudioSource>();
    }

    public void PlayMusic(MusicData newMusicData)
    {
        if (currentMusicData == null)
            currentMusicData = newMusicData;

        // stop music if name is empty or invalid.

        if(newMusicData == null || newMusicData.audioClip == null)
        {
            musicAudioSource.clip = null;
            StopMusic();
            return;
        }

        // do nothing is clip is already playing.

        if (musicAudioSource.clip != null
            && musicAudioSource.clip.name == newMusicData.audioClip.name)
            return;

        // play music.

        if(currentMusicData.code == newMusicData.code)
        {
            float pos = musicAudioSource.time;

            musicAudioSource.clip = newMusicData.audioClip;
            musicAudioSource.loop = newMusicData.isLoop;
            musicAudioSource.volume = volumeMusic;
            musicAudioSource.Play();

            musicAudioSource.time = pos;
        }
        else
        {
            musicAudioSource.clip = newMusicData.audioClip;
            musicAudioSource.loop = newMusicData.isLoop;
            musicAudioSource.volume = volumeMusic;
            musicAudioSource.Play();
        }

        // set current music data.

        this.currentMusicData = newMusicData;
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }
}
