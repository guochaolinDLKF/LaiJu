//===================================================
//Author      : DRB
//CreateTime  ：12/2/2017 4:56:39 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using proto.sss;

namespace ShiSanZhang
{
    public class UISceneShiSanZhangView : UISceneViewBase
    {
        public static UISceneShiSanZhangView Instance;
        [SerializeField]
        private UIItemSeat_ShiSanZhang[] m_Seats;
        [SerializeField]
        private Button m_btnShiSanZhangViewStartGame;//开始游戏
        [SerializeField]
        private Button m_btnReayShiSanZhangView;//准备
        [SerializeField]
        private Button m_ButtonMicroPhone;//语音
        [SerializeField]
        private Button m_ButtonShare; //微信邀请
        [SerializeField]
        private Button m_ButtonReady;//准备   



        public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
        {
            Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
            dic.Add(RoomShiSanZhangProxy.ON_SEAT_INFO_CHANGED, OnSeatInfoChanged);
            dic.Add(ShiSanZhangConstant.OnShiSanZhangGroupPokerShow, GroupPokerShow);
            return dic;
        }


        protected override void OnAwake()
        {
            base.OnAwake();
            EventTriggerListener.Get(m_ButtonMicroPhone.gameObject).onDown = OnBtnMouseDown;
            EventTriggerListener.Get(m_ButtonMicroPhone.gameObject).onUp = OnBtnMouseUp;          
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
                    SendNotification("OnShiSanZhangBtnMicroUp");
                }
                else
                {
                    Debug.Log("取消发送语音");
                    SendNotification("OnShiSanZhangBtnMicroCancel");
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
                case ShiSanZhangConstant.btnShiSanZhangViewChat://聊天
                    UIViewManager.Instance.OpenWindow(UIWindowType.Chat);
                    break;
                case ShiSanZhangConstant.btnShiSanZhangViewShare://微信邀请
                    SendNotification(ShiSanZhangConstant.btnShiSanZhangViewShare);
                    break;
                case ShiSanZhangConstant.btnShiSanZhangViewReady://准备
                    SendNotification(ShiSanZhangConstant.btnShiSanZhangViewReady);
                    break;
                case ShiSanZhangConstant.btnShiSanZhangViewStartGame://开始游戏
                    SendNotification(ShiSanZhangConstant.btnShiSanZhangViewStartGame);
                    break;
                case ShiSanZhangConstant.btnShiSanZhangViewPlayPoker://出牌
                    SendNotification(ShiSanZhangConstant.btnShiSanZhangViewPlayPoker);
                    break;                                        
            }
        }


        #region OnRoomInfoChanged 房间信息变更
        /// <summary>
        /// 房间信息变更
        /// </summary>
        /// <param name="obj"></param>
        private void OnSeatInfoChanged(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("SeatEntity");
            ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
            bool isPlayer = data.GetValue<bool>("IsPlayer");
            if (isPlayer)
            {
                m_btnShiSanZhangViewStartGame.gameObject.SetActive(roomStatus == ROOM_STATUS.ROOM_STATUS_IDLE && seat.Pos == 1);
                m_btnReayShiSanZhangView.gameObject.SetActive(seat.seatStatus == SEAT_STATUS.SEAT_STATUS_IDLE);
                m_ButtonShare.gameObject.SetActive(roomStatus == ROOM_STATUS.ROOM_STATUS_IDLE);
                if (!SystemProxy.Instance.IsInstallWeChat)
                {
                    m_ButtonShare.gameObject.SetActive(false);
                }
            }          
        }
        #endregion

        /// <summary>
        /// 开局
        /// </summary>
        public void Begin(RoomEntity room, bool isPlayAnimation)
        {
            StartCoroutine(BeginTor(room, isPlayAnimation));            
        }

        IEnumerator BeginTor(RoomEntity room,bool isPlayAnimation)
        {
            if (isPlayAnimation)
            {                
                for (int i = 0; i < room.SeatList.Count; i++)
                {
                    if (room.SeatList[i].handPokerList == null || room.SeatList[i].handPokerList.Count == 0 ) continue;
                    m_Seats[room.SeatList[i].Index].Begin(room.SeatList[i].handPokerList, isPlayAnimation);
                    yield return null;
                }
            }
            else
            {
                for (int i = 0; i < room.SeatList.Count; i++)
                {
                    if (room.SeatList[i].handPokerList.Count == 0) continue;
                    m_Seats[room.SeatList[i].Index].Begin(room.SeatList[i].handPokerList, isPlayAnimation);                  
                }
            }           
        }

        /// <summary>
        /// 组合牌结束
        /// </summary>
        /// <param name="data"></param>
        public void GroupPokerShow(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("Seat");
            m_Seats[seat.Index].GroupPokerShow();
        }

    }
}
