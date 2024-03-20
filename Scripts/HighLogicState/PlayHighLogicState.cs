using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using static Constants;

public class PlayHighLogicState : IState<StateHighLogic, HighLogicStateId>
{
    public HighLogicStateId StateId => HighLogicStateId.Play;

    public void BeginState(StateHighLogic controller, Dictionary<string, object> args = null)
    {
        UserInterfaceHighLogic.G.PlayUserInterface.BeginUserInterface();
        
        // If the previous state was load, display
        // the current scene info by enabling the
        // current scene pop up widget manuallly.
        if(StateHighLogic.G.PreviousState == HighLogicStateId.Load)
        {
            UserInterfaceHighLogic.G.PlayUserInterface.Widgets
                .First(x => x.WidgetType == UserInterfaceWidgetType.CurrentScenePopup)
                .BeginWidget();
        }

        Shader.SetGlobalFloat(SHADER_PROPERTY_NAME_STATE_SPEED, 1.0F);
    }

    public void EndState(StateHighLogic controller)
    {
        UserInterfaceHighLogic.G.PlayUserInterface.EndUserInterface();
    }

    public void FixedUpdateState(StateHighLogic controller)
    {
    }

    public void UpdateState(StateHighLogic controller)
    {
        if ( controller.StateTimer > INPUT_STATE_BLANK_INTERVAL
            && !InputHighLogic.G.WasStartPressed 
            && InputHighLogic.G.IsStartPressed)
            StateHighLogic.G.ChangeState(HighLogicStateId.Stat);
    }
}
