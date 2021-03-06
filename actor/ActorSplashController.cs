using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class ActorSplashController : MonoBehaviour
{
    const float SPAWN_INTERVAL = 0.25F;
    const float TIMER_MULTIPLIER = 1F;
    static readonly Vector3 SPAWN_OFFSET = new Vector3(0.0F, 0.005F, 0.0F);

    public IActorDataManager manager;
    public GameObject splash_prefab;
    public float splash_scale_multiplier = 1F;

    private float timer;
    ActorData manager_data;
    private float y_level;
    private bool is_in_water;
    private bool is_submerged;
    private Vector3 spawn_vector;
    private GameObject spawned_splash_prefab;

    // Start is called before the first frame update
    void Start()
    {
        manager = this.gameObject.GetComponent<IActorDataManager>();

        timer = 0.0F;
        y_level = 0F;
        is_in_water = false;
        is_submerged = false;
        spawn_vector = new Vector3(0,0,0);
        spawned_splash_prefab = null;
    }

    // Update is called once per frame
    void Update()
    {
        manager_data = manager.GetActorData();

        if (manager_data == null)
            return;

        y_level = manager_data.waterYLevel;
        is_in_water = manager_data.isInWater;
        is_submerged = manager_data.isSubmerged;

        timer += (Time.deltaTime * TIMER_MULTIPLIER);

        if(timer >= SPAWN_INTERVAL)
        {
            timer = 0.0F;

            

            if (!is_in_water)
                return;

            if (is_submerged)
                return;

            spawn_vector.x = this.transform.position.x;
            spawn_vector.y = y_level;
            spawn_vector.z = this.transform.position.z;

            spawn_vector += SPAWN_OFFSET;

            spawned_splash_prefab = Instantiate(splash_prefab, spawn_vector, Quaternion.identity);
            spawned_splash_prefab.transform.localScale = Vector3.zero;
            spawned_splash_prefab.GetComponent<ActorFxSplashController>().scaleMultiplier = splash_scale_multiplier;
        }
    }
}
