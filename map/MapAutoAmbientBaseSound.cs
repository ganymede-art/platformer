using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAutoAmbientBaseSound : MonoBehaviour
{
    private AudioSource baseAudioSource;
    public GameObject audioSourceObject;

    void Start()
    {
        if (audioSourceObject == null)
            audioSourceObject = gameObject;

        baseAudioSource = audioSourceObject.GetComponent<AudioSource>();
        if (baseAudioSource == null)
            baseAudioSource = audioSourceObject.AddComponent<AudioSource>();

        // setup the base sound.
        baseAudioSource.volume =
                baseAudioSource.volume * GameSettingsController.Global.volumeAmbience;
        baseAudioSource.loop = true;
        baseAudioSource.Play();
    }
}
