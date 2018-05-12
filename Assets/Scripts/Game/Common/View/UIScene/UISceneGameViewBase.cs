//===================================================
//Author      : DRB
//CreateTime  ：11/27/2017 1:57:43 PM
//Description ：游戏场景UI基类
//===================================================
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 游戏场景UI基类
/// </summary>
public class UISceneGameViewBase : UISceneViewBase
{
    [SerializeField]
    protected Button m_ButtonMicroPhone;//语音按钮
    [SerializeField]
    protected Button m_ButtonShare;//邀请按钮
    [SerializeField]
    protected Button m_ButtonChat;//聊天按钮
    [SerializeField]
    protected Button m_ButtonSetting;//设置按钮

    protected override void OnAwake()
    {
        base.OnAwake();
        if (m_ButtonMicroPhone != null)
        {
            EventTriggerListener.Get(m_ButtonMicroPhone.gameObject).onDown = OnBtnMacroDown;
            EventTriggerListener.Get(m_ButtonMicroPhone.gameObject).onUp = OnBtnMacroUp;
        }

        if (GameCtrl.Instance.GetRoomEntity() != null && GameCtrl.Instance.GetRoomEntity().isReplay)
        {
            m_ButtonMicroPhone.SafeSetActive(false);
            m_ButtonChat.SafeSetActive(false);
            m_ButtonSetting.SafeSetActive(false);
        }

        if (m_ButtonShare != null)
        {
            m_ButtonShare.SafeSetActive(GameCtrl.Instance.GetRoomEntity().matchId <= 0);
        }

        if (!SystemProxy.Instance.IsInstallWeChat)
        {
            m_ButtonShare.SafeSetActive(false);
        }
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);

        if (m_ButtonChat != null && go == m_ButtonChat.gameObject)
        {
            UIViewManager.Instance.OpenWindow(UIWindowType.Chat);
        }
        else if (m_ButtonSetting != null && go == m_ButtonSetting.gameObject)
        {
            UIViewManager.Instance.OpenWindow(UIWindowType.Setting);
        }
        else if (m_ButtonShare != null && go == m_ButtonShare.gameObject)
        {
            SendNotification(ConstDefine.BtnGameViewShare);
        }
    }


    #region OnBtnMouseDown 鼠标按下
    /// <summary>
    /// 语音按钮按下
    /// </summary>
    /// <param name="eventData"></param>
    protected virtual void OnBtnMacroDown(PointerEventData eventData)
    {
        if (eventData.selectedObject == m_ButtonMicroPhone.gameObject)
        {
            UIViewManager.Instance.OpenWindow(UIWindowType.Micro);
        }
    }
    #endregion

    #region OnBtnMacroUp 语音按钮抬起
    /// <summary>
    /// 语音按钮抬起
    /// </summary>
    /// <param name="eventData"></param>
    protected virtual void OnBtnMacroUp(PointerEventData eventData)
    {
        if (eventData.selectedObject == m_ButtonMicroPhone.gameObject)
        {
            if (eventData.pointerCurrentRaycast.gameObject == m_ButtonMicroPhone.gameObject)
            {
                SendNotification("OnBtnMicroUp");
            }
            else
            {
                Debug.Log("取消发送语音");
                SendNotification("OnBtnMicroCancel");
            }
        }
    }
    #endregion
}
