//===================================================
//Author      : DRB
//CreateTime  ：3/3/2017 4:24:41 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using com.oegame.guandan.protobuf;
using DRB.MahJong;
using UnityEngine;

/// <summary>
/// 麻将组合类型
/// </summary>
[System.Flags]
public enum CardType
{
    None = 0,
    /// <summary>
    /// 单牌
    /// </summary>
    Single = 1,
    /// <summary>
    /// 对子
    /// </summary>
    SameDouble = 2,
    /// <summary>
    /// 刻
    /// </summary>
    SameTriple = 4,
    /// <summary>
    /// 亮喜
    /// </summary>
    Xi = 8,
    /// <summary>
    /// 完整顺子
    /// </summary>
    StraightTriple = 16,
    /// <summary>
    /// 顺子缺中间
    /// </summary>
    StraightLackMiddle = 17,
    /// <summary>
    /// 顺子缺37
    /// </summary>
    StraightLack37 = 18,
    /// <summary>
    /// 顺子缺两张
    /// </summary>
    straightLackDouble = 19,
    /// <summary>
    /// 顺子缺左右
    /// </summary>
    StraightDouble = 20,
    /// <summary>
    /// 两张万能牌
    /// </summary>
    DoubleOther = 128,
    /// <summary>
    /// 三张万能牌
    /// </summary>
    TripleOther = 256,
    /// <summary>
    /// 十三烂
    /// </summary>
    ThirteenFar = 65536,
    /// <summary>
    /// 十三幺
    /// </summary>
    ThirteenYao = 131072,
}

public static class MahJongHelper
{
    #region CheckUniversal 检查一张牌是否为万能牌
    /// <summary>
    /// 检查一张牌是否为万能牌
    /// </summary>
    /// <param name="poker">要检查的牌</param>
    /// <param name="universal">万能牌列表</param>
    /// <returns></returns>
    public static bool CheckUniversal(Poker poker, List<Poker> universal)
    {
        if (universal == null) return false;
        if (poker == null) return false;
        for (int i = 0; i < universal.Count; ++i)
        {
            if (poker.color == universal[i].color && poker.size == universal[i].size)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region ContainPoker 检查列表里是否包含相同花色大小的牌
    /// <summary>
    /// 检查列表里是否包含相同花色大小的牌
    /// </summary>
    /// <param name="poker">要检查的牌</param>
    /// <param name="lst">牌列表</param>
    /// <returns></returns>
    public static bool HasPoker(Poker poker, List<Poker> lst)
    {
        if (poker == null) return false;
        if (lst == null) return false;
        for (int i = 0; i < lst.Count; ++i)
        {
            if (lst[i].color == poker.color && lst[i].size == poker.size)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region ContainPoker 检查是否包含这张牌
    /// <summary>
    /// 检查是否包含这张牌
    /// </summary>
    /// <param name="poker">要检查的牌</param>
    /// <param name="lst">牌列表</param>
    /// <returns></returns>
    public static bool ContainPoker(Poker poker, List<Poker> lst)
    {
        if (poker == null) return false;
        if (lst == null) return false;
        for (int i = 0; i < lst.Count; ++i)
        {
            if (lst[i].index == poker.index)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region GetSameCount 获取相同花色大小的牌的数量
    /// <summary>
    /// 获取相同花色大小的牌的数量
    /// </summary>
    /// <param name="poker">要检查的牌</param>
    /// <param name="lst">牌的列表</param>
    /// <returns></returns>
    public static int GetSameCount(Poker poker, List<Poker> lst)
    {
        if (lst == null) return 0;
        if (poker == null) return 0;
        int ret = 0;
        for (int i = 0; i < lst.Count; ++i)
        {
            if (lst[i].color == poker.color && lst[i].size == poker.size)
            {
                ++ret;
            }
        }
        return ret;
    }
    #endregion

    #region GetSamePoker 获取相同花色大小的牌
    /// <summary>
    /// 获取相同花色大小的牌
    /// </summary>
    /// <param name="poker">要检查的牌</param>
    /// <param name="lst">牌的列表</param>
    /// <returns></returns>
    public static List<Poker> GetSamePoker(Poker poker, List<Poker> lst)
    {

        if (lst == null) return null;
        if (poker == null) return null;
        List<Poker> ret = new List<Poker>();
        for (int i = 0; i < lst.Count; ++i)
        {
            if (lst[i].color == poker.color && lst[i].size == poker.size)
            {
                ret.Add(lst[i]);
            }
        }
        return ret;
    }
    #endregion

    #region Has19 检查是否包含19
    /// <summary>
    /// 检查是否包含19
    /// </summary>
    /// <param name="lst">要检查的牌的列表</param>
    /// <param name="includeHongZhong">红中是否可以代替19</param>
    /// <returns></returns>
    public static bool Has19(List<Poker> lst, bool hongzhongIs19)
    {
        for (int i = 0; i < lst.Count; ++i)
        {
            if ((lst[i].size == 1 || lst[i].size == 9) && lst[i].color < 4)
            {
                return true;
            }
            if (hongzhongIs19)
            {
                if (lst[i].color == 5)//字代替19
                {
                    return true;
                }
            }
        }
        return false;
    }
    #endregion

    #region GetNextPoker 获取下一张牌
    /// <summary>
    /// 获取下一张牌
    /// </summary>
    /// <param name="poker">要获取的牌</param>
    /// <returns></returns>
    public static Poker GetNextPoker(Poker poker)
    {
        if (poker == null) return null;
        int color = poker.color;
        int size = poker.size + 1;
        if (color < 4)
        {
            if (size > 9)
            {
                size = 1;
            }
        }
        else if (color == 5)
        {
            if (size > 3)
            {
                size = 1;
            }
        }
        else
        {
            if (size > 4)
            {
                size = 1;
            }
        }
        Poker ret = new Poker(color, size);
        return ret;
    }
    #endregion

    #region CheckLiangXi 检测该牌型可否亮喜
    /// <summary>
    /// 检测该牌型可否亮喜
    /// </summary>
    /// <param name="poker"></param>
    /// <returns></returns>
    public static bool CheckLiangXi(List<Poker> poker)
    {
        if (poker == null || poker.Count < 3 || poker.Count > 4) return false;

        List<Poker> lstZi = new List<Poker>();

        List<int> alreadyExists = new List<int>();
        for (int i = 0; i < poker.Count; ++i)
        {
            if (i > 0)
            {
                if (poker[i].color != poker[i - 1].color) return false;
            }
            if (poker[i].color != 4 && poker[i].color != 5) return false;
            if (GetSameCount(poker[i],poker) > 1) return false;
        }
        return true;
    }
    #endregion


    #region 检测听牌相关
    private static List<List<CardCombination>> Result = new List<List<CardCombination>>();//听牌结果
    private static bool IsLiangXiCanHu;

    #region CheckTing 检测听牌
    /// <summary>
    /// 检测听牌
    /// </summary>
    /// <param name="Cards">要检测的牌</param>
    /// <param name="universal">万能牌</param>
    /// <returns></returns>
    public static List<List<CardCombination>> CheckTing(List<Poker> Cards, List<Poker> universal, bool canSevenDouble, bool isLiangXiCanHu, bool is13YaoCanHu)
    {
        if (Cards == null) return null;
        IsLiangXiCanHu = isLiangXiCanHu;
        Cards = new List<Poker>(Cards);
        SimpleSort(Cards);

        Result.Clear();
        int universalCount = 0;
        if (universal != null)
        {
            for (int i = Cards.Count - 1; i >= 0; --i)
            {
                if (CheckUniversal(Cards[i], universal))
                {
                    ++universalCount;
                    Cards.RemoveAt(i);
                }
            }
        }

        List<CardCombination> prevCombination = new List<CardCombination>();

#if IS_LEPING  //检测十三烂
        CheckThirteenFar(Cards, universalCount);
        if (Result != null && Result.Count > 0)
        {
            return Result;
        }
#endif

#if IS_DAZHONG  //大众版13幺
        Check13Yao_DaZhong(Cards, universalCount);
        if (Result != null && Result.Count > 0)
        {
            return Result;
        }
#endif
        if (is13YaoCanHu)
        {
            Check13Yao(Cards, universalCount);
            if (Result != null && Result.Count > 0)
            {
                return Result;
            }
        }

        if (canSevenDouble)
        {
            CheckSevenDouble(Cards, universalCount);
        }
        Check(Cards, prevCombination, 0, 0, 0, universalCount);
        return Result;
    }
    #endregion

    #region CheckSevenDouble 检测七对
    /// <summary>
    /// 检测七对
    /// </summary>
    /// <param name="Cards"></param>
    /// <param name="universal"></param>
    /// <returns></returns>
    public static bool CheckSevenDouble(List<Poker> Cards, List<Poker> universal)
    {
        if (Cards == null) return false;
        if (Cards.Count % 2 == 0) return false;
        SimpleSort(Cards);

        Result.Clear();
        int universalCount = 0;

        if (universal != null)
        {
            for (int i = Cards.Count - 1; i >= 0; --i)
            {
                if (CheckUniversal(Cards[i], universal))
                {
                    ++universalCount;
                    Cards.RemoveAt(i);
                }
            }
        }

        CheckSevenDouble(Cards, universalCount);

        return Result.Count > 0;
    }
    #endregion

    #region CheckSevenDouble 七对听牌检测
    /// <summary>
    /// 七对听牌检测
    /// </summary>
    /// <param name="Cards">要检测的牌</param>
    /// <param name="UniversalCount">万能牌数量</param>
    private static void CheckSevenDouble(List<Poker> Cards, int UniversalCount)
    {
        if (Cards.Count + UniversalCount < 13) return;
        if (Cards.Count + UniversalCount % 2 == 0) return;
        List<CardCombination> prevCombination = new List<CardCombination>();
        List<Poker> overplus = new List<Poker>(Cards);
        List<Poker> currSingle = new List<Poker>();
        List<Poker> currSameDouble = new List<Poker>();

        for (int i = overplus.Count - 1; i >= 0; --i)
        {
            if (HasPoker(Cards[i], currSingle) && GetSameCount(Cards[i],overplus) <= GetSameCount(Cards[i], currSingle)) continue;
            Poker poker = overplus[i];
            currSingle.Add(poker);
            overplus.Remove(poker);
        }

        for (int i = currSingle.Count - 1; i>= 0; --i)
        {
            for (int j = overplus.Count - 1; j >= 0; --j)
            {
                if (currSingle[i].color == overplus[j].color && currSingle[i].size == overplus[j].size)
                {
                    currSameDouble.Add(currSingle[i]);
                    currSameDouble.Add(overplus[j]);
                    overplus.Remove(overplus[j]);
                    currSingle.Remove(currSingle[i]);
                    break;
                }
            }
        }
        
        if (currSameDouble.Count / 2 + UniversalCount < 6) return;

        for (int i = 0; i < currSingle.Count; ++i)
        {
            List<Poker> lackPoker = new List<Poker>() { new Poker(currSingle[i]) };
            List<Poker> currCombination = new List<Poker>() { new Poker(currSingle[i]) };
            CardCombination combination = new CardCombination(CardType.SameDouble, lackPoker, currCombination);
            prevCombination.Add(combination);
        }
        for (int i = 0; i < currSameDouble.Count; i += 2)
        {
            List<Poker> currCombination = new List<Poker>() { new Poker(currSameDouble[i]), new Poker(currSameDouble[i + 1]) };
            CardCombination combination = new CardCombination(CardType.SameDouble, null, currCombination);
            prevCombination.Add(combination);
        }

        if (currSingle.Count < UniversalCount)
        {
            List<Poker> lackList = new List<Poker>();
            for (int i = 1; i < 6; ++i)
            {
                for (int j = 1; j < 10; ++j)
                {
                    if (i == 4 && j > 4) break;
                    if (i == 5 && j > 3) break;

                    lackList.Add(new Poker(0, i, j));
                }
            }
            CardType type = CardType.DoubleOther;
            if (UniversalCount - currSingle.Count == 2)
            {
                type = CardType.TripleOther;
            }
            CardCombination com = new CardCombination(type, lackList, new List<Poker>());
            prevCombination.Add(com);
        }

        Result.Add(prevCombination);
    }
    #endregion

    #region CheckThirteenFar 检测十三烂听牌
    /// <summary>
    /// 检测十三烂听牌
    /// </summary>
    private static void CheckThirteenFar(List<Poker> cards, int UniversalCount)
    {
        if (cards == null) return;
        if (cards.Count + UniversalCount < 13) return;
        for (int i = 1; i < cards.Count; ++i)
        {
            if (cards[i].color < 4)
            {
                if (cards[i].color == cards[i - 1].color && cards[i].size - cards[i - 1].size < 3) return;
            }
            else
            {
                if (cards[i].color == cards[i - 1].color && cards[i].size == cards[i - 1].size) return;
            }
        }
        List<Poker> ret = new List<Poker>();
        int prevColor = 1;
        int prevSize = -2;
        bool isFirstZi = true;
        //听牌了，检测听什么
        for (int i = 0; i < cards.Count; ++i)
        {
            int currentColor = cards[i].color;
            int currentSize = cards[i].size;
            if (currentColor < 4)
            {
                if (currentColor == prevColor)
                {
                    for (int j = prevSize; j < currentSize; ++j)
                    {
                        if (j > 0 && j - prevSize > 2 && currentSize - j > 2)
                        {
                            ret.Add(new Poker(currentColor, j));
                        }
                    }
                }
                else
                {
                    for (int j = prevSize; j < 10; ++j)
                    {
                        if (j > 0 && j - prevSize > 2)
                        {
                            ret.Add(new Poker(prevColor, j));
                        }
                    }
                    ++prevColor;

                    if (prevColor != currentColor)
                    {
                        for (int j = 1; j < 10; ++j)
                        {
                            ret.Add(new Poker(prevColor, j));
                        }
                        ++prevSize;
                    }

                    prevColor = currentColor;
                    prevSize = -2;
                    for (int j = prevSize; j < currentSize; ++j)
                    {
                        if (j > 0 && j - prevSize > 2 && currentSize - j > 2)
                        {
                            ret.Add(new Poker(prevColor, j));
                        }
                    }
                }
            }
            else
            {
                if (isFirstZi)
                {
                    for (int j = prevSize; j < 10; ++j)
                    {
                        if (j > 0 && j - prevSize > 2)
                        {
                            ret.Add(new Poker(prevColor, j));
                        }
                    }
                    ++prevColor;
                    if (prevColor < currentColor)
                    {
                        for (int j = 1; j < 10; ++j)
                        {
                            ret.Add(new Poker(prevColor, j));
                        }
                    }
                    isFirstZi = false;
                }
                else
                {
                    for (int j = prevColor; j <= currentColor; ++j)//花色
                    {
                        for (int k = 1; k <= 4; ++k)//大小
                        {
                            if (j == 5 && k == 4) continue;
                            if (j == prevColor && k <= prevSize) continue;
                            if (j == currentColor && k >= currentSize) continue;
                            ret.Add(new Poker(j, k));
                        }
                    }

                }
            }


            prevColor = currentColor;
            prevSize = currentSize;
        }
        for (int i = prevColor; i < 6; ++i)
        {
            for (int j = 1; j <= 4; ++j)//大小
            {
                if (i == 5 && j == 4) continue;
                if (i == prevColor && j <= prevSize) continue;
                ret.Add(new Poker(i, j));
            }
        }
        List<CardCombination> combinationList = new List<CardCombination>();
        CardCombination combination = new CardCombination(CardType.ThirteenFar, ret, cards);

        combinationList.Add(combination);
        Result.Add(combinationList);
    }
    #endregion

    private static readonly List<Poker> m_ThirteenYaoNeedList = new List<Poker>()
    { new Poker(1, 1),
      new Poker(1, 9),
      new Poker(2, 1),
      new Poker(2, 9),
      new Poker(3, 1),
      new Poker(3, 9),
      new Poker(4, 1),
      new Poker(4, 2),
      new Poker(4, 3),
      new Poker(4, 4),
      new Poker(5, 1),
      new Poker(5, 2),
      new Poker(5, 3),
    };

    #region Check13Yao 检测13幺(通用)
    /// <summary>
    /// 检测13幺(通用)
    /// </summary>
    /// <param name="pokers"></param>
    /// <param name="universalCount"></param>
    private static void Check13Yao(List<Poker> pokers, int universalCount)
    {
        //判断能不能听牌
        if (pokers == null || pokers.Count + universalCount != 13) return;
        bool hasSame = false;
        List<Poker> currCombination = new List<Poker>();
        for (int i = 0; i < pokers.Count; ++i)
        {
            if (HasPoker(pokers[i], currCombination))
            {
                if (hasSame) return;
                hasSame = true;
            }
            if (!HasPoker(pokers[i], m_ThirteenYaoNeedList)) return;
            currCombination.Add(pokers[i]);
        }
        if (currCombination.Count != pokers.Count) return;

        //找缺什么牌
        List<Poker> lackPoker = new List<Poker>();
        if (hasSame)
        {
            for (int i = 0; i < m_ThirteenYaoNeedList.Count; ++i)
            {
                if (!HasPoker(m_ThirteenYaoNeedList[i], currCombination))
                {
                    lackPoker.Add(m_ThirteenYaoNeedList[i]);
                }
            }
        }
        else
        {
            for (int i = 0; i < m_ThirteenYaoNeedList.Count; ++i)
            {
                lackPoker.Add(m_ThirteenYaoNeedList[i]);
            }
        }

        List<CardCombination> prevCombination = new List<CardCombination>();
        CardCombination combination = new CardCombination(CardType.ThirteenYao, lackPoker, currCombination);
        prevCombination.Add(combination);
        Result.Add(prevCombination);
    }
    #endregion

    #region Check13Yao_DaZhong 检测13幺（江苏大众）
    /// <summary>
    /// 检测13幺（江苏大众）
    /// </summary>
    private static void Check13Yao_DaZhong(List<Poker> pokers, int universalCount)
    {
        if (pokers.Count < 13) return;
        //判断能不能听牌
        List<Poker> currCombination = new List<Poker>();
        for (int i = 0; i < pokers.Count; ++i)
        {
            if (HasPoker(pokers[i], currCombination))
                return;
            if (pokers[i].color >= 4)
            {
                currCombination.Add(pokers[i]);
            }
            else
            {
                for (int j = 0; j < pokers.Count; ++j)
                {
                    if (i == j) continue;
                    if (pokers[j].color >= 4) continue;
                    if (pokers[i].size == pokers[j].size)
                        return;
                    if (pokers[i].color != pokers[j].color && Mathf.Abs(pokers[i].size - pokers[j].size) % 3 == 0)
                        return;
                    if (pokers[i].color == pokers[j].color && Mathf.Abs(pokers[i].size - pokers[j].size) % 3 != 0)
                    {
                        return;
                    }
                }
                currCombination.Add(pokers[i]);
            }
        }
        if (currCombination.Count != pokers.Count)
            return;


        //找缺什么牌
        List<Poker> lackPoker = new List<Poker>();
        for (int i = 0; i < m_ThirteenYaoNeedList.Count; ++i)
        {
            if (m_ThirteenYaoNeedList[i].color >= 4 && !HasPoker(m_ThirteenYaoNeedList[i], currCombination))
            {
                lackPoker.Add(m_ThirteenYaoNeedList[i]);
            }
        }
        for (int i = 0; i < pokers.Count; ++i)
        {
            if (pokers[i].color >= 4) continue;
            for (int j = 1; j < 10; ++j)
            {
                if (j % 3 == pokers[i].size % 3)
                {
                    Poker lack = new Poker(pokers[i].color, j);
                    if (HasPoker(lack, lackPoker)) continue;
                    if (HasPoker(lack, pokers)) continue;
                    bool isExists = false;
                    for (int k = 0; k < pokers.Count; ++k)
                    {
                        if (pokers[i].color == pokers[k].color && pokers[k].size == j)
                        {
                            isExists = true;
                            break;
                        }
                    }
                    if (!isExists)
                    {
                        lackPoker.Add(lack);
                    }
                }
            }
        }

        List<CardCombination> prevCombination = new List<CardCombination>();
        CardCombination combination = new CardCombination(CardType.ThirteenYao, lackPoker, currCombination);
        prevCombination.Add(combination);
        Result.Add(prevCombination);
    }
    #endregion

    #region Check 递归通用听牌检测
    /// <summary>
    /// 递归通用听牌检测
    /// </summary>
    /// <param name="Cards">剩余未检测的牌</param>
    /// <param name="prevCombination">之前递归的结果</param>
    /// <param name="lackCount">缺的数量</param>
    /// <param name="singleCount">单牌的数量</param>
    /// <param name="sameDoubleCount">对的数量</param>
    /// <param name="UniversalCount">万能牌的数量</param>
    private static void Check(List<Poker> Cards, List<CardCombination> prevCombination, int lackCount, int singleCount, int sameDoubleCount, int UniversalCount)
    {
        if (lackCount > UniversalCount + 1)
        {
            return;
        }

        if (Cards.Count == 0)
        {
            List<CardCombination> newPrevCombination = new List<CardCombination>();
            for (int i = 0; i < prevCombination.Count; ++i)
            {
                List<Poker> lackPokers = prevCombination[i].LackCardIds == null ? null : new List<Poker>(prevCombination[i].LackCardIds);
                List<Poker> currentPokers = prevCombination[i].CurrentCombination == null ? null : new List<Poker>(prevCombination[i].CurrentCombination);
                newPrevCombination.Add(new CardCombination(prevCombination[i].CardType, lackPokers, currentPokers));
            }
            if (lackCount < UniversalCount)
            {
                List<Poker> lackList = new List<Poker>();
                for (int i = 1; i < 6; ++i)
                {
                    for (int j = 1; j < 10; ++j)
                    {
                        if (i == 4 && j > 4) break;
                        if (i == 5 && j > 3) break;

                        lackList.Add(new Poker(0, i, j));
                    }
                }
                CardType type = CardType.DoubleOther;
                if (UniversalCount - lackCount == 2)
                {
                    type = CardType.TripleOther;
                }
                CardCombination com = new CardCombination(type, lackList, new List<Poker>());
                newPrevCombination.Add(com);
            }

            sameDoubleCount = 0;
            for (int i = 0; i < newPrevCombination.Count; ++i)
            {
                if (newPrevCombination[i].CardType == CardType.SameDouble) ++sameDoubleCount;
            }
            if (sameDoubleCount == 1)
            {
                for (int i = 0; i < newPrevCombination.Count; ++i)
                {
                    if (newPrevCombination[i].CardType == CardType.SameDouble && newPrevCombination[i].CurrentCombination != null && newPrevCombination[i].CurrentCombination.Count == 2)
                    {
                        newPrevCombination[i].LackCardIds = null;
                        break;
                    }
                }
            }
            Result.Add(newPrevCombination);
            return;
        }
        CalculateStraightTriple(Cards, prevCombination, lackCount, singleCount, sameDoubleCount, UniversalCount);
        CalculateSameTriple(Cards, prevCombination, lackCount, singleCount, sameDoubleCount, UniversalCount);
        CalculateSameDouble(Cards, prevCombination, lackCount, singleCount, sameDoubleCount, UniversalCount);
        CalculateStraightDouble(Cards, prevCombination, lackCount, singleCount, sameDoubleCount, UniversalCount);
        CalculateStraightLackMiddle(Cards, prevCombination, lackCount, singleCount, sameDoubleCount, UniversalCount);
        CalculateSingle(Cards, prevCombination, lackCount, singleCount, sameDoubleCount, UniversalCount);
#if IS_LEPING
        CalculateStraightTriple_Feng(Cards, prevCombination, lackCount, singleCount, sameDoubleCount, UniversalCount);
        CalculateStraightDouble_Feng(Cards, prevCombination, lackCount, singleCount, sameDoubleCount, UniversalCount);
        CalculateSingle_Feng(Cards, prevCombination, lackCount, singleCount, sameDoubleCount, UniversalCount);
#endif
        if (IsLiangXiCanHu)
        {
            CalculateXi(Cards, prevCombination, lackCount, singleCount, sameDoubleCount, UniversalCount);
            CalculateXiDouble(Cards, prevCombination, lackCount, singleCount, sameDoubleCount, UniversalCount);
        }
    }
    #endregion

    #region CalculateSingle 检测单牌
    /// <summary>
    /// 检测单牌
    /// </summary>
    /// <param name="Cards"></param>
    /// <param name="prevCombination"></param>
    /// <param name="lackCount"></param>
    /// <param name="singleCount"></param>
    /// <param name="sameDoubleCount"></param>
    /// <param name="UniversalCount"></param>
    private static void CalculateSingle(List<Poker> Cards, List<CardCombination> prevCombination, int lackCount, int singleCount, int sameDoubleCount, int UniversalCount)
    {
        {
            if (sameDoubleCount == 0)
            {
                List<Poker> overPlusCards = new List<Poker>(Cards);
                List<CardCombination> lstCombination = new List<CardCombination>(prevCombination);
                List<Poker> currentLackCard = new List<Poker>() { new Poker(overPlusCards[0].index, overPlusCards[0].color, overPlusCards[0].size) };
                List<Poker> currentCombination = new List<Poker>() { new Poker(overPlusCards[0].index, overPlusCards[0].color, overPlusCards[0].size) };
                CardCombination newConbination = new CardCombination(CardType.SameDouble, currentLackCard, currentCombination);
                overPlusCards.RemoveAt(0);
                lstCombination.Add(newConbination);
                Check(overPlusCards, lstCombination, lackCount + 1, singleCount + 1, sameDoubleCount + 1, UniversalCount);
            }
        }

        {
            List<Poker> overPlusCards = new List<Poker>(Cards);
            List<CardCombination> lstCombination = new List<CardCombination>(prevCombination);
            List<Poker> currentLackCard = new List<Poker>() { new Poker(overPlusCards[0].index, overPlusCards[0].color, overPlusCards[0].size) };
            currentLackCard.Add(new Poker(overPlusCards[0].index, overPlusCards[0].color, overPlusCards[0].size));
            List<Poker> currentCombination = new List<Poker>() { new Poker(overPlusCards[0].index, overPlusCards[0].color, overPlusCards[0].size) };
            CardCombination newConbination = new CardCombination(CardType.SameTriple, currentLackCard, currentCombination);
            overPlusCards.RemoveAt(0);
            lstCombination.Add(newConbination);
            Check(overPlusCards, lstCombination, lackCount + 2, singleCount + 1, sameDoubleCount, UniversalCount);
        }

        {
            if (Cards[0].color > 3) return;

            List<Poker> overPlusCards1 = new List<Poker>(Cards);
            List<CardCombination> lstCombination1 = new List<CardCombination>(prevCombination);
            List<Poker> currentLackCard1 = new List<Poker>();
            if (Cards[0].size > 2)
            {
                currentLackCard1.Add(new Poker(0, Cards[0].color, Cards[0].size - 2));
            }
            if (Cards[0].size > 1)
            {
                currentLackCard1.Add(new Poker(0, Cards[0].color, Cards[0].size - 1));
            }
            if (Cards[0].size < 9)
            {
                currentLackCard1.Add(new Poker(0, Cards[0].color, Cards[0].size + 1));
            }
            if (Cards[0].size < 8)
            {
                currentLackCard1.Add(new Poker(0, Cards[0].color, Cards[0].size + 2));
            }
            List<Poker> currentCombination1 = new List<Poker>() { new Poker(overPlusCards1[0].index, overPlusCards1[0].color, overPlusCards1[0].size) };
            CardCombination newConbination1 = new CardCombination(CardType.straightLackDouble, currentLackCard1, currentCombination1);
            overPlusCards1.RemoveAt(0);
            lstCombination1.Add(newConbination1);
            Check(overPlusCards1, lstCombination1, lackCount + 2, singleCount + 1, sameDoubleCount, UniversalCount);
        }
    }
    #endregion

    #region CalculateSameDouble 检测对牌
    /// <summary>
    /// 检测对牌
    /// </summary>
    /// <param name="Cards"></param>
    /// <param name="prevCombination"></param>
    /// <param name="lackCount"></param>
    /// <param name="singleCount"></param>
    /// <param name="sameDoubleCount"></param>
    /// <param name="UniversalCount"></param>
    private static void CalculateSameDouble(List<Poker> Cards, List<CardCombination> prevCombination, int lackCount, int singleCount, int sameDoubleCount, int UniversalCount)
    {
        if (Cards.Count < 2) return;

        if (Cards[0].color != Cards[1].color) return;
        if (Cards[0].size != Cards[1].size) return;

        {
            if (sameDoubleCount == 0)
            {
                List<Poker> overPlusCards = new List<Poker>(Cards);
                List<CardCombination> lstCombination = new List<CardCombination>(prevCombination);
                List<Poker> currentCombination = new List<Poker>() { new Poker(overPlusCards[0].index, overPlusCards[0].color, overPlusCards[0].size), new Poker(overPlusCards[1].index, overPlusCards[1].color, overPlusCards[1].size) };
                CardCombination newConbination = new CardCombination(CardType.SameDouble, null, currentCombination);
                overPlusCards.RemoveAt(1);
                overPlusCards.RemoveAt(0);
                lstCombination.Add(newConbination);
                Check(overPlusCards, lstCombination, lackCount, singleCount, sameDoubleCount + 1, UniversalCount);
            }
        }

        {
            List<Poker> overPlusCards = new List<Poker>(Cards);
            List<CardCombination> lstCombination = new List<CardCombination>(prevCombination);
            List<Poker> currentLackCard = new List<Poker>() { };
            currentLackCard.Add(new Poker(overPlusCards[0].index, overPlusCards[0].color, overPlusCards[0].size));
            List<Poker> currentCombination = new List<Poker>() { new Poker(overPlusCards[0].index, overPlusCards[0].color, overPlusCards[0].size), new Poker(overPlusCards[1].index, overPlusCards[1].color, overPlusCards[1].size) };
            CardCombination newConbination = new CardCombination(CardType.SameTriple, currentLackCard, currentCombination);
            overPlusCards.RemoveAt(1);
            overPlusCards.RemoveAt(0);
            lstCombination.Add(newConbination);
            Check(overPlusCards, lstCombination, lackCount + 1, singleCount, sameDoubleCount, UniversalCount);
        }
    }
    #endregion

    #region CalculateSameTriple 检测刻牌
    /// <summary>
    /// 检测刻牌
    /// </summary>
    /// <param name="Cards"></param>
    /// <param name="prevCombination"></param>
    /// <param name="lackCount"></param>
    /// <param name="singleCount"></param>
    /// <param name="sameDoubleCount"></param>
    /// <param name="UniversalCount"></param>
    private static void CalculateSameTriple(List<Poker> Cards, List<CardCombination> prevCombination, int lackCount, int singleCount, int sameDoubleCount, int UniversalCount)
    {
        if (Cards.Count < 3) return;

        if (Cards[0].color != Cards[1].color) return;
        if (Cards[1].color != Cards[2].color) return;
        if (Cards[0].size != Cards[1].size) return;
        if (Cards[1].size != Cards[2].size) return;


        List<Poker> overPlusCards = new List<Poker>(Cards);
        List<CardCombination> lstCombination = new List<CardCombination>(prevCombination);
        List<Poker> currentCombination = new List<Poker>() { new Poker(overPlusCards[0]), new Poker(overPlusCards[1]), new Poker(overPlusCards[2]) };
        CardCombination newConbination = new CardCombination(CardType.SameTriple, null, currentCombination);
        lstCombination.Add(newConbination);
        overPlusCards.RemoveAt(2);
        overPlusCards.RemoveAt(1);
        overPlusCards.RemoveAt(0);
        Check(overPlusCards, lstCombination, lackCount, singleCount, sameDoubleCount, UniversalCount);
    }
    #endregion

    #region CalculateStraightTriple 检测顺子
    /// <summary>
    /// 检测顺子
    /// </summary>
    /// <param name="Cards"></param>
    /// <param name="prevCombination"></param>
    /// <param name="lackCount"></param>
    /// <param name="singleCount"></param>
    /// <param name="sameDoubleCount"></param>
    /// <param name="UniversalCount"></param>
    private static void CalculateStraightTriple(List<Poker> Cards, List<CardCombination> prevCombination, int lackCount, int singleCount, int sameDoubleCount, int UniversalCount)
    {
        if (Cards.Count < 3) return;
#if IS_LEPING
        if (Cards[0].color > 3 && Cards[0].color != 5) return;
#else
        if (Cards[0].color > 3) return;
#endif

        bool isFindSecond = false;
        bool isFindThird = false;
        int secondIndex = 0;
        int thirdIndex = 0;
        for (int i = 1; i < Cards.Count; ++i)
        {
            if (!isFindSecond && Cards[i].color == Cards[0].color && Cards[i].size == Cards[0].size + 1)
            {
                isFindSecond = true;
                secondIndex = i;
            }
            else if (!isFindThird && Cards[i].color == Cards[0].color && Cards[i].size == Cards[0].size + 2)
            {
                isFindThird = true;
                thirdIndex = i;
            }
            if (isFindSecond && isFindThird)
            {
                List<Poker> overPlusCards = new List<Poker>(Cards);
                List<CardCombination> lstCombination = new List<CardCombination>(prevCombination);
                List<Poker> currentCombination = new List<Poker>() { new Poker(overPlusCards[0]), new Poker(overPlusCards[secondIndex]), new Poker(overPlusCards[thirdIndex]) };
                CardCombination newConbination = new CardCombination(CardType.StraightTriple, null, currentCombination);
                if (secondIndex > thirdIndex)
                {
                    overPlusCards.RemoveAt(secondIndex);
                    overPlusCards.RemoveAt(thirdIndex);
                }
                else
                {
                    overPlusCards.RemoveAt(thirdIndex);
                    overPlusCards.RemoveAt(secondIndex);
                }
                lstCombination.Add(newConbination);
                overPlusCards.RemoveAt(0);
                Check(overPlusCards, lstCombination, lackCount, singleCount, sameDoubleCount, UniversalCount);
                break;
            }
        }
    }
    #endregion

    #region CalculateStraightDouble 检测顺子（二连）
    /// <summary>
    /// 检测顺子（二连）
    /// </summary>
    /// <param name="Cards"></param>
    /// <param name="prevCombination"></param>
    /// <param name="lackCount"></param>
    /// <param name="singleCount"></param>
    /// <param name="sameDoubleCount"></param>
    /// <param name="UniversalCount"></param>
    private static void CalculateStraightDouble(List<Poker> Cards, List<CardCombination> prevCombination, int lackCount, int singleCount, int sameDoubleCount, int UniversalCount)
    {
        if (Cards.Count < 2) return;
#if IS_LEPING
        if (Cards[0].color > 3 && Cards[0].color != 5) return;
#else
        if (Cards[0].color > 3) return;
#endif


        bool isFind = false;
        int findIndex = 0;
        for (int i = 1; i < Cards.Count; ++i)
        {
            if (Cards[i].color == Cards[0].color && Cards[i].size == Cards[0].size + 1)
            {
                isFind = true;
                findIndex = i;
                break;
            }
        }

        if (isFind)
        {
            List<Poker> overPlusCards = new List<Poker>(Cards);
            List<CardCombination> lstCombination = new List<CardCombination>(prevCombination);
            List<Poker> currentLackCard = null;
            CardType currType = CardType.StraightDouble;
            if (Cards[findIndex].size == 9)
            {
                currentLackCard = new List<Poker>() { new Poker(0, overPlusCards[0].color, overPlusCards[0].size - 1) };
                currType = CardType.StraightLack37;
            }
            else if (Cards[0].size == 1)
            {
                currentLackCard = new List<Poker>() { new Poker(0, overPlusCards[findIndex].color, overPlusCards[findIndex].size + 1) };
                currType = CardType.StraightLack37;
                if (Cards[0].color == 5)
                {
                    currType = CardType.StraightDouble;
                }
            }
            else
            {
                if (Cards[0].color == 5 && Cards[findIndex].size == 3)
                {
                    currentLackCard = new List<Poker>() { new Poker(0, overPlusCards[0].color, overPlusCards[0].size - 1) };
                }
                else
                {
                    currentLackCard = new List<Poker>() { new Poker(0, overPlusCards[0].color, overPlusCards[0].size - 1), new Poker(0, overPlusCards[findIndex].color, overPlusCards[findIndex].size + 1) };
                }

            }
            List<Poker> currentCombination = new List<Poker>() { new Poker(overPlusCards[0]), new Poker(overPlusCards[findIndex]) };
            CardCombination newConbination = new CardCombination(currType, currentLackCard, currentCombination);
            lstCombination.Add(newConbination);
            overPlusCards.RemoveAt(findIndex);
            overPlusCards.RemoveAt(0);
            Check(overPlusCards, lstCombination, lackCount + 1, singleCount, sameDoubleCount, UniversalCount);
        }
    }
    #endregion

    #region CalculateStraightLackMiddle 检测顺子(缺中间)
    /// <summary>
    /// 检测顺子(缺中间)
    /// </summary>
    /// <param name="Cards"></param>
    /// <param name="prevCombination"></param>
    /// <param name="lackCount"></param>
    /// <param name="singleCount"></param>
    /// <param name="sameDoubleCount"></param>
    /// <param name="UniversalCount"></param>
    private static void CalculateStraightLackMiddle(List<Poker> Cards, List<CardCombination> prevCombination, int lackCount, int singleCount, int sameDoubleCount, int UniversalCount)
    {
        if (Cards.Count < 2) return;
#if IS_LEPING
        if (Cards[0].color > 3 && Cards[0].color != 5) return;
#else
        if (Cards[0].color > 3) return;
#endif


        bool isFind = false;
        int findIndex = 0;
        for (int i = 1; i < Cards.Count; ++i)
        {
            if (Cards[i].color == Cards[0].color && Cards[i].size == Cards[0].size + 2)
            {
                isFind = true;
                findIndex = i;
                break;
            }
        }
        if (isFind)
        {
            List<Poker> overPlusCards = new List<Poker>(Cards);
            List<CardCombination> lstCombination = new List<CardCombination>(prevCombination);
            List<Poker> currentCombination = new List<Poker>() { new Poker(overPlusCards[0]), new Poker(overPlusCards[findIndex]) };
            List<Poker> currentLackCard = new List<Poker>() { new Poker(0, overPlusCards[0].color, overPlusCards[0].size + 1) };
            CardCombination newConbination = new CardCombination(CardType.StraightLackMiddle, currentLackCard, currentCombination);
            lstCombination.Add(newConbination);
            overPlusCards.RemoveAt(findIndex);
            overPlusCards.RemoveAt(0);
            Check(overPlusCards, lstCombination, lackCount + 1, singleCount, sameDoubleCount, UniversalCount);
        }
    }
    #endregion

    #region CalculateStraightTriple_Feng 检测顺子（风）
    /// <summary>
    /// 检测顺子（风）
    /// </summary>
    /// <param name="Cards"></param>
    /// <param name="prevCombination"></param>
    /// <param name="lackCount"></param>
    /// <param name="singleCount"></param>
    /// <param name="sameDoubleCount"></param>
    /// <param name="UniversalCount"></param>
    private static void CalculateStraightTriple_Feng(List<Poker> Cards, List<CardCombination> prevCombination, int lackCount, int singleCount, int sameDoubleCount, int UniversalCount)
    {
        if (Cards.Count < 3) return;
        if (Cards[0].color != 4) return;
        if (Cards[0].size > 2) return;

        List<Poker> feng = new List<Poker>();
        Poker dong = null;
        Poker nan = null;
        Poker xi = null;
        Poker bei = null;
        int fengCount = 0;
        for (int i = 0; i < Cards.Count; ++i)
        {
            if (Cards[i].color == 4 && Cards[i].size == 1)
            {
                if (dong == null)
                {
                    ++fengCount;
                }
                dong = Cards[i];

            }
            if (Cards[i].color == 4 && Cards[i].size == 2)
            {
                if (nan == null)
                {
                    ++fengCount;
                }
                nan = Cards[i];
            }
            if (Cards[i].color == 4 && Cards[i].size == 3)
            {
                if (xi == null)
                {
                    ++fengCount;
                }
                xi = Cards[i];
            }
            if (Cards[i].color == 4 && Cards[i].size == 4)
            {
                if (bei == null)
                {
                    ++fengCount;
                }
                bei = Cards[i];
            }
        }

        if (fengCount == 4)
        {
            {//东南西
                List<Poker> overPlusCards = new List<Poker>(Cards);
                List<CardCombination> lstCombination = new List<CardCombination>(prevCombination);
                List<Poker> currentLackCard = null;
                List<Poker> currentCombination = new List<Poker>() { new Poker(dong), new Poker(nan), new Poker(xi) };
                CardCombination newConbination = new CardCombination(CardType.StraightTriple, currentLackCard, currentCombination);
                lstCombination.Add(newConbination);
                overPlusCards.Remove(dong);
                overPlusCards.Remove(nan);
                overPlusCards.Remove(xi);
                Check(overPlusCards, lstCombination, lackCount, singleCount, sameDoubleCount, UniversalCount);
            }
            {//东南北
                List<Poker> overPlusCards = new List<Poker>(Cards);
                List<CardCombination> lstCombination = new List<CardCombination>(prevCombination);
                List<Poker> currentLackCard = null;
                List<Poker> currentCombination = new List<Poker>() { new Poker(dong), new Poker(nan), new Poker(bei) };
                CardCombination newConbination = new CardCombination(CardType.StraightTriple, currentLackCard, currentCombination);
                lstCombination.Add(newConbination);
                overPlusCards.Remove(dong);
                overPlusCards.Remove(nan);
                overPlusCards.Remove(bei);
                Check(overPlusCards, lstCombination, lackCount, singleCount, sameDoubleCount, UniversalCount);
            }
            {//东西北
                List<Poker> overPlusCards = new List<Poker>(Cards);
                List<CardCombination> lstCombination = new List<CardCombination>(prevCombination);
                List<Poker> currentLackCard = null;
                List<Poker> currentCombination = new List<Poker>() { new Poker(dong), new Poker(xi), new Poker(bei) };
                CardCombination newConbination = new CardCombination(CardType.StraightTriple, currentLackCard, currentCombination);
                lstCombination.Add(newConbination);
                overPlusCards.Remove(dong);
                overPlusCards.Remove(xi);
                overPlusCards.Remove(bei);
                Check(overPlusCards, lstCombination, lackCount, singleCount, sameDoubleCount, UniversalCount);
            }
        }
        else if (fengCount == 3)
        {
            List<Poker> overPlusCards = new List<Poker>(Cards);
            List<CardCombination> lstCombination = new List<CardCombination>(prevCombination);
            List<Poker> currentLackCard = null;
            List<Poker> currentCombination = new List<Poker>();
            if (dong != null)
            {
                currentCombination.Add(dong);
            }
            if (nan != null)
            {
                currentCombination.Add(nan);
            }
            if (xi != null)
            {
                currentCombination.Add(xi);
            }
            if (bei != null)
            {
                currentCombination.Add(bei);
            }
            CardCombination newConbination = new CardCombination(CardType.StraightTriple, currentLackCard, currentCombination);
            lstCombination.Add(newConbination);
            overPlusCards.Remove(currentCombination[0]);
            overPlusCards.Remove(currentCombination[1]);
            overPlusCards.Remove(currentCombination[2]);
            Check(overPlusCards, lstCombination, lackCount, singleCount, sameDoubleCount, UniversalCount);
        }
    }
    #endregion

    #region CalculateStraightDouble_Feng 检测顺子(二连风)
    /// <summary>
    /// 检测顺子(二连风)
    /// </summary>
    /// <param name="Cards"></param>
    /// <param name="prevCombination"></param>
    /// <param name="lackCount"></param>
    /// <param name="singleCount"></param>
    /// <param name="sameDoubleCount"></param>
    /// <param name="UniversalCount"></param>
    private static void CalculateStraightDouble_Feng(List<Poker> Cards, List<CardCombination> prevCombination, int lackCount, int singleCount, int sameDoubleCount, int UniversalCount)
    {

#if !IS_LEPING
        return;
#endif

        if (Cards.Count < 2) return;
        if (Cards[0].color != 4) return;

        {
            bool isFind = false;
            int findIndex = 0;
            for (int i = 1; i < Cards.Count; ++i)
            {
                if (Cards[i].color == Cards[0].color && Cards[i].size == Cards[0].size + 1)
                {
                    isFind = true;
                    findIndex = i;
                    break;
                }
            }

            if (isFind)
            {
                List<Poker> overPlusCards = new List<Poker>(Cards);
                List<CardCombination> lstCombination = new List<CardCombination>(prevCombination);
                List<Poker> currentLackCard = null;
                CardType currType = CardType.StraightDouble;
                currentLackCard = new List<Poker>();
                for (int i = 1; i < 5; ++i)
                {
                    if (i != Cards[0].size && i != Cards[findIndex].size)
                    {
                        currentLackCard.Add(new Poker(0, 4, i));
                    }
                }
                List<Poker> currentCombination = new List<Poker>() { new Poker(overPlusCards[0]), new Poker(overPlusCards[findIndex]) };
                CardCombination newConbination = new CardCombination(currType, currentLackCard, currentCombination);
                lstCombination.Add(newConbination);
                overPlusCards.RemoveAt(findIndex);
                overPlusCards.RemoveAt(0);
                Check(overPlusCards, lstCombination, lackCount + 1, singleCount, sameDoubleCount, UniversalCount);
            }
        }
        {
            bool isFind = false;
            int findIndex = 0;
            for (int i = 1; i < Cards.Count; ++i)
            {
                if (Cards[i].color == Cards[0].color && Cards[i].size == Cards[0].size + 2)
                {
                    isFind = true;
                    findIndex = i;
                    break;
                }
            }

            if (isFind)
            {
                List<Poker> overPlusCards = new List<Poker>(Cards);
                List<CardCombination> lstCombination = new List<CardCombination>(prevCombination);
                List<Poker> currentLackCard = null;
                CardType currType = CardType.StraightDouble;
                currentLackCard = new List<Poker>();
                for (int i = 1; i < 5; ++i)
                {
                    if (i != Cards[0].size && i != Cards[findIndex].size)
                    {
                        currentLackCard.Add(new Poker(0, 4, i));
                    }
                }
                List<Poker> currentCombination = new List<Poker>() { new Poker(overPlusCards[0]), new Poker(overPlusCards[findIndex]) };
                CardCombination newConbination = new CardCombination(currType, currentLackCard, currentCombination);
                lstCombination.Add(newConbination);
                overPlusCards.RemoveAt(findIndex);
                overPlusCards.RemoveAt(0);
                Check(overPlusCards, lstCombination, lackCount + 1, singleCount, sameDoubleCount, UniversalCount);
            }
        }
        {
            bool isFind = false;
            int findIndex = 0;
            for (int i = 1; i < Cards.Count; ++i)
            {
                if (Cards[i].color == Cards[0].color && Cards[i].size == Cards[0].size + 3)
                {
                    isFind = true;
                    findIndex = i;
                    break;
                }
            }

            if (isFind)
            {
                List<Poker> overPlusCards = new List<Poker>(Cards);
                List<CardCombination> lstCombination = new List<CardCombination>(prevCombination);
                List<Poker> currentLackCard = null;
                CardType currType = CardType.StraightDouble;
                currentLackCard = new List<Poker>();
                for (int i = 1; i < 5; ++i)
                {
                    if (i != Cards[0].size && i != Cards[findIndex].size)
                    {
                        currentLackCard.Add(new Poker(0, 4, i));
                    }
                }
                List<Poker> currentCombination = new List<Poker>() { new Poker(overPlusCards[0]), new Poker(overPlusCards[findIndex]) };
                CardCombination newConbination = new CardCombination(currType, currentLackCard, currentCombination);
                lstCombination.Add(newConbination);
                overPlusCards.RemoveAt(findIndex);
                overPlusCards.RemoveAt(0);
                Check(overPlusCards, lstCombination, lackCount + 1, singleCount, sameDoubleCount, UniversalCount);
            }
        }
    }
    #endregion

    #region CalculateSingle_Feng 检测单牌(风)
    /// <summary>
    /// 检测单牌(风)
    /// </summary>
    /// <param name="Cards"></param>
    /// <param name="prevCombination"></param>
    /// <param name="lackCount"></param>
    /// <param name="singleCount"></param>
    /// <param name="sameDoubleCount"></param>
    /// <param name="UniversalCount"></param>
    private static void CalculateSingle_Feng(List<Poker> Cards, List<CardCombination> prevCombination, int lackCount, int singleCount, int sameDoubleCount, int UniversalCount)
    {
#if !IS_LEPING
        return;
#endif
        if (Cards.Count < 1) return;
        if (Cards[0].color != 4) return;
        List<Poker> overPlusCards1 = new List<Poker>(Cards);
        List<CardCombination> lstCombination1 = new List<CardCombination>(prevCombination);
        List<Poker> currentLackCard1 = new List<Poker>();
        for (int i = 1; i < 5; ++i)
        {
            if (i != Cards[0].size)
            {
                currentLackCard1.Add(new Poker(0, 4, i));
            }
        }
        List<Poker> currentCombination1 = new List<Poker>() { new Poker(overPlusCards1[0].index, overPlusCards1[0].color, overPlusCards1[0].size) };
        CardCombination newConbination1 = new CardCombination(CardType.straightLackDouble, currentLackCard1, currentCombination1);
        overPlusCards1.RemoveAt(0);
        lstCombination1.Add(newConbination1);
        Check(overPlusCards1, lstCombination1, lackCount + 2, singleCount + 1, sameDoubleCount, UniversalCount);
    }
    #endregion

    #region CalculateXi 检测亮喜
    /// <summary>
    /// 检测亮喜
    /// </summary>
    /// <param name="Cards"></param>
    /// <param name="prevCombination"></param>
    /// <param name="lackCount"></param>
    /// <param name="singleCount"></param>
    /// <param name="sameDoubleCount"></param>
    /// <param name="UniversalCount"></param>
    private static void CalculateXi(List<Poker> Cards, List<CardCombination> prevCombination, int lackCount, int singleCount, int sameDoubleCount, int UniversalCount)
    {
        if (Cards.Count < 3) return;
        if (Cards[0].color < 4 && Cards[0].size > 1 && Cards[0].size < 9) return;
        if (Cards[0].color == 4) return;
        if (Cards[0].color == 5 && Cards[0].size > 1) return;

        List<Poker> lstXi = new List<Poker>();
        if (Cards[0].color < 4)
        {
            lstXi.Add(Cards[0]);
            for (int i = 1; i < Cards.Count; ++i)
            {
                if (Cards[i].size != Cards[0].size) continue;
                if (Cards[i].color > 3) continue;
                if (!HasPoker(Cards[i], lstXi))
                {
                    lstXi.Add(Cards[i]);
                }
            }
        }
        else if (Cards[0].color == 5)
        {
            lstXi.Add(Cards[0]);
            for (int i = 1; i < Cards.Count; ++i)
            {
                if (Cards[i].color != Cards[0].color) continue;
                if (!HasPoker(Cards[i], lstXi))
                {
                    lstXi.Add(Cards[i]);
                }
            }
        }

        if (lstXi.Count == 3)
        {
            List<Poker> overPlusCards = new List<Poker>(Cards);
            List<CardCombination> lstCombination = new List<CardCombination>(prevCombination);
            List<Poker> currentCombination = lstXi;
            CardCombination newConbination = new CardCombination(CardType.Xi, null, currentCombination);
            lstCombination.Add(newConbination);
            for (int i = 0; i < lstXi.Count; ++i)
            {
                overPlusCards.Remove(lstXi[i]);
            }
            Check(overPlusCards, lstCombination, lackCount, singleCount, sameDoubleCount, UniversalCount);
        }
    }
    #endregion

    #region CalculateXiDouble 检测亮喜（两张）
    /// <summary>
    /// 检测亮喜（两张）
    /// </summary>
    /// <param name="Cards"></param>
    /// <param name="prevCombination"></param>
    /// <param name="lackCount"></param>
    /// <param name="singleCount"></param>
    /// <param name="sameDoubleCount"></param>
    /// <param name="UniversalCount"></param>
    private static void CalculateXiDouble(List<Poker> Cards, List<CardCombination> prevCombination, int lackCount, int singleCount, int sameDoubleCount, int UniversalCount)
    {
        if (Cards.Count < 2) return;
        if (Cards[0].color < 4 && Cards[0].size > 1 && Cards[0].size < 9) return;
        if (Cards[0].color == 4) return;


        for (int i = 1; i < Cards.Count; ++i)
        {
            if (Cards[0].color < 4)
            {
                if (Cards[i].size == Cards[0].size && Cards[i].color != Cards[0].color && Cards[i].color < 4)
                {
                    List<Poker> overPlusCards = new List<Poker>(Cards);
                    List<CardCombination> lstCombination = new List<CardCombination>(prevCombination);
                    List<Poker> currentCombination = new List<Poker>() { Cards[0], Cards[i] };
                    List<Poker> lackPoker = new List<Poker>();
                    for (int j = 1; j <= 3; ++j)
                    {
                        bool isExists = false;
                        for (int k = 0; k < currentCombination.Count; ++k)
                        {
                            if (currentCombination[k].color == j)
                            {
                                isExists = true;
                                break;
                            }
                        }
                        if (!isExists)
                        {
                            lackPoker.Add(new Poker(j, Cards[0].size));
                        }
                    }
                    CardCombination newConbination = new CardCombination(CardType.Xi, lackPoker, currentCombination);
                    lstCombination.Add(newConbination);
                    for (int j = 0; j < currentCombination.Count; ++j)
                    {
                        overPlusCards.Remove(currentCombination[j]);
                    }
                    Check(overPlusCards, lstCombination, lackCount + 1, singleCount, sameDoubleCount, UniversalCount);
                }
            }
            else if (Cards[0].color == 5)
            {
                if (Cards[i].color == Cards[0].color && Cards[i].size != Cards[0].size)
                {
                    List<Poker> overPlusCards = new List<Poker>(Cards);
                    List<CardCombination> lstCombination = new List<CardCombination>(prevCombination);
                    List<Poker> currentCombination = new List<Poker>() { Cards[0], Cards[i] };
                    List<Poker> lackPoker = new List<Poker>();
                    for (int j = 1; j <= 3; ++j)
                    {
                        bool isExists = false;
                        for (int k = 0; k < currentCombination.Count; ++k)
                        {
                            if (currentCombination[k].size == j)
                            {
                                isExists = true;
                                break;
                            }
                        }
                        if (!isExists)
                        {
                            lackPoker.Add(new Poker(5, j));
                            break;
                        }
                    }
                    CardCombination newConbination = new CardCombination(CardType.Xi, lackPoker, currentCombination);
                    lstCombination.Add(newConbination);
                    for (int j = 0; j < currentCombination.Count; ++j)
                    {
                        overPlusCards.Remove(currentCombination[j]);
                    }
                    Check(overPlusCards, lstCombination, lackCount + 1, singleCount, sameDoubleCount, UniversalCount);
                }
            }
        }
    }
    #endregion

    #endregion

    #region 计算胡牌类型
    public static List<int> CalculateCardType(List<Poker> pokers, Poker universal)
    {
        List<Poker> universals = new List<Poker>();
        List<Poker> newPokers = new List<Poker>(pokers);
        if (universal != null)
        {
            for (int i = newPokers.Count - 1; i >= 0; --i)
            {
                if (newPokers[i].color == universal.color && newPokers[i].size == universal.size)
                {
                    universals.Add(newPokers[i]);
                    newPokers.RemoveAt(i);
                }
            }
        }

        int maxDoubleCount = GetDoubleCount(newPokers, universals.Count);//当前牌型最多对的数量(包括万能牌)
        int maxTripleCount = GetTripleCount(newPokers, universals.Count);//当前牌型最多刻的数量(包括万能牌)
        //int maxTripletCount = GetTripletCount(newPokers, universals.Count);
        int maxQuadrupleCount = GetQuadrupleCount(newPokers, universals.Count);//当前牌型最多杠的数量(包括万能牌)
        bool isSameColor = CheckColor(newPokers);//当前牌型是否清一色

        List<cfg_configEntity> lstConfig = cfg_configDBModel.Instance.GetList();
        List<int> ret = new List<int>();
        for (int i = 0; i < lstConfig.Count; ++i)
        {
            //Debug.Log(lstConfig[i].name);
            if (lstConfig[i].AllIsSameColor)
            {
                if (!isSameColor)
                {
                    //Debug.Log("花色不合适");
                    continue;
                }
            }
            int lackCount = GetLackMustCount(newPokers, lstConfig[i].Must, lstConfig[i].MustIsSameColor);
            if (lackCount > universals.Count)
            {
                //Debug.Log("必带牌不合适");
                continue;
            }
            if (maxDoubleCount < lstConfig[i].MinDoubleCount)
            {
                //Debug.Log("对不合适");
                continue;
            }
            if (maxTripleCount < lstConfig[i].MinTripleCount)
            {
                //Debug.Log("刻不合适");
                continue;
            }
            //if (maxTripletCount < lstConfig[i].MinTripletCount || minTripletCount > lstConfig[i].MaxTripletCount) continue;
            if (maxQuadrupleCount < lstConfig[i].MinQuadrupleCount)
            {
                //Debug.Log("杠不合适");
                continue;
            }
            //Debug.Log("都合适");
            AppDebug.Log(lstConfig[i].name);
            ret.Add(lstConfig[i].id);
        }
        return ret;
    }

    private static int GetDoubleCount(List<Poker> pokers, int universalCount)
    {
        int count = 0;

        for (int i = 0; i < pokers.Count; ++i)
        {
            Poker poker = pokers[i];
            int sameCount = 0;
            for (int j = 0; j < pokers.Count; ++j)
            {
                if (poker.color == pokers[j].color && poker.size == pokers[j].size)
                {
                    ++sameCount;
                }
            }
            if (sameCount == 2)
            {
                ++count;
            }
            else if (sameCount == 4)
            {
                count += 2;
            }
        }

        return count / 2 + universalCount;
    }

    private static int GetTripleCount(List<Poker> pokers, int universalCount)
    {
        int tripleCount = 0;
        int doubleCount = 0;
        for (int i = 0; i < pokers.Count; ++i)
        {
            Poker poker = pokers[i];
            int sameCount = 0;
            for (int j = 0; j < pokers.Count; ++j)
            {
                if (poker.color == pokers[j].color && poker.size == pokers[j].size)
                {
                    ++sameCount;
                }
            }
            if (sameCount >= 3)
            {
                ++tripleCount;
            }
            else if (sameCount == 2)
            {
                ++doubleCount;
            }
        }
        doubleCount /= 2;
        return (tripleCount / 3) + (doubleCount < universalCount ? doubleCount : universalCount) + (universalCount - doubleCount >= 2 ? 1 : 0);
    }

    private static int GetTripletCount(List<Poker> pokers, int universalCount)
    {
        int count = 0;



        return count;
    }

    private static int GetQuadrupleCount(List<Poker> pokers, int universalCount)
    {
        int count = 0;

        for (int i = 0; i < pokers.Count; ++i)
        {
            Poker poker = pokers[i];
            int sameCount = 0;
            for (int j = 0; j < pokers.Count; ++j)
            {
                if (poker.color == pokers[j].color && poker.size == pokers[j].size)
                {
                    ++sameCount;
                }
            }
            if (sameCount == 4)
            {
                ++count;
            }
        }

        return count / 4;
    }

    private static int GetLackMustCount(List<Poker> pokers, List<PokerConfig> must, bool isSameColor)
    {
        List<Poker> newPokers = new List<Poker>(pokers);
        int lackCount = must.Count;//缺少的牌的数量

        if (isSameColor)//如果需要0的花色相同
        {
            bool isLimit = false;//是否有0为花色的牌
            for (int o = 1; o < 5; ++o)
            {
                int hasCount = 0;//已经找到的数量
                int color = o;//这轮由哪个花色代替0
                for (int i = 0; i < must.Count; ++i)
                {
                    bool isFind = false;
                    for (int j = 0; j < must[i].Pokers.Count; ++j)
                    {
                        for (int k = 0; k < newPokers.Count; ++k)
                        {
                            if (must[i].Pokers[j].color == 0)
                            {
                                isLimit = true;
                                if (must[i].Pokers[j].size == newPokers[k].size && color == newPokers[k].color)
                                {
                                    isFind = true;
                                    newPokers.RemoveAt(k);
                                    break;
                                }
                            }
                            else
                            {
                                if (must[i].Pokers[j].size == newPokers[k].size && must[i].Pokers[j].color == newPokers[k].color)
                                {
                                    isFind = true;
                                    newPokers.RemoveAt(k);
                                    break;
                                }
                            }
                        }
                        if (isFind)
                        {
                            ++hasCount;
                            break;
                        }
                    }
                }
                int currentCalculateLackCount = must.Count - hasCount;//当前这轮计算差的牌的数量
                if (currentCalculateLackCount < lackCount)
                {
                    lackCount = currentCalculateLackCount;
                }
                if (!isLimit) break;
            }
        }
        else
        {
            int hasCount = 0;//已经找到的数量
            for (int i = 0; i < must.Count; ++i)
            {
                bool isFind = false;
                for (int j = 0; j < must[i].Pokers.Count; ++j)
                {
                    for (int k = 0; k < newPokers.Count; ++k)
                    {
                        if (must[i].Pokers[j].color == 0)
                        {
                            if (must[i].Pokers[j].size == newPokers[k].size)
                            {
                                isFind = true;
                                newPokers.RemoveAt(k);
                                break;
                            }
                        }
                        else
                        {
                            if (must[i].Pokers[j].size == newPokers[k].size && must[i].Pokers[j].color == newPokers[k].color)
                            {
                                isFind = true;
                                newPokers.RemoveAt(k);
                                break;
                            }
                        }
                    }
                    if (isFind)
                    {
                        ++hasCount;
                        break;
                    }
                }
            }
            if (must.Count - hasCount < lackCount)
            {
                lackCount = must.Count - hasCount;
            }
        }
        return lackCount;
    }

    private static bool CheckColor(List<Poker> pokers)
    {
        for (int i = 1; i < pokers.Count; ++i)
        {
            if (pokers[i].color != pokers[0].color)
            {
                return false;
            }
        }
        return true;
    }
    #endregion

    #region 排序
    /// <summary>
    /// 万能牌列表
    /// </summary>
    private static List<Poker> m_UniversalList = new List<Poker>();

    /// <summary>
    /// 已经成组的牌的列表
    /// </summary>
    private static List<Poker> m_Combination = new List<Poker>();

    #region SimpleSort 基本排序
    /// <summary>
    /// 基本排序
    /// </summary>
    /// <param name="lst"></param>
    public static void SimpleSort(List<Poker> lst)
    {
        lst.Sort((Poker card1, Poker card2) =>
        {
            if (card1.color < card2.color)
            {
                return -1;
            }
            else if (card1.color == card2.color)
            {
                if (card1.size < card2.size)
                {
                    return -1;
                }
                else if (card1.size == card2.size)
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

    #region UniversalLeftSort 癞子放最左排序
    /// <summary>
    /// 癞子放最左排序
    /// </summary>
    /// <param name="lst"></param>
    /// <param name="universal"></param>
    private static void UniversalLeftSort(List<Poker> lst, List<Poker> universal)
    {
        SimpleSort(lst);

        if (universal != null)
        {
            for (int i = 0; i < lst.Count; ++i)
            {
                if (CheckUniversal(lst[i], universal))
                {
                    Poker poker = lst[i];
                    lst.RemoveAt(i);
                    lst.Insert(0, poker);
                }
            }
        }
    }
    #endregion

    #region UniversalBestSort 最优排序
    /// <summary>
    /// 最优排序
    /// </summary>
    /// <param name="lst"></param>
    /// <param name="universal"></param>
    private static void UniversalBestSort(List<Poker> lst, List<Poker> universal)
    {
        if (lst == null || lst.Count == 0) return;
        if (lst[0].size == 0) return;
        m_UniversalList.Clear();
        m_Combination.Clear();
        if (universal != null)
        {
            for (int i = lst.Count - 1; i >= 0; --i)
            {
                if (CheckUniversal(lst[i], universal))
                {
                    m_UniversalList.Add(lst[i]);
                    lst.RemoveAt(i);
                    break;
                }
            }
        }

        lst.Sort((Poker card1, Poker card2) =>
        {
            if (card1.color < card2.color)
            {
                return -1;
            }
            else if (card1.color == card2.color)
            {
                if (card1.size < card2.size)
                {
                    return -1;
                }
                else if (card1.size == card2.size)
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

        if (m_UniversalList.Count > 0)
        {
            //剔除已经成组合的牌
            EliminateStraightTriple(lst, m_Combination);
            EliminateSameTriple(lst, m_Combination);

            //1.把万能牌放进缺中间的组合
            for (int i = 0; i < lst.Count - 1; ++i)
            {
                if (m_UniversalList.Count == 0) break;
                if (lst[i].color == lst[i + 1].color && lst[i].size == lst[i + 1].size - 2)
                {
                    lst.Insert(i + 1, m_UniversalList[0]);
                    m_UniversalList.RemoveAt(0);
                    i = i + 2;
                }
            }
            //2.把万能牌放进缺右或缺左的组合
            for (int i = 0; i < lst.Count - 1; ++i)
            {
                if (m_UniversalList.Count == 0) break;

                if (CheckUniversal(lst[i], universal))
                {
                    continue;
                }
                if (i + 2 < lst.Count && CheckUniversal(lst[i + 2], universal))
                {
                    continue;
                }

                if (lst[i].color == lst[i + 1].color && lst[i].size == lst[i + 1].size - 1)
                {
                    if (lst[i].size == 1)
                    {
                        lst.Insert(i + 2, m_UniversalList[0]);
                    }
                    else if (lst[i].size == 8)
                    {
                        lst.Insert(i, m_UniversalList[0]);
                    }
                    else
                    {
                        lst.Insert(i + 2, m_UniversalList[0]);
                    }
                    m_UniversalList.RemoveAt(0);
                    i = i + 2;
                }
            }
            //3.把万能牌放进对的组合
            for (int i = 0; i < lst.Count - 1; ++i)
            {
                if (m_UniversalList.Count == 0) break;

                if (CheckUniversal(lst[i], universal))
                {
                    continue;
                }

                int sameCount = 0;
                for (int j = 0; j < lst.Count; ++j)
                {
                    if (lst[i].color == lst[j].color && lst[i].size == lst[j].size)
                    {
                        ++sameCount;
                    }
                }
                if (sameCount == 2)
                {
                    lst.Insert(i + 2, m_UniversalList[0]);
                    m_UniversalList.RemoveAt(0);
                    i += 2;
                }
            }

            //4.把万能牌放进单牌的组合
            for (int i = 0; i < lst.Count; ++i)
            {
                if (m_UniversalList.Count == 0) break;
                if (CheckUniversal(lst[i], universal))
                {
                    continue;
                }

                if (i == lst.Count - 1)
                {
                    lst.Insert(i + 1, m_UniversalList[0]);
                    m_UniversalList.RemoveAt(0);
                    break;
                }

                if (CheckUniversal(lst[i + 1], universal))
                {
                    continue;
                }
                if (lst[i + 1].color == lst[i].color && lst[i + 1].size == lst[i].size)//如果他后边的牌和他一样 他不是单牌
                {
                    continue;
                }
                if (lst[i + 1].color == lst[i].color && lst[i + 1].size - 1 == lst[i].size)//如果他后边的牌比他大1 他不是单牌
                {
                    continue;
                }

                lst.Insert(i + 1, m_UniversalList[0]);
                m_UniversalList.RemoveAt(0);
                ++i;
            }
            //把组合放进手牌里
            for (int i = 0; i < m_Combination.Count; ++i)
            {
                bool isInsert = false;
                for (int j = 0; j < lst.Count; ++j)
                {
                    if (CheckUniversal(lst[j], universal))
                    {
                        continue;
                    }
                    if (m_Combination[i].color < lst[j].color || (m_Combination[i].color == lst[j].color && m_Combination[i].size <= lst[j].size))
                    {
                        lst.Insert(j, m_Combination[i]);
                        isInsert = true;
                        break;
                    }
                }
                if (!isInsert)
                {
                    lst.Add(m_Combination[i]);
                }
            }

            if (m_UniversalList.Count > 0)
            {
                for (int i = 0; i < m_UniversalList.Count; ++i)
                {
                    lst.Add(m_UniversalList[i]);
                }
            }
        }
    }
    #endregion

    #region Sort 排序
    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="lst"></param>
    /// <param name="universal"></param>
    /// <param name="type"></param>
    public static void Sort(List<Poker> lst, List<Poker> universal, UniversalSortType type)
    {
        switch (type)
        {
            case UniversalSortType.Left:
                UniversalLeftSort(lst, universal);
                break;
            case UniversalSortType.Normal:
                SimpleSort(lst);
                break;
            case UniversalSortType.Best:
                UniversalBestSort(lst, universal);
                break;
        }
    }
    #endregion

    private static void EliminateStraightTriple(List<Poker> Cards, List<Poker> combination)
    {
        for (int j = 0; j < Cards.Count; ++j)
        {
            bool isFindSecond = false;
            bool isFindThird = false;
            int secondIndex = 0;
            int thirdIndex = 0;
            for (int i = j + 1; i < Cards.Count; ++i)
            {
                if (!isFindSecond && Cards[i].color == Cards[j].color && Cards[i].size == Cards[j].size + 1)
                {
                    isFindSecond = true;
                    secondIndex = i;
                }
                else if (!isFindThird && Cards[i].color == Cards[j].color && Cards[i].size == Cards[j].size + 2)
                {
                    isFindThird = true;
                    thirdIndex = i;
                }
                if (isFindSecond && isFindThird)
                {
                    if (secondIndex > thirdIndex)
                    {
                        combination.Add(Cards[secondIndex]);
                        combination.Add(Cards[thirdIndex]);
                        Cards.RemoveAt(secondIndex);
                        Cards.RemoveAt(thirdIndex);
                    }
                    else
                    {
                        combination.Add(Cards[thirdIndex]);
                        combination.Add(Cards[secondIndex]);
                        Cards.RemoveAt(thirdIndex);
                        Cards.RemoveAt(secondIndex);
                    }
                    combination.Add(Cards[j]);
                    Cards.RemoveAt(j);
                    --j;
                    break;
                }
            }
        }

    }

    private static void EliminateSameTriple(List<Poker> Cards, List<Poker> combination)
    {
        for (int i = 0; i < Cards.Count - 2; ++i)
        {
            if (Cards[i].color != Cards[i + 1].color) continue;
            if (Cards[i + 1].color != Cards[i + 2].color) continue;
            if (Cards[i].size != Cards[i + 1].size) continue;
            if (Cards[i + 1].size != Cards[i + 2].size) continue;
            combination.Add(Cards[i + 2]);
            combination.Add(Cards[i + 1]);
            combination.Add(Cards[i]);
            Cards.RemoveAt(i + 2);
            Cards.RemoveAt(i + 1);
            Cards.RemoveAt(i + 0);
            --i;
        }
    }
    #endregion
}

/// <summary>
/// 听牌检测组合
/// </summary>
public class CardCombination
{
    public CardCombination(CardType type, List<Poker> lackCardIds, List<Poker> currentCombination)
    {
        if (type == CardType.SameTriple || type == CardType.StraightTriple)
        {
            IsComplete = true;
        }
        CardType = type;
        LackCardIds = lackCardIds;
        CurrentCombination = currentCombination;
    }
    public bool IsComplete;
    public CardType CardType;
    public List<Poker> LackCardIds;
    public List<Poker> CurrentCombination;
    public CardType TargetType;
}

