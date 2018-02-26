using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EventTriggerListener : EventTrigger
{

	public delegate void VoidDelegate (GameObject go);
	public VoidDelegate onClick;//点击
	public VoidDelegate onDown;//按下
	public VoidDelegate onEnter;//移入
	public VoidDelegate onExit;//移出
	public VoidDelegate onUp;//按起
	public VoidDelegate onSelect;//选中
	public VoidDelegate onUpdateSelect;//持续选中
	public VoidDelegate onMove;//移动
	
	static public EventTriggerListener Get (GameObject go)
	{
		EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
		if (listener == null) listener = go.AddComponent<EventTriggerListener>();
		return listener;
	}
	public override void OnPointerClick(PointerEventData eventData)
	{
        if (onClick != null)
        {
			if (GameMgr.audioMgr != null)
			{
				GameMgr.audioMgr.PlayeSound("click");
			}

            onClick(gameObject);
        } 
	}
	public override void OnPointerDown (PointerEventData eventData){
		if(onDown != null) onDown(gameObject);
	}
	public override void OnPointerEnter (PointerEventData eventData){
		if(onEnter != null) onEnter(gameObject);
	}
	public override void OnPointerExit (PointerEventData eventData){
		if(onExit != null) onExit(gameObject);
	}
	public override void OnPointerUp (PointerEventData eventData){
		if(onUp != null) onUp(gameObject);
	}
	public override void OnSelect (BaseEventData eventData){
		if(onSelect != null) onSelect(gameObject);
	}
	public override void OnUpdateSelected (BaseEventData eventData){
		if(onUpdateSelect != null) onUpdateSelect(gameObject);
	}
	public override void OnMove(AxisEventData eventData)
	{
		if(onMove != null) onMove(gameObject);
	}
}
