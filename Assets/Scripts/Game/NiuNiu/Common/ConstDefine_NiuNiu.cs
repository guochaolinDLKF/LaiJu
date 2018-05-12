//===================================================
//Author      : WZQ
//CreateTime  ：6/2/2017 10:34:09 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NiuNiu
{

    public static class ConstDefine_NiuNiu
    {
        
        //----------------申请退出------------------------------------------
        public static string ApplyQuitTitle = "提示";
        public static string ApplyQuitOk = "确定";
        public static string ApplyQuitCancel = "取消";
        public static string ApplyQuitContent = "是否申请退出本场游戏？";


        //----------------提示条------------------------------------------
        public static string RoomStateHint_Wait = "等待...";//等待
        public static string RoomStateHint_Pour = "下注中...";//下注中
        public static string RoomStateHint_LoopPoker = "拼牛中...";//拼牛中
        public static string RoomStateHint_SweepTable = "清理桌子中...";//清理桌子中
        public static string RoomStateHint_HOG = "抢庄中...";//抢庄中


        //----------------音乐-----------------------------------------------
        public static string BGM_NiuNiu = "bgm_niuniu";//牛牛背景音乐
        public static string UnitSettlement_NiuNiu = "UnitSettlement_niuniu";//牛牛每次小结声音（普通场）
        public static string AuidoVictorySettle_NiuNiu = "VictorySettle_niuniu";//牛牛小结胜利声音（激情场）
        public static string AudioFailureSettle_NiuNiu = "FailureSettle_niuniu";//牛牛小结失败声音（激情场）
        public static string StartGame_niuniu = "StartGame_niuniu";//牛牛开始声音GoldMove_niuniu
        public static string GoldMove_niuniu = "GoldMove_niuniu";//金币声音
        public static string AuidoRobBankerDi_niuniu = "robbankerdi_niuniu";//抢庄滴滴声音
        public static string PromotionToBanker_niuniu = "promotiontobanker_niuniu";//成为庄声音
        //



        //----------------预制体名字-----------------------------------------------
        public static string TaurenAni = "TaurenAni_niuniu";//游戏开始牛头弹出动画 (暂未使用预制体)
        public static string GoldName_NiuNiu = "gold_niuniu";//牛牛金币预制体
        public static string GoldImage_NiuNiu = "gold_image";//金币图片

        //----------------按钮名字-----------------------------------------------
        public static string AbdicateBanker_BtnName = "AbdicateBanker";//让庄
        public static string StartGame_BtnName = "StartGameBtn";//开始游戏 
        public static string NoRob_BtnName = "NoRobBtn";//不抢庄
        public static string RobBanker_BtnName = "RobBankerBtn";//抢庄

        public const string ViewChat_BtnName   = "ViewChatBtn";//聊天按钮
        public const string ViewShare_BtnName = "ViewShareClickBtn";//微信邀请

        //----------------观察者Key-----------------------------------------------
        public const string ObKey_RoomInfoChanged = "OnRoomInfoChanged_NiuNiu";// 房间信息变更 (局数 等)
        public const string ObKey_SeatInfoChanged = "OnSeatInfoChanged_NiuNiu";// 座位信息变更 
        public static string ObKey_OnBtnMicroUp = "OnBtnMicroUp_NiuNiu";//发送语音
        public static string ObKey_OnBtnMicroCancel = "OnBtnMicroCancel_NiuNiu";//语音取消
        public static string ObKey_OnNiuNiuViewHeadClick = "OnNiuNiuViewHeadClick_NiuNiu";//头像点击
        public static string ObKey_OnCountDownUpdate = "OnCountDownUpdate_NiuNiu";//设置倒计时
        public static string ObKey_SetRoomInteraction = "SetRoomInteraction_NiuNiu";//设置交互显影
        public static string ObKey_SetADHWindowSum = "SetADHWindowSum_NiuNiu";//刷新当前同意人数
        public static string ObKey_SetGameOverUISceneView = "SetGameOverUISceneView_NiuNiu";//处理牌局全结束
        public static string ObKey_SetNextGameUISceneView = "SetNextGameUISceneView_NiuNiu";//处理开始下一局数据
        public static string ObKey_RoomOpenPokerSettle = "RoomOpenPokerSettle_NiuNiu";//处理开牌结算数据
        public static string ObKey_SetShowPokersUI = "SetShowPokersUI_NiuNiu";//设置某玩家手牌
        public static string ObKey_SetDeal = "SetDeal_NiuNiu";//设置发牌显示
        public static string ObKey_SetRobBankerAni = "SetRobBankerAni_NiuNiu";//设置抢庄Ani
        public static string ObKey_SetHint = "SetHint_NiuNiu";//设置提示条
        public static string ObKey_EnableAllowStartBtn = "EnableAllowStartBtn_NiuNiu";//设置开始按钮遮罩
        //


        //----------------poker牌型文字-----------------------------------------------

        // public List<string> PokerTypeChinese;

        //private  void AddPokerTypeChinese()
        // {
        //     PokerTypeChinese = new List<string>();

        //     PokerTypeChinese.Add(PokerType0);
        //     PokerTypeChinese.Add(PokerType1);
        //     PokerTypeChinese.Add(PokerType2);
        //     PokerTypeChinese.Add(PokerType3);
        //     PokerTypeChinese.Add(PokerType4);
        //     PokerTypeChinese.Add(PokerType5);
        //     PokerTypeChinese.Add(PokerType6);
        //     PokerTypeChinese.Add(PokerType7);
        //     PokerTypeChinese.Add(PokerType8);
        //     PokerTypeChinese.Add(PokerType9);
        //     PokerTypeChinese.Add(PokerType10);
        //     PokerTypeChinese.Add(PokerType11);
        //     PokerTypeChinese.Add(PokerType12);
        //     PokerTypeChinese.Add(PokerType13);
        // }

        //private  string PokerType0 = "无牛";
        //private  string PokerType1 = "牛一";
        //private  string PokerType2 = "牛二";
        //private  string PokerType3 = "牛三";
        //private  string PokerType4 = "牛四";
        //private  string PokerType5 = "牛五";
        //private  string PokerType6 = "牛六";
        //private  string PokerType7 = "牛七";
        //private  string PokerType8 = "牛八";
        //private  string PokerType9 = "牛九";
        //private  string PokerType10 = "牛牛";
        //private  string PokerType11 = "五花牛";
        //private  string PokerType12 = "五小";
        //private  string PokerType13 = "炸弹";

        //---------------------------------------------------------------


        public enum PokerTypeChinese
        {

            无牛 = -1,
            牛一 = 1,
            牛二,
            牛三,
            牛四,
            牛五,
            牛六,
            牛七,
            牛八,
            牛九,
            牛牛 = 10,
            五花牛 = 11,
            五小 = 12,
            炸弹 = 13,
            顺子 = 14,
            同花 = 15,
            葫芦 = 16,
            同花顺 = 17,
        }

      
    }

}
