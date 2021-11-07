using Assets.script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class CameraAudioController : MonoBehaviour
{
    [FormerlySerializedAs("manager_game_object")]
    public GameObject managerObject;
    private IActorDataManager manager;
    private ActorData managerData;

    AudioLowPassFilter audioLowPassFilter;

    // Start is called before the first frame update
    void Start()
    {
        managerObject = GameObject.Find(GameConstants.NAME_PLAYER);
        manager = managerObject.GetComponent<IActorDataManager>();

        audioLowPassFilter = gameObject.AddComponent<AudioLowPassFilter>();
        audioLowPassFilter.enabled = false;
        audioLowPassFilter.cutoffFrequency = 500f;
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

        audioLowPassFilter.enabled = managerData.isSubmerged;
    }
}
