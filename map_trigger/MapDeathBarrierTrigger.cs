using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using static Assets.Script.GameConstants;

public class MapDeathBarrierTrigger : MonoBehaviour
{
    static readonly Vector3 PLAYER_RESTART_OFFSET = new Vector3(0.0F, 0.19F, 0.0F);

    public int restartDamageAmount;
    public Transform[] restartTransforms;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == NAME_PLAYER_COLLIDER)
        {
            Transform closestTransform = null;
            float closestDistance = float.MaxValue;

            foreach(Transform restartTransform in restartTransforms)
            {
                float thisDistance = Vector3.Distance
                    (GameMasterController.GlobalPlayerObject.transform.position, restartTransform.position);

                if(thisDistance < closestDistance)
                {
                    closestTransform = restartTransform;
                    closestDistance = thisDistance;
                }
            }

            GameMasterController.GlobalPlayerObject.transform.position 
                = closestTransform.position + PLAYER_RESTART_OFFSET;

            GameMasterController.GlobalPlayerController.behaviourDamage.SimpleDamage(restartDamageAmount);
        }
    }
}
