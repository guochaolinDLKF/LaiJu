//===================================================
//Author      : WZQ
//CreateTime  ：11/13/2017 4:04:19 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaoDeKuai
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

        public const string DefaultName = "0_0";

        #region Public Members
        public int index { get { return m_Index; } private set { m_Index = value; } }

        public int color { get { return m_Color; } private set { m_Color = value; } }

        public int size { get { return m_Size; } private set { m_Size = value; } }

        public int weight { get { return m_Weight; } }


        #endregion

        #region Constructor
        public Poker() { }

        public Poker( int size,int color)
        {
            m_Index = 0;
            m_Color = color;
            m_Size = size;
            m_Weight = 0;
        }

        public Poker(int index, int size, int color)
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
        public void SetPaker(int size, int color)
        {
            m_Color = color;
            m_Size = size;
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
            string stringSize = size.ToString();
            if (size == 14)
                stringSize = "1";
            else if(size==15)
                stringSize = "2";
            else if (size == 16)
                stringSize = "14";
            else if (size == 17)
                stringSize = "15";
            return string.Format("{0}_{1}", stringSize, color.ToString());
        }


        public string ToChinese()
        {
            string sizeString = string.Empty;
            string colorString = string.Empty;

            switch (color)
            {
                case 1:
                    colorString = "方块";
                    break;
                case 2:
                    colorString = "梅花";
                    break;
                case 3:
                    colorString = "红桃";
                    break;
                case 4:
                    colorString = "黑桃";
                    break;
            }

            if (size > 0 && size < 11) sizeString = size.ToString();
            switch (size)
            {
               
                case 11:
                    sizeString = "J";
                    break;
                case 12:
                    sizeString = "Q";
                    break;
                case 13:
                    sizeString = "K";
                    break;
                case 14:
                    sizeString = "A";
                    break;
                case 15:
                    sizeString = "2";
                    break;
                case 16:
                    sizeString = "小王";
                    break;
                case 17:
                    sizeString = "大王";
                    break;
            }
            return  colorString + sizeString;

        }
        #endregion

    }
}