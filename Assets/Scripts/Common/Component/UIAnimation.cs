//===================================================
//Author      : DRB
//CreateTime  ：4/24/2017 10:02:52 AM
//Description ：
//===================================================
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour 
{
    [SerializeField]
    private Sprite[] m_SpriteArr;
    [SerializeField]
    private bool m_isLoop = false;
    [SerializeField]
    private float m_Duration = 0.0f;
    [SerializeField]
    private float m_FrameSpace = 0.02f;
    [SerializeField]
    private float m_StayTime = 0.3f;

    private float m_Timer;

    private float m_LoopTimer;

    private Image m_Image;

    private int m_Index;

    private bool m_isStay;

    private int m_CompleteTimes = 0;

    private void Awake()
    {
        m_Image = GetComponent<Image>();
        if (m_SpriteArr != null && m_SpriteArr.Length > 0)
        {
            m_Image.overrideSprite = m_SpriteArr[m_Index];
        }
        m_Timer = Time.time;
        m_LoopTimer = Time.time;
    }



    private void Update()
    {
        if (m_isStay)
        {
            if (Time.time - m_Timer > m_StayTime)
            {
                Destroy(gameObject);
            }
            return;
        }
        if (m_isLoop && m_Duration > 0f)
        {
            if (Time.time - m_LoopTimer > m_Duration)
            {
                Destroy(gameObject);
                return;
            }
        }
        if (Time.time - m_Timer > m_FrameSpace)
        {
            m_Timer = Time.time;
            ++m_Index;
            if (m_Index > m_SpriteArr.Length - 1)
            {
                if (m_isLoop)
                {
                    m_Index = 0;
                }
                else
                {
                    ++m_CompleteTimes;
                    if (m_CompleteTimes - m_CompleteTimes > 0)
                    {
                        m_Index = 0;
                    }
                    else
                    {
                        if (m_StayTime > 0)
                        {
                            m_isStay = true;
                        }
                        else
                        {
                            Destroy(gameObject);
                        }
                        return;
                    }
                }
            }
            m_Image.overrideSprite = m_SpriteArr[m_Index];
        }
    }

}
