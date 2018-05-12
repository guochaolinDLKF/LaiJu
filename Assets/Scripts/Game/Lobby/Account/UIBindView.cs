//===================================================
//Author      : DRB
//CreateTime  ：5/9/2017 1:56:48 PM
//Description ：绑定手机号窗口
//===================================================
using UnityEngine;
using UnityEngine.UI;

public class UIBindView : UIWindowViewBase 
{

    public InputField Telephone;

    public InputField VerifyCode;
    [SerializeField]
    private Text m_TextPhone;
    [SerializeField]
    private Button m_ButtonBind;
    [SerializeField]
    private Button m_ButtonUnbind;


    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnBindViewGetVerifyCode":
                SendNotification("btnBindViewGetVerifyCode");
                break;
            case "btnBindViewValidate":
                SendNotification("btnBindViewValidate");
                break;
            case "btnBindViewUnBind":
                SendNotification("btnBindViewUnBind");
                break;
        }
    }

    public void SetUI(string phone)
    {
        if (!string.IsNullOrEmpty(phone))
        {
            Telephone.gameObject.SetActive(false);
            m_TextPhone.gameObject.SetActive(true);
            m_TextPhone.SafeSetText(phone);
            m_ButtonBind.gameObject.SetActive(false);
            m_ButtonUnbind.gameObject.SetActive(true);
        }
        else
        {
            Telephone.gameObject.SetActive(true);
            m_TextPhone.gameObject.SetActive(false);
            m_ButtonBind.gameObject.SetActive(true);
            m_ButtonUnbind.gameObject.SetActive(false);
        }
    }
}
