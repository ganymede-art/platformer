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
    private GameUserInterfaceController ui;

    [System.NonSerialized] public GameObject event_source;
    [System.NonSerialized] public IEventController current_event;
    [System.NonSerialized] public IEventController previous_event;

    float current_event_time = 0f;

    float current_event_step_time = 0f;
    float current_event_step_time_interval = 0.05f;

    bool is_current_event_item_started = false;
    bool is_current_event_item_finished = false;

    // text variables.

    public string message_box_text = string.Empty;

    // properties.

    public string Previous_Event_Type
    {
        get => (previous_event == null) ? GameConstants.EVENT_TYPE_NULL : previous_event.GetEventType();
    }

    public string Current_Event_Type
    {
        get => (current_event == null) ? GameConstants.EVENT_TYPE_NULL : current_event.GetEventType();
    }

    public bool Is_Current_Event_Process_Complete
    {
        get => (current_event == null) ? false : current_event.GetIsProcessComplete();
    }

    void Start()
    {
        master = this.GetComponentInParent<GameMasterController>();
        ui = master.user_interface_controller;
    }

    void Update()
    {
        if (master.game_state != GameState.Cutscene 
            && master.game_state != GameState.GameCutscene)
            return;

        if (current_event == null)
            return;

        // start event.
        if (!is_current_event_item_started)
            StartEventItem();

        current_event_time += Time.deltaTime;
        current_event_step_time += Time.deltaTime;

        current_event_step_time_interval = (master.input_controller.Is_Input_Interact)
            ? EVENT_STEP_INTERVAL_FAST
            : EVENT_STEP_INTERVAL_DEFAULT;

        if (current_event_step_time >= current_event_step_time_interval)
        {
            current_event_step_time = 0.0f;

            // continue event.
            ProcessEventItem();
        }

        // finish event if possible.
        FinishEventItem();

        if(is_current_event_item_finished)
        {
            is_current_event_item_started = false;
            is_current_event_item_finished = false;

            // move onto next event, or finish
            // when meeting right criteria.

            if (current_event.GetNextEventSource() == null)
            {
                // return to game if there are no more items.
                EndCutscene();
            }
            else
            {
                // start the next cutscene event.

                if(master.game_state == GameState.Cutscene)
                    StartCutscene(current_event.GetNextEventSource(), false);
                else if(master.game_state == GameState.GameCutscene)
                    StartCutscene(current_event.GetNextEventSource(), true);
            }
        }
    }

    private void StartEventItem()
    {
        current_event_time = 0.0f;
        current_event_step_time = 0.0f;

        is_current_event_item_started = true;

        current_event.StartEvent();
    }

    private void ProcessEventItem()
    {
        // handle the current event item.

        current_event.ProcessEvent();
    }
    
    private void FinishEventItem()
    {
        if (current_event == null)
            return;

        is_current_event_item_finished = (master.game_state == GameState.Cutscene)
            ? current_event.GetIsEventComplete()
            : current_event.GetIsGameEventComplete();
    }

    public void StartCutscene(GameObject event_source, bool is_game_cutscene)
    {
        if (!is_game_cutscene)
            master.ChangeState(GameState.Cutscene);
        else
            master.ChangeState(GameState.GameCutscene);

        this.previous_event = current_event;

        this.event_source = event_source;
        this.current_event = event_source.GetComponent<IEventController>();

        current_event_time = 0f;
        current_event_step_time = 0f;
    }

    public void EndCutscene()
    {
        master.ChangeState(GameState.Game);
    }
}
