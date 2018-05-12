//===================================================
//Author      : DRB
//CreateTime  ：12/6/2017 4:54:37 PM
//Description ：
//===================================================
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class UIPDKMoveText : MonoBehaviour
{

    /// <summary>
    /// 队列时间间隔
    /// </summary>
    public float TimeInterval = 0.5f;
    /// <summary>
    /// 目标局部坐标
    /// </summary>
    public Vector3 TargetLocalPosition = Vector3.zero;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float Duration = 1.5f;
    /// <summary>
    /// 字体动画时间
    /// </summary>
    public float TextAnimationDuration = 0.2f;
    /// <summary>
    /// 提示预制体
    /// </summary>
    public GameObject TipPrefab;

    private Queue<TextArgs> m_Queue = new Queue<TextArgs>();

    private Queue<GameObject> m_Cache = new Queue<GameObject>();


    /// <summary>
    /// 字体
    /// </summary>
    public Font[] m_TextFonts;

    private float m_PreviourTime = 0f;


    private void Update()
    {
        if (m_Queue.Count > 0)
        {
            if (Time.time - m_PreviourTime >= TimeInterval)
            {
                m_PreviourTime = Time.time;
                TextArgs args = m_Queue.Dequeue();
                GameObject go = null;
                if (m_Cache.Count > 0)
                {
                    go = m_Cache.Dequeue();
                    go.gameObject.SetActive(true);
                }
                else
                {
                    go = Instantiate(TipPrefab);
                }
                go.SetParent(transform, true);

                Text text = go.GetComponentInChildren<Text>();
                if (text != null)
                {
                    text.text = string.Empty;
                    if (args.Color != default(Color))
                    {
                        text.color = args.Color;
                    }
                    if (args.IsShowTyping)
                    {
                      text.DOText(args.Content, TextAnimationDuration, false, ScrambleMode.None, null);
                    }
                    else
                    {
                        text.text = args.Content;
                    }
                    if (args.FontIndex >= 0 && m_TextFonts!=null && args.FontIndex < m_TextFonts.Length)
                    {
                        text.font = m_TextFonts[args.FontIndex];
                    }

                }
                go.transform.DOLocalMove(TargetLocalPosition, Duration).SetEase(Ease.Linear).OnComplete(() =>
                {
                    go.SetActive(false);
                    m_Cache.Enqueue(go);
                    if (args.OnComplete != null) args.OnComplete();
                });
            }
        }
    }



    public void AddText(string content, Color color = default(Color),int fontIndex=-1,bool isShowTyping = false,System.Action onComplete=null)
    {
        TextArgs args = new TextArgs() { Color = color, Content = content, FontIndex = fontIndex, IsShowTyping = isShowTyping , OnComplete = onComplete };
        m_Queue.Enqueue(args);
    }

    private class TextArgs
    {
        public Color Color;
        public string Content;
        public int FontIndex;
        public bool IsShowTyping;
        public System.Action OnComplete = null;
    }



}
