using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using static Assets.Script.GameConstants;

public class GameEventController : MonoBehaviour
{
    private static GameEventController global;
    public static GameEventController Global
    {
        get
        {
            if (global == null)
            {
                global = GameMasterController.Global.cutsceneController;
            }
            return global;
        }
    }

    private GameMasterController master;

    float eventProcessInterval = 0.05F;

    public List<GameEvent> orderedEvents;
    public List<GameEvent> generalEvents;

    // properties.

    void Start()
    {
        master = GameMasterController.Global;
        SceneManager.sceneLoaded += SceneLoaded;

        orderedEvents = new List<GameEvent>();
        generalEvents = new List<GameEvent>();
    }

    void Update()
    {
        foreach(GameEvent orderedEvent in orderedEvents)
        {
            if (orderedEvent.gameState
                != GameMasterController.Global.gameState)
                continue;

            if (orderedEvent.controllerSource == null)
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
                != GameMasterController.Global.gameState)
                continue;

            if (generalEvent.controllerSource == null)
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
        if (GameMasterController.Global.gameState == GAME_STATE_CUTSCENE
            && orderedEvents.FindAll(oe => oe.gameState == GAME_STATE_CUTSCENE).Count == 0
            && generalEvents.FindAll(ge => ge.gameState == GAME_STATE_CUTSCENE).Count == 0)
            EndCutscene();
    }

    private void StartGameEvent(GameEvent gameEvent)
    {
        gameEvent.isStarted = true;

        gameEvent.runningTimer = 0.0F;
        gameEvent.processTimer = 0.0F;

        gameEvent.controller = gameEvent.controllerSource.GetComponent<IEventController>();
        gameEvent.controller.StartEvent(gameEvent);
    }

    private void ProcessGameEvent(GameEvent gameEvent)
    {
        gameEvent.runningTimer += Time.deltaTime;
        gameEvent.processTimer += Time.deltaTime;

        gameEvent.controller.UpdateEvent(gameEvent);
    }

    private void CheckGameEvent(GameEvent gameEvent)
    {
        gameEvent.isFinished = 
            gameEvent.controller.GetIsEventComplete(gameEvent);
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

        gameEvent.runningTimer = 0.0F;
        gameEvent.processTimer = 0.0F;

        gameEvent.controller.ResetEvent(gameEvent);
    }

    public void AddOrderedGameEvent(GameEvent newEvent)
    {
        // check if event has already been added.
        foreach (var existingEvent in orderedEvents)
        {
            if (existingEvent.eventGuid == newEvent.eventGuid)
                return;
        }

        // start the cutscene if it's a cutscene event.
        if (newEvent.gameState == GAME_STATE_CUTSCENE)
            StartCutscene();

        // add the event.
        orderedEvents.Add(newEvent);
    }

    public void InsertOrderedGameEvent(GameEvent newEvent)
    {
        // check if event has already been added.
        foreach (var existingEvent in orderedEvents)
        {
            if (existingEvent.eventGuid == newEvent.eventGuid)
                return;
        }

        // start the cutscene if it's a cutscene event.
        if (newEvent.gameState == GAME_STATE_CUTSCENE)
            StartCutscene();

        // insert the event.
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
        // check if event has already been added.
        foreach (var existingEvent in generalEvents)
        {
            if (existingEvent.eventGuid == newEvent.eventGuid)
                return;
        }

        // start the cutscene if it's a cutscene event.
        if (newEvent.gameState == GAME_STATE_CUTSCENE)
            StartCutscene();

        // add the event.
        generalEvents.Add(newEvent);
    }

    public void StartCutscene()
    {
        if(master.gameState != GAME_STATE_CUTSCENE)
            master.ChangeState(GAME_STATE_CUTSCENE);
    }

    public void EndCutscene()
    {
        if(master.gameState != GAME_STATE_GAME)
            master.ChangeState(GAME_STATE_GAME);
    }

    public void SceneLoaded(Scene scene, LoadSceneMode load_scene_mode)
    {
        var orderedIndexesToRemove = new List<int>();
        var generalIndexesToRemove = new List<int>();

        for(int i = 0; i < orderedEvents.Count; i++)
        {
            if (orderedEvents[i].controllerSource == null)
                orderedIndexesToRemove.Add(i);
            Debug.Log("[GameEventController] Removed event: " + orderedEvents[i].eventGuid);
        }

        for (int i = 0; i < generalEvents.Count; i++)
        {
            if (generalEvents[i].controllerSource == null)
                generalIndexesToRemove.Add(i);
            Debug.Log("[GameEventController] Removed event: " + generalEvents[i].eventGuid);
        }

        foreach (int i in orderedIndexesToRemove)
            orderedEvents.RemoveAt(i);

        foreach (int i in generalIndexesToRemove)
            generalEvents.RemoveAt(i);
    }
}
