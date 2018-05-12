//===================================================
//Author      : DRB
//CreateTime  ：4/21/2017 8:56:02 PM
//Description ：场景控制器基类
//===================================================
using System.Collections.Generic;
using UnityEngine;


public class SceneCtrlBase : MonoBehaviour
{
    #region Variable

    protected IGameAI m_AI;

    #endregion



    #region MonoBehaviour
    private void Awake()
    {
        FingerEvent.Instance.OnPlayerClickUp += OnPlayerClick;
        FingerEvent.Instance.OnFingerBeginDrag += OnFingerBeginDrag;
        FingerEvent.Instance.OnFingerDrag += OnFingerDrag;
        FingerEvent.Instance.OnFingerEndDrag += OnFingerEndDrag;
        EffectManager.Instance.Init(this);
        OnAwake();
    }

    private void Start()
    {
        OnStart();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameCtrl.Instance.QuitRoom();
        }

        if (m_AI != null)
        {
            m_AI.DoAI();
        }

        OnUpdate();
    }

    private void OnDestroy()
    {
        if (FingerEvent.Instance != null)
        {
            FingerEvent.Instance.OnPlayerClickUp -= OnPlayerClick;
            FingerEvent.Instance.OnFingerBeginDrag -= OnFingerBeginDrag;
            FingerEvent.Instance.OnFingerDrag -= OnFingerDrag;
            FingerEvent.Instance.OnFingerEndDrag -= OnFingerEndDrag;
        }
        BeforeOnDestroy();
    }
    #endregion

    #region Protected Function
    /// <summary>
    /// Start时调用
    /// </summary>
    protected virtual void OnStart() { }
    /// <summary>
    /// OnDestroy时调用
    /// </summary>
    protected virtual void BeforeOnDestroy() { }
    /// <summary>
    /// Awake时调用
    /// </summary>
    protected virtual void OnAwake() { }
    /// <summary>
    /// Update时调用
    /// </summary>
    protected virtual void OnUpdate() { }
    /// <summary>
    /// 玩家点击时调用
    /// </summary>
    protected virtual void OnPlayerClick() { }
    /// <summary>
    /// 手指拖拽开始时调用
    /// </summary>
    protected virtual void OnFingerBeginDrag() { }
    /// <summary>
    /// 手指拖拽中调用
    /// </summary>
    /// <param name="screenPos"></param>
    protected virtual void OnFingerDrag(Vector2 screenPos) { }
    /// <summary>
    /// 手指拖拽结束调用
    /// </summary>
    protected virtual void OnFingerEndDrag() { }

    #region CheckIP 检查IP
    /// <summary>
    /// 检查IP
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lst">座位列表</param>
    protected void CheckIP<T>(List<T> lst) where T : SeatEntityBase
    {
        for (int i = 0; i < lst.Count; ++i)
        {
            SeatEntityBase seat1 = lst[i];
            if (seat1.Latitude == 0)
            {
                UIViewManager.Instance.ShowTip(string.Format("未获取到{0}的位置信息", seat1.Nickname));
                continue;
            }
            for (int j = i + 1; j < lst.Count; ++j)
            {
                SeatEntityBase seat2 = lst[j];
                if (LPSUtil.CalculateDistance(seat1.Latitude, seat1.Longitude, seat2.Latitude, seat2.Longitude) < 0.5f)
                {
                    UIViewManager.Instance.ShowTip(string.Format("{0}和{1}距离小于500米", seat1.Nickname, seat2.Nickname));
                }

                if (seat1.IP.Equals(seat2.IP))
                {
                    UIViewManager.Instance.ShowTip(string.Format("{0}和{1}的IP相同", seat1.Nickname, seat2.Nickname));
                }
            }
        }
    }
    #endregion
    #endregion
}
