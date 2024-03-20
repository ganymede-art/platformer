using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VertexColourSampler : MonoBehaviour
{
    public MeshFilter sourceMeshFilter;
    public MeshRenderer targetMeshRenderer;
    [Space]
    public int sourceColourIndex;

    private void Awake()
    {
        SampleVertexColours(this);
    }

    public void SampleVertexColours(VertexColourSampler v)
    {
        if (targetMeshRenderer == null)
        {
            Debug.LogWarning($"[{GetType()}][{gameObject.name}] The target mesh is messing.");
            return;
        }

        if (sourceMeshFilter == null)
        {
            Debug.LogWarning($"[{GetType()}][{gameObject.name}] The source mesh is messing.");
            return;
        }

        if (!sourceMeshFilter.sharedMesh.isReadable)
        {
            Debug.LogWarning($"[{GetType()}][{gameObject.name}] The source mesh is not read/write enabled.");
            return;
        }

        var sourceColours = v.sourceMeshFilter.sharedMesh.colors;

        if (sourceColours.Length == 0)
        {
            Debug.LogWarning($"[{GetType()}][{gameObject.name}] The source mesh is missing vertex colour data.");
            return;
        }

        var colour = new Color();
        colour.r = sourceColours[sourceColourIndex].r;
        colour.g = sourceColours[sourceColourIndex].g;
        colour.b = sourceColours[sourceColourIndex].b;
        colour.a = 1.0F;

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mpb.SetColor("_Color", colour);
        targetMeshRenderer.SetPropertyBlock(mpb);

    }

    public void CalculateSourceColourIndex()
    {
        if (sourceMeshFilter == null)
        {
            Debug.LogWarning($"[{GetType()}] The source mesh is messing.");
        }

        if (!sourceMeshFilter.sharedMesh.isReadable)
        {
            Debug.LogWarning($"[{GetType()}] The source mesh is not read/write enabled.");
            return;
        }

        var sourceVerts = sourceMeshFilter.sharedMesh.vertices;
        var sourceColours = sourceMeshFilter.sharedMesh.colors;

        if (sourceColours.Length == 0)
        {
            Debug.LogWarning($"[{GetType()}] The source mesh is missing vertex colour data.");
            return;
        }

        int nearestVertIndex = 0;
        float nearest = 1000.0F;

        for (int j = 0; j < sourceVerts.Length; j++)
        {
            var sourceVert = sourceMeshFilter.gameObject.transform.TransformPoint(sourceVerts[j]);
            var thisDistance = Vector3.Distance(transform.position, sourceVert);

            if (thisDistance < nearest)
            {
                nearestVertIndex = j;
                nearest = thisDistance;
            }
        }

        sourceColourIndex = nearestVertIndex;
    }
}