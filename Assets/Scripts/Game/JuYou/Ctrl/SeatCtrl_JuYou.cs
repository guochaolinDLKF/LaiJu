//===================================================
//Author      : WZQ
//CreateTime  ：8/9/2017 7:41:05 PM
//Description ：聚友 场景座位控制器
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.jy;
using DG.Tweening;
namespace JuYou
{
    public class SeatCtrl_JuYou : MonoBehaviour
    {
      
        [SerializeField]
        private Grid3D m_HandContainer;//手牌挂载点
   
        [SerializeField]
        private Grid3D m_DeskTopContainer;//打出的牌挂载点
        [SerializeField]
        private Transform m_DrawContainer;//摸得牌挂载点: 第三张牌

        //[SerializeField]
        private Tweener m_winningAni = null;//兜中Ani

        [SerializeField]
        private int m_nSeatIndex;//座位Index  
        public int SeatIndex
        {
            get { return m_nSeatIndex; }
        }

        private float WAIT_SETTLING_TIME = 2f;
        //private SeatCtrl_JuYou m_CurrentPoker;    
        //private static SeatCtrl_JuYou s_CurrentMaJiang;

        #region MonoBehaviour
        //private void Awake()
        //{
           //ConstDefine_JuYou
            //m_WallTweener = m_WallModel.DOMove(m_WallModel.transform.position + new Vector3(0, -20, 0), 0.7f).SetEase(Ease.Linear).SetAutoKill(false).Pause();

            //string handPrefabName = "hand";
            //string handPath = string.Format("download/{0}/prefab/model/{1}.drb", ConstDefine.GAME_NAME, handPrefabName);
            //AssetBundleManager.Instance.LoadOrDownload(handPath, handPrefabName, (GameObject go) =>
            //{
            //    m_PushHand = Instantiate(go).GetComponent<HandCtrl>();
            //    m_PushHand.gameObject.SetParent(transform);
            //    m_PushHand.gameObject.SetActive(false);
            //});

            //string diceHandPrefabName = "dicehand";
            //string diceHandPath = string.Format("download/{0}/prefab/model/{1}.drb", ConstDefine.GAME_NAME, diceHandPrefabName);
            //AssetBundleManager.Instance.LoadOrDownload(diceHandPath, diceHandPrefabName, (GameObject go) =>
            //{
            //    m_DiceHand = Instantiate(go).GetComponent<HandCtrl>();
            //    m_DiceHand.gameObject.SetParent(transform);
            //    m_DiceHand.gameObject.SetActive(false);
            //});

            //ModelDispatcher.Instance.AddEventListener("OnSeatDrawPoker", OnSeatDrawPoker);
            //ModelDispatcher.Instance.AddEventListener("OnSeatPlayPoker", OnSeatPlayPoker);
            //ModelDispatcher.Instance.AddEventListener("OnSeatOperate", OnSeatOperate);
            //ModelDispatcher.Instance.AddEventListener("OnSeatZhiDui", OnSeatZhiDui);
        //}

        //private void OnDestroy()
        //{
            //ModelDispatcher.Instance.RemoveEventListener("OnSeatDrawPoker", OnSeatDrawPoker);
            //ModelDispatcher.Instance.RemoveEventListener("OnSeatPlayPoker", OnSeatPlayPoker);
            //ModelDispatcher.Instance.RemoveEventListener("OnSeatOperate", OnSeatOperate);
            //ModelDispatcher.Instance.RemoveEventListener("OnSeatZhiDui", OnSeatZhiDui);
        //}
        #endregion
        public void Init(int SeatListCount,ROOM_STATUS roomSeatus,int playerSeatPos)
        {
         

        }
        #region DrawPoker 摸牌
        /// <summary>
        /// 摸牌  pokerStart:设置poker状态
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="isPlayAnimation"></param>
        public void DrawPoker(List<MaJiangCtrl_JuYou> hand,bool pokerStart)
        {
            //发牌:长度为2  摸第三张牌:长度为1
            for (int i = 0; i < hand.Count; ++i)
            {
                hand[i].gameObject.SetParent(m_HandContainer.transform.childCount < 2 ? m_HandContainer.transform : m_DrawContainer);
                hand[i].SetPokerStatus(false,  pokerStart );
                //hand[i].SetPokerStatus(false, hand[i].transform.GetSiblingIndex() < 2 ? pokerStart : true);
            }

            if (hand.Count >= 2)
            {

                m_HandContainer.Sort();
                //HandSort();
            }

            //if (hand.Count == 1)
            //{

            //Debug.Log("第三张牌")
            //}
          if(!pokerStart)   StartCoroutine(AotoStartPoker());
           
            //DOTweenAnimation ani = GetComponent<DOTweenAnimation>();
            //ani.DORestart();

        }
        #endregion


     

        #region OnSeatDrawPoker 当座位摸牌时（第三张）
      
        /// <summary>
        /// //自动翻牌
        /// </summary>
        /// <param name="majiangs"></param>
        /// <returns></returns>
        IEnumerator AotoStartPoker()
        {

            List<MaJiangCtrl_JuYou> majiangs = MahJongManager_JuYou.Instance.GetHand(m_nSeatIndex);
            
            List<int> indexList = new List<int>();
            for (int i = 0; i < majiangs.Count; ++i)
            {
                indexList.Add(majiangs[i].Poker.index);
            }
            if (majiangs.Count <= 2)
            {
               yield return new WaitForSeconds(3);
            }


            //所有手牌
            System.Action OnComplete = null;
            if(majiangs.Count == 3) OnComplete = () => { StartCoroutine(DouAni(majiangs)); };
            SetPokerStatus(indexList, OnComplete );

            //if (majiangs.Count == 1)
            //{
            //    yield return new WaitForSeconds(2);
            //    //发送获取结算信息
            //    UIDispatcher.Instance.Dispatch(ConstDefine_JuYou.ObKey_AloneSettle);
            //}

        }


        


        #endregion

        #region
        /// <summary>
        /// 座位弃牌
        /// </summary>
        public void OnSeatDiscardPoker()
        {
          
            //找到手牌
            List<MaJiangCtrl_JuYou> handPokerList = MahJongManager_JuYou.Instance.GetHand(m_nSeatIndex);
            if (handPokerList == null) return;
            int handPokerListCount = handPokerList.Count;

            List<MaJiangCtrl_JuYou> TablePokerList = MahJongManager_JuYou.Instance.GetDicTable(m_nSeatIndex);

            //if (TablePokerList != null && TablePokerList.Count >= 16) Debug.Log("弃掉桌面牌" + m_nSeatIndex+ "TablePokerList.Count"+ TablePokerList.Count);
            if (TablePokerList != null && TablePokerList.Count >= 16)  MahJongManager_JuYou.Instance.ClearDicTable(m_nSeatIndex);

            for (int i = 0; i < handPokerListCount; i++)
            {
                
                AppDebug.Log(string.Format("座位Index{0}弃牌{1}", m_nSeatIndex, handPokerList[0].Poker.ToChinese()));
                MaJiangCtrl_JuYou majiang = MahJongManager_JuYou.Instance.ClearHandPoker(m_nSeatIndex, handPokerList[0].Poker);

                if (majiang != null)
                {
                majiang.gameObject.SetParent(m_DeskTopContainer.transform);
                majiang.gameObject.layer = m_DeskTopContainer.gameObject.layer;
                majiang.transform.localPosition = m_DeskTopContainer.GetLocalPos(majiang.transform);

                }
            }

            //if (TablePokerList != null && TablePokerList.Count > 18) MahJongManager_JuYou.Instance.ClearDicTable(m_nSeatIndex);

        }
        #endregion

        #region 翻牌
        public void SetPokerStatus(List<int> pokerIndexs, System.Action OnComplete = null)
        {
            //Debug.Log("-----------------------------------AotoStartPoker------------SetPokerStatus");
            List<MaJiangCtrl_JuYou> pokers = MahJongManager_JuYou.Instance.GetHand(m_nSeatIndex);
            if (pokers != null)
            {
                for (int i = 0; i < pokerIndexs.Count; i++)
                {
                    for (int j = 0; j < pokers.Count; j++)
                    {
                        if (pokerIndexs[i] == pokers[j].Poker.index)
                        {
                            Debug.Log("---翻开牌动画--Pokerindex" + pokers[j].Poker.index);

                            


                            pokers[j].SetPokerStatus(true,true, (pokerIndexs.Count == 3 && j == 2) ? OnComplete:null);
                            break;
                        }
                    }
                }

            }

        }
        #endregion


        //兜中或兜不中动画
      private IEnumerator  DouAni(List<MaJiangCtrl_JuYou> majiangs)
        {

            if (majiangs.Count != 3) yield break;

             float waitTime =  WAIT_SETTLING_TIME;
            if ((majiangs[0].Poker.ToValue() < majiangs[2].Poker.ToValue()) && majiangs[2].Poker.ToValue() < majiangs[1].Poker.ToValue() || (majiangs[0].Poker.ToValue() > majiangs[2].Poker.ToValue()) && majiangs[2].Poker.ToValue() > majiangs[1].Poker.ToValue())
            {
                ++waitTime;
                if (m_winningAni == null) m_winningAni = m_DrawContainer.DOScale(2, 0.2f).SetLoops(4, LoopType.Yoyo).SetAutoKill(false).Pause();

                AudioEffectManager.Instance.Play(ConstDefine_JuYou.AudioVictory, Vector3.zero);
                //兜中 闪烁
                m_winningAni.Restart();

            }
            else
            {
                AudioEffectManager.Instance.Play(ConstDefine_JuYou.AudioFailure, Vector3.zero);
            }

            //兜不中 等待 get结算
            yield return new WaitForSeconds(waitTime);

            Debug.Log("--------------- //发送获取结算信息----------------------------------");
            //发送获取结算信息
           if(m_nSeatIndex == 0) UIDispatcher.Instance.Dispatch(ConstDefine_JuYou.ObKey_AloneSettle);

        }




    }
}