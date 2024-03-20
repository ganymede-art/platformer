using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class FilmHighLogicState : IState<StateHighLogic, HighLogicStateId>
{
    public HighLogicStateId StateId => HighLogicStateId.Film;

    public void BeginState(StateHighLogic controller, Dictionary<string, object> args = null)
    {
        UserInterfaceHighLogic.G.FilmUserInterface.BeginUserInterface();

        Shader.SetGlobalFloat(SHADER_PROPERTY_NAME_STATE_SPEED, 1.0F);
    }

    public void EndState(StateHighLogic controller)
    {
        UserInterfaceHighLogic.G.FilmUserInterface.EndUserInterface();
    }

    public void FixedUpdateState(StateHighLogic controller)
    {
    }

    public void UpdateState(StateHighLogic controller)
    {
    }
}
