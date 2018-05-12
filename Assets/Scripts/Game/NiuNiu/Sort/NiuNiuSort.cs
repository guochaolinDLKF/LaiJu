//===================================================
//Author      : WZQ
//CreateTime  ：5/23/2017 5:18:56 PM
//Description ：牛牛 Seat排序 
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NiuNiu
{
    public class NiuNiuSort
    {

        //这个脚本存放列表 2 控制列表内容排序


        private NiuNiuSort()
        {
        }

        private static NiuNiuSort instance;

        public static NiuNiuSort Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NiuNiuSort();
                }
                return instance;

            }
        }

        private List<NiuNiu.Seat> sortList=new List<Seat> ();




        List<NiuNiu.Seat> AddItemList(List<NiuNiu.Seat> waitingSortList)
        {
            sortList.Clear();

            for (int i = 0; i < waitingSortList.Count; i++)
            {
                if (waitingSortList[i].PlayerId > 0)
                {
                    sortList.Add(waitingSortList[i]);

                }

            }
            return sortList;


        }





        //-----自定义排序规则---------------------------------------
        private int nameSort(NiuNiu.Seat info1, NiuNiu.Seat info2)
        {
            return string.Compare(info1.Nickname, info2.Nickname);
        }
        private int scoreSort(NiuNiu.Seat info1, NiuNiu.Seat info2)
        {
            return info1.Gold <= info2.Gold ? 1 : -1;
        }
        private int pokerSort(NiuNiu.Poker info1, NiuNiu.Poker info2)
        {
            int value1 = info1.size * 10 + info1.color;
            int value2 = info2.size * 10 + info2.color;
            return value1 >= value2 ? 1 : -1;
        }



        // 给玩家列表排序的方法    需要参数为自定义的匹配规则
        public List<NiuNiu.Seat> SortList(List<NiuNiu.Seat> waitingSortList, int sRule)
        {
            sortList.Clear();
            if (waitingSortList.Count == 0)
            {
                return sortList;
            }


            AddItemList(waitingSortList);

            if (sortList.Count == 0)
            {
                return sortList;
            }


            NiuNiu.sortRule rule = (NiuNiu.sortRule)sRule;

            switch (rule)
            {
                case sortRule.name:

                    sortList.Sort(nameSort);
                    break;
                case sortRule.score:
                    sortList.Sort(scoreSort);
                    break;
                default:
                    break;
            }

            return sortList;

        }

        /// <summary>
        /// 手牌排序
        /// </summary>
        /// <param name="pokerList"></param>
        /// <returns></returns>
        public List<Poker> SortList(List<Poker> pokerList)
        {
            pokerList.Sort(pokerSort);
            return pokerList;
        }




    }

}


