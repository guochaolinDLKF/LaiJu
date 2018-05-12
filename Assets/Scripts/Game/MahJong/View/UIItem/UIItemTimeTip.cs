//===================================================
//Author      : DRB
//CreateTime  ：4/24/2017 2:12:02 PM
//Description ：
//===================================================
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DRB.MahJong
{
    public class UIItemTimeTip : MonoBehaviour
    {
        [SerializeField]
        private Text m_TextCountDown;
        [SerializeField]
        private Image m_ImageOncePlace;
        [SerializeField]
        private Image m_ImageTensPlace;
        [SerializeField]
        private Sprite[] m_ArrTimeSprite;

        private float m_CurrentCountDown;

        private bool m_isCountDown;

        private bool m_isPlayedFirstAuido;

        private bool m_isPlayedSecondAudio;

        private bool m_isPlayer;


        private void Awake()
        {
            ModelDispatcher.Instance.AddEventListener("OnCountDownUpdate", OnCountDownUpdate);
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            ModelDispatcher.Instance.RemoveEventListener("OnCountDownUpdate", OnCountDownUpdate);
        }

        private void OnCountDownUpdate(TransferData data)
        {
            long serverTime = data.GetValue<long>("ServerTime");
            bool isPlayer = data.GetValue<bool>("IsPlayer");
            if (serverTime == 0)
            {
                SetTime(0, isPlayer);
                return;
            }
            int countTime = (int)(serverTime + GlobalInit.Instance.TimeDistance - TimeUtil.GetTimestampMS());
            int s = Mathf.RoundToInt(countTime / 1000f);
            SetTime(s, isPlayer);
        }

        private void Update()
        {
            if (m_isCountDown)
            {
                m_CurrentCountDown -= Time.deltaTime;
                if (m_CurrentCountDown < 8f)
                {
                    if (!m_isPlayedFirstAuido)
                    {
                        m_isPlayedFirstAuido = true;
#if !IS_LONGGANG
                        if (m_isPlayer)
                        {
                            AudioEffectManager.Instance.Play("daojishi", Vector3.zero, false);
                        }
#endif
                    }
                    if (m_CurrentCountDown < 3f)
                    {
                        if (!m_isPlayedSecondAudio)
                        {
                            m_isPlayedSecondAudio = true;
#if !IS_LONGGANG
                            if (m_isPlayer)
                            {
                                AudioEffectManager.Instance.Play("daojishi", Vector3.zero, false);
                            }
#endif
                        }
                    }
                }
                if (m_CurrentCountDown <= 0)
                {
                    m_CurrentCountDown = 0;
                    //gameObject.SetActive(false);
                    m_isCountDown = false;
#if (UNITY_IPHONE || UNITY_ANDROID) && !IS_LONGGANG
                    GameUtil.Shake();
#endif
                }
                SetTimeCount((int)m_CurrentCountDown);
            }
        }

        public void SetTime(int second, bool isPlayer)
        {
            m_isPlayedSecondAudio = false;
            m_isPlayedFirstAuido = false;
            m_isPlayer = isPlayer;
            if (second <= 0)
            {
                m_isCountDown = false;
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(true);
         
            SetTimeCount(second);

            m_CurrentCountDown = second;
            m_isCountDown = true;
        }

        private void SetTimeCount(int second)
        {
            if (m_TextCountDown != null)
            {
                m_TextCountDown.SafeSetText(second.ToString("0"));
            }

            int tens = second / 10;
            int once = second % 10;
            if (m_ImageOncePlace != null)
            {
                m_ImageOncePlace.overrideSprite = m_ArrTimeSprite[once];
            }

            if (m_ImageTensPlace != null)
            {
                m_ImageTensPlace.overrideSprite = m_ArrTimeSprite[Mathf.Clamp(tens, 0, 9)];
            }
        }
    }
}
