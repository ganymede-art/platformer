using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using static Assets.Script.GameConstants;

namespace Assets.Script.Event
{
    internal class EventMessageBox : MonoBehaviour, IEventController
    {
        // core constants.

        private const int GAME_CUTSCENE_DELAY_MULTIPLIER = 4;

        // update constants.

        const float EVENT_STEP_INTERVAL_DEFAULT = 0.04F;
        const float EVENT_STEP_INTERVAL_FAST = 0.01F;

        // delay constants.

        private const char DELAY_CHAR = '@';
        private readonly char[] DELAY_PUNCTUATION = new char[] {',','.','?','!','-','(',')'};

        // core variables.

        private GameMasterController master;
        private string inputText = string.Empty;
        private string outputText = string.Empty;
        private char outputTextNextChar = char.MinValue;
        private int outputTextIndex = 0;
        private bool isQuestionAnsweredPositive = false;
        private int gameCutsceneDelayProcessCount = 0;

        // audio variables.

        private AudioSource audioSource;
        private AudioClip voxSound = null;
        private int voxSoundsIndex = 0;

        // delay variables.

        private int delayProcessCount = 0;

        // update variables.

        private float eventStepInterval = EVENT_STEP_INTERVAL_DEFAULT;

        // vox variables.

        private Sprite voxSprite = null;

        // public vars.

        [Header("Event Attributes")]
        [FormerlySerializedAs("next_event_source")]
        public GameObject nextEventSource = null;
        [FormerlySerializedAs("conditional_event_source")]
        public GameObject conditionalEventSource = null;

        [Header("Message Box Attributes")]
        [TextArea(5,100)]
        [FormerlySerializedAs("messageText")]
        public string templateText = string.Empty;
        [FormerlySerializedAs("is_question")]
        public bool isQuestion = false;

        [Header("Vox Attributes")]
        public VoxData voxData;

        [Header("Replacer Attributes")]
        [FormerlySerializedAs("messageTextReplacers")]
        public ReplacerData[] templateTextReplacers; 

        private void Start()
        {
            master = GameMasterController.Global;
            audioSource = this.gameObject.AddComponent<AudioSource>();
        }

        public GameObject GetNextEventSource()
        {
            if (isQuestion && isQuestionAnsweredPositive)
                return conditionalEventSource;

            return nextEventSource;
        }

        public string GetEventType()
        {
            if (isQuestion)
                return GameConstants.EVENT_TYPE_MESSAGE_BOX_QUESTION;

            return GameConstants.EVENT_TYPE_MESSAGE_BOX;
        }

        public string GetEventDescription()
        {
            if (isQuestion)
                return GameConstants.EVENT_TYPE_MESSAGE_BOX_QUESTION
                    + "_" + GetEventDescriptionMessageText();

            return GameConstants.EVENT_TYPE_MESSAGE_BOX
                + "_" + GetEventDescriptionMessageText();
        }

        private string GetEventDescriptionMessageText()
        {
            string formattedMessageText = new string(templateText.Where(char.IsLetterOrDigit).ToArray());
            formattedMessageText = formattedMessageText.ToLower();

            if (formattedMessageText.Length > 20)
                return formattedMessageText.Substring(0, 20);
            else
                return formattedMessageText;
        }

        public void StartEvent(GameEvent gameEvent)
        {
            // reset the text.

            inputText = templateText;
            outputText = string.Empty;
            outputTextIndex = 0;
            outputTextNextChar = char.MinValue;

            // get localised text.

            try
            {
                inputText = GameLocalisationController.Global.locs[templateText];
            }
            catch
            {
                Debug.Log($"[EventMessageBox] Couldn't resolve LOC: {templateText}");
            }

            // process message text.

            InitialiseInputText();

            // set the vox data.

            if (voxData != null && voxData.voxSprite != null)
                voxSprite = voxData.voxSprite;
            else
                voxSprite = null;

            master.userInterfaceController.uiControllerMessageBox.SetMessageBox
                (voxSprite, isQuestion,gameEvent.gameState == GAME_STATE_CUTSCENE);

            gameCutsceneDelayProcessCount = 0;
        }

        public void UpdateEvent(GameEvent gameEvent)
        {
            eventStepInterval = (GameInputController.Global.isInputWest)
                ? EVENT_STEP_INTERVAL_FAST
                : EVENT_STEP_INTERVAL_DEFAULT;

            if (gameEvent.processTimer < eventStepInterval)
                return;

            gameEvent.processTimer = 0.0F;

            // delay the process step.
            if (delayProcessCount > 0)
            {
                delayProcessCount -= 1;
                return;
            }

            if (outputTextIndex < inputText.Length)
            {
                outputTextNextChar = inputText[outputTextIndex];

                if (outputTextNextChar == '<')
                {
                    // handle tag.

                    outputText += outputTextNextChar;

                    while (outputTextNextChar != '>')
                    {
                        outputTextIndex++;
                        outputTextNextChar = inputText[outputTextIndex];
                        outputText += outputTextNextChar;
                    }
                }
                else if(outputTextNextChar == DELAY_CHAR)
                {
                    delayProcessCount += 10;
                }
                else
                {
                    // handle regular char.

                    outputText += outputTextNextChar;

                    // add delays for punctuation.

                    if (DELAY_PUNCTUATION.Contains(outputTextNextChar))
                        delayProcessCount += 10;

                    // play vox for every word.

                    if (outputTextIndex == 0 || outputText.EndsWith(" ") || outputText.EndsWith("<br>"))
                    {
                        PlayVox(inputText, outputTextIndex);
                    }

                    
                }

                // increment to next character.
                outputTextIndex++;
            }
            else
            {
                // count the processing steps after the
                // text output is complete.

                gameCutsceneDelayProcessCount++;
            }

            master.userInterfaceController.uiControllerMessageBox.UpdateMessageBox(outputText,GetIsUpdateComplete(gameEvent));
        }

        public bool GetIsEventComplete(GameEvent gameEvent)
        {
            if (gameEvent.gameState == GAME_STATE_CUTSCENE)
            {
                // if the button is pressed, and
                // reached the end of the message.

                if (isQuestion)
                {
                    if (!master.inputController.wasInputSouth
                        && master.inputController.isInputSouth
                        && outputTextIndex == inputText.Length)
                    {
                        master.userInterfaceController.uiControllerMessageBox.UnsetMessageBox();
                        isQuestionAnsweredPositive = true;
                        return true;
                    }

                    if (!master.inputController.wasInputEast
                        && master.inputController.isInputEast
                        && outputTextIndex == inputText.Length)
                    {
                        master.userInterfaceController.uiControllerMessageBox.UnsetMessageBox();
                        isQuestionAnsweredPositive = false;
                        return true;
                    }
                }
                else
                {
                    if (!master.inputController.wasInputSouth
                        && master.inputController.isInputSouth
                        && outputTextIndex == inputText.Length)
                    {
                        master.userInterfaceController.uiControllerMessageBox.UnsetMessageBox();
                        isQuestionAnsweredPositive = true;
                        return true;
                    }
                }

                return false;
            }
            else
            {
                return (outputTextIndex == inputText.Length)
                    && (gameCutsceneDelayProcessCount >= outputText.Length * GAME_CUTSCENE_DELAY_MULTIPLIER);
            }
        }

        public void PlayVox(string text, int textIndex)
        {
            // cancel if no vox sounds.

            if(voxData == null 
                || voxData.voxSounds == null 
                || voxData.voxSounds.Length == 0)
                return;

            // work through the audio clips for this vox.

            if (voxSoundsIndex == voxData.voxSounds.Length)
                voxSoundsIndex = 0;

            // special index jumps to break up repetition.

            if (textIndex % 12 == 0)
                voxSoundsIndex = GameMasterController.staticRandom.Next(0, voxData.voxSounds.Length - 1);

            // get the clip.

            voxSound = voxData.voxSounds[voxSoundsIndex];
            voxSoundsIndex++;

            // play the clip.

            audioSource.clip = voxSound;
            audioSource.pitch = UnityEngine.Random.Range
                (voxData.minVoxSoundPitch, voxData.maxVoxSoundPitch);
            audioSource.Play();

            return;
        }

        private void InitialiseInputText()
        {
            // get text replacements from the
            // dictionary of replacer objects
            // and apply these replacements
            // to the message text.

            if (templateTextReplacers == null)
                return;

            if (templateTextReplacers.Length == 0)
                return;

            foreach(var item in templateTextReplacers)
            {
                var replacer = item.replacerObject.GetComponent<IReplacerController>();
                inputText = inputText.Replace(item.replacementKey, replacer.GetReplacement());
            }
        }

        public bool GetIsUpdateComplete(GameEvent gameEvent)
        {
            return (outputTextIndex == inputText.Length);
        }


        public void FinishEvent(GameEvent gameEvent)
        {
            master.userInterfaceController.uiControllerMessageBox.UnsetMessageBox();
        }

        public void ResetEvent(GameEvent gameEvent) { }

        private void OnDrawGizmos()
        {
            EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource);
        }
    }

    
}