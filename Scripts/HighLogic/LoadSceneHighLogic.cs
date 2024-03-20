using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants;

public class LoadSceneHighLogic : MonoBehaviour
{
    private bool isLoading;
    private LoadingSceneStatus loadingStatus;
    private float loadingTimer;

    private string loadingNewSceneName;
    private string loadingPreviousSceneName;
    private HighLogicStateId loadingHighLogicStateId;
    private Dictionary<string, object> loadingArgs;

    public static LoadSceneHighLogic G => GameHighLogic.G?.LoadSceneHighLogic;

    public bool IsLoading => isLoading;

    private void Awake()
    {
        isLoading = false;
        loadingStatus = LoadingSceneStatus.NotLoading;
        loadingTimer = 0.0F;
    }

    private void Update()
    {
        if (!isLoading)
            return;

        if(loadingStatus == LoadingSceneStatus.BeginLoading)
        {
            // Second point of loading.

            loadingTimer += Time.deltaTime;

            if (loadingTimer >= LOADING_SCENE_INTERVAL)
            {
                SceneManager.sceneLoaded += SetupNewScene;
                SceneManager.LoadScene(loadingNewSceneName, LoadSceneMode.Single);
            }
        }
        else if(loadingStatus == LoadingSceneStatus.EndLoading)
        {
            // Fourth point of loading.

            loadingTimer += Time.deltaTime;

            if(loadingTimer >= LOADING_SCENE_INTERVAL)
            {
                StateHighLogic.G.ChangeState(loadingHighLogicStateId);

                loadingNewSceneName = null;
                loadingPreviousSceneName = null;
                loadingHighLogicStateId = HighLogicStateId.None;
                loadingArgs = null;

                isLoading = false;
                loadingStatus = LoadingSceneStatus.NotLoading;
                loadingTimer = 0.0F;
            }
        }
    }

    public void LoadNewScene(string sceneName, HighLogicStateId highLogicStateId, Dictionary<string,object> args = null)
    {
        // First point of loading.

        StateHighLogic.G.ChangeState(HighLogicStateId.Load);

        isLoading = true;
        loadingStatus = LoadingSceneStatus.BeginLoading;
        loadingTimer = 0.0F;

        loadingNewSceneName = sceneName;
        loadingPreviousSceneName = SceneManager.GetActiveScene().name;
        loadingHighLogicStateId = highLogicStateId;
        loadingArgs = args;

        if (loadingHighLogicStateId == HighLogicStateId.Play)
            LoadNewSceneInPlayMode();
    }

    private void SetupNewScene(Scene scene, LoadSceneMode loadSceneMode)
    {
        // Third point of loading.

        SceneManager.sceneLoaded -= SetupNewScene;

        if (loadingHighLogicStateId == HighLogicStateId.Play)
            SetupNewSceneInPlayMode();

        isLoading = true;
        loadingStatus = LoadingSceneStatus.EndLoading;
        loadingTimer = 0.0F;
    }

    private void LoadNewSceneInPlayMode()
    {
        TimeHighLogic.G.ModifyTime(TIME_HOUR_INCREMENT_ON_SCENE_CHANGE);
    }

    private void SetupNewSceneInPlayMode()
    {
        string startingObjectName = $"From{loadingPreviousSceneName}";

        if (loadingArgs != null && loadingArgs.ContainsKey(LOAD_NEW_SCENE_ARG_STARTING_OBJECT_NAME))
            startingObjectName = loadingArgs[LOAD_NEW_SCENE_ARG_STARTING_OBJECT_NAME].ToString();

        var startingObject = GameObject.Find(startingObjectName);

        if (startingObject == null)
            startingObject = GameObject.Find(TRANSFORM_NAME_DEFAULT_LOAD_TRANSFORM);

        if (startingObject == null)
            startingObject = new GameObject(TRANSFORM_NAME_DEFAULT_LOAD_TRANSFORM);

        var startingPosition = startingObject.transform.position 
            + LOADING_SCENE_STARTING_POSITION_OFFSET;
        var startingRotation = startingObject.transform.rotation;

        var playerPrefab = AssetsHighLogic.G.PlayerPrefab;
        var camcorderPrefab = AssetsHighLogic.G.CamcorderPrefab;

        var playerObject = Instantiate
            ( playerPrefab
            , startingPosition
            , startingRotation);
        playerObject.name = playerPrefab.name;
        
        var camcorderObject = Instantiate
            ( camcorderPrefab
            , startingPosition
            , startingRotation);
        camcorderObject.name = camcorderPrefab.name;

        // Set the camcorder target.
        var player = playerObject.GetComponent<Player>();
        var camcorder = camcorderObject.GetComponent<Camcorder>();
        camcorder.target = player.camcorderTarget;
    }
}