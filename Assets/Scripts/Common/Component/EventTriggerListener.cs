//===================================================
//Author      : DRB
//CreateTime  ：7/5/2016 11:34:36 PM
//Description ：UI事件触发监听
//===================================================
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class EventTriggerListener : UnityEngine.EventSystems.EventTrigger
{
    public delegate void DelegateHandler(PointerEventData eventData);
    public DelegateHandler onDown;
    public DelegateHandler onUp;

    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate onClick;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;
    public static EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null) listener = go.AddComponent<EventTriggerListener>();
        return listener;
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null) onClick(gameObject);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null) onDown(eventData);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null) onEnter(gameObject);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null) onExit(gameObject);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null) onUp(eventData);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(gameObject);
    }
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null) onUpdateSelect(gameObject);
    }
}