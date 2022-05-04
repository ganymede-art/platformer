using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Script.GameConstants;
using Assets.Script;

public class MobBehaviourRaycastAlert : MonoBehaviour, IMobBehaviour
{
    const float RAYCAST_INTERVAL_MIN = 0.01f;
    const float RAYCAST_INTERVAL_DEFAULT = 0.1f;

    private MobController mobController;
    private GameObject playerObject;
    private Vector3 raycastDirection;

    [NonSerialized] public RaycastHit raycastHitInfo;
    [NonSerialized] public bool isRaycastHit;
    [NonSerialized] public bool isRaycastHitPlayer;
    [NonSerialized] public int raycastHitPlayerTicks;

    [Header("MobController Attributes")]
    public GameObject mobControllerObject;

    [Header("Behaviour Attributes")]
    public float raycastDistance;
    public float raycastInterval;

    [Header("State Attributes")]
    public string[] canBeAlertedStates;
    public int alertedTicks;
    public string alertedState;

    public string GetBehaviourType()
    {
        return MOB_BEHAVIOUR_RAYCAST_ALERT;
    }

    private void Start()
    {
        mobController = mobControllerObject.GetComponent<MobController>();
        playerObject = GameMasterController.GlobalPlayerObject;
        StartCoroutine(PerformAlertCheck());

        raycastHitPlayerTicks = 0;

        if (raycastInterval < RAYCAST_INTERVAL_MIN)
            raycastInterval = RAYCAST_INTERVAL_DEFAULT;
    }
    
    private IEnumerator PerformAlertCheck()
    {
        for (; ; )
        {
            // check if in game state.
            if (GameMasterController.Global.gameState != GAME_STATE_GAME)
                yield return null;

            // check if the mob is in a valid state.
            // if not, don't do anything.
            if(!canBeAlertedStates.Contains(mobController.currentState))
            {
                raycastHitPlayerTicks = 0;
                yield return null;
            }

            // raycast towards player and check
            // if they can be seen right now.
            raycastDirection = playerObject.transform.position - this.gameObject.transform.position;

            isRaycastHit = Physics.Raycast
                (gameObject.transform.position, raycastDirection, out raycastHitInfo, raycastDistance, GameConstants.MASK_EVERYTHING);


            if (isRaycastHit)
            {
                isRaycastHitPlayer = raycastHitInfo.collider.gameObject.layer == GameConstants.LAYER_PLAYER; 
            }
            else
            {
                isRaycastHitPlayer = false;  
            }

            if (isRaycastHitPlayer)
            {
                Debug.DrawRay(this.gameObject.transform.position, Vector3.up, Color.yellow);
                raycastHitPlayerTicks++;
            }
            else
            {
                raycastHitPlayerTicks = 0;
            }

            if(raycastHitPlayerTicks > alertedTicks)
            {
                raycastHitPlayerTicks = 0;
                mobController.ChangeState(alertedState);
            }
                
            yield return new WaitForSeconds(raycastInterval);
        }
    }
}
