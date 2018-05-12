//===================================================
//Author      : DRB
//CreateTime  ：4/1/2017 2:14:39 PM
//Description ：麻将场景UI
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace DRB.MahJong
{
    public class UISceneMaJiangView : UISceneViewBase
    {
        #region Variable
        [SerializeField]
        private UIItemSeat[] m_Seats;
        [SerializeField]
        private Button m_CancelAuto;
        [SerializeField]
        private GameObject[] m_Animations;
        [SerializeField]
        private Button m_ButtonMicroPhone;
        [SerializeField]
        private Button m_ButtonShare;
        [SerializeField]
        private Button m_ButtonReady;
        [SerializeField]
        private Button m_ButtonCancelReady;
        [SerializeField]
        private Button m_ButtonAuto;
        [SerializeField]
        private Button m_ButtonChat;
        [SerializeField]
        private Button m_ButtonSetting;
        [SerializeField]
        private Transform m_EffectContainer;
        [SerializeField]
        private Image m_WaitLiangXi;
        [SerializeField]
        private DOTweenAnimation m_RoomInfo;
        [SerializeField]
        private Button m_btnRoomInfoClose;
        [SerializeField]
        private GameObject m_SwapTip;
        [SerializeField]
        private GameObject m_LackColorTip;
        #endregion

        #region MonoBehaviour
        protected override void OnAwake()
        {
            base.OnAwake();
            EventTriggerListener.Get(m_ButtonMicroPhone.gameObject).onDown = OnBtnMacroDown;
            EventTriggerListener.Get(m_ButtonMicroPhone.gameObject).onUp = OnBtnMacroUp;
            for (int i = 0; i < m_Animations.Length; ++i)
            {
                m_Animations[i].gameObject.SetActive(false);
            }

            m_CancelAuto.gameObject.SetActive(false);
            ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_ROOM_INFO_CHANGED, OnRoomInfoChanged);
            ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_SEAT_INFO_CHANGED, OnSeatInfoChanged);
            ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_DRAW_POKER, OnSeatDrawPoker);
            ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_PLAY_POKER, OnSeatPlayPoker);
            ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_OPERATE, OnSeatOperate);
            ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_TING, OnSeatTing);
            ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_ZHIDUI, OnSeatZhiDui);
            ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_HU, OnSeatHu);

            if (RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Replay)
            {
                m_ButtonMicroPhone.gameObject.SetActive(false);
                m_ButtonChat.gameObject.SetActive(false);
                m_ButtonAuto.gameObject.SetActive(false);
                m_ButtonSetting.gameObject.SetActive(false);
            }

            m_ButtonShare.gameObject.SetActive(RoomMaJiangProxy.Instance.CurrentRoom.matchId <= 0);
            m_ButtonReady.gameObject.SetActive(RoomMaJiangProxy.Instance.CurrentRoom.matchId <= 0);
            if (m_ButtonCancelReady != null)
            {
                m_ButtonCancelReady.gameObject.SetActive(false);
            }

            if (!SystemProxy.Instance.IsInstallWeChat)
            {
                m_ButtonShare.gameObject.SetActive(false);
            }

            if (m_WaitLiangXi != null)
            {
                m_WaitLiangXi.gameObject.SetActive(false);
            }
        }

        protected override void BeforeOnDestroy()
        {
            base.BeforeOnDestroy();
            ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_ROOM_INFO_CHANGED, OnRoomInfoChanged);
            ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_SEAT_INFO_CHANGED, OnSeatInfoChanged);
            ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_DRAW_POKER, OnSeatDrawPoker);
            ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_PLAY_POKER, OnSeatPlayPoker);
            ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_OPERATE, OnSeatOperate);
            ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_TING, OnSeatTing);
            ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_ZHIDUI, OnSeatZhiDui);
            ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_HU, OnSeatHu);
        }

        protected override void OnBtnClick(GameObject go)
        {
            base.OnBtnClick(go);
            switch (go.name)
            {
                case ConstDefine.BtnSetting:
                    UIViewManager.Instance.OpenWindow(UIWindowType.Setting);
                    break;
                case ConstDefine.BtnMaJiangViewAuto:
                    SendNotification(ConstDefine.BtnMaJiangViewAuto);
                    break;
                case ConstDefine.BtnGameViewChat:
                    UIViewManager.Instance.OpenWindow(UIWindowType.Chat);
                    break;
                case ConstDefine.BtnMaJiangViewCancelAuto:
                    SendNotification(ConstDefine.BtnMaJiangViewCancelAuto);
                    break;
                case ConstDefine.BtnGameViewShare:
                    SendNotification(ConstDefine.BtnGameViewShare);
                    break;
                case ConstDefine.BtnMaJiangViewReady:
                    SendNotification(ConstDefine.BtnMaJiangViewReady);
                    break;
                case ConstDefine.BtnMaJiangViewCancelReady:
                    SendNotification(ConstDefine.BtnMaJiangViewCancelReady);
                    break;
                case "btnMaJiangViewRule":
                    UIItemMahJongRoomInfo.Instance.ChangeRuleActive();
                    break;
                case "btnMahjongViewRoomInfo":
                    m_btnRoomInfoClose.gameObject.SetActive(true);
                    m_RoomInfo.DORestart();
                    break;
                case "btnMahjongViewRoomInfoClose":
                    m_btnRoomInfoClose.gameObject.SetActive(false);
                    m_RoomInfo.DOPlayBackwards();
                    break;
            }
        }
        #endregion

        #region OnRoomInfoChanged 房间信息变更
        /// <summary>
        /// 房间信息变更
        /// </summary>
        /// <param name="obj"></param>
        private void OnRoomInfoChanged(TransferData data)
        {
            RoomEntity room = data.GetValue<RoomEntity>("Room");
            if (room == null) return;
            m_ButtonAuto.SafeSetActive(!(room.Status == RoomEntity.RoomStatus.Ready || room.Status == RoomEntity.RoomStatus.Replay));

            SetWaitLiangXi(room.Status == RoomEntity.RoomStatus.Show || room.Status == RoomEntity.RoomStatus.Jiao);
            SetSwapTip(room.Status == RoomEntity.RoomStatus.Swap);
            SetLackColorTip(room.Status == RoomEntity.RoomStatus.LackColor);
        }
        #endregion

        #region OnSeatDrawPoker 当座位摸牌时
        /// <summary>
        /// 当座位摸牌时
        /// </summary>
        /// <param name="data"></param>
        private void OnSeatDrawPoker(TransferData data)
        {

        }
        #endregion

        #region OnSeatPlayPoker 当座位出牌时
        /// <summary>
        /// 当座位出牌时
        /// </summary>
        /// <param name="data"></param>
        private void OnSeatPlayPoker(TransferData data)
        {
            SetWaitLiangXi(false);
        }
        #endregion

        #region OnSeatOperate 当座位碰牌时
        /// <summary>
        /// 当座位碰牌时
        /// </summary>
        /// <param name="data"></param>
        private void OnSeatOperate(TransferData data)
        {
            int seatIndex = data.GetValue<int>("SeatIndex");
            OperatorType type = data.GetValue<OperatorType>("OperateType");
            int subType = data.GetValue<int>("SubType");
            UIAnimationType animationType = UIAnimationType.UIAnimation_Gang;
            switch (type)
            {
                case OperatorType.Chi:
                    animationType = UIAnimationType.UIAnimation_Chi;
                    break;
                case OperatorType.Peng:
                    animationType = UIAnimationType.UIAnimation_Peng;
                    break;
                case OperatorType.Gang:
                    animationType = UIAnimationType.UIAnimation_Gang;
                    break;
                case OperatorType.Hu:
                    if (subType == 1)
                    {
                        animationType = UIAnimationType.UIAnimation_Hu;
                    }
                    else
                    {
                        animationType = UIAnimationType.UIAnimation_ZiMo;
                    }
                    break;
                case OperatorType.BuHua:
                    animationType = UIAnimationType.UIAnimation_BuHua;
                    break;
                case OperatorType.ChiTing:
                    animationType = UIAnimationType.UIAnimation_ChiTing;
                    break;
                case OperatorType.PengTing:
                    animationType = UIAnimationType.UIAnimation_PengTing;
                    break;
                case OperatorType.LiangXi:
                    animationType = UIAnimationType.UIAnimation_LiangXi;
                    break;
                case OperatorType.DingZhang:
                    animationType = UIAnimationType.UIAnimation_DingZhang;
                    break;
                case OperatorType.PiaoTing:
                    animationType = UIAnimationType.UIAnimation_PiaoTing;
                    break;
                case OperatorType.DingJiang:
                    animationType = UIAnimationType.UIAnimation_DingJiang;
                    break;
                case OperatorType.Kou:
                    animationType = UIAnimationType.UIAnimation_Kou;
                    break;
                case OperatorType.BuXi:
                    animationType = UIAnimationType.UIAnimation_BuXi;
                    break;
                case OperatorType.Jiao:
                    animationType = UIAnimationType.UIAnimation_Jiao;
                    break;
            }
            PlayUIAnimation(seatIndex, animationType);
        }
        #endregion

        #region OnSeatHu 当座位胡牌时
        /// <summary>
        /// 当座位胡牌时
        /// </summary>
        private void OnSeatHu(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("SeatEntity");
            int subType = data.GetValue<int>("SubType");
            UIAnimationType animationType = UIAnimationType.UIAnimation_ZiMo;
            if (subType == 1)
            {
                animationType = UIAnimationType.UIAnimation_Hu;
            }
            PlayUIAnimation(seat.Index, animationType);
        }
        #endregion

        #region OnSeatTing 当座位听牌时
        /// <summary>
        /// 当座位听牌时
        /// </summary>
        /// <param name="data"></param>
        private void OnSeatTing(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("Seat");
            PlayUIAnimation(seat.Index, UIAnimationType.UIAnimation_Ting);
        }
        #endregion

        #region OnSeatZhiDui 当座位支对时
        /// <summary>
        /// 当座位支对时
        /// </summary>
        /// <param name="obj"></param>
        private void OnSeatZhiDui(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("Seat");
            PlayUIAnimation(seat.Index, UIAnimationType.UIAnimation_ZhiDui);
        }
        #endregion

        #region OnSeatInfoChanged 座位信息变更回调
        /// <summary>
        /// 座位信息变更回调
        /// </summary>
        /// <param name="obj"></param>
        private void OnSeatInfoChanged(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("Seat");
            bool isPlayer = data.GetValue<bool>("IsPlayer");
            RoomEntity.RoomStatus roomStatus = data.GetValue<RoomEntity.RoomStatus>("RoomStatus");
            if (isPlayer)
            {
                m_ButtonReady.gameObject.SetActive(seat.Status == SeatEntity.SeatStatus.Idle && roomStatus == RoomEntity.RoomStatus.Ready);
                if (m_ButtonCancelReady != null)
                {
                    m_ButtonCancelReady.gameObject.SetActive(seat.Status == SeatEntity.SeatStatus.Ready && roomStatus == RoomEntity.RoomStatus.Ready);
                }
                m_ButtonShare.gameObject.SetActive(roomStatus == RoomEntity.RoomStatus.Ready);
                if (!SystemProxy.Instance.IsInstallWeChat)
                {
                    m_ButtonShare.gameObject.SetActive(false);
                }
                m_CancelAuto.gameObject.SetActive(seat.IsTrustee);
            }
        }
        #endregion

        #region OnBtnMouseDown 鼠标按下
        /// <summary>
        /// 语音按钮按下
        /// </summary>
        /// <param name="eventData"></param>
        private void OnBtnMacroDown(PointerEventData eventData)
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
        private void OnBtnMacroUp(PointerEventData eventData)
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

        #region Begin 开局重置
        /// <summary>
        /// 开局重置
        /// </summary>
        public void Begin(bool isPlayAnimation)
        {
            if (isPlayAnimation)
            {
                PlayBeginAnimation();
            }
        }
        #endregion

        #region PlayBeginAnimation 播放开局动画
        /// <summary>
        /// 播放开局动画
        /// </summary>
        private void PlayBeginAnimation()
        {
#if !IS_HONGHU
            return;
#endif
            for (int i = 0; i < m_Animations.Length; ++i)
            {
                m_Animations[i].gameObject.SetActive(true);
                if (i == 0)
                {

                    m_Animations[i].GetComponent<DOTweenAnimation>().tween.OnComplete(() =>
                    {
                        for (int j = 0; j < m_Animations.Length; ++j)
                        {
                            m_Animations[j].GetComponent<DOTweenAnimation>().tween.PlayBackwards();
                        }
                    }).OnRewind(() =>
                    {
                        for (int j = 0; j < m_Animations.Length; ++j)
                        {
                            m_Animations[j].gameObject.SetActive(false);
                        }
                    });
                }
                m_Animations[i].GetComponent<DOTweenAnimation>().tween.Restart();
            }
        }
        #endregion

        #region PlayUIAnimation 播放UI动画(座位)
        /// <summary>
        /// 播放UI动画(座位)
        /// </summary>
        /// <param name="seatIndex"></param>
        /// <param name="type"></param>
        public void PlayUIAnimation(int seatIndex, UIAnimationType type)
        {
            m_Seats[seatIndex].PlayUIAnimation(type);
        }
        #endregion

        #region PlayUIAnimation 播放UI动画
        /// <summary>
        /// 播放UI动画
        /// </summary>
        /// <param name="type"></param>
        public void PlayUIAnimation(UIAnimationType type)
        {
            string path = string.Format("download/{0}/prefab/uiprefab/uianimations/{1}.drb", ConstDefine.GAME_NAME, type.ToString().ToLower());
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

        #region PlayUIAnimation
        /// <summary>
        /// 播放UI动画
        /// </summary>
        /// <param name="animation"></param>
        public void PlayUIAnimation(string animation)
        {
            string path = string.Format("download/{0}/prefab/uiprefab/uianimations/{1}.drb", ConstDefine.GAME_NAME, animation.ToLower());
            AssetBundleManager.Instance.LoadOrDownload(path, animation.ToLower(), (GameObject go) =>
            {
                if (go != null)
                {
                    go = Instantiate(go);
                    go.SetParent(m_EffectContainer);
                }
            });
        }
        #endregion

        #region SetWaitLiangXi 设置等待亮喜UI显示
        /// <summary>
        /// 设置等待亮喜UI显示
        /// </summary>
        /// <param name="isShow"></param>
        public void SetWaitLiangXi(bool isShow)
        {
            if (m_WaitLiangXi != null)
            {
                m_WaitLiangXi.gameObject.SetActive(isShow);
            }
        }
        #endregion

        private void SetSwapTip(bool isActive)
        {
            if (m_SwapTip != null)
            {
                m_SwapTip.SetActive(isActive);
            }
        }

        private void SetLackColorTip(bool isActive)
        {
            if (m_LackColorTip != null)
            {
                m_LackColorTip.SetActive(isActive);
            }
        }

        public void SetOperating(int seatIndex)
        {
            for (int i = 0; i < m_Seats.Length; ++i)
            {
                m_Seats[i].SetOperating(seatIndex == i);
            }
        }
    }
}
