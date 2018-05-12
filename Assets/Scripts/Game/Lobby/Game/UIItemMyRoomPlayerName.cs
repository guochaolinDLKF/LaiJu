//===================================================
//Author      : DRB
//CreateTime  ：7/6/2017 12:21:31 PM
//Description ：
//===================================================
using UnityEngine;
using UnityEngine.UI;

public class UIItemMyRoomPlayerName : UIItemBase 
{
    [SerializeField]
    private Text m_TextName;

    public void SetUI(string name)
    {
        m_TextName.SafeSetText(name);
    }

}
