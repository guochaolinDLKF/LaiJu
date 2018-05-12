//===================================================
//Author      : DRB
//CreateTime  ：5/20/2017 3:39:48 PM
//Description ：结算详情数据实体
//===================================================
using com.oegame.mahjong.protobuf;
using UnityEngine;

namespace DRB.MahJong
{
    /// <summary>
    /// 结算详情数据实体
    /// </summary>
    public class IncomeDetailEntity
    {
        public int typeId; //组合类型 
        public int times; //倍数，几番 
        public Poker poker;

        public IncomeDetailEntity(int typeId,int times,Poker poker)
        {
            this.typeId = typeId;
            this.times = times;
            this.poker = poker;
        }

        public IncomeDetailEntity(int typeId, int times)
        {
            this.typeId = typeId;
            this.times = times;
        }

    }
}
