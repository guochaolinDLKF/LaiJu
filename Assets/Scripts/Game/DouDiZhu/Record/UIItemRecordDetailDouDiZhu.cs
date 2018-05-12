//===================================================
//Author      : DRB
//CreateTime  ：1/12/2018 11:10:46 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemRecordDetailDouDiZhu : UIItemRecordDetailBase
{
    [SerializeField]
    private Text m_txtLoop;
    [SerializeField]
    private Text m_txtDateTime;
    [SerializeField]
    private Text[] m_ArrGold;
    [SerializeField]
    private Text[] m_ArrNickName;
    [SerializeField]
    private RawImage[] m_ArrAvatar;

    public string m_WinColor;
    public string m_FailureColor;
    public override void SetUI(TransferData data)
    {
        base.SetUI(data);

        int loop = data.GetValue<int>("Loop");
        string time = data.GetValue<string>("DateTime");
        List<TransferData> lstPlayer = data.GetValue<List<TransferData>>("Player");
        for (int i = 0; i < m_ArrGold.Length; ++i)
        {
            if (i < lstPlayer.Count)
            {
                m_ArrGold[i].gameObject.SetActive(true);
                TransferData playerData = lstPlayer[i];
                int gold = playerData.GetValue<int>("Gold");
                m_ArrGold[i].SafeSetText(string.Format("<color=#{0}>{1}</color>", gold >= 0 ? m_WinColor : m_FailureColor, gold.ToString(ConstDefine.STRING_FORMAT_SIGN)));
            }
            else
            {
                m_ArrGold[i].gameObject.SetActive(false);
            }
        }

        //m_txtLoop.SafeSetText(string.Format("第{0}局", loop.ToString()));
        m_txtLoop.SafeSetText(string.Format("第{0}局", WordsUtil.ConvertToChinese(loop)));

        m_txtDateTime.SafeSetText(time);

        for (int i = 0; i < m_ArrAvatar.Length; ++i)
        {
            if (i < lstPlayer.Count)
            {
                m_ArrAvatar[i].gameObject.SetActive(true);
                TransferData playerData = lstPlayer[i];
                string avatar = playerData.GetValue<string>("Avatar");
                TextureManager.Instance.LoadHead(avatar, (Texture2D tex) =>
                {
                    if (m_ArrAvatar[i] != null)
                    {
                        m_ArrAvatar[i].texture = tex;
                    }
                });
            }
            else
            {
                m_ArrAvatar[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < m_ArrNickName.Length; ++i)
        {
            if (i < lstPlayer.Count)
            {
                m_ArrNickName[i].gameObject.SetActive(true);
                TransferData playerData = lstPlayer[i];
                m_ArrNickName[i].SafeSetText(playerData.GetValue<string>("NickName"));
            }
            else
            {
                m_ArrNickName[i].gameObject.SetActive(false);
            }
        }

    }

}