//===================================================
//Author      : WZQ
//CreateTime  ：9/5/2017 11:26:54 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuYou
{
    public class ChipSnumberList
    {
        public List<ChipSnumber<int, int>> m_ChipSnumberList;
        public ChipSnumberList()
        {
            m_ChipSnumberList = new List<ChipSnumber<int, int>>();

            //m_ChipSnumberList.Add(new ChipSnumber<int, int>(10, 0));
            //m_ChipSnumberList.Add(new ChipSnumber<int, int>(20, 0));
            //m_ChipSnumberList.Add(new ChipSnumber<int, int>(30, 0));
        }

        /// <summary>
        /// 添加键值对
        /// </summary>
        public void AddPair(ChipSnumber<int, int> chipSnumber)
        {
            for (int i = 0; i < m_ChipSnumberList.Count; i++)
            {
                if (m_ChipSnumberList[i].Key == chipSnumber.Key)
                {
                    m_ChipSnumberList[i] = chipSnumber;
                    return;

                }
            }

            //具体实现？？？

            m_ChipSnumberList.Add(chipSnumber);
            //Insert
        }

        /// <summary>
        /// 移除键值对
        /// </summary>
        public void RemovePair()
        {

        }

        /// <summary>
        /// 重置Value
        /// </summary>
        public void InitValue()
        {
            for (int i = 0; i < m_ChipSnumberList.Count; i++)
            {
                m_ChipSnumberList[i].Value = 0;
            }

        }
    }


    public class ChipSnumber<T,K> where T : struct  where K : struct
    {
       public ChipSnumber(T key,K value)
        {
            this.key = key;
            this.value = value;
        }


        private T key;
        public T Key{ get{ return key;}}

        private K value;
        public K Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }

        
    }
}