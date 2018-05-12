//===================================================
//Author      : DRB
//CreateTime  ：12/4/2017 2:09:35 PM
//Description ：
//===================================================
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class PageView : UIItemBase    //, IBeginDragHandler, IEndDragHandler
{
    //private ScrollRect rect;                        //滑动组件  
    //private float targethorizontal = 0;             //滑动的起始坐标  
    //private bool isDrag = false;                    //是否拖拽结束  
    //private List<float> posList = new List<float>();            //求出每页的临界角，页索引从0开始  
    //private int currentPageIndex = -1;
    //public Action<int> OnPageChanged;

    //private bool stopMove = true;
    //public float smooting = 4;      //滑动速度  
    //public float sensitivity = 0;
    //private float startTime;

    //private float startDragHorizontal;

    //[SerializeField]
    //private Toggle[] toggleArray;
    //[SerializeField]
    //private PageView pageView;

    //void Awake()
    //{
    //    rect = transform.GetComponent<ScrollRect>();
    //    float horizontalLength = rect.content.rect.width - GetComponent<RectTransform>().rect.width;
    //    posList.Add(0);
    //    for (int i = 1; i < rect.content.transform.childCount - 1; i++)
    //    {
    //        posList.Add(GetComponent<RectTransform>().rect.width * i / horizontalLength);
    //    }
    //    posList.Add(1);

    //    toggleArray[0].isOn = true;
    //    pageView.OnPageChanged = pageChanged;
    //}

    //void pageChanged(int index)
    //{
    //    for (int i = 0; i < toggleArray.Length; i++)
    //    {
    //        if (index == i)
    //        {
    //            toggleArray[i].isOn = true;
    //        }
    //    }
    //}
    //void Update()
    //{
    //    if (!isDrag && !stopMove)
    //    {
    //        startTime += Time.deltaTime;
    //        float t = startTime * smooting;
    //        rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targethorizontal, t);
    //        if (t >= 1)
    //            stopMove = true;
    //    }
    //}

    //public void pageTo(int index)
    //{
    //    if (index >= 0 && index < posList.Count)
    //    {
    //        rect.horizontalNormalizedPosition = posList[index];
    //        SetPageIndex(index);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("页码不存在");
    //    }
    //}
    //private void SetPageIndex(int index)
    //{
    //    if (currentPageIndex != index)
    //    {
    //        currentPageIndex = index;
    //        if (OnPageChanged != null)
    //            OnPageChanged(index);
    //    }
    //}

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    isDrag = true;
    //    startDragHorizontal = rect.horizontalNormalizedPosition;
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    float posX = rect.horizontalNormalizedPosition;
    //    posX += ((posX - startDragHorizontal) * sensitivity);
    //    posX = posX < 1 ? posX : 1;
    //    posX = posX > 0 ? posX : 0;
    //    int index = 0;
    //    float offset = Mathf.Abs(posList[index] - posX);
    //    for (int i = 1; i < posList.Count; i++)
    //    {
    //        float temp = Mathf.Abs(posList[i] - posX);
    //        if (temp < offset)
    //        {
    //            index = i;
    //            offset = temp;
    //        }
    //    }
    //    SetPageIndex(index);

    //    targethorizontal = posList[index]; //设置当前坐标，更新函数进行插值  
    //    isDrag = false;
    //    startTime = 0;
    //    stopMove = false;
    //}

    #region 
    //private ScrollRect scrollRect;//添加滚动矩形
    ////private float[] pageArray = { 0, 0.5f, 1 };//添加页数数组
    //private List<float> pageArray = new List<float>();
    //private float targetHorizontalPosition = 0;//要移动的目标
    //public float speed = 0.5f;
    //private bool isDraging = false;//是否滑动
    //public Toggle[] toggleArray;


    //[SerializeField]
    //private Text m_TextKey;
    //[SerializeField]
    //private Text m_TextValue;
    //private string m_Content;
    //[SerializeField]
    //private Button btncopy;
    //private Action<string> m_OnClick;


    //protected override void OnAwake()
    //{
    //    base.OnAwake();
    //    btncopy.gameObject.GetOrCreatComponent<Button>().onClick.AddListener(OnBtnClick);

    //}

    //private void OnBtnClick()
    //{
    //    if (m_OnClick != null)
    //    {
    //        m_OnClick(m_Content);
    //    }
    //}

    //public void SetUI(string key, string value, Action<string> onClick)
    //{
    //    m_TextKey.SafeSetText(key);
    //    m_TextValue.SafeSetText(value);
    //    m_Content = value;
    //    m_OnClick = onClick;

    //}








    //public void OnBeginDrag(PointerEventData eventData)//滑动开始方法
    //{
    //    isDraging = true;
    //}
    //public void OnEndDrag(PointerEventData eventData)//滑动结束方法
    //{
    //    isDraging = false;
    //    float posX = scrollRect.horizontalNormalizedPosition;//得到当前滑动位置
    //    int index = 0;//添加当前页
    //    float offset = Mathf.Abs(pageArray[index] - posX);//得到页数与当前位置的偏移
    //    for (int i = 1; i < pageArray.Count; i++)
    //    {
    //        float offsetTemp = Mathf.Abs(pageArray[i] - posX);//得到当前页与当前位置之间的偏移
    //        if (offsetTemp < offset)//当前页与当前位置之间的差小于每一页与当前位置的差时
    //        {
    //            index = i;//得到当前页数
    //            offset = offsetTemp;//得到离当前位置最近的页数
    //        }
    //    }
    //    targetHorizontalPosition = pageArray[index];//得到目标位置
    //    toggleArray[index].isOn = true;
    //}
    //void Start()
    //{
    //    scrollRect = GetComponent<ScrollRect>();
    //    for (int i = 0; i < scrollRect.content.childCount; i++)
    //    {
    //        pageArray.Add(i / (float)(scrollRect.content.childCount - 1));
    //    }
    //}
    //void Update()
    //{
    //    if (isDraging == false)
    //        scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, targetHorizontalPosition, Time.deltaTime * speed);
    //}

    //public void MoveToPage1(bool isOn)
    //{
    //    if (isOn)
    //    {
    //        targetHorizontalPosition = pageArray[0];
    //    }
    //}
    //public void MoveToPage2(bool isOn)
    //{
    //    if (isOn)
    //    {
    //        targetHorizontalPosition = pageArray[1];
    //    }
    //}
    //public void MoveToPage3(bool isOn)
    //{
    //    if (isOn)
    //    {
    //        targetHorizontalPosition = pageArray[2];
    //    }

    //}
    #endregion

    private ScrollRect scrollRect;//添加滚动矩形
   
    public List<float> m_PageArray = new List<float>();
    
    public float targetHorizontalPosition = 0;//要移动的目标
    public float speed = 10f;//滑动速度
    //[HideInInspector]
    public List<Toggle> toggleArray;//toggle列表
    [SerializeField]
    private float m_TotalTime = 3f;//每隔多久滑动一次
    private float timer = 0;
    private int m_Index = 0;//toggle索引
    private bool isOn=false;
    [SerializeField]
    private Transform m_moveTran;

    Vector3 m_ve3;
    UIItemNoticeThree m_item;  
    private List<UIItemNoticeThree> m_noticethree;

    void Awake()
    {
        m_ve3 = m_moveTran.position;
    }

    public void Init(List<UIItemNoticeThree> m_noticethree)
    {       
        this.m_noticethree = m_noticethree;    
        scrollRect = GetComponent<ScrollRect>();
        //timer = 0;
        //isOn = false;
        //m_Index = 0;
        //scrollRect.horizontalNormalizedPosition = 0;
        m_PageArray.Clear();
        for (int i = 0; i < toggleArray.Count; i++)
        {         
            m_PageArray.Add(i / (float)(toggleArray.Count - 1));
        }      
    }
    void Update()
    {
        if (m_PageArray.Count > 1)
        {
            if (timer <= m_TotalTime)
            {
                timer += Time.deltaTime;
            }
            else
            {               
                if (m_Index >= m_PageArray.Count)
                {
                    m_Index = 0;
                }             
               // targetHorizontalPosition = m_PageArray[m_Index];//得到目标位置  
                targetHorizontalPosition = m_PageArray[1];//得到目标位置   
                int index = m_Index + 1 >= m_noticethree.Count ? 0: m_Index + 1;
                toggleArray[index].isOn = true;            
                isOn = true;          
            }
            if (isOn)
            {
                scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, targetHorizontalPosition, Time.deltaTime * speed);
                if (targetHorizontalPosition - scrollRect.horizontalNormalizedPosition <= 0.0001f)
                {                                             
                   // int index = m_Index - 1 >= 0 ? m_Index - 1 : m_noticethree.Count-1;                
                    m_noticethree[m_Index].transform.SetSiblingIndex(m_noticethree.Count-1);                   
                    scrollRect.horizontalNormalizedPosition = 0;
                    m_moveTran.position = m_ve3;
                    m_Index++;
                    timer = 0;
                    isOn = false;
                }
            }
        }
       
       
    }
}
