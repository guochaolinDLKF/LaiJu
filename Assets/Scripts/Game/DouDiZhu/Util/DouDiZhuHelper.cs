//===================================================
//Author      : DRB
//CreateTime  ：11/30/2017 11:40:55 AM
//Description ：
//===================================================

using System;
using System.Collections.Generic;

namespace DRB.DouDiZhu
{
    /// <summary>
    /// 牌组类型
    /// </summary>
    public enum DeckType
    {
        A,
        AA,
        AAA = 16,
        AAAB = 17,
        AAABB = 18,
        ABCDE,
        AAAABBCC,
        AAAABC,
        AABBCC,
        AAABBB,
        AAABBBCD,
        AAABBBCCDD,
        AAAA,
        SS,
    }

    public class Deck : IComparable<Deck>
    {
        public DeckType type;

        public List<Poker> pokers;

        public Poker mainPoker;

        public Deck(DeckType type, List<Poker> pokers, Poker mainPoker)
        {
            this.type = type;
            this.pokers = pokers;
            this.mainPoker = mainPoker;
        }

        public static bool operator <(Deck l, Deck r)
        {
            if (l == null) return true;
            if (r == null) return false;
            if (l.type != r.type)
            {
                if (l.type == DeckType.SS)
                {
                    return false;
                }
                if (r.type == DeckType.SS)
                {
                    return true;
                }
                if (l.type == DeckType.AAAA)
                {
                    return false;
                }
                if (r.type == DeckType.AAAA)
                {
                    return true;
                }
                else
                {
                    return true;
                }
            }
            if (r.type == DeckType.ABCDE)
            {
                if (r.pokers.Count != l.pokers.Count)
                {
                    return true;
                }
            }
            if (r.type == DeckType.AABBCC)
            {
                if (r.pokers.Count != l.pokers.Count)
                {
                    return true;
                }
            }
            if (r.type == DeckType.AAABBB)
            {
                if (r.pokers.Count != l.pokers.Count)
                {
                    return true;
                }
            }
            if (r.type == DeckType.AAABBBCCDD)
            {
                if (r.pokers.Count != l.pokers.Count)
                {
                    return true;
                }
            }
            if (r.type == DeckType.AAABBBCD)
            {
                if (r.pokers.Count != l.pokers.Count)
                {
                    return true;
                }
            }
            return l.mainPoker.power < r.mainPoker.power;
        }

        public static bool operator >(Deck l, Deck r)
        {
            if (l == null) return false;
            if (r == null) return true;
            if (l.type != r.type)
            {
                if (l.type == DeckType.SS)
                {
                    return true;
                }
                if (r.type == DeckType.SS)
                {
                    return false;
                }
                if (l.type == DeckType.AAAA)
                {
                    return true;
                }
                if (r.type == DeckType.AAAA)
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
            if (r.type == DeckType.ABCDE)
            {
                if (r.pokers.Count != l.pokers.Count)
                {
                    return false;
                }
            }
            if (r.type == DeckType.AABBCC)
            {
                if (r.pokers.Count != l.pokers.Count)
                {
                    return false;
                }
            }
            return l.mainPoker.power > r.mainPoker.power;
        }

        public static bool operator <=(Deck l, Deck r)
        {
            if (l == null) return true;
            if (r == null) return false;
            if (l.type != r.type)
            {
                if (l.type == DeckType.SS)
                {
                    return false;
                }
                if (r.type == DeckType.SS)
                {
                    return true;
                }
                if (l.type == DeckType.AAAA)
                {
                    return false;
                }
                if (r.type == DeckType.AAAA)
                {
                    return true;
                }
                else
                {
                    return true;
                }
            }
            if (r.type == DeckType.ABCDE)
            {
                if (r.pokers.Count != l.pokers.Count)
                {
                    return true;
                }
            }
            if (r.type == DeckType.AABBCC)
            {
                if (r.pokers.Count != l.pokers.Count)
                {
                    return true;
                }
            }
            if (r.type == DeckType.AAABBB)
            {
                if (r.pokers.Count != l.pokers.Count)
                {
                    return true;
                }
            }
            if (r.type == DeckType.AAABBBCCDD)
            {
                if (r.pokers.Count != l.pokers.Count)
                {
                    return true;
                }
            }
            if (r.type == DeckType.AAABBBCD)
            {
                if (r.pokers.Count != l.pokers.Count)
                {
                    return true;
                }
            }
            return l.mainPoker.power <= r.mainPoker.power;
        }

        public static bool operator >=(Deck l, Deck r)
        {
            if (l == null) return false;
            if (r == null) return true;
            if (l.type != r.type)
            {
                if (l.type == DeckType.SS)
                {
                    return true;
                }
                if (r.type == DeckType.SS)
                {
                    return false;
                }
                if (l.type == DeckType.AAAA)
                {
                    return true;
                }
                if (r.type == DeckType.AAAA)
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
            if (r.type == DeckType.ABCDE)
            {
                if (r.pokers.Count != l.pokers.Count)
                {
                    return false;
                }
            }
            if (r.type == DeckType.AABBCC)
            {
                if (r.pokers.Count != l.pokers.Count)
                {
                    return false;
                }
            }
            if (r.type == DeckType.AAABBB)
            {
                if (r.pokers.Count != l.pokers.Count)
                {
                    return false;
                }
            }
            if (r.type == DeckType.AAABBBCCDD)
            {
                if (r.pokers.Count != l.pokers.Count)
                {
                    return false;
                }
            }
            if (r.type == DeckType.AAABBBCD)
            {
                if (r.pokers.Count != l.pokers.Count)
                {
                    return false;
                }
            }
            return l.mainPoker.power >= r.mainPoker.power;
        }

        public int CompareTo(Deck other)
        {
            if (other == null) return -1;
            if (this.mainPoker.power - other.mainPoker.power == 0)
            {
                return mainPoker.color - other.mainPoker.color;
            }
            return this.mainPoker.power - other.mainPoker.power;
        }
    }

    public static class DouDiZhuHelper
    {
        #region Check 检查类型
        /// <summary>
        /// 检查类型
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static Deck Check(List<Poker> pokers)
        {
            if (pokers == null || pokers.Count == 0) return null;

            Deck ret = null;

            ret = CheckA(pokers);
            if (ret != null) return ret;
            ret = CheckSS(pokers);
            if (ret != null) return ret;
            ret = CheckAA(pokers);
            if (ret != null) return ret;
            ret = CheckAAA(pokers);
            if (ret != null) return ret;
            ret = CheckAAAA(pokers);
            if (ret != null) return ret;
            ret = CheckAAAB(pokers);
            if (ret != null) return ret;
            ret = CheckAAABB(pokers);
            if (ret != null) return ret;
            ret = CheckABCDE(pokers);
            if (ret != null) return ret;
            ret = CheckAAAABC(pokers);
            if (ret != null) return ret;
            ret = CheckAAAABBCC(pokers);
            if (ret != null) return ret;
            ret = CheckAABBCC(pokers);
            if (ret != null) return ret;
            ret = CheckAAABBB(pokers);
            if (ret != null) return ret;
            ret = CheckAAABBBCD(pokers);
            if (ret != null) return ret;
            ret = CheckAAABBBCCDD(pokers);
            if (ret != null) return ret;

            return ret;
        }
        #endregion

        #region Check 检查类型
        /// <summary>
        /// 检查类型
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static List<Deck> Check2(List<Poker> pokers)
        {
            if (pokers == null || pokers.Count == 0) return null;

            List<Deck> lstRet = new List<Deck>();

            Deck ret = null;

            ret = CheckA(pokers);
            if (ret != null) lstRet.Add(ret);
            ret = CheckSS(pokers);
            if (ret != null) lstRet.Add(ret);
            ret = CheckAA(pokers);
            if (ret != null) lstRet.Add(ret);
            ret = CheckAAA(pokers);
            if (ret != null) lstRet.Add(ret);
            ret = CheckAAAA(pokers);
            if (ret != null) lstRet.Add(ret);
            ret = CheckAAAB(pokers);
            if (ret != null) lstRet.Add(ret);
            ret = CheckAAABB(pokers);
            if (ret != null) lstRet.Add(ret);
            ret = CheckABCDE(pokers);
            if (ret != null) lstRet.Add(ret);
            ret = CheckAAAABC(pokers);
            if (ret != null) lstRet.Add(ret);
            ret = CheckAAAABBCC(pokers);
            if (ret != null) lstRet.Add(ret);
            ret = CheckAABBCC(pokers);
            if (ret != null) lstRet.Add(ret);
            ret = CheckAAABBB(pokers);
            if (ret != null) lstRet.Add(ret);
            ret = CheckAAABBBCD(pokers);
            if (ret != null) lstRet.Add(ret);
            ret = CheckAAABBBCCDD(pokers);
            if (ret != null) lstRet.Add(ret);

            return lstRet;
        }
        #endregion

        #region CheckA 检测单牌
        /// <summary>
        /// 检测单牌
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static Deck CheckA(List<Poker> pokers)
        {
            if (pokers == null) return null;
            if (pokers.Count != 1) return null;
            List<Deck> lstDeck = GetAllA(pokers);
            if (lstDeck == null || lstDeck.Count == 0) return null;
            return lstDeck[0];
        }
        #endregion

        #region CheckAA 检测对
        /// <summary>
        /// 检测对
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static Deck CheckAA(List<Poker> pokers)
        {
            if (pokers == null) return null;
            if (pokers.Count != 2) return null;
            List<Deck> lstDeck = GetAllAA(pokers);
            if (lstDeck == null || lstDeck.Count == 0) return null;
            return lstDeck[0];
        }
        #endregion

        #region CheckAAA 检测三张
        /// <summary>
        /// 检测三张
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static Deck CheckAAA(List<Poker> pokers)
        {
            if (pokers == null) return null;
            if (pokers.Count != 3) return null;
            List<Deck> lstDeck = GetAllAAA(pokers);
            if (lstDeck == null || lstDeck.Count == 0) return null;
            return lstDeck[0];
        }
        #endregion

        #region CheckAAAA 检测炸弹
        /// <summary>
        /// 检测炸弹
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static Deck CheckAAAA(List<Poker> pokers)
        {
            if (pokers == null) return null;
            if (pokers.Count != 4) return null;
            List<Deck> lstDeck = GetAllAAAA(pokers);
            if (lstDeck == null || lstDeck.Count == 0) return null;
            return lstDeck[0];
        }
        #endregion

        #region CheckAAAB 检测三带一
        /// <summary>
        /// 检测三带一
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static Deck CheckAAAB(List<Poker> pokers)
        {
            if (pokers == null) return null;
            if (pokers.Count != 4) return null;

            List<Deck> lstDeck = GetAllAAAB(pokers);
            if (lstDeck == null || lstDeck.Count == 0) return null;
            return lstDeck[0];
        }
        #endregion

        #region CheckAAABB 检测三带一对
        /// <summary>
        /// 检测三带一对
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static Deck CheckAAABB(List<Poker> pokers)
        {
            if (pokers == null) return null;
            if (pokers.Count != 5) return null;

            List<Deck> lstDeck = GetAllAAABB(pokers);
            if (lstDeck == null || lstDeck.Count == 0) return null;
            return lstDeck[0];
        }
        #endregion

        #region CheckABCDE 检测顺子
        /// <summary>
        /// 检测顺子
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static Deck CheckABCDE(List<Poker> pokers)
        {
            if (pokers == null) return null;
            if (pokers.Count < 5) return null;

            List<Deck> lstDeck = GetAllABCDE(pokers, pokers.Count);
            if (lstDeck == null || lstDeck.Count == 0) return null;
            return lstDeck[0];
        }
        #endregion

        #region CheckAAAABBCC 检测四带两对
        /// <summary>
        /// 检测四带两对
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static Deck CheckAAAABBCC(List<Poker> pokers)
        {
            if (pokers == null) return null;
            if (pokers.Count != 8) return null;

            List<Deck> lstDeck = GetAllAAAABBCC(pokers);
            if (lstDeck == null || lstDeck.Count == 0) return null;
            return lstDeck[0];
        }
        #endregion

        #region CheckAAAABC 检测四带二
        /// <summary>
        /// 检测四带二
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static Deck CheckAAAABC(List<Poker> pokers)
        {
            if (pokers == null) return null;
            if (pokers.Count != 6) return null;

            List<Deck> lstDeck = GetAllAAAABC(pokers);
            if (lstDeck == null || lstDeck.Count == 0) return null;
            return lstDeck[0];
        }
        #endregion

        #region CheckAABBCC 检测双顺
        /// <summary>
        /// 检测双顺
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static Deck CheckAABBCC(List<Poker> pokers)
        {
            if (pokers == null) return null;
            if (pokers.Count < 6) return null;
            if (pokers.Count % 2 != 0) return null;

            List<Deck> lstDeck = GetAllAABBCC(pokers, pokers.Count / 2);
            if (lstDeck == null || lstDeck.Count == 0) return null;
            return lstDeck[0];
        }
        #endregion

        #region CheckAAABBB 检测三顺
        /// <summary>
        /// 检测三顺
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static Deck CheckAAABBB(List<Poker> pokers)
        {
            if (pokers == null) return null;
            if (pokers.Count < 6) return null;
            if (pokers.Count % 3 != 0) return null;

            List<Deck> lstDeck = GetAllAAABBB(pokers, pokers.Count / 3);
            if (lstDeck == null || lstDeck.Count == 0) return null;
            return lstDeck[0];
        }
        #endregion

        #region CheckAAABBBCD 检测飞机三带一
        /// <summary>
        /// 检测飞机三带一
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static Deck CheckAAABBBCD(List<Poker> pokers)
        {
            if (pokers == null) return null;
            if (pokers.Count < 8) return null;
            if (pokers.Count % 4 != 0) return null;

            List<Deck> lstDeck = GetAllAAABBBCD(pokers, pokers.Count / 4);
            if (lstDeck == null || lstDeck.Count == 0) return null;
            return lstDeck[0];
        }
        #endregion

        #region CheckAAABBBCCDD 检测飞机三带一对
        /// <summary>
        /// 检测飞机三带一对
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static Deck CheckAAABBBCCDD(List<Poker> pokers)
        {
            if (pokers == null) return null;
            if (pokers.Count < 10) return null;
            if (pokers.Count % 5 != 0) return null;

            List<Deck> lstDeck = GetAllAAABBBCCDD(pokers, pokers.Count / 5);
            if (lstDeck == null || lstDeck.Count == 0) return null;
            return lstDeck[0];
        }
        #endregion

        #region CheckSS 检测双王
        /// <summary>
        /// 检测双王
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static Deck CheckSS(List<Poker> pokers)
        {
            if (pokers == null) return null;
            if (pokers.Count != 2) return null;

            if (pokers[0].power == 16)
            {
                if (pokers[1].power == 17)
                {
                    return new Deck(DeckType.SS, pokers, pokers[1]);
                }
            }
            else if (pokers[0].power == 17)
            {
                if (pokers[1].power == 16)
                {
                    return new Deck(DeckType.SS, pokers, pokers[0]);
                }
            }
            return null;
        }
        #endregion

        #region GetLongestABCED 获得牌型中最长的顺子
        /// <summary>
        /// 获得牌型中最长的顺子
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static Deck GetLongestABCED(List<Poker> pokers)
        {
            List<Deck> ret = new List<Deck>();
            if (pokers == null) return null;
            if (pokers.Count < 5) return null;

            pokers.Sort();
            for (int i = 0; i < pokers.Count; ++i)
            {
                if (pokers[i].power + 5 > 15) continue;
                if (i > 0 && pokers[i].size == pokers[i - 1].size) continue;
                List<Poker> result = new List<Poker>();

                for (int j = 0; j < pokers.Count; ++j)
                {
                    bool isExists = false;
                    for (int k = 0; k < pokers.Count; ++k)
                    {
                        if (pokers[i].power + j > 14) continue;

                        if (pokers[k].power == pokers[i].power + j)
                        {
                            isExists = true;
                            result.Add(pokers[k]);
                            break;
                        }
                    }
                    if (!isExists)
                    {
                        break;
                    }
                }
                if (result.Count > 4)
                {
                    ret.Add(new Deck(DeckType.ABCDE, result, result[result.Count - 1]));
                }
            }
            ret.Sort();

            Deck longestDeck = null;

            if (ret.Count > 0)
            {
                longestDeck = ret[0];
            }

            for (int i = 0; i < ret.Count; i++)
            {
                if (ret[i].pokers.Count > longestDeck.pokers.Count)
                {
                    longestDeck = ret[i];
                }
            }
            return longestDeck;
        }
        #endregion

        #region GetAllDeck 获取所有组合
        /// <summary>
        /// 获取所有组合
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static List<Deck> GetAllDeck(List<Poker> pokers)
        {
            List<Deck> ret = new List<Deck>();

            if (pokers != null) pokers.Sort();
            ret.AddRange(GetAllA(pokers));
            ret.AddRange(GetAllAA(pokers));
            ret.AddRange(GetAllAAA(pokers));
            ret.AddRange(GetAllAAAA(pokers));
            ret.AddRange(GetAllABCDE(pokers));
            ret.AddRange(GetAllAAAB(pokers));
            ret.AddRange(GetAllAAABB(pokers));
            ret.AddRange(GetAllAABBCC(pokers));
            ret.AddRange(GetAllAAABBB(pokers));
            ret.AddRange(GetAllAAABBBCD(pokers));
            ret.AddRange(GetAllAAABBBCCDD(pokers));
            ret.AddRange(GetAllAAAABC(pokers));
            ret.AddRange(GetAllAAAABBCC(pokers));
            ret.AddRange(GetAllSS(pokers));
            //for (int i = 0; i < ret.Count; i++)
            //{
            //    AppDebug.Log(ret[i].type);
            //    for (int j = 0; j < ret[i].pokers.Count; j++)
            //    {
            //        AppDebug.Log(ret[i].pokers[j].ToString() + "?");
            //    }
            //}
            return ret;
        }
        #endregion

        public static List<Deck> GetAStrongerDeck(List<Poker> pokers)
        {
            if (pokers != null) pokers.Sort();

            List<Deck> ret = new List<Deck>();


            List<Deck> lst = new List<Deck>();
            lst.AddRange(GetAllAA(pokers));
            lst.AddRange(GetAllAAA(pokers));
            lst.AddRange(GetAllAAAA(pokers));

            for (int j = 0; j < pokers.Count; ++j)
            {
                bool isExists = false;
                for (int k = 0; k < lst.Count; ++k)
                {
                    if (lst[k].pokers.Contains(pokers[j]))
                    {
                        isExists = true;
                        break;
                    }
                }
                if (!isExists)
                {
                    ret.Add(new Deck(DeckType.A, new List<Poker>() { pokers[j] }, pokers[j]));
                }
            }
            for (int i = 0; i < lst.Count; i++)
            {
                bool isExists = false;
                for (int j = 0; j < ret.Count; j++)
                {
                    if (ret[j].pokers.Contains(lst[i].mainPoker))
                    {
                        isExists = true;
                        break;
                    }                    
                }
                if (!isExists)
                {
                    ret.Add(new Deck(DeckType.A, new List<Poker>() { lst[i].mainPoker }, lst[i].mainPoker));
                }
            }

            ret.AddRange(GetAllAAAA(pokers));
            ret.AddRange(GetAllSS(pokers));

            //if (ret != null)
            //{
            //    for (int i = ret.Count - 1; i >= 0; --i)
            //    {
            //        if (ret[i] <= deck)
            //        {
            //            ret.RemoveAt(i);
            //        }
            //    }
            //}
            return ret;
        }
        public static List<Deck> GetAStrongerDeck(Deck deck, List<Poker> pokers)
        {
            if (pokers != null) pokers.Sort();

            List<Deck> ret = new List<Deck>();

            ret.AddRange(GetAllA(pokers));

            if (ret != null)
            {
                for (int i = ret.Count - 1; i >= 0; --i)
                {
                    if (ret[i] <= deck)
                    {
                        ret.RemoveAt(i);
                    }
                }
            }
            return ret;
        }

        public static List<Deck> GetAAStrongerDeck(Deck deck, List<Poker> pokers)
        {
            if (deck == null) return GetAllDeck(pokers);

            if (pokers != null) pokers.Sort();

            List<Deck> ret = new List<Deck>();

            List<Deck> lst = new List<Deck>();

            List<Deck> lstDeck = new List<Deck>();

            ret.AddRange(GetAllAA(pokers));
            lst.AddRange(GetAllAAA(pokers));
            lst.AddRange(GetAllAAAA(pokers));

            for (int i = 0; i < ret.Count; i++)
            {
                for (int j = 0; j < lst.Count; j++)
                {
                    if (lst[j].mainPoker.size == ret[i].mainPoker.size)
                    {
                        lstDeck.Add(ret[i]);
                        ret.Remove(ret[i]);
                        break;
                    }
                }
            }
            ret.AddRange(lstDeck);

            ret.AddRange(GetAllAAAA(pokers));
            ret.AddRange(GetAllSS(pokers));

            //if (ret != null)
            //{
            //    for (int i = ret.Count - 1; i >= 0; --i)
            //    {
            //        if (ret[i] <= deck)
            //        {
            //            ret.RemoveAt(i);
            //        }
            //    }
            //}
            return ret;
        }

        #region GetStrongerDeck 获取更大的组合
        /// <summary>
        /// 获取更大的组合
        /// </summary>
        /// <param name="deck"></param>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static List<Deck> GetStrongerDeck(Deck deck, List<Poker> pokers)
        {
            if (deck == null) return GetAllDeck(pokers);

            if (pokers != null) pokers.Sort();

            List<Deck> ret = new List<Deck>();
            switch (deck.type)
            {
                case DeckType.A:
                    //ret.AddRange(GetAllA(pokers));
                    //ret.AddRange(GetAllAAAA(pokers));
                    //ret.AddRange(GetAllSS(pokers));
                    ret.AddRange(GetAStrongerDeck(pokers));
                    break;
                case DeckType.AA:
                    //ret.AddRange(GetAllAA(pokers));
                    //ret.AddRange(GetAllAAAA(pokers));
                    //ret.AddRange(GetAllSS(pokers));
                    ret.AddRange(GetAAStrongerDeck(deck, pokers));
                    break;
                case DeckType.AAA:
                    ret.AddRange(GetAllAAA(pokers));
                    ret.AddRange(GetAllAAAA(pokers));
                    ret.AddRange(GetAllSS(pokers));
                    break;
                case DeckType.AAAB:
                    ret.AddRange(GetAllAAAB(pokers));
                    ret.AddRange(GetAllAAAA(pokers));
                    ret.AddRange(GetAllSS(pokers));
                    break;
                case DeckType.AAABB:
                    ret.AddRange(GetAllAAABB(pokers));
                    ret.AddRange(GetAllAAAA(pokers));
                    ret.AddRange(GetAllSS(pokers));
                    break;
                case DeckType.AAAA:
                    ret.AddRange(GetAllAAAA(pokers));
                    ret.AddRange(GetAllSS(pokers));
                    break;
                case DeckType.AAAABC:
                    ret.AddRange(GetAllAAAA(pokers));
                    ret.AddRange(GetAllSS(pokers));
                    break;
                case DeckType.AAAABBCC:
                    ret.AddRange(GetAllAAAA(pokers));
                    ret.AddRange(GetAllSS(pokers));
                    break;
                case DeckType.AABBCC:
                    ret.AddRange(GetAllAABBCC(pokers, deck.pokers.Count / 2));
                    ret.AddRange(GetAllAAAA(pokers));
                    ret.AddRange(GetAllSS(pokers));
                    break;
                case DeckType.AAABBB:
                    ret.AddRange(GetAllAAABBB(pokers, deck.pokers.Count / 3));
                    ret.AddRange(GetAllAAAA(pokers));
                    ret.AddRange(GetAllSS(pokers));
                    break;
                case DeckType.AAABBBCD:
                    ret.AddRange(GetAllAAABBBCD(pokers, deck.pokers.Count / 4));
                    ret.AddRange(GetAllAAAA(pokers));
                    ret.AddRange(GetAllSS(pokers));
                    break;
                case DeckType.AAABBBCCDD:
                    ret.AddRange(GetAllAAABBBCCDD(pokers, deck.pokers.Count / 5));
                    ret.AddRange(GetAllAAAA(pokers));
                    ret.AddRange(GetAllSS(pokers));
                    break;
                case DeckType.ABCDE:
                    ret.AddRange(GetAllABCDE(pokers, deck.pokers.Count));
                    ret.AddRange(GetAllAAAA(pokers));
                    ret.AddRange(GetAllSS(pokers));
                    break;
            }

            if (ret != null)
            {
                for (int i = ret.Count - 1; i >= 0; --i)
                {
                    if (ret[i] <= deck)
                    {
                        ret.RemoveAt(i);
                    }
                }
            }
            return ret;
        }
        #endregion

        #region GetAllA 获取所有单牌
        /// <summary>
        /// 获取所有单牌
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        private static List<Deck> GetAllA(List<Poker> pokers)
        {
            List<Deck> ret = new List<Deck>();
            if (pokers == null) return ret;

            for (int i = 0; i < pokers.Count; ++i)
            {
                bool isExists = false;
                for (int j = 0; j < ret.Count; ++j)
                {
                    if (ret[j].pokers[0].size == pokers[i].size)
                    {
                        isExists = true;
                        break;
                    }
                }
                if (isExists) continue;
                ret.Add(new Deck(DeckType.A, new List<Poker>() { pokers[i] }, pokers[i]));
            }
            ret.Sort();
            return ret;
        }
        #endregion

        #region GetAllAA 获取所有对
        /// <summary>
        /// 获取所有对
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        private static List<Deck> GetAllAA(List<Poker> pokers)
        {
            List<Deck> ret = new List<Deck>();
            if (pokers == null) return ret;
            if (pokers.Count < 2) return ret;

            List<Poker> temp = new List<Poker>();
            for (int i = 0; i < pokers.Count; ++i)
            {
                if (i > 0 && pokers[i].size == pokers[i - 1].size) continue;

                temp.Clear();

                for (int j = 0; j < pokers.Count; ++j)
                {
                    if (pokers[i].size == pokers[j].size)
                    {
                        temp.Add(pokers[j]);
                        if (temp.Count == 2) break;
                    }
                }

                if (temp.Count < 2)
                {
                    for (int j = 0; j < pokers.Count; j++)
                    {
                        bool isExist = false;

                        for (int k = 0; k < temp.Count; k++)
                        {
                            if (pokers[j].size == temp[k].size && pokers[j].color == temp[k].color)
                            {
                                isExist = true;
                                break;
                            }
                        }

                        if (!isExist)
                        {
                            if (pokers[j].isUniversal)
                            {
                                temp.Add(pokers[j]);
                                if (temp.Count == 2) break;
                            }
                        }
                    }

                    //if (temp.Count == 2)
                    //{
                    //    bool isExist = false;
                    //    Poker mainPoker = null;
                    //    for (int j = 0; j < temp.Count; j++)
                    //    {
                    //        if (!temp[j].isUniversal)
                    //        {
                    //            isExist = true;
                    //            mainPoker = temp[j];
                    //            break;
                    //        }
                    //    }
                    //    if (isExist)
                    //    {
                    //        ret.Add(new Deck(DeckType.AA, new List<Poker>(temp), mainPoker));
                    //    }
                    //    else
                    //    {
                    //        ret.Add(new Deck(DeckType.AA, new List<Poker>(temp), temp[0]));
                    //    }
                    //}
                }
                //else
                //{
                //    if (temp.Count == 2)
                //    {
                //        ret.Add(new Deck(DeckType.AA, new List<Poker>(temp), pokers[i]));
                //    }
                //}
                if (temp.Count == 2)
                {
                    ret.Add(new Deck(DeckType.AA, new List<Poker>(temp), pokers[i]));
                }
            }
            ret.Sort();
            return ret;

            //=============================================================
            //List<Deck> ret = new List<Deck>();
            //if (pokers == null) return ret;
            //if (pokers.Count < 2) return ret;

            //List<Poker> temp = new List<Poker>();

            //for (int i = 0; i < pokers.Count; ++i)
            //{
            //    if (i > 0 && pokers[i].size == pokers[i - 1].size) continue;
            //    temp.Clear();
            //    for (int j = 0; j < pokers.Count; ++j)
            //    {
            //        if (pokers[i].size == pokers[j].size)
            //        {
            //            temp.Add(pokers[j]);
            //            if (temp.Count == 2) break;
            //        }
            //    }
            //    if (temp.Count == 2)
            //    {
            //        ret.Add(new Deck(DeckType.AA, new List<Poker>(temp), pokers[i]));
            //    }
            //}
            //ret.Sort();
            //return ret;
        }
        #endregion

        #region GetAllAAA 获取所有三张
        /// <summary>
        /// 获取所有三张
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        private static List<Deck> GetAllAAA(List<Poker> pokers)
        {
            List<Deck> ret = new List<Deck>();
            if (pokers == null) return ret;
            if (pokers.Count < 3) return ret;

            List<Poker> temp = new List<Poker>();

            List<Deck> lst = new List<Deck>();
            lst.AddRange(GetAllAAAA(pokers));

            for (int i = 0; i < pokers.Count; ++i)
            {
                if (i > 0 && pokers[i].size == pokers[i - 1].size) continue;
                temp.Clear();

                bool isExists = false;
                for (int k = 0; k < lst.Count; ++k)
                {
                    if (lst[k].pokers.Contains(pokers[i]))
                    {
                        isExists = true;
                        break;
                    }
                }
                if (!isExists)
                {
                    for (int k = 0; k < pokers.Count; ++k)
                    {
                        if (pokers[k].size == pokers[i].size)
                        {
                            temp.Add(pokers[k]);
                            if (temp.Count == 3) break;
                        }
                    }
                }

                if (temp.Count == 3)
                {
                    ret.Add(new Deck(DeckType.AAA, new List<Poker>(temp), temp[0]));
                    //AppDebug.LogWarning("====================");
                    //for (int m = 0; m < temp.Count; m++)
                    //{
                    //    AppDebug.LogWarning("Temp:" + temp[m].ToString());
                    //}
                }
            }
            for (int k = 0; k < lst.Count; k++)
            {
                temp.Clear();
                for (int j = 0; j < lst[k].pokers.Count; j++)
                {
                    if (!temp.Contains(lst[k].pokers[j]))
                    {
                        temp.Add(lst[k].pokers[j]);
                        if (temp.Count == 3) break;
                    }
                }
                if (temp.Count == 3)
                {
                    ret.Add(new Deck(DeckType.AAA, new List<Poker>(temp), temp[0]));
                }
            }

            //ret.Sort();
            return ret;

            //==================================================
            //List<Deck> ret = new List<Deck>();
            //if (pokers == null) return ret;
            //if (pokers.Count < 3) return ret;

            //List<Poker> temp = new List<Poker>();
            //for (int i = 0; i < pokers.Count; ++i)
            //{
            //    if (i > 0 && pokers[i].size == pokers[i - 1].size) continue;
            //    temp.Clear();
            //    for (int k = 0; k < pokers.Count; ++k)
            //    {
            //        if (pokers[k].size == pokers[i].size)
            //        {
            //            temp.Add(pokers[k]);
            //            if (temp.Count == 3) break;
            //        }
            //    }
            //    if (temp.Count == 3)
            //    {
            //        ret.Add(new Deck(DeckType.AAA, new List<Poker>(temp), temp[0]));
            //    }
            //}
            //ret.Sort();
            //return ret;
        }
        #endregion

        #region GetAllAAAA 获取所有炸弹
        /// <summary>
        /// 获取所有炸弹
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        private static List<Deck> GetAllAAAA(List<Poker> pokers)
        {
            List<Deck> ret = new List<Deck>();
            if (pokers == null) return ret;
            if (pokers.Count < 4) return ret;
            List<Poker> lst = new List<Poker>();
            for (int i = 0; i < pokers.Count; ++i)
            {
                if (i > 0 && pokers[i].size == pokers[i - 1].size) continue;
                lst.Clear();
                for (int k = 0; k < pokers.Count; ++k)
                {
                    if (pokers[k].size == pokers[i].size)
                    {
                        lst.Add(pokers[k]);
                    }
                }
                if (lst.Count < 4)
                {
                    for (int j = 0; j < pokers.Count; j++)
                    {
                        bool isExist = false;

                        for (int k = 0; k < lst.Count; k++)
                        {
                            if (pokers[j].size == lst[k].size && pokers[j].color == lst[k].color)
                            {
                                isExist = true;
                                break;
                            }
                        }
                        if (!isExist)
                        {
                            if (pokers[j].isUniversal)
                            {
                                lst.Add(pokers[j]);
                                if (lst.Count == 4) break;
                            }
                        }
                    }
                }

                if (lst.Count == 4)
                {
                    ret.Add(new Deck(DeckType.AAAA, new List<Poker>(lst), lst[0]));
                }
            }
            ret.Sort();
            return ret;
        }
        #endregion

        #region GetAllABCDE 获取所有顺子
        /// <summary>
        /// 获取所有顺子
        /// </summary>
        /// <param name="pokers"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static List<Deck> GetAllABCDE(List<Poker> pokers, int length = 5)
        {
            #region 癞子
            List<Deck> ret = new List<Deck>();
            if (pokers == null) return ret;
            if (pokers.Count < length) return ret;

            pokers.Sort();
            for (int i = 0; i < pokers.Count; ++i)
            {
                if (pokers[i].power + length > 15) continue;
                if (i > 0 && pokers[i].size == pokers[i - 1].size) continue;
                List<Poker> result = new List<Poker>();

                for (int j = 0; j < length; ++j)
                {
                    bool isExists = false;
                    for (int k = 0; k < pokers.Count; ++k)
                    {
                        if (pokers[k].power == pokers[i].power + j)
                        {
                            //isExists = true;
                            result.Add(pokers[k]);
                            break;
                        }
                    }
                    if (!isExists)
                    {
                        //break;
                    }
                }

                if (result.Count == length)
                {
                    ret.Add(new Deck(DeckType.ABCDE, result, result[result.Count - 1]));
                }
                else
                {
                    List<Poker> lstUniversalPoker = new List<Poker>();
                    for (int j = 0; j < pokers.Count; j++)
                    {
                        if (pokers[j].isUniversal)
                        {
                            lstUniversalPoker.Add(pokers[j]);
                        }
                    }
                    for (int j = 0; j < result.Count; j++)
                    {
                        if (result[j].isUniversal)
                        {
                            lstUniversalPoker.Remove(result[j]);
                        }
                    }

                    if (result.Count < length)
                    {
                        for (int j = 0; j < lstUniversalPoker.Count; j++)
                        {
                            result.Add(lstUniversalPoker[j]);
                            if (result.Count == length)
                            {
                                Poker mainPoker = new Poker(result[0].index, result[0].color, result[0].size, result[0].isUniversal);
                                
                                for (int k = 1; k < result.Count; k++)
                                {
                                    if (!result[k].isUniversal)
                                    {
                                        if (result[k].size < mainPoker.size)
                                        {
                                            mainPoker = new Poker(result[k].index, result[k].color, result[k].size, result[k].isUniversal);
                                        }
                                    }
                                }
                                ret.Add(new Deck(DeckType.ABCDE, result, new Poker(mainPoker.color, mainPoker.size + length - 1)));
                            }
                        }
                    }
                }
            }
            ret.Sort();
            return ret;

            #endregion

            //=================================================

            //List<Deck> ret = new List<Deck>();
            //if (pokers == null) return ret;
            //if (pokers.Count < length) return ret;

            //pokers.Sort();
            //for (int i = 0; i < pokers.Count; ++i)
            //{
            //    if (pokers[i].power + length > 15) continue;
            //    if (i > 0 && pokers[i].size == pokers[i - 1].size) continue;
            //    List<Poker> result = new List<Poker>();

            //    for (int j = 0; j < length; ++j)
            //    {
            //        bool isExists = false;
            //        for (int k = 0; k < pokers.Count; ++k)
            //        {
            //            if (pokers[k].power == pokers[i].power + j)
            //            {
            //                isExists = true;
            //                result.Add(pokers[k]);
            //                break;
            //            }
            //        }
            //        if (!isExists)
            //        {
            //            break;
            //        }
            //    }

            //    if (result.Count == length)
            //    {
            //        ret.Add(new Deck(DeckType.ABCDE, result, result[result.Count - 1]));
            //    }
            //}
            //ret.Sort();
            //return ret;

        }
        #endregion

        #region GetAllAAAB 获取所有三带一
        /// <summary>
        /// 获取所有三带一
        /// </summary>
        /// <param name="deck"></param>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static List<Deck> GetAllAAAB(List<Poker> pokers)
        {
            List<Deck> ret = new List<Deck>();
            if (pokers == null) return ret;
            if (pokers.Count < 4) return ret;

            pokers.Sort();

            List<Deck> aaaDeck = GetAllAAA(pokers);
            if (aaaDeck == null || aaaDeck.Count == 0) return ret;

            for (int i = 0; i < aaaDeck.Count; ++i)
            {
                List<Poker> temp = new List<Poker>(aaaDeck[i].pokers);
                Poker mainPoker = aaaDeck[i].mainPoker;
                List<Deck> lst = new List<Deck>();
                lst.AddRange(GetAllAA(pokers));
                lst.AddRange(aaaDeck);
                lst.AddRange(GetAllAAAA(pokers));
                //lst.AddRange(GetAllABCDE(pokers));
                for (int j = 0; j < pokers.Count; ++j)
                {
                    bool isExists = false;
                    for (int k = 0; k < lst.Count; ++k)
                    {
                        if (lst[k].pokers.Contains(pokers[j]))
                        {
                            isExists = true;
                            break;
                        }
                    }
                    if (!isExists)
                    {
                        temp.Add(pokers[j]);
                        break;
                    }
                }
                if (temp.Count < 4)
                {
                    for (int j = 0; j < pokers.Count; ++j)
                    {
                        if (!temp.Contains(pokers[j]))
                        {
                            temp.Add(pokers[j]);
                            break;
                        }
                    }
                }
                ret.Add(new Deck(DeckType.AAAB, temp, mainPoker));
            }
            //ret.Sort();
            return ret;
        }
        #endregion

        #region GetAllAAABB 获取所有三带一对
        /// <summary>
        /// 获取所有三带一对
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        private static List<Deck> GetAllAAABB(List<Poker> pokers)
        {
            List<Deck> ret = new List<Deck>();
            if (pokers == null) return ret;
            if (pokers.Count < 5) return ret;

            pokers.Sort();

            List<Deck> aaaDeck = GetAllAAA(pokers);
            List<Deck> aaDeck = new List<Deck>();
            aaDeck.AddRange(GetAllAA(pokers));
            aaDeck.AddRange(aaaDeck);
            aaDeck.AddRange(GetAllAAAA(pokers));
            if (aaaDeck == null || aaaDeck.Count == 0) return ret;
            if (aaDeck == null || aaDeck.Count == 0) return ret;
            for (int i = 0; i < aaaDeck.Count; ++i)
            {
                for (int j = 0; j < aaDeck.Count; ++j)
                {
                    if (aaDeck[j].mainPoker.size != aaaDeck[i].mainPoker.size)
                    {
                        List<Poker> lst = new List<Poker>();
                        lst.AddRange(aaaDeck[i].pokers);
                        lst.Add(aaDeck[j].pokers[0]);
                        lst.Add(aaDeck[j].pokers[1]);
                        ret.Add(new Deck(DeckType.AAABB, lst, aaaDeck[i].mainPoker));
                    }
                }
            }
            ret.Sort();
            return ret;
        }
        #endregion

        #region GetAllAABBCC 获取所有双顺
        /// <summary>
        /// 获取所有双顺
        /// </summary>
        /// <param name="pokers"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static List<Deck> GetAllAABBCC(List<Poker> pokers, int length = 3)
        {
            List<Deck> ret = new List<Deck>();
            if (pokers == null) return ret;
            if (pokers.Count < length * 2) return ret;

            List<Deck> lst = GetAllAA(pokers);
            if (lst.Count < length) return ret;
            for (int i = 0; i < lst.Count; ++i)
            {
                if (lst[i].mainPoker.power + length > 15) continue;
                List<Poker> temp = new List<Poker>();
                for (int j = 0; j < length; ++j)
                {
                    for (int k = 0; k < lst.Count; ++k)
                    {
                        if (lst[k].mainPoker.power - j == lst[i].mainPoker.power)
                        {
                            temp.AddRange(lst[k].pokers);
                            break;
                        }
                    }
                }
                if (temp.Count == length * 2)
                {
                    //Poker mainPoker = temp[temp.Count - 1];
                    //for (int j = temp.Count - 1; j >= 0; j--)
                    //{
                    //    if (!temp[j].isUniversal)
                    //    {
                    //        int addNum = length / 2 - j / 2;
                    //        mainPoker = new Poker(temp[j].color, temp[j].size + addNum);
                    //        break;
                    //    }
                    //}
                    if (!temp[temp.Count - 1].isUniversal)
                    {
                        ret.Add(new Deck(DeckType.AABBCC, temp, temp[temp.Count - 1]));
                    }
                    else
                    {
                        ret.Add(new Deck(DeckType.AABBCC, temp, temp[temp.Count - 2]));
                    }
                }
                else
                {
                    //int universalCount = 0;

                    //List<Poker> lstUniversalPoker = new List<Poker>();

                    //for (int j = 0; j < pokers.Count; j++)
                    //{
                    //    if (pokers[j].isUniversal)
                    //    {
                    //        lstUniversalPoker.Add(pokers[j]);
                    //    }
                    //}

                    //for (int j = 0; j < temp.Count; j++)
                    //{
                    //    if (temp[j].isUniversal)
                    //    {
                    //        lstUniversalPoker.Remove(temp[j]);
                    //    }
                    //}

                    //for (int j = 0; j < lstUniversalPoker.Count; j++)
                    //{
                    //    if (lstUniversalPoker[j].isUniversal)
                    //    {
                    //        universalCount++;
                    //    }
                    //}

                    //if (temp.Count + universalCount >= length * 2)
                    //{
                    //    if (temp[temp.Count - 1].power > )
                    //    {

                    //    }
                    //}

                }
            }
            ret.Sort();
            return ret;
        }
        #endregion

        #region GetAllAAABBB 获取所有三顺
        /// <summary>
        /// 获取所有三顺
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        private static List<Deck> GetAllAAABBB(List<Poker> pokers, int length = 2)
        {
            List<Deck> ret = new List<Deck>();
            if (pokers == null) return ret;
            if (pokers.Count < length * 3) return ret;

            List<Deck> lst = GetAllAAA(pokers);
            if (lst.Count < length) return ret;

            List<Poker> temp = new List<Poker>();
            for (int i = 0; i < lst.Count; ++i)
            {
                if (lst[i].mainPoker.power + length > 14) continue;
                temp.Clear();
                for (int j = 0; j < length; ++j)
                {
                    for (int k = 0; k < lst.Count; ++k)
                    {
                        if (lst[k].mainPoker.size - j == lst[i].mainPoker.size)
                        {
                            temp.AddRange(lst[k].pokers);
                            break;
                        }
                    }
                }
                if (temp.Count == length * 3)
                {
                    ret.Add(new Deck(DeckType.AAABBB, new List<Poker>(temp), temp[temp.Count - 1]));
                }
            }
            ret.Sort();
            return ret;
        }
        #endregion

        #region GetAllAAABBBCD 获取所有飞机三带一
        /// <summary>
        /// 获取所有飞机三带一
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        private static List<Deck> GetAllAAABBBCD(List<Poker> pokers, int length = 2)
        {
            List<Deck> ret = new List<Deck>();
            if (pokers == null) return ret;
            if (pokers.Count < length * 4) return ret;

            List<Deck> lst = GetAllAAA(pokers);

            if (lst.Count < length) return ret;

            List<Poker> temp = new List<Poker>();

            List<Deck> allDeck = new List<Deck>();
            allDeck.AddRange(GetAllAA(pokers));
            allDeck.AddRange(GetAllAAA(pokers));
            allDeck.AddRange(GetAllAAAA(pokers));
            allDeck.AddRange(GetAllABCDE(pokers));

            Poker mainPoker = null;
            for (int i = 0; i < lst.Count; ++i)
            {
                if (lst[i].mainPoker.power + length > 15) continue;
                temp.Clear();
                for (int j = 0; j < length; ++j)
                {
                    for (int k = 0; k < lst.Count; ++k)
                    {
                        if (lst[k].mainPoker.power - j == lst[i].mainPoker.power)
                        {
                            temp.AddRange(lst[k].pokers);
                            mainPoker = lst[k].mainPoker;
                        }
                    }
                }

                int aCount = 0;

                if (aCount < length)
                {
                    for (int l = 0; l < pokers.Count; ++l)
                    {
                        if (!temp.Contains(pokers[l]))
                        {
                            bool isExists = false;

                            for (int j = 0; j < allDeck.Count; ++j)
                            {
                                if (allDeck[j].pokers.Contains(pokers[l]))
                                {
                                    isExists = true;
                                    break;
                                }
                            }

                            if (!isExists)
                            {
                                temp.Add((pokers[l]));
                                aCount++;
                                if (aCount == length)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                if (aCount < length)
                {
                    for (int l = 0; l < pokers.Count; ++l)
                    {
                        if (!temp.Contains(pokers[l]))
                        {
                            temp.Add((pokers[l]));
                            aCount++;
                            if (aCount == length)
                            {
                                break;
                            }
                        }
                    }
                }

                if (temp.Count == length * 4)
                {
                    ret.Add(new Deck(DeckType.AAABBBCD, new List<Poker>(temp), mainPoker));
                }
            }
            ret.Sort();
            return ret;
        }
        #endregion

        #region GetAllAAABBBCCDD 获取所有飞机三带一对
        /// <summary>
        /// 获取所有飞机三带一对
        /// </summary>
        /// <param name="pokers"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static List<Deck> GetAllAAABBBCCDD(List<Poker> pokers, int length = 2)
        {
            List<Deck> ret = new List<Deck>();
            if (pokers == null) return ret;
            if (pokers.Count < length * 5) return ret;

            List<Deck> aaaDeck = GetAllAAA(pokers);
            if (aaaDeck.Count < length) return ret;
            List<Deck> aaDeck = new List<Deck>();
            aaDeck.AddRange(GetAllAA(pokers));
            aaDeck.AddRange(aaaDeck);
            aaDeck.AddRange(GetAllAAAA(pokers));
            if (aaaDeck == null || aaaDeck.Count == 0) return ret;
            if (aaDeck == null || aaDeck.Count == 0) return ret;

            List<Poker> temp = new List<Poker>();
            Poker mainPoker = null;
            for (int i = 0; i < aaaDeck.Count; ++i)
            {
                if (aaaDeck[i].mainPoker.power + length > 14) continue;
                temp.Clear();
                for (int j = 0; j < length; ++j)
                {
                    for (int k = 0; k < aaaDeck.Count; ++k)
                    {
                        if (aaaDeck[k].mainPoker.size - j == aaaDeck[i].mainPoker.size)
                        {
                            temp.AddRange(aaaDeck[k].pokers);
                            mainPoker = aaaDeck[k].mainPoker;
                            break;
                        }
                    }
                }

                int aaCount = 0;

                for (int l = 0; l < aaDeck.Count; ++l)
                {
                    if (!temp.Contains(aaDeck[l].pokers[0]))
                    {
                        temp.Add(aaDeck[l].pokers[0]);
                        temp.Add(aaDeck[l].pokers[1]);
                        aaCount++;
                        if (aaCount == length)
                        {
                            break;
                        }
                    }
                }

                if (temp.Count == length * 5)
                {
                    ret.Add(new Deck(DeckType.AAABBBCCDD, new List<Poker>(temp), mainPoker));
                }
            }
            ret.Sort();
            return ret;
        }
        #endregion

        #region GetAllAAAABC 获取所有四带二
        /// <summary>
        /// 获取所有四带二
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        private static List<Deck> GetAllAAAABC(List<Poker> pokers)
        {
            List<Deck> ret = new List<Deck>();
            if (pokers == null) return ret;
            if (pokers.Count < 6) return ret;

            List<Deck> lst = GetAllAAAA(pokers);
            if (lst.Count == 0) return ret;

            for (int i = 0; i < lst.Count; ++i)
            {
                List<Poker> temp = new List<Poker>();
                Poker mainPoker = lst[i].mainPoker;
                temp.AddRange(lst[i].pokers);
                List<Deck> allDeck = new List<Deck>();
                allDeck.AddRange(GetAllAA(pokers));
                allDeck.AddRange(GetAllAAA(pokers));
                allDeck.AddRange(GetAllAAAA(pokers));
                allDeck.AddRange(GetAllABCDE(pokers));
                for (int j = 0; j < pokers.Count; ++j)
                {
                    bool isExists = false;
                    for (int k = 0; k < allDeck.Count; ++k)
                    {
                        if (allDeck[k].pokers.Contains(pokers[j]) || temp.Contains(pokers[j]))
                        {
                            isExists = true;
                            break;
                        }
                    }
                    if (!isExists)
                    {
                        temp.Add(pokers[j]);
                        if (temp.Count == 6) break;
                    }
                }

                if (temp.Count < 6)
                {
                    for (int j = 0; j < pokers.Count; ++j)
                    {
                        if (!temp.Contains(pokers[j]))
                        {
                            temp.Add(pokers[j]);
                            if (temp.Count == 6) break;
                        }
                    }
                }

                if (temp.Count == 6)
                {
                    ret.Add(new Deck(DeckType.AAAABC, pokers, mainPoker));
                }
            }
            ret.Sort();
            return ret;
        }
        #endregion

        #region GetAllAAAABBCC 获取所有四带两对
        /// <summary>
        /// 获取所有四带两对
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        private static List<Deck> GetAllAAAABBCC(List<Poker> pokers)
        {
            List<Deck> ret = new List<Deck>();
            if (pokers == null) return ret;
            if (pokers.Count < 8) return ret;

            List<Deck> aaaaDeck = GetAllAAAA(pokers);
            if (aaaaDeck.Count == 0) return ret;
            List<Deck> aaDeck = new List<Deck>();
            aaDeck.AddRange(GetAllAA(pokers));
            aaDeck.AddRange(GetAllAAA(pokers));
            //aaDeck.AddRange(aaaaDeck);

            for (int i = 0; i < aaaaDeck.Count; ++i)
            {
                List<Poker> temp = new List<Poker>();
                Poker mainPoker = aaaaDeck[i].mainPoker;
                temp.AddRange(aaaaDeck[i].pokers);

                for (int j = 0; j < aaDeck.Count; ++j)
                {
                    if (temp.Contains(aaDeck[j].mainPoker)) continue;
                    temp.Add(aaDeck[j].pokers[0]);
                    temp.Add(aaDeck[j].pokers[1]);

                    if (temp.Count == 8) break;
                }
                if (temp.Count < 8)
                {
                    for (int j = 0; j < aaaaDeck.Count; j++)
                    {
                        for (int k = 0; k < aaaaDeck[j].pokers.Count; k++)
                        {
                            if (temp.Contains(aaaaDeck[j].pokers[k])) continue;
                            temp.Add(aaaaDeck[j].pokers[k]);
                            if (aaaaDeck[j].pokers[k].power > mainPoker.power)
                            {
                                mainPoker = aaaaDeck[j].pokers[k];
                            }
                            if (temp.Count == 8) break;
                        }

                        if (temp.Count == 8)
                        {
                            break;
                        }
                    }
                }
                if (temp.Count == 8)
                {
                    ret.Add(new Deck(DeckType.AAAABBCC, temp, mainPoker));
                    //AppDebug.LogWarning(mainPoker.ToString());
                }
            }
            ret.Sort();
            return ret;
        }
        #endregion

        #region GetAllSS 获取所有双王
        /// <summary>
        /// 获取所有双王
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        private static List<Deck> GetAllSS(List<Poker> pokers)
        {
            List<Deck> ret = new List<Deck>();
            if (pokers == null) return ret;
            if (pokers.Count < 2) return ret;

            Poker s = null;
            Poker S = null;
            for (int i = 0; i < pokers.Count; ++i)
            {
                if (pokers[i].power == 16) s = pokers[i];
                if (pokers[i].power == 17) S = pokers[i];
            }
            if (s != null && S != null)
            {
                ret.Add(new Deck(DeckType.SS, new List<Poker>() { s, S }, S));
            }
            return ret;
        }
        #endregion
    }
}
