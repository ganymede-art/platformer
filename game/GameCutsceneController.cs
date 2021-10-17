using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using TMPro;

public class GameCutsceneController : MonoBehaviour
{
    const float EVENT_STEP_INTERVAL_DEFAULT = 0.04f;
    const float EVENT_STEP_INTERVAL_FAST = 0.02f;

    private GameMasterController master;

    [System.NonSerialized] public GameObject currentEventSource;
    [System.NonSerialized] public IEventController currentEvent;
    [System.NonSerialized] public IEventController previousEvent;

    float eventTimer = 0f;

    float eventProcessTimer = 0f;
    float eventProcessInterval = 0.05f;

    bool isCurrentEventStarted = false;
    bool isCurrentEventFinished = false;

    // properties.

    public string Previous_Event_Type
    {
        get => (previousEvent == null) ? GameConstants.EVENT_TYPE_NULL : previousEvent.GetEventType();
    }

    public string Current_Event_Type
    {
        get => (currentEvent == null) ? GameConstants.EVENT_TYPE_NULL : currentEvent.GetEventType();
    }

    public bool Is_Current_Event_Process_Complete
    {
        get => (currentEvent == null) ? false : currentEvent.GetIsProcessComplete();
    }

    void Start()
    {
        master = this.GetComponentInParent<GameMasterController>();
    }

    void Update()
    {
        if (master.gameState != GameState.Cutscene 
            && master.gameState != GameState.GameCutscene)
            return;

        if (currentEvent == null)
            return;

        // start event.
        if (!isCurrentEventStarted)
            StartEventItem();

        eventTimer += Time.deltaTime;
        eventProcessTimer += Time.deltaTime;

        eventProcessInterval = (master.input_controller.isInputInteract)
            ? EVENT_STEP_INTERVAL_FAST
            : EVENT_STEP_INTERVAL_DEFAULT;

        if (eventProcessTimer >= eventProcessInterval)
        {
            eventProcessTimer = 0.0f;

            // continue event.
            ProcessEventItem();
        }

        // finish event if possible.
        FinishEventItem();

        if(isCurrentEventFinished)
        {
            isCurrentEventStarted = false;
            isCurrentEventFinished = false;

            // move onto next event, or finish
            // when meeting right criteria.

            if (currentEvent.GetNextEventSource() == null)
            {
                // return to game if there are no more items.
                EndCutscene();
            }
            else
            {
                // start the next cutscene event.

                if(master.gameState == GameState.Cutscene)
                    StartCutscene(currentEvent.GetNextEventSource(), false);
                else if(master.gameState == GameState.GameCutscene)
                    StartCutscene(currentEvent.GetNextEventSource(), true);
            }
        }
    }

    private void StartEventItem()
    {
        eventTimer = 0.0f;
        eventProcessTimer = 0.0f;

        isCurrentEventStarted = true;

        currentEvent.StartEvent();
    }

    private void ProcessEventItem()
    {
        // handle the current event item.

        currentEvent.ProcessEvent();
    }
    
    private void FinishEventItem()
    {
        if (currentEvent == null)
            return;

        isCurrentEventFinished = (master.gameState == GameState.Cutscene)
            ? currentEvent.GetIsEventComplete()
            : currentEvent.GetIsGameEventComplete();
    }

    public void StartCutscene(GameObject event_source, bool is_game_cutscene)
    {
        if (!is_game_cutscene)
            master.ChangeState(GameState.Cutscene);
        else
            master.ChangeState(GameState.GameCutscene);

        this.previousEvent = currentEvent;

        this.currentEventSource = event_source;
        this.currentEvent = event_source.GetComponent<IEventController>();

        eventTimer = 0f;
        eventProcessTimer = 0f;
    }

    public void EndCutscene()
    {
        master.ChangeState(GameState.Game);
    }
}
