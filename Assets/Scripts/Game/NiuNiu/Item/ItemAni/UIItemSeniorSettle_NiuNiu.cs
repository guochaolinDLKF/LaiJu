//===================================================
//Author      : WZQ
//CreateTime  ：9/12/2017 3:26:52 PM
//Description ：激情场 小结算
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NiuNiu
{
    public class UIItemSeniorSettle_NiuNiu : UIItemBase
    {

        [SerializeField]
        private Text m_fenshu;
        [SerializeField]
        private GameObject[] m_VictoryObj;//胜利物体Victory failure

        [SerializeField]
        private GameObject[] m_FailureObj;//失败物体

        [SerializeField]
        private Color[] VFColors;
        public void SetUI(int score)
        {
            //区别胜利失败
            for (int i = 0; i < m_VictoryObj.Length; i++)
            {
                m_VictoryObj[i].SetActive(score >= 0);
            }
            for (int i = 0; i < m_FailureObj.Length; i++)
            {
                m_FailureObj[i].SetActive(score < 0);
            }

            //播放音效
            AudioEffectManager.Instance.Play(score >= 0 ? ConstDefine_NiuNiu.AuidoVictorySettle_NiuNiu:ConstDefine_NiuNiu.AudioFailureSettle_NiuNiu, Vector3.zero);


            m_fenshu.SafeSetText(string.Format("{0}{1}", score > 0 ? "+" : "", score));

            m_fenshu.color = VFColors[score >= 0 ? 0 : 1];

            Destroy(gameObject, 2f);
        }

    }
}