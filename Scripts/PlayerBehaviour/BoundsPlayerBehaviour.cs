using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class BoundsPlayerBehaviour : MonoBehaviour, IBehaviour<Player, PlayerBehaviourId>
{
    private const float DEATH_BARRIER_Y_LEVEL = -50.0F;

    private int[] VALID_LAYERS =
    {
        SCENE_STATIC,
        SCENE_STATIC_IGNORE_CAMERA,
        SCENE_DYNAMIC,
        SCENE_DYNAMIC_IGNORE_CAMERA
    };

    private static readonly Vector3[] CHECK_OFFSETS = new Vector3[]
    {
        new Vector3(0.0F, 0.0F, 0.2F),
        new Vector3(0.0F, 0.0F, -0.2F),
        new Vector3(0.2F, 0.0F, 0.0F),
        new Vector3(-0.2F, 0.0F, 0.0F),
    };

    // Private fields.
    private Vector3 lastInBoundsPosition;
    bool[] isHits;
    RaycastHit[] hitInfos;

    // Public properties.
    public Vector3 LastInBoundsPosition => lastInBoundsPosition;
    public PlayerBehaviourId BehaviourId => PlayerBehaviourId.Bounds;

    // Public fields.
    public Player controller;

    private void Awake()
    {
        isHits = new bool[CHECK_OFFSETS.Length];
        hitInfos = new RaycastHit[CHECK_OFFSETS.Length];
    }

    public void BeginBehaviour(Player c, Dictionary<string, object> args = null) { }
    public void EndBehaviours(Player c) { }

    public void FixedUpdateBehaviour(Player c)
    {
        for(int i = 0; i < CHECK_OFFSETS.Length; i++)
        {
            isHits[i] = Physics.Raycast
                ( transform.position + CHECK_OFFSETS[i]
                , Vector3.down
                , out hitInfos[i]
                , 0.5F
                , LAYER_MASK_ALL_BUT_PLAYER
                , QueryTriggerInteraction.Ignore);
        }

        bool areAllHits = true;
        for(int i = 0; i < CHECK_OFFSETS.Length; i++)
        {
            if (!isHits[i])
            {
                areAllHits = false;
                break;
            }
        }

        if (areAllHits)
            lastInBoundsPosition = c.transform.position;

        if (c.transform.position.y < DEATH_BARRIER_Y_LEVEL)
            ResetPlayerToLastInBoundsPosition();
    }

    public void UpdateBehaviour(Player c) { }

    public void ResetPlayerToLastInBoundsPosition()
    {
        controller.transform.position = lastInBoundsPosition;
    }
}
