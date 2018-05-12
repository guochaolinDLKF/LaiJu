//===================================================
//Author      : DRB
//CreateTime  ：4/17/2017 11:00:23 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class cfg_chatExpressionDBModel
{


    public cfg_chatExpressionEntity GetEntityIdByCode(string code)
    {
        for (int i = 0; i < m_List.Count; ++i)
        {
            if (m_List[i].code == code)
            {
                return m_List[i];
            }
        }
        return null;
    }


}
