using Assets.script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.script.GameConstants;

public class ActorStepEffectController : MonoBehaviour
{
    private AudioSource audioSource;
    private Dictionary<string, AudioClip> stepSounds;
    private IActorDataManager manager;
    private ActorData managerData;

    public GameObject managerObject;

    public bool onlyUseDefault;

    public AudioClip defaultStepSound;
    public AudioClip sandStepSound;
    public AudioClip mudStepSound;
    public AudioClip grassStepSound;
    public AudioClip foliageStepSound;
    public AudioClip metalStepSound;
    public AudioClip stoneStepSound;
    public AudioClip woodStepSound;
    public AudioClip waterStepSound;

    void Start()
    {
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1;
        audioSource.volume = GameMasterController.Global.audioController.volumeFootstep;

        stepSounds = new Dictionary<string, AudioClip>();
        stepSounds.Add(GROUND_TYPE_DEFAULT, defaultStepSound);
        stepSounds.Add(GROUND_TYPE_SAND, sandStepSound);
        stepSounds.Add(GROUND_TYPE_MUD, mudStepSound);
        stepSounds.Add(GROUND_TYPE_GRASS, grassStepSound);
        stepSounds.Add(GROUND_TYPE_FOLIAGE, foliageStepSound);
        stepSounds.Add(GROUND_TYPE_METAL, metalStepSound);
        stepSounds.Add(GROUND_TYPE_STONE, stoneStepSound);
        stepSounds.Add(GROUND_TYPE_WOOD, woodStepSound);
        stepSounds.Add(GROUND_TYPE_WATER, waterStepSound);

        manager = managerObject.GetComponent<IActorDataManager>();
    }

    // triggered by animation event.

    public void PlayStepSound()
    {
        managerData = manager.GetActorData();

        if (managerData == null)
            return;

        if (stepSounds.ContainsKey(managerData.groundData.groundType) && !onlyUseDefault)
            audioSource.clip = stepSounds[managerData.groundData.groundType];
        else
            audioSource.clip = defaultStepSound;

        if (managerData.isInWater)
            audioSource.clip = waterStepSound;

        audioSource.pitch = Random.Range(0.8f, 1.2f);

        audioSource.Play();
    }
}
