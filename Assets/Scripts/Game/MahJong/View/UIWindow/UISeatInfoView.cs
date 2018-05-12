//===================================================
//Author      : DRB
//CreateTime  ：6/2/2017 2:45:54 PM
//Description ：座位信息窗口
//===================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISeatInfoView : UIWindowViewBase 
{
    [SerializeField]
    private RawImage m_ImgHead;
    [SerializeField]
    private Text m_TextId;
    [SerializeField]
    private Text m_TextNickname;
    [SerializeField]
    private Image m_ImageMan;
    [SerializeField]
    private Image m_ImageWoman;
    [SerializeField]
    private Text m_TextIP;
    [SerializeField]
    private Text[] m_ArrDistance;
    [SerializeField]
    private Transform m_InteractiveContainer;

    private int m_nSeatPos;

    private Action<int, int> OnEmojiClick;

    public void SetUI<T>(SeatEntityBase seat,List<T> lstOtherSeat) where T : SeatEntityBase
    {
        m_nSeatPos = seat.Pos;
        TextureManager.Instance.LoadHead(seat.Avatar,(Texture2D tex)=> 
        {
            m_ImgHead.texture = tex;
        });
        m_TextId.SafeSetText(seat.PlayerId.ToString());
        m_TextNickname.SafeSetText(seat.Nickname);
        m_ImageMan.gameObject.SetActive(seat.Gender == 1);
        m_ImageWoman.gameObject.SetActive(seat.Gender == 0);
        m_TextIP.SafeSetText(seat.IP);

        for (int i = 0; i < m_ArrDistance.Length; ++i)
        {
            if (i < lstOtherSeat.Count)
            {
                m_ArrDistance[i].gameObject.SetActive(true);
                if (seat.Latitude == 0)
                {
                    m_ArrDistance[i].SafeSetText(string.Format("未获取到<color=#306BD8FF>{0}</color>的位置",seat.Nickname));
                }
                else if (lstOtherSeat[i].Latitude == 0)
                {
                    m_ArrDistance[i].SafeSetText(string.Format("未获取到<color=#306BD8FF>{0}</color>的位置", lstOtherSeat[i].Nickname));
                }
                else
                {
                    float distance = LPSUtil.CalculateDistance(seat.Latitude, seat.Longitude, lstOtherSeat[i].Latitude, lstOtherSeat[i].Longitude);
                    m_ArrDistance[i].SafeSetText(string.Format("与<color=#306BD8FF>{0}</color>相距：{1}km", lstOtherSeat[i].Nickname, distance.ToString("0.00")));
                }
            }
            else
            {
                m_ArrDistance[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetEmoji(List<cfg_interactiveExpressionEntity> lst,Action<int,int> onEmojiClick)
    {
        if (m_InteractiveContainer == null) return;
        OnEmojiClick = onEmojiClick;
        for (int i = 0; i < lst.Count; ++i)
        {
            cfg_interactiveExpressionEntity entity = lst[i];
            GameObject go = new GameObject();
            go.SetParent(m_InteractiveContainer);
            Image img = go.AddComponent<Image>();
            UIItemChatExpression item = go.AddComponent<UIItemChatExpression>();
            string path = string.Format("download/{0}/source/uisource/gameuisource/interactive/{1}.drb",ConstDefine.GAME_NAME,entity.animation);
            img.overrideSprite = AssetBundleManager.Instance.LoadSprite(path, entity.image);
            item.SetUI(entity.id, entity.image, OnExpressionClick);
        }
    }

    private void OnExpressionClick(int expressionId)
    {
        if (OnEmojiClick != null)
        {
            OnEmojiClick(m_nSeatPos, expressionId);
        }
    }
}
