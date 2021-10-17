using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.script;

namespace Assets.script.enemy
{
    public enum EnemyStateType
    {
        empty,
        idle,
        alert,
        damage,
        death,
        returnHome,
        wander,
        hop,
        stampede
    }


    public struct EnemyCoreData
    {
        public GameObject rigidBodyObject;
        public Rigidbody rigidBody;

        public GameObject enemyRendererObject;
        public Renderer enemyRenderer;

        public GameObject sphereColliderObject;
        public SphereCollider sphereCollider;

        public Transform directionTransform;

        public GameObject enemyAnimatorObject;
        public Animator enemyAnimator;

        public GameObject enemyTriggerObject;
        public Collider enemyTrigger;

        public GameObject playerObject;
        public float distanceToPlayerObject;

        public bool isGrounded;
        public float groundedDistance;

        public AudioSource audioSource;

    }
}
