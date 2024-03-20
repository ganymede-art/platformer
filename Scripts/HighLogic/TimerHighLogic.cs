using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerHighLogic : MonoBehaviour
{
    private List<Timer> timers;
    private TimerArgs timerArgs;

    // Public properties.
    public static TimerHighLogic G => GameHighLogic.G == null ? null : GameHighLogic.G.TimerHighLogic;
    public List<Timer> Timers => timers;

    // Events.
    public event EventHandler<TimerArgs> TimerAdded;
    public event EventHandler<TimerArgs> TimerUpdated;
    public event EventHandler<TimerArgs> TimerCompleted;

    private void Awake()
    {
        timers = new List<Timer>();
        timerArgs = new TimerArgs();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        timers.Clear();
    }

    private void Update()
    {
        if (StateHighLogic.G.ActiveState != HighLogicStateId.Play)
            return;

        if (timers.Count == 0)
            return;
        for(int i = timers.Count - 1; i >= 0; i--)
        {
            timers[i].timerValue -= Time.deltaTime;
            timerArgs.timerId = timers[i].timerId;
            timerArgs.timerValue = timers[i].timerValue;
            timerArgs.timerInterval = timers[i].timerInterval;
            TimerUpdated?.Invoke(this, timerArgs);
            if (timers[i].timerValue <= 0.0F)
                CompleteTimer(i);
        }
    }

    private void CompleteTimer(int timerIndex)
    {
        timerArgs.timerId = timers[timerIndex].timerId;
        timerArgs.timerValue = 0.0F;
        timerArgs.timerInterval = timers[timerIndex].timerInterval;
        TimerCompleted?.Invoke(this, timerArgs);
        timers.RemoveAt(timerIndex);
    }

    public void AddTimer(string timerId, float timerInterval)
    {
        var timer = new Timer
        {
            timerId = timerId,
            timerValue = timerInterval,
            timerInterval = timerInterval,
        };
        timers.Add(timer);
        timerArgs.timerId = timer.timerId;
        timerArgs.timerValue = timer.timerValue;
        timerArgs.timerInterval = timer.timerInterval;
        TimerAdded?.Invoke(this, timerArgs);
    }

    public void RemoveTimer(string timerId)
    {
        for (int i = timers.Count; i >= 0; i--)
            if (timers[i].timerId == timerId)
                timers.RemoveAt(i);
    }
}

public class Timer
{
    public string timerId;
    public float timerValue;
    public float timerInterval;
}

public class TimerArgs
{
    public string timerId;
    public float timerValue;
    public float timerInterval;
}
