//===================================================
//Author      : WZQ
//CreateTime  ：11/6/2017 9:55:36 AM
//Description ：掼蛋扑克牌实体
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuanDan
{
    public class Poker 
    {
        [SerializeField]
        private int m_Index;
        [SerializeField]
        private int m_Color;
        [SerializeField]
        private int m_Size;
        [SerializeField]
        private int m_Weight;
        [SerializeField]
        private bool m_IsUniversal;

        #region Public Members
        public int index { get { return m_Index; } private set { m_Index = value; } }

        public int color { get { return m_Color; } private set { m_Color = value; } }

        public int size { get { return m_Size; } private set { m_Size = value; } }

        public int weight { get { return m_Weight; }  }

        public bool isUniversal { get { return m_IsUniversal; } set { m_IsUniversal = value; } }

        #endregion

        #region Constructor
        public Poker() { }

        public Poker(int color, int size)
        {
            m_Index = 0;
            m_Color = color;
            m_Size = size;
            m_Weight = 0;
        }

        public Poker(int index, int color, int size)
        {
            m_Index = index;
            m_Color = color;
            m_Size = size;
            m_Weight = 0;
        }

        

        public Poker(Poker poker)
        {
            if (poker != null)
            {
                m_Index = poker.index;
                m_Color = poker.color;
                m_Size = poker.size;
                m_Weight = poker.weight;
            }
        }
        #endregion


        public void SetPaker(Poker poker)
        {
            if (poker != null)
            {
                m_Index = poker.index;
                m_Color = poker.color;
                m_Size = poker.size;
                m_Weight = poker.weight;
            }
        }




        /// <summary>
        /// 计算单张牌权值
        /// </summary>
        /// <returns></returns>
        private int SetWeight()
        {

            //m_Weight = siz
            return 0;
        }

        #region ToString
        public override string ToString()
        {
            return string.Format("{0}_{1}", color.ToString(), size.ToString());
        }
        #endregion




    }
}