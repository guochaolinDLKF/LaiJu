//===================================================
//Author      : DRB
//CreateTime  ：6/9/2017 12:34:33 PM
//Description ：
//===================================================
using UnityEngine;
using UnityEngine.UI;

public class UIItemRechargeRecord : UIItemBase 
{

    [SerializeField]
    private Text m_TextRechargeTime;
    [SerializeField]
    private Text m_TextNickname;
    [SerializeField]
    private Text m_TextId;
    [SerializeField]
    private Text m_TextRecargeAmount;
    [SerializeField]
    private Text m_TextBeforeAmount;


    public void SetUI(string time,string nickname,int id,int recargeAmount, int beforeAmount)
    {
        m_TextRechargeTime.SafeSetText(time);
        m_TextNickname.SafeSetText(nickname);
        m_TextId.SafeSetText(id.ToString());
        m_TextRecargeAmount.SafeSetText(recargeAmount.ToString());
        m_TextBeforeAmount.SafeSetText(beforeAmount.ToString());
    }
}
