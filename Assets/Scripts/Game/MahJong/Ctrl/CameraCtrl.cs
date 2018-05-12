//===================================================
//Author      : DRB
//CreateTime  ：4/11/2017 9:46:58 AM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace DRB.MahJong
{
    public class CameraCtrl : MonoBehaviour
    {
        public static CameraCtrl Instance;

        [SerializeField]
        private Transform[] m_Pos;

        [SerializeField]
        public Transform[] ProbContainers;
        [SerializeField]
        public Transform CenterContainer;
        [SerializeField]
        private Camera HandPokerCamera;
        [SerializeField]
        private Camera HandPokerCamera17;
        [HideInInspector]
        public Camera MainCamera;


        private Camera m_CurrentHandPokerCamera;

        public Camera CurrentHandPokerCamera
        {
            get { return m_CurrentHandPokerCamera; }
        }

        private void Awake()
        {
            Instance = this;
            MainCamera = GetComponent<Camera>();

            MainCamera.aspect = 1920f / 1080f;

            if (HandPokerCamera != null)
            {
                HandPokerCamera.aspect = 1920f / 1080f;
            }
            if (HandPokerCamera17 != null)
            {
                HandPokerCamera17.aspect = 1920 / 1080f;
            }
        }


        public void SetPos(int playerSeatId, int seatCount)
        {
            int index = playerSeatId - 1;
            if (index < 0)
            {
                AppDebug.ThrowError("座位Id是0");
                index = 0;
            }
            if (seatCount == 2)
            {
                if (index == 1)
                {
                    index = 2;
                }
            }
            for (int i = 0; i < m_Pos.Length; ++i)
            {
                m_Pos[i].gameObject.SetActive(index == i);
            }
            gameObject.SetParent(m_Pos[index]);
        }

        public void SetPos(int seatId,Action onComplete)
        {
            int index = seatId - 1;
            if (index < 0)
            {
                AppDebug.ThrowError("座位Id是0");
                index = 0;
            }
            for (int i = 0; i < m_Pos.Length; ++i)
            {
                m_Pos[i].gameObject.SetActive(index == i);
            }
            transform.SetParent(m_Pos[index]);
            transform.DOLocalMove(Vector3.zero, 1f).SetEase(Ease.Linear).OnComplete(()=> 
            {
                if (onComplete != null)
                {
                    onComplete();
                }
            });
            transform.DOLocalRotate(Vector3.zero,1f).SetEase(Ease.Linear);
        }


        public void SetPokerTotal(int pokerTotal)
        {
            pokerTotal = pokerTotal;
            if (HandPokerCamera17 == null)
            {
                m_CurrentHandPokerCamera = HandPokerCamera;
                return;
            }
            HandPokerCamera.gameObject.SetActive(pokerTotal != 17);
            HandPokerCamera17.gameObject.SetActive(pokerTotal == 17);

            m_CurrentHandPokerCamera = pokerTotal == 17 ? HandPokerCamera17 : HandPokerCamera;
        }
    }
}
