//===================================================
//Author      : DRB
//CreateTime  ：9/1/2017 1:46:47 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuPaiJiu;

namespace GuPaiJiu
{
    public class LookupPokerType : MonoBehaviour
    {
        public  static LookupPokerType Instance;
        private List<int> pokerList1 = new List<int>();//牌型一集合
        private List<int> pokerList2 = new List<int>();//牌型二集合
        public  RoomEntity room;

        Dictionary<string, PokerType> pokerTypeDic = new Dictionary<string, PokerType>();

        void Awake()
        {
            Instance = this;
            AddDicPokerType();
        }
      
        /// <summary>
        /// 查找牌的类型
        /// </summary>
        /// <param name="list"></param>
        /// <param name="pokerType1"></param>
        /// <param name="pokerType2"></param>
        public void GetDicPokerType(List<int> list,out PokerType pokerType1,out PokerType pokerType2)
        {            
            if (pokerList1.Count != 0) pokerList1.Clear();
            if (pokerList2.Count != 0) pokerList2.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                if (i<2)
                {
                    pokerList1.Add(list[i]);
                }
                else
                {
                    pokerList2.Add(list[i]);
                }
            }
            pokerType1 = GetDicPokerType(pokerList1);
            pokerType2= GetDicPokerType(pokerList2);
        }
       
        /// <summary>
        /// 查找牌型的实现方式
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public PokerType GetDicPokerType(List<int> list)
        {
            if (list.Count != 0)
            {
                list.Sort((int pokerType1, int pokerType2) =>
                {
                    if (pokerType1 > pokerType2)
                        return -1;
                    else return 1;
                });
                string pokerTypeName = list[0] + list[1].ToString();
                if (room.enumGuiZi==EnumGuiZi.NO)
                {                   
                    if (pokerTypeName=="138"|| pokerTypeName == "128")
                    {
                        pokerTypeName = "0000";
                    }
                }
                if (room.enumTianJiuWang==EnumTianJiuWang.NO)
                {
                    if (pokerTypeName == "131" || pokerTypeName == "121")
                    {
                        pokerTypeName = "000000";
                    }
                }
                if (room.enumDiJiuWang==EnumDiJiuWang.DiJiuWang)
                {
                    if (pokerTypeName == "132" || pokerTypeName == "122")
                    {
                        pokerTypeName = "00000";
                    }
                }       
                if (pokerTypeDic.ContainsKey(pokerTypeName))
                {
                    return pokerTypeDic[pokerTypeName];
                }
                else
                {
                    return PokerType.Unknown;
                }
            }
            return PokerType.Kong;
        }

        /// <summary>
        /// 添加牌型
        /// </summary>
        private void AddDicPokerType()
        {            
            pokerTypeDic.Add("138", PokerType.Devil);
            pokerTypeDic.Add("128", PokerType.Devil);//鬼子
                  
            pokerTypeDic.Add("2120", PokerType.Emperor);//皇上
            pokerTypeDic.Add("131", PokerType.DayNineKing);
            pokerTypeDic.Add("121", PokerType.DayNineKing);//天九王

          

            pokerTypeDic.Add("11", PokerType.SubDay);//对天
            pokerTypeDic.Add("22", PokerType.SubLand);//对地
            pokerTypeDic.Add("33", PokerType.SubRedPerson);//对红人
            pokerTypeDic.Add("44", PokerType.SubGoose);//对鹅
            pokerTypeDic.Add("55", PokerType.SubLongFive);//对长五
            pokerTypeDic.Add("66", PokerType.SubLongSix);//对长六
            pokerTypeDic.Add("77", PokerType.SubLongFour);//对长四
            pokerTypeDic.Add("88", PokerType.SubTiger);//对虎头
            pokerTypeDic.Add("99", PokerType.SubJinping);//对金平
            pokerTypeDic.Add("1010", PokerType.SubShortSeven);//对短七
            pokerTypeDic.Add("1111", PokerType.SubShortSix);//对短六
            pokerTypeDic.Add("1312", PokerType.SubMixedNine);//对杂九
            pokerTypeDic.Add("1514", PokerType.SubMixedEight);//对杂八
            pokerTypeDic.Add("1716", PokerType.SubMixedSeven);//对杂七
            pokerTypeDic.Add("1918", PokerType.SubMixedFive);//对杂五
            pokerTypeDic.Add("31", PokerType.DayBar);
            pokerTypeDic.Add("141", PokerType.DayBar);
            pokerTypeDic.Add("151", PokerType.DayBar);//天杠

            pokerTypeDic.Add("32", PokerType.LandBar);
            pokerTypeDic.Add("142", PokerType.LandBar);
            pokerTypeDic.Add("152", PokerType.LandBar);//地杠

            pokerTypeDic.Add("101", PokerType.DaySevenNine);
            pokerTypeDic.Add("161", PokerType.DaySevenNine);
            pokerTypeDic.Add("171", PokerType.DaySevenNine);//天七九

            pokerTypeDic.Add("61", PokerType.DayEight);
            pokerTypeDic.Add("111", PokerType.DayEight);
            pokerTypeDic.Add("201", PokerType.DayEight);//天八

            pokerTypeDic.Add("181", PokerType.DaySeven);
            pokerTypeDic.Add("191", PokerType.DaySeven);//天七

            pokerTypeDic.Add("71", PokerType.DaySix);
            pokerTypeDic.Add("41", PokerType.DaySix);//天六

            pokerTypeDic.Add("211", PokerType.DayFive);//天五

            pokerTypeDic.Add("21", PokerType.DayFour);//天四

            pokerTypeDic.Add("81", PokerType.DayThree);//天三

            pokerTypeDic.Add("51", PokerType.DayTwo);//天二
            pokerTypeDic.Add("91", PokerType.DayTwo);//天二

            pokerTypeDic.Add("102", PokerType.LandNine);//地九
            pokerTypeDic.Add("172", PokerType.LandNine);//地九
            pokerTypeDic.Add("162", PokerType.LandNine);//地九

            pokerTypeDic.Add("202", PokerType.LandEight);//地八
            pokerTypeDic.Add("62", PokerType.LandEight);//地八
            pokerTypeDic.Add("112", PokerType.LandEight);//地八

            pokerTypeDic.Add("192", PokerType.LandSeven);//地七
            pokerTypeDic.Add("182", PokerType.LandSeven);//地七

            pokerTypeDic.Add("42", PokerType.LandSix);//地六
            pokerTypeDic.Add("72", PokerType.LandSix);//地六

            pokerTypeDic.Add("212", PokerType.LandFive);//地五

            pokerTypeDic.Add("82", PokerType.LandThree);//地三

            pokerTypeDic.Add("92", PokerType.LandTwo);//地二
            pokerTypeDic.Add("52", PokerType.LandTwo);//地二

            pokerTypeDic.Add("122", PokerType.LandOne);//地一
            pokerTypeDic.Add("132", PokerType.LandOne);//地一

            pokerTypeDic.Add("83", PokerType.PersonNine);//人九

            pokerTypeDic.Add("53", PokerType.PersonEight);//人八
            pokerTypeDic.Add("93", PokerType.PersonEight);//人八

            pokerTypeDic.Add("123", PokerType.PersonSeven);//人七
            pokerTypeDic.Add("133", PokerType.PersonSeven);//人七

            pokerTypeDic.Add("143", PokerType.PersonSix);
            pokerTypeDic.Add("153", PokerType.PersonSix);//人六

            pokerTypeDic.Add("103", PokerType.PersonFive);
            pokerTypeDic.Add("163", PokerType.PersonFive);
            pokerTypeDic.Add("173", PokerType.PersonFive);//人五

            pokerTypeDic.Add("63", PokerType.PersonFour);
            pokerTypeDic.Add("113", PokerType.PersonFour);
            pokerTypeDic.Add("203", PokerType.PersonFour);//人四

            pokerTypeDic.Add("183", PokerType.PersonThree);
            pokerTypeDic.Add("193", PokerType.PersonThree);//人三

            pokerTypeDic.Add("73", PokerType.PersonTwo);//人二
            pokerTypeDic.Add("43", PokerType.PersonTwo);//人二

            pokerTypeDic.Add("213", PokerType.PersonOne);//人一

            pokerTypeDic.Add("184", PokerType.GooseNine);//鹅九
            pokerTypeDic.Add("194", PokerType.GooseNine);//鹅九

            pokerTypeDic.Add("74", PokerType.GooseEight);//鹅八

            pokerTypeDic.Add("214", PokerType.GooseSeven);//鹅七

            pokerTypeDic.Add("84", PokerType.GooseFive);//鹅五

            pokerTypeDic.Add("94", PokerType.GooseFour);//鹅四
            pokerTypeDic.Add("54", PokerType.GooseFour);//鹅四

            pokerTypeDic.Add("124", PokerType.GooseThree);//鹅三
            pokerTypeDic.Add("134", PokerType.GooseThree);//鹅三

            pokerTypeDic.Add("144", PokerType.GooseTwo);//鹅二
            pokerTypeDic.Add("154", PokerType.GooseTwo);//鹅二

            pokerTypeDic.Add("104", PokerType.GooseOne);//鹅一
            pokerTypeDic.Add("174", PokerType.GooseOne);//鹅一
            pokerTypeDic.Add("164", PokerType.GooseOne);//鹅一

            pokerTypeDic.Add("125", PokerType.LongNine);//长九
            pokerTypeDic.Add("135", PokerType.LongNine);//长九
            pokerTypeDic.Add("187", PokerType.LongNine);//长九
            pokerTypeDic.Add("216", PokerType.LongNine);//长九
            pokerTypeDic.Add("197", PokerType.LongNine);//长九

            pokerTypeDic.Add("145", PokerType.LongEight);//长八
            pokerTypeDic.Add("155", PokerType.LongEight);//长八


            pokerTypeDic.Add("86", PokerType.LongSeven);
            pokerTypeDic.Add("105", PokerType.LongSeven);
            pokerTypeDic.Add("165", PokerType.LongSeven);
            pokerTypeDic.Add("175", PokerType.LongSeven);//长七
            pokerTypeDic.Add("217", PokerType.LongSeven);//长七

            pokerTypeDic.Add("65", PokerType.LongSix);
            pokerTypeDic.Add("96", PokerType.LongSix);
            pokerTypeDic.Add("115", PokerType.LongSix);
            pokerTypeDic.Add("205", PokerType.LongSix);//长六

            pokerTypeDic.Add("126", PokerType.LongFive);
            pokerTypeDic.Add("136", PokerType.LongFive);
            pokerTypeDic.Add("185", PokerType.LongFive);
            pokerTypeDic.Add("195", PokerType.LongFive);
            pokerTypeDic.Add("87", PokerType.LongFive);//长五

            pokerTypeDic.Add("75", PokerType.LongFour);
            pokerTypeDic.Add("146", PokerType.LongFour);
            pokerTypeDic.Add("156", PokerType.LongFour);
            pokerTypeDic.Add("97", PokerType.LongFour);//长四

            pokerTypeDic.Add("215", PokerType.LongThree);
            pokerTypeDic.Add("166", PokerType.LongThree);
            pokerTypeDic.Add("106", PokerType.LongThree);
            pokerTypeDic.Add("127", PokerType.LongThree);//长三
            pokerTypeDic.Add("137", PokerType.LongThree);//长三
            pokerTypeDic.Add("176", PokerType.LongThree);//长三


            pokerTypeDic.Add("116", PokerType.LongTwo);
            pokerTypeDic.Add("206", PokerType.LongTwo);
            pokerTypeDic.Add("157", PokerType.LongTwo);
            pokerTypeDic.Add("147", PokerType.LongTwo);//长二


            pokerTypeDic.Add("85", PokerType.LongOne);//长一
            pokerTypeDic.Add("196", PokerType.LongOne);//长一
            pokerTypeDic.Add("186", PokerType.LongOne);//长一
            pokerTypeDic.Add("107", PokerType.LongOne);//长一
            pokerTypeDic.Add("177", PokerType.LongOne);//长一
            pokerTypeDic.Add("167", PokerType.LongOne);//长一

            pokerTypeDic.Add("129", PokerType.ShortNine);//短九
            pokerTypeDic.Add("139", PokerType.ShortNine);//短九
            pokerTypeDic.Add("158", PokerType.ShortNine);//短九
            pokerTypeDic.Add("148", PokerType.ShortNine);//短九
            pokerTypeDic.Add("2111", PokerType.ShortNine);//短九


            pokerTypeDic.Add("159", PokerType.ShortEight);//短八
            pokerTypeDic.Add("149", PokerType.ShortEight);//短八
            pokerTypeDic.Add("178", PokerType.ShortEight);//短八
            pokerTypeDic.Add("168", PokerType.ShortEight);//短八
            pokerTypeDic.Add("108", PokerType.ShortEight);//短八

            pokerTypeDic.Add("169", PokerType.ShortSeven);
            pokerTypeDic.Add("179", PokerType.ShortSeven);
            pokerTypeDic.Add("208", PokerType.ShortSeven);
            pokerTypeDic.Add("118", PokerType.ShortSeven);
            pokerTypeDic.Add("109", PokerType.ShortSeven);//短七

            pokerTypeDic.Add("119", PokerType.ShortSix);//短六
            pokerTypeDic.Add("209", PokerType.ShortSix);//短六
            pokerTypeDic.Add("198", PokerType.ShortSix);//短六
            pokerTypeDic.Add("188", PokerType.ShortSix);//短六
            pokerTypeDic.Add("1210", PokerType.ShortSix);//短六
            pokerTypeDic.Add("1310", PokerType.ShortSix);//短六

            pokerTypeDic.Add("199", PokerType.ShortFive);//短五
            pokerTypeDic.Add("189", PokerType.ShortFive);//短五
            pokerTypeDic.Add("1211", PokerType.ShortFive);//短五
            pokerTypeDic.Add("1311", PokerType.ShortFive);//短五
            pokerTypeDic.Add("1410", PokerType.ShortFive);//短五
            pokerTypeDic.Add("1510", PokerType.ShortFive);//短五

            pokerTypeDic.Add("218", PokerType.ShortFour);//短四
            pokerTypeDic.Add("1610", PokerType.ShortFour);//短四
            pokerTypeDic.Add("1710", PokerType.ShortFour);//短四
            pokerTypeDic.Add("1411", PokerType.ShortFour);//短四
            pokerTypeDic.Add("1511", PokerType.ShortFour);//短四

            pokerTypeDic.Add("1110", PokerType.ShortThree);//短三
            pokerTypeDic.Add("219", PokerType.ShortThree);//短三
            pokerTypeDic.Add("2010", PokerType.ShortThree);//短三
            pokerTypeDic.Add("1611", PokerType.ShortThree);//短三
            pokerTypeDic.Add("1711", PokerType.ShortThree);//短三

            pokerTypeDic.Add("2011", PokerType.ShortTwo);//短二
            pokerTypeDic.Add("1810", PokerType.ShortTwo);//短二
            pokerTypeDic.Add("1910", PokerType.ShortTwo);//短二

            pokerTypeDic.Add("1811", PokerType.ShortOne);//短一
            pokerTypeDic.Add("1911", PokerType.ShortOne);//短一
            pokerTypeDic.Add("98", PokerType.ShortOne);//短一

            pokerTypeDic.Add("2118", PokerType.MixedEight);
            pokerTypeDic.Add("2119", PokerType.MixedEight);//杂八

            pokerTypeDic.Add("1412", PokerType.MixedSeven);
            pokerTypeDic.Add("1513", PokerType.MixedSeven);//杂七
            pokerTypeDic.Add("1413", PokerType.MixedSeven);//杂七
            pokerTypeDic.Add("1512", PokerType.MixedSeven);//杂七


            pokerTypeDic.Add("1612", PokerType.MixedSix);
            pokerTypeDic.Add("1613", PokerType.MixedSix);//杂六
            pokerTypeDic.Add("1713", PokerType.MixedSix);//杂六
            pokerTypeDic.Add("1712", PokerType.MixedSix);//杂六

            pokerTypeDic.Add("1714", PokerType.MixedFive);
            pokerTypeDic.Add("1615", PokerType.MixedFive);
            pokerTypeDic.Add("1614", PokerType.MixedFive);//杂五
            pokerTypeDic.Add("2013", PokerType.MixedFive);
            pokerTypeDic.Add("2012", PokerType.MixedFive);//杂五
            pokerTypeDic.Add("1715", PokerType.MixedFive);//杂五

            pokerTypeDic.Add("2015", PokerType.MixedFour);
            pokerTypeDic.Add("1812", PokerType.MixedFour);
            pokerTypeDic.Add("1912", PokerType.MixedFour);            
            pokerTypeDic.Add("1813", PokerType.MixedFour);
            pokerTypeDic.Add("1913", PokerType.MixedFour);
            pokerTypeDic.Add("2014", PokerType.MixedFour);//杂四

            pokerTypeDic.Add("2016", PokerType.MixedThree);
            pokerTypeDic.Add("1915", PokerType.MixedThree);
            pokerTypeDic.Add("1814", PokerType.MixedThree);
            pokerTypeDic.Add("1815", PokerType.MixedThree);
            pokerTypeDic.Add("1914", PokerType.MixedThree);//杂三
            pokerTypeDic.Add("2017", PokerType.MixedThree);//杂三


            pokerTypeDic.Add("1817", PokerType.MixedTwo);
            pokerTypeDic.Add("1816", PokerType.MixedTwo);//杂二
            pokerTypeDic.Add("1916", PokerType.MixedTwo);
            pokerTypeDic.Add("1917", PokerType.MixedTwo);//杂二
            pokerTypeDic.Add("2113", PokerType.MixedTwo);           
            pokerTypeDic.Add("2112", PokerType.MixedTwo);//杂二
           

            pokerTypeDic.Add("2018", PokerType.MixedOne);
            pokerTypeDic.Add("2114", PokerType.MixedOne);
            pokerTypeDic.Add("2115", PokerType.MixedOne);
            pokerTypeDic.Add("2019", PokerType.MixedOne);//杂一

            pokerTypeDic.Add("95", PokerType.CloseTen);
            pokerTypeDic.Add("2116", PokerType.CloseTen);
            pokerTypeDic.Add("64", PokerType.CloseTen);
            pokerTypeDic.Add("117", PokerType.CloseTen);
            pokerTypeDic.Add("204", PokerType.CloseTen);
            pokerTypeDic.Add("76", PokerType.CloseTen);
            pokerTypeDic.Add("207", PokerType.CloseTen);           
            pokerTypeDic.Add("2110", PokerType.CloseTen);
            pokerTypeDic.Add("114", PokerType.CloseTen);
            pokerTypeDic.Add("2117", PokerType.CloseTen);//闭十

            pokerTypeDic.Add("0000", PokerType.CloseTen);//不选择鬼子的时候，鬼子的牌型为 闭十
            pokerTypeDic.Add("00000", PokerType.LandNineKing);//地九王
            pokerTypeDic.Add("000000", PokerType.DayOne);//天一

            pokerTypeDic.Add("00",PokerType.Kong);

        }
    }  

}
