using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class DeadHighLogicState : IState<StateHighLogic, HighLogicStateId>
{
    public HighLogicStateId StateId => HighLogicStateId.Dead;

    public void BeginState(StateHighLogic controller, Dictionary<string, object> args = null)
    {
        Shader.SetGlobalFloat(SHADER_PROPERTY_NAME_STATE_SPEED, 1.0F);
    }

    public void EndState(StateHighLogic controller)
    {
        
    }

    public void FixedUpdateState(StateHighLogic controller)
    {
    }

    public void UpdateState(StateHighLogic controller)
    {
    }
}
