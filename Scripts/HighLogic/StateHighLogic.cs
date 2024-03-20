using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateHighLogic : MonoBehaviour, IStateMachine<StateHighLogic, HighLogicStateId>
{
    // Private fields.
    private float stateTimer;
    private Dictionary<HighLogicStateId, IState<StateHighLogic, HighLogicStateId>> states;
    private HighLogicStateId activeState;
    private HighLogicStateId previousState;

    // Public properties.
    public static StateHighLogic G => GameHighLogic.G?.StateHighLogic;

    public float StateTimer => stateTimer;
    public Dictionary<HighLogicStateId, IState<StateHighLogic, HighLogicStateId>> States => states;
    public HighLogicStateId ActiveState => activeState;
    public HighLogicStateId PreviousState => previousState;

    // Events.
    public event EventHandler HighLogicStateChanged;


    public void ChangeState(HighLogicStateId stateId, Dictionary<string, object> args = null)
    {
        states[activeState].EndState(this);
        previousState = activeState;
        activeState = stateId;
        stateTimer = 0.0F;
        states[activeState].BeginState(this, args);

        var handler = HighLogicStateChanged;
        if (handler != null)
            handler(this, null);
    }

    private void Awake()
    {
        stateTimer = 0.0F;
        activeState = HighLogicStateId.Init;
        previousState = HighLogicStateId.Init;

        states = new Dictionary<HighLogicStateId, IState<StateHighLogic, HighLogicStateId>>();

        var initState = new InitHighLogicState();
        var playState = new PlayHighLogicState();
        var filmState = new FilmHighLogicState();
        var loadState = new LoadHighLogicState();
        var statState = new StatHighLogicState();
        var deadState = new DeadHighLogicState();
        var menuState = new MenuHighLogicState();

        states.Add(initState.StateId, initState);
        states.Add(playState.StateId, playState);
        states.Add(filmState.StateId, filmState);
        states.Add(loadState.StateId, loadState);
        states.Add(statState.StateId, statState);
        states.Add(deadState.StateId, deadState);
        states.Add(menuState.StateId, menuState);

        ChangeState(activeState);
    }

    private void Update()
    {
        states[activeState].UpdateState(this);
        stateTimer += Time.deltaTime;
    }
}
