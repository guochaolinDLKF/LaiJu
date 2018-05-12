//===================================================
//Author      : DRB
//CreateTime  ：11/7/2017 4:10:52 PM
//Description ：
//===================================================
using UnityEngine;


public partial class cfg_gameDBModel 
{

    public cfg_gameEntity GetEntityByGameType(string gameType)
    {
        for (int i = 0; i < m_List.Count; ++i)
        {
            if (m_List[i].GameType.Equals(gameType,System.StringComparison.CurrentCultureIgnoreCase))
            {
                return m_List[i];
            }
        }

        return null;
    }

}
