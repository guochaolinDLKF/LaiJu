//===================================================
//Author      : DRB
//CreateTime  ：4/17/2017 10:06:08 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIItemChat : MonoBehaviour
{
    /// <summary>
    /// 互动表情移动时间
    /// </summary>
    private const float INTERACTIVE_EXPRESSION_DURATION = 0.4f;
    /// <summary>
    /// 互动表情移动时的alpha值
    /// </summary>
    private const float INTERACTIVE_EXPRESSION_ALPHA = 0.5f;

    public static UIItemChat Instance;


    [SerializeField]
    private UIItemChatMessage[] m_ChatMessages;

    private void Awake()
    {
        Instance = this;
    }

    #region ShowMessage 显示文字
    /// <summary>
    /// 显示文字
    /// </summary>
    /// <param name="seatIndex"></param>
    /// <param name="message"></param>
    public void ShowMessage(int seatIndex, string message)
    {
        m_ChatMessages[seatIndex].ShowMessage(message);
    }
    #endregion

    #region ShowExpression 显示表情
    /// <summary>
    /// 显示表情
    /// </summary>
    /// <param name="seatIndex"></param>
    /// <param name="imageName"></param>
    public void ShowExpression(int seatIndex,string imageName)
    {
        m_ChatMessages[seatIndex].ShowExpression(imageName);
    }
    #endregion

    #region ShowInteractiveExpression 显示互动表情
    /// <summary>
    /// 显示互动表情
    /// </summary>
    public void ShowInteractiveExpression(int seatIndex,string animation,int toSeatIndex,string audioName = "")
    {
        Debug.Log(audioName);
        AssetBundleManager.Instance.LoadOrDownload(string.Format("download/{0}/prefab/uiprefab/uianimations/{1}.drb",ConstDefine.GAME_NAME,animation), animation,(GameObject go)=> 
        {
            if (go != null)
            {
                go = Instantiate(go);
                go.SetParent(this.transform);
                go.GetComponent<UIAnimation>().enabled = false;
                go.transform.position = m_ChatMessages[seatIndex].InteractivePoint;
                Image img = go.GetComponent<Image>();
                img.color = new Color(img.color.r, img.color.g, img.color.b, INTERACTIVE_EXPRESSION_ALPHA);
                go.transform.DOMove(m_ChatMessages[toSeatIndex].InteractivePoint, INTERACTIVE_EXPRESSION_DURATION).SetEase(Ease.Linear).OnComplete(()=> 
                {
                    if (!string.IsNullOrEmpty(audioName))
                    {
                        AudioEffectManager.Instance.Play(string.Format("{0}", audioName), Vector3.zero, false);
                    }
                    go.GetComponent<UIAnimation>().enabled = true;
                    img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
                });
            }
        });
    }
    #endregion

    #region ShowMicroPhone 显示语音
    /// <summary>
    /// 显示语音
    /// </summary>
    /// <param name="seatIndex"></param>
    public void ShowMicroPhone(int seatIndex)
    {
        m_ChatMessages[seatIndex].ShowMicroPhone();
    }
    #endregion
}
