using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.script.Event
{
    internal class EventMessageBoxController : MonoBehaviour, IEventController
    {
        // audio constants.

        private readonly int[][] VOX_INDICES =
        {
            new int[] { 4, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 76, 79, 81, 84, 88},
            new int[] { 3, 6, 9, 11, 14, 17, 21, 25, 27, 32, 35, 39, 43, 47, 51, 54, 59, 63, 66, 70, 73, 79, 81, 84 },
            new int[] { 1, 5, 10, 12, 15, 19, 21, 24, 29, 30, 34, 38, 40, 43, 48, 53, 58, 61, 66, 71, 74, 79, 82, 85 },
            new int[] { 2, 6, 11, 12, 14, 15, 22, 27, 29, 32, 36, 39, 41, 46, 53, 55, 57, 62, 67, 73, 78, 79, 84, 87 }
        };

        // core constants.

        private int GAME_CUTSCENE_DELAY_MULTIPLIER = 4;

        // core variables.

        private GameMasterController master;
        private string inputText = string.Empty; // original message text is not modified, only this.
        private string outputText = string.Empty;
        private char outputTextNextChar = char.MinValue;
        private int outputTextIndex = 0;
        private bool isQuestionAnsweredPositive = false;
        private int gameCutsceneDelayProcessCount = 0;
        

        // audio variables.

        private AudioSource audioSource;
        private AudioClip voxSound = null;
        private int voxSoundsIndex = 0;
        private int[] indicesToPlayVoxSound = new int[20];

        // public vars.

        [Header("Event Attributes")]
        [FormerlySerializedAs("next_event_source")]
        public GameObject nextEventSource = null;
        [FormerlySerializedAs("conditional_event_source")]
        public GameObject conditionalEventSource = null;

        [Header("Message Box Attributes")]
        [FormerlySerializedAs("message_icon")]
        public Sprite messageIcon = null;
        [FormerlySerializedAs("message_text")]
        public string messageText = string.Empty;
        [FormerlySerializedAs("is_question")]
        public bool isQuestion = false;

        [Header("Vox Attributes")]
        public AudioClip[] voxSounds;
        [FormerlySerializedAs("message_audio_pitch")]
        public float voxSoundPitch = 1.0f;

        [Header("Replacer Attributes")]
        [FormerlySerializedAs("message_text_replacer_array")]
        public ReplacerData[] messageTextReplacers; 

        private void Start()
        {
            master = GameMasterController.GlobalMasterController;
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

        public void StartEvent()
        {
            inputText = messageText;
            outputText = string.Empty;
            outputTextIndex = 0;

            master.userInterfaceController.ui_controller_message_box.SetMessageBox(messageIcon);

            gameCutsceneDelayProcessCount = 0;

            indicesToPlayVoxSound
                = VOX_INDICES[GameMasterController.staticRandom.Next(0, VOX_INDICES.Length-1)];

            // process message text.

            
            ProcessReplacements();
        }

        public void ProcessEvent()
        {
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
                else
                {
                    // handle regular char.

                    outputText += outputTextNextChar;
                }

                if (indicesToPlayVoxSound.Contains(outputTextIndex))
                {
                    // play vox for every nth letter.
                    PlayVox(inputText, outputTextIndex);
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

            master.userInterfaceController.ui_controller_message_box.UpdateMessageBox(outputText);
        }

        public bool GetIsEventComplete()
        {
            // if the button is pressed, and
            // reached the end of the message.

            if (isQuestion)
            {
                if (!master.inputController.wasInputPositive
                    && master.inputController.isInputPositive
                    && outputTextIndex == inputText.Length)
                {
                    audioSource.clip = master.audioController.a_message_box_positive;
                    audioSource.pitch = 1.0f;
                    audioSource.Play();

                    master.userInterfaceController.ui_controller_message_box.UnsetMessageBox();
                    isQuestionAnsweredPositive = true;
                    return true;
                }

                if (!master.inputController.wasInputNegative
                    && master.inputController.isInputNegative
                    && outputTextIndex == inputText.Length)
                {
                    audioSource.clip = master.audioController.a_message_box_negative;
                    audioSource.pitch = 1.0f;
                    audioSource.Play();

                    master.userInterfaceController.ui_controller_message_box.UnsetMessageBox();
                    isQuestionAnsweredPositive = false;
                    return true;
                }
            }
            else
            {
                if (!master.inputController.wasInputPositive
                    && master.inputController.isInputPositive
                    && outputTextIndex == inputText.Length)
                {
                    audioSource.clip = master.audioController.a_message_box_continue;
                    audioSource.pitch = 1.0f;
                    audioSource.Play();

                    master.userInterfaceController.ui_controller_message_box.UnsetMessageBox();
                    isQuestionAnsweredPositive = true;
                    return true;
                }
            }

            return false;
        }

        public void PlayVox(string text, int textIndex)
        {
            // cancel if no vox sounds.

            if (voxSounds == null || voxSounds.Length == 0)
                return;

            // work through the audio clips for this vox.

            if (voxSoundsIndex == voxSounds.Length)
                voxSoundsIndex = 0;

            // special index jumps to break up repetition.

            if (textIndex % 12 == 0)
                voxSoundsIndex = GameMasterController.staticRandom.Next(0, voxSounds.Length - 1);

            // get the clip.

            voxSound = voxSounds[voxSoundsIndex];
            voxSoundsIndex++;

            // play the clip.

            audioSource.clip = voxSound;
            audioSource.pitch = voxSoundPitch * UnityEngine.Random.Range(1.0f, 1.25f);
            audioSource.Play();

            return;
        }

        private void ProcessReplacements()
        {
            // get text replacements from the
            // dictionary of replacer objects
            // and apply these replacements
            // to the message text.

            if (messageTextReplacers == null)
                return;

            if (messageTextReplacers.Length == 0)
                return;

            foreach(var item in messageTextReplacers)
            {
                var replacer = item.replacerObject.GetComponent<IReplacerController>();
                inputText = inputText.Replace(item.replacementKey, replacer.GetReplacement());
            }
        }

        public bool GetIsProcessComplete()
        {
            return (outputTextIndex == inputText.Length);
        }

        public bool GetIsGameEventComplete()
        {
            return (outputTextIndex == inputText.Length)
                && (gameCutsceneDelayProcessCount >= outputText.Length * GAME_CUTSCENE_DELAY_MULTIPLIER);
        }

        public void FinishEvent()
        {
            master.userInterfaceController.ui_controller_message_box.UnsetMessageBox();
        }
    }
}