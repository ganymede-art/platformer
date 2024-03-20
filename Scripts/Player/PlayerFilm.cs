using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFilm : MonoBehaviour
{
    // Private fields.
    private PlayerFilmStatus status;

    private Vector3 facingDirection;
    private GameObject facingTarget;

    // Public properties.
    public PlayerFilmStatus Status => status;

    // Public fields.
    public Player controller;

    private void Start()
    {
        status = PlayerFilmStatus.None;
        facingDirection = Vector3.zero;
        StateHighLogic.G.HighLogicStateChanged += OnHighLogicStateChanged;
    }

    private void OnDestroy()
    {
        if (StateHighLogic.G != null)
            StateHighLogic.G.HighLogicStateChanged -= OnHighLogicStateChanged;
    }

    private void OnHighLogicStateChanged(object sender, EventArgs args)
    {
        enabled = StateHighLogic.G.ActiveState == HighLogicStateId.Film;

        if (StateHighLogic.G.ActiveState != HighLogicStateId.Film)
            return;

        // Zero out player velocity if grounded.
        if (controller.GroundCheck.IsCheckSphereGrounded)
            controller.ClearCachedVelocity();
    }

    private void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Film)
            return;

        // Clear out cached velocity if grounded.
        if (ActiveSceneHighLogic.G.CachedPlayer.GroundCheck.IsCheckSphereGrounded)
            ActiveSceneHighLogic.G.CachedPlayer.ClearCachedVelocity();

        if (status == PlayerFilmStatus.FaceAction || (status == PlayerFilmStatus.None && ActionHighLogic.G.SequencedActions.Count > 0))
        {
            // Rotate player to active action.
            Vector3 actionPosition = ActionHighLogic.G.SequencedActions[0].activeActionObject.transform.position;

            Vector3 directionToAction
                = (actionPosition - transform.position).normalized;
            directionToAction.y = 0.0F;

            PlayerStatics.UpdateInternalDirection(ActiveSceneHighLogic.G.CachedPlayer, directionToAction);
            PlayerStatics.UpdateRendererDirection(ActiveSceneHighLogic.G.CachedPlayer, directionToAction);
        }
        else if(status == PlayerFilmStatus.FaceDirection)
        {
            PlayerStatics.UpdateInternalDirection(ActiveSceneHighLogic.G.CachedPlayer, facingDirection);
            PlayerStatics.UpdateRendererDirection(ActiveSceneHighLogic.G.CachedPlayer, facingDirection);
        }
        else if(status == PlayerFilmStatus.FaceTarget)
        {

        }
    }
}
