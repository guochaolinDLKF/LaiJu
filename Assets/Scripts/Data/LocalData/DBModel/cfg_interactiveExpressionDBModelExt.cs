//===================================================
//Author      : DRB
//CreateTime  ：8/22/2017 4:52:24 PM
//Description ：
//===================================================
using UnityEngine;


public partial class cfg_interactiveExpressionDBModel 
{

    public cfg_interactiveExpressionEntity GetEntityByCode(string code)
    {
        for (int i = 0; i < m_List.Count; ++i)
        {
            if (m_List[i].code.Equals(code))
            {
                return m_List[i];
            }
        }
        return null;
    }

}
