using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MapWaterController : MonoBehaviour
{
    Renderer meshRenderer;

    [FormerlySerializedAs("scrolling_speed_x")]
    public float layer1ScrollXSpeed = 0.1f;
    [FormerlySerializedAs("scrolling_speed_y")]
    public float layer1ScrollYSpeed = 0.1f;
    [FormerlySerializedAs("counter_scrolling_speed_x")]
    public float layer2ScrollXSpeed = 0.1f;
    [FormerlySerializedAs("counter_scrolling_speed_y")]
    public float layer2ScrollYSpeed = 0.1f;
    

    void Start()
    {
        meshRenderer = GetComponent<Renderer>();
    }

    
    void Update()
    {
        float offset_x = Time.time * layer1ScrollXSpeed;
        float offset_y = Time.time * layer1ScrollYSpeed;
        float counter_offset_x = Time.time * layer2ScrollXSpeed;
        float counter_offset_y = Time.time * layer2ScrollYSpeed;

        meshRenderer.material.SetTextureOffset("_1Tex", new Vector2(offset_x, offset_y));
        meshRenderer.material.SetTextureOffset("_2Tex", new Vector2(counter_offset_x, counter_offset_y));

    }
}
