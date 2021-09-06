using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAutoMusicController : MonoBehaviour
{
    GameMasterController master;
    public string music_name = string.Empty;
    public bool music_is_loop = true;

    void Start()
    {
        master = GameMasterController.GetMasterController();
        master.audio_controller.PlayMusic(music_name, music_is_loop);
        GameObject.Destroy(this);
    }
}
