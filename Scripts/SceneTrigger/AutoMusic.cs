using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMusic : MonoBehaviour, INameable
{
    public MusicData musicData;

    public string GetName()
    {
        return $"AutoMusic{musicData?.name}";
    }

    void Start()
    {
        MusicHighLogic.G.BeginMusic(musicData);
    }
}
