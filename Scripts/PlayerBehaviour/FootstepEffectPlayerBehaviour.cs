using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public class FootstepEffectPlayerBehaviour : MonoBehaviour, IBehaviour<Player, PlayerBehaviourId>
{
    // Private fields.
    private IRemoteAnimationEvent remoteAnimationEvent;
    private Player cachedPlayer;
    private GroundData activeGroundData;

    // Public properties.
    public PlayerBehaviourId BehaviourId => PlayerBehaviourId.FootstepEffect;

    // Public fields.
    public GameObject remoteAnimationEventObject;
    public GroundData defaultGroundData;
    public GroundData waterGroundData;

    private void Awake()
    {
        remoteAnimationEvent = remoteAnimationEventObject.GetComponent<IRemoteAnimationEvent>();
        remoteAnimationEvent.AnimationEventTriggered += OnAnimationEventTriggered;
    }

    private void Start()
    {
        cachedPlayer = ActiveSceneHighLogic.G.CachedPlayer;
    }

    public void BeginBehaviour(Player controller, Dictionary<string, object> args = null) { }
    public void UpdateBehaviour(Player c) { }
    public void FixedUpdateBehaviour(Player c) { }
    public void EndBehaviours(Player controller) { }

    public void OnAnimationEventTriggered(object sender, RemoteAnimationEventArgs args)
    {
        if (args.value != ANIMATION_EVENT_NAME_STEP)
            return;

        if (cachedPlayer == null)
            return;

        activeGroundData = null;

        bool isGroundDataAvailable = false;

        if(cachedPlayer.GroundCheck.SpherecastGroundObject != null)
            isGroundDataAvailable = ActiveSceneHighLogic.G.GroundDatas.TryGetValue
                ( cachedPlayer.GroundCheck.SpherecastGroundObject
                , out activeGroundData);

        if(!isGroundDataAvailable)
        {
            activeGroundData = defaultGroundData;
        }

        if (cachedPlayer.Water.IsWaterCollision)
            activeGroundData = waterGroundData;

        cachedPlayer.footstepSound.PlayPitchedOneShot
            ( activeGroundData.groundFootstepSound
            , SettingsHighLogic.G.PlayerVolume
            , SFX_MIN_PT
            , SFX_MAX_PT);
    }
}
