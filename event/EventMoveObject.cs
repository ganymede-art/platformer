using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script
{
    public class EventMoveObject : MonoBehaviour, IEventController
    {
        const float MOVE_DURATION_MIN = 0.1F;
        const float MOVE_DURATION_DEFAULT = 1.0F;

        const float ROTATE_DURATION_MIN = 0.1F;
        const float ROTATE_DURATION_DEFAULT = 1.0F;

        const float SCALE_DURATION_MIN = 0.1F;
        const float SCALE_DURATION_DEFAULT = 1.0F;

        private Vector3 startPosition;
        private Vector3 endPosition;
        private Quaternion startRotation;
        private Quaternion endRotation;
        private Vector3 startScale;
        private Vector3 endScale;

        private bool isActive = false;

        private float moveTimer = 0.0F;
        private float moveProgess = 0.0F;

        private float rotateTimer = 0.0F;
        private float rotateProgess = 0.0F;

        private float scaleTimer = 0.0F;
        private float scaleProgress = 0.0F;

        private Rigidbody objectRigidBody;

        private GameEvent parentEvent = null;

        [Header("Event Sources")]
        public GameObject nextEventSource;

        [Header("Move Attributes")]
        public GameObject objectToMove;

        public Transform startTransform;
        public Transform endTransform;

        public float moveDuration;
        public float rotateDuration;
        public float scaleDuration;

        public bool doScale;

        public bool isObjectKinematic;

        private void Start()
        {
            if (moveDuration < MOVE_DURATION_MIN)
                moveDuration = MOVE_DURATION_DEFAULT;

            if (rotateDuration < ROTATE_DURATION_MIN)
                rotateDuration = ROTATE_DURATION_DEFAULT;

            if (scaleDuration < SCALE_DURATION_MIN)
                scaleDuration = SCALE_DURATION_DEFAULT;

            if (rotateDuration > moveDuration)
                rotateDuration = moveDuration;

            if (scaleDuration > moveDuration)
                scaleDuration = moveDuration;

            if (isObjectKinematic)
            {
                objectRigidBody = objectToMove.GetComponent<Rigidbody>();

                if (objectRigidBody == null)
                    Debug.LogError("Rigid body not found.");
            }

            if (endTransform == null)
            {
                endTransform = transform;
                Debug.LogError("End transform was null. Setting to local transform.");
            }
        }

        private void Update()
        {

        }

        public void FinishEvent(GameEvent gameEvent)
        {
            isActive = false;
        }

        public string GetEventDescription()
        {
            return GameConstants.EVENT_TYPE_MOVE_OBJECT;
        }

        public string GetEventType()
        {
            return GameConstants.EVENT_TYPE_MOVE_OBJECT;
        }

        public bool GetIsEventComplete(GameEvent gameEvent)
        {
            return (moveProgess >= 1.0f);
        }

        public bool GetIsUpdateComplete(GameEvent gameEvent)
        {
            return (moveProgess >= 1.0f);
        }

        public GameObject GetNextEventSource()
        {
            return nextEventSource;
        }

        public void UpdateEvent(GameEvent gameEvent)
        {
            moveTimer += Time.deltaTime;
            rotateTimer += Time.deltaTime;
            scaleTimer += Time.deltaTime;

            moveProgess = Mathf.InverseLerp(0, moveDuration, moveTimer);
            rotateProgess = Mathf.InverseLerp(0, rotateDuration, rotateTimer);
            scaleProgress = Mathf.InverseLerp(0, scaleDuration, scaleTimer);

            moveProgess = Mathf.Clamp(moveProgess, 0.0f, 1.0f);
            rotateProgess = Mathf.Clamp(rotateProgess, 0.0f, 1.0f);
            scaleProgress = Mathf.Clamp(scaleProgress, 0.0f, 1.0f);

            if (isObjectKinematic && objectRigidBody != null)
            {
                objectRigidBody.MovePosition(Vector3.Lerp(startPosition, endPosition, moveProgess));
                objectRigidBody.MoveRotation(Quaternion.Lerp(startRotation, endRotation, rotateProgess));
            }
            else
            {
                objectToMove.transform.position = Vector3.Lerp(startPosition, endPosition, moveProgess);
                objectToMove.transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotateProgess);
                if(doScale)
                    objectToMove.transform.localScale = Vector3.Lerp(startScale, endScale, scaleProgress);
            }
        }

        public void ResetEvent(GameEvent gameEvent)
        {
            
        }

        public void StartEvent(GameEvent gameEvent)
        {
            parentEvent = gameEvent;

            if (startTransform == null)
            {
                startPosition = objectToMove.transform.position;
                startRotation = objectToMove.transform.rotation;
                startScale = objectToMove.transform.localScale;
            }
            else
            {
                startPosition = startTransform.position;
                startRotation = startTransform.rotation;
                startScale = startTransform.localScale;
            }

            endPosition = endTransform.position;
            endRotation = endTransform.rotation;
            endScale = endTransform.localScale;

            moveTimer = 0.0F;
            rotateTimer = 0.0F;
            scaleTimer = 0.0F;

            moveProgess = 0.0F;
            rotateProgess = 0.0F;
            scaleProgress = 0.0F;

            isActive = true;
        }

        private void OnDrawGizmos()
        {
            EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource,
                optionalObject1: endTransform.gameObject, optionalColour1: Color.red, optionalIcon1: "ev_moveobject.png");
        }
    }
}
