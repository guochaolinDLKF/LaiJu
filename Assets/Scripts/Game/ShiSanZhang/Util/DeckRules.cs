//===================================================
//Author      : DRB
//CreateTime  ：12/2/2017 3:22:12 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShiSanZhang
{

    public enum EnumSortPoker
    {
        BIG_SMALL,//按照大小排序
        POKER_COLOR,//按照花色排序
    }
    public enum LevelType
    {
        TOU_DAO,//头道
        ZHONG_DAO,//中道
        WEI_DAO,//尾道
    }


    /// <summary>
    /// 牌组类型
    /// </summary>
    public enum DeckType
    {
        WU_LONG = 0,//乌龙
        DUI_ZI = 1,//对子
        LIANG_DUI = 2,//两对
        SAN_TIAO = 3,//三条
        SHUN_ZI = 4,//顺子
        TONG_HUA = 5,//同花
        HU_LU = 6,//葫芦
        TIE_ZHI = 7,//铁支
        TONG_HUA_SHUN = 8,//同花顺
    }


    public class Deck : IComparable<Deck>
    {
        public DeckType deckType;
        public List<Poker> pokers;

        public Poker mainPoker;

        public Deck(DeckType deckType, List<Poker> pokers, Poker mainPoker)
        {
            this.deckType = deckType;
            this.pokers = pokers;
            this.mainPoker = mainPoker;
        }

        //public Deck(List<Poker> pokers, Poker mainPoker)
        //{
        //    this.pokers = pokers;
        //    this.mainPoker = mainPoker;
        //}


        //public void SetDeckInfo(List<Poker> pokers, Poker mainPoker)
        //{
        //    this.pokers = pokers;
        //    this.mainPoker = mainPoker;
        //}

        public int CompareTo(Deck other)
        {
            if (other == null) return -1;
            return this.mainPoker.Size - other.mainPoker.Size;
        }
    }


    public class DeckRules {

        public static bool AskIsPlayPoker(List<Poker> myPokers, List<Poker> myPokers1)
        {
            Deck deck = Check(myPokers);
            Deck deck1 = Check(myPokers1);
            Debug.Log((int)deck.deckType+"   牌型是什么   "+(int)deck1.deckType);
            if ((int)deck.deckType > (int)deck1.deckType)
            {
                return false;
            }
            else if ((int)deck.deckType < (int)deck1.deckType)
            {
                return true;
            }
            else if ((int)deck.deckType == (int)deck1.deckType)
            {
                if (deck1.mainPoker.Size > deck.mainPoker.Size)
                {
                    return true;
                }
                else if (deck1.mainPoker.Size < deck.mainPoker.Size)
                {
                    return false;
                }
                else if (deck1.mainPoker.Size == deck.mainPoker.Size)
                {
                    if (deck1.mainPoker.Color > deck.mainPoker.Color)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }




        #region 检测牌型
        public static Deck Check(List<Poker> myPokers)
        {
            if (myPokers == null) return null;

            Deck ret = null;

            ret = CheckTongHuaShun(myPokers);
            if (ret != null) return ret;
            ret = CheckTieZhi(myPokers);
            if (ret != null) return ret;
            ret = CheckHuLu(myPokers);
            if (ret != null) return ret;
            ret = CheckTongHua(myPokers);
            if (ret != null) return ret;
            ret = CheckShunZi(myPokers);
            if (ret != null) return ret;
            ret = CheckSanTiao(myPokers);
            if (ret != null) return ret;
            ret = CheckLiangDui(myPokers);
            if (ret != null) return ret;
            ret = CheckDuiZi(myPokers);
            if (ret != null) return ret;
            ret = CheckWuLong(myPokers);
            if (ret != null) return ret;

            return ret;
        }
        #endregion

        //------------------------------------------------检测牌型-----------------------------------------------------------

        #region 检测乌龙
        public static Deck CheckWuLong(List<Poker> myPoker)
        {
            if (myPoker == null) return null;
            if (myPoker.Count < 2) return null;
            SortCards(myPoker);

            return new Deck(DeckType.WU_LONG, myPoker, myPoker[myPoker.Count - 1]);
        }
        #endregion

        #region 检测对子
        public static Deck CheckDuiZi(List<Poker> myPokers)
        {
            if (myPokers == null) return null;
            List<Deck> aaDeck = GetAllDuiZi(myPokers);
            if (aaDeck == null) return null;
            if (aaDeck.Count == 1)
            {
                return aaDeck[0];
            }
            return null;
        }
        #endregion

        #region 检测两对
        public static Deck CheckLiangDui(List<Poker> myPokers)
        {
            if (myPokers == null) return null;
            if (myPokers.Count < 4) return null;
            List<Deck> aaDeck = GetAllLiangDui(myPokers);
            if (aaDeck == null) return null;
            if (aaDeck.Count == 1)
            {
                SortCards(aaDeck[0].pokers);
                return new Deck(DeckType.LIANG_DUI, aaDeck[0].pokers, aaDeck[0].pokers[3]);
            }
            return null;
        }
        #endregion

        #region 检测三条
        public static Deck CheckSanTiao(List<Poker> myPokers)
        {
            if (myPokers == null) return null;
            if (myPokers.Count < 3) return null;
            List<Deck> aaaDeck = GetAllSanTiao(myPokers);
            if (aaaDeck == null) return null;
            if (aaaDeck.Count == 1)
            {
                return aaaDeck[0];
            }
            return null;
        }
        #endregion

        #region 检测顺子
        public static Deck CheckShunZi(List<Poker> myPokers)
        {
            if (myPokers == null) return null;
            if (myPokers.Count < 5) return null;
            List<Deck> abcdeDeck = GetAllShunZi(myPokers);
            if (abcdeDeck == null) return null;
            if (abcdeDeck.Count == 1)
            {
                return abcdeDeck[0];
            }
            return null;
        }
        #endregion

        #region 检测同花
        public static Deck CheckTongHua(List<Poker> myPokers)
        {
            if (myPokers == null) return null;
            if (myPokers.Count < 5) return null;
            List<Deck> abcdeDeck = GetAllTongHua(myPokers);
            if (abcdeDeck == null) return null;
            if (abcdeDeck.Count == 1)
            {
                SortCards(abcdeDeck[0].pokers);
                return new Deck(DeckType.TONG_HUA, abcdeDeck[0].pokers, abcdeDeck[0].pokers[4]);
            }
            return null;

        }
        #endregion

        #region 检测葫芦
        public static Deck CheckHuLu(List<Poker> myPokers)
        {
            if (myPokers == null) return null;
            if (myPokers.Count < 5) return null;
            List<Deck> aaabbDeck = GetAllHuLu(myPokers);
            if (aaabbDeck == null) return null;
            if (aaabbDeck.Count == 1)
            {
                return aaabbDeck[0];
            }
            return null;
        }
        #endregion

        #region 检测铁支
        public static Deck CheckTieZhi(List<Poker> myPokers)
        {
            if (myPokers == null) return null;
            if (myPokers.Count < 5) return null;
            List<Deck> aaaabDeck = GetAllTieZhi(myPokers);
            if (aaaabDeck == null) return null;
            if (aaaabDeck.Count == 1)
            {
                return aaaabDeck[0];
            }
            return null;
        }
        #endregion

        #region 检测同花顺
        public static Deck CheckTongHuaShun(List<Poker> myPokers)
        {
            if (myPokers == null) return null;
            if (myPokers.Count < 5) return null;
            List<Deck> deck = GetAllTongHuaShun(myPokers);
            if (deck == null) return null;
            if (deck.Count == 1)
            {
                return deck[0];
            }
            return null;
        }
        #endregion

        //-------------------------------------------------------------------------------------------------------------------


        //---------------------------------------------提示--------------------------------------------------------------
        #region 提示获取所有的同花顺
        public static List<Deck> GetAllTongHuaShun(List<Poker> myPokers)
        {
            List<Deck> ret = new List<Deck>();
            if (myPokers == null) return ret;
            if (myPokers.Count < 5) return ret;
            for (int i = 0; i < myPokers.Count; ++i)
            {
                List<Poker> lst = new List<Poker>();
                for (int j = 0; j < 5; ++j)
                {
                    bool isExists = false;
                    for (int k = 0; k < myPokers.Count; ++k)
                    {
                        if (myPokers[k].Size == myPokers[i].Size + j && myPokers[k].Color == myPokers[i].Color)
                        {
                            isExists = true;
                            lst.Add(myPokers[k]);
                            break;
                        }
                    }
                    if (!isExists)
                    {
                        break;
                    }
                }
                if (lst.Count == 5)
                {
                    ret.Add(new Deck(DeckType.TONG_HUA_SHUN, lst, lst[lst.Count - 1]));
                }
            }
            ret.Sort();
            return ret;
        }
        #endregion

        #region 提示获取所有的炸弹（铁支）
        public static List<Deck> GetAllTieZhi(List<Poker> myPokers)
        {
            List<Deck> ret = new List<Deck>();
            if (myPokers == null) return ret;
            if (myPokers.Count < 4) return ret;
            List<Poker> lst = new List<Poker>();
            for (int i = 0; i < myPokers.Count; ++i)
            {
                bool isExists = false;
                for (int j = 0; j < ret.Count; j++)
                {
                    if (ret[j].pokers[0].Size == myPokers[i].Size)
                    {
                        isExists = true;
                        break;
                    }
                }
                if (isExists) continue;
                lst.Clear();
                for (int j = 0; j < myPokers.Count; ++j)
                {
                    if (myPokers[i].Size == myPokers[j].Size)
                    {
                        lst.Add(myPokers[j]);
                        if (lst.Count == 4) break;
                    }
                }
                if (lst.Count == 4)
                {
                    ret.Add(new Deck(DeckType.TIE_ZHI, new List<Poker>(lst), myPokers[i]));
                }
            }
            ret.Sort();
            return ret;
        }
        #endregion

        #region 提示获取所有的葫芦
        public static List<Deck> GetAllHuLu(List<Poker> myPokers)
        {
            List<Deck> ret = new List<Deck>();
            if (myPokers == null) return ret;
            if (myPokers.Count < 5) return ret;
            List<Deck> aaaDeck = GetAllSanTiao(myPokers);
            List<Deck> aaDeck = new List<Deck>();
            aaDeck.AddRange(GetAllDuiZi(myPokers));
            if (aaaDeck == null || aaaDeck.Count == 0) return ret;
            if (aaDeck == null || aaDeck.Count == 0) return ret;

            for (int i = 0; i < aaaDeck.Count; i++)
            {
                for (int j = 0; j < aaDeck.Count; j++)
                {
                    if (aaaDeck[i].mainPoker.Size != aaDeck[j].mainPoker.Size)
                    {
                        List<Poker> lst = new List<Poker>();
                        lst.AddRange(aaaDeck[i].pokers);
                        lst.AddRange(aaDeck[j].pokers);
                        ret.Add(new Deck(DeckType.HU_LU, lst, aaaDeck[i].mainPoker));
                    }
                }
            }
            ret.Sort();
            return ret;
        }
        #endregion

        #region 提示获取所有的同花
        public static List<Deck> GetAllTongHua(List<Poker> myPokers)
        {
            List<Deck> ret = new List<Deck>();
            if (myPokers == null) return ret;
            if (myPokers.Count < 5) return ret;
            Poker poker = myPokers[0];
            List<Poker> lst = new List<Poker>();

            for (int i = 0; i < myPokers.Count; i++)
            {
                for (int j = 0; j < myPokers.Count; j++)
                {
                    if (myPokers[i].Color == myPokers[j].Color)//&&myPokers[i].Size!=myPokers[j].Size
                    {
                        lst.Add(myPokers[j]);
                        if (lst.Count == 5) break;
                    }
                    else
                    {
                        lst.Clear();
                    }
                }
                if (lst.Count == 5)
                {
                    ret.Add(new Deck(DeckType.TONG_HUA, lst, lst[0]));
                    break;
                }
            }
            ret.Sort();
            return ret;
        }
        #endregion

        #region 提示获取所有的顺子
        public static List<Deck> GetAllShunZi(List<Poker> myPokers)
        {
            List<Deck> ret = new List<Deck>();
            if (myPokers == null) return ret;
            if (myPokers.Count < 5) return ret;
            for (int i = 0; i < myPokers.Count; ++i)
            {
                List<Poker> lst = new List<Poker>();
                for (int j = 0; j < 5; ++j)
                {
                    bool isExists = false;
                    for (int k = 0; k < myPokers.Count; ++k)
                    {
                        if (myPokers[k].Size == myPokers[i].Size + j)
                        {
                            isExists = true;
                            lst.Add(myPokers[k]);
                            break;
                        }
                    }
                    if (!isExists)
                    {
                        break;
                    }
                }
                if (lst.Count == 5)
                {
                    ret.Add(new Deck(DeckType.SHUN_ZI, lst, lst[lst.Count - 1]));
                }
            }
            ret.Sort();
            return ret;
        }
        #endregion

        #region 提示获取所有的三条
        public static List<Deck> GetAllSanTiao(List<Poker> myPokers)
        {
            List<Deck> ret = new List<Deck>();
            if (myPokers == null) return ret;
            if (myPokers.Count < 3) return ret;
            List<Poker> lst = new List<Poker>();
            for (int i = 0; i < myPokers.Count; ++i)
            {
                bool isExists = false;
                for (int z = 0; z < ret.Count; z++)
                {
                    if (ret[z].pokers[0].Size == myPokers[i].Size)
                    {
                        isExists = true;
                        break;
                    }
                }
                if (isExists) continue;
                lst.Clear();
                for (int k = 0; k < myPokers.Count; ++k)
                {
                    if (myPokers[k].Size == myPokers[i].Size)
                    {
                        lst.Add(myPokers[k]);
                        if (lst.Count == 3) break;
                    }
                }
                if (lst.Count == 3)
                {
                    ret.Add(new Deck(DeckType.SAN_TIAO, new List<Poker>(lst), myPokers[i]));
                }
            }
            ret.Sort();
            return ret;
        }
        #endregion

        #region 提示获取所有的两对
        public static List<Deck> GetAllLiangDui(List<Poker> myPokers)
        {
            List<Deck> ret = new List<Deck>();
            if (myPokers == null) return ret;
            if (myPokers.Count < 4) return ret;
            List<Deck> aaDeck = GetAllDuiZi(myPokers);
            List<Poker> lst = new List<Poker>();
            for (int i = 0; i < aaDeck.Count; ++i)
            {
                for (int j = i + 1; j < aaDeck.Count; ++j)
                {
                    lst.Clear();
                    if (aaDeck[i].pokers[0].Size != aaDeck[j].pokers[0].Size)
                    {
                        lst.AddRange(aaDeck[i].pokers);
                        lst.AddRange(aaDeck[j].pokers);
                        if (lst.Count == 4)
                        {
                            ret.Add(new Deck(DeckType.LIANG_DUI, new List<Poker>(lst), lst[0]));
                        }
                    }
                }
            }
            ret.Sort();
            return ret;
        }
        #endregion

        #region 提示获取所有的对子
        public static List<Deck> GetAllDuiZi(List<Poker> myPokers)
        {
            List<Deck> ret = new List<Deck>();
            if (myPokers == null) return ret;
            if (myPokers.Count < 2) return ret;
            List<Poker> lst = new List<Poker>();
            for (int i = 0; i < myPokers.Count; ++i)
            {
                bool isExists = false;
                for (int j = 0; j < ret.Count; ++j)
                {
                    if (ret[j].pokers[0].Size == myPokers[i].Size)
                    {
                        isExists = true;
                        break;
                    }
                }
                if (isExists) continue;
                lst.Clear();
                for (int j = 0; j < myPokers.Count; ++j)
                {
                    if (myPokers[i].Size == myPokers[j].Size)
                    {
                        lst.Add(myPokers[j]);
                        if (lst.Count == 2) break;
                    }
                }
                if (lst.Count == 2)
                {
                    ret.Add(new Deck(DeckType.DUI_ZI, new List<Poker>(lst), myPokers[i]));
                }
            }
            ret.Sort();
            return ret;
        }
        #endregion
        //------------------------------------------------------------------------------------------------------------------

        //---------------------------------------------排序--------------------------------------------------------------

        #region 对牌进行排序的方法
        /// <summary>
        /// 手牌排序
        /// </summary>
        /// <param name="Cards"></param>
        public static void SortCards(List<Poker> Pokers, bool isSize = true)
        {
            if (isSize)
            {
                Pokers.Sort(Comparator);
            }
            else
            {
                Pokers.Sort(ComparatorHuaSe);
            }
        }
        /// <summary>
        /// 按照大小排序
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

        public static void SortCards1(List<Poker> Pokers)
        {
            Pokers.Sort(Comparator1);
        }

        private static int Comparator1(Poker x, Poker y)
        {
            if (x.Size >= y.Size && x.Color >= y.Color)
                return 1;
            else
                return -1;
        }
        /// <summary>
        /// 按照花色排序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static int ComparatorHuaSe(Poker x, Poker y)
        {
            if (x.Color >= y.Color)
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
    }
}
