using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using static Assets.script.ActorEnums;

public class ActorEyeController : MonoBehaviour
{
    const float BLINK_INTERVAL = 0.075f;
    const float RANDOM_BLINK_INTERVAL_MIN = 1.0f;
    const float RANDOM_BLINK_INTERVAL_MAX = 3.0f;

    public GameObject actor_renderer_object;
    public int eye_material_index = 0;
    public Material[] blink_materials;

    private ActorEyeMode eye_mode;

    private Renderer actor_renderer;
    private Material[] actor_materials;

    private float blink_interval = 0.0f;
    private float blink_time = 0.0f;
    private int blink_index = 0;

    private bool is_blinking_started = false;
    private bool is_blinking_finished = false;

    private void Start()
    {
        actor_renderer = actor_renderer_object.GetComponent<Renderer>();
        actor_materials = actor_renderer.materials;
    }

    private void Update()
    {
        blink_time += Time.deltaTime;

        if(!is_blinking_started && blink_time >= blink_interval)
        {
            blink_time = 0.0f;
            blink_interval = BLINK_INTERVAL;
            blink_index = 0;
            is_blinking_started = true;
            is_blinking_finished = false;
        }

        if(is_blinking_started && blink_time >= blink_interval)
        {
            blink_time = 0.0f;

            actor_materials[eye_material_index] = blink_materials[blink_index];
            actor_renderer.materials = actor_materials;

            if(blink_index == blink_materials.Length - 1)
                is_blinking_finished = true;

            blink_index += is_blinking_finished ? -1 : 1;

            if (blink_index == -1)
            {
                is_blinking_started = false;
                is_blinking_finished = false;
                blink_index = 0;
                blink_interval = Random.Range(RANDOM_BLINK_INTERVAL_MIN, RANDOM_BLINK_INTERVAL_MAX);
            }

        }

    }

}
