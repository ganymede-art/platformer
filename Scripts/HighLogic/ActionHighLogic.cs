using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionHighLogic : MonoBehaviour
{
    // Private fields.

    private List<ActionSource> sequencedActions;
    private List<ActionSource> concurrentActions;

    // Public properties.
    public static ActionHighLogic G => GameHighLogic.G.ActionHighLogic;

    public List<ActionSource> SequencedActions => sequencedActions;
    public List<ActionSource> ConcurrentActions => concurrentActions;

    public bool IsSkipping => StateHighLogic.G.ActiveState == HighLogicStateId.Film && InputHighLogic.G.IsWestPressed;

    private void Awake()
    {
        sequencedActions = new List<ActionSource>();
        concurrentActions = new List<ActionSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        foreach (var actionSource in sequencedActions)
        {
            if (actionSource.actionHighLogicStateId != StateHighLogic.G.ActiveState)
                continue;

            if (actionSource.activeActionObject == null)
                continue;

            if (actionSource.actionStatus == ActionStatus.None)
                BeginAction(actionSource);

            if (actionSource.activeAction.IsActionComplete)
                actionSource.actionStatus = ActionStatus.Finished;

            if (actionSource.actionStatus == ActionStatus.Finished)
            {
                EndAction(actionSource);
            }
            else
            {
                UpdateAction(actionSource);
            }

            break;
        }

        foreach (var actionSource in concurrentActions)
        {
            if (actionSource.actionHighLogicStateId != StateHighLogic.G.ActiveState)
                continue;

            if (actionSource.activeActionObject == null)
                continue;

            if (actionSource.actionStatus == ActionStatus.None)
                BeginAction(actionSource);

            if (actionSource.activeAction.IsActionComplete)
                actionSource.actionStatus = ActionStatus.Finished;

            if(actionSource.actionStatus == ActionStatus.Finished)
            {
                EndAction(actionSource);
            }
            else
            {
                UpdateAction(actionSource);
            }
        }

        // Remove null actions.
        sequencedActions.RemoveAll(x => x.activeActionObject == null);
        concurrentActions.RemoveAll(x => x.activeActionObject == null);

        // End film state, if no more film state actions.
        if (StateHighLogic.G.ActiveState == HighLogicStateId.Film
            && sequencedActions.FindAll(x => x.actionHighLogicStateId == HighLogicStateId.Film).Count == 0
            && concurrentActions.FindAll(x => x.actionHighLogicStateId == HighLogicStateId.Film).Count == 0)
            EndFilmHighLogicState();
    }

    private void BeginAction(ActionSource actionSource)
    {
        actionSource.actionStatus = ActionStatus.Started;
        actionSource.actionTimer = 0.0F;
        actionSource.actionUpdateTimer = 0.0F;
        actionSource.activeAction = actionSource
            .activeActionObject
            .GetComponent<IAction>();
        actionSource.activeAction.BeginAction(actionSource);
    }

    private void UpdateAction(ActionSource actionSource)
    {
        actionSource.actionTimer += Time.deltaTime;
        actionSource.actionUpdateTimer += Time.deltaTime;
        actionSource.actionStatus = ActionStatus.Updating;
        actionSource.activeAction.UpdateAction(actionSource);
    }

    private void EndAction(ActionSource actionSource)
    {
        actionSource.actionStatus = ActionStatus.None;
        actionSource.activeAction.EndAction(actionSource);
        actionSource.previousActionObject = actionSource.activeActionObject;
        actionSource.previousAction = actionSource.activeAction;
        actionSource.activeActionObject = actionSource.activeAction.NextActionObject;
        actionSource.activeAction = null;
    }

    public void AddSequencedAction(ActionSource newActionSource)
    {
        foreach (var actionSource in sequencedActions)
            if (actionSource.actionId == newActionSource.actionId)
                return;

        if (newActionSource.actionHighLogicStateId == HighLogicStateId.Film)
            StartFilmHighLogicState();

        sequencedActions.Add(newActionSource);
    }

    public void AddConcurrentAction(ActionSource newActionSource)
    {
        foreach (var actionSource in concurrentActions)
            if (actionSource.actionId == newActionSource.actionId)
                return;

        if (newActionSource.actionHighLogicStateId == HighLogicStateId.Film)
            StartFilmHighLogicState();

        concurrentActions.Add(newActionSource);
    }

    private void StartFilmHighLogicState()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Film)
            StateHighLogic.G.ChangeState(HighLogicStateId.Film);
    }

    private void EndFilmHighLogicState()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play)
            StateHighLogic.G.ChangeState(HighLogicStateId.Play);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        sequencedActions.Clear();
        concurrentActions.Clear();
    }
}
