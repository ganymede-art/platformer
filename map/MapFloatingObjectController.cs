using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using static Assets.Script.GameConstants;

public class MapFloatingObjectController : MonoBehaviour
{
    const float WOBBLE_JITTER_MIN = 0.7F;
    const float WOBBLE_JITTER_MAX = 1.3F;

    const float BOB_JITTER_MIN = 0.7F;
    const float BOB_JITTER_MAX = 1.3F;

    private Rigidbody rb;
    private Vector3 originalPosition;
    private Vector3 originalRotation;

    private Vector3 wobbleVector;
    private float wobbleXTimer;
    private float wobbleZTimer;

    private Vector3 bobOffset;
    private float bobTimer;

    public GameObject rigidBodyObject;

    public float wobbleSpeed;
    public float wobbleDegrees;

    public float bobSpeed;
    public float bobDistance;

    void Start()
    {
        if (rigidBodyObject == null)
            rigidBodyObject = gameObject;

        rb = rigidBodyObject.GetComponent<Rigidbody>();

        originalPosition = transform.position;
        originalRotation = transform.rotation.eulerAngles;

        wobbleVector = new Vector3(0, 0, 0);
        wobbleXTimer = Random.Range(0.0F, 1.0F);
        wobbleZTimer = Random.Range(0.0F, 1.0F);

        bobOffset = new Vector3(0, 0, 0);
        bobTimer = Random.Range(0.0F, 1.0F);
    }

    void Update()
    {
        // wobble.

        wobbleXTimer += (Time.deltaTime * wobbleSpeed)
            * Random.Range(WOBBLE_JITTER_MIN,WOBBLE_JITTER_MAX);
        wobbleZTimer += (Time.deltaTime * wobbleSpeed)
            * Random.Range(WOBBLE_JITTER_MIN, WOBBLE_JITTER_MAX);

        wobbleVector.x = (Mathf.Sin(wobbleXTimer) * wobbleDegrees);
        wobbleVector.z = Mathf.Sin(wobbleZTimer) * wobbleDegrees;

        rb.MoveRotation(Quaternion.Euler(wobbleVector + originalRotation));

        // bob.

        bobTimer += (Time.deltaTime * bobSpeed) 
            * Random.Range(BOB_JITTER_MIN, BOB_JITTER_MAX);

        bobOffset.y = (Mathf.Sin(bobTimer) * bobDistance);

        rb.MovePosition(originalPosition + bobOffset);
    }

    private void OnTriggerEnter(Collider other)
    {
    }

    private void OnTriggerExit(Collider other)
    {
    }
}
