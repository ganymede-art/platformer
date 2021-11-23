using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using static Assets.script.PlayerConstants;

namespace Assets.script
{
    public class PlayerStateShootController : MonoBehaviour, IPlayerStateController
    {
        private GameObject projectileObject;
        private Rigidbody projectileRigidBody;
        public GameObject projectilePrefab;


        public void BeginState(PlayerController mc)
        {
            mc.playerAnimator.ResetAllAnimatorTriggers();
            mc.playerAnimator.SetTrigger("attack");

            // play attack sound.

            mc.audioSource.clip = mc.soundDive;
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
        }

        public void CheckState(PlayerController mc)
        {
            if (mc.stateUpdateCount >= 15)
            {
                mc.ChangePlayerState(PlayerStateType.playerDefault);
                return;
            }
        }

        public void FinishState(PlayerController mc)
        {
        }

        public void UpdateState(PlayerController mc)
        {
            mc.stateControllers[PlayerStateType.playerDefault].UpdateStateSpeed(mc);
        }

        public void UpdateStateAnimator(PlayerController mc)
        {
            
        }

        public void UpdateStateDragAndFriction(PlayerController mc)
        {
            mc.stateControllers[PlayerStateType.playerJump].UpdateStateDragAndFriction(mc);
        }

        public void UpdateStateSlide(PlayerController mc)
        {
            return;
        }

        public void UpdateStateSpeed(PlayerController mc)
        {
            mc.stateControllers[PlayerStateType.playerDefault].UpdateStateSpeed(mc);
        }

        public PlayerStateType GetStateType()
        {
            return PlayerStateType.PlayerShoot;
        }
    }
}
