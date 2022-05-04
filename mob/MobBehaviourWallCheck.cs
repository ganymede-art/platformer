using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Assets.Script.GameConstants;
using UnityEngine;
using Assets.Script;


public class MobBehaviourWallCheck : MonoBehaviour, IMobBehaviour
{
    private int raycastMask;

    [NonSerialized] public RaycastHit raycastHitInfo;
    [NonSerialized] public bool isRaycastHit;

    [Header("Behaviour Attributes")]
    public Transform directionTransform;
    public float raycastDistance;

    public string GetBehaviourType()
    {
        return MOB_BEHAVIOUR_WALL_CHECK;
    }

    private void Start()
    {
        raycastMask = gameObject.layer == GameConstants.LAYER_MOB ?
            MASK_MOB_IGNORES :
            MASK_NPC_IGNORES;

        StartCoroutine(PerformWallCheck());
    }

    private IEnumerator PerformWallCheck()
    {
        for (; ; )
        {
            isRaycastHit = Physics.Raycast
                (directionTransform.position, directionTransform.forward, out raycastHitInfo, raycastDistance, raycastMask);
            if (isRaycastHit)
                Debug.Log(" hit a wall...");
            yield return new WaitForSeconds(.1f);
        }
    }
}

