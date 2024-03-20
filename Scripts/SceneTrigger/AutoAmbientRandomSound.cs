using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAmbientRandomSound : MonoBehaviour
{
    private float timer;
    private float interval;

    private AudioSource[] audioSources;

    public GameObject audioSourcesContainerObject;
    public float minInterval;
    public float maxInterval;
    [Space]
    public float minPitch;
    public float maxPitch;

    private void Start()
    {
        audioSources = audioSourcesContainerObject.GetComponentsInChildren<AudioSource>();
        interval = Random.Range(minInterval, maxInterval);
    }

    private void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play
            && StateHighLogic.G.ActiveState != HighLogicStateId.Film)
            return;

        if(timer >= interval)
        {
            timer = 0.0F;
            interval = Random.Range(minInterval, maxInterval);

            int randomIndex = Random.Range(0, audioSources.Length);
            audioSources[randomIndex].PlayPitchedOneShot
                ( audioSources[randomIndex].clip
                , SettingsHighLogic.G.EnvironmentVolume
                , minPitch
                , maxPitch);
        }

        timer += Time.deltaTime;
    }
}
