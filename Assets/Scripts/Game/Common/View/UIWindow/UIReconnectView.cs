//===================================================
//Author      : DRB
//CreateTime  ：8/29/2017 4:29:18 PM
//Description ：断线重连界面
//===================================================
using UnityEngine;


public class UIReconnectView : MonoBehaviour 
{

    private float m_Timer;

    private bool m_isBeginShow;

    /// <summary>
    /// 延迟显示时间
    /// </summary>
    private const float DELAY_SHOW_TIME = 0.5f;


    private void Update()
    {
        //if (m_isBeginShow)
        //{
        //    if (Time.realtimeSinceStartup - m_Timer > DELAY_SHOW_TIME)
        //    {
        //        gameObject.SetActive(true);
        //        m_isBeginShow = false;
        //    }
        //}
    }


    public void Show()
    {
        //if (!m_isBeginShow)
        //{
        //    m_Timer = Time.realtimeSinceStartup;
        //    m_isBeginShow = true;
        //}

        gameObject.SetActive(true);
    }


    public void Close()
    {
        m_isBeginShow = false;
        gameObject.SetActive(false);
    }
}
