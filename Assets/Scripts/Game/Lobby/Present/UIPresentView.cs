//===================================================
//Author      : DRB
//CreateTime  ：4/17/2017 7:49:48 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPresentView : UIWindowViewBase
{

    [SerializeField]
    public InputField m_InputId;
    
    [SerializeField]
    private GameObject m_Page1;
    [SerializeField]
    private GameObject m_Page2;
    [SerializeField]
    private RawImage m_ImageHead;

    [SerializeField]
    private Text m_TextId;
    [SerializeField]
    private Text m_TextNickname;


    [SerializeField]
    private InputField inputField;

    public int index = 1;

    protected override void OnStart()
    {
        inputField.text = "1";
        base.OnStart();
        inputField.onEndEdit.AddListener(FiledClick);
        index = 1;
    }

    private void FiledClick(string text)
    {
        int num;
        
        num = text.ToInt();
        if(inputField)
        {
            index++;
            index = num;
       
        }
        else
        {
            index--;
        }
        
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnPresentViewPresent":
                SendNotification("btnPresentViewPresent");
                break;
            case "btnPresentViewNext":
                SendNotification("btnPresentViewNext");
                break;
            case "btnPresentViewReturn":
                SendNotification("btnPresentViewReturn");
                break;

            case "btnjiankashu":
                OnPresentSubtractClick();
                break;
            case "btnaddkashu":
                OnPresentAddClick();
                break;

        }
    }
    private void OnPresentAddClick()
    {
        this.index++;
        if (this.index > 999)
        {
            this.index = 999;
        }
        inputField.text = index.ToString();
    }

    private void OnPresentSubtractClick()
    {

       
        this.index--;
        if (this.index < 1)
        {
            this.index = 1;
           
        }
        inputField.text = index.ToString();
    }

    
    




    public void SetUI(int pageNumber,int id = 0,string head = "",string nickName = "")
    {
        bool isPageOne = true;
        isPageOne = pageNumber == 1;
        m_Page1.SetActive(isPageOne);
        m_Page2.SetActive(!isPageOne);
        TextureManager.Instance.LoadHead(head,OnLoadHeadFinish);
        m_TextId.SafeSetText(id.ToString());
        m_TextNickname.SafeSetText(nickName);
    }

    private void OnLoadHeadFinish(Texture2D tex)
    {
        if (m_ImageHead != null)
        {
            m_ImageHead.texture = tex;
        }
    }

    public UIPresentView()
    {

    }

 

}
