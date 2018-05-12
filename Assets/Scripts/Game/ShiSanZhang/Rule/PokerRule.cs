//===================================================
//Author      : DRB
//CreateTime  ：11/30/2017 1:28:07 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShiSanZhang
{
    public class PokerRule
    {

        #region 对牌进行排序的方法
        /// <summary>
        /// 手牌排序
        /// </summary>
        /// <param name="Cards"></param>
        public static void SortCards(List<Poker> Pokers)
        {
            Pokers.Sort(Comparator);
        }
        /// <summary>
        /// 排序方法
        /// </summary>
        /// <param name="x"> 前一张牌 </param>
        /// <param name="y"> 后一张牌 </param>
        /// <returns></returns>
        private static int Comparator(Poker x, Poker y)
        {
            if (x.Size >= y.Size)
                return 1;
            else
                return -1;
        }

        /// <summary>
        /// 手牌排序
        /// </summary>
        /// <param name="Cards"></param>
        public static void SortCards(List<int> Cards)
        {
            Cards.Sort(Comparator);
        }

        /// <summary>
        /// 排序方法
        /// </summary>
        /// <param name="x"> 前一张牌 </param>
        /// <param name="y"> 后一张牌 </param>
        /// <returns></returns>
        private static int Comparator(int x, int y)
        {
            if (x >= y)
                return 1;
            else
                return -1;
        }
        #endregion


        /// <summary>
        /// 判断是否有同花顺
        /// </summary>
        /// <param name="myPokers"></param>
        /// <returns></returns>
        public static bool isTongHuaShun(List<Poker> myPokers)
        {
            bool flag = true;
            if (myPokers!=null)
            {
                int size = myPokers.Count;
                //顺子牌的个数在5 到 12之间
                if (size < 5 || size > 12)
                {
                    return false;
                }
                SortCards(myPokers);
                for (int i = 0; i < size - 1; i++)
                {
                    Poker grade0 = myPokers[i];
                    Poker grade1 = myPokers[i + 1];                 
                    if (grade1.Size-grade0.Size!=1&&grade1.Color!=grade0.Color)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            return flag;
        }          
    }
}
