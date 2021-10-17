using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.script.enemy;
using static Assets.script.enemy.EnemyStaticMethods;

namespace Assets.script.enemy
{
    public class EnemyStateDamageController : MonoBehaviour, IEnemyStateController
    {
        private float stateLifetimeInterval = 0.0f;
        private float stateLifetimeTimer = 0.0f;

        public AudioClip sfxDamage;

        public EnemyStateType[] nextStates;

        public void BeginState(IEnemyCoreController ec)
        {
            stateLifetimeTimer = 0.0f;
            stateLifetimeInterval = 1.0f;

            ec.GetData().enemyAnimator.ResetAllAnimatorTriggers();
            ec.GetData().enemyAnimator.SetTrigger("actor_damage_up");

            // play alert sound.

            ec.GetData().audioSource.clip = sfxDamage;
            ec.GetData().audioSource.Play();

            // add a hop upwards for damage.

            ec.GetData().rigidBody.velocity = Vector3.zero;
            ec.GetData().rigidBody.AddForce(Vector3.up * 3, ForceMode.VelocityChange);

            // disable the enemy's
            // damage trigger while damaged.

            ec.GetData().enemyTriggerObject.SetActive(false);
        }

        public void CheckState(IEnemyCoreController ec)
        {

        }

        public void FinishState(IEnemyCoreController ec)
        {
            // re-enable the enemy's
            // damage trigger when finished.

            ec.GetData().enemyTriggerObject.SetActive(true);
        }

        public void FixedUpdateState(IEnemyCoreController ec)
        {
            return;
        }

        public EnemyStateType GetStateType()
        {
            return EnemyStateType.damage;
        }

        public void UpdateState(IEnemyCoreController ec)
        {
            stateLifetimeTimer += Time.deltaTime;

            if (stateLifetimeTimer >= stateLifetimeInterval)
            {
                ec.ChangeState(GetRandomNextStateType(nextStates));
            }
        }

        public void UpdateStateAnimator(IEnemyCoreController ec)
        {
            return;
        }
    }
}
