//===================================================
//Author      : DRB
//CreateTime  ：3/30/2017 10:11:33 AM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragView : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public Action OnDragBegin;
    public Action<Vector2> OnDragging;
    public Action OnDragEnd;


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (OnDragBegin != null)
        {
            OnDragBegin();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragging != null)
        {
            OnDragging(eventData.delta);
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (OnDragEnd != null)
        {
            OnDragEnd();
        }
    }
}
