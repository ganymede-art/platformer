using Assets.script;
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

    [System.NonSerialized] public float volumeMusic = 0.4f;
    [System.NonSerialized] public float volumeFootstep = 0.4f;
    [System.NonSerialized] public float volumeObject = 1.0f;
    [System.NonSerialized] public float volumeItem = 1.0f;

    // null sound.

    [System.NonSerialized] public AudioClip audioDefault;

    // message box.

    [System.NonSerialized] public AudioClip a_message_box_continue;
    [System.NonSerialized] public AudioClip a_message_box_negative;
    [System.NonSerialized] public AudioClip a_message_box_positive;

    // music.

    AudioSource musicAudioSource;
    GameMusicData musicData;

    private void Awake()
    {
        audioDefault = Resources.Load("sound/ui/sfx_message_box_negative") as AudioClip;

        a_message_box_continue = Resources.Load("sound/ui/sfx_message_box_continue") as AudioClip;
        a_message_box_negative = Resources.Load("sound/ui/sfx_message_box_negative") as AudioClip;
        a_message_box_positive = Resources.Load("sound/ui/sfx_message_box_positive") as AudioClip;

        musicAudioSource = this.gameObject.AddComponent<AudioSource>();
    }

    public void PlayMusic(GameMusicData music_data)
    {
        this.musicData = music_data;

        // stop music if name is empty or invalid.

        if(music_data.audioClip == null)
        {
            musicAudioSource.clip = null;
            StopMusic();
            return;
        }

        // do nothing is clip is already playing.

        if (musicAudioSource.clip != null 
            && musicAudioSource.clip.name == music_data.audioClip.name)
            return;

        // play music.

        musicAudioSource.clip = music_data.audioClip;
        musicAudioSource.loop = music_data.isLoop;
        musicAudioSource.volume = volumeMusic;
        musicAudioSource.Play();
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }
}
