//===================================================
//Author      : DRB
//CreateTime  ：12/2/2017 4:49:55 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.sss;

namespace ShiSanZhang
{
    public class ShiSanZhangSceneCtrl : SceneCtrlBase
    {
        public static ShiSanZhangSceneCtrl Instance;
        private UISceneShiSanZhangView m_UISceneShiSanZhangView;

        protected override void OnAwake()
        {
            base.OnAwake();
            Instance = this;
        }
        protected override void OnStart()
        {
            base.OnStart();
            if (DelegateDefine.Instance.OnSceneLoadComplete != null)
            {
                DelegateDefine.Instance.OnSceneLoadComplete();
            }
            GameObject go = UIViewManager.Instance.LoadSceneUIFromAssetBundle(UIViewManager.SceneUIType.ShiSanZhang);
            m_UISceneShiSanZhangView = go.GetComponent<UISceneShiSanZhangView>();
           // UIItemRoomInfo_ShiSanZhang.Instance.SetUI(RoomShiSanZhangProxy.Instance.CurrentRoom.roomId, RoomShiSanZhangProxy.Instance.CurrentRoom.BaseScore);//设置房间底分
            RoomShiSanZhangProxy.Instance.SendRoomInfoChangeNotify();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (ShiSanZhangGameCtrl.Instance.CommandQueue.Count > 0)
            {
                IGameCommand command = ShiSanZhangGameCtrl.Instance.CommandQueue.Dequeue();
                command.Execute();
            }

            if (!m_IsSelectedPoker && Input.GetMouseButtonDown(0))
            {
                if (RoomShiSanZhangProxy.Instance.CurrentRoom.SszRoomStatus != ROOM_STATUS.ROOM_STATUS_MATEPOKER) return;

                Ray ray = m_UISceneShiSanZhangView.CurrentCamare.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Debug.Log("-------------Ray ray -----------------");
                    if (hit.transform.gameObject.layer != 1 << LayerMask.NameToLayer("PlayerHand")) return;
                }
                UIItemPoker_ShiSanZhang ctrl = GetRayHitPoker();
                if (ctrl != null)
                {                
                    m_EndSelectedPoker = null; //结束选中的牌
                    m_CurrSelectedPoker.Clear(); //当前拖拽选中的牌
                    //获得第一张牌
                    m_StartSelectedPoker = ctrl;
                    m_CurrSelectedPoker.Add(ctrl);
                    m_IsSelectedPoker = true;
                    ctrl.SetSelected(true);
                }
            }
            if (m_IsSelectedPoker && Input.GetMouseButtonUp(0))
            {
                for (int i = 0; i < m_CurrSelectedPoker.Count; ++i)
                {
                    m_CurrSelectedPoker[i].SetSelected(false);
                    m_CurrSelectedPoker[i].isSelect = !m_CurrSelectedPoker[i].isSelect;
                }
                ResetDragPoker();
                UITipPokerShiSanZhangView.Instance.TipKuang();
            }
        }

      


        /// <summary>
        /// 开局
        /// </summary>
        /// <param name="room"></param>
        public void Begin(RoomEntity room, bool isPlayAnimation)
        {
            m_UISceneShiSanZhangView.Begin(room, isPlayAnimation);
        }





        private UIItemPoker_ShiSanZhang m_StartSelectedPoker;//初始选中的牌
        private UIItemPoker_ShiSanZhang m_EndSelectedPoker; //结束选中的牌
        private bool m_IsSelectedPoker;
        private List<UIItemPoker_ShiSanZhang> m_CurrSelectedPoker = new List<UIItemPoker_ShiSanZhang>();//当前拖拽选中的牌
        private List<Poker> m_CurrLiftUpPoker = new List<Poker>();//当前抬起的牌
        public List<Poker> CurrLiftUpPoker { get { return m_CurrLiftUpPoker; } }
        #region OnFingerBeginDrag 手势拖拽开始
        /// <summary>
        /// 手势拖拽开始
        /// </summary>
        protected override void OnFingerBeginDrag()
        {
            base.OnFingerBeginDrag();
            Debug.Log("OnFingerBeginDrag 手势拖拽开始");
            m_IsSelectedPoker = true;
        }
        #endregion

        #region OnFingerDrag 手势拖拽中
        /// <summary>
        /// 手势拖拽中
        /// </summary>
        /// <param name="screenPos"></param>
        protected override void OnFingerDrag(Vector2 screenPos)
        {
            base.OnFingerDrag(screenPos);
            Debug.Log("OnFingerDrag 手势拖拽中");
            if (m_IsSelectedPoker && m_StartSelectedPoker != null)
            {
                //计算拖拽时选中的牌
                UIItemPoker_ShiSanZhang ctrl = GetRayHitPoker();
                Debug.Log(ctrl.Poker.Size+"                                检测到的牌");                                  
                if (ctrl != null)
                {
                    m_CurrSelectedPoker.Clear();
                    m_EndSelectedPoker = ctrl;
                    for (int i = 0; i < m_CurrSelectedPoker.Count; ++i)
                    {
                        m_CurrSelectedPoker[i].SetSelected(true);                       
                    }



                    //int startIndex = PrefabManager.Instance.GetIndex(RoomPaoDeKuaiProxy.Instance.PlayerSeat.Pos, m_StartSelectedPoker);
                    //int endIndex = PrefabManager.Instance.GetIndex(RoomPaoDeKuaiProxy.Instance.PlayerSeat.Pos, m_EndSelectedPoker);

                    //List<UIItemPoker_ShiSanZhang> playerSeatPoker = PrefabManager.Instance.GetHand(RoomPaoDeKuaiProxy.Instance.PlayerSeat.Pos);
                    //int minIndex = endIndex - startIndex > 0 ? startIndex : endIndex;
                    //int maxIndex = endIndex + startIndex - minIndex;
                    //for (int i = minIndex; i <= maxIndex; ++i)
                    //{
                    //    m_CurrSelectedPoker.Add(playerSeatPoker[i]);

                    //}
                    ////设置拖拽选中状态
                    //for (int i = 0; i < playerSeatPoker.Count; ++i)
                    //{
                    //    playerSeatPoker[i].SetSelected(false);
                    //}
                    //for (int i = 0; i < m_CurrSelectedPoker.Count; ++i)
                    //{
                    //    m_CurrSelectedPoker[i].SetSelected(true);
                    //}
                }
            }
        }
        #endregion

        #region OnFingerEndDrag 手势拖拽结束
        /// <summary> 
        /// 手势拖拽结束
        /// </summary>
        protected override void OnFingerEndDrag()
        {
            base.OnFingerEndDrag();
            Debug.Log("OnFingerEndDrag 手势拖拽结束");


            ////根据拖拽选中的牌将其抬起  //根据这张牌当前状态设置抬起
            //m_IsSelectedPoker = false;

            //for (int i = 0; i < m_CurrSelectedPoker.Count; ++i)
            //{
            //    m_CurrSelectedPoker[i].SetSelected(false);
            //}

            //for (int i = 0; i < m_CurrSelectedPoker.Count; ++i)
            //{
            //    Debug.Log("手势拖拽结束 m_CurrSelectedPoker.Count; ++i" + i);
            //    m_CurrSelectedPoker[i].SetHold();
            //}




            //if (m_Temp == null) return;
            //if (m_Temp.transform.position.y - m_DragBeginPos.y > 12f)
            //{
            //    MaJiangGameCtrl.Instance.ClientSendPlayPoker(m_Temp.Poker);
            //    UIItemTingTip.Instance.Show(m_Temp, RoomMaJiangProxy.Instance.GetHu(m_Temp.Poker));
            //}
            //m_DragBeginPos = Vector3.zero;

            //if (m_Temp != null)
            //{
            //    MahJongManager.Instance.DespawnMaJiang(m_Temp);
            //    m_Temp = null;
            //}
        }
        #endregion




        private UIItemPoker_ShiSanZhang GetRayHitPoker()
        {
            UIItemPoker_ShiSanZhang ctrl = null;
            Vector3 ScreenmousePos = m_UISceneShiSanZhangView.CurrentCamare.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hitArr = Physics2D.RaycastAll(new Vector2(ScreenmousePos.x, ScreenmousePos.y), new Vector2(ScreenmousePos.x, ScreenmousePos.y - 1f), 1, 1 << LayerMask.NameToLayer("PlayerHand"));

            if (hitArr.Length > 0)
            {
                ctrl = hitArr[0].collider.gameObject.GetComponent<UIItemPoker_ShiSanZhang>();
                Debug.Log(" hitArr[0]" + ctrl.ToString() + ctrl.transform.position);
                for (int i = 1; i < hitArr.Length; ++i)
                {
                    UIItemPoker_ShiSanZhang ctrlTemp = hitArr[i].collider.gameObject.GetComponent<UIItemPoker_ShiSanZhang>();
                    Debug.Log("ctrlTemp" + ctrlTemp.ToString() + ctrlTemp.transform.position);
                    if (ctrl.Poker.Size > ctrlTemp.Poker.Size || (ctrl.Poker.Size == ctrlTemp.Poker.Size && ctrl.Poker.Color > ctrlTemp.Poker.Color))
                    {
                        ctrl = ctrlTemp;
                    }
                }
                Debug.Log("ctrl" + ctrl.ToString());
            }
            return ctrl;

        }

        private void ResetDragPoker()
        {
            m_StartSelectedPoker = null;//初始选中的牌
            m_EndSelectedPoker = null; //结束选中的牌
            m_IsSelectedPoker = false;
            m_CurrSelectedPoker.Clear(); //当前拖拽选中的牌
            m_CurrLiftUpPoker.Clear(); //当前抬起的牌

        }
    }
}
