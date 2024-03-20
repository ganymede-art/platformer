using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceHighLogic : MonoBehaviour
{
    private GameObject playUserInterfaceObject;
    private GameObject statUserInterfaceObject;
    private GameObject loadUserInterfaceObject;
    private GameObject filmUserInterfaceObject;
    private GameObject menuUserInterfaceObject;

    private PlayUserInterface playUserInterface;
    private StatUserInterface statUserInterface;
    private LoadUserInterface loadUserInterface;
    private FilmUserInterface filmUserInterface;
    private MenuUserInterface menuUserInterface;

    public static UserInterfaceHighLogic G => GameHighLogic.G.UserInterfaceHighLogic;

    public PlayUserInterface PlayUserInterface => playUserInterface;
    public StatUserInterface StatUserInterface => statUserInterface;
    public LoadUserInterface LoadUserInterface => loadUserInterface;
    public FilmUserInterface FilmUserInterface => filmUserInterface;
    public MenuUserInterface MenuUserInterface => menuUserInterface;

    private void Awake()
    {
        playUserInterfaceObject = Instantiate(AssetsHighLogic.G.PlayUserInterfacePrefab, transform);
        statUserInterfaceObject = Instantiate(AssetsHighLogic.G.StatUserInterfacePrefab, transform);
        loadUserInterfaceObject = Instantiate(AssetsHighLogic.G.LoadUserInterfacePrefab, transform);
        filmUserInterfaceObject = Instantiate(AssetsHighLogic.G.FilmUserInterfacePrefab, transform);
        menuUserInterfaceObject = Instantiate(AssetsHighLogic.G.MenuUserInterfacePrefab, transform);

        playUserInterface = playUserInterfaceObject.GetComponent<PlayUserInterface>();
        statUserInterface = statUserInterfaceObject.GetComponent<StatUserInterface>();
        loadUserInterface = loadUserInterfaceObject.GetComponent<LoadUserInterface>();
        filmUserInterface = filmUserInterfaceObject.GetComponent<FilmUserInterface>();
        menuUserInterface = menuUserInterfaceObject.GetComponent<MenuUserInterface>();

        playUserInterfaceObject.SetActive(false);
        statUserInterfaceObject.SetActive(false);
        loadUserInterfaceObject.SetActive(false);
        filmUserInterfaceObject.SetActive(false);
        menuUserInterfaceObject.SetActive(false);
    }
}
