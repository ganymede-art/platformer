using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using TMPro;
using System;

public class GameCutsceneController : MonoBehaviour
{
    const float EVENT_STEP_INTERVAL_DEFAULT = 0.04f;
    const float EVENT_STEP_INTERVAL_FAST = 0.02f;

    private GameMasterController master;

    float eventProcessInterval = 0.05f;

    public List<GameEvent> orderedEvents;
    public List<GameEvent> generalEvents;

    // properties.

    void Start()
    {
        master = GameMasterController.GlobalMasterController;

        orderedEvents = new List<GameEvent>();
        generalEvents = new List<GameEvent>();
    }

    void Update()
    {
        foreach(GameEvent orderedEvent in orderedEvents)
        {
            if (orderedEvent.gameState
                != GameMasterController.GlobalMasterController.gameState)
                continue;

            if (!orderedEvent.isStarted)
                StartGameEvent(orderedEvent);

            CheckGameEvent(orderedEvent);

            if (orderedEvent.isFinished)
            {
                FinishGameEvent(orderedEvent);
            }
            else
            {
                ProcessGameEvent(orderedEvent);
            }

            break;
        }

        foreach(GameEvent generalEvent in generalEvents)
        {
            if (generalEvent.gameState 
                != GameMasterController.GlobalMasterController.gameState)
                continue;

            if (!generalEvent.isStarted)
                StartGameEvent(generalEvent);

            CheckGameEvent(generalEvent);

            if(generalEvent.isFinished)
            {
                FinishGameEvent(generalEvent);
            }
            else
            {
                ProcessGameEvent(generalEvent);
            }
        }

        // remove any null events (i.e. they had no next events).
        orderedEvents.RemoveAll(oe => oe.controllerSource == null);
        generalEvents.RemoveAll(ge => ge.controllerSource == null);

        // end cutscene if no more cutscene events.
        if (GameMasterController.GlobalMasterController.gameState == GameState.Cutscene
            && orderedEvents.FindAll(oe => oe.gameState == GameState.Cutscene).Count == 0
            && generalEvents.FindAll(ge => ge.gameState == GameState.Cutscene).Count == 0)
            EndCutscene();
    }

    private void StartGameEvent(GameEvent gameEvent)
    {
        gameEvent.isStarted = true;

        gameEvent.runningTimer = 0.0f;
        gameEvent.processTimer = 0.0f;

        gameEvent.controller = gameEvent.controllerSource.GetComponent<IEventController>();
        gameEvent.controller.StartEvent(gameEvent);
    }

    private void ProcessGameEvent(GameEvent gameEvent)
    {
        gameEvent.runningTimer += Time.deltaTime;
        gameEvent.processTimer += Time.deltaTime;

        eventProcessInterval = (master.inputController.isInputInteract)
            ? EVENT_STEP_INTERVAL_FAST
            : EVENT_STEP_INTERVAL_DEFAULT;

        if(gameEvent.processTimer >= eventProcessInterval)
        {
            gameEvent.processTimer = 0.0f;
            gameEvent.controller.ProcessEvent(gameEvent);
        }
    }

    private void CheckGameEvent(GameEvent gameEvent)
    {
        gameEvent.isFinished = gameEvent.gameState == GameState.Cutscene
            ? gameEvent.controller.GetIsEventComplete(gameEvent)
            : gameEvent.controller.GetIsGameEventComplete(gameEvent);
    }

    private void FinishGameEvent(GameEvent gameEvent)
    {
        // reset the event.

        gameEvent.isStarted = false;
        gameEvent.isFinished = false;

        // run the finish method.

        gameEvent.controller.FinishEvent(gameEvent);

        // set the next event.

        gameEvent.previousControllerSource = gameEvent.controllerSource;
        gameEvent.previousController = gameEvent.controller;

        gameEvent.controllerSource = gameEvent.controller.GetNextEventSource();
        gameEvent.controller = null;
    }

    private void ResetGameEvent(GameEvent gameEvent)
    {
        gameEvent.isStarted = false;
        gameEvent.isFinished = false;

        gameEvent.runningTimer = 0.0f;
        gameEvent.processTimer = 0.0f;

        gameEvent.controller.ResetEvent(gameEvent);
    }

    public void AddOrderedGameEvent(GameEvent newEvent)
    {
        if (newEvent.gameState == GameState.Cutscene)
            StartCutscene();

        orderedEvents.Add(newEvent);
    }

    public void InsertOrderedGameEvent(GameEvent newEvent)
    {
        if (newEvent.gameState == GameState.Cutscene)
            StartCutscene();

        if (orderedEvents.Count == 0)
        {
            orderedEvents.Add(newEvent);
        }
        else
        {
            ResetGameEvent(orderedEvents[0]);
            orderedEvents.Insert(0, newEvent);
        }
    }

    public void AddGeneralGameEvent(GameEvent newEvent)
    {
        if (newEvent.gameState == GameState.Cutscene)
            StartCutscene();

        generalEvents.Add(newEvent);
    }

    public void StartCutscene()
    {
        if(master.gameState != GameState.Cutscene)
            master.ChangeState(GameState.Cutscene);
    }

    public void EndCutscene()
    {
        if(master.gameState != GameState.Game)
            master.ChangeState(GameState.Game);
    }
}
