//===================================================
//Author      : WZQ
//CreateTime  ：11/6/2017 11:26:46 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuanDan
{
    public class SeatCtrl : MonoBehaviour
    {
        // 将手牌以Size区分
        private Dictionary<int, List<Poker>> m_handDic;

        private List<Poker> m_universalList;

        /// <summary>
        /// 出牌提示
        /// </summary>
        /// <param name="lastPokers"></param>
        /// <param name="handPokers"></param>
        /// <returns></returns>
        private List<Poker> PlayPokerPrompt(List<Poker> lastPokers,List<Poker> handPokers, PokersType lastType)
        {
            if (lastPokers == null || lastPokers.Count == 0)
            {
                return null; 
            }

            //炸弹优先级 1 整炸 2 组炸  3 更大级别炸弹

            //单张： 不提示万能牌  优先级别 1 提示单个  2 多牌拆出 4 炸弹...

            //一对：  1 优先提示双牌 2 拆出双牌(只能从3张中拆出) 3 万能组牌 4 炸弹...

            //三不带： 1 优先整牌 2 万能组牌 3 拆出3张 4 炸弹...

            //三带二： 同上 额外注意挑选一对

            //三连对：1 整牌(不论拆牌) 2 万能组牌 3 炸弹...

            //钢板： 1 整牌(不论拆牌) 2 万能组牌 3 炸弹...

            //顺子： 1 整牌(非同花顺，不论拆牌) 2 万能组牌 3 炸弹...

   
            return null;
        }

        #region 
        #endregion

        #region 找牌

        private Poker ExistPoker(int size,int color)
        {
            if (!m_handDic.ContainsKey(size)) return null;
            for (int i = 0; i < m_handDic[size].Count; ++i)
            {
                if (m_handDic[size][i].color == color) return m_handDic[size][i];
            }
            return null;
        }





        #region  LookingForSingle 找大于某牌的单牌
        /// <summary>
        /// 找大于某牌的单牌
        /// </summary>
        /// <param name="poker"></param>
        /// <param name="LordSize"></param>
        /// <returns></returns>
        private Poker LookingForSingle(int currSize,int LordSize)
        {
            if (currSize == LordSize)
            {
                for (int i = 15; i <= 16; ++i)
                {
                    if (m_handDic[LordSize].Count == 1) return m_handDic[LordSize][0];
                }
                for (int i = 15; i <= 16; ++i)
                {
                    if (m_handDic[LordSize].Count > 0) return m_handDic[LordSize][0];
                }


            }
            else if (currSize > 14)
            {
                for (int i = currSize + 1; i <= 16; ++i)
                {
                    if (m_handDic[i].Count > 0) return m_handDic[i][0];
                }
            }
            else
            {
                for (int i = currSize + 1; i <= 14; ++i)
                {
                    if ( i == LordSize) continue;
                    if (m_handDic[i].Count == 1) return m_handDic[i][0];
                }

                if (m_handDic[LordSize].Count == 1) return m_handDic[LordSize][0];

                for (int i = 15; i <= 16; ++i)
                {
                    if (m_handDic[LordSize].Count == 1) return m_handDic[LordSize][0];
                }

                //拆
                for (int i = currSize; i <= 14; i++)
                {
                    if (i == LordSize) continue;
                    if (m_handDic[i].Count > 0) return m_handDic[i][0];
                }

                if (m_handDic[LordSize].Count > 0) return m_handDic[LordSize][0];

                for (int i = 15; i <= 16; ++i)
                {
                    if (m_handDic[LordSize].Count > 0) return m_handDic[LordSize][0];
                }

            }
          
            return null;
        }
        #endregion


        //找一对



        #region LookingForSmallBomb 找小于同花顺的炸弹
        /// <summary>
        /// 找小于同花顺的炸弹
        /// </summary>
        /// <param name="currSize"></param>
        /// <param name="LordSize"></param>
        /// <returns></returns>
        private List<Poker> LookingForSmallBomb(int currBombSize, int currLength, int LordSize)
        {
            if (currBombSize < 2) currBombSize = 1;
            for (int i = currBombSize + 1; i <= 14; ++i)
            {
                if (i == LordSize) continue;
                if (m_handDic[i].Count == currLength) return m_handDic[i];
            }

            if (m_handDic[LordSize].Count == currLength) return m_handDic[LordSize];

            //组
            if (m_universalList.Count > 0)
            {
                for (int k = 1; k <= m_universalList.Count; k++)
                {

                    for (int i = currBombSize + 1; i <= 14; ++i)
                    {
                        if (i == LordSize) continue;
                        if ((m_handDic[i].Count + k) == currLength) return m_handDic[i];
                    }

                    if ((m_handDic[LordSize].Count + k) == currLength) return m_handDic[LordSize];

                }

            }

            //拆
            for (int k = 1; k <= m_universalList.Count; k++)
            {

                for (int i = currBombSize + 1; i <= 14; ++i)
                {
                    if (i == LordSize) continue;
                    if (m_handDic[i].Count > currLength) return m_handDic[i];
                }

                if (m_handDic[LordSize].Count > currLength) return m_handDic[LordSize];

            }



            if (currLength == 4) return LookingForSmallBomb(0, 5, LordSize);
            return null;
        }
        #endregion

        /// <summary>
        ///  找同花顺  以最小牌为基准
        /// </summary>
        /// <param name="currStraightFlush"></param>
        /// <returns></returns>
        private List<Poker> LookingForStraightFlush(int currStraightFlush, List<Poker> ordinary, int universalSum)
        {
            if (currStraightFlush < 0) currStraightFlush = 0;

            //------1-------------按照当前手牌查找------------------------------------------

            if (currStraightFlush > 10) return null;

            // ---优先----查找 A 2 3 4 5 

            if (currStraightFlush == 0)
            {
                //黑红梅方
                //for (int i = 1; i < 5; i++)
                //{

                //}


                //Poker poker = ExistPoker(14, ordinary[i].color);

                //for (int j = 1; j < 5; j++)
                //{
                //    Poker poker = ExistPoker(ordinary[i].size + j, ordinary[i].color);
                //    if (poker == null) continue;
                //}
            }


            for (int i = 0; i < ordinary.Count; i++)
            {
                if (ordinary[i].size > 10) break;
                if (ordinary[i].size < currStraightFlush || (ordinary[i].size == ordinary[i - 1].size && ordinary[i].color == ordinary[i - 1].color) ) continue;

                for (int j = 1; j < 5; j++)
                {
                  Poker poker= ExistPoker(ordinary[i].size + j, ordinary[i].color);
                    if (poker == null) continue;
                }
            }

            


            //补

            //从后向前补 1 - 10
            //  补特殊的 J  Q  

            // ----优先---查找 A 2 3 4 5 

            for (int i = 0; i < ordinary.Count; i++)
            {
                if (ordinary[i].size > 12) break;
                int x = ordinary[i].size > 10 ? (5 + 10 - ordinary[i].size) : 5;
                //int universalSumClone = ordinary[i].size > 10 ? (universalSum + 10 - ordinary[i].size) : universalSum; ;
                for (int j = 1; j < x; j++)
                {
                    if (i > 0 && ordinary[i].size == ordinary[i - 1].size && ordinary[i].color == ordinary[i - 1].color) continue;

                    Poker poker = ExistPoker(ordinary[i].size + j, ordinary[i].color);
                    if (poker == null) --universalSum;
                    if (universalSum < 0) break;

                }
            }




            //------2-------------从12345 -- 10JQKA  全部查找------------------------------------------
            int universalSumClone = universalSum;

            //整

            //组
            if (currStraightFlush == 0)
            {
                //找 12345
                //区分花色
                for (int j = 1; j <= 4; ++j)
                {
                    universalSumClone = universalSum;

                   if(ExistPoker(14, j) ==null) --universalSumClone;

                    //顺子长度
                    for (int k = 2; k <= 5 ; ++k)
                    {
                        if (ExistPoker(k, j) == null) --universalSumClone;
                        if (universalSumClone >= 0)
                        {
                            //返回列表

                        }
                    }
                }


            }

            if (currStraightFlush > 0)
            {
                //找 23456 - 10JQKA
                for (int i = currStraightFlush + 1; i <= 10; ++i)
                {
                    //区分花色
                    for (int j  = 1; j <= 4; ++j)
                    {
                        universalSumClone = universalSum;
                        //顺子长度
                        for (int k = 0; k < 5; ++k)
                        {
                            Poker poker = ExistPoker(i + k, j);
                            if (poker == null) --universalSumClone;

                            if (universalSumClone >= 0)
                            {
                                //返回列表

                            }
                        }
                    }

                }
            }




            return null;
        }
        #region  LookingForFourKings 找四王
        /// <summary>
        /// 找四王
        /// </summary>
        /// <returns></returns>
        private List<Poker> LookingForFourKings()
        {
            if (m_handDic[14].Count == 4) return m_handDic[14];          
            return null;
        }
        #endregion

        #endregion


        #region GetPokerType 判断牌型
        /// <summary>
        /// 判断牌型
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        private PokersType GetPokerType(List<Poker> pokers ,out int maxSize)
        {

            
            //排序？？



            List<Poker> ordinary = new List<Poker>();

            for (int i = 0; i < pokers.Count; ++i)
            {
                if (!pokers[i].isUniversal) ordinary.Add(pokers[i]);
            }

            int universalSum = pokers.Count - ordinary.Count;
            maxSize = ordinary.Count > 0 ? ordinary[ordinary.Count - 1].size : pokers[0].size;


            if (pokers.Count == 1)
            {
                return PokersType.Single;
            }
            else if (pokers.Count == 2)
            {
                if (pokers[0].size == pokers[1].size || ordinary.Count < 2)
                {
                   
                    return PokersType.Two;
                }
            }
            else if (pokers.Count == 3)
            {
                bool isThree = true;
                for (int i = 0; i < ordinary.Count - 1; i++)
                {
                    if(ordinary[i].size != ordinary[i + 1].size) isThree = false;
                }
                if (isThree)
                {
                    maxSize = ordinary[0].size;
                    return PokersType.Three;
                }
            }
            else if (pokers.Count == 4)
            {
                if (pokers[0].size > 14) return PokersType.FourKings;

                if(CheckIsBomb(ordinary) != null)  return PokersType.Bomb;

            }
            else if (pokers.Count == 5)
            {
                if (CheckIsBomb(ordinary) != null) return PokersType.Bomb;//炸弹

                //三带二  顺子  炸弹 同花顺

                Poker poker = CheckIsStraight(pokers);
                if (poker != null) maxSize = poker.size;

                bool isStraight = poker != null;
                bool isFlush = true;

                //是否是同花
                for (int j = 1; j < ordinary.Count; ++j)
                {
                    if (ordinary[0].color != ordinary[j].color) isFlush = false;
                }

                if(isStraight && isFlush)
                    return PokersType.StraightFlush;
                else if(isStraight)
                    return PokersType.Straight;

                //是否是三带二
                int differentSizeSum = 0;
                if (ordinary.Count > 0) ++differentSizeSum;
                for (int i = 1; i < ordinary.Count; ++i)
                {
                    if (ordinary[0].size != ordinary[i].size) ++differentSizeSum;
                }
                if(differentSizeSum == 2) return PokersType.ThreeWithTwo;

            }
            else if (pokers.Count == 6)
            {
                //三连对 钢板 炸弹 
                if (CheckIsBomb(ordinary) != null) return PokersType.Bomb;//炸弹

                 if(CheckIsSteelPlate(ordinary, universalSum) !=null) return PokersType.SteelPlate;
                if (CheckIsThreeEven(ordinary, universalSum) != null) return PokersType.ThreeEven;
                
            }
            else if (pokers.Count > 6)
            {
                if (CheckIsBomb(ordinary) != null) return PokersType.Bomb;//炸弹
            }

            return PokersType.HighCard;
        }
        #endregion


        #region CheckIsFourKings 检测是否是四王
        /// <summary>
        /// 检测是否是四王
        /// </summary>
        /// <param name="ordinary"></param>
        /// <param name="universalSum"></param>
        /// <returns></returns>
        private Poker CheckIsFourKings(List<Poker> ordinary, int universalSum)
        {
            if (ordinary[0].size <= 14 || universalSum > 0) return null;
            return ordinary[ordinary.Count - 1];
        }
        #endregion


        #region CheckIsThreeEven 检测是否是三连对
        /// <summary>
        /// 检测是否是三连对
        /// </summary>
        /// <param name="ordinary"></param>
        /// <param name="universalSum"></param>
        /// <returns></returns>
        private Poker CheckIsThreeEven(List<Poker> ordinary, int universalSum)
        {
            if ((ordinary.Count + universalSum) != 6) return null;

            if (ordinary[ordinary.Count - 1].size > 14) return null;//排除大小王

            //-------------1-------------
            int onePoker = 0, twoPoker = 0, threePoker = 0;
            for (int i = 0; i < ordinary.Count; i++)
            {
                if (ordinary[i].size == ordinary[0].size) ++onePoker;
                if (ordinary[i].size == (ordinary[0].size + 1)) ++twoPoker;
                if (ordinary[i].size == (ordinary[0].size + 2)) ++threePoker;
            }
            if (onePoker <= 2 && twoPoker <= 2 && threePoker <= 2 && ordinary[ordinary.Count - 1].size <= (ordinary[0].size + 2))
            {
                if ((ordinary[0].size + 2) > 14)
                {
                  return new Poker(-1, ordinary[0].color, 14); //最大牌为A
                }
                else
                {
                    return  new Poker(-1, ordinary[0].color, (ordinary[0].size + 2));  //最大牌为: 最小牌+2
                }
            }

            //是否是23A
            int maxSamePokerSum = 1;
            bool isMinThreeEven = true;
            for (int i = 0; i < ordinary.Count; i++)
            {
                if (ordinary[i].size != 2 && ordinary[i].size != 3 && ordinary[i].size != 14)
                {
                    isMinThreeEven = false;
                    break;
                }

                if ((i - 1) >= 0)
                {
                    if (ordinary[i].size == ordinary[i - 1].size)
                    {
                        ++maxSamePokerSum;
                    }
                    else
                    {
                        if (maxSamePokerSum > 2)
                        {
                            isMinThreeEven = false;
                            break;
                        }
                        maxSamePokerSum = 1;
                    }

                }

            }
            if (isMinThreeEven) return new Poker(-1, ordinary[0].color, 3);

            return null;

            //-----2------------------------
        }
        #endregion


        #region CheckIsThreeWithTwo 检测是否是三带二
        /// <summary>
        ///  检测是否是三带二
        /// </summary>
        /// <param name="ordinary"></param>
        /// <param name="universalSum"></param>
        /// <returns></returns>
        private Poker CheckIsThreeWithTwo(List<Poker> ordinary, int universalSum)
        {
            if ((ordinary.Count + universalSum) != 5) return null;



            //---------------------------------------
            int onePokerSum = 1;
            for (int i = 1; i < ordinary.Count; ++i)
            {
                if (ordinary[i].size == ordinary[0].size)
                    ++onePokerSum;
                else
                {
                    //第二张
                    if ((onePokerSum + universalSum) < 3 || onePokerSum > 3 || (ordinary.Count - onePokerSum) > 3 ) return null;

                    for (int j = i; j < ordinary.Count-1; ++j)
                    {
                        if (ordinary[j].size != (ordinary[j + 1].size)) return null;
                    }

                    return ordinary[ordinary.Count - 1];
                }
            }

            return null;
        }
        #endregion

        #region CheckIsSteelPlate 检测是否是钢板

        /// <summary>
        /// 检测是否是钢板
        /// </summary>
        /// <param name="ordinary"></param>
        /// <param name="universalSum"></param>
        /// <returns></returns>
        private Poker CheckIsSteelPlate(List<Poker> ordinary, int universalSum)
        {
            if ((ordinary.Count + universalSum) != 6) return null;

            int onePokerSum = 1;
            for (int i = 1; i < ordinary.Count; ++i)
            {
                if (ordinary[i].size == ordinary[0].size)
                    ++onePokerSum;
                else
                {
                    //第二张
                    if ((onePokerSum + universalSum) < 3 || onePokerSum > 3 || (ordinary.Count - onePokerSum) > 3) return null;

                    for (int j = i; j < ordinary.Count; ++j)
                    {
                        if (ordinary[j].size != (ordinary[0].size + 1)) return null;
                    }
                    return ordinary[ordinary.Count - 1];
                }
            }

            return null;
        }
        #endregion


        #region CheckIsBomb 检测是否是炸弹
        /// <summary>
        /// 检测是否是炸弹
        /// </summary>
        /// <param name="ordinary"></param>
        /// <returns></returns>
        private Poker CheckIsBomb(List<Poker> ordinary)
        {
            Poker maxPoker = ordinary[0];

            for (int j = 0; j < ordinary.Count - 1; ++j)
            {
                if (ordinary[j].size != ordinary[j + 1].size)
                {
                    return null;
                }
            }
            return maxPoker;
        }
        #endregion

        #region CheckIsStraight 检测是否是顺子

        /// <summary>
        /// 检测是否是顺子
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        private Poker CheckIsStraight(List<Poker> pokers)
        {
            if (pokers == null || pokers.Count != 5) return null;

            List<Poker> ordinary = new List<Poker>();

            for (int i = 0; i < pokers.Count; ++i)
            {
                if (!pokers[i].isUniversal) ordinary.Add(pokers[i]);
            }



        
            for (int j = 0; j < ordinary.Count - 1; ++j)
            {
                if (ordinary[j].size == (ordinary[j + 1].size)) return null;//有一对
            }

            if (ordinary[ordinary.Count - 1].size > 14)
            {
                return null; //有王
            }


           
            int UniversalCount = pokers.Count - ordinary.Count; //万能牌数量


            Poker maxPoker = null; //当前最大单牌
            int maxSizeScope = ordinary[0].size - 1 + pokers.Count;
            if (ordinary[ordinary.Count - 1].size <= maxSizeScope)
            {
                if (maxSizeScope > 14)
                {
                    maxPoker = new Poker(-1, ordinary[0].color, 14); //最大牌为A
                }
                else
                {
                    maxPoker = new Poker(-1, ordinary[0].color, maxSizeScope);  //最大牌为: 最小牌+4
                }
            }

            if (ordinary[ordinary.Count - 1].size == 14)
            {
                bool isMinStraight = true;
                for (int i = 0; i < ordinary.Count - 1; i++)
                {
                    if (ordinary[i].size >= 5)
                    {
                        isMinStraight = false;
                        break;
                    }
                }
                if(isMinStraight) maxPoker = new Poker(-1, ordinary[0].color, 5);  //最大牌为: 5 //是12345
            }

            return maxPoker;
        }
        #endregion




        


    }


    public enum PokersType
    {
        HighCard,//散牌
        Single,//单张
        Two,//一对
        Three,//三不带
        ThreeWithTwo,//三带二
        SteelPlate,//钢板
        ThreeEven,//三连对
        Straight,//顺子
        StraightFlush,//同花顺
        Bomb,//炸弹
        FourKings,//四王
    }
}