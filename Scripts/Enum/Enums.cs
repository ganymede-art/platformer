using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HighLogicStateId
{
    None,
    Init,
    Play,
    Film,
    Load,
    Stat,
    Dead,
    Menu,
    Conf,
}

public enum UserInterfaceStatus
{
    BeginDisable,
    Disabled,
    BeginEnable,
    Enabled,
}

public enum UserInterfaceWidgetType
{
    ColourLerp,
    TranslateLerp,
    MessageBox,
    Time,
    Health,
    Oxygen,
    Ammo,
    Money,
    Choices,
    PointSelected,
    KeyItems,
    CurrentScenePopup,
    Item,
    Timer,
}

public enum ActionStatus
{
    None,
    Started,
    Updating,
    Finished,
}

public enum UserInterfaceWidgetStatus
{
    BeginDisable,
    Disabled,
    BeginEnable,
    Enabled,
}

public enum LoadingSceneStatus
{
    NotLoading,
    BeginLoading,
    EndLoading,
}

public enum MusicStatus
{
    Stopped,
    FadeIn,
    SwitchClips,
    Playing,
    FadeOut,
}

public enum SwitchStatus
{
    None,
    On,
    Off,
    TurningOn,
    TurningOff,
}

public enum PropStatus
{
    Resting,
    Shaking,
    Falling,
    Recovering,
    Coasting,
    Closed,
    Open,
    Closing,
    Opening,
    Uncollected,
    Collected,
}

public enum PlayerStateId
{
    Default,
    Jump,
    DoubleJump,
    WaterDefault,
    DiveUnderwater,
    Attack,
    AttackRecoil,
    AttackUnderwater,
    Hurt,
    Crouch,
    HighJump,
    Lunge,
    Die,
    Slam,
    Shoot,
    UseKeyItem,
}

public enum PlayerBehaviourId
{
    GroundCheck,
    Gravity,
    FootstepEffect,
    Water,
    Damage,
    ManagedEffect,
    Interact,
    KeyItemUse,
    Oxygen,
    Bounds,
}

public enum PlayerFilmStatus
{
    None,
    FaceAction,
    FaceDirection,
    FaceTarget,
}

public enum MobStateId
{
    Loiter,
    Wander,
    Hurt,
    Die,
    PreDead,
    Dead,
    Destroy,
    SpottedPlayer,
    TurnAround,
    ChasePlayer,
    StampedePlayer,
    StampedeWall,
    Recoil,
    Daze,
    JumpWander,
}

public enum MobBehaviourId
{
    GroundCheck,
    DistancePlayerCheck,
    WallCheck,
    Damage,
}

public enum CamcorderStateId
{
    Orbit,
    Fixed,
    Reorient,
}

public enum CamcorderBehaviourId
{
    Water,
}

public enum ItemStatus
{
    None,
    NotCollected,
    Collecting,
    Collected,
}

public enum ItemType
{
    Primary,
    Secondary,
    Tertiary,
    Quaternary,
}

public enum SoundType
{
    Player,
    Music,
    Environment,
    Mob,
    Prop,
    UserInterface,
}

public enum NavigationNodeType
{
    Perch,
    Fly,
}

public enum NpcStatus
{
    Idle,
    Reset,
    TrackingPlayer,
    TrackingTarget,
}

public enum ScenicFlyingNpcStatus
{
    Perching,
    Flying,
}

public enum ActionType
{
    PlaySound,
    PlayMusic,
    MoveObject,
    MovePlayer,
    MessageBox,
    BeginFixedCamcorder,
    BeginOrbitCamcorder,
    BeginReorientCamcorder,
    Choices,
    RunDelegate,
    Delay,
    SetObjectsActive,
    ModifyPlayerStats,
    ModifyPlayerAbilities,
    SetMusicTargetDynamicVolume,
    Save,
    SetPlayerAnimatorTrigger,
    SetAnimatorTrigger,
    BeginBlackOverlay,
    EndBlackOverlay,
    SetBoolVariable,
    SetIntVariable,
    SetStringVariable,
    AddAction,
    OverrideSwitchStatus,
}

public enum DamageType
{
    None,
    Player,
    PlayerIndirect,
    PlayerPassive,
    Mob,
    MobIndirect,
    MobPassive,
    Static,
    StaticIndirect,
    StaticPassive,
}

public enum PeriodType
{
    Early,
    Dawn,
    Morning,
    Afternoon,
    Dusk,
    Late,
}

public enum DayType
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday,
}

public enum EmoteType
{
    Default,
    Blinking,
    Sleeping,
    Happy,
    Sad,
    Calm,
    Angry,
    Shocked,
    Dead,
}

public enum ButtonType
{
    North,
    East,
    South,
    West,
}

