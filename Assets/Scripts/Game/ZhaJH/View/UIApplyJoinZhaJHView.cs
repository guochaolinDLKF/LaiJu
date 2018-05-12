//===================================================
//Author      : DRB
//CreateTime  ：7/13/2017 6:00:53 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhaJh;

public class UIApplyJoinZhaJHView : UIViewBase
{
    [SerializeField]
    private Transform tran;//生成申请进入房间挂载点

    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add(ZhaJHMethodname.OnZJHCloneApplyJoin, CloneApplyJoin);//高级房给房主发送申请进入房间的消息
        dic.Add(ZhaJHMethodname.OnZJHCleantRoom, CleantRoom);//高级房玩家取消进入房间。清除自己的申请消息
        return dic;
    }

    private void CloneApplyJoin(TransferData data)
    {        
        PlayerEntityZjh player = data.GetValue<PlayerEntityZjh>("player");
        ZJHPrefabManager.Instance.LoadApplyJoinRoom(player,(GameObject go) => {
            if (tran!=null)
            {
                go.name = player.playerId.ToString();
                go.transform.SetParent(tran);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
            }          
        });       
    }

    private void CleantRoom(TransferData data)
    {
        int playerId = data.GetValue<int>("playerId");
        for (int i = 0; i < tran.childCount; i++)
        {
            if (playerId.ToString()==tran.GetChild(i).name)
            {
                Destroy(tran.GetChild(i).gameObject);
            }
        }

    }
}
