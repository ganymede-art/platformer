using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using UnityEngine.Serialization;

public class GameDefaultsController : MonoBehaviour
{
    private static GameDefaultsController global;
    public static GameDefaultsController Global
    {
        get
        {
            if (global == null)
            {
                global = GameMasterController.Global.defaultsController;
            }
            return global;
        }
    }

    [FormerlySerializedAs("defaultAttributeGroundData")]
    public GroundData defaultGroundData;
    public DamageData defaultDamageData;
}
