using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class LoadHighLogicState : IState<StateHighLogic, HighLogicStateId>
{
    public HighLogicStateId StateId => HighLogicStateId.Load;

    public void BeginState(StateHighLogic controller, Dictionary<string, object> args = null)
    {
        UserInterfaceHighLogic.G.LoadUserInterface.BeginUserInterface();

        Shader.SetGlobalFloat(SHADER_PROPERTY_NAME_STATE_SPEED, 0.0F);
    }

    public void EndState(StateHighLogic controller)
    {
        UserInterfaceHighLogic.G.LoadUserInterface.EndUserInterface();
    }

    public void FixedUpdateState(StateHighLogic controller)
    {
    }

    public void UpdateState(StateHighLogic controller)
    {
    }
}
