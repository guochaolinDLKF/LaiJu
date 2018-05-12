//===================================================
//Author      : DRB
//CreateTime  ：11/24/2017 7:42:36 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaoDeKuai
{
    public static class PaoDeKuaiHelper
    {




        #region Sort 排序
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="lst"></param>
        public static void Sort(List<Poker> lst)
        {
            SimpleSort(lst);
        }
        #endregion


        #region SimpleSort 基本排序
        /// <summary>
        /// 基本排序
        /// </summary>
        /// <param name="lst"></param>
        public static void SimpleSort(List<Poker> lst)
        {
            lst.Sort((Poker card1, Poker card2) =>
            {
                if (card1.size < card2.size)
                {
                    return -1;
                }
                else if (card1.size == card2.size)
                {
                    if (card1.color < card2.color)
                    {
                        return -1;
                    }
                    else if (card1.color == card2.color)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }

                }
                else
                {
                    return 1;
                }
            });
        }
        #endregion

        #region GetPokersInStraight 从列表中找出顺子
        /// <summary>
        /// 从列表中找出顺子
        /// </summary>
        /// <param name="pokerList"></param>
        /// <returns></returns>
        public static List<Poker> GetPokersInStraight(List<Poker> pokerList)
        {
            if(pokerList==null || pokerList.Count==0) return null;

            List<Poker> pokerListClonae = CopyPokerList(pokerList);

            Sort(pokerListClonae);
            List<Poker> singleList = new List<Poker>();
            singleList.Add(pokerListClonae[0]);
            for (int i = 1; i < pokerListClonae.Count; ++i)
            {
                if (pokerListClonae[i].size == pokerListClonae[i - 1].size) continue;
                singleList.Add(pokerListClonae[i]);
            }
            List<Poker> straightList = new List<Poker>();
            for (int i = 0; i < singleList.Count; ++i)
            {
                if (singleList[i].size > 10) break;
                straightList.Clear();
                for (int j = i; j < singleList.Count; ++j)
                {
                    if (singleList[j].size > 14) break;
                    if(singleList[i].size==(singleList[j].size+(i-j))) straightList.Add(singleList[j]);
                }
                if (straightList.Count >= 5) return straightList;
            }


            return null;
        }
        #endregion

        //private static List<List<Poker>> m_HintPokers = new List<List<Poker>>();

        #region PokerIsInList 检测List中是否存在这张牌
        /// <summary>
        ///  检测 List中是否存在这张牌
        /// </summary>
        /// <param name="poker"></param>
        /// <param name="pokerList"></param>
        /// <param name="isNumerical"></param>
        /// <returns></returns>
        public static bool PokerIsInList(Poker poker,List<Poker> pokerList,bool isNumerical=false)
        {
            if (pokerList == null || pokerList.Count == 0 || poker == null) return false;
            if (isNumerical)
            {
                for (int i = 0; i < pokerList.Count; ++i)
                {
                    if (poker.size == pokerList[i].size && poker.color == pokerList[i].color) return true;
                }
            }
            else
            {
                for (int i = 0; i < pokerList.Count; ++i)
                {
                    if (poker == pokerList[i]) return true;
                }
            }
          

            return false;
        }

        #endregion

        #region
        #endregion


        //提示
        #region
        public static void HintPoker()
        {
            //前期处理


            List<Poker> othersPokers = new List<Poker>();//上家出的牌
            List<Poker> handPokers = new List<Poker>();//自己手牌

            //手牌排序  上家出的牌处理 

        }
        #endregion


        #region
        public static HintPokersEntity HintPoker(HintPokersEntity hint, List<Poker> handPokers)
        {
            if (hint.Others==null || hint.Others.PokersType== PokersType.None)
            {
                return HintPokerSingle(hint, handPokers);

            }
            else
            {
                switch (hint.Others.PokersType)
                {
                    case PokersType.None:
                        break;
                    case PokersType.Single:
                        return HintPokerSingle(hint, handPokers);
                    case PokersType.Two:
                        return HintPokerTwo(hint, handPokers);
                    case PokersType.Three:
                        return hint;//三不带不提示
                    case PokersType.ThreeWithTwo:
                        return HintPokerThreeWithTwo(hint, handPokers);
                    case PokersType.Plane:
                        return HintPokerPlane(hint, handPokers);
                    case PokersType.ConsecutivePair:
                        return HintPokerConsecutivePair(hint, handPokers);
                    case PokersType.Straight:
                        return HintPokerStraight(hint, handPokers);
                    case PokersType.FourWithThree:
                        return HintPokerFourWithThree(hint, handPokers);
                    case PokersType.Bomb:
                        return HintPokerBomb(hint, handPokers);
                    default:
                        break;
                }
            }

            return hint;
        }
        #endregion






        /// <summary>
        /// 复制List
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static List<Poker> CopyPokerList(List<Poker> pokers)
        {
            List<Poker> copy = new List<Poker>();
            for (int i = 0; i < pokers.Count; ++i)
            {
                copy.Add(pokers[i]);
            }

            return copy;
        }


        #region  提示 (找到提示具体信息)

        #region HintPokerSingle 关于单张的提示 (新)
        /// <summary>
        /// 对单张提示
        /// </summary>
        /// <param name="hint"></param>
        /// <param name="handPokers"></param>
        /// <returns></returns>
        private static HintPokersEntity HintPokerSingle(HintPokersEntity hint, List<Poker> handPokers)
        {
            List<Poker> CurrHintPoker = hint.CurrHint.Pokers;

            //int currSize = (CurrHintPoker == null || CurrHintPoker.Count == 0) ? hint.Others.CurrSize : CurrHintPoker[0].size;
            int currSize = hint.CurrHintLevel == hint.ExpectHintLevel ? (CurrHintPoker == null || CurrHintPoker.Count == 0) ? hint.Others.CurrSize : CurrHintPoker[0].size : hint.ExpectHintLevel == HintLevel.Bomb ? 0 : hint.Others.CurrSize;

            Debug.Log("关于单张的提示currSize：" + currSize);


            if (hint.ExpectHintLevel == HintLevel.Integer)
            {
                Debug.Log("提示Integer");
                //取整
                for (int i = 0; i < handPokers.Count; ++i)
                {
                    if (handPokers[i].size <= currSize) continue;


                    if (((i + 1) < handPokers.Count && handPokers[i].size != handPokers[i + 1].size) || i == handPokers.Count - 1)
                    {
                        if (i > 0 && handPokers[i].size == handPokers[i - 1].size) continue;

                        //找到了
                        hint.CurrHint.SetInfo(new List<Poker>() { handPokers[i] }, PokersType.Single, handPokers[i].size);
                        hint.CurrHintLevel = HintLevel.Integer;
                        return hint;
                    }
                }

                hint.ExpectHintLevel = HintLevel.SplitPokers;
                return HintPokerSingle(hint, handPokers);

            }
            else if (hint.ExpectHintLevel == HintLevel.SplitPokers)
            {
                Debug.Log("提示SplitPokers");
                for (int i = 0; i < handPokers.Count; i++)
                {
                    if (handPokers[i].size <= currSize) continue;
                    if (((i + 1) < handPokers.Count && handPokers[i].size != handPokers[i + 1].size) || i == handPokers.Count - 1) continue;
                    //CurrHintPoker.Clear();
                    //CurrHintPoker.Add(handPokers[i]);
                    hint.CurrHint.SetInfo(new List<Poker>() { handPokers[i] }, PokersType.Single, handPokers[i].size);
                    hint.CurrHintLevel = HintLevel.SplitPokers;
                    return hint;
                }

                hint.ExpectHintLevel = HintLevel.Bomb;
                return HintPokerSingle(hint, handPokers);
            }
            else
            {
                Debug.Log("提示炸弹");
                currSize = hint.CurrHint.PokersType != PokersType.Bomb ? 0 : CurrHintPoker[0].size;

                return HintPokerBomb(hint, handPokers, currSize);

            }
        }
        #endregion

        #region  HintPokerTwo 关于一对的提示
        /// <summary>
        /// 关于一对的提示
        /// </summary>
        /// <param name="hint"></param>
        /// <param name="handPokers"></param>
        /// <returns></returns>
        public static HintPokersEntity HintPokerTwo(HintPokersEntity hint, List<Poker> handPokers)
        {
            List<Poker> CurrHintPoker = hint.CurrHint.Pokers;
            //int currSize = hint.CurrHintLevel == hint.ExpectHintLevel ? hint.CurrHint.CurrSize : 0;//(CurrHintPoker == null || CurrHintPoker.Count == 0) ? Pokers[0].size : CurrHintPoker[0].size;
            int currSize = hint.CurrHintLevel == hint.ExpectHintLevel ?
                (CurrHintPoker == null || CurrHintPoker.Count == 0) ? hint.Others.CurrSize : CurrHintPoker[0].size : hint.ExpectHintLevel == HintLevel.Bomb ? 0 : hint.Others.CurrSize;
            if (hint.ExpectHintLevel == HintLevel.Integer || hint.ExpectHintLevel == HintLevel.SplitPokers)
            {
                List<Poker> lookingForTwo = LookingForTwo(handPokers, currSize, hint.ExpectHintLevel);
                if (lookingForTwo != null && lookingForTwo.Count > 0)
                {
                    //找到了
                    hint.CurrHint.SetInfo(lookingForTwo, PokersType.Two, lookingForTwo[0].size);
                    hint.CurrHintLevel = hint.ExpectHintLevel;
                    return hint;
                }
                else
                {
                    hint.ExpectHintLevel = hint.ExpectHintLevel == HintLevel.Integer ? HintLevel.SplitPokers : HintLevel.Bomb;
                    return HintPokerTwo(hint, handPokers);
                }

            }
            else
            {
                return HintPokerBomb(hint, handPokers, currSize);
            }

        }
        #endregion

        #region HintPokerThreeWithTwo 关于三带二的提示
        /// <summary>
        /// 关于三带二的提示
        /// </summary>
        /// <param name="hint"></param>
        /// <param name="handPokers"></param>
        /// <returns></returns>
        public static HintPokersEntity HintPokerThreeWithTwo(HintPokersEntity hint, List<Poker> handPokers)
        {
            List<Poker> CurrHintPoker = hint.CurrHint.Pokers;
            //int currSize = hint.CurrHintLevel == hint.ExpectHintLevel ? hint.CurrHint.CurrSize : 0;
            int currSize = hint.CurrHintLevel == hint.ExpectHintLevel ?
                (CurrHintPoker == null || CurrHintPoker.Count == 0) ? hint.Others.CurrSize : CurrHintPoker[0].size : hint.ExpectHintLevel == HintLevel.Bomb ? 0 : hint.Others.CurrSize;
            if (hint.ExpectHintLevel == HintLevel.Integer || hint.ExpectHintLevel == HintLevel.SplitPokers)
            {
                List<Poker> findList = LookingForThreeWithTwo(handPokers, currSize, hint.ExpectHintLevel);
                if (findList != null && findList.Count > 0)
                {
                    //找到了
                    hint.CurrHint.SetInfo(findList, PokersType.ThreeWithTwo, findList[0].size);
                    hint.CurrHintLevel = hint.ExpectHintLevel;
                    return hint;
                }
                else
                {
                    hint.ExpectHintLevel = hint.ExpectHintLevel == HintLevel.Integer ? HintLevel.SplitPokers : HintLevel.Bomb;
                    return HintPokerThreeWithTwo(hint, handPokers);
                }

            }
            else
            {
                return HintPokerBomb(hint, handPokers, currSize);
            }

            //int currSize = (hint.CurrHintPoker == null || hint.CurrHintPoker.Count == 0) ? hint.Pokers[0].size : hint.CurrHintPoker[0].size;
            //List<Poker> lookingForTwo = LookingForThreeWithTwo(handPokers, currSize, hint.CurrHintLevel);

            //if (lookingForTwo != null && lookingForTwo.Count > 0)
            //{
            //    //找到了
            //    return hint;
            //}  
        }
        #endregion

        #region HintPokerConsecutivePair 关于连对的提示
        /// <summary>
        /// 关于连对的提示
        /// </summary>
        /// <param name="hint"></param>
        /// <param name="handPokers"></param>
        /// <returns></returns>
        public static HintPokersEntity HintPokerConsecutivePair(HintPokersEntity hint, List<Poker> handPokers)
        {
            //int currSize = hint.CurrHintLevel == hint.ExpectHintLevel ? hint.CurrHint.CurrSize : 0;
            List<Poker> CurrHintPoker = hint.CurrHint.Pokers;
            int currSize = hint.CurrHintLevel == hint.ExpectHintLevel ?
                (CurrHintPoker == null || CurrHintPoker.Count == 0) ? hint.Others.CurrSize : CurrHintPoker[0].size : hint.ExpectHintLevel == HintLevel.Bomb ? 0 : hint.Others.CurrSize;
            if (hint.ExpectHintLevel == HintLevel.Integer || hint.ExpectHintLevel == HintLevel.SplitPokers)
            {
                List<Poker> findList = LookingForConsecutivePair(handPokers, currSize, hint.Others.Pokers.Count, hint.ExpectHintLevel);
                if (findList != null && findList.Count > 0)
                {
                    //找到了
                    hint.CurrHint.SetInfo(findList, PokersType.ConsecutivePair, findList[0].size);
                    hint.CurrHintLevel = hint.ExpectHintLevel;
                    return hint;
                }
                else
                {
                    hint.ExpectHintLevel = hint.ExpectHintLevel == HintLevel.Integer ? HintLevel.SplitPokers : HintLevel.Bomb;
                    return HintPokerConsecutivePair(hint, handPokers);
                }

            }
            else
            {
                return HintPokerBomb(hint, handPokers, currSize);
            }


        }


        #endregion

        #region HintPokerStraight 关于顺子的提示
        /// <summary>
        /// 关于顺子的提示
        /// </summary>
        /// <param name="hint"></param>
        /// <param name="handPokers"></param>
        /// <returns></returns>
        public static HintPokersEntity HintPokerStraight(HintPokersEntity hint, List<Poker> handPokers)
        {
            //int currSize = hint.CurrHintLevel == hint.ExpectHintLevel ? hint.CurrHint.CurrSize : 0;
            List<Poker> CurrHintPoker = hint.CurrHint.Pokers;
            int currSize = hint.CurrHintLevel == hint.ExpectHintLevel ?
                (CurrHintPoker == null || CurrHintPoker.Count == 0) ? hint.Others.CurrSize : CurrHintPoker[0].size : hint.ExpectHintLevel == HintLevel.Bomb ? 0 : hint.Others.CurrSize;
            if (hint.ExpectHintLevel == HintLevel.Integer || hint.ExpectHintLevel == HintLevel.SplitPokers)
            {
                List<Poker> findList = LookingForStraight(handPokers, currSize, hint.Others.Pokers.Count);
                if (findList != null && findList.Count > 0)
                {
                    //找到了
                    hint.CurrHint.SetInfo(findList, PokersType.Straight, findList[0].size);
                    hint.CurrHintLevel = hint.ExpectHintLevel;
                    return hint;
                }
                else
                {
                    hint.ExpectHintLevel = hint.ExpectHintLevel == HintLevel.Integer ? HintLevel.SplitPokers : HintLevel.Bomb;
                    return HintPokerStraight(hint, handPokers);
                }

            }
            else
            {
                return HintPokerBomb(hint, handPokers, currSize);
            }


        }


        #endregion

        #region HintPoker 关于四带三的提示
        /// <summary>
        /// 关于四带三的提示
        /// </summary>
        /// <param name="hint"></param>
        /// <param name="handPokers"></param>
        /// <returns></returns>
        public static HintPokersEntity HintPokerFourWithThree(HintPokersEntity hint, List<Poker> handPokers)
        {
            //int currSize = hint.CurrHintLevel == hint.ExpectHintLevel ? hint.CurrHint.CurrSize : 0;
            List<Poker> CurrHintPoker = hint.CurrHint.Pokers;
            int currSize = hint.CurrHintLevel == hint.ExpectHintLevel ?
                (CurrHintPoker == null || CurrHintPoker.Count == 0) ? hint.Others.CurrSize : CurrHintPoker[0].size : hint.ExpectHintLevel == HintLevel.Bomb ? 0 : hint.Others.CurrSize;
            if (hint.ExpectHintLevel == HintLevel.Integer || hint.ExpectHintLevel == HintLevel.SplitPokers)
            {
                List<Poker> findList = LookingFourFourWithThree(handPokers, currSize);
                if (findList != null && findList.Count > 0)
                {
                    //找到了
                    hint.CurrHint.SetInfo(findList, PokersType.FourWithThree, findList[0].size);
                    hint.CurrHintLevel = hint.ExpectHintLevel;
                    return hint;
                }
                else
                {
                    hint.ExpectHintLevel = HintLevel.Bomb;
                    return HintPokerFourWithThree(hint, handPokers);
                }

            }
            else
            {
                Debug.Log(" 关于四带三的提示  最后找炸弹");
                Debug.Log("size:" + currSize);
                return HintPokerBomb(hint, handPokers, currSize);
            }


        }

        #endregion

        #region HintPokerPlane 关于飞机的提示

        public static HintPokersEntity HintPokerPlane(HintPokersEntity hint, List<Poker> handPokers)
        {
            //int currSize = hint.CurrHintLevel == hint.ExpectHintLevel ? hint.CurrHint.CurrSize : 0;
            List<Poker> CurrHintPoker = hint.CurrHint.Pokers;
            int currSize = hint.CurrHintLevel == hint.ExpectHintLevel ?
                (CurrHintPoker == null || CurrHintPoker.Count == 0) ? hint.Others.CurrSize : CurrHintPoker[0].size : hint.ExpectHintLevel == HintLevel.Bomb ? 0 : hint.Others.CurrSize;
            if (hint.ExpectHintLevel == HintLevel.Integer || hint.ExpectHintLevel == HintLevel.SplitPokers)
            {
                List<Poker> findList = LookingFourPlane(handPokers, currSize, hint.Others.Pokers.Count, hint.ExpectHintLevel);
                if (findList != null && findList.Count > 0)
                {
                    //找到了
                    hint.CurrHint.SetInfo(findList, PokersType.Plane, findList[0].size);
                    hint.CurrHintLevel = hint.ExpectHintLevel;
                    return hint;
                }
                else
                {
                    hint.ExpectHintLevel = hint.ExpectHintLevel == HintLevel.Integer ? HintLevel.SplitPokers : HintLevel.Bomb;
                    return HintPokerPlane(hint, handPokers);
                }

            }
            else
            {
                return HintPokerBomb(hint, handPokers, currSize);
            }


        }

        #endregion

        #region HintPoker 关于炸弹的提示
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hint"></param>
        /// <param name="handPokers"></param>
        /// <returns></returns>
        public static HintPokersEntity HintPokerBomb(HintPokersEntity hint, List<Poker> handPokers)
        {
            int currSize = hint.CurrHintLevel == hint.ExpectHintLevel ? hint.CurrHint.CurrSize : 0;

            return HintPokerBomb(hint, handPokers, currSize);
        }
        #endregion

        #region  最后提示炸弹
        /// <summary>
        /// 最后提示炸弹
        /// </summary>
        /// <param name="hint"></param>
        /// <param name="handPokers"></param>
        /// <param name="currSize"></param>
        /// <returns></returns>
        private static HintPokersEntity HintPokerBomb(HintPokersEntity hint, List<Poker> handPokers, int currSize)
        {


            List<Poker> bombList = LookingForBomb(handPokers, currSize);
            if (bombList != null)
            {
                hint.CurrHint.SetInfo(bombList, PokersType.Bomb, bombList[0].size);
                hint.CurrHintLevel = HintLevel.Bomb;
                return hint;
            }
            else
            {
                //是否重新提示
                if (hint.CurrHint != null && hint.CurrHint.PokersType != PokersType.None)
                {
                   Debug.Log("没有找到更大的炸弹，但是有比上家出牌大的牌，重新开始找比上家大的牌");
                    hint.Reset();
                    return HintPoker(hint, handPokers);

                }
            }

            Debug.Log("没有找到大于上家的牌");
            return hint;

        }
        #endregion









        #endregion




        #region 寻找符合牌型的最小组合牌

        #region LookingForTwo 寻找一对
        /// <summary>
        /// 寻找一对
        /// </summary>
        /// <param name="handPokers"></param>
        /// <param name="LordSize"></param>
        /// <returns></returns>
        private static List<Poker> LookingForTwo(List<Poker> handPokers, int LordSize, HintLevel hintLevel)
        {

            if (handPokers == null || handPokers.Count < 2) return null;

            //if (hintLevel == HintLevel.Integer)
            //{
            for (int i = 0; i < handPokers.Count - 1; ++i)
            {
                if (handPokers[i].size <= LordSize) continue;
                if (handPokers[i].size == handPokers[i + 1].size)
                {
                    if (hintLevel == HintLevel.Integer)
                    {
                        bool meet = true;
                        for (int j = i + 2; j < handPokers.Count; j++)
                        {
                            if (handPokers[j].size == handPokers[i].size)
                            {
                                i = j;
                                meet = false;
                            }
                        }
                        if (!meet) break;
                    }



                    List<Poker> TwoList = new List<Poker>();
                    for (int j = 0; j < 2; ++j)
                    {
                        TwoList.Add(handPokers[i + j]);
                    }
                    return TwoList;
                }
            }


            //}
            //else if (hintLevel == HintLevel.SplitPokers)
            //{
            //    for (int i = 0; i < handPokers.Count - 1; ++i)
            //    {
            //        if (handPokers[i].size <= LordSize) continue;
            //        if (handPokers[i].size == handPokers[i + 1].size)
            //        {
            //            List<Poker> TwoList = new List<Poker>();
            //            for (int j = 0; j < 2; ++j)
            //            {
            //                TwoList.Add(handPokers[i + j]);
            //            }
            //            return TwoList;
            //        }
            //    }


            //}


            return null;
        }
        #endregion

        #region LookingForThreeWithTwo 寻找三带二
        /// <summary>
        /// 寻找三带二
        /// </summary>
        /// <param name="handPokers"></param>
        /// <param name="LordSize"></param>
        /// <returns></returns>
        private static List<Poker> LookingForThreeWithTwo(List<Poker> handPokers, int LordSize, HintLevel hintLevel)
        {
            if (handPokers == null || handPokers.Count < 5) return null;


            for (int i = 0; i < handPokers.Count - 2; ++i)
            {
                if (handPokers[i].size <= LordSize) continue;
                if (handPokers[i].size == handPokers[i + 1].size && handPokers[i].size == handPokers[i + 2].size)
                {
                    if (hintLevel == HintLevel.Integer)
                    {
                        if ((i + 3) < handPokers.Count && handPokers[i].size == handPokers[i + 3].size)
                        {
                            i = i + 3;
                            continue;
                        }
                    }
                    //---2--任意带-----------------------------------
                    List<Poker> ThreeWithTwo = new List<Poker>();

                    for (int k = 0; k < 3; ++k)
                    {
                        ThreeWithTwo.Add(handPokers[i + k]);
                    }

                    for (int k = 0, j = 0; j < 2 && k < handPokers.Count; ++k)
                    {

                        if (k < i || k > (i + 2))
                        {
                            ThreeWithTwo.Add(handPokers[k]);
                            ++j;
                        }
                    }

                    return ThreeWithTwo;

                }
            }



            return null;
            //-----1------只能带一对-----------------------
            //for (int j = 0; j < handPokers.Count - 1; ++j)
            //{
            //    if (handPokers[j].size == handPokers[i].size) continue;
            //    if (handPokers[j].size == handPokers[j + 1].size)
            //    {
            //        List<Poker> ThreeWithTwo = new List<Poker>();
            //        for (int k = 0; k < 3; ++k)
            //        {
            //            ThreeWithTwo.Add(handPokers[i + k]);
            //        }
            //        for (int k = 0; k < 2; ++k)
            //        {
            //            ThreeWithTwo.Add(handPokers[j + k]);
            //        }
            //        return ThreeWithTwo;

            //    }
            //}
            //return null;
        }
        #endregion

        #region ConsecutivePair 寻找连对
        /// <summary>
        /// 寻找连对
        /// </summary>
        /// <param name="handPokers"></param>
        /// <param name="LordSize"> 最小对 </param>
        /// <param name="ConsecutiveSum"></param>
        /// <returns></returns>
        private static List<Poker> LookingForConsecutivePair(List<Poker> handPokers, int LordSize, int ConsecutiveSum, HintLevel hintLevel)
        {
            if (handPokers == null || handPokers.Count < ConsecutiveSum) return null;
            if ((LordSize + (ConsecutiveSum / 2) - 1) >= 14) return null;


            //区分 HintLevel.Integer HintLevel.SplitPokers


            for (int i = 0; i < handPokers.Count - ConsecutiveSum + 1; ++i)
            {
                if (handPokers[i].size <= LordSize) continue;
                if ((handPokers[i].size + (ConsecutiveSum / 2) - 1) > 14) return null;
                Debug.Log("---------------a----handPokers[i].size----------------------------------"+ handPokers[i].size);
                List<Poker> ConsecutivePairList = new List<Poker>();
                ConsecutivePairList.Add(handPokers[i]);

               

                for (int j = i + 1, k = 1; j < handPokers.Count; ++j)
                {
                    //相同
                    if (handPokers[j].size == (handPokers[i].size + (k / 2)))
                    {
                        //连对下一个成员
                        //++currP;
                        ++k;
                        //如果找到的成员足够 返回
                        ConsecutivePairList.Add(handPokers[j]);
                        Debug.Log("---------b---- ConsecutivePairList.Add(handPokers[currP]);---------------" + handPokers[j].size);
                        if (ConsecutivePairList.Count == ConsecutiveSum) return ConsecutivePairList;
                        //break;
                    }
                    else if (handPokers[j].size > (handPokers[i].size + (k / 2)))
                    {
                        //失败
                        i = j-1;

                        break;
                    }
                    else
                    {
                        //下移
                        continue;
                    }



                }



            }


            return null;
        }
        #endregion

        #region LookingForStraight 寻找顺子
        /// <summary>
        /// 寻找顺子
        /// </summary>
        /// <param name="handPokers"></param>
        /// <param name="LordSize">最小牌 </param>
        /// <param name="ConsecutiveSum"></param>
        /// <returns></returns>
        private static List<Poker> LookingForStraight(List<Poker> handPokers, int LordSize, int ConsecutiveSum)
        {
            if (handPokers == null || handPokers.Count < ConsecutiveSum || (LordSize + ConsecutiveSum) > 14) return null;

            List<Poker> singlePoker = new List<Poker>();
            Poker currPoker = handPokers[0];
            if (handPokers[0].size > LordSize) singlePoker.Add(handPokers[0]);
            for (int i = 1; i < handPokers.Count; ++i)
            {
                if (handPokers[i].size != currPoker.size && handPokers[i].size > LordSize)
                {
                    singlePoker.Add(handPokers[i]);
                    currPoker = handPokers[i];
                }

            }

            if (singlePoker.Count < ConsecutiveSum) return null;
            bool isLooking = true;
            for (int i = 0; i < singlePoker.Count; ++i)
            {
                if ((singlePoker[i].size + ConsecutiveSum - 1) > 14) return null;
                isLooking = true;
                for (int j = 1; j < ConsecutiveSum; ++j)
                {
                    if (singlePoker[i + j].size != (singlePoker[i].size + j))
                    {
                        i = i + j - 1;
                        isLooking = false;
                        break;
                    }
                }
                if (isLooking)
                {
                    //找到了
                    List<Poker> ForStraightList = new List<Poker>();
                    for (int j = 0; j < ConsecutiveSum; j++)
                    {
                        ForStraightList.Add(singlePoker[i + j]);
                    }
                    return ForStraightList;
                }
            }

            return null;
        }
        #endregion

        #region LookingFourPlane 寻找飞机
        /// <summary>
        /// 寻找飞机
        /// </summary>
        /// <param name="handPokers"></param>
        /// <param name="LordSize"></param>
        /// <returns></returns>
        private static List<Poker> LookingFourPlane(List<Poker> handPokers, int LordSize, int pokersCount, HintLevel hintLevel)
        {
            int planeCount = pokersCount / 5;
            if (handPokers == null || handPokers.Count < pokersCount || (LordSize + planeCount - 1) >= 14) return null;



            List<Poker> threePoker = new List<Poker>();
            int sizeP = 0;
            for (int i = 0; i < handPokers.Count - 2; ++i)
            {
                if (handPokers[i].size <= LordSize) continue;
                if (handPokers[i].size == sizeP) continue;
                if ((handPokers[i].size + planeCount - 1) > 14) break;
                if (handPokers[i].size == handPokers[i + 1].size && handPokers[i].size == handPokers[i + 2].size)
                {
                    //找到一个刻

                    //-----取整的区别-------------------------
                    if (hintLevel == HintLevel.Integer)
                    {
                        if ((i + 3) < handPokers.Count && handPokers[i].size == handPokers[i + 3].size)
                        {

                            i = i + 3;
                            continue;
                        }
                    }
                    //---------------------------

                    threePoker.Add(handPokers[i]);
                    sizeP = handPokers[i].size;
                    i = i + 2;


                }
            }

            //找到N连刻
            bool isLooking = true;
            for (int i = 0; i < threePoker.Count - (planeCount-1) ; ++i)
            {
                isLooking = true;
                for (int j = 1; j < planeCount; ++j)
                {
                    if (threePoker[i].size != (threePoker[i + j].size - j))
                    {
                        isLooking = false;
                        i = i + j - 1;
                        break;
                    }
                }
                if (isLooking)
                {
                    List<Poker> planeList = new List<Poker>();
                    List<Poker> copyHandPokers = CopyPokerList(handPokers);

                    for (int j = 0,k=0; j < handPokers.Count-2 && k < planeCount; ++j)
                    {
                        if (handPokers[j].size == threePoker[i + k].size)
                        {
                            planeList.Add(copyHandPokers[j]);
                            planeList.Add(copyHandPokers[j+1]);
                            planeList.Add(copyHandPokers[j+2]);
                            ++k;
                        }

                    }

                    //继续找翅膀

                    for (int j = 0; j < planeList.Count; ++j)
                    {
                        copyHandPokers.Remove(planeList[j]);
                    }
                    int wingsSum = planeCount * 2;
                    for (int j = 0; j < wingsSum; j++)
                    {
                        planeList.Add(copyHandPokers[j]);
                    }

                    return planeList;
                }

            }



            return null;
        }
        #endregion

        #region LookingFourFourWithThree 寻找四带三
        /// <summary>
        /// 寻找四带三
        /// </summary>
        /// <param name="handPokers"></param>
        /// <param name="LordSize"></param>
        /// <returns></returns>
        private static List<Poker> LookingFourFourWithThree(List<Poker> handPokers, int LordSize)
        {
            if (handPokers == null || handPokers.Count < 7) return null;

            for (int i = 0; i < handPokers.Count - 3; ++i)
            {
                if (handPokers[i].size <= LordSize) continue;

                if (handPokers[i].size == handPokers[i + 3].size)
                {
                    List<Poker> FourWithThreeList = new List<Poker>();
                    for (int j = 0; j < 4; ++j)
                    {
                        FourWithThreeList.Add(handPokers[i + j]);
                    }

                    for (int j = 0, k = 0; k < 3 && j < handPokers.Count; ++j)
                    {
                        if (handPokers[j].size != handPokers[i].size)
                        {
                            FourWithThreeList.Add(handPokers[j]);
                            ++k;
                        }
                    }

                    return FourWithThreeList;
                }

            }

            return null;
        }
        #endregion

        #region LookingForBomb 寻找炸弹
        private static List<Poker> LookingForBomb(List<Poker> handPokers, int LordSize)
        {
            if (handPokers == null || handPokers.Count < 4) return null;


            for (int i = 0; i < handPokers.Count-3; ++i)
            {
                if (handPokers[i].size <= LordSize) continue;
                if (handPokers[i].size == handPokers[i + 3].size)
                {
                    List<Poker> BombList = new List<Poker>();
                    for (int j=0; j<4; ++j)
                    {
                        BombList.Add(handPokers[i+j]);

                    }
                    return BombList;
                }
            }
            //Poker currPoker = null;
            //int sameSum = 1;
            //for (int i = 0; i < handPokers.Count - 3; ++i)
            //{
            //    if (handPokers[i].size <= LordSize) continue;
            //    currPoker = handPokers[i];
            //    for (int j = i + 1; (j - i) <= 3 && j < handPokers.Count; ++j)
            //    {
            //        if (handPokers[j].size == currPoker.size)
            //        {
            //            ++sameSum;
            //        }
            //        else
            //        {
            //            i = j;
            //            sameSum = 1;
            //            break;
            //        }
            //    }
            //    if (sameSum == 4)
            //    {
            //        List<Poker> BombList = new List<Poker>();
            //        for (int j = i; (j - i) <= 3 && j < handPokers.Count; ++j)
            //        {
            //            BombList.Add(handPokers[j]);

            //        }
            //        return BombList;
            //    }

            //}

            return null;
        }
        #endregion

        #endregion





        #region 根据输入的组合牌 判断出其牌型

        #region CheckPokerType 检测牌型
        /// <summary>
        /// 检测牌型
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public static CombinationPokersEntity CheckPokerType(CombinationPokersEntity combinationPokers)
        {

            List<Poker> pokers = combinationPokers.Pokers;


            if (pokers == null || pokers.Count == 0)
            {
                Debug.Log("检测牌型：null 或 Count = 0");

                combinationPokers.PokersType = PokersType.None;

            }
            if (pokers.Count == 1)
            {
                //单
                combinationPokers.PokersType = PokersType.Single;
                combinationPokers.CurrSize = pokers[0].size;
                return combinationPokers;
            }
            else if (pokers.Count == 2)
            {
                //一对
                CheckIsTwo(combinationPokers);
            }
            else if (pokers.Count == 3)
            {
                //3不带
                CheckIsThree(combinationPokers);
            }
            else if (pokers.Count == 4)
            {
                //炸弹 连对
                CheckIsBomb(combinationPokers);
                if (combinationPokers.PokersType == PokersType.None) CheckIsConsecutivePair(combinationPokers);
            }
            else if (pokers.Count == 5)
            {
                //顺子 3带二
                CheckIsStraight(combinationPokers);
                if (combinationPokers.PokersType == PokersType.None) CheckIsThreeWithTwo(combinationPokers);

            }
            else if (pokers.Count == 7)
            {
                //顺子 4带3
                CheckIsStraight(combinationPokers);
                if (combinationPokers.PokersType == PokersType.None) CheckIsFourWithThree(combinationPokers);

            }
            else
            {
                //顺子 连队 飞机
                CheckIsStraight(combinationPokers);
                if (combinationPokers.PokersType == PokersType.None) CheckIsConsecutivePair(combinationPokers);
                if (combinationPokers.PokersType == PokersType.None) CheckIsSteelPlane(combinationPokers);

            }

            if (combinationPokers.PokersType == PokersType.None)
                Debug.Log("检测牌型：非法牌型");


            return combinationPokers;

        }
        #endregion



        #region CheckIsTwo 检测是否是一对
        /// <summary>
        /// 检测是否是一对
        /// </summary>
        /// <param name="ordinary"></param>
        /// <returns></returns>
        private static CombinationPokersEntity CheckIsTwo(CombinationPokersEntity pokers)
        {
            List<Poker> ordinary = pokers.Pokers;

            if (ordinary == null || ordinary.Count != 2) return pokers;

            if (ordinary[0].size == ordinary[1].size )
            {
                pokers.CurrSize = ordinary[0].size;
                pokers.PokersType = PokersType.Two;
            }


            return pokers;

        }
        #endregion

        #region CheckIsThreeEven 检测是否是连对
        /// <summary>
        /// 检测是否是连对
        /// </summary>
        /// <param name="ordinary"></param>
        /// <param name="universalSum"></param>
        /// <returns></returns>
        private static CombinationPokersEntity CheckIsConsecutivePair(CombinationPokersEntity pokers)
        {
            List<Poker> ordinary = pokers.Pokers;
            if ((ordinary.Count % 2) != 0 || ordinary.Count < 4 || ordinary[ordinary.Count - 1].size > 14) return pokers;

            for (int i = 1; i < ordinary.Count; ++i)
            {
                if ((i % 2) == 0)
                {
                    if (ordinary[i].size != (ordinary[i - 1].size + 1) ) return pokers;
                }
                else
                {
                    if (ordinary[i].size != ordinary[i - 1].size) return pokers;
                }
            }

            pokers.CurrSize = ordinary[0].size;
            pokers.PokersType = PokersType.ConsecutivePair;

            return pokers;


        }
        #endregion

        #region CheckIsThree 检测是否是三不带 
        /// <summary>
        /// 检测是否是三不带
        /// </summary>
        /// <param name="ordinary"></param>
        /// <returns></returns>
        private static CombinationPokersEntity CheckIsThree(CombinationPokersEntity pokers)
        {
            List<Poker> ordinary = pokers.Pokers;
            if ((ordinary.Count) != 3) return pokers;


            for (int i = 1; i < ordinary.Count; ++i)
            {
                if (ordinary[i].size != ordinary[0].size) return pokers;
            }
            pokers.CurrSize = ordinary[0].size;
            pokers.PokersType = PokersType.Three;

            return pokers;

        }
        #endregion

        #region CheckIsThreeWithTwo 检测是否是三带二  
        /// <summary>
        ///  检测是否是三带二
        /// </summary>
        /// <param name="ordinary"></param>
        /// <param name="universalSum"></param>
        /// <returns></returns>
        private static CombinationPokersEntity CheckIsThreeWithTwo(CombinationPokersEntity pokers)
        {
            List<Poker> ordinary = pokers.Pokers;
            if ((ordinary.Count) != 5) return pokers;

            int sameSum = 0;
            for (int i = 0; i < ordinary.Count; i++)
            {
                if (ordinary[i].size == ordinary[2].size) ++sameSum;
            }

            if (sameSum == 3)
            {
                pokers.CurrSize = ordinary[2].size;
                pokers.PokersType = PokersType.ThreeWithTwo;
                return pokers;
            }


            return pokers;

        }
        #endregion

        #region CheckIsSteelPlane 检测是否是飞机

        /// <summary>
        /// 检测是否是飞机
        /// </summary>
        /// <param name="ordinary"></param>
        /// <returns> 最小有效牌 </returns>
        private static CombinationPokersEntity CheckIsSteelPlane(CombinationPokersEntity pokers)
        {
            List<Poker> ordinary = pokers.Pokers;
            if (ordinary == null || ordinary.Count == 0 || (ordinary.Count % 5) != 0) return pokers;

            List<Poker> planePoker = new List<Poker>();


            Poker currPoker = null;
            for (int i = 0; i < ordinary.Count ; ++i)
            {
                if (ordinary[i].size > 14) continue;

                currPoker = ordinary[i];

                int sameSum = 0;
                for (int j = i; j < ordinary.Count; ++j)
                {
                    if (ordinary[j].size == currPoker.size)
                    {
                        ++sameSum;
                    }
                    else
                    {
                        i = j -1;
                        break;
                    }
                }
                if (sameSum >= 3) planePoker.Add(currPoker);

                sameSum = 0;

            }

            //--------------找到符合牌数量的连
            int evenSum = ordinary.Count / 5;

            int currEvenSum = 1;
            for (int i = planePoker.Count - 1; i > 0; --i)
            {

                if (planePoker[i].size == (planePoker[i - 1].size + 1))
                {
                    ++currEvenSum;
                    if (currEvenSum == evenSum)
                    {
                        //成功找到飞机
                        pokers.CurrSize = planePoker[0].size;
                        pokers.PokersType = PokersType.Plane;

                        return pokers;
                    }
                }
                else
                {
                    currEvenSum = 1;
                }

            }






            ////找到最大3张连

            //Poker currThreePoker = null;
            //for (int i = 0; i < planePoker.Count - 1; ++i)
            //{

            //    currThreePoker = planePoker[i];

            //    int sameSum = 0;
            //    int sameMax = 0;
            //    for (int j = i; j < planePoker.Count ; ++j)
            //    {
            //        if (planePoker[j].size == currPoker.size + j - i)
            //        {
            //            ++sameSum;
            //        }
            //        else
            //        {
            //            if (sameSum == 3) planePoker.Add(currPoker);

            //            sameSum = 0;
            //            if (sameSum >= sameMax) sameMax = sameSum;
            //            i = j + 1;
            //            break;
            //        }

            //    }

            //}



            return pokers;
        }
        #endregion

        #region CheckIsStraight 检测是否是顺子

        /// <summary>
        /// 检测是否是顺子
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        private static CombinationPokersEntity CheckIsStraight(CombinationPokersEntity pokers)
        {
            List<Poker> ordinary = pokers.Pokers;
            if (ordinary == null || ordinary.Count < 5 || ordinary[ordinary.Count - 1].size > 14) return pokers;


            for (int i = 0; i < ordinary.Count - 1; i++)
            {
                if (ordinary[i].size != (ordinary[i + 1].size - 1)) return pokers;
            }

            pokers.CurrSize = ordinary[0].size;
            pokers.PokersType = PokersType.Straight;
            return pokers;
        }
        #endregion

        #region CheckIsStraight 检测是否是四带三

        /// <summary>
        /// 检测是否是四带三
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        private static CombinationPokersEntity CheckIsFourWithThree(CombinationPokersEntity pokers)
        {
            List<Poker> ordinary = pokers.Pokers;
            if (ordinary == null || ordinary.Count != 7) return pokers;
            int sameSum = 0;
            for (int i = 0; i < ordinary.Count; ++i)
            {
                if (ordinary[i].size == ordinary[3].size) ++sameSum;
            }

            if (sameSum == 4)
            {
                pokers.CurrSize = ordinary[3].size;
                pokers.PokersType = PokersType.FourWithThree;
            }

            return pokers;

        }
        #endregion

        #region CheckIsBomb 检测是否是炸弹
        /// <summary>
        /// 检测是否是炸弹
        /// </summary>
        /// <param name="ordinary"></param>
        /// <returns></returns>
        private static CombinationPokersEntity CheckIsBomb(CombinationPokersEntity pokers)
        {
            List<Poker> ordinary = pokers.Pokers;
            if (ordinary == null || ordinary.Count != 4) return pokers;
            for (int j = 0; j < ordinary.Count - 1; ++j)
            {
                if (ordinary[j].size != ordinary[j + 1].size)
                {
                    return pokers;
                }
            }

            pokers.CurrSize = ordinary[0].size;
            pokers.PokersType = PokersType.Bomb;
            return pokers;
        }
        #endregion

        #endregion


        /// <summary>
        /// 检测是否存在黑桃3
        /// </summary>
        /// <returns></returns>
        public static bool CheckThree(List<Poker> pokerList)
        {
            for (int i = 0; i < pokerList.Count; ++i)
            {
                if (pokerList[i].size == 3 && pokerList[i].color == 4) return true;
            }
            return false;
        }



    }



    /// <summary>
    /// 跑的快所有牌型
    /// </summary>
    public enum PokersType
    {
        None,//散牌
        Single,//单张
        Two,//一对
        Three,//三不带
        ThreeWithTwo,//三带二
        Plane,//飞机
        ConsecutivePair,//连对
        Straight,//顺子
        //StraightFlush,//同花顺
        FourWithThree,//四带三
        Bomb,//炸弹

    }

    /// <summary>
    /// 提示的优先程度
    /// </summary>
    public enum HintLevel
    {
        Integer,//取整
        SplitPokers,//拆牌
        UniversalGroup,//万能组合
        Bomb,//炸弹
    }

}