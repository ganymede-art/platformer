using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void ResetAllAnimatorTriggers(this Animator animator)
    {
        foreach (var trigger in animator.parameters)
        {
            if (trigger.type == AnimatorControllerParameterType.Trigger)
                animator.ResetTrigger(trigger.name);
        }
    }

    public static void PlayPitchedOneShot
        (this AudioSource audioSource
        , AudioClip clip
        , float? volumeScale = null
        , float? minPitch = null
        , float? maxPitch = null)
    {
        volumeScale ??= 1.0F;
        minPitch ??= 1.0F;
        maxPitch ??= 1.0F;

        audioSource.pitch = UnityEngine.Random.Range(minPitch.Value, maxPitch.Value);
        audioSource.PlayOneShot(clip, volumeScale.Value);
    }
}
