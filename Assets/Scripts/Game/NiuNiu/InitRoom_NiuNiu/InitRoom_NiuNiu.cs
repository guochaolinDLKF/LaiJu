//===================================================
//Author      : WZQ
//CreateTime  ：6/1/2017 4:20:10 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NiuNiu
{
    public class InitRoom_NiuNiu : SceneCtrlBase
    {

     
        private UISceneView_NiuNiu m_View;


        protected override void OnAwake()
        {
            base.OnAwake();

            m_View = UIViewManager.Instance.LoadSceneUIFromAssetBundle(UIViewManager.SceneUIType.NiuNiu2D, () =>
            {
            }).GetComponent<UISceneView_NiuNiu>();

                //其中房间信息来源更改结构脚本后 应为RoomNiuNiuProxy
                m_View.InitEnterRoomUI(RoomNiuNiuProxy.Instance.CurrentRoom, RoomNiuNiuProxy.Instance.PlayerSeat, RoomNiuNiuProxy.Instance.playerNumber);

                //设置房间UI   由模型层发消息
                RoomNiuNiuProxy.Instance.SendRoomInfoChangeNotify();



            //设置bgm
            AudioBackGroundManager.Instance.Play(NiuNiu.ConstDefine_NiuNiu.BGM_NiuNiu);

          if(DelegateDefine.Instance.OnSceneLoadComplete != null)  DelegateDefine.Instance.OnSceneLoadComplete();

        }

        //游戏切入切出
        private void OnApplicationPause(bool isPause)
        {
            if (!isPause)
            {
                if (!NetWorkSocket.Instance.Connected(GameCtrl.Instance.SocketHandle))
                {
                    //if (RoomMaJiangProxy.Instance.CurrentRoom.Status != RoomEntity.RoomStatus.Replay)
                    //{
                    NiuNiuGameCtrl.Instance.RebuildRoom();
                    //}
                }
            }
        }








    }
}