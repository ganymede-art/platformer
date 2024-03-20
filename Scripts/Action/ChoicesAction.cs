using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static Constants;

public class ChoicesAction : MonoBehaviour, IAction
{
    // Private fields.
    private string outputText;
    private Dictionary<string, object> messageBoxWidgetArgs;
    private MessageBoxWidget messageBoxWidget;
    private ChoicesWidget choicesWidget;
    private int chosenIndex;
    private bool hasChosenIndex;

    // Public properties.
    public GameObject NextActionObject => choices[chosenIndex].nextActionObject;
    public ActionType ActionType => ActionType.Choices;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => hasChosenIndex;
    public bool IsActionUpdateComplete => hasChosenIndex;

    [Header("Message Box Attributes")]
    public string textId;
    public VoxData voxData;
    public GameObject[] replacerObjects;

    [Header("Choices Attributes")]
    public Choice[] choices;

    public void BeginAction(ActionSource actionSource)
    {
        outputText = TextsHighLogic.G.GetText(textId);

        if (replacerObjects != null && replacerObjects.Length > 0)
        {
            var replacers = ActionStatics.GetReplacersFromObjects(replacerObjects);
            outputText = ActionStatics.GetTextWithDynamicReplacers(outputText, replacers);
        }

        messageBoxWidgetArgs = new Dictionary<string, object>();

        if (voxData != null && voxData.voxSprite != null)
            messageBoxWidgetArgs[WIDGET_ARG_MESSAGE_BOX_VOX_SPRITE] = voxData.voxSprite;

        messageBoxWidgetArgs[WIDGET_ARG_MESSAGE_BOX_IS_CONTINUE_PROMPT_ENABLED] = false;
        messageBoxWidgetArgs[WIDGET_ARG_MESSAGE_BOX_TEXT] = outputText;

        messageBoxWidget = UserInterfaceHighLogic.G.FilmUserInterface.Widgets
            .FirstOrDefault(x => x.WidgetId == WIDGET_ID_MESSAGE_BOX)
            as MessageBoxWidget;

        messageBoxWidget.BeginWidget(messageBoxWidgetArgs);
        messageBoxWidget.RefreshWidget(messageBoxWidgetArgs);

        choicesWidget = UserInterfaceHighLogic.G.FilmUserInterface.Widgets
            .FirstOrDefault(x => x.WidgetId == WIDGET_ID_CHOICES) 
            as ChoicesWidget;

        choicesWidget.BeginWidget();

        foreach (var choice in choices)
        {
            string choiceText = TextsHighLogic.G.GetText(choice.textId);
        }

        chosenIndex = 0;
        hasChosenIndex = false;

        RefreshChoicesWidget();
    }

    public void UpdateAction(ActionSource actionSource)
    {
        if(!InputHighLogic.G.WasUpPressed
            && InputHighLogic.G.IsUpPressed
            && InputHighLogic.G.IsInputActive)
        {
            if (chosenIndex > 0)
                chosenIndex--;

            RefreshChoicesWidget();
            choicesWidget.navigateAudioSource.PlayPitchedOneShot
                ( choicesWidget.navigateAudioSource.clip
                , SettingsHighLogic.G.UserInterfaceVolume
                , 1.0F
                , 1.0F);
        }
        
        if(!InputHighLogic.G.WasDownPressed
            && InputHighLogic.G.IsDownPressed
            && InputHighLogic.G.IsInputActive)
        {
            if (chosenIndex < choices.Length-1)
                chosenIndex++;

            RefreshChoicesWidget();
            choicesWidget.navigateAudioSource.PlayPitchedOneShot
                (choicesWidget.navigateAudioSource.clip
                , SettingsHighLogic.G.UserInterfaceVolume
                , 1.0F
                , 1.0F);
        }

        if(!InputHighLogic.G.WasSouthPressed
            && InputHighLogic.G.IsSouthPressed
            && InputHighLogic.G.IsInputActive)
        {
            hasChosenIndex = true;
            choicesWidget.continueAudioSource.PlayPitchedOneShot
                (choicesWidget.continueAudioSource.clip
                , SettingsHighLogic.G.UserInterfaceVolume
                , 1.0F
                , 1.0F);
        }
    }

    public void EndAction(ActionSource actionSource)
    {
        messageBoxWidget.EndWidget();
        choicesWidget.EndWidget();
    }

    private void RefreshChoicesWidget()
    {
        // Construct choices string.

        var choicesBuilder = new StringBuilder();

        for (int i = 0; i < choices.Length; i++)
        {
            string text = TextsHighLogic.G.GetText(choices[i].textId);
            choicesBuilder.AppendLine($"{(chosenIndex == i ? ">" : string.Empty)} {text}");
        }

        var chosenText = TextsHighLogic.G.GetText(choices[chosenIndex].textId);

        var args = new Dictionary<string, object>();
        args[WIDGET_ARG_CHOICES_CHOICES_TEXT] = choicesBuilder.ToString();
        args[WIDGET_ARG_CHOICES_CHOICE_TEXT] = chosenText;

        choicesWidget.RefreshWidget(args);
    }

    [Serializable]
    public class Choice
    {
        public GameObject nextActionObject;
        public string textId;
    }
}
