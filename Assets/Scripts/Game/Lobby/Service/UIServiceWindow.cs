//===================================================
//Author      : DRB
//CreateTime  ：4/7/2017 6:12:42 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIServiceWindow : UIWindowViewBase
{
    [SerializeField]
    private Transform m_Container;
    [SerializeField]
    private RawImage m_ImgLogo;

    private List<UIItemService> m_Cache = new List<UIItemService>();

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

    public void SetUI(string logoUrl,List<TransferData> lst,Action<string> onClick)
    {
        for (int i = 0; i < m_Cache.Count; ++i)
        {
            m_Cache[i].gameObject.SetActive(false);
        }
        TextureManager.Instance.LoadHead(logoUrl,(Texture2D tex)=> 
        {
            if (tex != null && m_ImgLogo != null)
            {
                m_ImgLogo.texture = tex;
            }
        });      
        UIViewManager.Instance.LoadItemAsync("UIItemService",(GameObject prefab)=> 
        {
            for (int i = 0; i < lst.Count; ++i)
            {
                string key = lst[i].GetValue<string>("Key");
                string value = lst[i].GetValue<string>("Value");
                UIItemService service = null;
                if (i < m_Cache.Count)
                {
                    service = m_Cache[i];
                    service.gameObject.SetActive(true);
                }
                else
                {
                    GameObject go = Instantiate(prefab);
                    go.SetParent(m_Container);
                    service = go.GetComponent<UIItemService>();
                    m_Cache.Add(service);
                }
                service.SetUI(key, value, onClick);
            }
        });

    }
}
