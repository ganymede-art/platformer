using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Script;
using static Assets.Script.GameConstants;

public class PlayerBehaviourRepel : MonoBehaviour, IPlayerBehaviour
{
    // repel variables.

    [NonSerialized] public GameObject repelSourceObject = null;
    [NonSerialized] public DamageData repelData = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GameConstants.TAG_REPEL_SOURCE
            || other.gameObject.tag == GameConstants.TAG_INDIRECT_REPEL_SOURCE)
        {
            HandleRepelObject(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == GameConstants.TAG_REPEL_SOURCE
            || other.gameObject.tag == GameConstants.TAG_INDIRECT_REPEL_SOURCE)
        {
            HandleRepelObject(other.gameObject);
        }
    }

    private void HandleRepelObject(GameObject repelObject)
    {
        // get the objects repel attributes (or default)
        // the handle moving into the repel state.

        repelSourceObject = repelObject.gameObject;
        repelData = repelObject.gameObject.GetComponent<DamageDataController>()?.damageData;
        if (repelData == null)
            repelData = GameDefaultsController.Global.defaultDamageData;

        if (GameMasterController.GlobalPlayerController.currentStateType != GameConstants.PLAYER_STATE_REPEL)
            GameMasterController.GlobalPlayerController.ChangePlayerState(GameConstants.PLAYER_STATE_REPEL, repelObject, repelData);
    }

    public string GetBehaviourType()
    {
        return PLAYER_BEHAVIOUR_REPEL;
    }
}
