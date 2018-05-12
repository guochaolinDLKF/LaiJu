//===================================================
//Author      : WZQ
//CreateTime  ：8/10/2017 11:09:33 AM
//Description ：聚友字符串
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuYou
{
    public static class ConstDefine_JuYou
    {
        //ConstDefine_JuYou
        //----------------观察者事件Key------------------------------------------

        public const string ObKey_RoomInfoChanged = "OnRoomInfoChanged_JuYou";// 房间信息变更 (局数 等)
        public const string ObKey_SeatInfoChanged = "OnSeatInfoChanged_JuYou";// 座位信息变更 UI 及 3d场景OnBtnPaiJiuViewPour

        public const string ObKey_SeatGoldChanged = "OnSeatGoldChanged_JuYou"; //刷新座位金币
        public const string ObKey_RoomGoldChanged = "OnRoomGoldChanged_JuYou"; //刷新房间底注
        public const string ObKey_SendSeatGoldChanged = "SendSeatGoldChanged_JuYou"; //发送刷新座位金币
        public const string ObKey_SendRoomGoldChanged = "SendRoomGoldChanged_JuYou"; //发送刷新房间底注

        public const string ObKey_btnStartGame = "OnBtnStartGame_JuYou";//庄家开始游戏点击
        public const string ObKey_btnPour = "btnPour_JuYou";// 下注按钮 发送下注选分
        public const string ObKey_btnChooseBanker = "btnChooseBanker_JuYou";// 是否坐庄ChooseBanker
        public const string ObKey_SetPourBtn = "OnOffPour_JuYou";//设置下注按钮显影
        public const string ObKey_SetChooseBanker = "OnChooseBanker_JuYou";//设置选庄显影 //ChooseBanker
        public const string ObKey_SetPokerStatus = "OnPokerClick_JuYou";//点击UIPoker


        public const string ObKey_OpenViewJuYou = "GameCtrlOpenView_JuYou";//由GameCtrl加载窗口
        public const string ObKey_btnResultViewBack = "btnResultViewBack_JuYou";//游戏总结算返回大厅
        public const string ObKey_btnResultViewShare = "btnResultViewShare_JuYou";//游戏总结算炫耀战绩
        public const string ObKey_SetEnterRoomView = "SetEnterRoomView_JuYou";//刚进入游戏时 加载已有窗口
        public const string ObKey_AloneSettle = "ObKey_AloneSettle_JuYou";//获取个人结算信息
        public const string ObKey_NextGame = "ObKeyNextGame_JuYou";//开始下一把
        public const string ObKey_GameOver = "ObKeyGameOver_JuYou";//游戏结束
        //------------------加载路径------------------------------------------
        public const string UIItemsPath = "download/{0}/prefab/uiprefab/uiitems/{1}.drb";//    UI预制体路径
        public const string MaJiangPrefabPath = "download/{0}/prefab/model/{1}.drb";//    麻将预制体路径
        public const string UISpritePath = "download/{0}/source/uisource/paijiuroom.drb";//   UI图片路径




        //------------------按钮名字------------------------------------------
        public const string BtnSetting = "btn_setting";//设置按钮
        public const string BtnViewChat = "btnViewChat";//聊天按钮
        public const string BtnViewShare = "btnJuYouViewShare";//微信邀请
        public const string BtnViewReady = "btnJuYouViewReady";//准备
        public const string BtnViewStart = "btnJuYouViewStart";//开始



        //预制体名字UIItemEarnings_PaiJiu
        public const string UIItemNameEarnings = "UIItemEarnings_JuYou";//飘分预制体
        public const string UIItemNameGold = "UIItemGold_juyou";//金币预制体
        public const string UIItemName = "UIItemGold_juyou";//金币预制体
        public const string GoldImage_JuYou = "gold_image";//金币图片



        //声音资源名字
        public const string AudioGoldMove = "goldmove";//金币声音
        public const string AudioZhaGuo = "zhaguo";//炸锅声音
        public const string AudioVictory = "victory";//胜利声音
        public const string AudioFailure = "failure";//失败声音
        public const string AudioDou = "dou_{0}";//兜声音
        public const string AudioBuDou = "pass_{0}";//不兜声音
        public const string AudioQuanDou = "quandou_{0}";//全兜声音


        //------------------动画------------------------------------------
        public const string UIAniBaoGuo_JuYou = "UIAnimation_BaoGuo";//爆锅（到达上限）
        public const string UIAniZhaGuo_JuYou = "UIAnimation_ZhaGuo";//炸锅
        public const string UIAniGoldMove_JuYou = "UIAnimation_Gold";//金币移动

    }
}