using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataController : MonoBehaviour
{
    public GameMasterController master;

    private Dictionary<string, bool> game_var_bool;
    private Dictionary<string, string> game_var_string;
    private Dictionary<string, int> game_var_int;


    private string json_save_directory;
    private string json_save_path;

    void Start()
    {
        master = this.GetComponentInParent<GameMasterController>();

        game_var_bool = new Dictionary<string, bool>();
        game_var_string = new Dictionary<string, string>();
        game_var_int = new Dictionary<string, int>();

        //json_save_path = Application.persistentDataPath + "/save_data.json";

        json_save_directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\kiwi";
        json_save_path = json_save_directory + @"\save_data.json";
    }

    public void UpdateGameVar(string key, bool value)
    {
        if (game_var_bool.ContainsKey(key))
            game_var_bool[key] = value;
        else
            game_var_bool.Add(key, value);
    }

    public void UpdateGameVar(string key, string value)
    {
        if (game_var_string.ContainsKey(key))
            game_var_string[key] = value;
        else
            game_var_string.Add(key, value);
    }

    public void UpdateGameVar(string key, int value)
    {
        if (game_var_int.ContainsKey(key))
            game_var_int[key] = value;
        else
            game_var_int.Add(key, value);
    }

    public bool GetGameVarBool(string key)
    {
        if (game_var_bool.ContainsKey(key))
            return game_var_bool[key];
        else
            return false;
    }

    public string GetGameVarString(string key)
    {
        if (game_var_string.ContainsKey(key))
            return game_var_string[key];
        else
            return string.Empty;
    }

    public int GetGameVarInt(string key)
    {
        if (game_var_int.ContainsKey(key))
            return game_var_int[key];
        else
            return 0;
    }

    public void SaveData(string player_start_transform_name, string camera_start_transform_name)
    {
        var save_data = new SaveData();

        save_data.game_var_bool = this.game_var_bool;
        save_data.game_var_string = this.game_var_string;
        save_data.game_var_int = this.game_var_int;

        save_data.load_scene_name = master.game_scene_data.scene_name;
        save_data.load_player_start_transform_name = player_start_transform_name;
        save_data.load_camera_start_transform_name = camera_start_transform_name;

        save_data.player_health = master.player_controller.player_health;
        save_data.player_max_health = master.player_controller.player_max_health;

        Debug.Log("Saving data to: " + json_save_path);
        string json_data = JsonUtility.ToJson(save_data, true);

        if (!Directory.Exists(json_save_directory))
            Directory.CreateDirectory(json_save_directory);

        if (!File.Exists(json_save_path))
            File.Create(json_save_path).Close();

        File.WriteAllText(json_save_path, json_data);
    }

    public void LoadData()
    {
        var save_data = JsonUtility.FromJson<SaveData>
            (File.ReadAllText(json_save_path));

        this.game_var_bool = save_data.game_var_bool;
        this.game_var_string = save_data.game_var_string;
        this.game_var_int = save_data.game_var_int;

        master.player_controller.player_health = save_data.player_health;
        master.player_controller.player_max_health = save_data.player_max_health;

        master.load_level_controller.StartLoadLevel(
            save_data.load_scene_name, 
            save_data.load_player_start_transform_name, 
            save_data.load_camera_start_transform_name);

    }
}

public class SaveData
{
    public Dictionary<string, bool> game_var_bool;
    public Dictionary<string, string> game_var_string;
    public Dictionary<string, int> game_var_int;

    public string load_scene_name;
    public string load_player_start_transform_name;
    public string load_camera_start_transform_name;

    public int player_health;
    public int player_max_health;
}
