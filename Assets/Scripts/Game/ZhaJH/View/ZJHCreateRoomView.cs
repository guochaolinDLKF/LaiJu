//===================================================
//Author      : DRB
//CreateTime  ：8/21/2017 11:59:05 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZJHCreateRoomView : MonoBehaviour {
    [SerializeField]
    private Toggle gjRoom;    //选择超级房
    [SerializeField]
    private GameObject page1;    //显示普通房创建房间界面
    [SerializeField]
    private GameObject page2;   //显示超级房创建房间界面
    [SerializeField]
    private Toggle homeMianMoney;   //房主支付
    [SerializeField]
    private Text diamondsNumber8;   //显示消耗房卡数量
    [SerializeField]
    private Text diamondsNumber16; //显示消耗房卡数量

    [SerializeField]
    private Button btnAskPlay;//激情玩法问号
    [SerializeField]
    private Image explainPlay;//玩法说明
    [SerializeField]
    private Button btnAskColor;//派彩 问号
    [SerializeField]
    private Image explainColor;//派彩说明   
    [SerializeField]
    private Button btnAskBloon;//血拼问号
    [SerializeField]
    private Image explainBloon;//血拼说明
    [SerializeField]
    private Button btnMask;//遮罩





    void Awake()
    {
        if (gjRoom!=null) gjRoom.onValueChanged.AddListener(OnToggleSwitch);
        if(homeMianMoney!=null) homeMianMoney.onValueChanged.AddListener(OnToggleHome);
        if(btnAskPlay!=null) btnAskPlay.onClick.AddListener(OnBtnAskPlayClik);
        if(btnAskColor!=null) btnAskColor.onClick.AddListener(OnBtnAskColorClik);
        if(btnAskBloon!=null) btnAskBloon.onClick.AddListener(OnBtnAskBloonClik);
        if(btnMask!=null) btnMask.onClick.AddListener(OnAskMaskClik);
        OnToggleHome(false);
        OnToggleSwitch(false);
        OnAskMaskClik();
    }


    private  void OnToggleSwitch(bool isSelect)
    {
        if (gjRoom.isOn)
        {
            page1.SetActive(false);
            page2.SetActive(true);
        }
        else
        {
            page1.SetActive(true);
            page2.SetActive(false);
        }
    }

    private void OnToggleHome(bool isSelect)
    {        
        if (homeMianMoney.isOn)
        {
            if(diamondsNumber8!=null) diamondsNumber8.SafeSetText("(5钻石)");
            if(diamondsNumber16!=null) diamondsNumber16.SafeSetText("(10钻石)");
        }
        else
        {
            if(diamondsNumber8!=null) diamondsNumber8.SafeSetText("(1钻石)");
            if(diamondsNumber16!=null) diamondsNumber16.SafeSetText("(2钻石)");
        }
    }
    //玩法问号点击事件
    private void OnBtnAskPlayClik()
    {
        explainPlay.gameObject.SetActive(true);
        btnMask.gameObject.SetActive(true);
    }
    //派彩问号点击事件
    private void OnBtnAskColorClik()
    {
        explainColor.gameObject.SetActive(true);
        btnMask.gameObject.SetActive(true);
    }
    //血拼问号点击事件
    private void OnBtnAskBloonClik()
    {
        explainBloon.gameObject.SetActive(true);
        btnMask.gameObject.SetActive(true);
    }
    /// <summary>
    /// 遮罩点击事件
    /// </summary>
    private void OnAskMaskClik()
    {            
        if(explainPlay!=null) explainPlay.gameObject.SetActive(false);
        if(explainColor!=null) explainColor.gameObject.SetActive(false);
        if(explainBloon!=null) explainBloon.gameObject.SetActive(false);
        if(btnMask!=null) btnMask.gameObject.SetActive(false);
    }


}
