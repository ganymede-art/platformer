using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoActiveBoolVariable : MonoBehaviour
{
    private bool boolVariable;

    public GameObject setObject;
    public VariableIdConstant variableId;
    public bool isInverted;

    void Awake()
    {
        if (setObject == null)
            setObject = gameObject;

        boolVariable = PersistenceHighLogic.G.GetBoolVariable(variableId.VariableId);

        if (isInverted)
            setObject.SetActive(!boolVariable);
        else
            setObject.SetActive(boolVariable);

    }
}
