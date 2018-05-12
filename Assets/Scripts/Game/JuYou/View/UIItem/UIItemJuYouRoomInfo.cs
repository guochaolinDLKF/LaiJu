//===================================================
//Author      : WZQ
//CreateTime  ：8/17/2017 7:20:33 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace JuYou
{
    public class UIItemJuYouRoomInfo : UIItemRoomInfoBase
    {
        public static UIItemJuYouRoomInfo Instance;

        //[SerializeField]
        //protected Text m_Guo;//分锅


        protected override void OnAwake()
        {
            base.OnAwake();
            Instance = this;
           
            ModelDispatcher.Instance.AddEventListener(ConstDefine_JuYou.ObKey_RoomInfoChanged, OnRoomInfoChanged); //(重新注册设置局数)
            //ModelDispatcher.Instance.AddEventListener(ConstDefine_JuYou.ObKey_RoomGoldChanged, OnRoomGoldChanged);//房间底注
        }

        protected override void BeforeOnDestroy()
        {
            base.BeforeOnDestroy();
          
            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_JuYou.ObKey_RoomInfoChanged, OnRoomInfoChanged);
            //ModelDispatcher.Instance.RemoveEventListener(ConstDefine_JuYou.ObKey_RoomGoldChanged, OnRoomGoldChanged);//房间底注
        }

     

        /// <summary>
        /// 房间信息变更 （base中已注册  若要改变 需覆盖该方法）
        /// </summary>
        /// <param name="obj"></param>
        private void OnRoomInfoChanged(TransferData data)
        {
            Debug.Log("子类UIItemPaiJiuRoomInfo");
            RoomEntity entity = data.GetValue<RoomEntity>("Room");
            ShowLoop(entity.currentLoop, entity.maxLoop);
            

            ////房间底注由 初始及动画更新
            //SetBaseScore(entity.baseScore);
        }

        /// <summary>
        /// 设置房间第注 由初始及动画事件变更
        /// </summary>
        /// <param name="data"></param>
        private void OnRoomGoldChanged(TransferData data)
        {
            RoomEntity entity = data.GetValue<RoomEntity>("Room");
            SetBaseScore(entity.baseScore);
        }



        private void ShowLoop(int currentLoop, int maxLoop)
        {
            m_TextLoop.SafeSetText(string.Format("游戏局数:{0}/{1}", currentLoop, maxLoop));
        }



        private void SetBaseScore(int baseScore)
        {
            m_TextBaseScore.transform.parent.gameObject.SetActive(baseScore > 0);
            m_TextBaseScore.SafeSetText(baseScore.ToString());
        }

        //private void ShowGuo(int guo)
        //{
        //    m_Guo.SafeSetText(string.Format("分锅限制:{0}", guo));
        //}

    }
}