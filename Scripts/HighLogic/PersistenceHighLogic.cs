using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using static Constants;

public class PersistenceHighLogic : MonoBehaviour, IPersistenceLoadable
{
    // Consts.
    private const string SAVE_FOLDER = @"\kiwi";
    private const string SAVE_FILE_NAME = @"\save_data.yaml";

    // Data dictionaries.
    private Dictionary<string, bool> boolVariables;
    private Dictionary<string, int> intVariables;
    private Dictionary<string, string> stringVariables;

    public static PersistenceHighLogic G => GameHighLogic.G?.PersistenceHighLogic;

    // Events.
    public event EventHandler BoolVariablesChanged;
    public event EventHandler IntVariablesChanged;
    public event EventHandler StringVariablesChanged;

    private void Awake()
    {
        boolVariables = new Dictionary<string, bool>();
        intVariables = new Dictionary<string, int>();
        stringVariables = new Dictionary<string, string>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        // Remove any temporary variables.
        boolVariables.Keys
            .Where(x => x.StartsWith(PERSISTENCE_TEMP_VARIABLE_PREFIX))
            .ToList()
            .ForEach(x => boolVariables.Remove(x));
        intVariables.Keys
            .Where(x => x.StartsWith(PERSISTENCE_TEMP_VARIABLE_PREFIX))
            .ToList()
            .ForEach(x => intVariables.Remove(x));
        stringVariables.Keys
            .Where(x => x.StartsWith(PERSISTENCE_TEMP_VARIABLE_PREFIX))
            .ToList()
            .ForEach(x => stringVariables.Remove(x));
    }

    public void SetBoolVariable(string variableId, bool variableValue)
    {
        boolVariables[variableId] = variableValue;

        var handler = BoolVariablesChanged;
        if (handler != null)
            handler(this, null);
    }

    public void SetIntVariable(string variableId, int variableValue)
    {
        intVariables[variableId] = variableValue;

        var handler = IntVariablesChanged;
        if (handler != null)
            handler(this, null);
    }

    public void SetStringVariable(string variableId, string variableValue)
    {
        stringVariables[variableId] = variableValue;

        var handler = StringVariablesChanged;
        if (handler != null)
            handler(this, null);
    }

    public bool GetBoolVariable(string variableId) => boolVariables.GetValueOrDefault(variableId);
    public void GetIntVariable(string variableId) => intVariables.GetValueOrDefault(variableId);
    public string GetStringVariable(string variableId) => stringVariables.GetValueOrDefault(variableId);

    public void SavePersistence(string sceneName, string sceneStartingTransformName)
    {
        var pi = new PersistenceInfo();

        pi.boolVariables = boolVariables;
        pi.intVariables = intVariables;
        pi.stringVariables = stringVariables;

        pi.scene = sceneName;
        pi.sceneStartingTransformName = sceneStartingTransformName;

        pi.health = PlayerHighLogic.G.Health;
        pi.maxHealth = PlayerHighLogic.G.MaxHealth;
        pi.oxygen = PlayerHighLogic.G.Oxygen;
        pi.maxOxygen = PlayerHighLogic.G.MaxOxygen;
        pi.ammo = PlayerHighLogic.G.Ammo;
        pi.maxAmmo = PlayerHighLogic.G.MaxAmmo;
        pi.money = PlayerHighLogic.G.Money;
        pi.maxMoney = PlayerHighLogic.G.MaxMoney;

        pi.heldPrimaryItemCount = PlayerHighLogic.G.HeldPrimaryItemCount;
        pi.heldSecondaryItemCount = PlayerHighLogic.G.HeldSecondaryItemCount;
        pi.heldTertiaryItemCount = PlayerHighLogic.G.HeldTertiaryItemCount;
        pi.heldQuaternaryItemCount = PlayerHighLogic.G.HeldQuaternaryItemCount;

        pi.canDoubleJump = PlayerHighLogic.G.CanDoubleJump;
        pi.canAttack = PlayerHighLogic.G.CanAttack;
        pi.canDiveUnderwater = PlayerHighLogic.G.CanDiveUnderwater;
        pi.canAttackUnderwater = PlayerHighLogic.G.CanAttackUnderwater;
        pi.canLunge = PlayerHighLogic.G.CanLunge;
        pi.canSlam = PlayerHighLogic.G.CanSlam;
        pi.canHighJump = PlayerHighLogic.G.CanHighJump;
        pi.canShoot = PlayerHighLogic.G.CanShoot;

        pi.collectedItemIds = PlayerHighLogic.G.CollectedItemIds;
        pi.collectedKeyItemIds = PlayerHighLogic.G.CollectedKeyItemIds;
        pi.heldKeyItemIds = PlayerHighLogic.G.HeldKeyItemIds;
        pi.selectedKeyItemId = PlayerHighLogic.G.SelectedKeyItemId;

        string yamlSaveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + SAVE_FOLDER;
        string yamlSavePath = yamlSaveDirectory + SAVE_FILE_NAME;

        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        var yaml = serializer.Serialize(pi);

        if (!Directory.Exists(yamlSaveDirectory))
            Directory.CreateDirectory(yamlSaveDirectory);

        if (!File.Exists(yamlSavePath))
            File.Create(yamlSavePath).Close();

        File.WriteAllText(yamlSavePath, yaml);
    }

    public void LoadPersistence()
    {
        string yamlSaveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + SAVE_FOLDER;
        string yamlSavePath = yamlSaveDirectory + SAVE_FILE_NAME;

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var pi = deserializer.Deserialize<PersistenceInfo>(File.ReadAllText(yamlSavePath));

        PersistenceHighLogic.G.LoadFromPersistence(pi);
        PlayerHighLogic.G.LoadFromPersistence(pi);

        var loadSceneArgs = new Dictionary<string, object>();
        loadSceneArgs.Add(LOAD_NEW_SCENE_ARG_STARTING_OBJECT_NAME, pi.sceneStartingTransformName);

        LoadSceneHighLogic.G.LoadNewScene(pi.scene, HighLogicStateId.Play, loadSceneArgs);
    }

    public void LoadFromPersistence(PersistenceInfo pi)
    {
        boolVariables = pi.boolVariables;
        intVariables = pi.intVariables;
        stringVariables = pi.stringVariables;
    }

    public class PersistenceInfo
    {
        // Persistence fields.
        public Dictionary<string, bool> boolVariables;
        public Dictionary<string, int> intVariables;
        public Dictionary<string, string> stringVariables;

        // Scene fields.
        public string scene;
        public string sceneStartingTransformName;

        // Time fields.
        public float hour;
        public int day;

        // Player fields.
        public int health;
        public int maxHealth;
        public int oxygen;
        public int maxOxygen;
        public int ammo;
        public int maxAmmo;
        public int money;
        public int maxMoney;

        public int heldPrimaryItemCount;
        public int heldSecondaryItemCount;
        public int heldTertiaryItemCount;
        public int heldQuaternaryItemCount;

        public bool canDoubleJump;
        public bool canAttack;
        public bool canDiveUnderwater;
        public bool canAttackUnderwater;
        public bool canLunge;
        public bool canSlam;
        public bool canHighJump;
        public bool canShoot;

        public List<string> collectedItemIds;
        public List<string> collectedKeyItemIds;
        public List<string> heldKeyItemIds;
        public string selectedKeyItemId;
    }
}
