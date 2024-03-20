using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class SelectSound : MonoBehaviour, ISelectHandler
{
    [FormerlySerializedAs("sound")]
    public AudioSource audioSource;
    public float minPitch;
    public float maxPitch;

    public void OnSelect(BaseEventData eventData)
    {
        audioSource.PlayPitchedOneShot
            (audioSource.clip
            , SettingsHighLogic.G.UserInterfaceVolume
            , minPitch
            , maxPitch);
    }
}
