//===================================================
//Author      : DRB
//CreateTime  ：9/1/2017 3:46:30 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GuPaiJiu
{
    public class ConstantGuPaiJiu
    {
        //————————————————————按钮点击方法类——————————————————————————————————————
        public const string GuPaiJiuClientSendReady = "GuPaiJiuClientSendReady";//执行准备的方法
        public const string GuPaiJiuClientSendGameStart = "GuPaiJiuClientSendGameStart";//开局
        public const string GuPaiJiuClientSendBottomPour = "GuPaiJiuClientSendBottomPour";//向服务器发送下注分数
        public const string GuPaiJiuClientSendGroupPoker = "GuPaiJiuClientSendGroupPoker";//客户端发送组合牌
        public const string GuPaiJiuClientEmptyReceive = "GuPaiJiuClientEmptyReceive";//客户端发送发牌结束
        public const string GuPaiJiuClientSendInformNext = "GuPaiJiuClientSendInformNext";//客户端发送开始下一次
        public const string GuPaiJiuClisentSendAutoGroupPoker = "GuPaiJiuClisentSendAutoGroupPoker";//客户端发送自动组合牌
        public const string GuPaiJiuClisentSendPokerWallEnd = "GuPaiJiuClisentSendPokerWallEnd";//客户端发送牌墙生产完毕
        public const string GuPaiJiuClisentSendCutPoker = "GuPaiJiuClisentSendCutPoker";//客户端发送切牌或者不切

        public const string OnBtnResultViewGuPaiJiuShareClick = "OnBtnResultViewGuPaiJiuShareClick";










        //-------------------------------------------按钮名字----------------------------------------------------------------------------------------
        public const string btnGuPaiJiuViewChat = "btnGuPaiJiuViewChat";//聊天按钮
        public const string btnGuPaiJiuViewShare = "btnGuPaiJiuViewShare";//微信邀请
        public const string btnGuPaiJiuViewReady = "btnGuPaiJiuViewReady";//准备
        public const string btnGuPaiJiuViewOpening = "btnGuPaiJiuViewOpening";//开局
        public const string btnGuPaiJiuViewCut = "btnGuPaiJiuViewCut";//切牌
        public const string btnGuPaiJiuViewNoCut = "btnGuPaiJiuViewNoCut";//不切牌
        public const string btnGuPaiJiuQiang = "btnGuPaiJiuQiang";//抢庄
        public const string btnGuPaiJiuNoQiang = "btnGuPaiJiuNoQiang";//不抢庄
        public const string btnGuPaiJiuViewQuanOpen = "btnGuPaiJiuViewQuanOpen";//全开
        public const string btnGuPaiJiuCutPan = "btnGuPaiJiuCutPan";//切锅
        public const string btnGuPaiJiuNoCutPan = "btnGuPaiJiuNoCutPan";//不切







        //---------------------------------------------下注按钮-----------------------------------------------------------------------------------
        public const string OnGuPaibtnhand = "OnGuPaibtnhand";//头
        public const string OnGuPaibtnavenue = "OnGuPaibtnavenue";//二道
        public const string OnGuPaibtnthree = "OnGuPaibtnthree";//三道
        public const string OnGuPaibtnComplete = "OnGuPaibtnComplete";//下注完成完成
        public const string OnGuPaibtnWithdraw = "OnGuPaibtnWithdraw";//撤回
        public const string OnGuPaibtnGroupComplete = "OnGuPaibtnGroupComplete";//组合牌完成的点击
        public const string OnGuPaibtnGroupComplete1 = "OnGuPaibtnGroupComplete1";//组合牌完成的点击
        public const string OnGuPaibtnAutomaticPoker = "OnGuPaibtnAutomaticPoker";//自动配牌
        public const string OnGuPaibtnBeginPoker = "OnGuPaibtnBeginPoker";//搓牌开牌
        public const string OnGuPaiJiubtnPrompt = "OnGuPaiJiubtnPrompt";//提示
        public const string OnGuPaibtnDetermine = "OnGuPaibtnDetermine";//确定




        //-----------------------------------------------//数据类调用的方法（Proxy）-------------------------------------------------------------------
        public const string OnGuPaiSetBetPour = "OnGuPaiSetBetPour";//设置下注分数
        public const string OnGuPaiTellBetInfoChanged = "OnGuPaiTellBetInfoChanged";//通知下注
        public const string CloseCutPokerImage = "OnGuPaiCloseCutPokerImage";//通知下注关闭切牌等待提示
        public const string RollDice = "OnGuPaiRollDice";//摇骰子
        public const string ShuffleAnimation = "OnGuPaiShuffleAnimation";//开局动画
        public const string GroupValidPoker = "OnGuPaiGroupValidPoker";//通知玩家开始组合牌
        public const string GroupEnd = "OnGuPaiGroupEnd";//组合拍结束
        public const string GroupEndJieSuan = "OnGuPaiGroupEndJieSuan";//结算的时候实例化别人的牌
        public const string EndIamge = "OnGuPaiEndIamge";//组合完成显示组合完成的图片
        public const string SetLeave = "OnGuPaiSetLeave";//设置玩家离开图片
        public const string PlayMusic = "OnGuPaiPlayMusic";//每次结算的时候播放音乐
        public const string LoadSmallResult = "OnGuPaiLoadSmallResult";//小结算
        public const string CloseHandContainer = "OnGuPaiCloseHandContainer";//清空手牌挂载点
        public const string TellIsBankeDraw = "OnGuPaiTellIsBankeDraw";//通知翻牌
        public const string DrawPoker = "OnGuPaiDrawPoker";//翻牌方法
        public const string CloseDrawPoker = "OnGuPaiCloseDrawPoker";//结算的时候关闭翻牌
        public const string TellCutPoker = "OnGuPaiTellCutPoker";//通知切牌
        public const string StartCutPoker = "OnGuPaiStartCutPoker";//开始切牌改变房间状态
        public const string StartCutPoker1 = "OnGuPaiStartCutPoker1";//向服务器发送切牌的顿数
        public const string NoCutPoker = "OnGuPaiNoCutPoker";
        public const string CutPokerAni = "OnGuPaiCutPokerAni";//切牌动画
        public const string CutPokerEnd = "OnGuPaiCutPokerEnd";//切牌完毕
        public const string TellCutPan = "OnGuPaiTellCutPan";//切锅
        public const string CutPanResult = "OnGuPaiCutPanResult";//切锅结果
        public const string GarbBankerSceneView = "OnGuPaiGarbBankerSceneView";//通知抢庄
        public const string ISImageGarbBanker = "OnGuPaiISImageGarbBanker";//显示抢庄或者不抢的图片
        public const string BigDealAniPoker = "OnGuPaiBigDealAniPoker";
        public const string BigDealAni = "OnGuPaiBigDealAni";//发牌动画
        public const string CloseTime = "OnGuPaiCloseTime";//关闭时间


        public const string SetSeatGold = "OnGuPaiSetSeatGold";//更新座位分数
        public const string isSend = "OnGuPaiisSend";//是否向服务器发自动配牌的消息

        public const string LoadRoomPoker = "OnGuPaiLoadRoomPoker";//加载房间牌
        public const string CloseRoomPokerTran = "OnGuPaiCloseRoomPokerTran";//清空房间牌的挂载点

        public const string OnGuPaiRoomInfoChanged = "OnGuPaiOnGuPaiRoomInfoChanged";

        public const string OnGuPaiJiuPromptPoker = "OnGuPaiJiuPromptPoker";//提示

        public const string OnSeatCtrlGroupPoker = "OnSeatCtrlGroupPoker";//自合牌






    }
}
