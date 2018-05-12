//===================================================
//Author      : WZQ
//CreateTime  ：11/22/2017 11:55:07 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace PaoDeKuai
{
    public class UIItemPaoDeKuaiSeat : UIItemBase
    {
        [SerializeField]
        protected UIItemPaoDeKuaiPlayerInfo m_PlayerInfo;//玩家信息

        [SerializeField]
        protected GameObject m_Ready;//已准备

        [SerializeField]
        protected Transform m_UIAnimationContainer;//座位特效挂载点

        [SerializeField]
        protected Text m_RemainingPokerNum;//剩余牌数量
        [SerializeField]
        protected Grid3D m_UIHandPokerContainer;//手牌挂载点
        [SerializeField]
        protected Transform m_UIRoundPokerContainer;//打出的牌挂载点
        [SerializeField]
        protected Image m_UIRoundPokerType;//打出的牌牌型
        private Tweener m_PokerTypeTween;
        

        [SerializeField]
        protected Image m_UIPass;//过

      
 



        [SerializeField]
        protected int m_nSeatIndex = -1;//index
        [SerializeField]
        protected Button m_BtnChangeSeat;//换座位按钮

        private int m_SeatPos;
        public int SeatPos { get { return m_SeatPos; }  }

        public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
        {
            Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
            dic.Add(ConstDefine_PaoDeKuai.ON_SEAT_INFO_CHANGED, OnSeatInfoChanged);
            //dic.Add(ConstDefine_PaoDeKuai.ON_StatePass_CHANGED, OnSeatPassChanged);//Pass变更
            dic.Add(ConstDefine_PaoDeKuai.ON_SEAT_GOLD_CHANGED, OnSeatGoldChanged);
            //dic.Add(ConstDefine_PaoDeKuai.ON_SEAT_INFO_CLEAR, OnSeatInfoClear);
            return dic;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            m_PlayerInfo.gameObject.SetActive(false);
            m_UIPass.gameObject.SetActive(false);
            m_Ready.gameObject.SetActive(false);
            m_RemainingPokerNum.transform.parent.gameObject.SetActive(false);
            if (m_BtnChangeSeat != null)
            {
                m_BtnChangeSeat.gameObject.SetActive(false);
                m_BtnChangeSeat.onClick.AddListener(OnChangeSeatClick);
            }
           
            m_UIRoundPokerType.gameObject.SetActive(false);
            m_PokerTypeTween = m_UIRoundPokerType.transform.DOLocalMove(Vector3.zero, 1.5f).SetEase(Ease.OutQuint ).SetAutoKill(false).Pause();
          
      

        }

        private void OnChangeSeatClick()
        {
            SendNotification("OnBtnChangeSeatClick", m_nSeatIndex);
        }

        private void OnSeatGoldChanged(TransferData data)
        {
            int seatIndex = data.GetValue<int>("SeatIndex");
            int changeGold = data.GetValue<int>("ChangeGold");
            int gold = data.GetValue<int>("Gold");

            if (m_nSeatIndex == seatIndex)
            {
                m_PlayerInfo.SetGold(changeGold, gold);
            }
            
        }
        #region 座位信息变更
        private void OnSeatInfoChanged(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("Seat");
            RoomEntity.RoomStatus roomStatus = data.GetValue<RoomEntity.RoomStatus>("RoomStatus");
            SeatEntity.SeatStatus playerStatus = data.GetValue<SeatEntity.SeatStatus>("PlayerStatus");
            
            SetSeatInfo(seat, roomStatus, playerStatus);
        }

        private void SetSeatInfo(SeatEntity seat, RoomEntity.RoomStatus roomStatus, SeatEntity.SeatStatus playerStatus)
        {
            if (m_nSeatIndex == seat.Index)
            {
                m_SeatPos = seat.Pos;
                if (seat.PlayerId == 0)
                {
                    m_PlayerInfo.gameObject.SetActive(false);
                    m_Ready.gameObject.SetActive(false);
                    m_UIPass.gameObject.SetActive(false);
                    if (roomStatus == RoomEntity.RoomStatus.Ready && playerStatus == SeatEntity.SeatStatus.Idle)
                    {
                        if (m_BtnChangeSeat != null)
                        {
                            m_BtnChangeSeat.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        if (m_BtnChangeSeat != null)
                        {
                            m_BtnChangeSeat.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    m_UIPass.gameObject.SetActive(seat.IsPass);
                    m_PlayerInfo.gameObject.SetActive(true);
                    m_Ready.gameObject.SetActive(seat.Status == SeatEntity.SeatStatus.Ready);
                    if (m_BtnChangeSeat != null)
                    {
                        m_BtnChangeSeat.gameObject.SetActive(false);
                    }
                }
                m_RemainingPokerNum.transform.parent.gameObject.SetActive(roomStatus == RoomEntity.RoomStatus.Begin);
               if (m_RemainingPokerNum.transform.parent.gameObject.activeInHierarchy) m_RemainingPokerNum.SafeSetText(seat.pokerList.Count.ToString());

                m_PlayerInfo.SetUI(seat);

                //if (seat.isDouble && roomStatus == RoomEntity.RoomStatus.Pao)
                //{
                //    PlayUIAnimation(UIAnimationType.UIAnimation_Pao);
                //}
            }
        }
        #endregion

        #region Begin 开局发牌
        public void Begin(List<PokerCtrl> handPoker,bool isPlayAnimation, bool showHandPokers=false)
        {
            m_UIHandPokerContainer.gameObject.SetActive( (m_nSeatIndex == 0 || showHandPokers) );
            //手牌挂载
            for (int i = handPoker.Count - 1; i >= 0; --i)
            {
                //Debug.Log("挂载"+ handPoker[i].Poker.ToChinese());
                handPoker[i].gameObject.SetParent(m_UIHandPokerContainer.transform, true);
            }
            if (isPlayAnimation)
            {
                StartCoroutine(BeginAni(handPoker));
            }
            else
            {
                m_UIHandPokerContainer.Sort();
                for (int i = 0; i < handPoker.Count; ++i)
                {
                    handPoker[i].gameObject.SetActive(true);
                }
            }
        }
        /// <summary>
        /// 开局发牌动画
        /// </summary>
        /// <param name="handPoker"></param>
        /// <returns></returns>
        private IEnumerator BeginAni(List<PokerCtrl> handPoker)
        {
           
            yield return 0;
            for (int i = handPoker.Count - 1; i >= 0; --i)
            {
                //Debug.Log("移动"+ m_UIHandPokerContainer.GetLocalPos(handPoker[i].transform).ToString() + handPoker[i].Poker.ToChinese());
                handPoker[i].gameObject.SetActive(true);
             
                handPoker[i].transform.DOLocalMove(m_UIHandPokerContainer.GetLocalPos(handPoker[handPoker.Count - 1 - i].transform), 0.3f,true);
                yield return 0;

            }


        }
        #endregion


        #region
        #endregion
        #region PlayPokers  出牌
        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="playPoker"></param>
        /// <param name="pokerType"></param>
        public void PlayPokers( List<PokerCtrl> playPoker, PokersType pokerType)
        {
           

            for (int i = 0; i < playPoker.Count; ++i)
            {
                playPoker[i].gameObject.SetParent(m_UIRoundPokerContainer.transform, true);
                playPoker[i].gameObject.SetActive(true);
            }
           //int childCount= m_UIHandPokerContainer.transform.GetChildCount();

            m_UIHandPokerContainer.Sort();


            string spriteName = "PDKPokersType_" + pokerType.ToString();
            string path = string.Format("download/{0}/source/uisource/paodekuai.drb", ConstDefine.GAME_NAME);
            Sprite sprite = AssetBundleManager.Instance.LoadSprite(path, spriteName);

            Debug.Log("-----------sprite == null-----------------------------------------"+ sprite == null);
            if (sprite != null)
            {



                m_UIRoundPokerType.sprite = sprite;
                m_UIRoundPokerType.gameObject.SetActive(true);

                m_PokerTypeTween.OnComplete(
                    () => { m_UIRoundPokerType.gameObject.SetActive(false); }

                    ).Restart();



            }
           

        }
        #endregion

     

        #region ShowHandPokers 开牌 
        /// <summary>
        /// 开牌
        /// </summary>
        public void ShowHandPokers(List<PokerCtrl> handPoker)
        {
            for (int i = 0; i < handPoker.Count; ++i)
            {
                handPoker[i].transform.SetSiblingIndex(i);
            }
            m_UIHandPokerContainer.Sort();
            m_UIHandPokerContainer.gameObject.SetActive(true);

        }
        #endregion




        #region PlayUIAnimation 播放座位UI动画
        /// <summary>
        /// 播放座位UI动画
        /// </summary>
        /// <param name="type"></param>
        public void PlayUIAnimation(UISeatAnimationType type)
        {
            string path = string.Format("download/{0}/prefab/uiprefab/uianimations/{1}.drb", ConstDefine.GAME_NAME, type.ToString().ToLower());
            AssetBundleManager.Instance.LoadOrDownload(path, type.ToString().ToLower(), (GameObject go) =>
            {
                if (go != null)
                {
                    go = Instantiate(go);
                    go.SetParent(m_UIAnimationContainer);
                }
                else
                {
                    AppDebug.LogWarning("UI动画" + type + "不存在");
                }
            });
        }
        #endregion

        public void SetOperating(bool isOperating)
        {
            //m_PlayerInfo.SetOperating(isOperating);
        }


        #region OnSeatInfoClear 清空座位信息
        /// <summary>
        /// 清空座位信息
        /// </summary>
        /// <param name="obj"></param>
        private void OnSeatInfoClear(TransferData obj)
        {
            m_PlayerInfo.gameObject.SetActive(false);
            m_Ready.gameObject.SetActive(false);
            if (m_BtnChangeSeat != null)
            {
                m_BtnChangeSeat.gameObject.SetActive(false);
            }
        }
        #endregion
    }




}