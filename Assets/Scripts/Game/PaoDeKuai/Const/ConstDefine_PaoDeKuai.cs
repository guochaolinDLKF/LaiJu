//===================================================
//Author      : WZQ
//CreateTime  ：11/21/2017 10:55:19 AM
//Description ：跑得快字符串常量
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaoDeKuai
{
    public class ConstDefine_PaoDeKuai
    {

  
        //======================================Model数据变化消息======================================
        public const string ON_COUNTDOWN_CHANGED = "OnPDKCountDownChanged";//倒计时变更

        public const string ON_ROOM_INFO_CHANGED = "OnPDKRoomInfoChanged";//房间信息变更

        public const string ON_SEAT_INFO_CHANGED = "OnPDKSeatInfoChanged";//座位信息变更

        public const string ON_OperateState_CHANGED = "OnPDKOperateStateChanged";//玩家操作状态变更

        public const string ON_StatePass_CHANGED = "OnPDKPassStateChanged";//玩家Pass状态变更

        public const string ON_HistoryPoker_CHANGED = "OnPDKHistoryPokerChanged";//历史出牌变更

        //玩家金币变更。。。。。。。。

        public const string ON_COUNT_DOWN_CHANGED = "OnCountDownUpdate";//设置倒计时
        public const string ON_SEAT_GOLD_CHANGED = "OnPDKSeatGoldChanged";//设置倒计时  

        //======================================游戏场景按钮======================================
        public const string BtnPDKViewChat = "btnPDKViewChat";//聊天
        public const string BtnPDKViewAuto = "btnPDKViewAuto";//托管
        public const string BtnPDKViewCancelAuto = "btnPDKViewCancelAuto";//取消托管
        public const string BtnPDKViewShare = "btnPDKViewShare";//微信邀请
        public const string BtnPDKViewReady = "btnPDKViewReady";//准备
        public const string BtnPDKViewCancelReady = "btnPDKViewCancelReady";//取消准备
        public const string BtnPDKSettleViewReady = "btnPDKSettleViewReady";//结算界面准备
        public const string BtnPDKSettleViewResult = "btnPDKSettleViewResult";//结算界面查看结果按钮
        public const string BtnPDKResultViewBack = "btnPDKResultViewBack";//结束界面返回按钮
        public const string BtnPDKResultViewShare = "btnPDKResultViewShare";//结束界面分享按钮
        public const string BtnPDKViewJiPaiQi = "btnPDKViewJiPaiQi";//记牌器点击
        public const string BtnPDKViewHeadClick = "OnPDKViewHeadClick";//头像点击


        public const string BtnPDKViewBuQiang = "btnPDKViewBuQiang";//抢关不抢
        public const string BtnPDKViewQiang = "btnPDKViewQiang";//抢关
        public const string BtnPDKViewBuChu = "btnPDKViewBuChu";//不出牌
        public const string BtnPDKViewTiShi = "btnPDKViewTiShi";//提示
        public const string BtnPDKViewChuPai = "btnPDKViewChuPai";//出牌


     






        //======================================ItemName=======================================

        public const string UIItemNamePoker = "UIItemPDKPoker";//poker预制体名字
        public const string UIItemNameSpades3 = "UIItemPDKSpadesThree";//黑桃3动画UIItemPDKSpades3
        public const string UIItemNameResult = "UIItemResult_PaoDeKuai";//结束界面玩家信息Item
        //======================================图片名字=======================================
        public const string SpriteNameDefaultPoker = "0_0_paodekuai";//默认poker图片名字

    }
}