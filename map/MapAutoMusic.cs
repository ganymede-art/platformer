using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.Serialization;

public class MapAutoMusic : MonoBehaviour
{
    public GameMusicData musicData;

    void Start()
    {
        GameAudioController.Global.PlayMusic(musicData);
        GameObject.Destroy(this);
    }
}
