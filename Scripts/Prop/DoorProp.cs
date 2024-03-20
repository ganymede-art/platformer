using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class DoorProp : MonoBehaviour, IInteractable, IKeyItemUsable, IProp
{
    private static readonly Vector3 DUMMY_TRANSFORM_OFFSET = new Vector3(0.0F, -1000.0F, 0.0F);
    private const float POSITION_PLAYER_INTERVAL = 0.333F;
    private const float CLOSE_DOOR_DURATION_MULT = 0.5F;
    private const float PLAYER_WALK_ANIM_SPEED_MULT = 3.0F;

    private GameObject openingEventObject;

    private PropStatus activeStatus;
    private PropStatus previousStatus;
    private PropArgs args;
    private bool isLocked;

    // dummy objects.

    private GameObject dummyObject;

    // event position objects.

    private GameObject playerStartObject;
    private GameObject playerEndObject;
    private GameObject doorStartObject;
    private GameObject doorEndObject;

    [Header("Door Attributes")]
    public GameObject doorObject;
    public Transform doorClosedTransform;
    public Transform doorOpenFrontTransform;
    public Transform doorOpenBackTransform;
    public Transform standFrontTransform;
    public Transform standBackTransform;
    public float doorOpenInterval;
    public bool doWalkThrough;
    public bool doCloseAfterWalkThrough;

    [Header("Lock Attributes")]
    public KeyItemIdConstant keyItemId;
    public GameObject lockObject;
    public GameObject removeLockFxPrefab;

    [Header("Interactable Attributes")]
    public Transform interactableTransform;
    public Vector3 interactableOffset;
    public float interactableRange;

    [Header("Usable Attributes")]
    public Transform usableTransform;
    public Vector3 usableOffset;
    public float usableRange;

    [Header("Open One Shot Attributes")]
    public bool isOpenOneShot;
    public VariableIdConstant openVariableId;
    public bool doSetOpenOneShot;

    [Header("Unlock One Shot Attributes")]
    public bool isUnlockOneShot;
    public VariableIdConstant unlockVariableId;
    public bool doSetUnlockOneShot;

    [Header("Sound Attributes")]
    public AudioSource beginOpenAudioSource;
    public AudioSource whileOpenAudioSource;
    public AudioSource endOpenAudioSource;

    

    // Public properties.
    public bool IsInteractable => (activeStatus == PropStatus.Closed && !isLocked);
    public float InteractableRange => interactableRange;
    public GameObject InteractableGameObject => interactableTransform.gameObject;
    public Transform InteractableTransform => interactableTransform;
    public Vector3 InteractablePromptOffset => interactableOffset;

    public bool IsKeyItemUsable => (activeStatus == PropStatus.Closed && isLocked);
    public float KeyItemUsableRange => usableRange;
    public GameObject KeyItemUsableGameObject => usableTransform.gameObject;
    public Transform KeyItemUsableTransform => usableTransform;
    public Vector3 KeyItemUsablePromptOffset => usableOffset;

    public PropStatus ActiveStatus => activeStatus;
    public PropStatus PreviousStatus => previousStatus;
    public GameObject PropObject => gameObject;

    // Events.
    public event EventHandler<PropArgs> StatusChanged;

    private void Start()
    {
        activeStatus = PropStatus.Closed;

        args = new PropArgs();

        isLocked = (keyItemId != null);

        if (lockObject != null)
            lockObject.SetActive(isLocked);

        ActiveSceneHighLogic.G.Interactables[gameObject] = this;
        ActiveSceneHighLogic.G.KeyItemUsables[gameObject] = this;

        dummyObject = new GameObject("DummyObject");
        dummyObject.transform.position = DUMMY_TRANSFORM_OFFSET;

        playerStartObject = new GameObject();
        playerEndObject = new GameObject();
        doorStartObject = new GameObject();
        doorEndObject = new GameObject();

        SetupOpenEvent();

        // check one shot variables.

        if (isOpenOneShot)
        {
            StartOpenOneShot();
        }

        if (isUnlockOneShot)
        {
            StartUnlockOneShot();
        }
    }

    private void StartOpenOneShot()
    {
        bool isOpenVarSet = PersistenceHighLogic.G.GetBoolVariable(openVariableId.VariableId);

        if (!isOpenVarSet)
            return;

        doorObject.transform.position = doorOpenBackTransform.position;
        doorObject.transform.rotation = doorOpenBackTransform.rotation;

        ChangeStatus(PropStatus.Open);
    }

    private void StartUnlockOneShot()
    {
        bool isUnlockVarSet = PersistenceHighLogic.G.GetBoolVariable(unlockVariableId.VariableId);

        if (!isUnlockVarSet)
            return;

        isLocked = false;

        if (lockObject != null && removeLockFxPrefab != null)
            Instantiate(removeLockFxPrefab, lockObject.transform.position, lockObject.transform.rotation);

        if (lockObject != null)
            lockObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (!ActiveSceneHighLogic.G.Interactables.ContainsKey(gameObject))
            ActiveSceneHighLogic.G.Interactables.Add(gameObject, this);

        if (!ActiveSceneHighLogic.G.KeyItemUsables.ContainsKey(gameObject))
            ActiveSceneHighLogic.G.KeyItemUsables.Add(gameObject, this);
    }

    private void OnDisable()
    {
        if (ActiveSceneHighLogic.G == null)
            return;

        if (ActiveSceneHighLogic.G.Interactables.ContainsKey(gameObject))
            ActiveSceneHighLogic.G.Interactables.Remove(gameObject);

        if (ActiveSceneHighLogic.G.KeyItemUsables.ContainsKey(gameObject))
            ActiveSceneHighLogic.G.KeyItemUsables.Remove(gameObject);
    }

    private void ChangeStatus(PropStatus newState)
    {
        previousStatus = activeStatus;
        activeStatus = newState;
        args.activeStatus = activeStatus;
        args.previousStatus = previousStatus;
        StatusChanged?.Invoke(this, args);
    }

    public void OnInteract()
    {
        var eventContainer = openingEventObject.GetComponent<AddActionHighLogicTrigger>();
        eventContainer.AddAction();

        if (doSetOpenOneShot)
            PersistenceHighLogic.G.SetBoolVariable(openVariableId.VariableId, true);
    }

    public void OnKeyItemUse(string keyItemId)
    {
        if (!isLocked)
            return;

        if (keyItemId != this.keyItemId.KeyItemId)
            return;

        if (lockObject != null)
            lockObject.SetActive(false);

        isLocked = false;

        var eventContainer = openingEventObject.GetComponent<AddActionHighLogicTrigger>();
        eventContainer.AddAction();

        if (doSetUnlockOneShot)
        {
            PersistenceHighLogic.G.SetBoolVariable(unlockVariableId.VariableId, true);
            PlayerHighLogic.G.RemoveKeyItem(keyItemId);
        }

        if (doSetOpenOneShot)
            PersistenceHighLogic.G.SetBoolVariable(openVariableId.VariableId, true);
    }

    private void SetupOpenEvent()
    {
        openingEventObject = new GameObject($"{gameObject.name}Event");
        openingEventObject.transform.SetParent(transform, false);

        var stateId = ScriptableObject.CreateInstance<HighLogicStateIdConstant>();
        stateId.name = HighLogicStateId.Film.ToString();

        var eventContainer = openingEventObject.AddComponent<AddActionHighLogicTrigger>();
        eventContainer.actionId = $"{gameObject.name}Event";
        eventContainer.actionHighLogicStateId = stateId;
        eventContainer.isSequenced = false;

        // create event objects.

        var setupEventObject = new GameObject();

        var prePositionPlayerEventObject = new GameObject();
        var positionPlayerEventObject = new GameObject();
        var postPositionPlayerEventObject = new GameObject();

        var preMoveDoorEventObject = new GameObject();
        var moveDoorEventObject = new GameObject();
        var postMoveDoorEventObject = new GameObject();

        var preMovePlayerEventObject = new GameObject();
        var movePlayerEventObject = new GameObject();
        var postMovePlayerEventObject = new GameObject();

        var preCloseDoorEventObject = new GameObject();
        var closeDoorEventObject = new GameObject();
        var postCloseDoorEventObject = new GameObject();

        // create events.

        var setupEvent = setupEventObject.AddComponent<RunDelegateAction>();

        var prePositionPlayerEvent = prePositionPlayerEventObject.AddComponent<RunDelegateAction>();
        var positionPlayerEvent = positionPlayerEventObject.AddComponent<MovePlayerAction>();
        var postPositionPlayerEvent = postPositionPlayerEventObject.AddComponent<RunDelegateAction>();

        var preMoveDoorEvent = preMoveDoorEventObject.AddComponent<RunDelegateAction>();
        var moveDoorEvent = moveDoorEventObject.AddComponent<MoveObjectAction>();
        var postMoveDoorEvent = postMoveDoorEventObject.AddComponent<RunDelegateAction>();

        var preMovePlayerEvent = preMovePlayerEventObject.AddComponent<RunDelegateAction>();
        var movePlayerEvent = movePlayerEventObject.AddComponent<MovePlayerAction>();
        var postMovePlayerEvent = postMovePlayerEventObject.AddComponent<RunDelegateAction>();

        var preCloseDoorEvent = preCloseDoorEventObject.AddComponent<RunDelegateAction>();
        var closeDoorEvent = closeDoorEventObject.AddComponent<MoveObjectAction>();
        var postCloseDoorEvent = postCloseDoorEventObject.AddComponent<RunDelegateAction>();

        // configure events.

        setupEvent.beginActionDelegate = StartSetupEvent;
        setupEvent.nextActionObject = prePositionPlayerEventObject;

        prePositionPlayerEvent.beginActionDelegate = StartPrePositionPlayerEvent;
        prePositionPlayerEvent.nextActionObject = positionPlayerEventObject;

        positionPlayerEvent.startTransform = null;
        positionPlayerEvent.finishTransform = playerStartObject.transform;
        positionPlayerEvent.moveInterval = POSITION_PLAYER_INTERVAL;
        positionPlayerEvent.nextActionObject = postPositionPlayerEventObject;

        postPositionPlayerEvent.beginActionDelegate = StartPostPositionPlayerEvent;
        postPositionPlayerEvent.nextActionObject = preMoveDoorEventObject;

        preMoveDoorEvent.beginActionDelegate = StartPreMoveDoorEvent;
        preMoveDoorEvent.nextActionObject = moveDoorEventObject;

        moveDoorEvent.moveObject = doorObject;
        moveDoorEvent.startTransform = doorStartObject.transform;
        moveDoorEvent.finishTransform = doorEndObject.transform;
        moveDoorEvent.moveInterval = doorOpenInterval;
        moveDoorEvent.rotateInterval = doorOpenInterval;
        moveDoorEvent.scaleInterval = doorOpenInterval;
        moveDoorEvent.doMove = true;
        moveDoorEvent.doRotate = true;
        moveDoorEvent.doScale = true;
        moveDoorEvent.nextActionObject = postMoveDoorEventObject;

        postMoveDoorEvent.beginActionDelegate = StartPostMoveDoorEvent;
        postMoveDoorEvent.nextActionObject = (doWalkThrough) ? preMovePlayerEventObject : null;

        preMovePlayerEvent.beginActionDelegate = StartPreMovePlayerEvent;
        preMovePlayerEvent.nextActionObject = movePlayerEventObject;

        movePlayerEvent.startTransform = playerStartObject.transform;
        movePlayerEvent.finishTransform = playerEndObject.transform;
        movePlayerEvent.moveInterval = 1.0F;
        movePlayerEvent.nextActionObject = postMovePlayerEventObject;

        postMovePlayerEvent.beginActionDelegate = StartPostMovePlayerEvent;
        postMovePlayerEvent.nextActionObject = (doCloseAfterWalkThrough) ? preCloseDoorEventObject : null;

        preCloseDoorEvent.beginActionDelegate = StartPreCloseDoorEvent;
        preCloseDoorEvent.nextActionObject = closeDoorEventObject;

        closeDoorEvent.moveObject = doorObject;
        closeDoorEvent.startTransform = doorEndObject.transform;
        closeDoorEvent.finishTransform = doorStartObject.transform;
        closeDoorEvent.moveInterval = doorOpenInterval * CLOSE_DOOR_DURATION_MULT;
        closeDoorEvent.rotateInterval = doorOpenInterval * CLOSE_DOOR_DURATION_MULT;
        closeDoorEvent.scaleInterval = doorOpenInterval * CLOSE_DOOR_DURATION_MULT;
        closeDoorEvent.doMove = true;
        closeDoorEvent.doRotate = true;
        closeDoorEvent.doScale = true;
        closeDoorEvent.nextActionObject = postCloseDoorEventObject;

        postCloseDoorEvent.beginActionDelegate = StartPostCloseDoorEvent;
        postCloseDoorEvent.nextActionObject = null;


        // finalise event container.

        eventContainer.activeActionObject = setupEventObject;
    }

    private void StartSetupEvent(ActionSource actionSource)
    {
        doorStartObject.transform.position = doorClosedTransform.position;
        doorStartObject.transform.rotation = doorClosedTransform.rotation;

        float distToFront = Vector3.Distance(ActiveSceneHighLogic.G.CachedPlayerObject.transform.position, standFrontTransform.position);
        float distToBack = Vector3.Distance(ActiveSceneHighLogic.G.CachedPlayerObject.transform.position, standBackTransform.position);

        doorEndObject.transform.position = (distToFront > distToBack)
            ? doorOpenFrontTransform.position
            : doorOpenBackTransform.position;

        doorEndObject.transform.rotation = (distToFront > distToBack)
            ? doorOpenFrontTransform.rotation
            : doorOpenBackTransform.rotation;

        playerStartObject.transform.position = (distToFront > distToBack)
            ? standBackTransform.position
            : standFrontTransform.position;

        playerStartObject.transform.rotation = (distToFront > distToBack)
            ? standBackTransform.rotation
            : standFrontTransform.rotation;

        playerEndObject.transform.position = (distToFront > distToBack)
            ? standFrontTransform.position
            : standBackTransform.position;

        playerEndObject.transform.rotation = (distToFront > distToBack)
            ? standFrontTransform.rotation
            : standBackTransform.rotation;
    }

    private void StartPrePositionPlayerEvent(ActionSource actionSource)
    {
        ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.ResetAllAnimatorTriggers();
        ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.SetTrigger(ANIMATION_TRIGGER_JUMP_UP);
    }

    private void StartPostPositionPlayerEvent(ActionSource actionSource)
    {
        ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.ResetAllAnimatorTriggers();
        ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.SetTrigger(ANIMATION_TRIGGER_IDLE);
    }

    private void StartPreMoveDoorEvent(ActionSource actionSource)
    {
        if (beginOpenAudioSource != null)
            beginOpenAudioSource.PlayOneShot(beginOpenAudioSource.clip, SettingsHighLogic.G.PropVolume);

        if (whileOpenAudioSource != null)
            whileOpenAudioSource.PlayOneShot(whileOpenAudioSource.clip, SettingsHighLogic.G.PropVolume);
    }

    private void StartPostMoveDoorEvent(ActionSource actionSource)
    {
        if (whileOpenAudioSource != null)
            whileOpenAudioSource.Stop();

        if (endOpenAudioSource != null)
            endOpenAudioSource.PlayOneShot(endOpenAudioSource.clip, SettingsHighLogic.G.PropVolume);

        ChangeStatus(PropStatus.Open);
    }

    private void StartPreMovePlayerEvent(ActionSource actionSource)
    {
        ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.ResetAllAnimatorTriggers();
        ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.SetTrigger(ANIMATION_TRIGGER_MOVE);
        ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.SetFloat(ANIMATION_TRIGGER_SPEED_MULTIPLIER, PLAYER_WALK_ANIM_SPEED_MULT);
    }

    private void StartPostMovePlayerEvent(ActionSource actionSource)
    {
        ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.ResetAllAnimatorTriggers();
        ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.SetTrigger(ANIMATION_TRIGGER_IDLE);
    }

    private void StartPreCloseDoorEvent(ActionSource actionSource)
    {
        if (beginOpenAudioSource != null)
            beginOpenAudioSource.PlayOneShot(beginOpenAudioSource.clip, SettingsHighLogic.G.PropVolume);

        if (whileOpenAudioSource != null)
            whileOpenAudioSource.PlayOneShot(whileOpenAudioSource.clip, SettingsHighLogic.G.PropVolume);
    }

    private void StartPostCloseDoorEvent(ActionSource actionSource)
    {
        if (whileOpenAudioSource != null)
            whileOpenAudioSource.Stop();

        if (endOpenAudioSource != null)
            endOpenAudioSource.PlayOneShot(endOpenAudioSource.clip, SettingsHighLogic.G.PropVolume);

        ChangeStatus(PropStatus.Closed);
    }
}