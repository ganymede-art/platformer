using Assets.script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class CameraAudioController : MonoBehaviour
{
    public GameObject managerObject;
    private IActorDataManager manager;
    private ActorData managerData;

    private bool isSubmerged;
    private bool wasSubmerged;

    AudioLowPassFilter audioLowPassFilter;

    AudioReverbZone[] audioReverbZoneObjects;

    // Start is called before the first frame update
    void Start()
    {
        isSubmerged = false;
        wasSubmerged = false;

        managerObject = GameObject.Find(GameConstants.NAME_PLAYER);
        manager = managerObject.GetComponent<IActorDataManager>();

        audioLowPassFilter = gameObject.AddComponent<AudioLowPassFilter>();
        audioLowPassFilter.enabled = false;
        audioLowPassFilter.cutoffFrequency = 500F;

        audioReverbZoneObjects = GameObject.FindObjectsOfType<AudioReverbZone>();
    }

    private void FixedUpdate()
    {
        if(manager == null)
        {
            managerObject = GameObject.Find(GameConstants.NAME_PLAYER);
            manager = managerObject.GetComponent<IActorDataManager>();

            if (manager == null)
                return;
        }

        managerData = manager.GetActorData();

        if (managerData == null)
            return;

        wasSubmerged = isSubmerged;
        isSubmerged = managerData.isSubmerged;

        if(isSubmerged != wasSubmerged)
        {
            audioLowPassFilter.enabled = isSubmerged;

            foreach (var zone in audioReverbZoneObjects)
                zone.enabled = !isSubmerged;
        }
    }
}
