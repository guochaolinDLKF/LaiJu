//===================================================
//Author      : DRB
//CreateTime  ：10/14/2017 2:53:29 PM
//Description ：
//===================================================
using UnityEngine;
using UnityEngine.UI;

public class UIItemChatGroupMessage : UIItemBase 
{
    [SerializeField]
    private RawImage m_ImgHead;
    [SerializeField]
    private Text m_TxtMessage;
    [SerializeField]
    private Transform m_RightContainer;
    [SerializeField]
    private Transform m_LeftContainer;

    private static Texture m_DefaultHead;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_DefaultHead = m_ImgHead.texture;
    }


    public void SetUI(string avatarUrl, string message, bool isRight)
    {
        TextureManager.Instance.LoadHead(avatarUrl,(Texture2D tex)=> 
        {
            if (m_ImgHead != null)
            {
                m_ImgHead.texture = tex != null?tex: m_DefaultHead;
            }
        });

        m_TxtMessage.SafeSetText(message);
        m_TxtMessage.alignment = isRight ? TextAnchor.UpperRight : TextAnchor.UpperLeft;
        m_ImgHead.gameObject.SetParent(isRight? m_RightContainer: m_LeftContainer);
    }
}
