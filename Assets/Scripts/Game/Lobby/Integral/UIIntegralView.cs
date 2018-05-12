//===================================================
//Author      : DRB
//CreateTime  ：11/28/2017 3:20:17 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIntegralView : UIItemBase
{
    [SerializeField]
    private RawImage m_image;
    [SerializeField]
    private Text m_priceText;
    [SerializeField]
    private Text m_nameText;
    [SerializeField]
    private Button m_button;

    public int index;
    public void SetUI(TransferData data)
    {
        EventTriggerListener.Get(m_button.gameObject).onClick = OnBtnSpendGiftClick;

        IntegralEntity IntegralEntity = data.GetValue<IntegralEntity>("integralEntity");

        index = IntegralEntity.id;

        TextureManager.Instance.LoadHead(IntegralEntity.img_url, (Texture2D texture2d) =>
        {
            m_image.texture = texture2d;
            m_priceText.SafeSetText(IntegralEntity.need_score.ToString());
            m_nameText.SafeSetText(IntegralEntity.name.ToString());
        });
    }
    private void OnBtnSpendGiftClick(GameObject go)
    {
        switch (go.name)
        {
            case "btn_spendGift":
                SendNotification("btn_spendGift", index);
                break;
        }
    }
}
