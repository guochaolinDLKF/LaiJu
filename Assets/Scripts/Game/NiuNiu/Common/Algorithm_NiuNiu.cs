//===================================================
//Author      : WZQ
//CreateTime  ：5/19/2017 5:40:33 PM
//Description ：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using niuniu.proto;

namespace NiuNiu
{

    /// <summary>
    ///   牛牛算法 返回排序后下标集合
    /// </summary>
    public class Algorithm_NiuNiu
    {

        private Algorithm_NiuNiu() { }
        private static Algorithm_NiuNiu instance;

        public static Algorithm_NiuNiu Instance
        {
            get
             {
                if (instance == null)
                {
                    instance = new Algorithm_NiuNiu();
                }
                return instance;
            }

        }

        //3张牌下标组合  （每个List<int> 是3下标组合）
        List<int[]> lst_Combination = new List<int[]>();

        //存储自己手牌 牛的下标


        #region 普通场
        //计算传入的Poker列表 是否有牛 （有牛自动排序）
        /// <summary>
        ///  返回排列下标 是否有牛  
        /// </summary>
        /// <param name="pokerList"></param>
        /// <param name="whetherNiu"></param>
        /// <returns></returns>
        public int[] Calculate(List<NiuNiu.Poker> pokerList, out bool whetherNiu)
        {
            int[] pokerSubscript = new int[pokerList.Count];
            List<int> arrPart = new List<int>();
            int openPokerSubscript = 0;
            for (int i = 0; i < pokerList.Count; i++)
            {
                pokerSubscript[i] = i;
                if (pokerList[i].status == NN_ENUM_POKER_STATUS.POKER_STATUS_UPWARD)
                {
                    openPokerSubscript++;
                    arrPart.Add(i);
                }
            }
            if (openPokerSubscript < 3)
            {
                whetherNiu = false;
                return pokerSubscript;
            }


            //arr  为翻开的poker 下标数组
            int[] arr = arrPart.ToArray();


            lst_Combination.Clear();
            //求组合          arr 是 下标集合
            lst_Combination = MyNiuNiuCombination<int>.GetCombination(arr, 3);




            if (lst_Combination.Count == 0)
            {
                Debug.Log("没有3张牌的组合");
            }


            
            //计算是否有牛  --------------得到排序后数组--------------------

            pokerSubscript = SumNiuNiu(pokerList, pokerSubscript, out whetherNiu);

            return pokerSubscript;


        }



        //计算是否有牛
        private int[] SumNiuNiu(List<NiuNiu.Poker> pokerList, int[] pokerSubscriptC, out bool whetherNiu)
        {
            //if (lst_Combination.Count == 0)
            //{
            //    return false;
            //}

            for (int i = 0; i < lst_Combination.Count; i++)
            {

                int sum = 0;
                for (int j = 0; j < lst_Combination[i].Length; j++)
                {
                    sum += Mathf.Clamp(pokerList[lst_Combination[i][j]].size, 0, 10);
                }
                //sum = pokerList[lst_Combination[i][0]].size+ pokerList[lst_Combination[i][1]].size  ;
                //对10取余 等于0 说明有牛
                if ((sum % 10) == 0)
                {
                    Debug.Log("有牛");
                    Debug.Log("这三张牌大小为：" + pokerList[lst_Combination[i][0]].size + " " + pokerList[lst_Combination[i][1]].size + " " + pokerList[lst_Combination[i][2]].size);

                    whetherNiu = true;
                    //参数 将参数一  以参数二  排列
                    pokerSubscriptC = MySort(pokerSubscriptC, lst_Combination[i]);
                    return pokerSubscriptC;
                }

            }
            whetherNiu = false;
            return pokerSubscriptC;


        }


        private int[] MySort(int[] pokerSubscriptC, int[] subscripts)
        {

            List<int> part = new List<int>();

            for (int i = 0; i < subscripts.Length; i++)
            {
                part.Add(subscripts[i]);
            }

        

            for (int i = subscripts.Length; i < pokerSubscriptC.Length; i++)
            {
                if (!part.Contains(pokerSubscriptC[i]))
                {
                    part.Add(pokerSubscriptC[i]);
                }
                else
                {

                    for (int j = 0; j < i; j++)
                    {
                        if (!part.Contains(pokerSubscriptC[j]))
                        {
                            part.Add(pokerSubscriptC[j]);
                        }

                    }
                }

            }
            Debug.Log("变化后poker序列" + part[0] + part[1] + part[2] + part[3] + part[4] + "--------------------------------------------------------");





            int[] sortResult = part.ToArray();

            return sortResult;

        }
        #endregion

        //----------------新版计算---------------------------------------------------------------------------------------------------
        #region 高级场算法
        public List<NiuNiu.Poker> Calculate(List<NiuNiu.Poker> pokerList, out bool whetherNiu, out int type,Room.SuperModel superModel = Room.SuperModel.PassionRoom)
        {
            whetherNiu = false;
            type = 0;

            
            //一 排序
             NiuNiuSort.Instance.SortList(pokerList);

            //if (superModel == Room.SuperModel.CommonRoom)
            //{
            //    if (pokerList.Count >= 4)
            //    {
            //        //炸弹
            //        if (pokerList[0].size == pokerList[3].size)
            //        {
            //            type = 13;
            //            return pokerList;
            //        }

            //    }

            //    //牛
            //    if (!whetherNiu)
            //    {
            //        CalculateNiu(pokerList, out whetherNiu);
            //        if (whetherNiu) type = 1;
            //    }

            //}

            if (pokerList.Count == 3)
            {
                //牛
                if (!whetherNiu)
                {
                    CalculateNiu(pokerList, out whetherNiu);
                    if (whetherNiu) type = 1;
                }

            }
            else if(pokerList.Count == 4)
            {
                //炸弹
                if (pokerList[0].size == pokerList[3].size)
                {
                    type = 13;
                    return pokerList;
                }

                //牛
                if (!whetherNiu)
                {
                    CalculateNiu(pokerList, out whetherNiu);
                    if (whetherNiu) type = 1;

                }


            }
            else if (pokerList.Count == 5)
            {

                
                bool tonghua = true;//计算同花
                bool shunza = true; //计算顺子

                for (int i = 0; i < pokerList.Count - 1; i++)
                {
                    if (pokerList[i].color != pokerList[i + 1].color ) tonghua = false;
                    if (pokerList[i].size != (pokerList[i + 1].size - 1))
                    {
                        if (pokerList[i].size == 1 && pokerList[i].size == 10) continue;
                        shunza = false;

                    }

                }
                for (int i = 0; i < pokerList.Count; i++)
                {
                    if (pokerList[i].size > 13)
                    {
                        tonghua = false;
                        shunza = false;
                    }
                }


                //同花顺
                if (tonghua && shunza)
                {

                    type = 17;
                    return pokerList;
                }

                //炸弹
                if (pokerList[0].size == pokerList[3].size)
                {
                    type = 13;
                    return pokerList;
                }

                //葫芦
                if ( ((pokerList[0].size == pokerList[2].size) && pokerList[3].size == pokerList[4].size) || ((pokerList[0].size == pokerList[1].size) && pokerList[2].size == pokerList[4].size))
                {
                    type = 16;
                    return pokerList;
                }

                if (tonghua)
                {
                    type = 15;
                    return pokerList;

                }

                if (shunza)
                {
                    type = 14;
                    return pokerList;
                }
                //牛
                if (!whetherNiu)
                {

                     CalculateNiu(pokerList, out whetherNiu);
                    if (whetherNiu) type = 1;
                }
            }
          
      
            //炸弹 = 13,
            //顺子 = 14,
            //同花 = 15,
            //葫芦 = 16,
            //同花顺 = 17,


            return pokerList;


        }


        //-----计算牛-------------------

        public List<Poker> CalculateNiu(List<Poker> pokerList ,out bool whetherNiu)
        {
            whetherNiu = false;

            //求3张组合 
            List<Poker[]> lst_CombinationNew =  MyNiuNiuCombination<Poker>.GetCombination(pokerList.ToArray(), 3);

            if (lst_CombinationNew.Count == 0)
            {
                Debug.Log("没有3张牌的组合");
            }

            for (int i = 0; i < lst_CombinationNew.Count; i++)
            {

                int sum = 0;
                for (int j = 0; j < lst_CombinationNew[i].Length; j++)
                {
                    sum += Mathf.Clamp(lst_CombinationNew[i][j].size, 0, 10);
                }

                //对10取余 等于0 说明有牛
                if ((sum % 10) == 0)
                {
                    Debug.Log("有牛");
                    Debug.Log("这三张牌大小为：" + lst_CombinationNew[i][0].size + " " + lst_CombinationNew[i][1].size + " " + lst_CombinationNew[i][2].size);

                    whetherNiu = true;

                    for (int j = 0; j < pokerList.Count; ++j)
                    {
                        //pokerList[j] 是否在 lst_CombinationNew[i]中
                        for (int k = 0; k < lst_CombinationNew[i].Length; ++k)
                        {
                            if (lst_CombinationNew[i][k].index == pokerList[j].index )
                            {
                                //判断是否前移
                                for (int t = j; t > 0; --t)
                                {
                                    //前一位不在lst_CombinationNew[i]zhong中 那么前移  
                                    bool isZai = false;
                                    for (int y = 0; y < lst_CombinationNew[i].Length; ++y)
                                    {
                                        if (lst_CombinationNew[i][y].index == pokerList[t - 1].index)
                                        {
                                            isZai = true;
                                        }
                                    }

                                    if (!isZai)
                                    {
                                        Poker temp = pokerList[t];
                                        pokerList[t]= pokerList[t-1];
                                        pokerList[t - 1] = temp;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    
                                }


                                break;
                            }
                        }

                    }

                    return pokerList;


                }

            }


            //pokerSubscript = SumNiuNiu(pokerList, pokerSubscript, out whetherNiu);

           
            return pokerList;
        }
        #endregion



    }








    public class MyNiuNiuCombination<T>
    {



        /// <summary>
        /// 求数组中n个元素的组合
        /// </summary>
        /// <param name="t">所求数组</param>
        /// <param name="n">元素个数</param>
        /// <returns>数组中n个元素的组合的范型</returns>
        public static List<T[]> GetCombination(T[] t, int n)
        {
            if (t.Length < n)
            {
                return null;
            }
            int[] temp = new int[n];
            List<T[]> list = new List<T[]>();
            GetCombination(ref list, t, t.Length, n, temp, n);
            return list;
        }

        //参数一是 存放结果列表          ref List<T[]> list
        //参数二是 在这个数组中找值        T[] t
        //参数三是   需要数的长度         int n
        //参数四是                        int m
        //参数五是                          int[] b
        //参数六是 需要找M个数              int M


        private static void GetCombination(ref List<T[]> list, T[] t, int n, int m, int[] b, int M)
        {
            for (int i = n; i >= m; i--)
            {
                b[m - 1] = i - 1;
                if (m > 1)
                {
                    GetCombination(ref list, t, i - 1, m - 1, b, M);
                }
                else
                {
                    if (list == null)
                    {
                        list = new List<T[]>();
                    }
                    T[] temp = new T[M];
                    for (int j = 0; j < b.Length; j++)
                    {
                        temp[j] = t[b[j]];
                    }
                    list.Add(temp);
                }
            }
        }


    }

}

