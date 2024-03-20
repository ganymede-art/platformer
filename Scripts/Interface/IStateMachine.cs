using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachine<T,U>
    where T : class
    where U : System.Enum
{
    public float StateTimer { get; }
    public Dictionary<U, IState<T, U>> States { get; }
    public U ActiveState { get; }
    public U PreviousState { get; }
    public void ChangeState(U stateId, Dictionary<string, object> args = null);
}