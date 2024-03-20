using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;
using System;

public class PlayerProjectile : MonoBehaviour
{
    // Private fields.
    private Vector3 cachedVelocity;
    private Vector3 cachedAngularVelocity;
    private float projTimer;

    // Public fields.
    public Rigidbody projRigidBody;
    public GameObject onDestroyFxPrefab;

    void Start()
    {
        // Initial kick force.
        projRigidBody.AddForce(transform.forward * PROJECTILE_FORCE_MULT, ForceMode.VelocityChange);

        StateHighLogic.G.HighLogicStateChanged += OnHighLogicStateChanged;
    }

    private void OnDestroy()
    {
        if (StateHighLogic.G != null)
            StateHighLogic.G.HighLogicStateChanged -= OnHighLogicStateChanged;
    }

    void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play)
            return;

        if(projTimer >= PROJECTILE_MAX_INTERVAL)
        {
            OnImpact();
        }

        projTimer += Time.deltaTime;
    }

    private void OnImpact()
    {
        Instantiate(onDestroyFxPrefab, transform.position, transform.rotation);
        GameObject.Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnImpact();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnImpact();
    }

    public void OnHighLogicStateChanged(object sender, EventArgs e)
    {
        if (StateHighLogic.G.ActiveState == HighLogicStateId.Play
            && StateHighLogic.G.PreviousState != HighLogicStateId.Play)
        {
            projRigidBody.isKinematic = false;

            projRigidBody.velocity = cachedVelocity;
            projRigidBody.angularVelocity = cachedAngularVelocity;
        }
        else if (StateHighLogic.G.ActiveState != HighLogicStateId.Play
            && StateHighLogic.G.PreviousState == HighLogicStateId.Play)
        {
            cachedVelocity = projRigidBody.velocity;
            cachedAngularVelocity = projRigidBody.angularVelocity;

            projRigidBody.isKinematic = true;
        }
    }
}
