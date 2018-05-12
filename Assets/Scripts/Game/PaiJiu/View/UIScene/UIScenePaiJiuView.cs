//===================================================
//Author      : WZQ
//CreateTime  ：7/4/2017 11:18:00 AM
//Description ：牌九UIRoot
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using PaiJiu;
using proto.paigow;
public class UIScenePaiJiuView : UISceneViewBase {



    #region Variable
    [SerializeField]
    private UIItemSeat_PaiJiu[] m_Seats;
    [SerializeField]
    private Operater_PaiJiu m_Operater;//Operater项

    [SerializeField]
    private Grid3D m_HandConatiner;//手牌挂载点
    [SerializeField]
    private Transform m_DrawContainer;//摸牌？？

    [SerializeField]
    private GameObject[] m_Animations;
    [SerializeField]
    private Button m_ButtonMicroPhone;//语音按钮
    [SerializeField]
    private Button m_ButtonShare;//微信分享(邀请好友)
    [SerializeField]
    private Button m_ButtonReady;//准备按钮

    [SerializeField]
    private Button m_ButtonChat;//文字聊天
    [SerializeField]
    private Button m_ButtonSetting;//菜单按钮
    [SerializeField]
    private Transform m_EffectContainer;

    //private Tweener m_HandTween; //手动画

    //private UIItemHandPoker m_CurrentPoker;//当前操作的牌

    [SerializeField]
    private float PokerTypeAniIntervalTime = 0.5f;//顺序开牌间隔
    [SerializeField]
    private float delaySetGoldTime = 1f;//延迟更改金币text显示时间
    [SerializeField]
    private float popupUnitAni = 4f;//延迟弹出小结算时间

    private List<UIItemHandPoker_PaiJiu> m_HandList = new List<UIItemHandPoker_PaiJiu>();//UI手牌列表

    #endregion



    #region MonoBehaviour
    protected override void OnAwake()
    {
        base.OnAwake();

        EventTriggerListener.Get(m_ButtonMicroPhone.gameObject).onDown = OnBtnMouseDown;
        EventTriggerListener.Get(m_ButtonMicroPhone.gameObject).onUp = OnBtnMouseUp;
        for (int i = 0; i < m_Animations.Length; ++i)
        {
            m_Animations[i].gameObject.SetActive(false);
        }

    
        ModelDispatcher.Instance.AddEventListener(ConstDefine_PaiJiu.ObKey_SeatInfoChanged, OnSeatInfoChanged);//座位信息变更回调
   

        m_ButtonShare.gameObject.SetActive(RoomPaiJiuProxy.Instance.CurrentRoom.matchId <= 0);
        m_ButtonReady.gameObject.SetActive(RoomPaiJiuProxy.Instance.CurrentRoom.matchId <= 0);

        if (!SystemProxy.Instance.IsInstallWeChat || !SystemProxy.Instance.IsOpenWXLogin)
        {
            m_ButtonShare.gameObject.SetActive(false);
        }
 
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        ModelDispatcher.Instance.RemoveEventListener(ConstDefine_PaiJiu.ObKey_SeatInfoChanged, OnSeatInfoChanged);
   
    }

    #endregion


    #region OnSeatDrawPoker 当座位摸牌时
    /// <summary>
    /// 当座位摸牌时
    /// </summary>
    /// <param name="data"></param>
    private void OnSeatDrawPoker(TransferData data)
    {
        //int seatPos = data.GetValue<int>("SeatPos");
        //bool isPlayer = data.GetValue<bool>("IsPlayer");
        //if (isPlayer)
        //{
        //    bool isLast = data.GetValue<bool>("IsLast");
        //    bool isBuhua = data.GetValue<bool>("IsBuhua");
        //    Poker hitPoker = data.GetValue<Poker>("HitPoker");
        //    MaJiangCtrl ctrl = MahJongManager.Instance.DrawMaJiang(seatPos, hitPoker, false, isLast);
        //    DrawPoker(ctrl, isBuhua);

        //    CheckTing();
        //}
    }
    #endregion


    #region OnSeatInfoChanged 座位信息变更回调  由RoomPaiJiuProxy发送
    /// <summary>
    /// 座位信息变更回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnSeatInfoChanged(TransferData data)
    {
        PaiJiu.Seat seat = data.GetValue<PaiJiu.Seat>("Seat");//座位
        bool isPlayer = data.GetValue<bool>("IsPlayer");//是否自己
        ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");//房间状态
        PaiJiu.Room currentRoom = data.GetValue<PaiJiu.Room>("CurrentRoom");//当前房间
        PaiJiu.Seat BankerSeat = data.GetValue<PaiJiu.Seat>("BankerSeat");//庄家座位
        PaiJiu.Seat ChooseBankerSeat = data.GetValue<PaiJiu.Seat>("ChooseBankerSeat");//当前选庄座位

        if (isPlayer)
        {
            m_ButtonReady.gameObject.SetActive(seat.seatStatus == SEAT_STATUS.SEAT_STATUS_IDLE && (roomStatus == ROOM_STATUS.IDLE || roomStatus == ROOM_STATUS.READY));
            m_ButtonShare.gameObject.SetActive(seat.seatStatus == SEAT_STATUS.SEAT_STATUS_IDLE && roomStatus == ROOM_STATUS.IDLE || roomStatus == ROOM_STATUS.READY);
            if (!SystemProxy.Instance.IsInstallWeChat)
            {
                m_ButtonShare.gameObject.SetActive(false);
            }
            //m_CancelAuto.gameObject.SetActive(seat.IsTrustee);
            m_Operater.SetUI(currentRoom, seat, BankerSeat);

        }
    }
    #endregion


    #region  OnBtnMouseDown 语音按钮抬起按下
    private void OnBtnMouseDown(PointerEventData eventData)
    {
        if (eventData.selectedObject == m_ButtonMicroPhone.gameObject)
        {
            UIViewManager.Instance.OpenWindow(UIWindowType.Micro);
        }
    }

    private void OnBtnMouseUp(PointerEventData eventData)
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
    

    #region OnBtnClick 按钮点击
    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case ConstDefine_PaiJiu.BtnSetting:
                UIViewManager.Instance.OpenWindow(UIWindowType.Setting);
                break;
            //case ConstDefine.BtnMaJiangViewAuto://暂无托管
            //    SendNotification(ConstDefine.BtnMaJiangViewAuto);
            //    break;
            case ConstDefine_PaiJiu.BtnPaiJiuViewChat:
                UIViewManager.Instance.OpenWindow(UIWindowType.Chat);
                break;
            //case ConstDefine.BtnMaJiangViewCancelAuto://暂无托管取消
            //    SendNotification(ConstDefine.BtnMaJiangViewCancelAuto);
            //    break;
            case ConstDefine_PaiJiu.BtnPaiJiuViewShare:
                SendNotification(ConstDefine_PaiJiu.BtnPaiJiuViewShare);
                break;
            case ConstDefine_PaiJiu.BtnPaiJiuViewReady:
                SendNotification(ConstDefine_PaiJiu.BtnPaiJiuViewReady);
                break;
        }
    }    
    #endregion

    public void Begin()
    {
        for (int i = m_HandList.Count - 1; i >= 0; --i)
        {
            m_HandList[i].transform.SetParent(null);
            Destroy(m_HandList[i].gameObject);
        }
        m_HandList.Clear();
    }

    #region DrawPoker 麻将UI 摸牌
    /// <summary>
    ///  麻将UI 摸牌
    /// </summary>
    /// <param name="majiang"></param>
    /// <param name="isInit"></param>
    public void DrawPoker(MaJiangCtrl_PaiJiu majiang, bool isInit = false)
    {
        CreatePoker(majiang, isInit, null);
    }

    /// <summary>
    /// 创建Poker
    /// </summary>
    /// <param name="majiang"></param>
    /// <param name="isInit"></param>
    /// <param name="onComplete"></param>
    private void CreatePoker(MaJiangCtrl_PaiJiu majiang, bool isInit, Action<UIItemHandPoker_PaiJiu> onComplete)
    {
        UIViewManager.Instance.LoadItemAsync("UIItemHandPoker_PaiJiu", (GameObject prefab) =>
        {
            GameObject go = Instantiate(prefab);
            majiang.gameObject.SetLayer(LayerMask.NameToLayer("UI"));
            UIItemHandPoker_PaiJiu uiPoker = go.GetComponent<UIItemHandPoker_PaiJiu>();
            uiPoker.SetUI(majiang);
            m_HandList.Add(uiPoker);


            uiPoker.gameObject.SetParent(m_HandConatiner.transform);
            m_HandConatiner.Sort();
            if (onComplete != null)
            {
                onComplete(uiPoker);
            }
        });

    }
    #endregion

    #region SetHandPokerStatus  设置UI手牌 状态
    /// <summary>
    /// 设置UI手牌 状态
    /// </summary>
    public void SetHandPokerStatus(Seat seat)
    {
        AppDebug.Log(string.Format("翻开UI手牌"));
        for (int i = 0; i < m_HandList.Count; i++)
        {
            if (m_HandList[i].Majiang != null) m_HandList[i].Majiang.SetPokerStatus(true);
        }

     }
    #endregion


    #region  EverytimeSettle 小结算UI管理  处理小结算一系列显示
    /// <summary>
    /// 每次结算
    /// </summary>
    public void EverytimeSettle(PaiJiu.Room room)
    {
        // 分数 金币动画
        StartCoroutine("RoomOpenPokerSettleAni", room);
    }

    #region RoomOpenPokerSettleAni 设置结算动画动画 
    IEnumerator RoomOpenPokerSettleAni(PaiJiu.Room room)
    {
        
#if IS_ZHANGJIAKOU
#endif
            List<PaiJiu.Seat> seatList = room.SeatList;
        if (!room.isCutPan)
        {
            //ROOM_STATUS roomStatus = room.roomStatus;
            yield return 0;

            for (int i = 0; i < seatList.Count; i++)
            {
                if (seatList[i] != null && seatList[i].PlayerId > 0 && !seatList[i].IsBanker)
                {
                    RoomOpenPokerSettleAniPerform(seatList[i], room);
                    yield return new WaitForSeconds(PokerTypeAniIntervalTime);
                }
                else
                {
                    continue;
                }

            }

            //显示庄的各种动画
            for (int i = 0; i < seatList.Count; i++)
            {
                if (seatList[i].IsBanker)
                {
                    //
                    RoomOpenPokerSettleAniPerform(seatList[i], room);
                    yield return new WaitForSeconds(PokerTypeAniIntervalTime);
                    break;
                }
            }

            //播放通赔 通杀
            PlayTongCompensateKill(seatList);
            yield return new WaitForSeconds(PokerTypeAniIntervalTime);

            //显示金币移动                        《-------金币动画
            GoldFlowCtrl_PaiJiu.Instance.GoldFlowAni(seatList);

        }
        else
        {
            //如果是切锅 只刷新金币
            for (int i = 0; i <seatList.Count ; i++)
            {
                if (seatList[i].PlayerId > 0)
                {
                    //改为发消息刷新金币
                    TransferData data = new TransferData();
                    data.SetValue("seat", seatList[i]);
                    ModelDispatcher.Instance.Dispatch(ConstDefine_PaiJiu.ObKey_SetGoldAni, data);
                }
            }

        }

        //每局小结算
        if (room.loopEnd)
        {
#if IS_ZHANGJIAKOU
            //是否炸锅
            PlayUIAnimation(true ? ConstDefine_PaiJiu.UIAniZhaGuo_JuYou:ConstDefine_PaiJiu.UIAniBaoGuo_JuYou); //加载爆锅炸锅
            yield return new WaitForSeconds(2f);
#endif
            if (!room.isCutPan)
                yield return new WaitForSeconds(popupUnitAni);
            UIDispatcher.Instance.Dispatch(ConstDefine_PaiJiu.ObKey_OpenViewPaiJiu, new object[] { UIWindowType.UnitSettlement_PaiJiu });
        }

            yield return new WaitForSeconds(2f);
            //完成结算
            UIDispatcher.Instance.Dispatch(ConstDefine_PaiJiu.ObKey_SettleOnComplete, null);
       

    }
    #endregion

    #region RoomOpenPokerSettleAniPerform 具体设置座位结算效果   
    void RoomOpenPokerSettleAniPerform(PaiJiu.Seat seat, Room CurrentRoom)
    {
        m_Seats[seat.Index].SetSettle(seat);                                  //设置结算效果
        //m_Seats[seat.Index].SetScoresGoUp(seat.Earnings);
        //m_Seats[seat.Index].SetHandPokersType(seat,true);                   //牌型

        StartCoroutine(DelaySetGold(seat));                                // 更新玩家积分 SetGold

    }
    #endregion

    #region DelaySetGold 延迟刷新金币
    IEnumerator DelaySetGold(PaiJiu.Seat seat)
    {      
        yield return new WaitForSeconds(delaySetGoldTime);
        //改为发消息刷新金币
        TransferData data = new TransferData();
        data.SetValue("seat", seat);
        ModelDispatcher.Instance.Dispatch(ConstDefine_PaiJiu.ObKey_SetGoldAni, data);
    }
    #endregion

    #region PlayTongCompensateKill 设置通吃通杀效果
    void PlayTongCompensateKill(List<Seat> seatList)
    {
        Seat bankerSeat = null;

        int playerSum = 0;
        for (int i = 0; i < seatList.Count; i++)
        {
            if (seatList[i].IsBanker) bankerSeat = seatList[i];
            if (seatList[i].PlayerId > 0) playerSum++;
        }

        if (playerSum <= 0) return;
        if (bankerSeat != null)
        {
            //通杀
            bool isTongSha = true;
            for (int i = 0; i < seatList.Count; i++)
            {
                if (!seatList[i].IsBanker && seatList[i].Earnings >= 0) isTongSha = false;
            }

            if (isTongSha)

            {

                AudioEffectManager.Instance.Play(ConstDefine_PaiJiu.TongSha_paijiu, Vector3.zero);//播放通杀声音
                return;
            }

            // 通赔
            bool isTongPei = true;
            for (int i = 0; i < seatList.Count; i++)
            {
                if (!seatList[i].IsBanker && seatList[i].Earnings < 0) isTongPei = false;
            }

            if (isTongPei) AudioEffectManager.Instance.Play(ConstDefine_PaiJiu.TongPei_paijiu, Vector3.zero);//播放通赔声音

        }
    }
    #endregion

    #endregion

    #region ClearHandPoker 清除UI手牌
    public void ClearHandPoker()
    {
        for (int i = m_HandList.Count; i >0; --i)
        {
            m_HandList[i-1].transform.SetParent(null);
            UIItemHandPoker_PaiJiu UIHandPoker = m_HandList[i - 1];
            m_HandList.Remove(UIHandPoker);
            Destroy(UIHandPoker.gameObject);
        }

    }
    #endregion

    #region PlayUIAnimation 播放UI动画
    /// <summary>
    /// 播放UI动画
    /// </summary>
    /// <param name="type"></param>
    public void PlayUIAnimation(string type)//UIAnimationType
    {
        string path = string.Format("download/{0}/prefab/uiprefab/uianimations/{1}.drb", ConstDefine.GAME_NAME, type.ToLower());
        AssetBundleManager.Instance.LoadOrDownload(path, type.ToString().ToLower(), (GameObject go) =>
        {
            if (go != null)
            {
                go = Instantiate(go);
                go.SetParent(m_EffectContainer);
            }
        });
    }
    #endregion



}
