//===================================================
//Author      : DRB
//CreateTime  ：4/26/2017 5:05:17 PM
//Description ：
//===================================================
using System.Collections.Generic;
using UnityEngine;


public partial class cfg_commonMessageDBModel 
{



    public cfg_commonMessageEntity GetEntityByMessage(string message)
    {
        for (int i = 0; i < m_List.Count; ++i)
        {
            if (m_List[i].message == message)
            {
                return m_List[i];
            }
        }
        return null;
    }


    public List<cfg_commonMessageEntity> GetListByGameId(int gameId)
    {
        List<cfg_commonMessageEntity> lst = new List<cfg_commonMessageEntity>();
        for (int i = 0; i < m_List.Count; ++i)
        {
            if (m_List[i].gameId == gameId)
            {
                lst.Add(m_List[i]);
            }
        }

        if (lst.Count == 0)
        {
            return GetList();
        }
        return lst;
    }

}
