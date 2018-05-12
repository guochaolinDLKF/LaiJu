//===================================================
//Author      : DRB
//CreateTime  ：10/30/2017 5:20:43 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GuPaiJiu;

public class GuPaiJiuCutPanView : UIBtnGuPaiJiuViewBase
{
    [SerializeField]
    private GameObject m_CutPan;//切锅，不切


    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();            
        dic.Add(ConstantGuPaiJiu.TellCutPan, TellCutPan);//通知切锅
        dic.Add(ConstantGuPaiJiu.CutPanResult, CutPanResult);//切锅结果
               
        return dic;
    }


    protected override void OnAwake()
    {
        base.OnAwake();           
        if (m_CutPan != null) m_CutPan.SetActive(false);       
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {            
            case ConstantGuPaiJiu.btnGuPaiJiuCutPan://切锅
                SendIsCutPan(EnumCutPan.Cut);
                break;
            case ConstantGuPaiJiu.btnGuPaiJiuNoCutPan://不切
                SendIsCutPan(EnumCutPan.NoCut);
                break;
        }
    }

    /// <summary>
    /// 向客户端发送切锅或者不切
    /// </summary>
    /// <param name="enumCutPan"></param>
    private void SendIsCutPan(EnumCutPan enumCutPan)
    {
        GuPaiJiuGameCtrl.Instance.ClientSendCutPan((int)enumCutPan);
    }

    /// <summary>
    /// 通知切锅
    /// </summary>
    /// <param name="data"></param>
    private void TellCutPan(TransferData data)
    {
        bool IsPlayer = data.GetValue<bool>("IsPlayer");
        long cutPanTime = data.GetValue<long>("Time");
        if (IsPlayer)
        {         
            if (m_CutPan != null) m_CutPan.SetActive(true);
        }
        OpenInvertedTime(cutPanTime);
    }

    /// <summary>
    /// 切锅结果
    /// </summary>
    private void CutPanResult(TransferData data)
    {
        m_CutPan.SetActive(false);
        CloseTime(null);
    }
    /// <summary>
    /// 关闭时间的方法
    /// </summary>
    /// <param name="data"></param>
    private void CloseTime(TransferData data)
    {
        TimeObj.SetActive(false);
        isOpenTime = false;
    }
}
