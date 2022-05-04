using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Script.GameConstants;

public class GameStateRigidBodyController : MonoBehaviour
{

    private Vector3 storedVelocity;
    private Vector3 storedAngularVelocity;
    private Rigidbody rigidBodyComponent;


    [Header("Rigid Body Attributes")]
    public GameObject rigidBodyObject;

    private void Start()
    {
        if (rigidBodyObject == null)
            return;

        storedVelocity = Vector3.zero;
        storedAngularVelocity = Vector3.zero;
        rigidBodyComponent = rigidBodyObject.GetComponent<Rigidbody>();

        GameMasterController.Global.GameStateChange += OnGameStateChange;
    }

    private void OnDestroy()
    {
        if(GameMasterController.Global != null)
            GameMasterController.Global.GameStateChange -= OnGameStateChange;
    }

    private void OnGameStateChange(object sender, EventArgs e)
    {
        GameStateChangeEventArgs args = e as GameStateChangeEventArgs;

        if (args.gameState == GAME_STATE_GAME)
        {
            rigidBodyComponent.velocity = storedVelocity;
            rigidBodyComponent.angularVelocity = storedAngularVelocity;
            rigidBodyComponent.isKinematic = false;

        }
        else
        {
            storedVelocity = rigidBodyComponent.velocity;
            storedAngularVelocity = rigidBodyComponent.angularVelocity;
            rigidBodyComponent.isKinematic = true;
        }
    }
}

