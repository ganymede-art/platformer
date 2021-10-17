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
    public class EnemyStateDeathController : MonoBehaviour, IEnemyStateController
    {
        private float stateLifetimeInterval = 0.0f;
        private float stateLifetimeTimer = 0.0f;

        public AudioClip sfxDeath;

        public void BeginState(IEnemyCoreController ec)
        {
            stateLifetimeTimer = 0.0f;
            stateLifetimeInterval = 4.0f;

            ec.GetData().enemyAnimator.ResetAllAnimatorTriggers();
            ec.GetData().enemyAnimator.SetTrigger("actor_die");

            // play alert sound.

            ec.GetData().audioSource.clip = sfxDeath;
            ec.GetData().audioSource.Play();

            // disable the enemy's
            // damage trigger while dead.

            ec.GetData().enemyTriggerObject.SetActive(false);
        }

        public void CheckState(IEnemyCoreController ec)
        {

        }

        public void FinishState(IEnemyCoreController ec)
        {
        }

        public void FixedUpdateState(IEnemyCoreController ec)
        {
            return;
        }

        public EnemyStateType GetStateType()
        {
            return EnemyStateType.death;
        }

        public void UpdateState(IEnemyCoreController ec)
        {
            stateLifetimeTimer += Time.deltaTime;

            if (stateLifetimeTimer >= stateLifetimeInterval)
            {
                GameObject.Destroy(this.gameObject);
            }
        }

        public void UpdateStateAnimator(IEnemyCoreController ec)
        {
            return;
        }
    }
}
