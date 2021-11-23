using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapThunderController : MonoBehaviour
{
    const float RANDOM_THUNDER_INTERVAL_MIN = 5.0f;
    const float RANDOM_THUNDER_INTERVAL_MAX = 30.0f;

    const float LIGHTNING_INTERVAL_MAX = 1.0f;

    private List<Light> lightningLights;
    
    private float thunderTimer;
    private float thunderInterval;
    private AudioSource thunderAudioSource;

    private bool isLightningActive;
    private float lightningTimer;

    public bool isThunder;
    public AudioClip[] thunderSounds;

    public bool isLightning;
    public GameObject[] lightningObjects;
    public float lightningIntensity;
    
    void Start()
    {
        if(isThunder)
        {
            thunderAudioSource = gameObject.GetComponent<AudioSource>();

            if (thunderAudioSource == null)
                thunderAudioSource = gameObject.AddComponent<AudioSource>();
        }

        if(isLightning)
        {
            lightningLights = new List<Light>();

            foreach(var lightningObject in lightningObjects)
            {
                var component = lightningObject.GetComponent<Light>();

                if (component == null)
                    continue;

                lightningLights.Add(component);
            }
        }

        thunderInterval = Random.Range
            (RANDOM_THUNDER_INTERVAL_MIN, RANDOM_THUNDER_INTERVAL_MAX);
    }

    void Update()
    {
        thunderTimer += Time.deltaTime;

        if(thunderTimer >= thunderInterval)
        {
            thunderTimer = 0.0f;
            thunderInterval = Random.Range
                (RANDOM_THUNDER_INTERVAL_MIN, RANDOM_THUNDER_INTERVAL_MAX);

            if(isThunder)
            {
                thunderAudioSource.clip = thunderSounds[Random.Range(0, thunderSounds.Length)];
                thunderAudioSource.volume = Random.Range(1.4f, 2.2f);
                thunderAudioSource.pitch = Random.Range(0.7f, 1.2f);
                thunderAudioSource.Play();
            }

            if (isLightning)
            {
                isLightningActive = true;

                lightningTimer = 0.0f;

                foreach (var lightningObject in lightningObjects)
                {
                    lightningObject.SetActive(true);
                }

                foreach (var light in lightningLights)
                {
                    light.intensity = lightningIntensity;
                }
            }
        }

        if(isLightningActive)
        {
            if(lightningTimer < LIGHTNING_INTERVAL_MAX)
            {
                lightningTimer += Time.deltaTime;

                float progress = Mathf.InverseLerp(LIGHTNING_INTERVAL_MAX, 0.0f, lightningTimer);
                float newIntensity = Mathf.Lerp(0.0f, lightningIntensity, progress);

                foreach (var light in lightningLights)
                {
                    light.intensity = newIntensity;
                }
            }
            else
            {
                isLightningActive = false;

                foreach (var lightningObject in lightningObjects)
                {
                    lightningObject.SetActive(false);
                }
            }
        }
    }
}
