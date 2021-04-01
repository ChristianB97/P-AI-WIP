using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.Events;

public class UIActionEventConstructor
{
    public void ConstructEvent(GameObject itemUI, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = itemUI.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = itemUI.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }
}
