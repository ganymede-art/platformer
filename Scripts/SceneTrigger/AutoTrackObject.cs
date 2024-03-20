using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTrackObject : MonoBehaviour
{
    // Public fields.
    public GameObject trackObject;
    public Vector3 offset;
    public bool doReparent;

    private void Start()
    {
        if(doReparent && trackObject != null)
        {
            transform.SetParent(trackObject.transform, false);
        }
    }

    void Update()
    {
        if (trackObject != null)
            transform.position = trackObject.transform.TransformPoint(offset);
    }
}
