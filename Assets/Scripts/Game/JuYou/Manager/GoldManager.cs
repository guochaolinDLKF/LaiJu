//===================================================
//Author      : WZQ
//CreateTime  ：8/25/2017 2:39:06 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace JuYou
{

    public class GoldManager : MonoBehaviour
    {
        [SerializeField]
        private Transform[] m_seatHeadPos;//座位头像点

        [SerializeField]
          private Transform m_GoldContainer;//注池挂载点点
        [SerializeField]
        private int MaxKeepSum = 10;
       
        private List<GameObject> goldAniList = new List<GameObject>();

        private void  RemoveGoldAniList(int index)
        {
            if ((goldAniList.Count - 1) >= index)
            {
                GameObject go = goldAniList[index].gameObject;
                goldAniList.RemoveAt(index);
                Destroy(go);
            }

        }
     

        /// <summary>
        /// 下底注
        /// </summary>
        public void PutBaseScore(RoomEntity room)
        {
            List<SeatEntity> seatList = room.SeatList;
            for (int i = 0; i < seatList.Count; i++)
            {
                if (seatList[i].PlayerId > 0)
                {
                    SeatEntity seat = seatList[i];
                    PlayUIAnimation(ConstDefine_JuYou.UIAniGoldMove_JuYou, m_seatHeadPos[seatList[i].Index].position, m_GoldContainer.position,10,true
                        , () => {
                            UIDispatcher.Instance.Dispatch(ConstDefine_JuYou.ObKey_SendRoomGoldChanged);
                            UIDispatcher.Instance.Dispatch(ConstDefine_JuYou.ObKey_SendSeatGoldChanged, new object[] { seat });

                        }
                        
                        );
                }

            }

        }


        public void BaoZhaGuo(RoomEntity room)
        {
            AudioEffectManager.Instance.Play(ConstDefine_JuYou.AudioZhaGuo, Vector3.zero, false);

            if (room.isBaoGuo)
            {

                //爆锅


                List<SeatEntity> seatList = room.SeatList;
                for (int i = 0; i < seatList.Count; i++)
                {
                    if (seatList[i].PlayerId > 0 && seatList[i].isJoinGame)
                    {
                        SeatEntity seat = seatList[i];
                        UIDispatcher.Instance.Dispatch(ConstDefine_JuYou.ObKey_SendRoomGoldChanged);
                        PlayUIAnimation(ConstDefine_JuYou.UIAniGoldMove_JuYou, m_GoldContainer.position, m_seatHeadPos[seatList[i].Index].position,seatList[i].Earnings, true,
                            () => {
                               
                              UIDispatcher.Instance.Dispatch(ConstDefine_JuYou.ObKey_SendSeatGoldChanged, new object[] { seat });
                          }
                            );
                    }

                }


            }

        }

        //普通金币移动
        public void AloneSettleGoldMove(SeatEntity seat)
        {
            Vector3 startPos = seat.Earnings >= 0 ? m_GoldContainer.position : m_seatHeadPos[seat.Index].position;
            Vector3 endPos = seat.Earnings >= 0 ? m_seatHeadPos[seat.Index].position : m_GoldContainer.position;

         


            PlayUIAnimation(ConstDefine_JuYou.UIAniGoldMove_JuYou, startPos, endPos,seat.Earnings, true/*seat.Earnings >= 0*/,
                () => {
                    UIDispatcher.Instance.Dispatch(ConstDefine_JuYou.ObKey_SendRoomGoldChanged);
                    UIDispatcher.Instance.Dispatch(ConstDefine_JuYou.ObKey_SendSeatGoldChanged,new object[] { seat});

                }
                
                );
        }



        #region PlayUIAnimation 播放金币移动动画
        /// <summary>
        /// 播放UI动画
        /// </summary>
        /// <param name="type"></param>
        private void PlayUIAnimation(string type, Vector3 startPos, Vector3 endPos, int goldValue, bool autoDestruction, Action OnComplete = null)//UIAnimationType
        {
            string path = string.Format("download/{0}/prefab/uiprefab/uianimations/{1}.drb", ConstDefine.GAME_NAME, type.ToLower());
            AssetBundleManager.Instance.LoadOrDownload(path, type.ToString().ToLower(), (GameObject go) =>
            {
                if (go != null)
                {
                    go = Instantiate(go);
                    go.SetParent(m_GoldContainer);
                    go.transform.position = startPos;
                    //if (!autoDestruction) goldAniList.Add(go);
                    GoldMoveAni ani = go.GetComponent<GoldMoveAni>();
                    if (ani != null) ani.PlayAni(startPos, endPos, goldValue, autoDestruction, OnComplete);
                }
            });
        }
        #endregion
    }
}