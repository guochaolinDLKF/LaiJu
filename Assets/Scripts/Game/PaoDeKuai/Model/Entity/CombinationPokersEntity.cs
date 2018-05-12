//===================================================
//Author      : WZQ
//CreateTime  ：11/16/2017 5:11:47 PM
//Description ：组合牌实体
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaoDeKuai
{
    public class CombinationPokersEntity
    {
        private int m_Pos=0;

        private List<Poker> m_pokers;

        private PokersType m_pokersType = PokersType.None;

        private int m_currSize;

        /// <summary>
        /// 牌的所属座位
        /// </summary>
        public int Pos { get { return m_Pos; } }

        /// <summary>
        /// 组合牌
        /// </summary>
        public List<Poker> Pokers { get { return m_pokers; } }

        /// <summary>
        /// 组合牌型
        /// </summary>
        public PokersType PokersType { get { return m_pokersType; } set { m_pokersType = value; } }

        /// <summary>
        /// 当前牌大小（相对同牌型大小）
        /// </summary>
        public int CurrSize { get { return m_currSize; } set { m_currSize = value; } }


        public CombinationPokersEntity()
        {
            this.m_Pos = 0;
            this.m_pokers = new List<Poker> ();
            this.m_pokersType = PokersType.None;
            m_currSize = 0;
        }
        public CombinationPokersEntity(int pos,List<Poker> pokers, PokersType pokersType, int size)
        {
            this.m_Pos = pos;
            this.m_pokers = pokers;
            this.m_pokersType = pokersType;
            m_currSize = size;
            if(pokers!=null&&pokers.Count>0) PaoDeKuaiHelper.Sort(pokers);

        }

        public void Reset()
        {
            m_Pos = 0;
            Pokers.Clear();
            m_pokersType = PokersType.None;
            m_currSize = 0;
        }


        public void  SetInfo(List<Poker> pokers, PokersType pokersType, int size)
        {
            this.m_pokers = pokers;
            this.m_pokersType = pokersType;
            m_currSize = size;

        }


    }
}