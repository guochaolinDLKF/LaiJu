//===================================================
//Author      : DRB
//CreateTime  ：4/10/2017 6:58:47 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace DRB.MahJong
{
    public class CompassCtrl : MonoBehaviour
    {
        public static CompassCtrl Instance;

        [SerializeField]
        private Transform m_Table;

        [SerializeField]
        private GameObject[] m_Direction;

        [SerializeField]
        private Material[] Mats;
    

        private Tweener m_CurrentTween;


        private void Awake()
        {
            Instance = this;
            ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_CURRENT_OPERATOR_CHANGED, OnCurrentOperatorChanged);
            ModelDispatcher.Instance.AddEventListener(RoomMaJiangProxy.ON_BEGIN,OnBegin);
            ModelDispatcher.Instance.AddEventListener(RoomPaiJiuProxy.ON_BEGIN, OnBegin);
        }

        private void Start()
        {
            SetDirection(RoomMaJiangProxy.Instance.PlayerSeat.Pos, RoomMaJiangProxy.Instance.CurrentRoom.SeatList.Count, RoomMaJiangProxy.Instance.PlayerSeat.Direction);
        }

        private void OnDestroy()
        {
            ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_CURRENT_OPERATOR_CHANGED, OnCurrentOperatorChanged);
            ModelDispatcher.Instance.RemoveEventListener(RoomMaJiangProxy.ON_BEGIN, OnBegin);
            ModelDispatcher.Instance.RemoveEventListener(RoomPaiJiuProxy.ON_BEGIN, OnBegin);

            if (Mats != null)
            {
                for (int i = 0; i < Mats.Length; ++i)
                {
                    Mats[i].color = new Color(1, 1, 1, 0);
                }
            }
        }

        private void OnBegin(TransferData data)
        {
            int seatPos = data.GetValue<int>("SeatPos");
            int seatCount = data.GetValue<int>("SeatCount");
            int seatDirection = data.GetValue<int>("SeatDirection");
            SetDirection(seatPos, seatCount, seatDirection);
        }

        private void OnCurrentOperatorChanged(TransferData data)
        {
            int seatPos = data.GetValue<int>("SeatPos");
            int seatCount = data.GetValue<int>("SeatCount");
            int seatDirection = data.GetValue<int>("SeatDirection");
            SetCurrent(seatPos, seatCount, seatDirection);
        }

        public void SetDirection(int seatPos,int seatCount,int seatDirection)
        {
            if (seatDirection != 0)
            {
                int distance = seatDirection - seatPos;
                transform.eulerAngles = new Vector3(0, 180 + distance * 90f, 0);
            }
        }

        public void SetCurrent(int seatPos,int seatCount,int seatDirection)
        {
            SetNormal();
            int index = 0;
            index = seatPos - 1;
            if (seatDirection != 0)
            {
                int distance = seatDirection - seatPos;
                index = seatDirection - 1;
                transform.eulerAngles = new Vector3(0, 180 + distance * 90f, 0);
            }
            m_CurrentTween = Mats[index].DOColor(new Color(1, 1, 1, 1), 1f).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.Linear);
            m_Direction[index].SetActive(true);
        }

        public void SetNormal()
        {
            if (m_CurrentTween != null)
            {
                m_CurrentTween.Kill();
            }
            for (int i = 0; i < Mats.Length; ++i)
            {
                m_Direction[i].gameObject.SetActive(false);
                Mats[i].color = new Color(1, 1, 1, 0);
            }
        }

        
    }
}
