using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyPlayerAbilitiesAction : MonoBehaviour, IAction
{
    public GameObject NextActionObject => nextActionObject;
    public ActionType ActionType => ActionType.ModifyPlayerAbilities;
    public string ActionName => ActionType.ToString();
    public bool IsActionComplete => true;
    public bool IsActionUpdateComplete => true;

    [Header("Action Attributes")]
    public GameObject nextActionObject;

    [Header("Power Attributes")]
    public bool canNowDoubleJump;
    public bool canNowAttack;
    public bool canNowDiveUnderwater;
    public bool canNowAttackUnderwater;
    public bool canNowLunge;
    public bool canNowSlam;
    public bool canNowHighJump;

    public void BeginAction(ActionSource actionSource)
    {
        if (canNowDoubleJump)
            PlayerHighLogic.G.ModifyCanDoubleJump(true);

        if (canNowAttack)
            PlayerHighLogic.G.ModifyCanAttack(true);

        if (canNowDiveUnderwater)
            PlayerHighLogic.G.ModifyCanDiveUnderwater(true);

        if (canNowAttackUnderwater)
            PlayerHighLogic.G.ModifyCanAttackUnderwater(true);

        if (canNowLunge)
            PlayerHighLogic.G.ModifyCanLunge(true);

        if (canNowSlam)
            PlayerHighLogic.G.ModifyCanSlam(true);

        if (canNowHighJump)
            PlayerHighLogic.G.ModifyCanHighJump(true);
    }

    public void EndAction(ActionSource actionSource) { }
    public void UpdateAction(ActionSource actionSource) { }
}
