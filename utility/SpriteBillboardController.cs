using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboardController : MonoBehaviour
{
    private Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        transform.rotation = camera.transform.rotation;
    }
}
