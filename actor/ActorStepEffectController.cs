using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Script.GameConstants;

public class ActorStepEffectController : MonoBehaviour
{
    private AudioSource audioSource;
    private IActorDataManager manager;
    private ActorData managerData;

    public GameObject managerObject;

    public bool onlyUseDefault;

    public AudioClip defaultStepSound;
    public AudioClip waterStepSound;

    void Start()
    {
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1;
        audioSource.volume = GameMasterController.Global.settingsController.volumeFootstep;

        manager = managerObject.GetComponent<IActorDataManager>();
    }

    // triggered by animation event.

    public void PlayStepSound()
    {
        managerData = manager.GetActorData();

        if (managerData == null)
            return;

        if (managerData.groundData != null && !onlyUseDefault)
            audioSource.clip = managerData.groundData.groundStepSound;
        else
            audioSource.clip = defaultStepSound;

        if (managerData.isInWater)
            audioSource.clip = waterStepSound;

        audioSource.pitch = Random.Range(0.8f, 1.2f);

        audioSource.Play();
    }
}
