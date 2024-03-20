using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Constants;

public class InteractSaveTrigger : MonoBehaviour, IInteractable
{
    // Consts.
    private const float MOVE_PLAYER_INTERVAL = 0.25F;
    private const float MOVE_CAMCORDER_INTERVAL = 0.5F;
    private const float MOVE_ITEM_INTERVAL = 1.0F;
    private const float DELAY_INTERVAL = 1.0F;
    private static readonly Vector3 ACTION_SOURCE_POSITION_OFFSET = new Vector3(0.0F, 0.0F, 1.0F);
    private static readonly Vector3 CAMCORDER_POSITION_OFFSET = new Vector3(0.0F, 0.25F, 0.0F);
    private static readonly Vector3 ITEM_FINISH_POSITION_OFFSET = new Vector3(0.0F, 0.5F, 0.0F);

    private const string SAVE_CHOICES_TEXT_ID = @"SaveChoices";
    private const string SAVE_CHOICE_0_TEXT_ID = @"SaveChoice0";
    private const string SAVE_CHOICE_1_TEXT_ID = @"SaveChoice1";
    private const string SAVE_CHOICE_2_TEXT_ID = @"SaveChoice2";

    // Private fields.
    private GameObject addActionObject;
    private AddActionHighLogicTrigger addActionHighLogicTrigger;

    // Public properties.
    public bool IsInteractable => gameObject.activeInHierarchy;
    public float InteractableRange => interactableRange;
    public GameObject InteractableGameObject => gameObject;
    public Transform InteractableTransform => transform;
    public Vector3 InteractablePromptOffset => interactablePromptOffset;

    // Public fields.
    [Header("Interaction Attributes")]
    public float interactableRange;
    public Vector3 interactablePromptOffset;
    public VoxData voxData;
    [Header("Save Attributes")]
    public Transform startingTransform;

    private void Start()
    {
        ActiveSceneHighLogic.G.Interactables[gameObject] = this;

        // Create the add action.
        addActionObject = new GameObject($"AddActionSave");
        addActionObject.transform.SetPositionAndRotation
            ( transform.TransformPoint(ACTION_SOURCE_POSITION_OFFSET)
            , transform.rotation);

        // Create the action positions.
        var playerPositionObject = new GameObject($"PlayerPosition");
        var camcorderPositionObject = new GameObject($"CamcorderPosition");

        // Position the positions.
        playerPositionObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
        camcorderPositionObject.transform.SetPositionAndRotation(addActionObject.transform.position + CAMCORDER_POSITION_OFFSET, transform.rotation);
        camcorderPositionObject.transform.LookAt(transform.position + ITEM_FINISH_POSITION_OFFSET);

        // Create the action objects.
        var beginActionObject = new GameObject("1");
        var movePlayerActionObject = new GameObject("2");
        var choicesActionObject = new GameObject("3");
        var endActionObject = new GameObject("4a");
        var saveActionObject = new GameObject("4b");
        var waitThreeHoursObject = new GameObject("4d");

        // Position the actions.
        beginActionObject.transform.SetParent(addActionObject.transform, false);
        movePlayerActionObject.transform.SetParent(addActionObject.transform, false);
        choicesActionObject.transform.SetParent(addActionObject.transform, false);
        saveActionObject.transform.SetParent(addActionObject.transform, false);
        waitThreeHoursObject.transform.SetParent(addActionObject.transform, false);
        endActionObject.transform.SetParent(addActionObject.transform, false);

        // Configure the add action.
        var stateIdConstant = ScriptableObject.CreateInstance<HighLogicStateIdConstant>();
        stateIdConstant.name = HighLogicStateId.Film.ToString();

        addActionHighLogicTrigger = addActionObject.AddComponent<AddActionHighLogicTrigger>();
        addActionHighLogicTrigger.actionHighLogicStateId = stateIdConstant;
        addActionHighLogicTrigger.actionId = $"{Guid.NewGuid()}";
        addActionHighLogicTrigger.isSequenced = true;
        addActionHighLogicTrigger.isOneShot = false;
        addActionHighLogicTrigger.activeActionObject = beginActionObject;

        // Configurate the actions.
        Action<ActionSource> beginDelegate = (x) =>
        { 
            ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.ResetAllAnimatorTriggers();
            ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.SetTrigger(ANIMATION_TRIGGER_CROUCH);
            var camcorderArgs = new Dictionary<string, object>();
            camcorderArgs[CAMCORDER_STATE_ARG_FIXED_POSITION_OBJECT] = camcorderPositionObject;
            camcorderArgs[CAMCORDER_STATE_ARG_FIXED_TRANSITION_INTERVAL] = MOVE_CAMCORDER_INTERVAL;
            ActiveSceneHighLogic.G.CachedCamcorder.ChangeState(CamcorderStateId.Fixed, camcorderArgs);
        };
        var beginAction = beginActionObject.AddComponent<RunDelegateAction>();
        beginAction.beginActionDelegate = beginDelegate;

        var movePlayerAction = movePlayerActionObject.AddComponent<MovePlayerAction>();
        movePlayerAction.startTransform = null;
        movePlayerAction.finishTransform = playerPositionObject.transform;
        movePlayerAction.moveInterval = MOVE_PLAYER_INTERVAL;

        var choicesAction = choicesActionObject.AddComponent<ChoicesAction>();
        choicesAction.textId = SAVE_CHOICES_TEXT_ID;
        choicesAction.voxData = voxData;
        choicesAction.replacerObjects = null;
        choicesAction.choices = new ChoicesAction.Choice[]
        {
            new ChoicesAction.Choice()
            {
                nextActionObject = endActionObject,
                textId = SAVE_CHOICE_0_TEXT_ID,
            },
            new ChoicesAction.Choice()
            {
                nextActionObject = saveActionObject,
                textId = SAVE_CHOICE_1_TEXT_ID,
            },
            new ChoicesAction.Choice()
            {
                nextActionObject = waitThreeHoursObject,
                textId = SAVE_CHOICE_2_TEXT_ID,
            },
        };

        Action<ActionSource> endDelegate = (x) =>
        {
            ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.ResetAllAnimatorTriggers();
            ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.SetTrigger(ANIMATION_TRIGGER_IDLE);
            ActiveSceneHighLogic.G.CachedCamcorder.ChangeState(CamcorderStateId.Orbit);
        };
        var endAction = endActionObject.AddComponent<RunDelegateAction>();
        endAction.beginActionDelegate = endDelegate;

        Action<ActionSource> saveDelegate = (x) =>
        {
            string sceneName = SceneManager.GetActiveScene().name;
            string startingTransformName = startingTransform.name;
            PersistenceHighLogic.G.SavePersistence(sceneName, startingTransformName);
            PersistenceHighLogic.G.LoadPersistence();
        };
        var saveAction = saveActionObject.AddComponent<RunDelegateAction>();
        saveAction.beginActionDelegate = saveDelegate;

        Action<ActionSource> waitThreeHoursDelegate = (x) =>
        {
            TimeHighLogic.G.ModifyTime(3);
            string sceneName = SceneManager.GetActiveScene().name;
            string startingTransformName = startingTransform.name;
            PersistenceHighLogic.G.SavePersistence(sceneName, startingTransformName);
            PersistenceHighLogic.G.LoadPersistence();
        };
        var waitThreeHoursAction = waitThreeHoursObject.AddComponent<RunDelegateAction>();
        waitThreeHoursAction.beginActionDelegate = waitThreeHoursDelegate;

        // Chain the actions.
        beginAction.nextActionObject = movePlayerActionObject;
        movePlayerAction.nextActionObject = choicesActionObject;
    }

    private void OnDestroy()
    {
        if (ActiveSceneHighLogic.G == null)
            return;
        ActiveSceneHighLogic.G.Interactables.Remove(gameObject);
    }

    public void OnInteract()
    {
        addActionHighLogicTrigger.AddAction();
    }
}
