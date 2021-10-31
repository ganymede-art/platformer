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
    [FormerlySerializedAs("event_source")]
    public GameObject eventSource;
    public GameState gameState = GameState.Cutscene;
    public bool isOrdered;
    public bool isPriority;

    void Start()
    {
        master = GameMasterController.GlobalMasterController;
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
            && !master.inputController.wasInputInteract
            && master.inputController.isInputInteract)
        {
            var gameEvent = new GameEvent(gameState,eventSource);

            if (isOrdered)
            {
                if(isPriority)
                    master.cutsceneController.InsertOrderedGameEvent(gameEvent);
                else
                    master.cutsceneController.AddOrderedGameEvent(gameEvent);
            }
            else
            {
                master.cutsceneController.AddGeneralGameEvent(gameEvent);
            }
        }
    }
}
