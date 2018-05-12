//===================================================
//Author      : DRB
//CreateTime  ：7/12/2017 7:35:47 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZhaJh;

public class UIZhaJHItemBtnTiShi : UIViewBase
{    
    [SerializeField]
    private RawImage m_ImageHead;//玩家头像   
    [SerializeField]
    private Text playerName;//玩家名字

    private PlayerEntityZjh player;


    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add(ZhaJHMethodname.OnZJHLookupPlayer, LookupPlayer);//高级房给房主发送申请进入房间的消息
        return dic;
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnAgree":
                SendNotification(ZhaJHMethodname.OnZJHAgreeRefuse, true, player);
                Destroy(this.gameObject);               
                break;
            case "btnRefuse":
                SendNotification(ZhaJHMethodname.OnZJHAgreeRefuse, false, player);
                Destroy(this.gameObject);
                 break;         
        }
    }



    /// <summary>
    /// 高级房申请加入房间的时候需要房主同意的提示框
    /// </summary>
    /// <param name="data"></param>    
    public void PromptSwitch(PlayerEntityZjh player)
    {
        this.player = player;               
        playerName.SafeSetText(player.playerId.ToString());
        TextureManager.Instance.LoadHead(player.avatar, OnAvatarLoadCallBack);//加载头像
    }

    /// <summary>
    /// 头像
    /// </summary>
    /// <param name="obj"></param>
    private void OnAvatarLoadCallBack(Texture2D tex)
    {
        m_ImageHead.texture = tex;
    }


    private void LookupPlayer(TransferData data)
    {
        PlayerEntityZjh player = data.GetValue< PlayerEntityZjh>("player");
        if (this.player.playerId==player.playerId)
        {
            Destroy(this.gameObject);
        }
    }





}
