//===================================================
//Author      : DRB
//CreateTime  ：10/30/2017 2:40:11 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuPaiJiu;
using UnityEngine.UI;
using System;

public class UIBtnGuPaiJiuViewBase : UIViewBase
{
    [SerializeField]
    private EnumBtn btnObj;

    public  GameObject TimeObj; 
    public  Text m_TextCountDown;
    [HideInInspector]
    public bool isOpenTime = false;//是否开启倒计时 
    [HideInInspector]
    public  float m_CurrentTime;//倒计时时间
    public Action onComplete;

    public EnumBtn BtnObj
    {
        get
        {
            return btnObj;
        }
    }

    protected virtual void OnUpdate() { }

    //计算时间（对内接口）
    public  void OpenInvertedTime(long drawTime)
    {
        long currentTime = TimeUtil.GetTimestampMS();//获取当前时间                                                          
        int countTime = (int)(drawTime + GlobalInit.Instance.TimeDistance - TimeUtil.GetTimestampMS());
        int s = Mathf.RoundToInt(countTime / 1000f);
        Debug.Log(s+"                                切牌时间");
        if (s > 0)
        {
            TimeObj.SetActive(true);
            isOpenTime = true;
            SetTime(s);
        }
    }

    public void SetTime(int second)
    {
        m_CurrentTime = second;
    }

    void Update()
    {
        if (isOpenTime)
        {
            m_CurrentTime -= Time.deltaTime;
            if (m_CurrentTime <= 0)
            {
                isOpenTime = false;
                m_CurrentTime = 0;
                TimeObj.SetActive(false);
                if (onComplete!=null) onComplete();               
            }
            SetTimeCount((int)m_CurrentTime);
        }       
    }
    private void SetTimeCount(int second)
    {
        if (m_TextCountDown != null)
        {
            m_TextCountDown.SafeSetText(second.ToString("0"));
        }
    }



}
