using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using static Assets.script.GameConstants;

namespace Assets.script.actor 
{
    class ActorNpcController : MonoBehaviour
    {
        const float FACING_RANGE_MIN = 0.5F;
        const float FACING_RANGE_DEFAULT = 2.0F;

        const float FACING_SPEED_MIN = 0.1F;
        const float FACING_SPEED_DEFAULT = 5F;

        private GameObject playerObject;

        private Quaternion startingRotation;

        private Vector3 facingDirectionDelta;
        private Vector3 facingTargetPosition;

        private bool wasInRange = false;
        private bool isInRange = false;

        [Header("Facing Attributes")]
        [FormerlySerializedAs("facing_range")]
        public float facingRange;
        public float facingSpeed;

        private void Start()
        {
            if (facingRange < FACING_RANGE_MIN)
                facingRange = FACING_RANGE_DEFAULT;

            if (facingSpeed < FACING_SPEED_MIN)
                facingSpeed = FACING_SPEED_DEFAULT;

            playerObject = GameMasterController.GlobalPlayerObject;

            startingRotation = transform.rotation;
        }

        private void Update()
        {
            if (GameMasterController.Global.gameState == GAME_STATE_GAME)
            {
                UpdateFacing();
            }
        }

        private void UpdateFacing()
        {
            wasInRange = isInRange;

            isInRange = Vector3.Distance(this.transform.position, playerObject.transform.position) <= facingRange;

            if (isInRange)
            {
                facingTargetPosition = playerObject.transform.position;
                facingDirectionDelta = Vector3.RotateTowards
                    (transform.forward, facingTargetPosition - transform.position, facingSpeed * Time.deltaTime, 0);
                facingDirectionDelta.y = 0;
                transform.rotation = Quaternion.LookRotation(facingDirectionDelta);
            }
        }
    }
}
