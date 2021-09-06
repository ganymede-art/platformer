using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioController : MonoBehaviour
{
    [System.NonSerialized] public float volume_music = 1.0f;
    [System.NonSerialized] public float volume_footstep = 1.0f;
    [System.NonSerialized] public float volume_object = 1.0f;

    // null sound.

    [System.NonSerialized] public AudioClip audio_default;

    // player.

    [System.NonSerialized] public AudioClip a_player_jump;
    [System.NonSerialized] public AudioClip a_player_water_jump;
    [System.NonSerialized] public AudioClip a_player_splash;
    [System.NonSerialized] public AudioClip a_player_slide;
    [System.NonSerialized] public AudioClip a_player_slide_loop;
    [System.NonSerialized] public AudioClip a_player_dive;
    [System.NonSerialized] public AudioClip a_player_hurt_default;
    [System.NonSerialized] public AudioClip a_player_hurt_fire;

    // message box.

    [System.NonSerialized] public AudioClip a_message_box_continue;
    [System.NonSerialized] public AudioClip a_message_box_negative;
    [System.NonSerialized] public AudioClip a_message_box_positive;

    // general.

    [System.NonSerialized] public Dictionary<string, AudioClip> audio_dictionary;

    // vox.

    [System.NonSerialized] public Dictionary<string, AudioClip[]> vox_dictionary;

    // music.

    [System.NonSerialized] public Dictionary<string, AudioClip> music_dictionary;
    AudioSource music_audio_source;

    private void Awake()
    {
        audio_default = Resources.Load("sound/ui/sfx_message_box_negative") as AudioClip;

        a_player_jump = Resources.Load("sound/player/sfx_player_jump") as AudioClip;
        a_player_water_jump = Resources.Load("sound/player/sfx_player_water_jump") as AudioClip;
        a_player_splash = Resources.Load("sound/player/sfx_player_splash") as AudioClip;
        a_player_slide = Resources.Load("sound/player/sfx_player_slide") as AudioClip;
        a_player_slide_loop = Resources.Load("sound/player/sfx_player_slide_loop") as AudioClip;
        a_player_dive = Resources.Load("sound/player/sfx_player_dive") as AudioClip;
        a_player_hurt_default = Resources.Load("sound/player/sfx_player_hurt_default") as AudioClip;
        a_player_hurt_fire = Resources.Load("sound/player/sfx_player_hurt_fire") as AudioClip;

        a_message_box_continue = Resources.Load("sound/ui/sfx_message_box_continue") as AudioClip;
        a_message_box_negative = Resources.Load("sound/ui/sfx_message_box_negative") as AudioClip;
        a_message_box_positive = Resources.Load("sound/ui/sfx_message_box_positive") as AudioClip;

        LoadAudioDictionary();
        LoadVoxDictionary();
        LoadMusicDictionary();
    }

    private void LoadAudioDictionary()
    {
        // load prop.

        var samples = Resources.LoadAll<AudioClip>("sound/general");

        audio_dictionary = new Dictionary<string, AudioClip>();

        foreach (var sample in samples)
        {
            audio_dictionary.Add(sample.name, sample);
        }
    }

    private void LoadVoxDictionary()
    {
        // load vox.

        var samples = Resources.LoadAll<AudioClip>("sound/vox");
        var temp_sample_dictionary = new Dictionary<string, List<AudioClip>>();

        // build up samples lists, with relevant sample prefix for key.

        foreach (var sample in samples)
        {
            string sample_name = sample.name.Split('_')[0];

            if (!temp_sample_dictionary.ContainsKey(sample_name))
                temp_sample_dictionary.Add(sample_name, new List<AudioClip>());

            temp_sample_dictionary[sample_name].Add(sample);
        }

        // add sample lists into the vox dictionary.

        vox_dictionary = new Dictionary<string, AudioClip[]>();

        foreach (var item in temp_sample_dictionary)
        {
            vox_dictionary.Add(item.Key, item.Value.ToArray());
        }
    }


    private void LoadMusicDictionary()
    {
        // load music.

        var samples = Resources.LoadAll<AudioClip>("music");

        music_dictionary = new Dictionary<string, AudioClip>();
        music_audio_source = this.gameObject.AddComponent<AudioSource>();

        foreach (var sample in samples)
        {
            music_dictionary.Add(sample.name, sample);
        }
    }

    public void PlayMusic(string name, bool is_loop)
    {
        // stop music if name is empty or invalid.

        if(name == null 
            || name == string.Empty 
            || !music_dictionary.ContainsKey(name))
        {
            StopMusic();
            return;
        }

        // do nothing is clip is already playing.

        if (music_audio_source.clip != null 
            && music_audio_source.clip.name == name)
            return;

        // play music.

        music_audio_source.clip = music_dictionary[name];
        music_audio_source.loop = is_loop;
        music_audio_source.Play();
    }

    public void StopMusic()
    {
        music_audio_source.Stop();
    }

    public AudioClip GetAudioClip(string name)
    {
        if (audio_dictionary.ContainsKey(name))
            return audio_dictionary[name];

        return audio_default;
    }
}
