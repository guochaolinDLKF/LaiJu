//===================================================
//Author      : DRB
//CreateTime  ：11/30/2017 8:23:48 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ShiSanZhang
{
    public class UIItemPoker_ShiSanZhang : UIItemBase
    {
        [SerializeField]
        private Button m_Button;      
        [SerializeField]
        private  Image positive;//正面图片
        [SerializeField]
        private  Image reverse;//背面图片

        public Action<UIItemPoker_ShiSanZhang> onClick;

        private Tweener flipCardsAni;
        private bool isBeenPlayed = false;

        private Poker poker;
        public Poker Poker
        {
            get { return poker; }
            private set { poker = value; }
        }

        private bool m_isSelect;

        public bool isSelect
        {
            get { return m_isSelect; }
            set
            {
                if (m_isSelect == value) return;
                m_isSelect = value;
                m_Button.gameObject.transform.localPosition = m_isSelect ? new Vector3(0f, 40f, 0f) : Vector3.zero;
            }
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            positive.gameObject.SetActive(false);
            reverse.gameObject.SetActive(true);
            flipCardsAni = transform.DOScaleX(0, 0.1f).SetEase(Ease.Linear).SetAutoKill(false).Pause();
            m_Button.onClick.AddListener(OnClick);
        }

        public  void Init(Poker poker,bool isPlayAnimation=false, bool isInit=false)
        {
            Poker = poker;
            string path = string.Format("download/{0}/source/uisource/gameuisource/shisanzhangpoker.drb", ConstDefine.GAME_NAME, poker.ToString());
            string name = poker.ToString();
            if (isInit)
                InitFlipCardsAni();
            if (!isPlayAnimation)
                InitPlayAnimation();
            if (name == "0_0")          
                positive.overrideSprite = AssetBundleManager.Instance.LoadSprite(path, name);           
            else           
                positive.overrideSprite = ShiSanZhangPrefabManager.Instance.LoadSprite(Poker);           
            isSelect = false;
        }


        //button 点击事件
        private void OnClick()
        {
            if (onClick != null)
            {
                onClick(this);
            }
        }


        public void InitFlipCardsAni()
        {
            isBeenPlayed = false;
            positive.gameObject.SetActive(false);
            reverse.gameObject.SetActive(true);
        }
        public void InitPlayAnimation()
        {
            isBeenPlayed = false;
            positive.gameObject.SetActive(true);
            reverse.gameObject.SetActive(false);
        }

        /// <summary>
        /// 正向翻牌
        /// </summary>
        public void FlipCardsForward()
        {
            if (isBeenPlayed) return;
            isBeenPlayed = true;
            StartCoroutine(FlipCards(true));      
        }
        IEnumerator FlipCards(bool isForward)
        {
            yield return 0;
            //正向旋转
            flipCardsAni.OnComplete(() =>
            {
                positive.gameObject.SetActive(isForward);
                reverse.gameObject.SetActive(!isForward);
                flipCardsAni.PlayBackwards();
            }
            ).Restart();
            //transform.localEulerAngles = new Vector3(0, 90, 0);
        }


        public void SetSelected(bool isSelected)
        {
            //SelectedSprites.gameObject.SetActive(isSelected);

        }
    }
}
