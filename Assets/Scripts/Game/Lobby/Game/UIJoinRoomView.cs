//===================================================
//Author      : DRB
//CreateTime  ：3/8/2017 6:09:54 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIJoinRoomView : UIWindowViewBase
{
    [SerializeField]
    private Button[] m_Buttons;
    [SerializeField]
    private Text[] m_Texts;

    /// <summary>
    /// 房间号
    /// </summary>
    private string m_RoomId;
    /// <summary>
    /// 已经输入的数量
    /// </summary>
    private int m_nAlreadyInputCount;
    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        for (int i = 0; i < m_Buttons.Length; ++i)
        {
            if (m_Buttons[i].name.Equals(go.name))
            {
                if (m_nAlreadyInputCount == m_Texts.Length) return;
                m_RoomId += i.ToString();
                m_Texts[m_nAlreadyInputCount].SafeSetText(i.ToString());
                ++m_nAlreadyInputCount;
               
                if (m_nAlreadyInputCount == m_Texts.Length)
                {
                    SendNotification(ConstDefine.BtnJoinRoomViewJoin, new object[1] { m_RoomId.ToInt() });
                }
                return;
            }
        }
        switch (go.name)
        {
            case "btnResetRoomId":
                ResetUI();
                break;
            case "btnDeleteRoomId":
                DeleteRoomId();
                break;
        }
    }

    private void ResetUI()
    {
        for (int i = 0; i < m_Texts.Length; ++i)
        {
            m_Texts[i].SafeSetText("");
        }
        m_RoomId = string.Empty;
        m_nAlreadyInputCount = 0;
    }

    private void DeleteRoomId()
    {
        if (m_nAlreadyInputCount == 0) return;
        --m_nAlreadyInputCount;
        m_RoomId = m_RoomId.Substring(0,m_nAlreadyInputCount);
        m_Texts[m_nAlreadyInputCount].SafeSetText("");
    }


    public override void Hide()
    {
        base.Hide();
        ResetUI();
    }


}
