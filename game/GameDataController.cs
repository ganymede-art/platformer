using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Script;
using System.Linq;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using YamlDotNet;


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

    private string yamlSaveDirectory;
    private string yamlSavePath;

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

        yamlSaveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\kiwi";
        yamlSavePath = yamlSaveDirectory + @"\save_data.yaml";
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

    public void AppendGameVar(string key, string value)
    {
        if (gameVarString.ContainsKey(key))
            gameVarString[key] = gameVarString[key] + value;
        else
            gameVarString.Add(key, value);
    }

    public void UpdateItem(GameItemInfo item)
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

    public bool GetIsItemCollected(GameItemInfo item)
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
        saveInfo.canHighJump = master.playerController.canHighJump;
        saveInfo.canDive = master.playerController.canDive;
        saveInfo.canSwim = master.playerController.canSwim;
        saveInfo.canWaterJump = master.playerController.canWaterJump;
        saveInfo.canFlutter = master.playerController.canFlutter;
        saveInfo.canFireProjectile = master.playerController.canFireProjectile;
        saveInfo.canSlam = master.playerController.canSlam;

        Debug.Log("Saving data to: " + yamlSavePath);
        //string json_data = JsonUtility.ToJson(save_data, true);
        //string saveInfoJson = JsonConvert.SerializeObject(saveInfo,Formatting.Indented);

        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        var yaml = serializer.Serialize(saveInfo);

        if (!Directory.Exists(yamlSaveDirectory))
            Directory.CreateDirectory(yamlSaveDirectory);

        if (!File.Exists(yamlSavePath))
            File.Create(yamlSavePath).Close();

        File.WriteAllText(yamlSavePath, yaml);
    }

    public void LoadData()
    {
        //var save_data = JsonUtility.FromJson<SaveData>
        //    (File.ReadAllText(json_save_path));

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var saveInfo = deserializer.Deserialize<SaveInfo>(File.ReadAllText(yamlSavePath));

        //var saveInfo = JsonConvert.DeserializeObject<SaveInfo>
        //    (File.ReadAllText(jsonSavePath));

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
        master.playerController.canHighJump = saveInfo.canHighJump;
        master.playerController.canDive = saveInfo.canDive;
        master.playerController.canSwim = saveInfo.canSwim;
        master.playerController.canWaterJump = saveInfo.canWaterJump;
        master.playerController.canFlutter = saveInfo.canFlutter;
        master.playerController.canFireProjectile = saveInfo.canFireProjectile;
        master.playerController.canSlam = saveInfo.canSlam;

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
    public bool canHighJump;
    public bool canDive;
    public bool canSwim;
    public bool canWaterJump;
    public bool canFlutter;
    public bool canFireProjectile;
    public bool canSlam;
}

public class GameItemChangeEventArgs : EventArgs
{
    public GameItemInfo item;
}
