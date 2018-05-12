//===================================================
//Author      : DRB
//CreateTime  ：4/7/2017 3:40:21 PM
//Description ：分享模块控制器
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareCtrl : SystemCtrlBase<ShareCtrl>, ISystemCtrl
{
    public UIShareWindow m_UIShareWindow;

    private List<ShareEntity> m_ListShare;

    #region DicNotificationInterests 注册UI事件
    /// <summary>
    /// 注册UI事件
    /// </summary>
    /// <returns></returns>
    public override Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler> dic = new Dictionary<string, DispatcherBase<UIDispatcher, object[], string>.Handler>();
        dic.Add(ConstDefine.BtnShareViewBack, OnBtnShareViewBackClick);
        dic.Add(ConstDefine.BtnShareFriend, OnBtnShareFriend);
        dic.Add(ConstDefine.BtnShareMoments, OnBtnShareMoments);
        return dic;
    }
    #endregion

    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.Share:
                UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Share, (GameObject go) =>
                {
                    m_UIShareWindow = go.GetComponent<UIShareWindow>();
                });
                break;

        }

    }
    #region OnBtnShareViewBackClick 分享界面返回按钮点击
    /// <summary>
    /// 分享界面返回按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnShareViewBackClick(object[] obj)
    {
        m_UIShareWindow.Close();
    }
    #endregion

    #region OnBtnShareFriend 分享好友按钮点击
    /// <summary>
    /// 分享好友按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnShareFriend(object[] obj)
    {
        ShareURL(ShareType.Friend);
    }
    #endregion

    #region OnBtnShareMoments 分享朋友圈按钮点击
    /// <summary>
    /// 分享朋友圈按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnBtnShareMoments(object[] obj)
    {
        ShareURL(ShareType.Moments);
    }
    #endregion

    #region ShareURL 分享地址
    /// <summary>
    /// 分享地址
    /// </summary>
    /// <param name="type"></param>
    public void ShareURL(ShareType type)
    {
        if (m_ListShare == null)
        {
            RequestShare();
            return;
        }
        ShareEntity entity = null;
        for (int i = 0; i < m_ListShare.Count; ++i)
        {
            if (m_ListShare[i].id == (int)type)
            {
                entity = m_ListShare[i];
                break;
            }
        }
        SDK.Instance.WXShareUrl((WXShareType)entity.type, StringConvert(entity.url), StringConvert(entity.title), StringConvert(entity.content), WXShareCallBack,entity.isReward == 1);
    }
    #endregion

    #region InviteFriend 邀请好友
    /// <summary>
    /// 邀请好友
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="gameId"></param>
    /// <param name="cfgId"></param>
    /// <param name="groupId"></param>
    public void InviteFriend(int roomId,int gameId,int[] cfgId, int groupId = 0)
    {
        if (m_ListShare == null)
        {
            RequestShare();
            return;
        }
        ShareEntity entity = null;
        for (int i = 0; i < m_ListShare.Count; ++i)
        {
            if (m_ListShare[i].id == (int)ShareType.InGame)
            {
                entity = m_ListShare[i];
                break;
            }
        }
        SDK.Instance.WXShareUrl((WXShareType)entity.type, StringConvert(entity.url, roomId,gameId,cfgId,groupId), StringConvert(entity.title, roomId, gameId, cfgId, groupId), StringConvert(entity.content, roomId, gameId, cfgId, groupId), WXShareCallBack, entity.isReward == 1);
    }
    #endregion

    #region RequestShare 请求分享数据
    /// <summary>
    /// 请求分享数据
    /// </summary>
    public void RequestShare()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + "game/share_cfg/", OnRequestShareCallBack,true, "share_cfg", dic);
    }
    #endregion

    #region OnRequestShareCallBack 请求分享数据回调
    /// <summary>
    /// 请求分享数据回调
    /// </summary>
    /// <param name="args"></param>
    private void OnRequestShareCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            ShowMessage("提示", "网络连接失败", MessageViewType.Ok);
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("提示", args.Value.msg, MessageViewType.Ok);
                return;
            }

            m_ListShare = LitJson.JsonMapper.ToObject<List<ShareEntity>>(args.Value.data.ToJson());
        }
    }
    #endregion

    #region StringConvert 字符串转换
    /// <summary>
    /// 字符串转换
    /// </summary>
    /// <param name="str"></param>
    /// <param name="roomId"></param>
    /// <param name="gameId"></param>
    /// <param name="cfgId"></param>
    /// <param name="groupId"></param>
    /// <returns></returns>
    private string StringConvert(string str,int roomId = 0,int gameId = 0,int[] cfgId = null, int groupId = 0)
    {
        if (str.IndexOf("[&playerId]") > -1)
        {
            str = str.Replace("[&playerId]", AccountProxy.Instance.CurrentAccountEntity.passportId.ToString());
        }
        if (str.IndexOf("[&groupId]") > -1)
        {
            if (groupId > 0)
            {
                str = str.Replace("[&groupId]", "群Id:" + groupId.ToString());
            }
            else
            {
                str = str.Replace("[&groupId]", string.Empty);
            }
        }
        if (str.IndexOf("[&roomId]") > -1)
        {
            if (roomId == 0)
            {
                roomId = GameCtrl.Instance.GetRoomEntity().roomId;
            }
            str = str.Replace("[&roomId]", roomId.ToString());
        }
        if (str.IndexOf("[&gameName]") > -1)
        {
            if (gameId == 0)
            {
                gameId = GameCtrl.Instance.CurrentGameId;
            }
            cfg_gameEntity entity = cfg_gameDBModel.Instance.Get(gameId);
            if (entity != null)
            {
                str = str.Replace("[&gameName]", entity.GameName);
            }
        }
        if (str.IndexOf("[&rule]") > -1)
        {
            if (cfgId == null)
            {
                List<cfg_settingEntity> lst = GameCtrl.Instance.GetRoomEntity().Config;
                string temp = string.Empty;
                for (int i = 0; i < lst.Count; ++i)
                {
                   
                    temp += lst[i].name + " ";
                }
                str = str.Replace("[&rule]", temp);
            }
            else
            {
                string temp = string.Empty;
                for (int i = 0; i < cfgId.Length; ++i)
                {
                    cfg_settingEntity entity = cfg_settingDBModel.Instance.Get(cfgId[i]);
                    if (entity != null)
                    {
                        temp += entity.name + " ";
                    }
                }
                str = str.Replace("[&rule]", temp);
            }
        }
        return str;
    }
    #endregion

    #region ShareTexture 分享图片
    /// <summary>
    /// 分享图片
    /// </summary>
    /// <param name="type"></param>
    /// <param name="path"></param>
    public void ShareTexture(WXShareType type,string path)
    {
        SDK.Instance.WXSharePicture(type, path, WXShareCallBack);
    }
    #endregion

    #region WXShareCallBack 微信分享回调
    /// <summary>
    /// 微信分享回调
    /// </summary>
    /// <param name="isSuccess"></param>
    /// <param name="isReward"></param>
    public void WXShareCallBack(bool isSuccess,bool isReward)
    {
        if (AccountProxy.Instance.CurrentAccountEntity.shared) return;

        if (isSuccess && isReward)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["passportId"] = AccountProxy.Instance.CurrentAccountEntity.passportId;
            dic["token"] = AccountProxy.Instance.CurrentAccountEntity.token;
            NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrShared, OnSharedRewardCallBack, true, ConstDefine.HTTPFuncShared,dic);
        }
    }
    #endregion

    #region OnSharedRewardCallBack 分享奖励回调
    /// <summary>
    /// 分享奖励回调
    /// </summary>
    /// <param name="args"></param>
    private void OnSharedRewardCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            ShowMessage("错误","网络连接失败");
        }
        else
        {
            if (args.Value.code < 0)
            {
                ShowMessage("提示", args.ErrorMsg);
                return;
            }
            AccountProxy.Instance.SetCards(args.Value.data["cards"].ToString().ToInt());
            AccountProxy.Instance.CurrentAccountEntity.shared = true;
        }
    }
    #endregion

    #region ScreenCapture 屏幕截图
    /// <summary>
    /// 屏幕截图
    /// </summary>
    /// <param name="onComplete"></param>
    /// <returns></returns>
    public IEnumerator ScreenCapture(Action<Texture2D> onComplete)
    {
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0, true);
        if (onComplete != null)
        {
            onComplete(tex);
        }
    }
    #endregion
}

