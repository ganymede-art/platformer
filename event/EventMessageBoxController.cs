using System.Linq;
using UnityEngine;

namespace Assets.script.Event
{
    internal class EventMessageBoxController : MonoBehaviour, IEventController
    {
        // audio constants.

        private readonly int[] VOX_INDICES_1 = { 4, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 76 };
        private readonly int[] VOX_INDICES_2 = { 3, 6, 9, 11, 14, 17, 21, 25, 27, 32, 35, 39, 43, 47, 51, 54, 59, 63, 66, 70 };

        // core constants.

        private int GAME_CUTSCENE_DELAY_MULTIPLIER = 4;

        // core variables.

        private GameMasterController master;
        public GameObject next_event_source = null;

        public string message_icon = "default";
        public string message_audio_vox = "default";
        public float message_audio_pitch = 1.0f;
        public string message_text = string.Empty;

        private string output_text = string.Empty;
        private char output_next_char = char.MinValue;
        private int output_text_index = 0;

        public bool is_question = false;
        private bool is_question_answered_positive = false;
        public GameObject next_event_source_answered_negative = null;

        private int game_cutscene_delay_process_count = 0;

        // audio variables.

        private AudioSource audio_source;

        private AudioClip vox_clip = null;
        private AudioClip[] vox_clip_array = null;
        private int vox_clip_array_index = 0;
        private int[] play_vox_on_index_array = new int[20];

        // rng.

        private System.Random sys_random;

        private void Start()
        {
            master = GameMasterController.GetMasterController();
            audio_source = this.gameObject.AddComponent<AudioSource>();

            sys_random = new System.Random();
        }

        public GameObject GetNextEventSource()
        {
            if (is_question && !is_question_answered_positive)
                return next_event_source_answered_negative;

            return next_event_source;
        }

        public string GetEventType()
        {
            if (is_question)
                return GameConstants.EVENT_TYPE_MESSAGE_BOX_QUESTION;

            return GameConstants.EVENT_TYPE_MESSAGE_BOX;
        }

        public void StartEvent()
        {
            output_text = string.Empty;
            output_text_index = 0;
            master.user_interface_controller.ui_controller_message_box.SetMessageBox(message_icon);

            game_cutscene_delay_process_count = 0;

            play_vox_on_index_array = (sys_random.Next(0, 1) == 0)
                ? VOX_INDICES_1
                : VOX_INDICES_2;
        }

        public void ProcessEvent()
        {
            if (output_text_index < message_text.Length)
            {
                output_next_char = message_text[output_text_index];

                if (output_next_char == '<')
                {
                    // handle tag.

                    output_text += output_next_char;

                    while (output_next_char != '>')
                    {
                        output_text_index++;
                        output_next_char = message_text[output_text_index];
                        output_text += output_next_char;
                    }
                }
                else
                {
                    // handle regular char.

                    output_text += output_next_char;
                }

                if (play_vox_on_index_array.Contains(output_text_index))
                {
                    // play vox for every nth letter.
                    PlayVox(message_audio_vox, message_text, output_text_index);
                }

                // increment to next character.
                output_text_index++;
            }
            else
            {
                // count the processing steps after the
                // text output is complete.

                game_cutscene_delay_process_count++;
            }

            master.cutscene_controller.message_box_text = output_text;
            master.user_interface_controller.ui_controller_message_box.UpdateMessageBox(output_text);
        }

        public bool GetIsEventComplete()
        {
            // if the button is pressed, and
            // reached the end of the message.

            if (is_question)
            {
                if (!master.input_controller.Was_Input_Positive
                    && master.input_controller.Is_Input_Positive
                    && output_text_index == message_text.Length)
                {
                    audio_source.clip = master.audio_controller.a_message_box_positive;
                    audio_source.pitch = 1.0f;
                    audio_source.Play();

                    master.user_interface_controller.ui_controller_message_box.UnsetMessageBox();
                    is_question_answered_positive = true;
                    return true;
                }

                if (!master.input_controller.Was_Input_Negative
                    && master.input_controller.Is_Input_Negative
                    && output_text_index == message_text.Length)
                {
                    audio_source.clip = master.audio_controller.a_message_box_negative;
                    audio_source.pitch = 1.0f;
                    audio_source.Play();

                    master.user_interface_controller.ui_controller_message_box.UnsetMessageBox();
                    is_question_answered_positive = false;
                    return true;
                }
            }
            else
            {
                if (!master.input_controller.Was_Input_Positive
                    && master.input_controller.Is_Input_Positive
                    && output_text_index == message_text.Length)
                {
                    audio_source.clip = master.audio_controller.a_message_box_continue;
                    audio_source.pitch = 1.0f;
                    audio_source.Play();

                    master.user_interface_controller.ui_controller_message_box.UnsetMessageBox();
                    is_question_answered_positive = true;
                    return true;
                }
            }

            return false;
        }

        public AudioClip PlayVox(string vox, string output_text, int output_text_index)
        {
            // work through the audio clips for this vox.

            vox_clip_array = master.audio_controller.vox_dictionary[vox];

            if (vox_clip_array_index == vox_clip_array.Length)
                vox_clip_array_index = 0;

            // special index jumps to break up repetition.

            if (output_text_index % 12 == 0)
                vox_clip_array_index = sys_random.Next(0, vox_clip_array.Length - 1);

            // get the clip.

            vox_clip = vox_clip_array[vox_clip_array_index];
            vox_clip_array_index++;

            // play the clip.

            audio_source.clip = vox_clip;
            audio_source.pitch = message_audio_pitch * UnityEngine.Random.Range(1.0f, 1.25f);
            audio_source.Play();

            return null;
        }

        public bool GetIsProcessComplete()
        {
            return (output_text_index == message_text.Length);
        }

        public bool GetIsGameEventComplete()
        {
            return (output_text_index == message_text.Length)
                && (game_cutscene_delay_process_count >= output_text.Length * GAME_CUTSCENE_DELAY_MULTIPLIER);
        }

        public void FinishEvent()
        {
            return;
        }
    }
}