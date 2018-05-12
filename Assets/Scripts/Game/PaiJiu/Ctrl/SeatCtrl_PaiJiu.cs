//===================================================
//Author      : WZQ
//CreateTime  ：7/5/2017 5:28:06 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using proto.paigow;

namespace PaiJiu
{
    public class SeatCtrl_PaiJiu : MonoBehaviour
    {
        [SerializeField]
        private Grid3D m_WallContainer;//墙挂载点
        [SerializeField]
        private Grid3D m_HandContainer;//手牌挂载点
        [SerializeField]
        private Grid3D m_ShowHandContainer;//展示手牌挂载点（玩家自己展示牌的挂载点）   未使用


        [SerializeField]
        private Grid3D m_DeskTopContainer;//打出的牌挂载点
        //[SerializeField]
        //private Transform m_DrawContainer;//摸得牌挂载点

        [SerializeField]
        private Transform m_WallModel;//墙模型（出麻将的横板）
        [SerializeField]
        private int m_nSeatPos;//座位方位  东南西北

        private SeatCtrl_PaiJiu m_CurrentPoker;


        private Tweener m_WallTweener;//横板下移
        private Tweener m_WallTweenerBackwards;//横板上移
        private Tweener m_WallContainerTweener;//挂载点前移
        private static SeatCtrl_PaiJiu s_CurrentMaJiang;

        private Vector3 wallModelInitPos;
        public int SeatPos
        {
            get { return m_nSeatPos; }
        }
        private void Awake()
        {
            wallModelInitPos = m_WallContainer.transform.position; ;
            m_WallTweener = m_WallModel.DOMove(m_WallModel.transform.position + new Vector3(0, -20, 0), 0.7f).SetEase(Ease.Linear).SetAutoKill(false).Pause();
            m_WallTweenerBackwards = m_WallModel.DOMove(m_WallModel.transform.position , 0.7f).SetEase(Ease.Linear).SetAutoKill(false).Pause();
            m_WallContainerTweener = m_WallContainer.transform.DOMove(m_WallContainer.transform.position + m_WallContainer.transform.forward * 20, 1f).SetEase(Ease.Linear).SetAutoKill(false).Pause();

            //ModelDispatcher.Instance.AddEventListener("OnSeatDrawPoker", OnSeatDrawPoker);摸牌
            //ModelDispatcher.Instance.AddEventListener("OnSeatPlayPoker", OnSeatPlayPoker);出牌
            //ModelDispatcher.Instance.AddEventListener("OnSeatOperate", OnSeatOperate);操作
        }


        public void Init(int seatCount, ROOM_STATUS status,int PlayerSeatPos)
        {

            //m_WallContainer.constraintCount = 4;
            if (seatCount == 2)
            {
                if (m_nSeatPos == 2)
                {
                    m_nSeatPos = 3;
                }
                else if (m_nSeatPos == 3)
                {
                    m_nSeatPos = 2;
                }
                //m_DeskTopContainer.constraintCount = 12;
                //m_DeskTopContainer.transform.localPosition = new Vector3(-56f, 5f, -52f);
            }

          
            ////设置手牌Scale
            //if (m_nSeatPos==PlayerSeatPos)
            //{
            //    m_HandContainer.transform.localScale = Vector3.one * 2;
            //}

            //if ( ((PlayerSeatPos + 1) % seatCount)  ==(m_nSeatPos % 4))
            //{
            //    m_HandContainer.transform.localScale = Vector3.one * 1.6f;
            //}
            //if (  ((PlayerSeatPos + 2) % seatCount) == (m_nSeatPos % 4))
            //{
            //    m_HandContainer.transform.localScale = Vector3.one * 1.5f;
            //}
            //if ( ((PlayerSeatPos + 3) % seatCount) == (m_nSeatPos % 4))
            //{
            //    m_HandContainer.transform.localScale = Vector3.one * 1.6f;
            //}
          
            ////重播（暂未使用）
            //if (status == RoomEntity.RoomStatus.Replay)
            //{
            //    m_HandContainer.transform.localEulerAngles = new Vector3(0, -90, 0);
            //    m_DrawContainer.transform.localEulerAngles = new Vector3(0, -90, 0);
            //}
        }


      
            



       
        /// <summary>
        /// 初始化墙
        /// </summary>
        /// <param name="wall"></param>
        public void InitWall(List<MaJiangCtrl_PaiJiu> wall, bool isPlayAnimation, System.Action OnComplete = null)
        {
            //还原牌墙挂载点位置
            m_WallContainer.transform.position = wallModelInitPos ;

            //for (int i = 0; i < m_PengCtrls.Length; ++i)
            //{
            //    m_PengCtrls[i].Reset();
            //}

            for (int i = 0; i < wall.Count; ++i)
            {
               
                wall[i].gameObject.SetParent(m_WallContainer.transform);
            }
            m_WallContainer.Sort();
            if (isPlayAnimation)
            {
                
               StartCoroutine(InitWallCoroutine(wall, OnComplete));
            }
            else
            {
                m_WallContainer.transform.position = wallModelInitPos + m_WallContainer.transform.forward * 25;
            }
        }

        /// <summary>
        /// 初始化墙动画
        /// </summary>
        /// <param name="wall"></param>
        /// <returns></returns>
        private IEnumerator InitWallCoroutine(List<MaJiangCtrl_PaiJiu> wall,System.Action OnComplete = null)
        {

            for (int i = 0; i < wall.Count; ++i)
            {
                wall[i].gameObject.SetActive(false);
            }

            yield return null;
            
            m_WallTweener.OnComplete(() =>
            {
                
                for (int i = 0; i < wall.Count; ++i)
                {
                    wall[i].gameObject.SetActive(true);
                }

               
                //横板上移后 牌墙挂载点前移
                m_WallTweenerBackwards.OnComplete(() =>
                {
                    
                    m_WallContainerTweener.OnComplete(
                        () => {
                            if (OnComplete != null) OnComplete();
                        }
                        ).Restart();

                }).Restart();

            }).Restart();
        }

        //设置翻开牌墙
        public void DrawMaJiangWall(List<MaJiangCtrl_PaiJiu> wall, bool isPlayAnimation)
        {

            if (isPlayAnimation)
            {

                for (int i = 0; i < wall.Count; i++)
                {
                    //wall[i].SetPokerStatus(true);

                    if ((wall[i].transform.GetSiblingIndex() % 2) == 0)
                    {
                                int k = i;
                        wall[i].transform.DOLocalMoveZ(-10f, 0.4f).OnComplete(
                            () =>
                            {
                                if ((k + 2) > (wall.Count - 1))
                                {

                                    wall[k].transform.DOLocalMoveY(4f, 0.2f).OnComplete(
                                    () =>
                                    {

                                        for (int j = 0; j < wall.Count; j++)
                                        {
                                            wall[j].SetPokerStatus(true);

                                        }
                                    }


                                    );

                                }
                                else
                                {
                                    wall[k].transform.DOLocalMoveY(4f, 0.2f);
                                }

                            }
                            );
                        //wall[i].transform.localPosition += new Vector3(0, 4, -10);
                    }

                }

            }
            else
            {

                for (int i = 0; i < wall.Count; i++)
                {
                    wall[i].SetPokerStatus(false);

                    if ((wall[i].transform.GetSiblingIndex() % 2) == 0)
                    {
                        wall[i].transform.localPosition += new Vector3(0, 4, -10);
                    }
                   
                }

            }

        }

       


        /// <summary>
        /// 座位摸牌 摸牌到场景手牌上
        /// </summary>
        /// <param name="majiang">摸的牌</param>
        public void DrawPoker(MaJiangCtrl_PaiJiu majiang)
        {
            
           //设置
           majiang.gameObject.SetParent(m_HandContainer.transform);
           majiang.SetPokerStatus(false);
            m_HandContainer.Sort();
          
        }

        /// <summary>
        ///  设置Poker状态 播放动画
        /// </summary>
        /// <param name="seat"></param>
        public void SetPokerStatus(Seat  seat)
        {
            
            //获得MaJiangCtrl_PaiJiu  list
            List<MaJiangCtrl_PaiJiu> handPokerList = MahJongManager_PaiJiu.Instance.GetHand(m_nSeatPos);
            //设置状态
            if (handPokerList == null) return;
            for (int i = 0; i < handPokerList.Count; i++)
            {
                if (handPokerList[i].Poker.type == 0 || handPokerList[i].Poker.size == 0) continue;
                //加载预制  设置数据
                //判断名字是否是相应数据
                string prabfName = handPokerList[i].gameObject.name.Split('(')[0];
                if (prabfName != handPokerList[i].Poker.ToString())
                {
                    MahJongManager_PaiJiu.Instance.SetHand(m_nSeatPos, seat.PokerList[i]);

                }

                AppDebug.Log(string.Format("座位{0} Index{1} 变更牌状态", seat.Pos, handPokerList[i].Poker.index));
                handPokerList[i].SetPokerStatus(true);



            }
        }

        /// <summary>
        /// 清空手牌
        /// </summary>
        public void ClearHandPoker()
        {
            //找到手牌
            List<MaJiangCtrl_PaiJiu> handPokerList = MahJongManager_PaiJiu.Instance.GetHand(m_nSeatPos);
            if (handPokerList == null) return;
            int handPokerListCount = handPokerList.Count;

            for (int i = 0; i < handPokerListCount; i++)
            {

                AppDebug.Log(string.Format("座位{0}弃牌{1}", m_nSeatPos, handPokerList[0].Poker.ToChinese()));

                MaJiangCtrl_PaiJiu majiang =  MahJongManager_PaiJiu.Instance.ClearHandPoker(m_nSeatPos, handPokerList[0].Poker);
                
                majiang.gameObject.SetParent(m_DeskTopContainer.transform);
                majiang.gameObject.layer = m_DeskTopContainer.gameObject.layer;
                majiang.transform.localPosition = m_DeskTopContainer.GetLocalPos(majiang.transform);

                
            }

        }

        //清空打过的牌
        public void ClearDeskTopContainer()
        {
            List<MaJiangCtrl_PaiJiu> dicTablePokerList = MahJongManager_PaiJiu.Instance.GetDicTable(m_nSeatPos);
            if (dicTablePokerList == null) return;
            for (int i = dicTablePokerList.Count - 1; i >= 0 ; i--)
            {
                Destroy(dicTablePokerList[i].gameObject);
                dicTablePokerList.Remove(dicTablePokerList[i]);

            }

        }


        /// <summary>
        /// 切牌动画
        /// </summary>
        /// <param name="wallList">牌墙list</param>
        /// <param name="dun">切第几墩 </param>
        /// <param name="onComplete">回调</param>
        public void CutPokerAni(List<MaJiangCtrl_PaiJiu> wallList, int dun ,System.Action onComplete)
        {

            MaJiangCtrl_PaiJiu pokerOne = wallList[(2 * dun)];
            MaJiangCtrl_PaiJiu pokerTwe = wallList[(2 * dun + 1)];

            //改变顺序  选择距离远的切
            bool tou = dun < (wallList.Count / 4); //是否距离头部近
            Debug.Log(string.Format("是否距离头部近:{0}",tou));
            pokerOne.transform.SetSiblingIndex(tou ? (pokerOne.transform.parent.childCount - 1) : 0);
            pokerTwe.transform.SetSiblingIndex(tou ?( pokerOne.transform.parent.childCount - 1 ):1);

            wallList.Remove(pokerOne);
            wallList.Remove(pokerTwe);
            wallList.Insert(tou ? (wallList.Count ) : 0, pokerOne);
            wallList.Insert(tou ? (wallList.Count ) : 1, pokerTwe);


            //上移
            pokerOne.transform.DOMove(pokerOne.transform.position + new Vector3(0, 15, 0), 0.3f).OnComplete(()=> {
                //移动至头尾
                pokerOne.transform.DOLocalMove(m_WallContainer.GetPos(tou ? (wallList.Count - 2) : 0) + new Vector3(0, -15, 0), 0.7f).OnComplete(()=> {

                    //全部归位
                    for (int i = 0; i < wallList.Count; i++)
                    {
                        if (i == wallList.Count - 1)
                        {

                            wallList[i].transform.DOLocalMove(m_WallContainer.GetPos(i),0.4f).OnComplete(()=> {
                                if (onComplete != null) onComplete();

                            });
                        }
                        else
                        {
                            wallList[i].transform.DOLocalMove(m_WallContainer.GetPos(i), 0.4f);
                        }
                    }

                });

            });

            pokerTwe.transform.DOMove(pokerTwe.transform.position + new Vector3(0, 15, 0), 0.3f).OnComplete(()=>{

                pokerTwe.transform.DOLocalMove(m_WallContainer.GetPos(tou ? (wallList.Count - 1) : 1) + new Vector3(0, -15, 0), 0.7f);
            });

           

        }



    }
}