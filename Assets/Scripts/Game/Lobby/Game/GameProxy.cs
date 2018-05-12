//===================================================
//Author      : DRB
//CreateTime  ：5/5/2017 2:35:22 PM
//Description ：
//===================================================
using System.Collections.Generic;
using UnityEngine;


public class GameProxy : Singleton<GameProxy>
{

    /// <summary>
    /// 服务器列表
    /// </summary>
    public List<GameEntity> GameServers;

    public GameEntity Get(int gameId)
    {
        if (gameId == 0) return null;
        if (GameServers == null) return null;
        for (int i = 0; i < GameServers.Count; ++i)
        {
            if (gameId == GameServers[i].gameId)
            {
                return GameServers[i];
            }
        }
        return null;
    }

    public GameEntity GetChatServer()
    {
        for (int i = 0; i < GameServers.Count; ++i)
        {
            if (GameServers[i].gameId == 0)
            {
                return GameServers[i];
            }
        }
        return null;
    }
}
