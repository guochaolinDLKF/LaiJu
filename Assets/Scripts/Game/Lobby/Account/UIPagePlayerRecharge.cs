//===================================================
//Author      : DRB
//CreateTime  ：5/19/2017 3:57:18 PM
//Description ：
//===================================================
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPagePlayerRecharge : MonoBehaviour 
{
    [SerializeField]
    private Text m_CardsCount;
    [SerializeField]
    private Text m_TargetId;
    [SerializeField]
    private Text m_TargetNickName;
    [SerializeField]
    private Text m_TargetCardsCount;
    [SerializeField]
    private Button m_BtnAdd;
    [SerializeField]
    private Button m_BtnMinus;
    [SerializeField]
    private InputField m_GiveCount;

    [HideInInspector]
    public int CurrentCardsCount = 0;

    private void Awake()
    {
        m_BtnAdd.onClick.AddListener(OnBtnAddClick);
        m_BtnMinus.onClick.AddListener(OnBtnMinusClick);
        m_GiveCount.onEndEdit.AddListener(OnGiveCountEndEdit);
    }

    private void OnGiveCountEndEdit(string arg0)
    {
        CurrentCardsCount = arg0.ToInt();
        if (CurrentCardsCount < 0)
        {
            CurrentCardsCount = 0;
        }
        m_GiveCount.text = CurrentCardsCount.ToString();
    }

    private void OnBtnAddClick()
    {
        AudioEffectManager.Instance.Play("btnclick", Vector3.zero, false);
        ++CurrentCardsCount;

        m_GiveCount.text = CurrentCardsCount.ToString();
    }

    private void OnBtnMinusClick()
    {
        AudioEffectManager.Instance.Play("btnclick", Vector3.zero, false);
        --CurrentCardsCount;
        if (CurrentCardsCount < 0)
        {
            CurrentCardsCount = 0;
        }
        m_GiveCount.text = CurrentCardsCount.ToString();
    }


    public void SetUI(int playerCardsCount,int targetId,string targetNickname,int targetCardsCount)
    {
        m_CardsCount.SafeSetText(playerCardsCount.ToString());
        m_TargetId.SafeSetText(targetId.ToString());
        m_TargetNickName.SafeSetText(targetNickname);
        m_TargetCardsCount.SafeSetText(targetCardsCount.ToString());
    }

    public void SetUI(int playerCards,int targetCardsCount)
    {
        m_CardsCount.SafeSetText(playerCards.ToString());
        m_TargetCardsCount.SafeSetText(targetCardsCount.ToString());
    }
}
