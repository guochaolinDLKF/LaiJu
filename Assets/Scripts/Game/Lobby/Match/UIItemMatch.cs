//===================================================
//Author      : DRB
//CreateTime  ：5/4/2017 2:13:58 PM
//Description ：
//===================================================
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIItemMatch : MonoBehaviour 
{
    [SerializeField]
    private RawImage m_Image;
    [SerializeField]
    private Text m_Title;
    [SerializeField]
    private Text m_Reward;
    [SerializeField]
    private Text m_PlayerCount;
    [SerializeField]
    private Text m_Cost;
    [SerializeField]
    private Text m_Description;

    [SerializeField]
    private Button m_ButtonEnter;

    [SerializeField]
    private Button m_ButtonSeeDetail;


    private MatchHTTPEntity m_Entity;

    private Action<MatchHTTPEntity> m_OnEnterClick;

    private Action<MatchHTTPEntity> m_OnSeeDetailClick;

    private int matchId;

    private void Awake()
    {
        m_ButtonEnter.onClick.AddListener(OnEnterClick);
        m_ButtonSeeDetail.onClick.AddListener(OnSeeDetailClick);
    }

    /// <summary>
    /// 报名按钮点击
    /// </summary>
    private void OnEnterClick()
    {
        AudioEffectManager.Instance.Play("btnclick", Vector3.zero, false);
        if (m_OnEnterClick != null)
        {
            m_OnEnterClick(m_Entity);
        }
    }

    /// <summary>
    /// 查看详情按钮点击
    /// </summary>
    private void OnSeeDetailClick()
    {
        AudioEffectManager.Instance.Play("btnclick", Vector3.zero, false);
        if (m_OnSeeDetailClick != null)
        {
            m_OnSeeDetailClick(m_Entity);
        }
    }


    public void SetUI(MatchHTTPEntity entity,Action<MatchHTTPEntity> onEnterClick,Action<MatchHTTPEntity> onSeeDetailClick)
    {
        m_Entity = entity;
        TextureManager.Instance.LoadHead(entity.cover,OnLoadFinish);
        m_Title.SafeSetText(entity.title);
        m_Reward.SafeSetText(entity.reward[0]);
        m_PlayerCount.SafeSetText(string.Format("报名人数：{0}人",entity.player.ToString()));
        m_Cost.SafeSetText(string.Format("报名费用房卡×{0}",entity.costNums.ToString()));
        m_Description.SafeSetText(entity.desc);
        m_OnEnterClick = onEnterClick;
        m_OnSeeDetailClick = onSeeDetailClick;
    }

    private void OnLoadFinish(Texture2D tex)
    {
        m_Image.texture = tex;
    }
}
