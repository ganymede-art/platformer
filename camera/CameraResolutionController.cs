using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

namespace Assets.script.camera
{
    [ExecuteInEditMode]
    public class CameraResolutionController : MonoBehaviour
    {
        private int w = 720;
        public int h = 480;
        protected void Start()
        {
        }

        void Update()
        {

            float ratio = ((float)Camera.main.pixelWidth / (float)Camera.main.pixelHeight);
            w = Mathf.RoundToInt(h * ratio);

        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            source.filterMode = FilterMode.Bilinear;
            RenderTexture buffer = RenderTexture.GetTemporary(w, h, -1);
            buffer.filterMode = FilterMode.Point;
            Graphics.Blit(source, buffer);
            Graphics.Blit(buffer, destination);
            RenderTexture.ReleaseTemporary(buffer);
        }
    }
}
