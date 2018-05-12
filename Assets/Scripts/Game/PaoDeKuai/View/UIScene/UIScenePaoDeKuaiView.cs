//===================================================
//Author      : WZQ
//CreateTime  ：11/18/2017 9:48:46 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
namespace PaoDeKuai {
    public class UIScenePaoDeKuaiView : UISceneViewBase
    {
        #region Variable
        [SerializeField]
        private UIItemPaoDeKuaiSeat[] m_Seats;//座位Item
        [SerializeField]
        private Button m_CancelAuto;//取消托管
        [SerializeField]
        private GameObject[] m_Animations;//开局动画物体
        [SerializeField]
        private Button m_ButtonMicroPhone;//语音
        [SerializeField]
        private Button m_ButtonShare;//微信分享
        [SerializeField]
        private Button m_ButtonReady;//准备
        [SerializeField]
        private Button m_ButtonCancelReady;//取消准备
        [SerializeField]
        private Button m_ButtonAuto;//托管
        [SerializeField]
        private Button m_ButtonChat;//聊天
        [SerializeField]
        private Button m_ButtonSetting;//设置
        [SerializeField]
        private Transform m_EffectContainer;//特效挂载点

        [SerializeField]
        private UIItemPaoDeKuaiOperator m_Operator;//操作项

        [SerializeField]
        private Image m_OperateFeedback;//玩家操作反馈
        private Tweener operateFeedbackTween;

        private bool m_IsBeginAni;//是否正在开局动画

        //private 

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
           
            //m_CancelAuto.gameObject.SetActive(false);
            ModelDispatcher.Instance.AddEventListener(ConstDefine_PaoDeKuai.ON_ROOM_INFO_CHANGED, OnRoomInfoChanged);
            ModelDispatcher.Instance.AddEventListener(ConstDefine_PaoDeKuai.ON_SEAT_INFO_CHANGED, OnSeatInfoChanged);
            //ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_DRAW_POKER, OnSeatDrawPoker);
            //ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_PLAY_POKER, OnSeatPlayPoker);
            //ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_OPERATE, OnSeatOperate);
            //ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_TING, OnSeatTing);
            //ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_ZHIDUI, OnSeatZhiDui);
            //ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_HU, OnSeatHu);

            //回放
            //if (RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Replay)
            //{
            //    m_ButtonMicroPhone.gameObject.SetActive(false);
            //    m_ButtonChat.gameObject.SetActive(false);
            //    m_ButtonAuto.gameObject.SetActive(false);
            //    m_ButtonSetting.gameObject.SetActive(false);
            //}

            m_ButtonShare.gameObject.SetActive(RoomPaoDeKuaiProxy.Instance.CurrentRoom.matchId <= 0);
            m_ButtonReady.gameObject.SetActive(RoomPaoDeKuaiProxy.Instance.CurrentRoom.matchId <= 0);
            if (m_ButtonCancelReady != null)
            {
                m_ButtonCancelReady.gameObject.SetActive(false);
            }

            if (!SystemProxy.Instance.IsInstallWeChat)
            {
                m_ButtonShare.gameObject.SetActive(false);
            }
            m_OperateFeedback.gameObject.SetActive(false);
            operateFeedbackTween = m_OperateFeedback.DOColor(Color.clear, 2).SetEase(Ease.InQuint).SetAutoKill(false).Pause();
        }

        protected override void BeforeOnDestroy()
        {
            base.BeforeOnDestroy();
            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_PaoDeKuai.ON_ROOM_INFO_CHANGED, OnRoomInfoChanged);
            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_PaoDeKuai.ON_SEAT_INFO_CHANGED, OnSeatInfoChanged);
            //ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_DRAW_POKER, OnSeatDrawPoker);
            //ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_PLAY_POKER, OnSeatPlayPoker);
            //ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_OPERATE, OnSeatOperate);
            //ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_TING, OnSeatTing);
            //ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_ZHIDUI, OnSeatZhiDui);
            //ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_HU, OnSeatHu);
        }
        protected override void OnBtnClick(GameObject go)
        {
            base.OnBtnClick(go);
            switch (go.name)
            {
                case ConstDefine.BtnSetting://设置
                    UIViewManager.Instance.OpenWindow(UIWindowType.Setting);
                    break;
                case ConstDefine_PaoDeKuai.BtnPDKViewAuto://托管
                    SendNotification(ConstDefine_PaoDeKuai.BtnPDKViewAuto);
                    break;
                case ConstDefine_PaoDeKuai.BtnPDKViewChat://聊天
                    UIViewManager.Instance.OpenWindow(UIWindowType.Chat);
                    break;
                case ConstDefine_PaoDeKuai.BtnPDKViewCancelAuto://取消托管
                    SendNotification(ConstDefine_PaoDeKuai.BtnPDKViewCancelAuto);
                    break;
                case ConstDefine_PaoDeKuai.BtnPDKViewShare://微信邀请
                    SendNotification(ConstDefine_PaoDeKuai.BtnPDKViewShare);
                    break;
                case ConstDefine_PaoDeKuai.BtnPDKViewReady://准备
                    SendNotification(ConstDefine_PaoDeKuai.BtnPDKViewReady);
                    break;
                case ConstDefine_PaoDeKuai.BtnPDKViewCancelReady://取消准备
                    SendNotification(ConstDefine_PaoDeKuai.BtnPDKViewCancelReady);
                    break;
                case ConstDefine_PaoDeKuai.BtnPDKViewJiPaiQi://记牌器
                    SendNotification(ConstDefine_PaoDeKuai.BtnPDKViewJiPaiQi);
                    break;


                case "btnMaJiangViewRule"://配置详情
                    //UIItemMahJongRoomInfo.Instance.ChangeRuleActive();
                    break;
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

        #region OnRoomInfoChanged 房间信息变更
        /// <summary>
        /// 房间信息变更
        /// </summary>
        /// <param name="obj"></param>
        private void OnRoomInfoChanged(TransferData data)
        {
            RoomEntity seat = data.GetValue<RoomEntity>("Room");

            //SetWaitLiangXi(seat.Status == RoomEntity.RoomStatus.Show || seat.Status == RoomEntity.RoomStatus.Jiao);
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
                if(m_CancelAuto!=null)  m_CancelAuto.gameObject.SetActive(seat.IsTrustee);

                //出牌项
                if(roomStatus!= RoomEntity.RoomStatus.Begin|| seat.Status!= SeatEntity.SeatStatus.Operate )  m_Operator.ShowChuPaiItem(false);


            }
        }
        #endregion

        #region Begin 开局
        /// <summary>
        /// 开局
        /// </summary>
        /// <param name="seatList"></param>
        /// <param name="isPlayAnimation"></param>
        public void Begin(RoomEntity.RoomStatus rommStatus,List< SeatEntity> seatList,bool isPlayAnimation, SeatEntity SpadesThreeSeat, System.Action OnComplete)
        {
            int index = SpadesThreeSeat == null ? 0 : SpadesThreeSeat.Index;

            for (int i = 0; i < seatList.Count; ++i)
            {
                if (seatList[i] == null || seatList[i].PlayerId <= 0) continue;
                List<PokerCtrl> pokerList = PrefabManager.Instance.GetHand(seatList[i].Pos);

              if(pokerList!=null)  m_Seats[seatList[i].Index].Begin(pokerList, isPlayAnimation);

            }

            if (isPlayAnimation)
            {
                m_IsBeginAni = true;
                //黑桃3动画 UIItemNameSpades3
                string path = string.Format("download/{0}/prefab/uiprefab/UIItems/{1}.drb", ConstDefine.GAME_NAME, ConstDefine_PaoDeKuai.UIItemNameSpades3);
                AssetBundleManager.Instance.LoadOrDownload(path, ConstDefine_PaoDeKuai.UIItemNameSpades3, (GameObject go) =>
                {
                    Debug.Log(go.name);
                   

                    if (go != null)
                    {
                      
                        go = Instantiate(go);
                        go.SetParent(m_EffectContainer);
                     
                        go.GetComponent<UIItemPDKSpadesThree>().SetUI(index,OnComplete);
                    }
                });

            }
            else
            {
                if (OnComplete != null) OnComplete();
            }


            //PlayUIAnimation("");
        }
        #endregion


        #region PlayPokers 出牌

        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="playPoker"></param>
        /// <param name="handPoker"></param>
        public void PlayPokers(int seatIndex, List<PokerCtrl> playPoker,PokersType pokerType)
        {
            m_Seats[seatIndex].PlayPokers(playPoker, pokerType);
            if (pokerType == PokersType.Plane) PlayUIAnimation(UIRoomAnimationType.UIRoomAnimation_Plane.ToString());
            if (pokerType == PokersType.Bomb)
            {
                PlayUIAnimation(UIRoomAnimationType.UIRoomAnimation_Bomb.ToString(),
                (GameObject go) => {
                    UIItemPDKBombAni bombAni= go.GetComponent<UIItemPDKBombAni>();
                    if (bombAni != null) bombAni.SetUI(seatIndex);
                   
                }
                );

            }

        }
        #endregion

     



        #region playerOperateFeedback 根据反馈类型提示玩家
        /// <summary>
        /// 根据反馈类型提示玩家
        /// </summary>
        public void playerOperateFeedback(OperateFeedbackType operateFeedbackType)
        {
            string spriteName = "pdk_" + operateFeedbackType.ToString();
            string path = string.Format("download/{0}/source/uisource/paodekuai.drb", ConstDefine.GAME_NAME);
            Sprite sprite = AssetBundleManager.Instance.LoadSprite(path, spriteName);
            if (sprite != null)
            {
                m_OperateFeedback.sprite = sprite;
                m_OperateFeedback.gameObject.SetActive(true);
                operateFeedbackTween.OnComplete(
                    () => { m_OperateFeedback.gameObject.SetActive(false); }
                    ).Restart();
            }

        }
        #endregion


     

        #region       
        /// <summary>
        /// 结算
        /// </summary>
        /// <param name="onComplete"></param>
      public  IEnumerator Settle(List<SeatEntity> seatList,System.Action onComplete)
        {
            yield return 0;
            for (int i = 0; i < seatList.Count; ++i)
            {
                List<PokerCtrl> handPoker = PrefabManager.Instance.GetHand(seatList[i].Pos);
                PrefabManager.Instance.SortHandPokers(seatList[i].Pos);
                for (int j = 0; j < handPoker.Count; ++j)
                {
                    handPoker[j].SetSprites();
                }

             if(seatList[i].Index!=0) m_Seats[seatList[i].Index].ShowHandPokers( handPoker);

            }


            //for (int i = 1; i < m_Seats.Length; ++i)
            //{
            //    m_Seats[i].ShowHandPokers();
            //}
            yield return new  WaitForSeconds(2f);
            if (onComplete != null) onComplete();


        }
        #endregion




        #region 
        #endregion
        /// <summary>
        /// 以座位号获得UIItemSeat
        /// </summary>
        /// <param name="seatPos"></param>
        /// <returns></returns>
        private UIItemPaoDeKuaiSeat GetUIItemSeat(int seatPos)
        {
            for (int i = 0; i < m_Seats.Length; ++i)
            {
                if (seatPos == m_Seats[i].SeatPos) return m_Seats[i];
            }
            return null;
        }



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


        #region PlayUIAnimation
        /// <summary>
        /// 播放UI动画
        /// </summary>
        /// <param name="animation"></param>
        public void PlayUIAnimation(string animation,System.Action<GameObject> onLoadComplete=null)
        {
            string path = string.Format("download/{0}/prefab/uiprefab/uianimations/{1}.drb", ConstDefine.GAME_NAME, animation.ToLower());
            AssetBundleManager.Instance.LoadOrDownload(path, animation.ToLower(), (GameObject go) =>
            {
                if (go != null)
                {
                    go = Instantiate(go);
                    go.SetParent(m_EffectContainer);
                    if (onLoadComplete != null) onLoadComplete(go);
                }
            });
        }
        #endregion




    }






    //房间动画
    public enum UIRoomAnimationType
    {
        UIRoomAnimation_Plane,//飞机
        UIRoomAnimation_Bomb,//炸弹
    }

    //座位动画
    public enum UISeatAnimationType
    {
       
        UISeatAnimation_Bomb,//炸弹
    }

    public enum OperateFeedbackType
    {
        None,
        PokerTypeError,//牌型不正确
        NoBigPoker, //没有大于上家的牌


    }

}