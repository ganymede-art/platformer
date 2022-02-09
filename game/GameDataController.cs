using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.script;
using System.Linq;
using Newtonsoft.Json;

public class GameDataController : MonoBehaviour
{
    private static GameDataController global;
    public static GameDataController Global
    {
        get
        {
            if (global == null)
            {
                global = GameMasterController.Global.dataController;
            }
            return global;
        }
    }

    public GameMasterController master;

    private Dictionary<string, bool> gameVarBool;
    private Dictionary<string, string> gameVarString;
    private Dictionary<string, int> gameVarInt;

    private List<int> collectedGameItemFlags;
    private Dictionary<string,int> gameItemCountByType;
    private Dictionary<string,int> gameItemTotalCountByType;
    private Dictionary<string,int> gameItemTotalCountByTypeAndGroup;

    public event EventHandler GameItemChange;
    private GameItemChangeEventArgs gameItemChangeEventArgs;

    private string jsonSaveDirectory;
    private string jsonSavePath;

    void Start()
    {
        master = this.GetComponentInParent<GameMasterController>();

        // core variables.

        gameVarBool = new Dictionary<string, bool>();
        gameVarString = new Dictionary<string, string>();
        gameVarInt = new Dictionary<string, int>();

        collectedGameItemFlags = new List<int>();
        gameItemCountByType = new Dictionary<string, int>();
        gameItemTotalCountByType = new Dictionary<string, int>();
        gameItemTotalCountByTypeAndGroup = new Dictionary<string, int>();

        // event args.

        gameItemChangeEventArgs = new GameItemChangeEventArgs();

        // save directory.

        jsonSaveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\kiwi";
        jsonSavePath = jsonSaveDirectory + @"\save_data.json";
    }

    public void UpdateGameVar(string key, bool value)
    {
        if (gameVarBool.ContainsKey(key))
            gameVarBool[key] = value;
        else
            gameVarBool.Add(key, value);
    }

    public void UpdateGameVar(string key, string value)
    {
        if (gameVarString.ContainsKey(key))
            gameVarString[key] = value;
        else
            gameVarString.Add(key, value);
    }

    public void UpdateGameVar(string key, int value)
    {
        if (gameVarInt.ContainsKey(key))
            gameVarInt[key] = value;
        else
            gameVarInt.Add(key, value);
    }

    public void UpdateItem(GameItemData item)
    {
        if(GetIsItemCollected(item))
            return;

        collectedGameItemFlags.Add(item.GetHashCode());

        int count = GetItemCountByType(item.type);
        count++;
        gameItemCountByType[item.type] = count;

        int total_count = GetItemTotalCountByType(item.type);
        total_count++;
        gameItemTotalCountByType[item.type] = total_count;

        int total_count_tg = GetItemTotalCountByTypeAndGroup(item.GetTypeAndGroup());
        total_count_tg++;
        gameItemTotalCountByTypeAndGroup[item.GetTypeAndGroup()] = total_count_tg;

        gameItemChangeEventArgs.item = item;
        EventHandler handler = GameItemChange;
        if (handler != null) handler(this, gameItemChangeEventArgs);
    }

    public int GetItemCountByType(string type)
    {
        if (!gameItemCountByType.ContainsKey(type))
            gameItemCountByType.Add(type, 0);

        return gameItemCountByType[type];
    }

    public int GetItemTotalCountByType(string type)
    {
        if (!gameItemTotalCountByType.ContainsKey(type))
            gameItemTotalCountByType.Add(type, 0);

        return gameItemTotalCountByType[type];
    }

    public int GetItemTotalCountByTypeAndGroup(string type_and_group)
    {
        if (!gameItemTotalCountByTypeAndGroup.ContainsKey(type_and_group))
            gameItemTotalCountByTypeAndGroup.Add(type_and_group, 0);

        return gameItemTotalCountByTypeAndGroup[type_and_group];
    }

    public bool GetGameVarBool(string key)
    {
        if (gameVarBool.ContainsKey(key))
            return gameVarBool[key];
        else
            return false;
    }

    public string GetGameVarString(string key)
    {
        if (gameVarString.ContainsKey(key))
            return gameVarString[key];
        else
            return string.Empty;
    }

    public int GetGameVarInt(string key)
    {
        if (gameVarInt.ContainsKey(key))
            return gameVarInt[key];
        else
            return 0;
    }

    public bool GetIsItemCollected(GameItemData item)
    {
        return collectedGameItemFlags.Contains(item.GetHashCode());
    }

    public void SaveData(string player_start_transform_name, string camera_start_transform_name)
    {
        var saveInfo = new SaveInfo();

        saveInfo.gameVarBool = this.gameVarBool;
        saveInfo.gameVarString = this.gameVarString;
        saveInfo.gameVarInt = this.gameVarInt;

        saveInfo.collectedGameItemFlags = collectedGameItemFlags;
        saveInfo.gameItemCountByType = gameItemCountByType;
        saveInfo.gameItemTotalCountByType = gameItemTotalCountByType;
        saveInfo.gameItemTotalCountByTypeAndGroup = gameItemTotalCountByTypeAndGroup;

        saveInfo.loadSceneName = SceneManager.GetActiveScene().name;
        saveInfo.loadPlayerStartTransformName = player_start_transform_name;
        saveInfo.loadCameraStartTransformName = camera_start_transform_name;

        saveInfo.playerHealth = master.playerController.health;
        saveInfo.playerMaxHealth = master.playerController.maxHealth;

        saveInfo.ammo = master.playerController.ammo;
        saveInfo.ammo = master.playerController.maxAmmo;

        saveInfo.canAttack = master.playerController.canAttack;
        saveInfo.canCrouchJump = master.playerController.canCrouchJump;
        saveInfo.canDive = master.playerController.canDive;
        saveInfo.canWaterDive = master.playerController.canWaterDive;
        saveInfo.canWaterJump = master.playerController.canWaterJump;
        saveInfo.canDoubleJump = master.playerController.canDoubleJump;
        saveInfo.canFireProjectile = master.playerController.canFireProjectile;

        Debug.Log("Saving data to: " + jsonSavePath);
        //string json_data = JsonUtility.ToJson(save_data, true);
        string saveInfoJson = JsonConvert.SerializeObject(saveInfo,Formatting.Indented);

        if (!Directory.Exists(jsonSaveDirectory))
            Directory.CreateDirectory(jsonSaveDirectory);

        if (!File.Exists(jsonSavePath))
            File.Create(jsonSavePath).Close();

        File.WriteAllText(jsonSavePath, saveInfoJson);
    }

    public void LoadData()
    {
        //var save_data = JsonUtility.FromJson<SaveData>
        //    (File.ReadAllText(json_save_path));

        var saveInfo = JsonConvert.DeserializeObject<SaveInfo>
            (File.ReadAllText(jsonSavePath));

        this.gameVarBool = saveInfo.gameVarBool;
        this.gameVarString = saveInfo.gameVarString;
        this.gameVarInt = saveInfo.gameVarInt;

        this.collectedGameItemFlags = saveInfo.collectedGameItemFlags;
        this.gameItemCountByType = saveInfo.gameItemCountByType;
        this.gameItemTotalCountByType = saveInfo.gameItemTotalCountByType;
        this.gameItemTotalCountByTypeAndGroup = saveInfo.gameItemTotalCountByTypeAndGroup;

        master.playerController.health = saveInfo.playerHealth;
        master.playerController.maxHealth = saveInfo.playerMaxHealth;

        master.playerController.ammo = saveInfo.ammo;
        master.playerController.maxAmmo = saveInfo.ammo;

        master.playerController.canAttack = saveInfo.canAttack;
        master.playerController.canCrouchJump = saveInfo.canCrouchJump;
        master.playerController.canDive = saveInfo.canDive;
        master.playerController.canWaterDive = saveInfo.canWaterDive;
        master.playerController.canWaterJump = saveInfo.canWaterJump;
        master.playerController.canDoubleJump = saveInfo.canDoubleJump;
        master.playerController.canFireProjectile = saveInfo.canFireProjectile;

        master.loadSceneController.StartLoadGameScene(
            saveInfo.loadSceneName, 
            saveInfo.loadPlayerStartTransformName, 
            saveInfo.loadCameraStartTransformName);

    }
}

public struct SaveInfo
{
    public Dictionary<string, bool> gameVarBool;
    public Dictionary<string, string> gameVarString;
    public Dictionary<string, int> gameVarInt;

    public List<int> collectedGameItemFlags;
    public Dictionary<string, int> gameItemCountByType;
    public Dictionary<string, int> gameItemTotalCountByType;
    public Dictionary<string, int> gameItemTotalCountByTypeAndGroup;

    public string loadSceneName;
    public string loadPlayerStartTransformName;
    public string loadCameraStartTransformName;

    public int playerHealth;
    public int playerMaxHealth;

    public int ammo;
    public int maxAmmo;

    public bool canAttack;
    public bool canCrouchJump;
    public bool canDive;
    public bool canWaterDive;
    public bool canWaterJump;
    public bool canDoubleJump;
    public bool canFireProjectile;
}

public class GameItemChangeEventArgs : EventArgs
{
    public GameItemData item;
}
