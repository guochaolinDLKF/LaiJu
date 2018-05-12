//===================================================
//Author      : CZH
//CreateTime  ：9/1/2017 11:13:12 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuPaiJiu;

namespace GuPaiJiu
{
    public enum PokerType
    {
        Devil,//鬼子（包括红鬼，黑鬼）
        Emperor,//皇上
        DayNineKing,//天九王
        LandNineKing,//地九王

        SubDay,//对天
        SubLand,//对地
        SubRedPerson,//对红人
        SubGoose,//对鹅
        SubLongFive,//对长五
        SubLongSix,//对长六
        SubLongFour,//对长四
        SubTiger,//对虎头
        SubJinping,//对金平
        SubShortSeven,//对短七
        SubShortSix,//对短六

        SubMixedNine,//对杂九
        SubMixedEight,//对杂八
        SubMixedSeven,//对杂七
        SubMixedFive,//对杂五

        DayBar,//天杠
        LandBar,//地杠

        DaySevenNine,//天七九
        DayEight,//天八
        DaySeven,//天七
        DaySix,//天六
        DayFive,//天五
        DayFour,//天四
        DayThree,//天三
        DayTwo,//天二
        DayOne,//天一

        LandNine,//地九
        LandEight,//地八
        LandSeven,//地七
        LandSix,//地六
        LandFive,//地五
       // LandFour,//地四
        LandThree,//地三
        LandTwo,//地二
        LandOne,//地一

        PersonNine,//人九
        PersonEight,//人八
        PersonSeven,//人七
        PersonSix,//人六
        PersonFive,//人五
        PersonFour,//人四
        PersonThree,//人三
        PersonTwo,//人二
        PersonOne,//人一

        GooseNine,//鹅九
        GooseEight,//鹅八
        GooseSeven,//鹅七
        GooseSix,//鹅六
        GooseFive,//鹅五
        GooseFour,//鹅四
        GooseThree,//鹅三
        GooseTwo,//鹅二
        GooseOne,//鹅一

        LongNine,//长九
        LongEight,//长八
        LongSeven,//长七
        LongSix,//长六
        LongFive,//长五
        LongFour,//长四
        LongThree,//长三
        LongTwo,//长二
        LongOne,//长一

        ShortNine,//短九
        ShortEight,//短八
        ShortSeven,//短七
        ShortSix,//短六
        ShortFive,//短五
        ShortFour,//短四
        ShortThree,//短三
        ShortTwo,//短二
        ShortOne,//短一

      
        MixedEight,//杂八
        MixedSeven,//杂七
        MixedSix,//杂六
        MixedFive,//杂五
        MixedFour,//杂四
        MixedThree,//杂三
        MixedTwo,//杂二
        MixedOne,//杂一

        CloseTen,//闭十

        Unknown,//未知牌型

        Kong,//空牌
    }

    public enum EnumBtn
    {
        CutPokerObj,//切牌
        CutPanObj,//切锅
        GarbBankerObj,//抢庄
    }

    public enum ClientName
    {
        BaoDingQiPai,//保定棋牌
        No,//空
    }


}
