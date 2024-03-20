using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class AssetsHighLogic : MonoBehaviour
{
    private GameObject playerPrefab;
    private GameObject camcorderPrefab;

    private GameObject playUserInterfacePrefab;
    private GameObject statUserInterfacePrefab;
    private GameObject loadUserInterfacePrefab;
    private GameObject filmUserInterfacePrefab;
    private GameObject menuUserInterfacePrefab;

    private Sprite[] keyItemSprites;

    public static AssetsHighLogic G => GameHighLogic.G?.AssetsHighLogic;

    public GameObject PlayerPrefab => playerPrefab;
    public GameObject CamcorderPrefab => camcorderPrefab;

    public GameObject PlayUserInterfacePrefab => playUserInterfacePrefab;
    public GameObject StatUserInterfacePrefab => statUserInterfacePrefab;
    public GameObject LoadUserInterfacePrefab => loadUserInterfacePrefab;
    public GameObject FilmUserInterfacePrefab => filmUserInterfacePrefab;
    public GameObject MenuUserInterfacePrefab => menuUserInterfacePrefab;

    public Sprite[] KeyItemSprites => keyItemSprites;

    private void Awake()
    {
        playerPrefab = Resources.Load<GameObject>(RESOURCE_PATH_PLAYER_PREFAB);
        camcorderPrefab = Resources.Load<GameObject>(RESOURCE_PATH_CAMCORDER_PREFAB);

        playUserInterfacePrefab = Resources.Load<GameObject>(RESOURCE_PATH_PLAY_USER_INTERFACE_PREFAB);
        statUserInterfacePrefab = Resources.Load<GameObject>(RESOURCE_PATH_STAT_USER_INTERFACE_PREFAB);
        loadUserInterfacePrefab = Resources.Load<GameObject>(RESOURCE_PATH_LOAD_USER_INTERFACE_PREFAB);
        filmUserInterfacePrefab = Resources.Load<GameObject>(RESOURCE_PATH_FILM_USER_INTERFACE_PREFAB);
        menuUserInterfacePrefab = Resources.Load<GameObject>(RESOURCE_PATH_MENU_USER_INTERFACE_PREFAB);

        keyItemSprites = Resources.LoadAll<Sprite>(RESOURCE_PATH_KEY_ITEM_SPRITES);
    }
}
