//===================================================
//Author      : WZQ
//CreateTime  ：12/2/2017 9:47:43 AM
//Description ：跑得快倒计时
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace PaoDeKuai
{
    public class UIItemPDKTimeTip : MonoBehaviour
    {

        [SerializeField]
        private Text m_TextCountDown;//倒计时文本

        [SerializeField]
        private Image m_ImageOncePlace;//个位
        [SerializeField]
        private Image m_ImageTensPlace;//十位
        [SerializeField]
        private Sprite[] m_ArrTimeSprite;//图片形式倒计时资源

        [SerializeField]
        private Transform[] m_CountDownContainer;//倒计时挂载点

        [SerializeField]
        private bool m_IsCloseHide = false;//计时结束是否隐藏

        private float m_CurrentCountDown;//倒计时时间

        private bool m_isCountDown;//是否继续倒计时

        private bool m_isPlayedFirstAuido;//第一次播放声音

        private bool m_isPlayedSecondAudio;//重复播放

        private bool m_isPlayer;//是否是自己


        private void Awake()
        {
            Debug.Log("------注册倒计时事件----------");
            Debug.Log("m_CountDownContainer != null" + m_CountDownContainer != null);
           Debug.Log("m_CountDownContainer.Length" + m_CountDownContainer.Length);
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
            int seatIndex = data.GetValue<int>("SeatIndex");
            bool isClose = data.GetValue<bool>("IsClose");


            if (isClose)
            {
                gameObject.SetActive(false);
                return;
            }
          
            if (m_CountDownContainer != null)
            {
                if (m_CountDownContainer.Length > seatIndex) gameObject.SetParent(m_CountDownContainer[seatIndex], true);
            }

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

                        if (m_isPlayer)
                        {
                            AudioEffectManager.Instance.Play("daojishi", Vector3.zero, false);
                        }

                    }
                    if (m_CurrentCountDown < 3f)
                    {
                        if (!m_isPlayedSecondAudio)
                        {
                            m_isPlayedSecondAudio = true;

                            if (m_isPlayer)
                            {
                                AudioEffectManager.Instance.Play("daojishi", Vector3.zero, false);
                            }

                        }
                    }
                }
                if (m_CurrentCountDown <= 0)
                {
                    m_CurrentCountDown = 0;
                    //gameObject.SetActive(false);
                    m_isCountDown = false;
#if (UNITY_IPHONE || UNITY_ANDROID)
                    Handheld.Vibrate();
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
                if (m_IsCloseHide)
                {
                    gameObject.SetActive(false);
                    return;
                }
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
                m_TextCountDown.SafeSetText(second.ToString("00"));
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