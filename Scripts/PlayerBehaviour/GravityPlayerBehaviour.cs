using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerConstants;

public class GravityPlayerBehaviour : MonoBehaviour, IBehaviour<Player, PlayerBehaviourId>
{
    // Private fields.
    private bool isGravityEnabled;

    // Public properties.
    public PlayerBehaviourId BehaviourId => PlayerBehaviourId.Gravity;
    public bool IsGravityEnabled
    {
        get => isGravityEnabled;
        set => isGravityEnabled = value;
    }

    private void Awake()
    {
        isGravityEnabled = true;
    }

    public void BeginBehaviour(Player c, Dictionary<string, object> args = null) { }

    public void UpdateBehaviour(Player c) { }

    public void FixedUpdateBehaviour(Player c)
    {
        if (!isGravityEnabled)
            return;

        if(c.playerRigidBody.velocity.y < 0 && !c.GroundCheck.IsCheckSphereGrounded)
            c.playerRigidBody.AddForce(Physics.gravity * GRAVITY_MULT, ForceMode.Acceleration);
        else
            c.playerRigidBody.AddForce(Physics.gravity, ForceMode.Acceleration);
    }
    public void EndBehaviours(Player c) { }
}