//===================================================
//Author      : DRB
//CreateTime  ：4/26/2017 2:52:54 PM
//Description ：结束界面窗口视图
//===================================================
using System.Collections.Generic;
using com.oegame.mahjong.protobuf;
using proto.mahjong;
using UnityEngine;
using UnityEngine.UI;

public class UIResultView : UIWindowViewBase
{
    [SerializeField]
    private Text m_TextRoomId;
    [SerializeField]
    private Transform m_SeatInfoContainer;

    [SerializeField]
    private Text m_TextBack;
    [SerializeField]
    private Button m_BtnShare;
    [SerializeField]
    private Text m_TextDateTime;
    [SerializeField]
    private Text m_TextGameLoop;

    private List<UIItemResult> m_Cache = new List<UIItemResult>();

    protected override void OnAwake()
    {
        base.OnAwake();
        m_BtnShare.gameObject.SetActive(SystemProxy.Instance.IsInstallWeChat);
    }

    public void SetUI(OP_ROOM_RESULT proto, bool isMatch)
    {
        m_TextRoomId.SafeSetText(proto.roomId.ToString());
        int winerIndex = 0;
        int gold = 0;
        for (int i = 0; i < proto.resultCount(); ++i)
        {
            if (proto.getResult(i).gold > gold)
            {
                winerIndex = i;
                gold = proto.getResult(i).gold;
            }
        }
        m_TextDateTime.SafeSetText(System.DateTime.Now.ToString(ConstDefine.TIME_FORMAT));
        m_TextGameLoop.SafeSetText(string.Format("游戏局数:{0}", proto.maxLoop));
#if IS_SHUANGLIAO

#endif
        for (int i = 0; i < m_Cache.Count; ++i)
        {
            UIPoolManager.Instance.Despawn(m_Cache[i].transform);
        }
        m_Cache.Clear();

        for (int i = 0; i < proto.resultCount(); ++i)
        {
            GameObject go = null;
            if (winerIndex == i)
            {
                go = UIPoolManager.Instance.Spawn("UIItemResult2").gameObject;
            }
            else
            {
                go = UIPoolManager.Instance.Spawn("UIItemResult").gameObject;
            }
            go.SetParent(m_SeatInfoContainer);
            UIItemResult result = go.GetComponent<UIItemResult>();
            result.SetUI(proto.getResult(i), winerIndex == i);
            m_Cache.Add(result);
        }

        m_TextBack.SafeSetText(isMatch ? "继续" : "返回");
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnResultViewBack":
                SendNotification("btnResultViewBack");
                break;
            case "btnResultViewShare":
                SendNotification("btnResultViewShare");
                break;
        }
    }

}
