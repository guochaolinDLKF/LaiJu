//===================================================
//Author      : DRB
//CreateTime  ：5/11/2017 1:48:32 PM
//Description ：游戏控制器接口
//===================================================
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏控制器接口
/// </summary>
public interface IGameCtrl 
{
    /// <summary>
    /// 创建房间
    /// </summary>
    void CreateRoom(int groupId, List<int> settingIds);
    /// <summary>
    /// 加入房间
    /// </summary>
    /// <param name="roomId"></param>
    void JoinRoom(int roomId);
    /// <summary>
    /// 重建房间
    /// </summary>
    void RebuildRoom();
    /// <summary>
    /// 退出房间
    /// </summary>
    void QuitRoom();
    /// <summary>
    /// 解散房间
    /// </summary>
    void DisbandRoom();
    /// <summary>
    /// 接收聊天消息
    /// </summary>
    void OnReceiveMessage(ChatType type, int playerId, string message,string audioName,int toPlayerId);

    RoomEntityBase GetRoomEntity();
}
