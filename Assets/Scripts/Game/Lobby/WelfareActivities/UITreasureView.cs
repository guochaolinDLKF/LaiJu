//===================================================
//Author      : DRB
//CreateTime  ：12/2/2017 2:34:36 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITreasureView : MonoBehaviour
{
    [SerializeField]
    private List<UIItemBoxInfo> lstUIItemBoxInfo;
    [SerializeField]
    private Text keyText;


    public void SetTreasure(TransferData data)
    {
        int timeNum = data.GetValue<int>("treasureKeyNum");

        GetComponent<UIKeyInfo>().SetKeyCount(timeNum);

        keyText.SafeSetText(timeNum.ToString());

        if (data.GetValue<List<int>>("lstBoxIndex") != null)
        {
            List<int> lstBoxIndex = data.GetValue<List<int>>("lstBoxIndex");

            for (int i = 0; i < lstBoxIndex.Count; i++)
            {
                lstUIItemBoxInfo[i].SetBoxIndex(lstBoxIndex[i]);
            }
        }
    }
    public void PlayerBoxAnimation(int index)
    {

    }
}
