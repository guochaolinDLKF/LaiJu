//===================================================
//Author      : CZH
//CreateTime  ：6/14/2017 11:12:38 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZhaJh;
using UnityEngine.EventSystems;
using zjh.proto;
using System;

public class UISceneZhaJHView : UISceneViewBase
{
    [SerializeField]
    private UIZhaJHItemSeat[] m_Seats;
    // [SerializeField]
    //private Button m_CancelAuto;   
    [SerializeField]
    private Button m_ButtonMicroPhone;//语音
    [SerializeField]
    private Button m_ButtonShare; //微信邀请
    [SerializeField]
    private Button m_ButtonReady;//准备   
    [SerializeField]
    private Button m_ButtonChat;//聊天
    [SerializeField]
    private Button m_ButtonSetting;//设置
    [SerializeField]
    private Button m_ButtonLicensing;//发牌
    [SerializeField]
    private Button m_ButtonLightPoker;//亮牌按钮   
    [SerializeField]
    private GameObject seatMask;//游戏结束后取消遮罩   

    private Image aaa;
                               


    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add(ZhaJHMethodname.OnZJHSeatInfoChanged, OnSeatInfoChanged);        
        dic.Add(ZhaJHMethodname.OnZJHLicensingShow, LicensingShow);
        dic.Add(ZhaJHMethodname.OnZJHSettlementInterfaceView, SettlementInterfaceView);//高级房间玩家离开的时候通知个人结算信息  
        dic.Add(ZhaJHMethodname.OnZJHTotalSettlementView, TotalSettlementView);     //普通房游戏结束的总结算
        dic.Add(ZhaJHMethodname.OnZJHApplyForDissolution, ApplyForDissolution);//答复解散房间
        dic.Add(ZhaJHMethodname.OnZJHHideApplyForDissolution, HideApplyForDissolution);//解散房间失败，隐藏答复界面
        dic.Add(ZhaJHMethodname.OnZJHAutomaticReady, AutomaticReady);//5秒后自动准备
        dic.Add(ZhaJHMethodname.OnZJHCloseIEtor, CloseIEtor);
        return dic;
    }



    protected override void OnAwake()
    {
        base.OnAwake();
        EventTriggerListener.Get(m_ButtonMicroPhone.gameObject).onDown = OnBtnMouseDown;
        EventTriggerListener.Get(m_ButtonMicroPhone.gameObject).onUp = OnBtnMouseUp;
        if (m_ButtonLightPoker != null)m_ButtonLightPoker.gameObject.SetActive(false);
    }

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
                SendNotification(ZhaJHMethodname.OnZJHBtnMicroUp);
            }
            else
            {
                Debug.Log("取消发送语音");
                SendNotification(ZhaJHMethodname.OnZJHBtnMicroCancel);
            }
        }        
    }


    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case ConstDefine.BtnSetting:
                UIViewManager.Instance.OpenWindow(UIWindowType.Setting);
                break;
            case ZhaJHButtonConstant.btnZhaJHViewChat:
                UIViewManager.Instance.OpenWindow(UIWindowType.Chat);
                break;
            case ZhaJHButtonConstant.btnZhaJHViewShare:
                SendNotification(ZhaJHButtonConstant.btnZhaJHViewShare);
                break;            
            case ZhaJHButtonConstant.btnZhaJHViewReady:
                SendNotification(ZhaJHButtonConstant.btnZhaJHViewReady);
                break;
            case ZhaJHButtonConstant.btnZhaJHViewLicensing:
                if (RoomZhaJHProxy.Instance.CurrentRoom.roomStatus==ENUM_ROOM_STATUS.IDLE)
                {
                    SendNotification(ZhaJHButtonConstant.btnZhaJHViewLicensing);
                }               
                break;
            case ZhaJHButtonConstant.btnZhaJHViewLightPoker:
                SendNotification(ZhaJHButtonConstant.btnZhaJHViewLightPoker);
                break; //btnOpenPoker
            case "btnOpenPoker":
                ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHOpenPoker);
                break;
            case "btnShufflingPoker":
                ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHShufflingPoker);
                break;
            case "btnClose"://关闭搓牌
                ModelDispatcher.Instance.Dispatch(ZhaJHMethodname.OnZJHCloseShufflingPoker);
                break;
        }
    }

    /// <summary>
    /// 开启携程
    /// </summary>
    /// <param name="data"></param>
    private void AutomaticReady(TransferData data)
    {
        
        StartCoroutine("AutomaticReadyIEtor");
    }

    /// <summary>
    /// 关闭协程
    /// </summary>
    /// <returns></returns>
    private void CloseIEtor(TransferData data)
    {
        StopCoroutine("AutomaticReadyIEtor");
    }
    private IEnumerator AutomaticReadyIEtor()
    {       
        yield return new WaitForSeconds(5);        
        SendNotification(ZhaJHButtonConstant.btnZhaJHViewReady);
    }

    /// <summary>
    /// 座位信息变更回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnSeatInfoChanged(TransferData data)
    {
        if (seatMask.activeSelf)
        {
            seatMask.GetComponent<ItemSeatMask>().OnBtnClik();
        }
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        RoomEntity room = data.GetValue<RoomEntity>("Room");
        bool isPlayer = data.GetValue<bool>("IsPlayer");
        ENUM_ROOM_STATUS roomStatus = data.GetValue<ENUM_ROOM_STATUS>("RoomStatus");        
        if (isPlayer||RoomZhaJHProxy.Instance.PlayerSeat==seat)
        {
            if (seat.pos == 7)
            {
                m_ButtonLicensing.gameObject.SetActive(false);
                m_ButtonReady.gameObject.SetActive(false);
                //检查手机上是否有微信，如果有的话打开微信邀请按钮
                if (!SystemProxy.Instance.IsInstallWeChat || !SystemProxy.Instance.IsOpenWXLogin)
                {
                    if (m_ButtonShare != null) m_ButtonShare.gameObject.SetActive(false);
                }
                else
                {
                    m_ButtonShare.gameObject.SetActive(true);
                }               
            }
            else
            {                
                m_ButtonReady.gameObject.SetActive(room.currentLoop != room.maxLoop && (seat.seatStatus == ENUM_SEAT_STATUS.IDLE|| seat.seatStatus==ENUM_SEAT_STATUS.SETTLEMENT)&& (roomStatus == ENUM_ROOM_STATUS.MATCH_STATUS_RESULT || roomStatus == ENUM_ROOM_STATUS.IDLE || roomStatus == ENUM_ROOM_STATUS.SETTLEMENT));                               
                if (room.roomSettingId ==RoomMode.Senior)
                {
                    m_ButtonLicensing.gameObject.SetActive(false);                 
                }
                else 
                {                   
                    m_ButtonLicensing.gameObject.SetActive(room.currentLoop==0&&seat.homeLorder && (roomStatus == ENUM_ROOM_STATUS.IDLE || roomStatus == ENUM_ROOM_STATUS.ROOMDISSOLUTION)); //&& seat.LicensingButton == true  if (seat.pos == 1)                                                                
                    if (roomStatus == ENUM_ROOM_STATUS.IDLE) m_ButtonLightPoker.gameObject.SetActive(false);
                    //else m_ButtonLicensing.gameObject.SetActive(false);                   
                }
                if (!SystemProxy.Instance.IsInstallWeChat || !SystemProxy.Instance.IsOpenWXLogin)
                {
                    if (m_ButtonShare != null) m_ButtonShare.gameObject.SetActive(false);
                }
               else
                {
                    m_ButtonShare.gameObject.SetActive((seat.seatStatus == ENUM_SEAT_STATUS.IDLE || seat.seatStatus == ENUM_SEAT_STATUS.READY) && seat.homeLorder && roomStatus == ENUM_ROOM_STATUS.IDLE && room.roomSettingId != RoomMode.Senior);
                }
            }          
        }
    }


    /// <summary>
    /// 发牌按钮   和亮牌按钮
    /// </summary>
    /// <param name="data"></param>
    private void LicensingShow(TransferData data)
    {           
        bool isLightPoker = data.GetValue<bool>("isLightPoker");
        m_ButtonLightPoker.gameObject.SetActive(isLightPoker);
        //StartCoroutine("ButtonLightPokerYelayed",isLightPoker);
    }


    /// <summary>
    /// 延迟亮牌按钮显示
    /// </summary>
    /// <param name="isLightPoker"></param>
    /// <returns></returns>
    //IEnumerator ButtonLightPokerYelayed(bool isLightPoker)
    //{
    //    if (isLightPoker)
    //    {
    //        yield return new WaitForSeconds(2);
    //    }
    //    m_ButtonLightPoker.gameObject.SetActive(isLightPoker);
    //    if (!isLightPoker)
    //    {
    //        StopCoroutine("ButtonLightPokerYelayed");
    //    }
       
    //}

    /// <summary>
    /// 高级房玩家离开的时候显示玩家的小结算
    /// </summary>
    /// <param name="data"></param>
    public void SettlementInterfaceView(TransferData data)
    {       
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        if (seat.profit>=0)
        {
            AudioEffectManager.Instance.Play("zjh_sheng", Vector3.zero);
        }
        else
        {
            AudioEffectManager.Instance.Play("zjh_bai", Vector3.zero);
        }
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.UIZJHRecord, (GameObject go) => 
        {
            go.GetComponent<UIZJHSmallSettlement>().SettlementUI(seat);
        });     
        //StartCoroutine(SettlementInterTor());
    }

    //IEnumerator SettlementInterTor()
    //{
    //    yield return new WaitForSeconds(6);          
    //    NetWorkSocket.Instance.SafeClose();
    //    SceneMgr.Instance.LoadScene(SceneType.Main);
    //}


    /// <summary>
    /// 普通房结束的时候显示总结算
    /// </summary>
    /// <param name="data"></param>
    private void TotalSettlementView(TransferData data)
    {        
        List<SeatEntity> seatList = data.GetValue<List<SeatEntity>>("seatList");
        RoomEntity room = data.GetValue<RoomEntity>("Room");
        bool isGameOver = data.GetValue<bool>("isGameOver");
        StartCoroutine(TotalSettlementTor(seatList, room, isGameOver));                     
    }

    IEnumerator TotalSettlementTor(List<SeatEntity> seatList, RoomEntity room, bool isGameOver)
    {
        if (seatList.Count != 0)
        {
            if (isGameOver)
            {
                yield return new WaitForSeconds(2);
            }
            AudioEffectManager.Instance.Play("zjh_over", Vector3.zero);
            UIViewUtil.Instance.LoadWindowAsync(UIWindowType.UIZJHTotalSettlement, (GameObject go) =>
            {
                ZhaJHGameCtrl.Instance.m_UIResultView = go.GetComponent<UIZJHTotalSettlement>();
                go.GetComponent<UIZJHTotalSettlement>().TotalSettlement(seatList, room);
            });
        }
        //yield return new WaitForSeconds(6);
        //NetWorkSocket.Instance.SafeClose();
        //SceneMgr.Instance.LoadScene(SceneType.Main);
    }
    /// <summary>
    /// 答复解散房间的消息
    /// </summary>
    /// <param name="data"></param>
    private GameObject go1;
    private void ApplyForDissolution(TransferData data)
    {
        int cout = data.GetValue<int>("SetADHWindowSum");
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.ADH_ZJH, (GameObject go) =>
        {                   
            go.GetComponent<UIZJHADHWindow>().DisplayPeopleNumber(cout);
            go1 = go;
        });
    }

    private void HideApplyForDissolution(TransferData data)
    {
        Destroy(go1);    
        //go1.SetActive(false);       
    }
}

