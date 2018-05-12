//===================================================
//Author      : DRB
//CreateTime  ：4/6/2017 3:57:09 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DRB.MahJong
{
    public class PengCtrl : MonoBehaviour
    {
        public Combination3D Combination;
        [SerializeField]
        private Transform[] m_Containers;

        private Dictionary<MaJiangCtrl, int> m_XiCount = new Dictionary<MaJiangCtrl, int>();

        [HideInInspector]
        public int PokerCount;

        /// <summary>
        /// 动画起始点
        /// </summary>
        [SerializeField]
        private Transform[] m_AnimationSrc;

        public void SetUI(Combination3D combination,int seatPos)
        {
            if (combination.OperatorType == OperatorType.BuXi)
            {
                BuXi(combination.PokerList[0]);
                return;
            }
            Combination = combination;
            for (int i = 0; i < combination.PokerList.Count; ++i)
            {
                if (Combination.OperatorType == OperatorType.LiangXi && i == 3)
                {
                    combination.PokerList[i].gameObject.SetParent(m_Containers[4]);
                    combination.PokerList[i].transform.position = m_AnimationSrc[4].position;
                    combination.PokerList[i].transform.rotation = m_AnimationSrc[4].rotation;
                }
                else
                {
                    combination.PokerList[i].gameObject.SetParent(m_Containers[i]);
                    combination.PokerList[i].transform.position = m_AnimationSrc[i].position;
                    combination.PokerList[i].transform.rotation = m_AnimationSrc[i].rotation;
                }
                if (combination.OperatorType == OperatorType.Gang && combination.SubTypeId == 4 && i < 3)
                {
                    combination.PokerList[i].transform.localEulerAngles = new Vector3(180f, 0f, 0f);
                }
                if (combination.OperatorType == OperatorType.Kou)
                {
                    combination.PokerList[i].transform.localEulerAngles = new Vector3(180f, 0f, 0f);
                }
                if (Combination.OperatorType == OperatorType.LiangXi && i == 3)
                {
                    combination.PokerList[i].transform.DOMove(m_Containers[4].position, 0.3f);
                }
                else
                {
                    combination.PokerList[i].transform.DOMove(m_Containers[i].position, 0.3f);
                }
            
            }
        }

        private void BuXi(MaJiangCtrl majiang)
        {
            if (Combination == null) return;
            majiang.gameObject.SetLayer("Table");
            for (int i = 0; i < Combination.PokerList.Count; ++i)
            {
                if (majiang.Poker.color == Combination.PokerList[i].Poker.color && majiang.Poker.size == Combination.PokerList[i].Poker.size)
                {
                    if (!m_XiCount.ContainsKey(Combination.PokerList[i]))
                    {
                        m_XiCount[Combination.PokerList[i]] = 0;
                    }
                    ++m_XiCount[Combination.PokerList[i]];
                    if (i == 3)
                    {
                        majiang.gameObject.SetParent(m_Containers[4]);
                        majiang.transform.position = m_Containers[4].position + new Vector3(0, m_XiCount[Combination.PokerList[i]] * 4.5f, 0);
                    }
                    else
                    {
                        majiang.gameObject.SetParent(m_Containers[i]);
                        majiang.transform.position = m_Containers[i].position + new Vector3(0, m_XiCount[Combination.PokerList[i]] * 4.5f, 0);
                    }
                    return;
                }
            }
        }

        #region OpenDoor 开门
        /// <summary>
        /// 开门
        /// </summary>
        public void OpenDoor()
        {
            if (Combination == null) return;
            Debug.Log("开门!!!!");
            if (Combination.OperatorType == OperatorType.Gang && (SubOperateType)Combination.SubTypeId == SubOperateType.AnGang)
            {
                Combination.PokerList[3].transform.localEulerAngles = Vector3.zero;
            }
        }
        #endregion

        public void Reset()
        {
            Combination = null;
            m_XiCount.Clear();
            PokerCount = 0;
        }
    }
}
