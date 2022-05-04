using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using UnityEngine.Serialization;

public class MapAutoMusic : MonoBehaviour
{
    public MusicData musicData;

    void Start()
    {
        GameAudioController.Global.PlayMusic(musicData);
        GameObject.Destroy(this);
    }
}
