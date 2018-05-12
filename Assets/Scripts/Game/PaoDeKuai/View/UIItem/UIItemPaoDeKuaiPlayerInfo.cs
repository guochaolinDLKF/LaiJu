//===================================================
//Author      : WZQ
//CreateTime  ：11/22/2017 11:59:50 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace PaoDeKuai
{
    public class UIItemPaoDeKuaiPlayerInfo : UIModuleBase
    {
        [SerializeField]
        private RawImage m_ImageHead;
        [SerializeField]
        private Text m_TextGold;
        [SerializeField]
        private Text m_TextNickname;
        [SerializeField]
        private Image m_Banker;
        [SerializeField]
        private Image m_Trustee;
        [SerializeField]
        private Image m_isLandlord;//房主
        [SerializeField]
        private GameObject m_Leave;
        [SerializeField]
        private Image m_ImgOperating;
        [SerializeField]
        private UIPDKMoveText m_MoveText;

        private int m_SeatPos;

        public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
        {
            Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
            //dic.Add(RoomMaJiangProxy.ON_CURRENT_OPERATOR_CHANGED, OnOperatorChanged);
            return dic;
        }

        private void OnOperatorChanged(TransferData obj)
        {
            int seatPos = obj.GetValue<int>("SeatPos");
            SetOperating(seatPos == m_SeatPos);
        }

        protected override void OnAwake()
        {
            Button btn = m_ImageHead.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(OnHeadClick);
            }

            if (m_ImgOperating != null)
            {
                m_ImgOperating.gameObject.SetActive(false);
                m_ImgOperating.fillAmount = 0f;
                PlayForwardAnimation();
            }
        }

        private void PlayForwardAnimation()
        {
            m_ImgOperating.transform.localEulerAngles = Vector3.zero;
            m_ImgOperating.DOFillAmount(1f, 2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                PlayBackwardsAnimation();
            });
        }

        private void PlayBackwardsAnimation()
        {
            m_ImgOperating.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            m_ImgOperating.DOFillAmount(0f, 2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                PlayForwardAnimation();
            });
        }

        private void OnHeadClick()
        {
            AudioEffectManager.Instance.Play("btnclick", Vector3.zero, false);

            SendNotification(ConstDefine_PaoDeKuai.BtnPDKViewHeadClick, m_SeatPos);
        }

        public void SetUI(SeatEntity seat)
        {
            m_SeatPos = seat.Pos;
            SetPlayerInfo(seat);
        }

        private void SetPlayerInfo(SeatEntity seat)
        {
            //m_Leave.SetActive(seat.IsWaiver || !seat.IsFocus);
            m_Leave.SetActive( !seat.IsFocus);
            m_TextGold.SafeSetText(seat.Gold.ToString());
           if(m_Banker!=null) m_Banker.gameObject.SetActive(seat.IsBanker);
            m_TextNickname.SafeSetText(seat.Nickname);
            m_Trustee.gameObject.SetActive(seat.IsTrustee);
            m_isLandlord.gameObject.SetActive(seat.isLandlord);

            TextureManager.Instance.LoadHead(seat.Avatar, (Texture2D tex) =>
            {
                if (m_ImageHead != null)
                {
                    m_ImageHead.texture = tex;
                }
            });

 
        }

        public void SetGold(int changeGold, int gold)
        {
          

            if (m_MoveText != null)
            {
                m_MoveText.AddText(changeGold.ToString(ConstDefine.STRING_FORMAT_SIGN), default(Color), changeGold >= 0 ? 0 : 1, false,
                       () =>
                       {
                           m_TextGold.SafeSetText(gold.ToString());
                       }
                       );
            }
        }

        public void SetOperating(bool isOperating)
        {
            if (m_ImgOperating != null)
            {
                m_ImgOperating.gameObject.SetActive(isOperating);
            }
        }


    }
}