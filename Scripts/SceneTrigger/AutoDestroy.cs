using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    // Private fields.
    private float destroyTimer;

    // Public fields.
    public GameObject destroyObject;
    public float destroyInterval;

    void Start()
    {
        if (destroyInterval <= 0.0F)
            Destroy(destroyObject);
    }

    private void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play)
            return;

        destroyTimer += Time.deltaTime;

        if (destroyTimer > destroyInterval)
            Destroy(destroyObject);
    }
}
