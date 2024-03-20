using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class ButtonPressSound : MonoBehaviour
{
    public Button button;
    [FormerlySerializedAs("sound")]
    public AudioSource audioSource;
    public float minPitch;
    public float maxPitch;

    private void Start()
    {
        button.onClick.AddListener(OnButtonPress);
    }

    public void OnButtonPress()
    {
        audioSource.PlayPitchedOneShot
            (audioSource.clip
            , SettingsHighLogic.G.UserInterfaceVolume
            , minPitch
            , maxPitch);
    }
}
