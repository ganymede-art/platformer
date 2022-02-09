using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using static Assets.script.PlayerConstants;

namespace Assets.script
{
    public class PlayerStateShootController : MonoBehaviour, IPlayerState
    {
        private GameObject projectileObject;
        private Rigidbody projectileRigidBody;
        public GameObject projectilePrefab;


        public void BeginState(PlayerController mc, params object[] parameters)
        {
            mc.playerAnimator.ResetAllAnimatorTriggers();
            mc.playerAnimator.SetTrigger("attack");

            // decrement player ammo.

            GamePlayerController.Global.modifyPlayerAmmo(-1);

            // play attack sound.

            mc.audioSource.clip = mc.shootSound;
            mc.audioSource.Play();

            // zero out vertical velocity and add diving force.

            mc.rigidBody.velocity = new Vector3
                (mc.rigidBody.velocity.x, 0, mc.rigidBody.velocity.z);
            mc.rigidBody.velocity = new Vector3
                (0, 0, 0);

            mc.rigidBody.AddForce(Vector3.up, ForceMode.VelocityChange);

            // create the projectile.

            projectileObject = Instantiate(projectilePrefab, this.transform.position, this.transform.rotation);
            projectileRigidBody = projectileObject.GetComponent<Rigidbody>();
            projectileRigidBody.AddForce(mc.playerRenderer.transform.forward * 8, ForceMode.VelocityChange);
            projectileRigidBody.AddForce(Vector3.up * 3, ForceMode.VelocityChange);

            // apply friction.

            PlayerStaticMethods.ApplyStaticFriction(mc, DRAG_AIR, 0, PhysicMaterialCombine.Minimum);
        }

        public void CheckState(PlayerController mc)
        {
            if (mc.stateFixedUpdateCount >= 15)
            {
                mc.ChangePlayerState(GameConstants.PLAYER_STATE_DEFAULT);
                return;
            }
        }

        public void FinishState(PlayerController mc)
        {
        }

        public void FixedUpdateState(PlayerController mc)
        {
            PlayerStaticMethods.LimitSpeedTwoAxis(mc, MAX_SPEED_GROUNDED);
        }

        public void UpdateState(PlayerController mc)
        {
            
        }

        public string GetStateType()
        {
            return GameConstants.PLAYER_STATE_SHOOT;
        }
    }
}
