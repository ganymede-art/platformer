using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Camcorder : MonoBehaviour
    , IStateMachine<Camcorder, CamcorderStateId>
    , IBehaviourMachine<Camcorder,CamcorderBehaviourId>
{
    // Private fields.
    private float stateTimer;
    private Dictionary<CamcorderStateId, IState<Camcorder, CamcorderStateId>> states;
    private Dictionary<CamcorderBehaviourId, IBehaviour<Camcorder, CamcorderBehaviourId>> behaviours;
    private CamcorderStateId activeState;
    private CamcorderStateId previousState;

    private PostProcessVolume postVolume;
    private ColorGrading postColourGrading;
    private LensDistortion postLensDistortion;

    private AudioListener audioListener;
    private AudioLowPassFilter audioLowPassFilter;

    // Public properties.
    public float StateTimer => stateTimer;
    public Dictionary<CamcorderStateId, IState<Camcorder, CamcorderStateId>> States => states;
    public Dictionary<CamcorderBehaviourId, IBehaviour<Camcorder, CamcorderBehaviourId>> Behaviours => behaviours;
    public CamcorderStateId ActiveState => activeState;
    public CamcorderStateId PreviousState => previousState;

    public PostProcessVolume PostVolume => postVolume;
    public ColorGrading PostColourGrading => postColourGrading;
    public LensDistortion PostLensDistortion => postLensDistortion;

    public AudioListener AudioListener => audioListener;
    public AudioLowPassFilter AudioLowPassFilter => audioLowPassFilter;

    // Hidden public fields.

    [NonSerialized] public float x;
    [NonSerialized] public float y;

    [NonSerialized] public float xInput;
    [NonSerialized] public float yInput;
    [NonSerialized] public float zInput;

    [NonSerialized] public float xSensitivity;
    [NonSerialized] public float ySensitivity;
    [NonSerialized] public float zSensitivity;

    [NonSerialized] public Transform target;
    [NonSerialized] public float targetDistance;

    [NonSerialized] public float distance;

    // Exposed public fields.
    public GameObject camcorderStatesObject;
    public GameObject camcorderBehavioursObject;

    public Camera camcorderCamera;

    private void Awake()
    {
        stateTimer = 0.0F;
        states = new Dictionary<CamcorderStateId, IState<Camcorder, CamcorderStateId>>();
        behaviours = new Dictionary<CamcorderBehaviourId, IBehaviour<Camcorder, CamcorderBehaviourId>>();
        activeState = CamcorderStateId.Orbit;
        previousState = CamcorderStateId.Orbit;

        var statesToAdd = camcorderStatesObject
            .GetComponentsInChildren<IState<Camcorder, CamcorderStateId>>();
        foreach (var stateToAdd in statesToAdd)
            states.Add(stateToAdd.StateId, stateToAdd);

        var behavioursToAdd = camcorderBehavioursObject
            .GetComponentsInChildren<IBehaviour<Camcorder, CamcorderBehaviourId>>();
        foreach (var behaviourToAdd in behavioursToAdd)
            behaviours.Add(behaviourToAdd.BehaviourId, behaviourToAdd);

        // TODO initialise from settings.
        ySensitivity = 1.0F;
        xSensitivity = 1.0F;
        zSensitivity = 1.0F;

        // Initialise starting rotation.
        y = transform.rotation.eulerAngles.y;

        // Enable depth testing
        camcorderCamera.depthTextureMode = DepthTextureMode.Depth;
    }

    private void Start()
    {
        foreach (var behaviour in behaviours.Values)
            behaviour.BeginBehaviour(this);

        states[activeState].BeginState(this);

        postVolume = GetComponent<PostProcessVolume>();
        postVolume.profile.TryGetSettings(out postColourGrading);
        postVolume.profile.TryGetSettings(out postLensDistortion);
        postColourGrading.enabled.Override(false);
        postLensDistortion.enabled.Override(false);

        audioListener = GetComponent<AudioListener>();
        audioLowPassFilter = GetComponent<AudioLowPassFilter>();
        audioLowPassFilter.enabled = false;
    }

    private void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play
            && StateHighLogic.G.ActiveState != HighLogicStateId.Film)
            return;

        foreach (var behaviour in behaviours.Values)
            behaviour.UpdateBehaviour(this);

        states[activeState].UpdateState(this);

        stateTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play
            && StateHighLogic.G.ActiveState != HighLogicStateId.Film)
            return;

        foreach (var behaviour in behaviours.Values)
            behaviour.FixedUpdateBehaviour(this);

        states[activeState].FixedUpdateState(this);
    }

    public void ChangeState(CamcorderStateId stateId, Dictionary<string, object> args = null)
    {
        states[activeState].EndState(this);
        foreach (var behaviour in behaviours.Values)
            behaviour.EndBehaviours(this);
        previousState = activeState;
        activeState = stateId;
        stateTimer = 0.0F;
        foreach (var behaviour in behaviours.Values)
            behaviour.BeginBehaviour(this);
        states[activeState].BeginState(this, args);  
    }
}
