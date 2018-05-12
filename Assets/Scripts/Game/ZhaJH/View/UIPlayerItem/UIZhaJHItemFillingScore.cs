//===================================================
//Author      : DRB
//CreateTime  ：7/1/2017 3:48:03 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZhaJh;

public class UIZhaJHItemFillingScore : UIViewBase
{    
    public Image m_liangHui;//二
    public Image m_wuHui;//五
    public Image m_shiHui;//十  
    public Image m_wuShiHui;//五十
    [SerializeField]
    private GameObject hidMaskImag;//关闭遮罩和筹码

    protected override void OnAwake()
    {
        base.OnAwake();
        HidFenG(null);
        this.gameObject.SetActive(false);
    }

    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();        
        dic.Add(ZhaJHMethodname.OnZJHHidFen, HidFen);
        dic.Add(ZhaJHMethodname.OnZJHHidFenG, HidFenG);//开始下一局的时候加注筹码按钮恢复正常
        dic.Add(ZhaJHMethodname.OnZJHHidImage, HidImage);        
        return dic;
    }  
 
    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "liangfen":               
                SendNotification(ZhaJHMethodname.OnZJHliangfen, 2);
                HidImage(null);
                break;
            case "wufen":                 
                SendNotification(ZhaJHMethodname.OnZJHliangfen, 5);
                HidImage(null);
                break;
            case "shifen":               
                SendNotification(ZhaJHMethodname.OnZJHliangfen, 10);
                HidImage(null);
                break;
            case "twoshifen":
                SendNotification(ZhaJHMethodname.OnZJHliangfen, 20);
                HidImage(null);
                break;
            case "wushi":
                SendNotification(ZhaJHMethodname.OnZJHliangfen, 50);
                HidImage(null);
                break;
            case "onebaifen":
                SendNotification(ZhaJHMethodname.OnZJHliangfen, 100);
                HidImage(null);
                break;                
        }
    }

    public void HidFenG(TransferData data)
    {
        if (m_liangHui != null) m_liangHui.gameObject.SetActive(false);
        if (m_wuHui != null) m_wuHui.gameObject.SetActive(false);
        if (m_shiHui != null) m_shiHui.gameObject.SetActive(false);
        if (m_wuShiHui != null) m_wuShiHui.gameObject.SetActive(false);
    }


    //把加注分数按钮变成灰色的
    public void HidFen(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        if (seat==RoomZhaJHProxy.Instance.PlayerSeat)
        {
            float fen = data.GetValue<float>("Fen");

            if (fen == 0)
            {
                if (m_liangHui != null) m_liangHui.gameObject.SetActive(false);
                if (m_wuHui != null) m_wuHui.gameObject.SetActive(false);
                if (m_shiHui != null) m_shiHui.gameObject.SetActive(false);
                if (m_wuShiHui != null) m_wuShiHui.gameObject.SetActive(false);
            }
            for (int i = 0; i <= fen; i++)
            {
                switch (i)
                {
                    case 2:
                        if (m_liangHui != null) m_liangHui.gameObject.SetActive(true);
                        break;
                    case 5:
                        if (m_wuHui != null) m_wuHui.gameObject.SetActive(true);
                        break;
                    case 10:
                        if (m_shiHui != null) m_shiHui.gameObject.SetActive(true);
                        break;
                    case 50:
                        if (m_wuShiHui != null) m_wuShiHui.gameObject.SetActive(true);
                        break;
                }
            }
        }      
    }


    private void HidImage(TransferData data)
    {
        if (hidMaskImag!=null)
        {
            hidMaskImag.GetComponent<BtnJiaZ>().BtnOnClik();
        }
    }




}
