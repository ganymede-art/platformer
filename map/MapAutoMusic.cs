using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.Serialization;

public class MapAutoMusic : MonoBehaviour
{
    GameMasterController master;
    [FormerlySerializedAs("music_data")]
    public GameMusicData musicData;

    void Start()
    {
        master = GameMasterController.GlobalMasterController;
        master.audioController.PlayMusic(musicData);
        GameObject.Destroy(this);
    }
}
