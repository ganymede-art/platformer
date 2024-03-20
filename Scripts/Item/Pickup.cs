using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class Pickup : MonoBehaviour, INameable
{
    // Consts.
    private const float BOUNCE_FORCE_MULT = 2.0F;
    private static readonly Vector3 FX_OFFSET = new Vector3(0.0F, 0.125F, 0.0F);

    // Public fields.
    [Header("Pickup Attributes")]
    public bool doModifyHealth;
    public int healthChangeAmount;
    public bool doModifyOxygen;
    public int oxygenChangeAmount;
    public bool doModifyAmmo;
    public int ammoChangeAmount;
    public bool doModifyMoney;
    public int moneyChangeAmount;
    [Space]
    public GameObject fxPrefab;

    [Header("Physics Attributes")]
    public Rigidbody pickupRigidBody;

    private void Start()
    {
        if (pickupRigidBody == null)
            return;

        pickupRigidBody.AddForce(Vector3.up * BOUNCE_FORCE_MULT, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LAYER_PLAYER)
            return;

        if (other.name != TRANSFORM_NAME_PLAYER_COLLIDER)
            return;

        if (doModifyHealth)
            PlayerHighLogic.G.ModifyHealth(healthChangeAmount);
        if (doModifyOxygen)
            PlayerHighLogic.G.ModifyOxygen(oxygenChangeAmount);
        if (doModifyAmmo)
            PlayerHighLogic.G.ModifyAmmo(ammoChangeAmount);
        if (doModifyMoney)
            PlayerHighLogic.G.ModifyMoney(moneyChangeAmount);

        Instantiate(fxPrefab, transform.position + FX_OFFSET, transform.rotation);

        Destroy(gameObject);
    }

    public string GetName() => $"Pickup"
        + $"{(doModifyHealth ? "Health" : "")}"
        + $"{(doModifyOxygen ? "Oxygen" : "")}"
        + $"{(doModifyAmmo ? "Ammo" : "")}"
        + $"{(doModifyMoney ? "Money" : "")}"
        + $"{(ammoChangeAmount + moneyChangeAmount + healthChangeAmount + oxygenChangeAmount)}";
}
