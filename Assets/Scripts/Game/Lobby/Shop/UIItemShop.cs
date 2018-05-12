//===================================================
//Author      : DRB
//CreateTime  ：4/22/2017 1:29:15 PM
//Description ：
//===================================================
using System;
using UnityEngine;
using UnityEngine.UI;


public class UIItemShop : MonoBehaviour 
{
    [SerializeField]
    private Text m_TextPrice;
    [SerializeField]
    private Text m_TextCount;
    [SerializeField]
    private Text m_TextGive;
    [SerializeField]
    private Button m_Button;
    [SerializeField]
    private RawImage m_ImgIcon;
    [SerializeField]
    private Image m_ImgGiveBg;
    [SerializeField]
    private Image m_ImgHot;

    private int m_nPropId;

    private Action<int> m_OnClick;

    /// <summary>
    /// 商品单位
    /// </summary>
#if IS_LAOGUI
    private const string SHOP_UNIT = "个";
#else
    private const string SHOP_UNIT = "张";
#endif



    private void Start()
    {
        m_Button.onClick.AddListener(OnBtnClick);
    }

    private void OnBtnClick()
    {
        AudioEffectManager.Instance.Play("btnclick", Vector3.zero, false);
        if (m_OnClick != null)
        {
            m_OnClick(m_nPropId);
        }
    }

    public void SetUI(string iconName,int propId,int price,int count,int give, bool isHot, Action<int> onClick,bool isBind = true)
    {
        m_nPropId = propId;
        m_TextPrice.SafeSetText(string.Format("￥{0}", price.ToString()));
        m_TextCount.SafeSetText(string.Format("{0}{1}", count.ToString(), SHOP_UNIT));

        if (give != 0 && isBind)
        {
            m_TextGive.SafeSetText(string.Format("赠+{0}", give.ToString()));
            if (m_ImgGiveBg != null)
            {
                m_ImgGiveBg.gameObject.SetActive(true);
            }
        }
        else
        {
            if (m_ImgGiveBg != null)
            {
                m_ImgGiveBg.gameObject.SetActive(false);
            }
            m_TextGive.SafeSetText(string.Empty);
        }
        m_OnClick = onClick;

        TextureManager.Instance.LoadHead(iconName,(Texture2D tex)=> 
        {
            if (m_ImgIcon != null && tex != null)
            {
                m_ImgIcon.texture = tex;
            }
        });


        if (m_ImgHot != null)
        {
            m_ImgHot.gameObject.SetActive(isHot);
        }
    }
}
