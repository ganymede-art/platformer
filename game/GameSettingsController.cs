using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class GameSettingsController : MonoBehaviour
{
    private static GameSettingsController global;
    public static GameSettingsController Global
    {
        get
        {
            if (global == null)
            {
                global = GameMasterController.Global.settingsController;
            }
            return global;
        }
    }

    const float VOLUME_DEFAULT = 1.0F;

    private string jsonSaveDirectory;
    private string jsonSavePath;

    [NonSerialized] public float volumeMaster;
    [NonSerialized] public float volumePlayer;
    [NonSerialized] public float volumeMob;
    [NonSerialized] public float volumeProp;
    [NonSerialized] public float volumeFootstep;
    [NonSerialized] public float volumeVox;
    [NonSerialized] public float volumeMusic;
    [NonSerialized] public float volumeAmbience;

    private void Start()
    {
        // save directory.

        jsonSaveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\kiwi";
        jsonSavePath = jsonSaveDirectory + @"\settings_data.json";

        // initialise the default settings.

        volumeMaster = VOLUME_DEFAULT;
        volumePlayer = VOLUME_DEFAULT;
        volumeMob = VOLUME_DEFAULT;
        volumeProp = VOLUME_DEFAULT;
        volumeFootstep = VOLUME_DEFAULT;
        volumeVox = VOLUME_DEFAULT;
        volumeMusic = VOLUME_DEFAULT;
        volumeAmbience = VOLUME_DEFAULT;

        // check if settings file exists,
        // create it if it does not.

        bool doesFileExist = System.IO.File.Exists(jsonSavePath);

        if(!doesFileExist)
        {
            SaveSettings();
        }
        else
        {
            LoadSettings();
        }

    }

    public void LoadSettings()
    {
        string settingsInfoJson = null;

        settingsInfoJson = File.ReadAllText(jsonSavePath);
        var settingsInfo = JsonUtility.FromJson<SettingsInfo>(settingsInfoJson);

        volumeMaster = settingsInfo.volumeMaster;
        volumePlayer = settingsInfo.volumePlayer;
        volumeMob = settingsInfo.volumeMob;
        volumeProp = settingsInfo.volumeProp;
        volumeFootstep = settingsInfo.volumeFootstep;
        volumeVox = settingsInfo.volumeVox;
        volumeMusic = settingsInfo.volumeMusic;
        volumeAmbience = settingsInfo.volumeAmbience;
    }

    public void SaveSettings()
    {
        var settingsInfo = new SettingsInfo();

        settingsInfo.volumeMaster = volumeMaster;
        settingsInfo.volumePlayer = volumePlayer;
        settingsInfo.volumeMob = volumeMob;
        settingsInfo.volumeProp = volumeProp;
        settingsInfo.volumeFootstep = volumeFootstep;
        settingsInfo.volumeVox = volumeVox;
        settingsInfo.volumeMusic = volumeMusic;
        settingsInfo.volumeAmbience = volumeAmbience;

        string settingsInfoJson = JsonUtility.ToJson(settingsInfo);

        if (!Directory.Exists(jsonSaveDirectory))
            Directory.CreateDirectory(jsonSaveDirectory);

        if (!File.Exists(jsonSavePath))
            File.Create(jsonSavePath).Close();

        File.WriteAllText(jsonSavePath, settingsInfoJson);
    }
}

[System.Serializable]
public class SettingsInfo
{
    public float volumeMaster;
    public float volumePlayer;
    public float volumeMob;
    public float volumeProp;
    public float volumeFootstep;
    public float volumeVox;
    public float volumeMusic;
    public float volumeAmbience;
}

