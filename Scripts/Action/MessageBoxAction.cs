using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Constants;

public class MessageBoxAction : MonoBehaviour, IAction
{
    private const float PLAY_STATE_INTERVAL_PER_CHAR = 0.2F;

    private const float NEXT_CHAR_INTERVAL = 0.01F;
    private const float DELAYED_NEXT_CHAR_INTERVAL = 0.25F;
    private const string BLANKING_CHARS = @"<alpha=#00><noparse>";

    private const float MIN_NEXT_CHAR_PITCH = 0.8F;
    private const float MAX_NEXT_CHAR_PITCH = 1.2F;

    private static readonly char[] DELAY_CHARS = { ',','.','?','!' };

    // Private fields.
    private float nextCharInterval;
    private string outputText;
    private float nextCharTimer;
    private int blankingIndex;
    private bool isInsideTag;
    private Dictionary<string, object> messageBoxWidgetArgs;
    private MessageBoxWidget messageBoxWidget;
    private bool isUpdateComplete;
    private bool isComplete;
    
    // Public properties.
    public GameObject NextActionObject => nextActionObject;
    public ActionType ActionType => ActionType.MessageBox;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => isComplete;
    public bool IsActionUpdateComplete => isUpdateComplete;

    // Public fields.
    [Header("Action Attributes")]
    public GameObject nextActionObject;

    [Header("Message Box Attributes")]
    public string textId;
    public VoxData voxData;
    public GameObject[] replacerObjects;

    public void BeginAction(ActionSource actionSource)
    {
        outputText = TextsHighLogic.G.GetText(textId);

        // Apply zws to output text tags.
        outputText = outputText.Replace(">", ">\u200B");
        outputText = outputText.Replace("<", "\u200B<");

        // Apply replacers to output text.
        if(replacerObjects != null && replacerObjects.Length > 0)
        {
            var replacers = ActionStatics.GetReplacersFromObjects(replacerObjects);
            outputText = ActionStatics.GetTextWithDynamicReplacers(outputText, replacers);
        }

        // Setup message box.
        nextCharTimer = 0.0F;
        blankingIndex = 0;
        isInsideTag = false;

        messageBoxWidgetArgs = new Dictionary<string, object>();
        
        if(voxData != null && voxData.voxSprite != null)
            messageBoxWidgetArgs[WIDGET_ARG_MESSAGE_BOX_VOX_SPRITE] = voxData.voxSprite;

        // Find either the film widget, or play widget.
        if(actionSource.actionHighLogicStateId == HighLogicStateId.Film)
            messageBoxWidget = UserInterfaceHighLogic.G.FilmUserInterface.Widgets
                .FirstOrDefault(x => x.WidgetId == WIDGET_ID_MESSAGE_BOX)
                as MessageBoxWidget;
        else if(actionSource.actionHighLogicStateId == HighLogicStateId.Play)
            messageBoxWidget = UserInterfaceHighLogic.G.PlayUserInterface.Widgets
                .FirstOrDefault(x => x.WidgetId == WIDGET_ID_MESSAGE_BOX)
                as MessageBoxWidget;

        messageBoxWidget.BeginWidget(messageBoxWidgetArgs);

        PlayVoxSound();

        isComplete = false;
        isUpdateComplete = false;
    }

    public void UpdateAction(ActionSource actionSource)
    {
        // If in play mode, check the widget is visible (i.e. it's hidden after pause).
        if(actionSource.actionHighLogicStateId == HighLogicStateId.Play
            && messageBoxWidget.Status == UserInterfaceWidgetStatus.Disabled)
        {
            messageBoxWidgetArgs = new Dictionary<string, object>();
            if (voxData != null && voxData.voxSprite != null)
                messageBoxWidgetArgs[WIDGET_ARG_MESSAGE_BOX_VOX_SPRITE] = voxData.voxSprite;
            messageBoxWidget.BeginWidget(messageBoxWidgetArgs);
        }

        // Check for completion in play state.
        if(actionSource.actionHighLogicStateId == HighLogicStateId.Play
            && actionSource.actionTimer >= (outputText.Length * PLAY_STATE_INTERVAL_PER_CHAR)
            && isUpdateComplete)
        {
            isComplete = true;
        }

        // Check for completion in film state.
        if (actionSource.actionHighLogicStateId == HighLogicStateId.Film
            && !InputHighLogic.G.WasSouthPressed
            && InputHighLogic.G.IsSouthPressed
            && isUpdateComplete)
        {
            isComplete = true;
        }

        // Time up to next char display.
        nextCharTimer += Time.deltaTime;

        nextCharInterval = blankingIndex > 0 && DELAY_CHARS.Contains(outputText[blankingIndex-1]) 
            ? DELAYED_NEXT_CHAR_INTERVAL 
            : NEXT_CHAR_INTERVAL;

        if (nextCharTimer < nextCharInterval)
            return;

        nextCharTimer = 0.0F;

        if (blankingIndex < outputText.Length)
        {
            PROCESS_CHAR:
            if (!isInsideTag && outputText[blankingIndex] == '<')
                isInsideTag = true;

            if (isInsideTag && outputText[blankingIndex] == '>')
                isInsideTag = false;

            blankingIndex++;

            if (isInsideTag && blankingIndex < outputText.Length)
                goto PROCESS_CHAR;

            messageBoxWidget.nextCharAudioSource.PlayPitchedOneShot
                (messageBoxWidget.nextCharAudioSource.clip
                , SettingsHighLogic.G.UserInterfaceVolume
                , MIN_NEXT_CHAR_PITCH
                , MAX_NEXT_CHAR_PITCH);

            string blankingText = outputText.Insert(blankingIndex, BLANKING_CHARS);

            messageBoxWidgetArgs[WIDGET_ARG_MESSAGE_BOX_TEXT] = blankingText;
            messageBoxWidgetArgs[WIDGET_ARG_MESSAGE_BOX_IS_CONTINUE_PROMPT_ENABLED] = isUpdateComplete 
                && actionSource.actionHighLogicStateId == HighLogicStateId.Film; ;
            messageBoxWidget.RefreshWidget(messageBoxWidgetArgs);
        }
        else
        {
            isUpdateComplete = true;
            messageBoxWidgetArgs[WIDGET_ARG_MESSAGE_BOX_TEXT] = outputText;
            messageBoxWidgetArgs[WIDGET_ARG_MESSAGE_BOX_IS_CONTINUE_PROMPT_ENABLED] = isUpdateComplete 
                && actionSource.actionHighLogicStateId == HighLogicStateId.Film;
            messageBoxWidget.RefreshWidget(messageBoxWidgetArgs);
        }
    }

    public void EndAction(ActionSource actionSource)
    {
        messageBoxWidget.EndWidget();
        messageBoxWidget = null;
    }

    public void PlayVoxSound()
    {
        if (voxData == null 
            || voxData.voxSounds == null 
            || voxData.voxSounds.Length == 0)
            return;

        int voxIndex = Random.Range(0, voxData.voxSounds.Length);
        var voxSound = voxData.voxSounds[voxIndex];
        messageBoxWidget.voxAudioSource.PlayPitchedOneShot
            ( voxSound
            , SettingsHighLogic.G.UserInterfaceVolume
            , voxData.minPitch
            , voxData.maxPitch);
    }
}
