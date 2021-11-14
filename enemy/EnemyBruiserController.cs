using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.script;
using static Assets.script.enemy.EnemyConstants;
using UnityEngine.Serialization;
using static Assets.script.AttributeDataClasses;

namespace Assets.script.enemy
{
    class EnemyBruiserController : MonoBehaviour, IEnemyCoreController
    {
        private GameMasterController master;
        private EnemyCoreData data;

        private Rigidbody rigidBody = null;
        private Renderer enemyRenderer = null;
        private SphereCollider sphereCollider = null;
        private Animator enemyAnimator = null;
        private AudioSource audioSource = null;
        private Collider enemyTrigger = null;
        private ActorDamageEffectController damageEffect = null;

        private EnemyStateType previousStateType;
        private EnemyStateType currentStateType;
        private Dictionary<EnemyStateType, IEnemyStateController> states;

        private GameObject playerObject;
        private float distanceToPlayerObject;

        private float alertTimer = 0.0f;

        private float damageTimer = 0.0f;
        private bool isDamaged = false;

        [Header("Component References")]
        public GameObject rigidBodyObject;
        public GameObject enemyRendererObject;
        public GameObject sphereColliderObject;
        public GameObject enemyAnimatorObject;
        public GameObject audioSourceObject;
        public GameObject enemyTriggerObject;
        public GameObject damageEffectObject;

        public Transform directionTransform;

        [Header("Basic Attributes")]
        public int health;

        [Header("State Attributes")]
        public EnemyStateType defaultStateType;

        [Header("Alert Attributes")]
        public EnemyStateType alertStateType;
        public EnemyStateType[] stateTypesCanBeAlerted;
        public float distanceCanBeAlerted;
        public float alertInterval;

        [Header("Damage Attributes")]
        public EnemyStateType damageStateType;
        public EnemyStateType[] stateTypesCanBeDamaged;
        public bool canAlwaysBeDamaged;
        public float damageInterval;

        [Header("Death Attributes")]
        public bool isDead;
        public EnemyStateType deathStateType;

        private void Start()
        {
            master = GameMasterController.Global;
            data = new EnemyCoreData();

            // get components in objects.

            if (rigidBodyObject == null)
                rigidBodyObject = this.gameObject;

            if (enemyRendererObject == null)
                enemyRendererObject = this.gameObject;

            if (sphereColliderObject == null)
                sphereColliderObject = this.gameObject;

            if (enemyAnimatorObject == null)
                enemyAnimatorObject = this.gameObject;

            if (audioSourceObject == null)
                audioSourceObject = this.gameObject;

            if (enemyTriggerObject != null)
                enemyTrigger = enemyTriggerObject.GetComponent<Collider>();

            rigidBody = rigidBodyObject.GetComponent<Rigidbody>();
            enemyRenderer = enemyRendererObject.GetComponent<Renderer>();
            sphereCollider = sphereColliderObject.GetComponent<SphereCollider>();
            enemyAnimator = enemyAnimatorObject.GetComponent<Animator>();

            audioSource = audioSourceObject.GetComponent<AudioSource>();
            audioSource.volume = master.audioController.volumeObject;

            // get damage effect controller.

            if(damageEffectObject != null)
            {
                damageEffect = damageEffectObject.GetComponent<ActorDamageEffectController>();
            }

            // add all states.

            states = new Dictionary<EnemyStateType, IEnemyStateController>();

            var states_to_add = gameObject.GetComponents<IEnemyStateController>();

            foreach(var state_to_add in states_to_add)
            {
                states.Add(state_to_add.GetStateType(), state_to_add);
            }

            previousStateType = defaultStateType;
            currentStateType = defaultStateType;

            // start the default state;

            states[currentStateType].BeginState(this);

            // get a reference to the player.

            playerObject = GameMasterController.GlobalPlayerObject;
            distanceToPlayerObject = float.MaxValue;

            // check values.

            if (distanceCanBeAlerted < ALERT_DISTANCE_MIN)
                distanceCanBeAlerted = ALERT_DISTANCE_MIN;

            if (alertInterval < ALERT_INTERVAL_MIN)
                alertInterval = ALERT_INTERVAL_MIN;

            if (health <= 0)
                health = ENEMY_HEALTH_MIN;
        }

        public EnemyCoreData GetData()
        {
            data.rigidBodyObject = rigidBodyObject;
            data.rigidBody = rigidBody;

            data.enemyRendererObject = enemyRendererObject;
            data.enemyRenderer = enemyRenderer;

            data.sphereColliderObject = sphereColliderObject;
            data.sphereCollider = sphereCollider;

            data.enemyAnimatorObject = enemyAnimatorObject;
            data.enemyAnimator = enemyAnimator;

            data.enemyTriggerObject = enemyTriggerObject;
            data.enemyTrigger = enemyTrigger;

            data.directionTransform = directionTransform;

            data.playerObject = playerObject;
            data.distanceToPlayerObject = distanceToPlayerObject;


            data.audioSource = audioSource;

            return data;
        }

        public Dictionary<EnemyStateType,IEnemyStateController> GetStates()
        {
            return states;
        }

        public IEnemyStateController GetCurrentState()
        {
            return states[currentStateType];
        }

        public bool GetDoesStateExist(EnemyStateType enemy_state_type)
        {
            return states.ContainsKey(enemy_state_type);
        }

        public void ChangeState(EnemyStateType new_state_type)
        {
            previousStateType = currentStateType;
            currentStateType = states.ContainsKey(new_state_type) ? new_state_type : defaultStateType;

            states[previousStateType].FinishState(this);
            states[currentStateType].BeginState(this);

        }

        private void Update()
        {
            if (master.gameState == GameState.Game)
            {
                states[currentStateType].UpdateState(this);
                states[currentStateType].UpdateStateAnimator(this);

                if (health <= 0 && !isDead)
                {
                    isDead = true;
                    ChangeState(deathStateType);
                }

                distanceToPlayerObject = GetDistanceToPlayer();

                if (distanceToPlayerObject <= distanceCanBeAlerted
                    && stateTypesCanBeAlerted.Contains(currentStateType))
                {
                    alertTimer += Time.deltaTime;

                    if (alertTimer >= alertInterval)
                    {
                        ChangeState(alertStateType);
                        alertTimer = 0.0f;
                    }
                }
                else
                {
                    alertTimer = 0.0f;
                }

                if(isDamaged)
                {
                    damageTimer += Time.deltaTime;

                    if(damageTimer >= damageInterval)
                    {
                        UnsetDamaged();
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (master.gameState == GameState.Game)
            {
                states[currentStateType].CheckState(this);
                states[currentStateType].FixedUpdateState(this);
            }
        }

        public EnemyStateType GetDefaultStateType()
        {
            return defaultStateType;
        }

        private float GetDistanceToPlayer()
        {
            if (playerObject == null)
                return float.MaxValue;

            return Vector3.Distance(gameObject.transform.position, playerObject.transform.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == GameConstants.TAG_PLAYER_DAMAGE_OBJECT
                || other.tag == GameConstants.TAG_INDIRECT_PLAYER_DAMAGE_OBJECT)
            {
                if (isDamaged)
                    return;

                // if the object is the player itself, repel them.

                if (other.transform.root.gameObject.name == GameConstants.NAME_PLAYER)
                    GameMasterController.GlobalPlayerController.SimpleRepel(this.gameObject,3);

                var damageData = other.gameObject.GetComponent<AttributeDamageController>()?.data;
                if (damageData == null)
                    damageData = AttributeDamageData.GetDefault();

                if (stateTypesCanBeDamaged.Contains(currentStateType) 
                    || canAlwaysBeDamaged)
                {
                    SetDamaged(damageData);
                }
            }
        }

        public void SetDamaged(AttributeDamageData damageData)
        {
            isDamaged = true;

            damageTimer = 0.0f;
            health -= damageData.damageAmount;
            ChangeState(damageStateType);

            damageEffect.SetDamageEffect();
        }

        public void UnsetDamaged()
        {
            isDamaged = false;

            damageEffect.UnsetDamageEffect();
        }
    }
}
