//===================================================
//Author      : DRB
//CreateTime  ：9/19/2017 2:32:56 PM
//Description ：询问操作命令
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DRB.MahJong
{
    public class AskOperateCommand : IGameCommand
    {
        private List<PokerCombinationEntity> m_AskPokerGroup;

        private long m_CountDown;

        public AskOperateCommand(List<PokerCombinationEntity> askPokerGroup, long countDown)
        {
            m_AskPokerGroup = askPokerGroup;
            m_CountDown = countDown;
        }


        public void Execute()
        {
            Debug.Log("===========================================执行询问吃碰杠胡命令");
            AudioEffectManager.Instance.Play("daojishi", Vector3.zero, false);
            ShowOperate(m_AskPokerGroup);
            RoomMaJiangProxy.Instance.SetCountDown(m_CountDown, true);
        }

        public void Revoke()
        {

        }

        #region ShowOperate 显示吃碰杠胡
        /// <summary>
        /// 显示吃碰杠胡
        /// </summary>
        /// <param name="pbGroup"></param>
        public void ShowOperate(List<PokerCombinationEntity> pbGroup)
        {
            if (pbGroup == null)
            {
                throw new Exception("服务器询问吃碰杠的列表是空的");
            }

            RoomMaJiangProxy.Instance.AskPokerGroup = pbGroup;

            bool canHu = false;
            bool isZiMo = false;
            List<Poker> lstPeng = null;
            List<List<Poker>> lstGangs = null;
            List<List<Poker>> lstChi = null;
            List<List<Poker>> lstZhiDui = null;
            List<List<Poker>> lstChiTing = null;
            List<Poker> lstPengTing = null;
            List<List<Poker>> lstLiangXi = null;
            bool isMustZhiDui = false;
            bool canDingZhang = false;
            bool canDingJiang = false;
            List<List<Poker>> lstKou = null;
            List<Poker> lstBuXi = null;
            bool canPiaoTing = false;
            bool isPao = false;
            bool isJiao = false;
            bool isMingTi = false;

            for (int i = 0; i < pbGroup.Count; ++i)
            {
                OperatorType type = pbGroup[i].CombinationType;
                Debug.Log("客户端可以" + type);
                Poker poker = null;
                if (pbGroup[i].PokerList.Count > 0)
                {
                    poker = pbGroup[i].PokerList[0];
                    Debug.Log(poker.ToString("{0}_{1}_{2}"));
                }
                switch (type)
                {
                    case OperatorType.Peng:
                        lstPeng = RoomMaJiangProxy.Instance.GetPeng(poker);
                        if (lstPeng == null || lstPeng.Count == 0)
                        {
                            AppDebug.ThrowError("没检测出来可以碰");
                        }
                        break;
                    case OperatorType.Gang:
                        if (poker == null)
                        {
                            lstGangs = RoomMaJiangProxy.Instance.GetAnGang();

                            List<Poker> lstBuGangs = RoomMaJiangProxy.Instance.GetBuGang();
                            for (int j = 0; j < lstBuGangs.Count; ++j)
                            {
                                lstGangs.Add(new List<Poker> { lstBuGangs[j] });
                            }
                        }
                        else
                        {
                            lstGangs = RoomMaJiangProxy.Instance.GetMingGang(poker);
                        }
                        if (lstGangs == null || lstGangs.Count == 0)
                        {
                            AppDebug.ThrowError("没检测出来可以杠,服务器发送的poker是" + (poker == null ? "空的" : poker.ToString()));
                        }
                        break;
                    case OperatorType.Chi:
                        lstChi = RoomMaJiangProxy.Instance.GetChi(poker);
                        if (lstChi == null || lstChi.Count == 0)
                        {
                            AppDebug.ThrowError("没检测出来可以吃");
                        }
                        break;
                    case OperatorType.Hu:
                        canHu = true;
                        isZiMo = pbGroup[i].SubTypeId == 2;
                        break;
                    case OperatorType.ZhiDui:
                        lstZhiDui = RoomMaJiangProxy.Instance.GetZhiDui(out isMustZhiDui);
                        break;
                    case OperatorType.ChiTing:
                        lstChiTing = RoomMaJiangProxy.Instance.GetChiTing(poker);
                        if (lstChiTing == null || lstChiTing.Count == 0)
                        {
                            AppDebug.ThrowError("没检测出来可以吃听");
                        }
                        break;
                    case OperatorType.PengTing:
                        lstPengTing = RoomMaJiangProxy.Instance.GetPengTing(poker);
                        if (lstPengTing == null || lstPengTing.Count == 0)
                        {
                            AppDebug.ThrowError("没检测出来可以碰听");
                        }
                        break;
                    case OperatorType.LiangXi:
                        lstLiangXi = RoomMaJiangProxy.Instance.GetLiangXi();
                        if (lstLiangXi == null || lstLiangXi.Count == 0)
                        {
                            AppDebug.ThrowError("没检测出来可以亮喜");
                        }
                        break;
                    case OperatorType.DingZhang:
                        canDingZhang = true;
                        break;
                    case OperatorType.FengGang:
                        List<Poker> lstFengGang = RoomMaJiangProxy.Instance.GetFengGang();
                        if (lstFengGang != null)
                        {
                            if (lstGangs == null)
                            {
                                lstGangs = new List<List<Poker>>();
                            }
                            lstGangs.Add(lstFengGang);
                        }
                        break;
                    case OperatorType.DingJiang:
                        Poker dingjiang = RoomMaJiangProxy.Instance.GetDingJiang(poker);
                        if (dingjiang != null)
                        {
                            canDingJiang = true;
                        }
                        break;
                    case OperatorType.Kou:
                        lstKou = RoomMaJiangProxy.Instance.GetKou();
                        break;
                    case OperatorType.BuXi:
                        lstBuXi = RoomMaJiangProxy.Instance.GetBuXi();
                        if (lstBuXi == null || lstBuXi.Count == 0)
                        {
                            AppDebug.ThrowError("没检测出来可以补喜");
                        }
                        break;
                    case OperatorType.PiaoTing:
                        canPiaoTing = true;
                        break;
                    case OperatorType.Pao:
                        isPao = true;
                        break;
                    case OperatorType.Jiao:
                        isJiao = true;
                        break;
                    case OperatorType.MingTi:
                        isMingTi = true;
                        break;
                }
            }

            if (UIViewManager.Instance.CurrentUIScene is UISceneMaJiangView)
            {
                if (lstZhiDui != null)
                {
                    UIItemOperator.Instance.ShowZhiDui(lstZhiDui, !isMustZhiDui);
                }
                else
                {
                    UIItemOperator.Instance.Show(lstChi, lstPeng, lstGangs, canHu, isZiMo, lstChiTing,
                        lstPengTing, lstLiangXi, canDingZhang, canDingJiang, lstKou, lstBuXi,
                        canPiaoTing, isPao, isJiao, isMingTi);
                }
            }
        }
        #endregion
    }
}
