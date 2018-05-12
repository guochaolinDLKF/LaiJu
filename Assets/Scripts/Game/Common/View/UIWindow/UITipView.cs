//===================================================
//Author      : DRB
//CreateTime  ：8/31/2017 9:04:54 PM
//Description ：
//===================================================
using UnityEngine;


public class UITipView : MonoBehaviour 
{
    [SerializeField]
    private UIMoveText m_MoveText;


    public void ShowTip(string content)
    {
        m_MoveText.AddText(content);
    }
}
