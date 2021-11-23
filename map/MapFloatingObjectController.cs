using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;

public class MapFloatingObjectController : MonoBehaviour
{
    const float SINKING_DURATION_MIN = 0.1f;

    List<GameObject> weightObjects = new List<GameObject>();
    bool wasWeighedDown = false;
    bool isWeighedDown = false;
    private float sinkingTimer = 0.0f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Rigidbody objectRigidBody;

    public Vector3 sinkingOffset;
    public float sinkingDuration;
    public AudioClip sinkingSound;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = gameObject.transform.position;
        endPosition = startPosition + sinkingOffset;
        objectRigidBody = this.gameObject.GetComponent<Rigidbody>();

        if (sinkingDuration < SINKING_DURATION_MIN)
            sinkingDuration = SINKING_DURATION_MIN;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMasterController.Global.gameState != GameState.Game)
            return;

        if (isWeighedDown && sinkingTimer < 1)
        {
            sinkingTimer += 1f * Time.deltaTime;
            if (sinkingTimer > sinkingDuration)
                sinkingTimer = sinkingDuration;
        }

        if (!isWeighedDown && sinkingTimer > 0)
        {
            sinkingTimer -= 1f * Time.deltaTime;
            if (sinkingTimer < 0)
                sinkingTimer = 0.0f;
        }

        float t = sinkingTimer / sinkingDuration;
        t = t * t * (3f - 2f * t);

        objectRigidBody.MovePosition(Vector3.Lerp(startPosition,endPosition,t));
    }

    private void OnTriggerEnter(Collider other)
    {
        wasWeighedDown = isWeighedDown;

        if (other.gameObject.tag == GameConstants.TAG_PLAYER)
        {
            weightObjects.Add(other.gameObject);
        }

        if (weightObjects.Count > 0)
            isWeighedDown = true;

        if(!wasWeighedDown && isWeighedDown)
        {
            AudioSource.PlayClipAtPoint(sinkingSound, this.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        wasWeighedDown = isWeighedDown;

        if (other.gameObject.tag == GameConstants.TAG_PLAYER)
        {
            weightObjects.Remove(other.gameObject);
        }

        if (weightObjects.Count == 0)
            isWeighedDown = false;
    }
}
