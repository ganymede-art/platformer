using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitHighLogicState : IState<StateHighLogic, HighLogicStateId>
{
    public HighLogicStateId StateId => HighLogicStateId.Init;

    public void BeginState(StateHighLogic controller, Dictionary<string, object> args = null) { }
    public void FixedUpdateState(StateHighLogic controller) { }
    public void UpdateState(StateHighLogic controller) { }
    public void EndState(StateHighLogic controller) { }
}
