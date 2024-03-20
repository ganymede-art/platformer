using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotateObject : MonoBehaviour
{
    // Private fields.
    private Rigidbody rotateRigidBody;

    // Public fields.
    [Header("Rotation Attributes")]
    public GameObject rotateObject;
    public bool isRotateObjectKinematic;
    public float rotationSpeed;
    public bool doRotateInFilm;


    private void Start()
    {
        if (isRotateObjectKinematic)
            rotateRigidBody = rotateObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        bool doRotate = (StateHighLogic.G.ActiveState == HighLogicStateId.Play
            || (doRotateInFilm && StateHighLogic.G.ActiveState == HighLogicStateId.Film));

        if (!doRotate)
            return;

        if (!isRotateObjectKinematic)
        {
            rotateObject.transform.Rotate(transform.up * (rotationSpeed * Time.deltaTime));
        }
        else
        {
            var rotationVelocity = new Vector3(0, rotationSpeed, 0);
            Quaternion deltaRotation = Quaternion.Euler(rotationVelocity * Time.fixedDeltaTime);
            rotateRigidBody.MoveRotation(rotateRigidBody.rotation * deltaRotation);
        }
    }
}
