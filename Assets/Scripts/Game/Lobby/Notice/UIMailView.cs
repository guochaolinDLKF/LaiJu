//===================================================
//Author      : DRB
//CreateTime  ：5/9/2017 4:07:44 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMailView : UIWindowViewBase
{
    #region
    [SerializeField]
    private Transform m_Container;
    [SerializeField]
    private Transform m_btnContent;
    [SerializeField]
    private RawImage m_NoticeImg;
    [SerializeField]
    private Text m_NoticeText;

    private List<UIItemMail> m_List = new List<UIItemMail>();
    private List<BtnItem> ItemList = new List<BtnItem>();
    [SerializeField]
    private List<MailEntity> m_maillst;
    private List<NoticeEntity> m_lst;

    [SerializeField]
    private GameObject m_Notice;
    [SerializeField]
    private GameObject m_Mail;
    [SerializeField]
    private List<ToggleItem> TagList;//上面两个按钮列表

    private string m_link_url = "";
    #region 加载图片的方法
    /// <summary>
    /// 加载图片方法
    /// </summary>
    /// <param name="_name"></param>
    /// <returns></returns>
    //Sprite LoadSpriteM(int _name)
    //{

    //    string spriteName = string.Format("ljbg_page{0}", _name.ToString());//图片名称
    //    string path = string.Format("download/{0}/source/uisource/mail.drb", ConstDefine.GAME_NAME);//图片路径
    //    Sprite image = AssetBundleManager.Instance.LoadSprite(path, spriteName);
    //    return image;
    //}
    #endregion


    public void SetNotice(string notice)
    {
        // m_TextNotice.SafeSetText(notice);
        // AssetBundleManager.Instance.LoadSprite()

    }
    public void OnChangeTag(int _index)
    {

        for (int i = 0; i < TagList.Count; i++)
        {
            if (_index == i)
            {

                m_Notice.SetActive(false);
                m_Mail.SetActive(true);
                NoticeCtrl.Instance.RequestMail();
            }
            else
            {

                m_Notice.SetActive(true);
                m_Mail.SetActive(false);
            }
        }
    }


    public void SetBtnLeft(List<NoticeEntity> lst)
    {
        m_lst = lst;
        for (int i = 0; i < ItemList.Count; ++i)
        {
            UIPoolManager.Instance.Despawn(ItemList[i].transform);
        }
        ItemList.Clear();
        for (int i = 0; i < lst.Count; i++)
        {
            Transform tran = UIPoolManager.Instance.Spawn("UIItemNoticeBtn");
            tran.SetParent(m_btnContent);//设置父物体
            tran.localScale = Vector3.one;
            tran.localPosition = Vector3.zero;
            Toggle toggle = tran.GetComponent<Toggle>();
            toggle.group = m_btnContent.gameObject.GetComponent<ToggleGroup>();
            BtnItem btnItem = tran.gameObject.GetComponent<BtnItem>();
            ItemList.Add(btnItem);
            btnItem.SetUI(lst[i].title, lst[i].id, OnToggleCallBack);
            if (i == 0)
            {
                Debug.Log(btnItem.name + "======================");
                toggle.isOn = true;
                //btnItem.SetToggle();
            }
            else
            {
                toggle.isOn = false;
            }
        }
    }

    private void OnToggleCallBack(int id)
    {
        
        for (int i = 0; i < m_lst.Count; i++)
        {
            if (id == m_lst[i].id)
            {
                TextureManager.Instance.LoadHead(m_lst[i].img_url, (Texture2D texture2d) =>
                {
                    Debug.LogWarning("图片路径---"+m_lst[i].img_url);
                    m_NoticeImg.texture = texture2d;
                },true);
                m_NoticeText.text = m_lst[i].content;
                m_link_url = m_lst[i].link_url;//在脚本中定义变量存储网址
            }
        }
    }



    public void SetMail(List<MailEntity> lst)
    {
        m_maillst = lst;
        for (int i = 0; i < m_List.Count; ++i)
        {
            m_List[i].gameObject.SetActive(false);
        }

        if (lst == null) return;

        UIViewManager.Instance.LoadItemAsync("UIItemMail", (GameObject prefab) =>
        {
            for (int i = 0; i < lst.Count; ++i)
            {
                UIItemMail mail = null;
                if (i < m_List.Count)
                {
                    mail = m_List[i];
                    mail.gameObject.SetActive(true);
                }
                else
                {
                    GameObject go = Instantiate(prefab);
                    go.SetParent(m_Container);
                    mail = go.GetComponent<UIItemMail>();
                    m_List.Add(mail);
                }
                mail.SetUI(lst[i].msg, lst[i].time);

            }

        });
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnMailViewNotice":
                SendNotification("btnMailViewNotice");
                break;
            case "btnMailViewMail":
                SendNotification("btnMailViewMail");
                break;
            case "RawImage":
                if (m_link_url!="")//点击的时候并且当网址不是空的时候，将网址作为参数传给NoticeCtrl.cs脚本
                {
                    SendNotification("RawImage", m_link_url);//这句代码测试完后取消注释
                    //SendNotification("RawImage", "http://www.baidu.com");//这句代码仅用来测试
                }
                
                break;
        }
    }
    #endregion
}
