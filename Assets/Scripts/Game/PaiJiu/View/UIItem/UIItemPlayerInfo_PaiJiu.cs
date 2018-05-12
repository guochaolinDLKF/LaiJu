//===================================================
//Author      : WZQ
//CreateTime  ：7/4/2017 8:49:08 PM
//Description ：玩家UI信息
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace PaiJiu
{
    public class UIItemPlayerInfo_PaiJiu : UIModuleBase
    {
        [SerializeField]
        private RawImage m_ImageHead;//头像
        [SerializeField]
        private Text m_TextGold;//金币
        [SerializeField]
        private Text m_TextNickname;//昵称
        [SerializeField]
        private Image m_Banker;//是否是庄
        //[SerializeField]
        //private Text m_Pour;//下注分数（由UIItemSeat_PaiJiu控制）
        //[SerializeField]
        //private Image m_Trustee;//是否托管（无此项）
        //[SerializeField]
        //private Image m_Ting;//是否听牌（无此项）
        //[SerializeField]
        //private GameObject m_Leave;//是否弃牌（暂无此项）

        private int m_SeatPos;//座位位置


        //private Tweener m_bankerAni;//庄动画

        public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
        {
            Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
            dic.Add(ConstDefine_PaiJiu.ObKey_SetBankerAni, SetBankerAni);
            dic.Add(ConstDefine_PaiJiu.ObKey_SetGoldAni, SetGoldAni); 
            return dic;
        }


        protected override void OnAwake()
        {
            base.OnAwake();
            //注册点击头像事件（暂无此项）
            //Button btn = m_ImageHead.GetComponent<Button>();
            //if (btn != null)
            //{
            //    btn.onClick.AddListener(OnHeadClick);
            //}

            //生成庄动画
            //Transform m_BankerAni = m_Banker.transform.GetChild(0);
            //if (m_BankerAni != null)  m_bankerAni= m_BankerAni.transform.DOScale(2, 0.4f).From().SetAutoKill(false).Pause();
            //m_Banker.transform.localScale = Vector3.one;

        }



        ////头像点击
        //private void OnHeadClick()
        //{
        //    AudioEffectManager.Instance.Play("btnclick", Vector3.zero, false);

        //    if (DelegateDefine.Instance.OnHeadClick != null)
        //    {
        //        DelegateDefine.Instance.OnHeadClick(m_SeatPos);
        //    }
        //}

        #region  SetUI SetAllUI 刷新信息
        //刷新部分
        public void SetUI(PaiJiu.Seat seat)
        {
            m_SeatPos = seat.Pos;
            SetPlayerInfo(seat );
        }

        //刷新全部
        public void SetAllUI(Seat seat)
        {
            m_SeatPos = seat.Pos;
            m_TextGold.SafeSetText(seat.TotalEarnings.ToString());
            m_Banker.gameObject.SetActive(seat.IsBanker );
            m_TextNickname.SafeSetText(seat.Nickname);
            TextureManager.Instance.LoadHead(seat.Avatar, OnAvatarLoadCallBack);
        }
        #endregion

        private void SetPlayerInfo(PaiJiu.Seat seat)
        {
            //m_TextGold.SafeSetText(seat.TotalEarnings.ToString());
            //m_Banker.gameObject.SetActive(seat.IsBanker &&  (currRoom.roomModel !=  proto.paigow.ROOM_MODEL.ROOM_MODEL_GRAB || currRoom.roomStatus !=  proto.paigow.ROOM_STATUS.GRABBANKER));
            m_TextNickname.SafeSetText(seat.Nickname);
            TextureManager.Instance.LoadHead(seat.Avatar, OnAvatarLoadCallBack);
        }


        private void OnAvatarLoadCallBack(Texture2D tex)
        {
            m_ImageHead.texture = tex;
        }

        private void SetBankerAni(TransferData data)
        {
            Seat seat= data.GetValue<Seat>("seat");
            if (seat.Pos != m_SeatPos) return;

            if (seat.IsBanker == m_Banker.gameObject.activeSelf) return;
            m_Banker.gameObject.SetActive(seat.IsBanker);
            Debug.Log("播放庄动画");
            //if (m_bankerAni != null && seat.IsBanker) m_bankerAni.Restart();
        }

        private void SetGoldAni(TransferData data)
        {
            Seat seat = data.GetValue<Seat>("seat");
            if (seat.Pos != m_SeatPos) return;
            Debug.Log(string.Format("刷新金币 pos: {0} gold:{1} ", seat.Pos, seat.TotalEarnings));
            m_TextGold.SafeSetText(seat.TotalEarnings.ToString());

        }

    }
}