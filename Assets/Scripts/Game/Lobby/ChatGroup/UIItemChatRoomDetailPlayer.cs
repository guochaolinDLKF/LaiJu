//===================================================
//Author      : DRB
//CreateTime  ：9/27/2017 2:40:48 PM
//Description ：群友会房间详情界面玩家UI项
//===================================================
using UnityEngine;
using UnityEngine.UI;

public class UIItemChatRoomDetailPlayer : UIItemBase 
{
    [SerializeField]
    private RawImage m_ImgHead;
    [SerializeField]
    private Text m_TxtNickname;
    [SerializeField]
    private Text m_TxtPlayerId;
    [SerializeField]
    private Image m_ImgOwner;
    
    public void SetUI(int playerId, string nickName, string avatar, bool isOwner)
    {
        m_TxtPlayerId.SafeSetText(playerId.ToString());
        m_TxtNickname.SafeSetText(nickName);
        TextureManager.Instance.LoadHead(avatar, (Texture2D tex) => 
        {
            if (m_ImgHead != null)
            {
                m_ImgHead.texture = tex;
            }
        });
        m_ImgOwner.gameObject.SetActive(isOwner);
    }
}
