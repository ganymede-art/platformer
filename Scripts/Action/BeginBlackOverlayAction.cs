using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using System.Linq;

public class BeginBlackOverlayAction : MonoBehaviour, IAction
{
    // Consts.
    private const float ACTION_INTERVAL = 1.0F;

    // Private fields.
    private bool isActionComplete;

    // Public properties.
    public GameObject NextActionObject => nextActionObject;
    public ActionType ActionType => ActionType.BeginBlackOverlay;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => isActionComplete;
    public bool IsActionUpdateComplete => isActionComplete;

    [Header("Action Attributes")]
    public GameObject nextActionObject;

    public void BeginAction(ActionSource actionSource)
    {
        var blackOverlayWidget = UserInterfaceHighLogic.G.FilmUserInterface.Widgets
                .FirstOrDefault(x => x.WidgetId == WIDGET_ID_BLACK_OVERLAY)
                as ColourLerpWidget;
        blackOverlayWidget.BeginWidget();
        isActionComplete = false;
    }

    public void UpdateAction(ActionSource actionSource)
    {
        if (actionSource.actionTimer > ACTION_INTERVAL)
            isActionComplete = true;
    }

    public void EndAction(ActionSource actionSource) { }
}
