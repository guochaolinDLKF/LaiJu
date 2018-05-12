//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 10:09:28 AM
//Description ：跑得快炸弹动画
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
namespace PaoDeKuai
{
    public class UIItemPDKBombAni : MonoBehaviour
    {
        [SerializeField]
        private DOTweenPath[] m_Paths;

        [SerializeField]
        private Image m_BombImage;

       
        public void SetUI(int seatIndex,System.Action onComplete=null)
        {
            if (seatIndex >= 0 && m_Paths != null && m_Paths.Length > seatIndex)
            {
                m_BombImage.gameObject.SetParent(m_Paths[seatIndex].transform,true);
                m_Paths[seatIndex].gameObject.SetActive(true);
            }
                StartCoroutine(Complete(onComplete));

        }

        IEnumerator Complete(System.Action onComplete = null)
        {
            yield return new WaitForSeconds(2f);
            if (onComplete != null)  onComplete();
            DestroyObject(gameObject);
        }

    }
}