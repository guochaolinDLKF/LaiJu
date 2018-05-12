//===================================================
//Author      : DRB
//CreateTime  ：5/20/2017 3:38:55 PM
//Description ：麻将组合数据实体
//===================================================
using UnityEngine;
using System.Collections.Generic;
using DRB.MahJong;

namespace DRB.MahJong
{
    /// <summary>
    /// 麻将组合数据实体
    /// </summary>
    public class PokerCombinationEntity
    {
        public OperatorType CombinationType;
        public int SubTypeId;
        public List<Poker> PokerList;

        public PokerCombinationEntity(OperatorType typeId, int subType, List<Poker> lst)
        {
            CombinationType = typeId;
            SubTypeId = subType;
            PokerList = lst;
            if (PokerList == null)
            {
                PokerList = new List<Poker>();
            }
        }
    }
}