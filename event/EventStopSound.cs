using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;

public class EventStopSound : MonoBehaviour, IEventController
{
    private AudioSource source;

    public GameObject nextEventSource;

    public GameObject audioSourceObject;

    private void Start()
    {
        if (audioSourceObject == null)
            audioSourceObject = gameObject;

        source = audioSourceObject.GetComponent<AudioSource>();

        if (source == null)
        {
            source = this.gameObject.AddComponent<AudioSource>();
        }
    }

    public void FinishEvent(GameEvent gameEvent) { }

    public string GetEventType()
    {
        return GameConstants.EVENT_TYPE_STOP_SOUND;
    }

    public string GetEventDescription()
    {
        return GameConstants.EVENT_TYPE_STOP_SOUND + ((audioSourceObject == null) ? "_null" : "_" + audioSourceObject.name); ;
    }

    public bool GetIsEventComplete(GameEvent gameEvent)
    {
        return true;
    }

    public bool IsGameEventComplete(GameEvent gameEvent)
    {
        return true;
    }

    public bool GetIsUpdateComplete(GameEvent gameEvent)
    {
        return true;
    }

    public GameObject GetNextEventSource()
    {
        return nextEventSource;
    }

    public void UpdateEvent(GameEvent gameEvent) { }

    public void StartEvent(GameEvent gameEvent)
    {
        source.Stop();
    }

    public void ResetEvent(GameEvent gameEvent) { }

    private void OnDrawGizmos()
    {
        EventStaticMethods.DrawEventGizmo(this, this.gameObject, nextEventSource);
    }
}
