//===================================================
//Author      : DRB
//CreateTime  ：4/21/2017 8:16:54 PM
//Description ：
//===================================================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIInformView : MonoBehaviour 
{
    [SerializeField]
    private Text m_TextInform;


    private Queue<string> m_QueueMessage = new Queue<string>();


    private const float SHOW_SPACE = 19.0f;

    private bool m_isIdle = true;

    private float m_fTimer;

    private Tweener m_Tweener;

    private void Awake()
    {
        m_TextInform.transform.localPosition = new Vector3(1200f, 0f, 0f);
        m_Tweener = m_TextInform.transform.DOLocalMoveX(-2000f, SHOW_SPACE).SetEase(Ease.Linear).SetAutoKill(false).Pause();
    }


    public void Show(string msg)
    {
        gameObject.SetActive(true);
        m_QueueMessage.Enqueue(msg);
    }


    private void Update()
    {
        if (m_isIdle)
        {
            if (m_QueueMessage.Count > 0)
            {
                string msg = m_QueueMessage.Dequeue();
                m_TextInform.SafeSetText(msg);
                m_Tweener.Restart();
                m_fTimer = Time.time;
                m_isIdle = false;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (Time.time - m_fTimer > SHOW_SPACE)
            {
                m_isIdle = true;
            }
        }
    }
}
