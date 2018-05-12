//===================================================
//Author      : DRB
//CreateTime  ：7/25/2017 8:20:25 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetroactionCtrl : SystemCtrlBase<RetroactionCtrl>, ISystemCtrl
{
    private UIRetroactionView m_UIRetroactionView;



    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.Retroaction:
                OpenRetroactionView();
                break;
        }
    }

    private void OpenRetroactionView()
    {
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Retroaction, (GameObject go) =>
        {
            m_UIRetroactionView = go.GetComponent<UIRetroactionView>();
            //m_UIRetroactionView.SetUI(AccountProxy.Instance.CurrentAccountEntity.codebind);
        });
    }


}
