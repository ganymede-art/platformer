using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventController : MonoBehaviour
{
    const string PLAY_SOUND_TYPE_STEP = "step";

    public void PlaySound(string s)
    {
        if(s == PLAY_SOUND_TYPE_STEP)
        {
            GameMasterController.GlobalPlayerController.stepEffectController.PlayStepSound();
        }
    }
}
