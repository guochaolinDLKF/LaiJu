//===================================================
//Author      : WZQ
//CreateTime  ：11/29/2017 9:56:39 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PaoDeKuai {
    public class UIItemPaoDeKuaiOperator : UIItemBase
    {

        [SerializeField]
        private GameObject m_QiangGuan;//抢关

        [SerializeField]
        private GameObject m_ChuPaiItem;//出牌

        public override Dictionary<string, ModelDispatcher.Handler> DicNotificationInterests()
        {
            Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
            dic.Add(ConstDefine_PaoDeKuai.ON_OperateState_CHANGED, OnOperateStateChanged);//通知玩家操作状态变更
             

            return dic;
        }


        protected override void OnAwake()
        {
            base.OnAwake();

            Button[] arr = GetComponentsInChildren<Button>();
            for (int i = 0; i < arr.Length; ++i)
            {
                EventTriggerListener.Get(arr[i].gameObject).onClick = OnBtnClick;
            }
            m_QiangGuan.SetActive(false);
            m_ChuPaiItem.SetActive(false);
        }

        private void OnBtnClick(GameObject go)
        {
            switch (go.name)
            {
                case ConstDefine_PaoDeKuai.BtnPDKViewBuQiang://抢关不抢
                    SendNotification(ConstDefine_PaoDeKuai.BtnPDKViewBuQiang);
                    break;
                case ConstDefine_PaoDeKuai.BtnPDKViewQiang://抢关
                    SendNotification(ConstDefine_PaoDeKuai.BtnPDKViewQiang);
                    break;
                case ConstDefine_PaoDeKuai.BtnPDKViewBuChu://不出牌
                    SendNotification(ConstDefine_PaoDeKuai.BtnPDKViewBuChu);
                    break;
                case ConstDefine_PaoDeKuai.BtnPDKViewTiShi://提示
                    SendNotification(ConstDefine_PaoDeKuai.BtnPDKViewTiShi);
                    break;
                case ConstDefine_PaoDeKuai.BtnPDKViewChuPai://出牌
                    SendNotification(ConstDefine_PaoDeKuai.BtnPDKViewChuPai);
                    break;

            }
        }


        /// <summary>
        /// 玩家操作状态变更
        /// </summary>
        private void OnOperateStateChanged(TransferData data)
        {
            bool isPlayer = data.GetValue<bool>("IsPlayer");
            if (isPlayer)
            {
                RoomEntity.RoomStatus roomStatus = data.GetValue<RoomEntity.RoomStatus>("RoomStatus");
                SeatEntity.SeatStatus seatStatus = data.GetValue<SeatEntity.SeatStatus>("PlayerStatus");
                RefreshAll(roomStatus, seatStatus);
            }

        }










        /// <summary>
        /// 刷新全部操作项目
        /// </summary>
        /// <param name="roomStatus"></param>
        /// <param name="seatStatus"></param>
        public void RefreshAll(RoomEntity.RoomStatus roomStatus,SeatEntity.SeatStatus seatStatus)
        {

            ShowChuPaiItem( roomStatus==  RoomEntity.RoomStatus.Begin&& seatStatus == SeatEntity.SeatStatus.Operate);
            ShowQiangGuan(false);
        }




        public void ShowQiangGuan(bool isShow)
        {
            if (m_QiangGuan != null)
                m_QiangGuan.SetActive(isShow);
        }

        public void ShowChuPaiItem(bool isShow)
        {
            if (m_ChuPaiItem != null)
                m_ChuPaiItem.SetActive(isShow);
        }



    }
}