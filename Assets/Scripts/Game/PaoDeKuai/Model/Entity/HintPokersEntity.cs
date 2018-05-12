//===================================================
//Author      : WZQ
//CreateTime  ：11/16/2017 5:12:21 PM
//Description ：提示出牌实体
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaoDeKuai
{
    public class HintPokersEntity
    {
        private CombinationPokersEntity m_others;

        private CombinationPokersEntity m_currHint;

        private HintLevel m_currHintLevel = HintLevel.Integer;

        private HintLevel m_expectHintLevel = HintLevel.Integer;

        /// <summary>
        /// 上家出牌
        /// </summary>
        public CombinationPokersEntity Others { get { return m_others; }  set { m_others = value; } }

        /// <summary>
        /// 最近提示
        /// </summary>
        public CombinationPokersEntity CurrHint { get { return m_currHint; } set { m_currHint = value; } }

        /// <summary>
        /// 当前提示级别
        /// </summary>
        public HintLevel CurrHintLevel { get { return m_currHintLevel; } set { m_currHintLevel = value; } }


        /// <summary>
        /// 期望提示级别
        /// </summary>
        public HintLevel ExpectHintLevel { get { return m_expectHintLevel; } set { m_expectHintLevel = value; } }

        public HintPokersEntity(CombinationPokersEntity others)
        {
            m_others = others;
            m_currHint = new CombinationPokersEntity ();
        }


        public void Reset()
        {
            m_currHint.Reset();
            m_currHintLevel = HintLevel.Integer;
            m_expectHintLevel = HintLevel.Integer;
        }



        public void SetCurrHint(CombinationPokersEntity hintEntity,HintLevel hintLevel)
        {
            m_currHint = hintEntity;
            m_currHintLevel = hintLevel;

        }

    }
}