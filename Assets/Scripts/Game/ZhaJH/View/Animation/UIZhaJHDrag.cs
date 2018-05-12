//===================================================
//Author      : DRB
//CreateTime  ：8/1/2017 7:22:14 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIZhaJHDrag : MonoBehaviour, IDragHandler, IBeginDragHandler
{
   

    private GameObject m_Temp;

    //private void Awake()
    //{
    //    m_DragView.OnDragBegin = OnDragBegin;
    //    m_DragView.OnDragging = OnDragging;
    //}



    public void OnBeginDrag(PointerEventData eventData)
    {
        m_Temp = this.gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //GetComponent<RectTransform>().pivot.Set(0, 0);
        //this.transform.position = Input.mousePosition + UIViewManager.Instance.CurrentUIScene.CurrentCanvas.worldCamera.ScreenToWorldPoint(eventData.delta);
        if (m_Temp != null)
        {
            Vector2 screenPos = UIViewManager.Instance.CurrentUIScene.CurrentCanvas.worldCamera.WorldToScreenPoint(m_Temp.transform.position);
            Vector3 worldPos;
            screenPos += eventData.delta;
            RectTransform rect = UIViewManager.Instance.CurrentUIScene.CurrentCanvas.GetComponent<RectTransform>();

            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, screenPos, UIViewManager.Instance.CurrentUIScene.CurrentCanvas.worldCamera, out worldPos))
            {
                m_Temp.transform.position = worldPos;
            }
            m_Temp.transform.position = new Vector3(m_Temp.transform.position.x + eventData.delta.x, m_Temp.transform.position.y + eventData.delta.y, m_Temp.transform.position.z);
        }
    }


    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
   



    private void OnDragBegin()
    {        
       
    }

    


    //private void OnDragging(Vector2 deltaPos)
    //{
    //    if (m_Temp != null)
    //    {
    //        Vector2 screenPos = UIViewManager.Instance.CurrentUIScene.CurrentCanvas.worldCamera.WorldToScreenPoint(m_Temp.transform.position);
    //        Vector3 worldPos;
    //        screenPos += deltaPos;
    //        RectTransform rect = UIViewManager.Instance.CurrentUIScene.CurrentCanvas.GetComponent<RectTransform>();

    //        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, screenPos, UIViewManager.Instance.CurrentUIScene.CurrentCanvas.worldCamera, out worldPos))
    //        {
    //            m_Temp.transform.position = worldPos;
    //        }
    //        m_Temp.transform.position = new Vector3(m_Temp.transform.position.x + deltaPos.x, m_Temp.transform.position.y + deltaPos.y, m_Temp.transform.position.z);
    //    }
    //}


}
