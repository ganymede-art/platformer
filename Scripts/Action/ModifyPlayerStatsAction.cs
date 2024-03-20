using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyPlayerStatsAction : MonoBehaviour, IAction
{
    public GameObject NextActionObject => nextActionObject;
    public ActionType ActionType => ActionType.ModifyPlayerStats;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => true;
    public bool IsActionUpdateComplete => true;

    [Header("Action Attributes")]
    public GameObject nextActionObject;

    [Header("Stat Attributes")]
    public bool doModifyMaxHealth;
    public int modifyMaxHealthAmount;
    [Space]
    public bool doModifyMaxOxygen;
    public int modifyMaxOxygenAmount;
    [Space]
    public bool doModifyMaxAmmo;
    public int modifyMaxAmmoAmount;
    [Space]
    public bool doModifyMaxMoney;
    public int modifyMaxMoneyAmount;

    public void BeginAction(ActionSource actionSource)
    {
        if (doModifyMaxHealth)
            PlayerHighLogic.G.ModifyMaxHealth(modifyMaxHealthAmount);
        if (doModifyMaxOxygen)
            PlayerHighLogic.G.ModifyMaxOxygen(modifyMaxOxygenAmount);
        if (doModifyMaxAmmo)
            PlayerHighLogic.G.ModifyMaxAmmo(modifyMaxAmmoAmount);
        if (doModifyMaxMoney)
            PlayerHighLogic.G.ModifyMaxMoney(modifyMaxMoneyAmount);
    }

    public void EndAction(ActionSource actionSource) { }
    public void UpdateAction(ActionSource actionSource) { }
}
