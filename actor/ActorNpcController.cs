using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.script.actor 
{
    class ActorNpcController : MonoBehaviour
    {
        const float FACING_RANGE_MIN = 0.5f;
        const float FACING_RANGE_DEFAULT = 2.0f;

        const float FACING_SPEED_MIN = 0.1f;
        const float FACING_SPEED_DEFAULT = 5f;

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
            if (GameMasterController.Global.gameState == GameState.Game)
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
