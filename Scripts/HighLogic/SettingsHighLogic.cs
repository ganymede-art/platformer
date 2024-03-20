using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsHighLogic : MonoBehaviour
{
    public static SettingsHighLogic G => GameHighLogic.G?.SettingsHighLogic;

    public float XLookSensitivity => 1.0F;
    public float YLookSensitivity => 1.0F;
    public float ZLookSensititity => 1.0F;

    public float MasterVolume => 1.0F;
    public float PlayerVolume => 1.0F;
    public float MusicVolume => 1.0F;
    public float EnvironmentVolume => 1.0F;
    public float MobVolume => 1.0F;
    public float PropVolume => 1.0F;
    public float UserInterfaceVolume => 1.0F;

    public float ActionSpeedMultiplier = 1.0F;
}
