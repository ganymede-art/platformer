using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class MenuHighLogicState : IState<StateHighLogic, HighLogicStateId>
{
    // Public properties.
    public HighLogicStateId StateId => HighLogicStateId.Menu;

    public void BeginState(StateHighLogic controller, Dictionary<string, object> args = null)
    {
        UserInterfaceHighLogic.G.MenuUserInterface.BeginUserInterface();
        MusicHighLogic.G.SetTargetDynamicVolume(MUS_TGT_DYN_VOL_MIN);

        Shader.SetGlobalFloat(SHADER_PROPERTY_NAME_STATE_SPEED, 0.0F);
    }

    public void EndState(StateHighLogic controller)
    {
        UserInterfaceHighLogic.G.MenuUserInterface.EndUserInterface();
        MusicHighLogic.G.SetTargetDynamicVolume(MUS_TGT_DYN_VOL_MAX);
    }

    public void FixedUpdateState(StateHighLogic controller)
    {
    }

    public void UpdateState(StateHighLogic controller)
    {
    }
}