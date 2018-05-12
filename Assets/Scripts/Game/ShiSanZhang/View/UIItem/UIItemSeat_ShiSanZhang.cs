//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 3:58:45 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using proto.sss;
using DG.Tweening;

namespace ShiSanZhang
{
    public class UIItemSeat_ShiSanZhang : UIItemBase
    {
        [SerializeField]
        private UIItemPlayerInfo_ShiSanZhang m_PlayerInfo;      
        [SerializeField]
        private Image m_imgReady;
        [SerializeField]
        private int m_Index;
        [SerializeField]
        private Transform m_DealContainer;//发牌挂载点     
        [SerializeField]
        private Transform[] m_MoveContainer;//移动挂载点
        [SerializeField]
        private Transform m_HandContainer;//手牌挂载点

        [SerializeField]
        private Transform m_TouGroupContainer;//组合完成头道牌挂载点
        [SerializeField]
        private Transform m_ZhongGroupContainer;//组合完成中道牌挂哉点
        [SerializeField]
        private Transform m_WeiGroupContainer;//组合完成尾道牌挂载点
        [SerializeField]
        private Transform[] RoundOntainer;//旋转的挂载点


        private int m_SeatPos;
        public int SeatPos { get { return m_SeatPos; } }

        public List<UIItemPoker_ShiSanZhang> HandList
        {
            get{return m_HandList;}
            set{m_HandList = value;}
        }

        public Transform HandContainer
        {
            get{ return m_HandContainer;}
            set{m_HandContainer = value; }
        }

        public List<UIItemPoker_ShiSanZhang> HandPokerTou
        {
            get{return m_HandPokerTou;}
            set{m_HandPokerTou = value;}
        }

        public List<UIItemPoker_ShiSanZhang> HandPokerZhong
        {
            get{ return m_HandPokerZhong;}
            set{m_HandPokerZhong = value;}
        }

        public List<UIItemPoker_ShiSanZhang> HandPokerWei
        {
            get{return m_HandPokerWei;}
            set{m_HandPokerWei = value;}
        }

        private List<UIItemPoker_ShiSanZhang> m_HandList = new List<UIItemPoker_ShiSanZhang>();//手牌存储

        private List<UIItemPoker_ShiSanZhang> m_HandPokerTou = new List<UIItemPoker_ShiSanZhang>();//头道手牌

        private List<UIItemPoker_ShiSanZhang> m_HandPokerZhong = new List<UIItemPoker_ShiSanZhang>();//中道手牌

        private List<UIItemPoker_ShiSanZhang> m_HandPokerWei = new List<UIItemPoker_ShiSanZhang>();//尾道手牌


        public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
        {
            Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
            dic.Add(RoomShiSanZhangProxy.ON_SEAT_INFO_CHANGED, OnSeatInfoChanged);        
            return dic;
        }



        #region OnSeatInfoChanged 座位信息变更
        /// <summary>
        /// 座位信息变更
        /// </summary>
        /// <param name="data"></param>
        private void OnSeatInfoChanged(TransferData data)
        {                     
            SeatEntity seat = data.GetValue<SeatEntity>("SeatEntity");
            ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
            SetSeatInfo(seat, roomStatus);        
        }
        #endregion


        private void SetSeatInfo(SeatEntity seat, ROOM_STATUS roomStatus)
        {
            if (m_Index == seat.Index)
            {
                m_SeatPos = seat.Pos;
                if (seat.PlayerId == 0)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    gameObject.SetActive(true);
                }
                m_PlayerInfo.SetUI(seat);
                m_imgReady.gameObject.SetActive(seat.seatStatus == SEAT_STATUS.SEAT_STATUS_READY && roomStatus == ROOM_STATUS.ROOM_STATUS_IDLE);               
            }
        }

        #region 开局相关 
        /// <summary>
        /// 开局
        /// </summary>
        /// <param name="myPoker"></param>
        public void Begin(List<Poker> myPoker, bool isPlayAnimation)
        {                    
            myPoker.Sort(Comparator);
            for (int i = 0; i < myPoker.Count; i++)
            {       
                UIItemPoker_ShiSanZhang item = SpawnPoker(myPoker[i], isPlayAnimation);              
                SetPokerContainer(item.gameObject.transform, m_DealContainer, 0.75f);
                if (m_Index == 0) item.gameObject.layer = 11;
                m_HandList.Add(item);
            }
            StartCoroutine(DealMovePokerTor(isPlayAnimation));
            
        }

        IEnumerator DealMovePokerTor(bool isPlayAnimation)
        {
            yield return new WaitForSeconds(0.5f);
            DealMovePoker(isPlayAnimation);
        }


        private void SetPokerContainer(Transform tran, Transform ParentTran, float scale = 1)
        {
            tran.SetParent(ParentTran);
            tran.localScale = new Vector3(scale, scale, scale);
            tran.localPosition = Vector3.zero;
        }
        public int Comparator(Poker x, Poker y)
        {
            if (x.Size <= y.Size)
                return 1;
            else
                return -1;
        }

        /// <summary>
        /// 生成牌
        /// </summary>
        /// <param name="poker"></param>
        /// <returns></returns>
        private UIItemPoker_ShiSanZhang SpawnPoker(Poker poker,bool isPlayAnimation)
        {
            Transform trans = UIPoolManager.Instance.Spawn("UIItemPoker_ShiSanZhang");
            UIItemPoker_ShiSanZhang item = trans.GetComponent<UIItemPoker_ShiSanZhang>();
            item.Init(poker, isPlayAnimation, true);          
            return item;
        }



        /// <summary>
        /// 发牌位移
        /// </summary>
        private void DealMovePoker(bool isPlayAnimation)
        {
            if (isPlayAnimation)
            {
                for (int i = 0; i < m_HandList.Count; i++)
                {
                    m_HandList[i].gameObject.transform.DOMove(m_MoveContainer[0].position, 0.5f).OnComplete(MovePokerX);
                    if(m_Index==0) m_HandList[i].gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f);
                      else m_HandList[i].gameObject.transform.DOScale(new Vector3(0.6f, 0.6f, 0.6f), 0.3f);
                }
            }
            else
            {
                for (int i = 0; i < m_HandList.Count; i++)
                {
                    if(m_Index == 0)
                    {
                        m_HandList[i].gameObject.transform.SetParent(m_HandContainer);
                        m_HandList[i].gameObject.transform.localScale = Vector3.one;
                        m_HandList[i].gameObject.transform.localPosition = Vector3.zero;
                    }
                    else
                    {
                        m_HandList[i].gameObject.transform.SetParent(m_HandContainer);
                        m_HandList[i].gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                        m_HandList[i].gameObject.transform.localPosition = Vector3.zero;
                    }                    
                }
            }            
        }
        /// <summary>
        /// 展开牌
        /// </summary>
        private void MovePokerX()
        {
            for (int i = 0; i < m_HandList.Count; i++)
            {
                if (m_Index == 0)
                {
                    m_HandList[i].gameObject.transform.DOMove(m_MoveContainer[i].position, 1f).OnComplete(OnCompleteDoTween);
                }
                else
                {
                    m_HandList[i].gameObject.transform.DOMove(m_MoveContainer[i].position, 1f);
                    
                }               
            }
        }
        /// <summary>
        /// 翻牌
        /// </summary>
        private  void OnCompleteDoTween()
        {
            for (int i = 0; i < m_HandList.Count; i++)
            {
                m_HandList[i].gameObject.SetParent(m_HandContainer);
                m_HandList[i].FlipCardsForward();
                m_HandList[i].onClick = OnPokerClick;
            }

        }

        #region OnPokerClick 牌点击
        /// <summary>
        /// 牌点击
        /// </summary>
        /// <param name="poker"></param>
        private void OnPokerClick(UIItemPoker_ShiSanZhang poker)
        {
            Debug.Log("点击牌了啊！！！！！！！！     "+ poker.Poker.Size);
            for (int i = 0; i < m_HandList.Count; ++i)
            {
                if (m_HandList[i] == poker)
                {
                    Debug.Log("点击牌了啊！！！！！！！！     " + poker.Poker.Size);
                    m_HandList[i].isSelect = !m_HandList[i].isSelect;
                    UITipPokerShiSanZhangView.Instance.TipKuang();
                    //SendNotification("OnShiSanZhangPokerClick", poker);
                    break;
                }
            }
        }
        #endregion
        #endregion

        #region 组合牌完成
        /// <summary>
        /// 组合牌完成
        /// </summary>
        /// <param name="seat"></param>
        public void GroupPokerShow()
        {
            if (m_Index == 0)
            {
                for (int i = 0; i < m_HandPokerTou.Count; i++)
                {
                    SetPokerContainer(m_HandPokerTou[i].gameObject.transform, m_TouGroupContainer);
                }
                for (int i = 0; i < m_HandPokerZhong.Count; i++)
                {
                    SetPokerContainer(m_HandPokerZhong[i].gameObject.transform, m_ZhongGroupContainer);
                    SetPokerContainer(m_HandPokerWei[i].gameObject.transform, m_WeiGroupContainer);
                }
                SetAngle();
            }
            else
            {
                for (int i = 0; i < m_HandList.Count; i++)
                {
                    if (i<3)
                    {
                        m_HandPokerTou.Add(m_HandList[i]);
                        SetPokerContainer(m_HandList[i].gameObject.transform, m_TouGroupContainer);                      
                    }
                    else if (3<=i&&i<8)
                    {
                        m_HandPokerZhong.Add(m_HandList[i]);
                        SetPokerContainer(m_HandList[i].gameObject.transform, m_ZhongGroupContainer);
                    }
                    else
                    {
                        m_HandPokerWei.Add(m_HandList[i]);
                        SetPokerContainer(m_HandList[i].gameObject.transform, m_WeiGroupContainer);
                    }
                }
                m_HandList.Clear();
                SetAngle();
            }
        }
        /// <summary>
        /// 设置父物体
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="panrt"></param>
        private void SetPokerContainer(Transform tran,Transform panrt)
        {
            tran.SetParent(panrt);
            tran.localPosition = Vector3.zero;
            tran.localScale = new Vector3(0.58f,0.58f,0.58f);
        }
        /// <summary>
        /// 设置旋转角度
        /// </summary>
        private void SetAngle()
        {
            for (int i = 0; i < m_HandPokerTou.Count; i++)
            {
                float a = ((10f * (m_HandPokerTou.Count - 1)) / 2) - i * 10f;
                m_HandPokerTou[i].gameObject.transform.RotateAround(RoundOntainer[0].position, Vector3.forward, a);
            }
            for (int i = 0; i < m_HandPokerZhong.Count; i++)
            {
                float a = ((10f * (m_HandPokerZhong.Count - 1)) / 2) - i * 10f;
                m_HandPokerZhong[i].gameObject.transform.RotateAround(RoundOntainer[1].position, Vector3.forward, a);
            }
            for (int i = 0; i < m_HandPokerWei.Count; i++)
            {
                float a = ((10f * (m_HandPokerWei.Count - 1)) / 2) - i * 10f;
                m_HandPokerWei[i].gameObject.transform.RotateAround(RoundOntainer[2].position, Vector3.forward, a);
            }
        }
        #endregion
    }
}

