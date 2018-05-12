//===================================================
//Author      : DRB
//CreateTime  ：5/12/2017 11:25:19 AM
//Description ：
//===================================================
using UnityEngine;
using UnityEngine.UI;

public class UIItemCountTip : MonoBehaviour 
{
    [SerializeField]
    private Text m_TextCount;



    public void SetUI(int count)
    {
        if (count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        m_TextCount.SafeSetText(count.ToString());
    }
}
