using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Script.GameConstants;

public class MapOxygenFillTrigger : MonoBehaviour
{
    private bool hasSound;
    private AudioSource audioSource;

    public GameObject audioSourceObject;

    private void Start()
    {
        hasSound = false;

        if(audioSourceObject != null)
        {
            hasSound = true;
            audioSource = audioSourceObject.GetComponent<AudioSource>();
            audioSource.volume = audioSource.volume * GameSettingsController.Global.volumeProp;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == NAME_PLAYER_COLLIDER)
        {
            if (hasSound)
                audioSource.Play();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.name == NAME_PLAYER_COLLIDER)
        {
            GamePlayerController.Global.ModifyPlayerOxygen(1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == NAME_PLAYER_COLLIDER)
        {
            if (hasSound)
                audioSource.Stop();
        }
    }
}
