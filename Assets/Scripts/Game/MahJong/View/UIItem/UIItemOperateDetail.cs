//===================================================
//Author      : DRB
//CreateTime  ：4/28/2017 12:01:56 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DRB.MahJong
{
    public class UIItemOperateDetail : MonoBehaviour
    {
        [SerializeField]
        private Transform m_Container;
        [SerializeField]
        private Image[] m_ImagePoker;
        [SerializeField]
        private Image m_BG;

        private Action<List<Poker>> m_OnPokerClick;

        private List<Poker> m_PokerList;

        private void Awake()
        {
            m_BG.GetComponent<Button>().onClick.AddListener(OnBtnClick);
        }

        private void OnBtnClick()
        {
            if (m_OnPokerClick != null)
            {
                m_OnPokerClick(m_PokerList);
            }
        }

        public void SetUI(List<Poker> lst, Action<List<Poker>> onClick)
        {
            m_PokerList = lst;
            for (int i = 0; i < m_ImagePoker.Length; ++i)
            {
                m_ImagePoker[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < lst.Count; ++i)
            {
                m_ImagePoker[i].gameObject.SetActive(true);
                m_ImagePoker[i].overrideSprite = MahJongManager.Instance.LoadPokerSprite(lst[i], false);
            }

            m_BG.rectTransform.sizeDelta = new Vector2(86 * lst.Count + 100, m_BG.rectTransform.sizeDelta.y);
            m_OnPokerClick = onClick;
        }
    }
}
