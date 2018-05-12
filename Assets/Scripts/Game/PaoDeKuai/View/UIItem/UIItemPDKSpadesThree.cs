//===================================================
//Author      : WZQ
//CreateTime  ：11/29/2017 10:56:38 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
namespace PaoDeKuai
{
    public class UIItemPDKSpadesThree : MonoBehaviour
    {
        [SerializeField]
        private Transform[] m_FlyEnd;

        [SerializeField]
        private Image m_Spades3;

  
        private Transform m_selectedEnd;

        private System.Action m_OnComplete;

        public void SetUI(int index, System.Action onComplete)
        {
            m_OnComplete = onComplete;
            if (m_FlyEnd == null || index < 0 || index >= m_FlyEnd.Length)
            {
                Close();
                return;
            }

            m_Spades3.transform.DOScale(1.2f, 1).SetLoops(4, LoopType.Yoyo).OnComplete(
                () => {
                    m_Spades3.DOColor(Color.clear,0.7f);
           
                m_Spades3.transform.DOMove(m_FlyEnd[index].position, 0.7f).OnComplete(
                    () => {
                        if (m_OnComplete != null) m_OnComplete();
                        Close();  
                    }

                    );

                }
                );


        }

        public void Close()
        {
            DestroyObject(gameObject);
        }
    }
}