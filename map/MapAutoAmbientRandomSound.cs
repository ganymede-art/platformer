using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAutoAmbientRandomSound : MonoBehaviour
{
    private List<AudioSource> audioSources;
    
    private float randomSoundTimer;
    private float randomSoundInterval;

    public GameObject[] audioSourceObjects;

    public float minRandomSoundInterval;
    public float maxRandomSoundInterval;
    public float minSoundPitch;
    public float maxSoundPitch;

    void Start()
    {
        audioSources = new List<AudioSource>();

        foreach(GameObject audioSourceObject in audioSourceObjects)
        {
            var audioSource = audioSourceObject.GetComponent<AudioSource>();

            if (audioSource == null)
                continue;

            audioSource.volume = 
                audioSource.volume * GameSettingsController.Global.volumeAmbience;
            audioSources.Add(audioSource);
        }

        randomSoundTimer = 0.0F;
        randomSoundInterval = Random.Range
            (minRandomSoundInterval, maxRandomSoundInterval);
    }

    private void Update()
    {
        randomSoundTimer += Time.deltaTime;

        if(randomSoundTimer >= randomSoundInterval)
        {
            randomSoundTimer = 0.0F;
            randomSoundInterval = Random.Range
                (minRandomSoundInterval, maxRandomSoundInterval);

            var audioSource = audioSources[Random.Range(0, audioSources.Count)];

            if (audioSource.isPlaying)
                return;

            audioSource.pitch = Random.Range(minSoundPitch, maxSoundPitch);

            audioSource.Play();
        }
    }
}
