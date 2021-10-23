using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using System.Collections;

namespace Assets.script.camera
{
    [ExecuteInEditMode]
    public class CameraResolutionController : MonoBehaviour
    {
        public int w = 720;
        int h;
        protected void Start()
        {

        }

        void Update()
        {

            float ratio = ((float)Camera.main.pixelHeight / (float)Camera.main.pixelWidth);
            h = Mathf.RoundToInt(w * ratio);

        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            source.filterMode = FilterMode.Bilinear;
            RenderTexture buffer = RenderTexture.GetTemporary(w, h, -1);
            buffer.filterMode = FilterMode.Bilinear;
            Graphics.Blit(source, buffer);
            Graphics.Blit(buffer, destination);
            RenderTexture.ReleaseTemporary(buffer);
        }
    }
}
