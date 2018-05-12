//===================================================
//Author      : DRB
//CreateTime  ：10/21/2017 10:59:55 AM
//Description ：
//===================================================
using UnityEngine;
using UnityEngine.UI;

public class UIItemChatGroupRecordPlayer : UIItemBase 
{
    [SerializeField]
    private RawImage m_ImgHead;
    [SerializeField]
    private Text m_TxtGold;
    [SerializeField]
    private Text m_TxtNickname;
    [SerializeField]
    private Text m_TxtPlayerId;


    private static Texture m_DefaultHead;

    protected override void OnStart()
    {
        base.OnStart();
        m_DefaultHead = m_ImgHead.texture;
    }

    public void SetUI(TransferData data)
    {
        string avatar = data.GetValue<string>("Avatar");
        TextureManager.Instance.LoadHead(avatar, (Texture2D tex) => 
        {
            if (m_ImgHead != null)
            {
                m_ImgHead.texture = tex != null ? tex : m_DefaultHead;
            }
        });

        m_TxtGold.SafeSetText(data.GetValue<int>("Gold").ToString());
        m_TxtNickname.SafeSetText(data.GetValue<string>("Nickname"));
        m_TxtPlayerId.SafeSetText(data.GetValue<int>("PlayerId").ToString());
    }

}
