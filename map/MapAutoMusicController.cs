using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;

public class MapAutoMusicController : MonoBehaviour
{
    GameMasterController master;
    public GameMusicData music_data;

    void Start()
    {
        master = GameMasterController.GetMasterController();
        master.audio_controller.PlayMusic(music_data);
        GameObject.Destroy(this);
    }
}
