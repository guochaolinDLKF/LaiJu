//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 3:46:35 PM
//Description ：
//===================================================
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace DRB.DouDiZhu
{
    public class UIItemPoker : UIItemBase, IComparable<UIItemPoker>, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
    {

        private static readonly Color GrayColor = new Color(0.8f, 0.8f, 0.8f, 1);

        public delegate void OnPointerHandler(UIItemPoker item);
        public OnPointerHandler OnPointerDown;
        public OnPointerHandler OnPointerUp;
        public OnPointerHandler OnPointerEnter;

        [SerializeField]
        private Image m_Image;
        [SerializeField]
        private Image m_BankerImage;
        public Image image
        {
            get
            {
                return m_Image;
            }
        }
        public Image bankerImage
        {
            get
            {
                return m_BankerImage;
            }
        }

        public Poker Poker;
        [SerializeField]
        private bool m_isSelect;
        public bool isSelect
        {
            get
            {
                return m_isSelect;
            }
            set
            {
                if (m_isSelect == value) return;
                m_isSelect = value;
                m_Image.transform.localPosition = m_isSelect ? new Vector3(0f, 40f, 0f) : Vector3.zero;
            }
        }

        private bool m_isGray;
        public bool isGray
        {
            get
            {
                return m_isGray;
            }
            set
            {
                if (m_isGray == value) return;
                m_isGray = value;
                m_Image.color = m_isGray ? GrayColor : Color.white;
            }
        }

        [SerializeField]
        private bool m_isBanker;
        public bool isBanker
        {
            get
            {
                return m_isBanker;
            }
            set
            {
                if (m_isBanker == value) return;
                m_isBanker = value;
                m_BankerImage.SafeSetActive(m_isBanker);
            }
        }



        public void Init(Poker poker)
        {
            Poker = poker;
            string path = string.Format("download/{0}/source/uisource/gameuisource/doudizhupoker.drb", ConstDefine.GAME_NAME, poker.ToString());
            string name = poker.ToString();
            m_Image.overrideSprite = AssetBundleManager.Instance.LoadSprite(path, name);

            isSelect = false;
        }


        protected override void OnAwake()
        {
            base.OnAwake();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (OnPointerUp != null)
            {
                OnPointerUp(this);
            }
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (OnPointerDown != null)
            {
                OnPointerDown(this);
            }
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (OnPointerEnter != null)
            {
                OnPointerEnter(this);
            }
        }

        public int CompareTo(UIItemPoker other)
        {
            if (Poker == null) return 1;
            if (other == null) return -1;
            if (other.Poker == null) return -1;

            return Poker.CompareTo(other.Poker);
        }
        public void MovePoker(Vector3 targetVector, float duration, Action<UIItemPoker> complete = null)
        {           
            Tweener tweener = transform.DOLocalMove(transform.localPosition + targetVector, duration).OnComplete(() => { image.transform.localPosition = Vector3.zero; complete(this); });
            tweener.SetEase(Ease.OutQuad);
        }
        public void ClearAction()
        {
            OnPointerDown = null;
            OnPointerEnter = null;
            OnPointerUp = null;
        }
    }
}
