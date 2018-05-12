//===================================================
//Author      : DRB
//CreateTime  ：4/6/2017 10:19:23 AM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using DRB.MahJong;
using UnityEngine;
using proto.common;
using proto.mahjong;

public class TEST1
{
    private int a;

    public TEST1(int a)
    {
        this.a = a;
    }
}


public class test : MonoBehaviour
{


    private List<TestNet> m_ListTest = new List<TestNet>();

    void Start()
    {
        //for (int i = 0; i < 500; ++i)
        //{
        //    TestNet test = new TestNet();
        //    test.GuestLogin();
        //    m_ListTest.Add(test);
        //}
        //string str1 = new string(new char[] { 'h', 'e' });
        //string str2 = new string(new char[] { 'h', 'e' });

        //Debug.Log(str1 == str2);
        //Debug.Log(str1.Equals(str2));
        System.Diagnostics.Process.Start("http://www.baidu.com");


        //TEST1 test1 = new TEST1(1);
        //TEST1 test2 = new TEST1(1);

        //Debug.Log(test1 == test2);
        //Debug.Log(test1.Equals(test2));

        //object obj1 = test1;
        //object obj2 = test2;

        //Debug.Log(obj2 == obj1);
        //Debug.Log(object.Equals(obj1, obj2));


        //CheckXiUnitTest();
        // Init();
        //MingGangUnitTest();
        //ChiUnitTest();
        //ChiTingUnitTest();
        //AnGangUnitTest();
        //TingUnitTest();
        //BuGangUnitTest();
        //ZhiDuiUnitTest();
    }

    //private void Update()
    //{
    //    for (int i = 0; i < m_ListTest.Count; ++i)
    //    {
    //        m_ListTest[i].OnUpdate();
    //    }
    //}

    #region Init 测试初始化数据
    /// <summary>
    /// 测试初始化数据
    /// </summary>
    private void Init()
    {
        RoomMaJiangProxy.Instance.CurrentRoom = new RoomEntity();
        RoomMaJiangProxy.Instance.PlayerSeat = new SeatEntity();
        RoomMaJiangProxy.Instance.Rule = new MahjongRule();
        RoomMaJiangProxy.Instance.InitConfig();
        //RoomMaJiangProxy.Instance.Rule.IsHardYao = false;
        //RoomMaJiangProxy.Instance.Rule.IsSoftYao = true;
        //RoomMaJiangProxy.Instance.Rule.IsNotJiaCantHu = false;//不加不胡
        //RoomMaJiangProxy.Instance.Rule.Is37Jia = false;//37夹
        //RoomMaJiangProxy.Instance.Rule.IsSingleJia = false;//单调夹
        //RoomMaJiangProxy.Instance.Rule.IsZhiDui = false;//支对
        //RoomMaJiangProxy.Instance.Rule.IsUniversalCanChi = true;
        //RoomMaJiangProxy.Instance.Rule.IsFengCanChi = true;
        //RoomMaJiangProxy.Instance.Rule.IsSevenDoubleCantHu = true;
        //RoomMaJiangProxy.Instance.PlayerSeat.IsTing = true;
        RoomMaJiangProxy.Instance.PlayerSeat.PokerList = new List<Poker>()
        {
            new Poker(0,1,1),
            new Poker(0,1,1),
            new Poker(0,1,1),
            new Poker(0,1,1),
            new Poker(0,1,6),
            new Poker(0,1,6),
            new Poker(0,2,4),
            new Poker(0,2,4),
            new Poker(0,3,3),
            new Poker(0,3,3),
            new Poker(0,3,9),
            new Poker(0,3,8),
            new Poker(0,4,4),
            //new Poker(0,5,3),
        };
        RoomMaJiangProxy.Instance.PlayerSeat.HitPoker = new Poker(0, 2, 8);
        // RoomMaJiangProxy.Instance.PlayerSeat.UsedPokerList.Add(new PokerCombinationEntity(OperatorType.Chi, 0, new List<Poker>()
        //{
        //    new Poker(0,2,6),
        //    new Poker(0,2,7),
        //    new Poker(0,2,8),
        //}));
        // RoomMaJiangProxy.Instance.PlayerSeat.UsedPokerList.Add(new PokerCombinationEntity(OperatorType.Chi, 0, new List<Poker>()
        // {
        //     new Poker(0,3,1),
        //     new Poker(0,3,2),
        //     new Poker(0,3,3),
        // }));
        // RoomMaJiangProxy.Instance.PlayerSeat.UsedPokerList.Add(new PokerCombinationEntity(OperatorType.LiangXi, 0, new List<Poker>()
        //{
        //    new Poker(0,1,9),
        //    new Poker(0,2,9),
        //    new Poker(0,3,9),
        //}));
        //RoomMaJiangProxy.Instance.PlayerSeat.UsedPokerList.Add(new PokerCombinationEntity(OperatorType.LiangXi, 0, new List<Poker>()
        //{
        //    new Poker(0,3,9),
        //    new Poker(0,1,9),
        //    new Poker(0,5,2),
        //}));
        RoomMaJiangProxy.Instance.PlayerSeat.UniversalList.Add(new Poker(0, 4, 4));
    }
    #endregion

    #region EfficiencyTest 和牌算法效率测试
    /// <summary>
    /// 和牌算法效率测试
    /// </summary>
    private void EfficiencyTest()
    {
        long before = TimeUtil.GetTimestampMS();
        int huTimes = 0;
        for (int i = 0; i < 100000; ++i)
        {
            List<Poker> lst = new List<Poker>();
            for (int j = 0; j < 13; ++j)
            {
                int color = UnityEngine.Random.Range(1, 6);
                int size = color > 3 ? UnityEngine.Random.Range(1, 4) : UnityEngine.Random.Range(1, 10);
                lst.Add(new Poker(0, color, size));
            }
            List<List<CardCombination>> result = MahJongHelper.CheckTing(lst, new List<Poker>() { new Poker(0, 5, 1) },true,true,true);
            if (result != null && result.Count > 0)
            {
                ++huTimes;
            }
        }
        long after = TimeUtil.GetTimestampMS();
        Debug.Log("计算之前时间戳=" + before);
        Debug.Log("计算之后时间戳=" + after);
        Debug.Log("计算所需时间=" + (after - before) + "毫秒");
        Debug.Log("胡了" + huTimes + "次");
    }
    #endregion

    #region ChiTingUnitTest 吃听单元测试
    /// <summary>
    /// 吃听单元测试
    /// </summary>
    private void ChiTingUnitTest()
    {
        List<List<Poker>> result = RoomMaJiangProxy.Instance.GetChiTing(new Poker(0, 1, 7));
        Debug.Log(result.Count);
    }
    #endregion

    #region ChiUnitTest 吃单元测试
    /// <summary>
    /// 吃单元测试
    /// </summary>
    private void ChiUnitTest()
    {
        List<List<Poker>> result = RoomMaJiangProxy.Instance.GetChi(new Poker(0, 4, 2));
        if (result == null) return;
        Debug.Log(result.Count);
    }
    #endregion

    #region AnGangUnitTest 暗杠单元测试
    /// <summary>
    /// 暗杠单元测试
    /// </summary>
    private void AnGangUnitTest()
    {
        List<List<Poker>> result = RoomMaJiangProxy.Instance.GetAnGang();
        if (result == null) return;
        Debug.Log(result.Count);
    }
    #endregion

    #region TingUnitTest2 听单元测试
    /// <summary>
    /// 听单元测试
    /// </summary>
    private void TingUnitTest()
    {
        Dictionary<Poker,List<Poker>> result = RoomMaJiangProxy.Instance.CheckAllTing();
        Debug.Log(result.Count);

        foreach (var pair in result)
        {
            Debug.Log("打" + pair.Key.ToString());
            for (int i = 0; i < pair.Value.Count; ++i)
            {
                Debug.Log("胡" + pair.Value[i].ToString());
            }
        }
        //for (int i = 0; i < result.Count; ++i)
        //{
        //    Debug.Log(result[i].ToString());
        //}
        //for (int i = 0; i < result.Count; ++i)
        //{
        //    string str = "";
        //    for (int j = 0; j < result[i].Count; ++j)
        //    {
        //        str += result[i][j].CardType + ",";
        //        if (result[i][j].LackCardIds != null)
        //        {

        //            str += "缺的牌：";
        //            for (int k = 0; k < result[i][j].LackCardIds.Count; ++k)
        //            {
        //                str += result[i][j].LackCardIds[k].ToString() + "  ";
        //            }
        //        }

        //        if (result[i][j].CurrentCombination == null) continue;
        //        str += "当前牌:";
        //        for (int k = 0; k < result[i][j].CurrentCombination.Count; ++k)
        //        {

        //            str += result[i][j].CurrentCombination[k].ToString() + "  ";
        //        }
        //        str += " | ";
        //    }
        //    Debug.Log(str);
        //    Debug.Log("---------------------------------");
        //}
    }
    #endregion

    #region BuGangUnitTest 补杠单元测试
    /// <summary>
    /// 补杠单元测试
    /// </summary>
    private void BuGangUnitTest()
    {
        List<Poker> result = RoomMaJiangProxy.Instance.GetBuGang();
        if (result == null) return;
        Debug.Log(result.Count);
    }
    #endregion

    #region ZhiDuiUnitTest 支对单元测试
    /// <summary>
    /// 支对单元测试
    /// </summary>
    private void ZhiDuiUnitTest()
    {
        bool isMust = false;
        List<List<Poker>> lst = RoomMaJiangProxy.Instance.GetZhiDui(out isMust);
        Debug.Log(isMust);
        Debug.Log(lst.Count);
    }
    #endregion

    #region CheckXiUnitTest 检测亮喜单元测试
    /// <summary>
    /// 检测亮喜单元测试
    /// </summary>
    private void CheckXiUnitTest()
    {
        bool result = MahJongHelper.CheckLiangXi(new List<Poker>()
        {
            new Poker(3,9),
            new Poker(5,1),
            new Poker(5,2),
            new Poker(5,3),
        });

        Debug.Log(result);
    }
    #endregion

    #region MingGangUnitTest 明杠单元测试
    /// <summary>
    /// 明杠单元测试
    /// </summary>
    private void MingGangUnitTest()
    {
        //List<Poker> result = RoomMaJiangProxy.Instance.GetMingGang(new Poker(3,9));
        //Debug.Log(result.Count);
    }
    #endregion
}



public class TestNet
{
    private AccountEntity m_Account;

    private int m_SocketHandle;

    private float m_Timer;

    private bool m_isConnect;

    private const float CLOSE_TIME = 5f;


    #region GuestLogin 游客登录
    /// <summary>
    /// 游客登录
    /// </summary>
    public void GuestLogin()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrGuest, OnGuestCallBack, true, ConstDefine.HTTPFuncGuest, dic);
    }
    #endregion

    #region OnGuestCallBack 游客登陆回调
    /// <summary>
    /// 游客登陆回调
    /// </summary>
    /// <param name="args"></param>
    private void OnGuestCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            LogSystem.LogError("网络炸了");
            return;
        }
        if (args.Value.code < 0)
        {
            LogSystem.LogError(args.Value.msg);
            return;
        }
        int passportId = args.Value.data["passportId"].ToString().ToInt();
        string token = args.Value.data["token"].ToString();
        AccountLogin(passportId, token);
    }
    #endregion

    #region AccountLogin 账号登陆
    /// <summary>
    /// 账号登陆
    /// </summary>
    /// <param name="passportId"></param>
    /// <param name="token"></param>
    public void AccountLogin(int passportId, string token)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["passportId"] = passportId;
        dic["token"] = token;

        NetWorkHttp.Instance.SendData(ConstDefine.WebUrl + ConstDefine.HTTPAddrRelogin, OnLoginCallBack, true, ConstDefine.HTTPFuncRelogin, dic);
    }
    #endregion

    #region OnLoginCallBack 账号登录回调
    /// <summary>
    /// 账号登录回调
    /// </summary>
    /// <param name="args"></param>
    private void OnLoginCallBack(NetWorkHttp.CallBackArgs args)
    {
        if (args.HasError)
        {
            LogSystem.LogError("网络炸了");
            return;
        }
        if (args.Value.code < 0)
        {
            LogSystem.LogError(args.Value.msg);
            return;
        }
        m_Account = LitJson.JsonMapper.ToObject<AccountEntity>(LitJson.JsonMapper.ToJson(args.Value.data));
        m_SocketHandle = NetWorkSocket.Instance.BeginConnect("192.168.1.165", 8188, CallBack);
    }
    #endregion

    private void CallBack(bool isSuccess)
    {
        if (!isSuccess)
        {
            Debug.LogWarning("失败了卧槽" + m_SocketHandle.ToString());
            return;
        }
        m_isConnect = true;
        m_Timer = Time.time;
        OP_PLAYER_CONNECT_GET proto = new OP_PLAYER_CONNECT_GET();
        proto.passportId = m_Account.passportId;
        proto.token = m_Account.token;
        proto.latitude = LPSManager.Instance.Latitude;
        proto.longitude = LPSManager.Instance.Longitude;
        NetWorkSocket.Instance.Send(proto.encode(), OP_PLAYER_CONNECT_GET.CODE, m_SocketHandle);


        OP_ROOM_CREATE_GET proto2 = new OP_ROOM_CREATE_GET();
        proto2.addSettingId(1001);
        proto2.addSettingId(1013);
        proto2.addSettingId(1018);
        proto2.clubId = 0;
        NetWorkSocket.Instance.Send(proto2.encode(), OP_ROOM_CREATE_GET.CODE, m_SocketHandle);
    }

    public void OnUpdate()
    {
        if (m_isConnect && Time.time - m_Timer > CLOSE_TIME)
        {
            NetWorkSocket.Instance.Close(m_SocketHandle);
            m_isConnect = false;

            m_SocketHandle = NetWorkSocket.Instance.BeginConnect("192.168.1.165", 8188, CallBack);
        }
    }
}
