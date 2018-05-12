//===================================================
//Author      : DRB
//CreateTime  ：10/16/2017 2:28:29 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class GuPaiJiuRubCtrl : MonoBehaviour, IDragHandler
{   
    private Vector2 m_RubCtrlV2;
    private float m_Width;//宽
    private float m_Heigth;//高
    private GameObject m_Temp;
    private Vector2 m_Ve2;
    public Action<Vector3> onComplete;
   
    void Awake()
    {       
        m_RubCtrlV2 = this.gameObject.GetComponent<RectTransform>().anchoredPosition;
        m_Width = this.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        m_Heigth = this.gameObject.GetComponent<RectTransform>().sizeDelta.y;
        m_Temp = this.gameObject;
       
    }

	void Update () {
        if (Mathf.Abs(m_Ve2.x- m_RubCtrlV2.x)>= m_Width*0.8f|| Mathf.Abs(m_Ve2.y - m_RubCtrlV2.y) >= m_Heigth*0.8f)
        {           
            onComplete(m_RubCtrlV2);
            m_Ve2 = Vector2.zero;
        }
	}



 
    public void OnDrag(PointerEventData data)
    {
        if (m_Temp != null)
        {
            
            Vector2 screenPos = UIViewManager.Instance.CurrentUIScene.CurrentCanvas.worldCamera.WorldToScreenPoint(m_Temp.transform.position);          
            screenPos += data.delta;                     
            Vector2 localPointerPosition;
            // 获取本地鼠标位置  
            //if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Temp.transform.parent.gameObject.GetComponent<RectTransform>(), screenPos, data.pressEventCamera, out localPointerPosition))
            //{
            //    m_Temp.GetComponent<RectTransform>().anchoredPosition = localPointerPosition;
            //    m_Ve2 = localPointerPosition;
            //}
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Temp.transform.parent as RectTransform, screenPos, data.pressEventCamera, out localPointerPosition))
            {
                m_Temp.GetComponent<RectTransform>().anchoredPosition = localPointerPosition;
                m_Ve2 = localPointerPosition;
            }
        }
    }


}
