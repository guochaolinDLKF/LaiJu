//===================================================
//Author      : DRB
//CreateTime  ：4/17/2017 9:50:41 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemChatMessage : MonoBehaviour
{
    [SerializeField]
    private Text m_TextMessage;



    [SerializeField]
    private Transform m_ImageExpressionContainer;

    [SerializeField]
    private UIAnimation m_MicroPhone;
    [SerializeField]
    private Transform m_InteractivePoint;


    private float m_Timer;


    private bool m_isShow;


    private const float SHOW_TIME = 5f;

    private GameObject m_CurrentExpression;

    public Vector3 InteractivePoint
    {
        get
        {
            if (m_InteractivePoint != null)
            {
                return m_InteractivePoint.position;
            }
            return m_ImageExpressionContainer.position;
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (m_isShow)
        {
            if (Time.time - m_Timer > SHOW_TIME)
            {
                gameObject.SetActive(false);
                m_isShow = false;
            }
        }
    }

    #region ShowMessage 显示文字消息
    /// <summary>
    /// 显示文字消息
    /// </summary>
    /// <param name="message">文字内容</param>
    public void ShowMessage(string message)
    {
        m_isShow = true;
        m_Timer = Time.time;
        gameObject.SetActive(true);
        m_TextMessage.SafeSetText(message);
        m_TextMessage.gameObject.SetActive(true);
        m_ImageExpressionContainer.gameObject.SetActive(false);
        if (m_MicroPhone != null)
        {
            m_MicroPhone.gameObject.SetActive(false);
        }
    }
    #endregion

    #region ShowExpression 显示表情
    /// <summary>
    /// 显示表情
    /// </summary>
    /// <param name="imageName">表情图片名称</param>
    public void ShowExpression(string imageName)
    {
        m_isShow = true;
        m_Timer = Time.time;
        gameObject.SetActive(true);

        if (m_CurrentExpression != null)
        {
            Destroy(m_CurrentExpression);
        }
        AssetBundleManager.Instance.LoadOrDownload(string.Format("download/{0}/prefab/uiprefab/emoji/{1}.drb", ConstDefine.GAME_NAME, imageName), imageName, (GameObject go) =>
        {
            if (go != null)
            {
                m_CurrentExpression = Instantiate(go);
                m_CurrentExpression.SetParent(m_ImageExpressionContainer);
            }
        });

        m_ImageExpressionContainer.gameObject.SetActive(true);
        m_TextMessage.gameObject.SetActive(false);
        if (m_MicroPhone != null)
        {
            m_MicroPhone.gameObject.SetActive(false);
        }
    }
    #endregion

    #region ShowInteractiveExpression 显示互动表情
    /// <summary> 
    /// 显示互动表情
    /// </summary>
    public void ShowInteractiveExpression()
    {

    }
    #endregion

    #region ShowMicroPhone 显示语音图标
    /// <summary>
    /// 显示语音图标
    /// </summary>
    public void ShowMicroPhone()
    {
        if (m_MicroPhone != null)
        {
            gameObject.SetActive(true);
            m_isShow = true;
            m_Timer = Time.time;
            m_MicroPhone.gameObject.SetActive(true);
            m_TextMessage.gameObject.SetActive(false);
            m_ImageExpressionContainer.gameObject.SetActive(false);
        }
    }
    #endregion
}
