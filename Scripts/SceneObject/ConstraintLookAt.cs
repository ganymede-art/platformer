using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintLookAt : MonoBehaviour
{
    public Transform lookAtTarget;

    void Update()
    {
        transform.LookAt(lookAtTarget);    
    }
}
