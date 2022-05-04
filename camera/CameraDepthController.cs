using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraDepthController : MonoBehaviour
{
    private void OnEnable()
    {
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
    }
}
