//===================================================
//Author      : DRB
//CreateTime  ：4/15/2017 6:14:02 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChatView : UIWindowViewBase
{
    [SerializeField]
    private Transform m_CommonMessageGrid;

    [SerializeField]
    private Transform m_ExpressionGrid;


    [SerializeField]
    private InputField m_InputMessage;

    [SerializeField]
    private Button m_Button;


    private Action<string> m_OnSendClick;

    private List<UIItemChatCommonMessage> m_CommonMessageCache = new List<UIItemChatCommonMessage>();

    private List<UIItemChatExpression> m_EmojiCache = new List<UIItemChatExpression>();


    protected override void OnAwake()
    {
        base.OnAwake();
        m_Button.onClick.AddListener(OnSendClick);
    }

    private void OnSendClick()
    {
        AudioEffectManager.Instance.Play("btnclick", Vector3.zero, false);
        if (m_OnSendClick != null)
        {
            m_OnSendClick(m_InputMessage.text);
        }
    }

    public void SetUI(List<cfg_commonMessageEntity> lstCommonMessageEntity,List<cfg_chatExpressionEntity> lstExpressionEntity,Action<string> onCommonMessageClick,Action<int> onExpressionClick)
    {
        for (int i = 0; i < m_CommonMessageCache.Count; ++i)
        {
            UIPoolManager.Instance.Despawn(m_CommonMessageCache[i].transform);
        }
        m_CommonMessageCache.Clear();

        for (int i = 0; i < lstCommonMessageEntity.Count; ++i)
        {
            GameObject go = UIPoolManager.Instance.Spawn("UIItemCommonMessage").gameObject;
            go.SetParent(m_CommonMessageGrid);
            UIItemChatCommonMessage uiCommonMessage = go.GetComponent<UIItemChatCommonMessage>();
            uiCommonMessage.SetUI(lstCommonMessageEntity[i].message, onCommonMessageClick);
            m_CommonMessageCache.Add(uiCommonMessage);
        }

        StartCoroutine(CreateEmoji(lstExpressionEntity, onExpressionClick));

        m_OnSendClick = onCommonMessageClick;
    }

    private IEnumerator CreateEmoji(List<cfg_chatExpressionEntity> lstExpressionEntity, Action<int> onExpressionClick)
    {
        if (m_EmojiCache.Count > 0) yield break;

        for (int i = 0; i < lstExpressionEntity.Count; ++i)
        {
            AssetBundleManager.Instance.LoadOrDownload(string.Format("download/{0}/prefab/uiprefab/emoji/{1}.drb", ConstDefine.GAME_NAME, lstExpressionEntity[i].image), lstExpressionEntity[i].image, (GameObject go) =>
            {
                if (go != null)
                {
                    go = Instantiate(go);
                    go.SetParent(m_ExpressionGrid);
                    UIItemChatExpression uiExpression = go.GetOrCreatComponent<UIItemChatExpression>();
                    go.GetComponent<UIAnimation>().enabled = false;
                    uiExpression.SetUI(lstExpressionEntity[i].id, lstExpressionEntity[i].image, onExpressionClick);
                    m_EmojiCache.Add(uiExpression);
                }
            });

            yield return null;
        }
    }
}
