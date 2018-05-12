//===================================================
//Author      : DRB
//CreateTime  ：5/11/2017 5:56:24 PM
//Description ：
//===================================================
using UnityEngine;


public class UIPageGroup : UIButtonGroup 
{

    [SerializeField]
    protected GameObject[] m_Pages;


    protected override void OnBtnClick(GameObject go)
    {
        for (int i = 0; i < m_Buttons.Length; ++i)
        {
            if (go == m_Buttons[i].gameObject)
            {
                m_Buttons[i].image.overrideSprite = m_SelectSprite;
                m_Pages[i].SetActive(true);
            }
            else
            {
                m_Buttons[i].image.overrideSprite = m_NormalSprite;
                m_Pages[i].SetActive(false);
            }
        }
    }
}
