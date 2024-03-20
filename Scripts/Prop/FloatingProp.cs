using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingProp : MonoBehaviour
{
    // Consts.
    const float WOBBLE_JITTER_MIN = 0.7F;
    const float WOBBLE_JITTER_MAX = 1.3F;
    const float BOB_JITTER_MIN = 0.7F;
    const float BOB_JITTER_MAX = 1.3F;

    // Private fields.
    private Vector3 originalPosition;
    private Vector3 originalRotation;

    private Vector3 wobbleVector;
    private float wobbleXTimer;
    private float wobbleZTimer;

    private Vector3 bobOffset;
    private float bobTimer;

    // Public fields.
    public Rigidbody propRigidBody;
    public float wobbleSpeed;
    public float wobbleDegrees;
    public float bobSpeed;
    public float bobDistance;

    void Start()
    {
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
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play
            && StateHighLogic.G.ActiveState != HighLogicStateId.Film)
            return;

        // wobble.

        wobbleXTimer += (Time.deltaTime * wobbleSpeed)
            * Random.Range(WOBBLE_JITTER_MIN, WOBBLE_JITTER_MAX);
        wobbleZTimer += (Time.deltaTime * wobbleSpeed)
            * Random.Range(WOBBLE_JITTER_MIN, WOBBLE_JITTER_MAX);

        wobbleVector.x = (Mathf.Sin(wobbleXTimer) * wobbleDegrees);
        wobbleVector.z = Mathf.Sin(wobbleZTimer) * wobbleDegrees;

        propRigidBody.MoveRotation(Quaternion.Euler(wobbleVector + originalRotation));

        // bob.

        bobTimer += (Time.deltaTime * bobSpeed)
            * Random.Range(BOB_JITTER_MIN, BOB_JITTER_MAX);

        bobOffset.y = (Mathf.Sin(bobTimer) * bobDistance);

        propRigidBody.MovePosition(originalPosition + bobOffset);
    }
}
