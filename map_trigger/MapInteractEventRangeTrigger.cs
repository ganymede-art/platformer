using Assets.script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MapInteractEventRangeTrigger : MonoBehaviour
{
    private GameMasterController master;
    private GameObject playerObject;
    private PlayerController playerController;

    private bool wasInRange = false;
    private bool isInRange = false;

    [Header("Interation Attributes")]
    [FormerlySerializedAs("interact_range")]
    public float interactRange = 1.0f;

    [Header("Event Attributes")]
    public GameObject gameEventTriggerObject;

    void Start()
    {
        master = GameMasterController.Global;
        playerObject = GameMasterController.GlobalPlayerObject;
        playerController = playerObject.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (master.gameState != GameState.Game)
            return;

        wasInRange = isInRange;

        isInRange = Vector3.Distance(this.transform.position, playerObject.transform.position) <= interactRange;

        if (!isInRange)
            return;

        if (playerController.isSpherecastGrounded
            && !master.inputController.wasInputWest
            && master.inputController.inInputWest)
        {
            if (gameEventTriggerObject == null)
            {
                Debug.LogError("Missing event trigger object.");
                return;
            }

            var triggerComponent = gameEventTriggerObject
                .GetComponent<GameEventTrigger>();

            if(triggerComponent == null)
            {
                Debug.LogError("Missing event trigger component.");
                return;
            }

            triggerComponent.StartGameEvent();
        }
    }
}
