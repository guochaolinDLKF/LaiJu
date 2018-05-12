//===================================================
//Author      : DRB
//CreateTime  ：5/9/2017 9:57:46 AM
//Description ：
//===================================================
using UnityEngine;
using UnityEngine.UI;

public class UIButtonGroup : MonoBehaviour 
{
    [SerializeField]
    protected Button[] m_Buttons;
    [SerializeField]
    protected Sprite m_NormalSprite;
    [SerializeField]
    protected Sprite m_SelectSprite;

    private void Awake()
    {
        for (int i = 0; i < m_Buttons.Length; ++i)
        {
            EventTriggerListener.Get(m_Buttons[i].gameObject).onClick += OnBtnClick;
        }
    }

    protected virtual void OnBtnClick(GameObject go)
    {
        for (int i = 0; i < m_Buttons.Length; ++i)
        {
            if (m_Buttons[i].gameObject == go)
            {
                m_Buttons[i].image.overrideSprite = m_SelectSprite;
            }
            else
            {
                m_Buttons[i].image.overrideSprite = m_NormalSprite;
            }
        }
    }

}
