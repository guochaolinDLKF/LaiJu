//===================================================
//Author      : DRB
//CreateTime  ：1/15/2018 9:50:26 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIAgentServiceWindow : UIWindowViewBase
{
    [SerializeField]
    private Transform m_Container;
    //[SerializeField]
    //private RawImage m_ImgLogo;

    private List<UIItemAgentService> m_Cache = new List<UIItemAgentService>();

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case ConstDefine.BtnSevice:
                SendNotification(ConstDefine.BtnSevice);
                break;
        }
    }


    public void SetUI(List<TransferData> lst, Action<string> onClick)
    {
        for (int i = 0; i < m_Cache.Count; ++i)
        {
            m_Cache[i].gameObject.SetActive(false);
        }
        //TextureManager.Instance.LoadHead(logoUrl, (Texture2D tex) =>
        //{
        //    if (tex != null && m_ImgLogo != null)
        //    {
        //        m_ImgLogo.texture = tex;
        //    }
        //});
        UIViewManager.Instance.LoadItemAsync("UIItemAgentService", (GameObject prefab) =>
        {
            for (int i = 0; i < lst.Count; ++i)
            {
                string textWX = lst[i].GetValue<string>("wx");
                string phoneNumber = lst[i].GetValue<string>("tel");
                UIItemAgentService service = null;
                if (i < m_Cache.Count)
                {
                    service = m_Cache[i];
                    service.gameObject.SetActive(true);
                }
                else
                {
                    GameObject go = Instantiate(prefab);
                    go.SetParent(m_Container);
                    service = go.GetComponent<UIItemAgentService>();
                    m_Cache.Add(service);
                }
                service.SetUI(textWX, onClick);
            }
        });

    }
}
