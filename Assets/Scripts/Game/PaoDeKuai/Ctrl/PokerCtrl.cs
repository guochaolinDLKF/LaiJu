//===================================================
//Author      : WZQ
//CreateTime  ：11/22/2017 6:59:20 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace PaoDeKuai
{
    public class PokerCtrl : MonoBehaviour
    {
        [SerializeField]
        private Image m_PokerSprites;

        [SerializeField]
        private Image SelectedSprites;//拖拽选中图片
        /// <summary>
        /// 数据
        /// </summary>
        [SerializeField]
        private Poker m_Poker;
        public Poker Poker
        {
            get { return m_Poker; }
            private set { m_Poker = value; }
        }


     


        private bool m_isHold = false;
        public bool IsHold { get { return m_isHold; } }
        private Vector3 m_HoldPosDiff = new Vector3(0, 50, 0);


        private void Awake()
        {
            //m_Model = transform.GetChild(0).gameObject;
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.size = new Vector2(200, 316);
            //BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            //collider.size = new Vector3(200, 316,0);
            SelectedSprites.gameObject.SetActive(false);
        }


        /// <summary>
        /// Poker初始化
        /// </summary>
        /// <param name="poker"></param>
        public void Init(Poker poker)
        {
            Reset();
            m_Poker = poker;
            SetSprites();
        }


        /// <summary>
        /// 设置图片
        /// </summary>
        public void SetSprites()
        {
            if (m_Poker == null || string.Compare(m_PokerSprites.sprite.name, m_Poker.ToString(), System.StringComparison.CurrentCultureIgnoreCase) == 0)
                return;
            Sprite pokerSprite = PrefabManager.Instance.LoadPokerSprite(m_Poker);
            if (pokerSprite != null)
                m_PokerSprites.sprite = pokerSprite;

        }



        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            m_Poker = null;
            //将抽出状态还原等s
            SetHold(false);
        }



        //设置拖拽选中
        public void SetSelected(bool isSelected)
        {
            SelectedSprites.gameObject.SetActive(isSelected);

        }



        //设置抽出状态
        public void SetHold()
        {
            //if (m_isHold == isHold) return;

            m_isHold = !m_isHold;
            //设置位置等
            transform.localPosition = transform.localPosition + (m_isHold ? m_HoldPosDiff : -1 * m_HoldPosDiff);
        }

        public void SetHold(bool isHold)
        {
            if (m_isHold == isHold) return;
            m_isHold = isHold;
            //设置位置等
            transform.localPosition = transform.localPosition+( m_isHold ? m_HoldPosDiff :-1*m_HoldPosDiff);
           

        }








    }
}