//===================================================
//Author      : WZQ
//CreateTime  ：5/31/2017 11:54:19 AM
//Description ：设置手机信息UI：房间号 电量 wifi 
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NiuNiu
{
    public class UIItemRoomInfo_NiuNiu : UIItemRoomInfoBase
    {

       
        private static UIItemRoomInfo_NiuNiu instance;

        public static UIItemRoomInfo_NiuNiu Instance { get { return instance; } }

        private NiuNiu.Room room;
        protected override void OnAwake()
        {
            base.OnAwake();
            instance = this;
            ModelDispatcher.Instance.AddEventListener(ConstDefine_NiuNiu.ObKey_RoomInfoChanged, OnRoomInfoChanged); //(重新注册设置局数)
            //ModelDispatcher.Instance.AddEventListener(ConstDefine_NiuNiu, OnRoomInfoChanged);
        }

        protected override void BeforeOnDestroy()
        {
            base.BeforeOnDestroy();
            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_NiuNiu.ObKey_RoomInfoChanged, OnRoomInfoChanged); //(重新注册设置局数)
        }





        protected override void OnStart()
        {
            base.OnStart();
        }


        /// <summary>
        /// 房间信息变更 （base中已注册  若要改变 需覆盖该方法）
        /// </summary>
        /// <param name="obj"></param>
        private void OnRoomInfoChanged(TransferData data)
        {
            Room entity = data.GetValue<Room>("Room");
            ShowLoop(entity.currentLoop, entity.maxLoop);
            //SetBaseScore(entity.baseScore);
        }


     
        private  void ShowLoop(int currentLoop, int maxLoop)
        {
            m_TextLoop.SafeSetText(string.Format("{0}/{1}局", currentLoop, maxLoop));

        }




    }
}