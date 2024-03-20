using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Constants;

public static class ItemStatics
{
    // Consts.
    private const float MOVE_PLAYER_INTERVAL = 0.25F;
    private const float MOVE_CAMCORDER_INTERVAL = 0.5F;
    private const float MOVE_ITEM_INTERVAL = 1.0F;
    private const float DELAY_INTERVAL = 1.0F;
    private static readonly Vector3 ACTION_SOURCE_POSITION_OFFSET = new Vector3(0.0F, 0.0F, 1.0F);
    private static readonly Vector3 CAMCORDER_POSITION_OFFSET = new Vector3(0.0F, 0.25F, 0.0F);
    private static readonly Vector3 ITEM_FINISH_POSITION_OFFSET = new Vector3(0.0F, 0.5F, 0.0F);

    public static AddActionHighLogicTrigger GetNewKeyItemAddActionTrigger
        ( GameObject itemObject
        , GameObject rendererContainerObject
        , KeyItemIdConstant keyItemId)
    {
        Action<ActionSource> initDelegate = (x) =>
        {
            PlayerHighLogic.G.AddKeyItem(keyItemId.KeyItemId);
        };

        return GetNewAddActionTrigger(itemObject, rendererContainerObject, initDelegate);
    }

    public static AddActionHighLogicTrigger GetNewAddActionTrigger
        ( GameObject itemObject
        , GameObject rendererContainerObject
        , Action<ActionSource> initDelegate)
    {
        GameObject addActionObject;
        AddActionHighLogicTrigger addActionHighLogicTrigger;

        GameObject playerPositionObject;
        GameObject camcorderPositionObject;
        GameObject itemStartPositionObject;
        GameObject itemFinishPositionObject;

        // Create the add action.
        addActionObject = new GameObject($"AddAction");
        addActionObject.transform.SetPositionAndRotation
            ( itemObject.transform.TransformPoint(ACTION_SOURCE_POSITION_OFFSET)
            , itemObject.transform.rotation);

        // Create the action positions.
        playerPositionObject = new GameObject($"PlayerPosition");
        camcorderPositionObject = new GameObject($"CamcorderPosition");
        itemStartPositionObject = new GameObject($"ItemStartPosition");
        itemFinishPositionObject = new GameObject($"ItemFinishPosition");

        // Position the positions.
        playerPositionObject.transform.SetPositionAndRotation(itemObject.transform.position, itemObject.transform.rotation);
        camcorderPositionObject.transform.SetPositionAndRotation(addActionObject.transform.position + CAMCORDER_POSITION_OFFSET, itemObject.transform.rotation);
        camcorderPositionObject.transform.LookAt(itemObject.transform.position + ITEM_FINISH_POSITION_OFFSET);
        itemStartPositionObject.transform.SetPositionAndRotation(itemObject.transform.position, itemObject.transform.rotation);
        itemStartPositionObject.transform.localScale = Vector3.zero;
        itemFinishPositionObject.transform.SetPositionAndRotation(itemObject.transform.position + ITEM_FINISH_POSITION_OFFSET, itemObject.transform.rotation);

        // Create the actions.
        var initActionObject = new GameObject("0");
        var beginActionObject = new GameObject("1");
        var movePlayerActionObject = new GameObject("2");
        var moveItemActionObject = new GameObject("3");
        var delayActionObject = new GameObject("4");
        var endActionObject = new GameObject("5");

        // Position the actions.
        initActionObject.transform.SetParent(addActionObject.transform, false);
        beginActionObject.transform.SetParent(addActionObject.transform,false);
        movePlayerActionObject.transform.SetParent(addActionObject.transform,false);
        moveItemActionObject.transform.SetParent(addActionObject.transform,false);
        delayActionObject.transform.SetParent(addActionObject.transform,false);
        endActionObject.transform.SetParent(addActionObject.transform, false);

        // Configure the add action.
        var stateIdConstant = ScriptableObject.CreateInstance<HighLogicStateIdConstant>();
        stateIdConstant.name = HighLogicStateId.Film.ToString();

        addActionHighLogicTrigger = addActionObject.AddComponent<AddActionHighLogicTrigger>();
        addActionHighLogicTrigger.actionHighLogicStateId = stateIdConstant;
        addActionHighLogicTrigger.actionId = $"{Guid.NewGuid()}";
        addActionHighLogicTrigger.isSequenced = true;
        addActionHighLogicTrigger.isOneShot = true;
        addActionHighLogicTrigger.activeActionObject = beginActionObject;

        // Configurate the actions.

        var initAction = initActionObject.AddComponent<RunDelegateAction>();
        initAction.beginActionDelegate = initDelegate;

        Action<ActionSource> beginDelegate = (x) =>
        {
            
            rendererContainerObject.transform.localScale = Vector3.zero;
            ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.ResetAllAnimatorTriggers();
            ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.SetTrigger(ANIMATION_TRIGGER_EMOTE_REACT_KEY_ITEM);
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

        var moveItemAction = moveItemActionObject.AddComponent<MoveObjectAction>();
        moveItemAction.moveObject = rendererContainerObject;
        moveItemAction.doMove = true;
        moveItemAction.doScale = true;
        moveItemAction.moveInterval = MOVE_ITEM_INTERVAL;
        moveItemAction.scaleInterval = MOVE_ITEM_INTERVAL;
        moveItemAction.startTransform = itemStartPositionObject.transform;
        moveItemAction.finishTransform = itemFinishPositionObject.transform;

        var delayAction = delayActionObject.AddComponent<DelayAction>();
        delayAction.delayInterval = DELAY_INTERVAL;

        Action<ActionSource> endDelegate = (x) =>
        {
            ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.ResetAllAnimatorTriggers();
            ActiveSceneHighLogic.G.CachedPlayer.playerAnimator.SetTrigger(ANIMATION_TRIGGER_IDLE);
            ActiveSceneHighLogic.G.CachedCamcorder.ChangeState(CamcorderStateId.Orbit);
            GameObject.Destroy(itemObject);
        };

        var endAction = endActionObject.AddComponent<RunDelegateAction>();
        endAction.beginActionDelegate = endDelegate;

        // Chain the actions.
        beginAction.nextActionObject = movePlayerActionObject;
        movePlayerAction.nextActionObject = moveItemActionObject;
        moveItemAction.nextActionObject = delayActionObject;
        delayAction.nextActionObject = endActionObject;
        endAction.nextActionObject = null;

        return addActionHighLogicTrigger;
    }
}
