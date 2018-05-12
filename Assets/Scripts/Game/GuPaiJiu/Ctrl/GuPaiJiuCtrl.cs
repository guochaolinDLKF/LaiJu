//===================================================
//Author      : DRB
//CreateTime  ：9/9/2017 12:31:29 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class GuPaiJiuCtrl : MonoBehaviour,IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [HideInInspector]
    public Action<bool> onComplete;////牌型检测回调


    [SerializeField]
    private Image mySprite;
    private Transform _parent;//当前UI对象的父物体
    private GameObject m_Temp;//

    [HideInInspector]
    public bool isBool=false;


    void Start()
    { }

    public void SetUI(Sprite sprite)
    {
        mySprite.sprite = sprite;
    }

    //开始拖拽
    public void OnBeginDrag(PointerEventData data)
    {
        _parent = transform.parent;        //记录当前物体的父物体
        transform.SetParent(this.transform.parent.parent.parent);//修改当前父物体
        this.GetComponent<Image>().raycastTarget = false;//当开始拖拽的时候,将射线检测关闭（UGUI的射线无法检测到当前的物体）;       
        m_Temp = this.gameObject;
    }
    //拖拽中
    public void OnDrag(PointerEventData data)
    {      
        if (m_Temp != null)
        {
            Vector2 screenPos = UIViewManager.Instance.CurrentUIScene.CurrentCanvas.worldCamera.WorldToScreenPoint(m_Temp.transform.position);           
            Vector3 worldPos;
            screenPos += data.delta;
            RectTransform rect = UIViewManager.Instance.CurrentUIScene.CurrentCanvas.GetComponent<RectTransform>();                 
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, screenPos, UIViewManager.Instance.CurrentUIScene.CurrentCanvas.worldCamera, out worldPos))
            {
                m_Temp.transform.position = worldPos;
            }        
        }
    }
    //拖拽结束
    public void OnEndDrag(PointerEventData data)
    {
        //如果鼠标在UGUI上返回true,否则返回false
        bool isonUGUI = EventSystem.current.IsPointerOverGameObject();     
        //如果射线检测到的物体的 tag 为 goods（物体） 的时候，就让两个物体进行交换
        if ( data.pointerCurrentRaycast.gameObject.tag == "poker")//需要交换物体的标签
        {            
            transform.SetParent(data.pointerCurrentRaycast.gameObject.transform.parent);         
            data.pointerCurrentRaycast.gameObject.transform.SetParent(_parent);
            data.pointerCurrentRaycast.gameObject.transform.localPosition = Vector3.zero;            
        }       
        else
        {
            transform.SetParent(_parent.transform);
        }
        this.GetComponent<Image>().raycastTarget = true;//当拖拽结束的时候,将射线检测开启（UGUI的射线可以检测到当前的物体）；       
        transform.localPosition = Vector3.zero;
        onComplete(isBool);//牌型检测回调
    }


}
