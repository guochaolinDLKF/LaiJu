//===================================================
//Author      : WZQ
//CreateTime  ：7/6/2017 11:57:50 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaiJiu
{
    public static class ConstDefine_PaiJiu
    {
        //----------------观察者事件Key------------------------------------------
        //ConstDefine_PaiJiu
        public const string ObKey_RoomInfoChanged = "OnRoomInfoChanged_PaiJiu";// 房间信息变更 (局数 等)
        public const string ObKey_SeatInfoChanged = "OnSeatInfoChanged_PaiJiu";// 座位信息变更 UI 及 3d场景OnBtnPaiJiuViewPour
        public const string ObKey_btnStartGame = "OnBtnStartGame_PaiJiu";//庄家开始游戏点击
        public const string ObKey_btnPour = "btnPour_PaiJiu";// 下注按钮 发送下注选分
        public const string ObKey_btnChooseBanker = "btnChooseBanker_PaiJiu";// 是否坐庄ChooseBanker
        public const string ObKey_btnRobBanker = "btnRobBanker_PaiJiu";// 是否抢庄
        public const string ObKey_SetPourBtn = "OnOffPour_PaiJiu";//设置下注按钮显影
        public const string ObKey_SetChooseBanker = "OnChooseBanker_PaiJiu";//设置选庄显影 //ChooseBanker
        public const string ObKey_SetRobBanker = "OnRobBanker_PaiJiu";//设置是否抢庄状态显影 //SetRobBanker

        //public static string ObKey_SetStartGameBtn = "OnOffStartGame_PaiJiu";//设置开始按钮显影(无)
        public const string ObKey_SetPokerStatus = "OnPokerClick_PaiJiu";//点击UIPoker
        public const string ObKey_SetOpenPokers = "OnOpenPokers_PaiJiu";//变更手牌牌状态（开牌）
        public const string ObKey_OpenViewPaiJiu = "GameCtrlOpenView_PaiJiu";//由GameCtrl加载窗口
        public const string ObKey_btnResultViewBack = "btnResultViewBack_PaiJiu";//游戏总结算返回大厅
        public const string ObKey_btnResultViewShare = "btnResultViewShare_PaiJiu";//游戏总结算炫耀战绩
        public const string ObKey_SetEnterRoomView = "SetEnterRoomView_PaiJiu";//刚进入游戏时 加载已有窗口

        public const string ObKey_SetBankerAni = "SetBankerAni_PaiJiu";//设置UIItem庄家标识动画
        public const string ObKey_SetGoldAni = "SetGoldAni_PaiJiu";//设置UIItem金币刷新动画
        public const string ObKey_btnCutPoker = "OnBtnCutPoker_PaiJiu";//设置切牌按钮点击
        public const string ObKey_btnQieGuo = "OnBtnQieGuo_PaiJiu";//设置切锅按钮点击
        public const string ObKey_SettleOnComplete = "OnSettleOnComplete_PaiJiu";//结算完成回调
        
        //------------------加载路径------------------------------------------
        public const string UIItemsPath = "download/{0}/prefab/uiprefab/uiitems/{1}.drb";//    UI预制体路径
        public const string MaJiangPrefabPath = "download/{0}/prefab/model/{1}.drb";//    麻将预制体路径
        public const string UISpritePath = "download/{0}/source/uisource/paijiuroom.drb";//   UI图片路径




        //------------------按钮名字------------------------------------------
        public const string BtnSetting = "btn_setting";//设置按钮
        public const string BtnPaiJiuViewChat = "btnPaiJiuViewChat";//聊天按钮
        public const string BtnPaiJiuViewShare = "btnPaiJiuViewShare";//微信邀请
        public const string BtnPaiJiuViewReady = "btnPaiJiuViewReady";//准备




        //预制体名字UIItemEarnings_PaiJiu
        public const string UIItemNameEarnings = "UIItemEarnings_PaiJiu";//飘分预制体
        public const string UIItemNameGold = "UIItemGold_paijiu";//金币预制体
        public const string UIItemName = "UIItemGold_paijiu";//金币预制体
        public const string GoldImage_NiuNiu = "gold_image";//金币图片



        //声音资源名字
        public static string GoldMove_paijiu = "GoldMove_paijiu";//金币声音
        public static string PokerType_paijiu  =  "pokertype{0}_paijiu";//牌型声音
         
        public static string KaiPai_paijiu = "kaipai_paijiu";//开牌
        public static string TongPei_paijiu = "tongpei_paijiu";//通赔
        public static string TongSha_paijiu = "tongsha_paijiu";//通杀
        public static string FaPai_paijiu = "fapai_paijiu";//发牌

        public static string AuidoVictorySettle_paijiu = "VictorySettle_paijiu";//小结胜利声音
        public static string AudioFailureSettle_paijiu = "FailureSettle_paijiu";//小结失败声音
       //tag 标签
       //public static string HandPokerTag = "Player";

        //UI动画
        public const string UIAniBaoGuo_JuYou = "UIAnimation_BaoGuo";//爆锅（本局庄家分 >0）
        public const string UIAniZhaGuo_JuYou = "UIAnimation_ZhaGuo";//炸锅
    }
}