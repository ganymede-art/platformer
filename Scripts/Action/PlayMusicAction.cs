using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicAction : MonoBehaviour, IAction
{
    // Public fields.
    [Header("Action Attributes")]
    public GameObject nextActionObject;

    [Header("Music Attributes")]
    public MusicData musicData;

    // Public properties.
    public GameObject NextActionObject => nextActionObject;
    public ActionType ActionType => ActionType.PlayMusic;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => true;
    public bool IsActionUpdateComplete => true;

    public void BeginAction(ActionSource actionSource)
    {
        if (musicData == null || musicData.musicAudioClip == null)
            MusicHighLogic.G.EndMusic();
        else
            MusicHighLogic.G.BeginMusic(musicData);
    }

    public void EndAction(ActionSource actionSource) { }
    public void UpdateAction(ActionSource actionSource) { }
}
