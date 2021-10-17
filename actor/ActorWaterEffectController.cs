using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;


public class ActorWaterEffectController : MonoBehaviour
{
    private GameMasterController master;
    public IActorDataManager manager;
    private ActorDataManager? manager_data;

    public GameObject air_bubble_origin_object;
    public GameObject air_bubble_fx_prefab;
    private GameObject air_bubble_fx_object;
    private ParticleSystem air_bubble_ps;
    private ParticleSystem.EmissionModule air_bubble_ps_em;
    private ParticleSystem.TriggerModule air_bubble_ps_tm;

    void Start()
    {
        master = GameMasterController.GetMasterController();
        manager = this.gameObject.GetComponent<IActorDataManager>();
        air_bubble_fx_object = Instantiate(air_bubble_fx_prefab, air_bubble_origin_object.transform);

        air_bubble_ps = air_bubble_fx_object.GetComponent<ParticleSystem>();

        var shape = air_bubble_ps.shape;
        shape.enabled = false;

        air_bubble_ps_em = air_bubble_ps.emission;

        air_bubble_ps_tm = air_bubble_ps.trigger;
        air_bubble_ps_tm.enabled = true;

        // set to kill particles if leaving water trigger.

        air_bubble_ps_tm.enter = ParticleSystemOverlapAction.Ignore;
        air_bubble_ps_tm.exit = ParticleSystemOverlapAction.Kill;
        air_bubble_ps_tm.inside = ParticleSystemOverlapAction.Ignore;
        air_bubble_ps_tm.outside = ParticleSystemOverlapAction.Kill;

        // get a list of all water triggers in the scene.

        var water_trigger_objects = GameObject.FindGameObjectsWithTag(GameConstants.TAG_WATER);

        for(int i = 0; i < water_trigger_objects.Length; i++)
        {
            air_bubble_ps_tm.SetCollider(i, water_trigger_objects[i].GetComponent<Collider>());
        }
    }

    void Update()
    {
        manager_data = manager.UpdateActorController();
        air_bubble_ps_em.enabled = manager_data.Value.is_submerged;
    }
}
