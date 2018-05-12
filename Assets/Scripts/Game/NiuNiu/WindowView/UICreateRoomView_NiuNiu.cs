//===================================================
//Author      : WZQ
//CreateTime  ：9/15/2017 4:41:41 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace NiuNiu
{
    public class UICreateRoomView_NiuNiu : MonoBehaviour
    {

        [SerializeField]
        private Toggle m_commonRoom;//普通场

        //[SerializeField]
        //private Toggle m_passionRoom;//激情场


        [SerializeField]
        private Toggle m_lunzhuang;//轮庄
        [SerializeField]
        private Toggle m_gudingzhuang;//固定庄
        [SerializeField]
        private Toggle m_qiangzhuang;//抢庄

        void Awake()
        {

            m_commonRoom.onValueChanged.AddListener(OnToggleCommonRoom);

            OnToggleCommonRoom(m_commonRoom.isOn);
        }


      
        private void OnToggleCommonRoom(bool isSelect)
        {

            m_lunzhuang.gameObject.SetActive(m_commonRoom.isOn);
            m_gudingzhuang.gameObject.SetActive(m_commonRoom.isOn);
            m_qiangzhuang.gameObject.SetActive(!m_commonRoom.isOn);

            m_lunzhuang.isOn = m_commonRoom.isOn;
            m_qiangzhuang.isOn = !m_commonRoom.isOn;
        }
        

    }
}