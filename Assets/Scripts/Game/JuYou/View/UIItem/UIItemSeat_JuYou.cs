//===================================================
//Author      : WZQ
//CreateTime  ：8/9/2017 8:46:06 PM
//Description ：聚友UISeat
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using proto.jy;
namespace JuYou
{
    public class UIItemSeat_JuYou : UIItemBase
    {
        [SerializeField]
        protected UIItemPlayerInfo_JuYou m_PlayerInfo;

        [SerializeField]
        protected GameObject m_Ready;//准备好了

        [SerializeField]
        protected Transform m_UIAnimationContainer;

        [SerializeField]
        protected Transform m_UIHintLight;//座位提示灯
        private bool isUIHintLight = false;//提示灯是否开启
        private float HintLightInterval = 0.2f;//提示灯闪烁间隔
        [SerializeField]
        protected int m_nSeatIndex = -1;

        [SerializeField]
        protected Text m_UIEarningsText;//飘分物体
        private Tweener tweener;//飘分动画
        [SerializeField]
        private Font[] m_font;

        //[SerializeField]
        //protected Image m_HandPokersType;//手牌牌型
        [SerializeField]
        private Text m_Pour;//下注分数

       
        private void Awake()
        {
            tweener = m_UIEarningsText.transform.DOLocalMoveY(m_UIEarningsText.transform.localPosition.y + 200, 1.5f).SetEase(Ease.Flash).SetAutoKill(false).Pause();
            ModelDispatcher.Instance.AddEventListener(ConstDefine_JuYou.ObKey_SeatInfoChanged, OnSeatInfoChanged);
            ModelDispatcher.Instance.AddEventListener(ConstDefine_JuYou.ObKey_SeatGoldChanged, OnSeatGoldChanged);//座位gold
            //ObKey_SeatGoldChanged
            gameObject.SetActive(false);
            m_UIHintLight.gameObject.SetActive(false);
            
        }

        private void OnDestroy()
        {
            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_JuYou.ObKey_SeatInfoChanged, OnSeatInfoChanged);
            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_JuYou.ObKey_SeatGoldChanged, OnSeatGoldChanged);
        }
        private void OnSeatInfoChanged(TransferData data)
        {
            //PaiJiu.UIItemSeat_PaiJiu
            SeatEntity seat = data.GetValue<SeatEntity>("Seat");
            ROOM_STATUS roomStatus = data.GetValue<ROOM_STATUS>("RoomStatus");
            SeatEntity BankerSeat = data.GetValue<SeatEntity>("BankerSeat");//庄家座位
           
           // PAIGOW_ENUM_ROOM_STATUS roomStatus = data.GetValue<RoomEntity.RoomStatus>("RoomStatus");
            bool IsPlayer = data.GetValue<bool>("IsPlayer");

            if (m_nSeatIndex != seat.Index) return;

            SetSeatInfo(seat, roomStatus);

            //设置已下注
            m_Pour.transform.parent.gameObject.SetActive(seat.Pour > 0);
            m_Pour.SafeSetText(seat.Pour.ToString());
            SetChooseBankerHint(seat.seatStatus == SEAT_STATUS.POUR || seat.seatStatus == SEAT_STATUS.SETTLE);//走马灯
           
        }



        public void SetSeatInfo(SeatEntity seat, ROOM_STATUS roomStatus)
        {

           if(!gameObject.activeSelf  && seat.PlayerId != 0) m_PlayerInfo.SetGold(seat);


            gameObject.SetActive(seat.PlayerId != 0);
            m_PlayerInfo.SetUI(seat);
           m_Ready.gameObject.SetActive(seat.isReady && seat.seatStatus == SEAT_STATUS.IDLE);

        }



        /// <summary>
        /// 设置提示灯（开启提示 选择是否坐庄）
        /// </summary>
        public void SetChooseBankerHint(bool OnOff)
        {
            if (OnOff == isUIHintLight) return;
            m_UIHintLight.gameObject.SetActive(OnOff);
            isUIHintLight = OnOff;
            //开关提示 （灯旋转）
            if (OnOff)
            {
                StartCoroutine("SetUIHintLight");
            }
            else
            {
                StopCoroutine("SetUIHintLight");

            }

        }

        IEnumerator SetUIHintLight()
        {
            while (isUIHintLight)
            {
                m_UIHintLight.Rotate(new Vector3(0, 0, 90));
                yield return new WaitForSeconds(HintLightInterval);
            }

        }


        //个人结算 飘分
       public void  AloneSettle(int earnings )
        {
            if (earnings == 0) return;
            m_UIEarningsText.text = earnings > 0 ? "+" + earnings.ToString() : earnings.ToString();
            m_UIEarningsText.font = earnings >= 0 ? m_font[0] : m_font[1];

            m_UIEarningsText.gameObject.SetActive(true);

            tweener.OnComplete(
                () => {
                   
                    m_UIEarningsText.gameObject.SetActive(false);
                }


                ).Restart();

        }


        private void OnSeatGoldChanged(TransferData data)
        {
            SeatEntity seat = data.GetValue<SeatEntity>("Seat");
            if (m_nSeatIndex != seat.Index) return;
            m_PlayerInfo.SetGold(seat);

        }


    }
}