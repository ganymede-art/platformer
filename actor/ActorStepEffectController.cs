using Assets.script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.script.GameConstants;

public class ActorStepEffectController : MonoBehaviour
{
    private AudioSource audioSource;
    private Dictionary<GroundType, AudioClip> stepSounds;
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

        stepSounds = new Dictionary<GroundType, AudioClip>();
        stepSounds.Add(GroundType.ground_default, defaultStepSound);
        stepSounds.Add(GroundType.ground_sand, sandStepSound);
        stepSounds.Add(GroundType.ground_mud, mudStepSound);
        stepSounds.Add(GroundType.ground_grass, grassStepSound);
        stepSounds.Add(GroundType.ground_foliage, foliageStepSound);
        stepSounds.Add(GroundType.ground_metal, metalStepSound);
        stepSounds.Add(GroundType.ground_stone, stoneStepSound);
        stepSounds.Add(GroundType.ground_wood, woodStepSound);
        stepSounds.Add(GroundType.ground_water, waterStepSound);

        manager = managerObject.GetComponent<IActorDataManager>();
    }

    // triggered by animation event.

    public void PlayStepSound()
    {
        managerData = manager.GetActorData();

        if (managerData == null)
            return;

        if (stepSounds.ContainsKey(managerData.groundType) && !onlyUseDefault)
            audioSource.clip = stepSounds[managerData.groundType];
        else
            audioSource.clip = defaultStepSound;

        if (managerData.isInWater)
            audioSource.clip = waterStepSound;

        audioSource.pitch = Random.Range(0.8f, 1.2f);

        audioSource.Play();
    }
}
