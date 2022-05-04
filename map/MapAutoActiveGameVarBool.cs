using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAutoActiveGameVarBool : MonoBehaviour
{
    private bool gameVarBool;

    public GameObject setObject;
    public string gameVarBoolName;
    public bool isInverted;

    void Awake()
    {
        if (setObject == null)
            setObject = gameObject;

        gameVarBool = GameDataController.Global.GetGameVarBool(gameVarBoolName);

        if (isInverted)
            setObject.SetActive(!gameVarBool);
        else
            setObject.SetActive(gameVarBool);
           
    }
}
