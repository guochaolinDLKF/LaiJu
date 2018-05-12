//===================================================
//Author      : DRB
//CreateTime  ：4/1/2017 4:09:15 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DRB.MahJong;
using DRBPool;
using UnityEngine;

public class MahJongManager : Singleton<MahJongManager>
{
    /// <summary>
    /// 手牌
    /// key:座位序号,value:所有手牌
    /// </summary>
    private Dictionary<int, List<MaJiangCtrl>> m_DicHand;

    /// <summary>
    /// 牌墙
    /// </summary>
    private List<MaJiangCtrl> m_ListWall;
    /// <summary>
    /// 反向牌墙
    /// </summary>
    private List<MaJiangCtrl> m_ListWallInverse;


    /// <summary>
    /// 已打出的牌
    ///  key:座位序号,value:所有已打出的牌
    /// </summary>
    private Dictionary<int, List<MaJiangCtrl>> m_DicTable;

    /// <summary>
    /// 碰的牌
    /// key:座位序号,value:所有碰的牌
    /// </summary>
    private Dictionary<int, List<Combination3D>> m_DicPeng;

    /// <summary>
    /// 所有牌
    /// </summary>
    private List<MaJiangCtrl> m_AllPoker;

    private string m_CurrentColor;

    /// <summary>
    /// 麻将总数
    /// </summary>
    private int m_nMaJiangCount;
    public int MaJiangCount
    {
        get { return m_nMaJiangCount; }
    }




    /// <summary>
    /// 麻将墙池
    /// </summary>
    private SpawnPool m_WallPool;
    /// <summary>
    /// 翻的牌
    /// </summary>
    private MaJiangCtrl m_LuckPoker;

    private List<int> m_TaiLaiLuckPokerIndexList = new List<int>();

    public int GetHandCount(int seatIndex)
    {
        if (!m_DicHand.ContainsKey(seatIndex)) return 0;
        return m_DicHand[seatIndex].Count;
    }


    public MahJongManager()
    {
        m_DicHand = new Dictionary<int, List<MaJiangCtrl>>();
        m_ListWall = new List<MaJiangCtrl>();
        m_ListWallInverse = new List<MaJiangCtrl>();
        m_DicTable = new Dictionary<int, List<MaJiangCtrl>>();
        m_DicPeng = new Dictionary<int, List<Combination3D>>();
        m_AllPoker = new List<MaJiangCtrl>();
    }

    public override void Dispose()
    {
        base.Dispose();
        m_DicHand.Clear();
        m_ListWall.Clear();
        m_ListWallInverse.Clear();
        m_DicTable.Clear();
        m_DicPeng.Clear();
        m_AllPoker.Clear();
    }

    #region LoadPrefab 加载预制体
    /// <summary>
    /// 加载预制体
    /// </summary>
    /// <param name="poker"></param>
    /// <returns></returns>
    public GameObject LoadPrefab(Poker poker)
    {
        string pokerName = (poker == null || poker.size == 0) ? Poker.DefaultName : string.Format("{0}_{1}", poker.color, poker.size);
        string path = string.Format("download/{0}/prefab/model/{1}.drb", ConstDefine.GAME_NAME, pokerName);
        return AssetBundleManager.Instance.LoadAssetBundle<GameObject>(path, pokerName);
    }
    #endregion

    #region Init 初始化
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="onComplete"></param>
    public void Init()
    {
        m_WallPool = PoolManager.Pools.Create("MaJiang");
        m_WallPool.Group.parent = null;
        m_WallPool.Group.position = new Vector3(0f, 5000f, 0f);

        //InitPoker(0, 0, onComplete);

        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 10; ++j)
            {
                if (i != 0 && j == 0) continue;
                if ((i == 4 || i == 6 || i == 7) && j > 4) break;
                if (i == 5 && j > 3) break;
                GameObject prefab = LoadPrefab(new Poker(i, j));
                PrefabPool prefabPool = new PrefabPool(prefab.transform);
                if (i == 0 && j == 0)
                {
                    prefabPool.PreloadAmount = 136;
                }
                else
                {
                    prefabPool.PreloadAmount = 5;
                }
                m_WallPool.CreatePrefabPool(prefabPool);
                if (i == 0) break;
            }
        }
    }
    #endregion

    #region Rebuild 初始化
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="majiangCount">麻将总数</param>
    /// <param name="diceSeatPos2">第二个摇骰子的座位号</param>
    /// <param name="firstDice">第一次骰子</param>
    /// <param name="secondDice">第二次骰子</param>
    /// <param name="playerCount">玩家数量</param>
    /// <param name="bankerPos">庄家座位号</param>
    /// <returns></returns>
    public List<MaJiangCtrl> Rebuild(int majiangCount, int diceSeatPos2, int firstDice, int secondDice, int playerCount, int bankerPos)
    {
        m_DicHand.Clear();
        m_ListWall.Clear();
        m_ListWallInverse.Clear();
        m_DicTable.Clear();
        m_DicPeng.Clear();
        for (int i = 0; i < m_AllPoker.Count; ++i)
        {
            m_AllPoker[i].Reset();
        }
        m_AllPoker.Clear();

        m_WallPool.DespawnAll();

        m_nMaJiangCount = majiangCount;

        m_TaiLaiLuckPokerIndexList.Clear();


        int pos = bankerPos;
        for (int i = 1; i < firstDice + secondDice; ++i)
        {
            --pos;
            if (pos < 1)
            {
                pos += 4;
            }
        }
        int fromWallIndex = (secondDice < firstDice ? secondDice : firstDice) * 2 + (int)((pos - 1) * (majiangCount / (float)playerCount % 2 == 1 ? majiangCount / (float)playerCount - 1f : majiangCount / (float)playerCount));
        if (fromWallIndex % 2 == 1)
        {
            fromWallIndex += 1;
        }
        AppDebug.Log(string.Format("从第{0}张开始摸牌", fromWallIndex));

        List<MaJiangCtrl> tempWall = new List<MaJiangCtrl>();
        for (int i = 0; i < m_nMaJiangCount; ++i)
        {
            MaJiangCtrl majiang = SpawnMaJiang(1, null, "Hand");
            tempWall.Add(majiang);
        }

        for (int i = fromWallIndex; i < tempWall.Count; ++i)
        {
            m_ListWall.Add(tempWall[i]);
        }
        for (int i = 0; i < fromWallIndex; ++i)
        {
            m_ListWall.Add(tempWall[i]);
        }

        for (int i = m_ListWall.Count - 2; i >= 0; i -= 2)
        {
            m_ListWallInverse.Add(m_ListWall[i]);
            m_ListWallInverse.Add(m_ListWall[i + 1]);
        }
        return tempWall;
    }
    #endregion

    #region SpawnMaJiang 生成麻将
    /// <summary>
    /// 生成麻将
    /// </summary>
    /// <param name="seatPos">座位号</param>
    /// <param name="poker">牌</param>
    /// <param name="layer">层级</param>
    /// <returns></returns>
    public MaJiangCtrl SpawnMaJiang(int seatPos, Poker poker, string layer)
    {
        MaJiangCtrl ctrl = m_WallPool.Spawn((poker == null || poker.color == 0) ? Poker.DefaultName : poker.ToString()).gameObject.GetOrCreatComponent<MaJiangCtrl>();
        ctrl.Color = m_CurrentColor;

        SeatEntity seat = RoomMaJiangProxy.Instance.GetSeatBySeatId(seatPos);
        bool isUniversal = false;
        if (seat != null)
        {
            isUniversal = MahJongHelper.CheckUniversal(poker, seat.UniversalList);
        }
        ctrl.Init(poker, isUniversal);
        ctrl.gameObject.SetLayer(LayerMask.NameToLayer(layer));

        m_AllPoker.Add(ctrl);
        return ctrl;
    }
    #endregion

    #region SpawnDice 生成骰子
    /// <summary>
    /// 生成骰子
    /// </summary>
    /// <returns></returns>
    public GameObject SpawnDice()
    {
        string prefabName = "dice";
        string path = string.Format("download/{0}/prefab/model/{1}.drb", ConstDefine.GAME_NAME, prefabName);
        GameObject prefab = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(path, prefabName);
        return UnityEngine.Object.Instantiate(prefab);
    }
    #endregion

    #region SpawnHand_Tui 生成手（推牌）
    /// <summary>
    /// 生成手（推牌）
    /// </summary>
    /// <returns></returns>
    public Transform SpawnHand_Tui()
    {
        return m_WallPool.Spawn("hand");
    }
    #endregion

    #region SpawnHand_Fang 生成手（放牌）
    /// <summary>
    /// 生成手（放牌）
    /// </summary>
    /// <returns></returns>
    public GameObject SpawnHand_Fang()
    {
        string handPrefabName = "dicehand";
        string handPath = string.Format("download/{0}/prefab/model/{1}.drb", ConstDefine.GAME_NAME, handPrefabName);
        GameObject prefab = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(handPath, handPrefabName);
        return UnityEngine.Object.Instantiate(prefab);
    }
    #endregion


    #region HoldLuckPoker 乐平翻宝
    /// <summary>
    /// 乐平翻宝
    /// </summary>
    /// <param name="poker"></param>
    /// <returns></returns>
    public MaJiangCtrl HoldLuckPoker(Poker poker)
    {
        MaJiangCtrl ctrl = m_ListWallInverse[34];
        m_ListWall.Remove(ctrl);
        m_ListWallInverse.Remove(ctrl);

        m_LuckPoker = SpawnMaJiang(1, poker, "Hand");
        m_LuckPoker.Init(poker, false);
        m_LuckPoker.transform.position = ctrl.transform.position;
        m_LuckPoker.transform.rotation = ctrl.transform.rotation;
        m_LuckPoker.transform.localScale = ctrl.transform.lossyScale;
        DespawnMaJiang(ctrl);
        return m_LuckPoker;
    }
    #endregion

    #region HoldLuckPoker_TaiLai 泰来翻宝
    /// <summary>
    /// 泰来翻宝
    /// </summary>
    /// <param name="poker"></param>
    /// <param name="dice"></param>
    /// <returns></returns>
    public MaJiangCtrl HoldLuckPoker_TaiLai(Poker poker, int dice)
    {
        int index = dice * 2 - 2;
        MaJiangCtrl ctrl = m_ListWallInverse[index];
        while (m_TaiLaiLuckPokerIndexList.Contains(index))
        {
            ++index;
            ctrl = m_ListWallInverse[index];
        }

        m_TaiLaiLuckPokerIndexList.Add(index);

        m_LuckPoker = SpawnMaJiang(1, poker, "Hand");
        m_LuckPoker.Init(poker, false);
        m_LuckPoker.transform.position = ctrl.transform.position;
        m_LuckPoker.transform.rotation = ctrl.transform.rotation;
        m_LuckPoker.transform.localScale = ctrl.transform.lossyScale;
        DespawnMaJiang(ctrl);
        return m_LuckPoker;
    }
    #endregion

    #region HoldLuckPoker_GuGeng 古耿翻宝
    /// <summary>
    /// 古耿翻宝
    /// </summary>
    /// <param name="poker"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public MaJiangCtrl HoldLuckPoker_GuGeng(Poker poker, int index)
    {
        m_LuckPoker = GetWall(poker, index, true);
        return m_LuckPoker;
    }
    #endregion

    #region GetWallLastMahJong 获取墙最后一张麻将
    /// <summary>
    /// 获取墙最后一张麻将
    /// </summary>
    /// <param name="poker"></param>
    /// <returns></returns>
    public MaJiangCtrl GetWallLastMahJong(Poker poker)
    {
        return GetWall(poker, 0, true);
    }
    #endregion

    #region GetWallFirstMahJong 获取墙第一张麻将
    /// <summary>
    /// 获取墙第一张麻将
    /// </summary>
    /// <param name="poker"></param>
    /// <returns></returns>
    public MaJiangCtrl GetWallFirstMahJong(Poker poker)
    {
        return GetWall(poker, 0, false);
    }
    #endregion

    public MaJiangCtrl GetWall(Poker poker, int index, bool isLast)
    {
        List<MaJiangCtrl> wall = isLast ? m_ListWallInverse : m_ListWall;
        MaJiangCtrl majiang = wall[index];
        m_ListWall.Remove(majiang);
        m_ListWallInverse.Remove(majiang);

        MaJiangCtrl ret = SpawnMaJiang(1, poker, "Hand");
        ret.Init(poker, false);
        ret.transform.position = majiang.transform.position;
        ret.transform.rotation = majiang.transform.rotation;
        ret.transform.localScale = majiang.transform.lossyScale;

        DespawnMaJiang(majiang);

        return ret;
    }

    #region DespawnMaJiang 回收牌
    /// <summary>
    /// 回收牌
    /// </summary>
    /// <param name="majiang"></param>
    public void DespawnMaJiang(MaJiangCtrl majiang)
    {
        majiang.Reset();
        m_WallPool.Despawn(majiang.transform);
    }
    #endregion

    #region LoadPokerSprite 加载牌图片
    /// <summary>
    /// 加载牌图片
    /// </summary>
    /// <param name="poker"></param>
    /// <param name="isPeng"></param>
    /// <returns></returns>
    public Sprite LoadPokerSprite(Poker poker, bool isPeng = false)
    {
        string spriteName = string.Empty;
        if (poker == null || poker.color == 0)
        {
            spriteName = "0_b";
        }
        else
        {
            spriteName = "0_" + poker.ToString() + (isPeng ? "_t" : "");
        }

        string path = string.Format("download/{0}/source/uisource/gameuisource/majiang.drb", ConstDefine.GAME_NAME);

        Sprite sprite = AssetBundleManager.Instance.LoadSprite(path, spriteName);
        return sprite;
    }
    #endregion

    #region DrawMaJiang 摸牌
    /// <summary>
    /// 摸牌
    /// </summary>
    /// <param name="fromSeatIndex">从哪摸</param>
    /// <param name="toSeatPos">摸到哪</param>
    /// <param name="poker">摸啥牌</param>
    /// <returns></returns>
    public MaJiangCtrl DrawMaJiang(int toSeatPos, Poker poker, bool isPlayer, bool isLast, bool isSort)
    {
        if (isPlayer && poker.color == 0)
        {
            AppDebug.ThrowError("生成了一张空牌 1:" + poker.ToLog());
        }
        MaJiangCtrl majiang = null;
        if (isLast)
        {
#if IS_LEPING
            MaJiangCtrl temp = m_ListWall[34];
            if (m_LuckPoker != null)
            {
                m_LuckPoker.transform.SetParent(temp.transform.parent);
                m_LuckPoker.transform.position = temp.transform.position;
                m_LuckPoker.transform.localEulerAngles = temp.transform.localEulerAngles + new Vector3(0f, 0f, 180f);
                m_LuckPoker.transform.localScale = temp.transform.localScale;
            }
            m_ListWall.Remove(temp);
            m_ListWallInverse.Remove(temp);
            DespawnMaJiang(temp);
            majiang = m_ListWall[34];
#elif IS_TAILAI
            majiang = m_ListWallInverse[12];
#else
            majiang = m_ListWallInverse[0];
#endif

        }
        else
        {
            majiang = m_ListWall[0];
        }
        m_ListWall.Remove(majiang);
        m_ListWallInverse.Remove(majiang);
        MaJiangCtrl ctrl = SpawnMaJiang(toSeatPos, poker, ((isPlayer && RoomMaJiangProxy.Instance.CurrentRoom.isReplay) ? "PlayerHand" : "Hand"));
        ctrl.transform.SetParent(majiang.transform.parent);
        ctrl.transform.position = majiang.transform.position;
        ctrl.transform.rotation = majiang.transform.rotation;
        DespawnMaJiang(majiang);
        if (!m_DicHand.ContainsKey(toSeatPos))
        {
            m_DicHand.Add(toSeatPos, new List<MaJiangCtrl>());
        }

        m_DicHand[toSeatPos].Add(ctrl);

        SeatEntity seat = RoomMaJiangProxy.Instance.GetSeatBySeatId(toSeatPos);
        if (seat != null)
        {
            if (seat.LackColor != 0)
            {
                if (ctrl.Poker.color == seat.LackColor)
                {
                    ctrl.isGray = true;
                }
            }
        }

        if (isSort)
        {
            Sort(m_DicHand[toSeatPos], RoomMaJiangProxy.Instance.GetSeatBySeatId(toSeatPos).UniversalList);
        }

        if (isPlayer && (ctrl.Poker == null || ctrl.Poker.color == 0))
        {
            AppDebug.ThrowError("生成了一张空牌 2:" + poker.ToLog());
        }
        return ctrl;
    }
    #endregion

    #region PlayMaJiang 打牌
    /// <summary>
    /// 打牌
    /// </summary>
    /// <returns></returns>
    public MaJiangCtrl PlayMaJiang(int seatPos, Poker poker, bool isReplay = false)
    {
        //从手牌里找牌
        MaJiangCtrl majiang = null;
        if (m_DicHand.ContainsKey(seatPos))
        {
            List<MaJiangCtrl> lst = m_DicHand[seatPos];

            if (poker.color == 0)
            {
                AppDebug.ThrowError("生成了一张空牌 3:" + poker.ToLog());
            }


            for (int i = 0; i < lst.Count; ++i)
            {
                if (lst[i].Poker.index == poker.index)
                {

                    majiang = lst[i];
                    lst.RemoveAt(i);
                    break;
                }
            }
            Sort(lst, RoomMaJiangProxy.Instance.GetSeatBySeatId(seatPos).UniversalList);
        }

        if (majiang == null)
        {
            string str = string.Format("没找到要打的牌!!!!!!{0}", poker.ToString("{0}_{1}_{2}_{3}"));
            AppDebug.ThrowError(str);
        }

        MaJiangCtrl ctrl = SpawnMaJiang(seatPos, poker, "Table");
        if (majiang != null)
        {
            ctrl.transform.position = majiang.transform.position;
            ctrl.transform.rotation = majiang.transform.rotation;
            DespawnMaJiang(majiang);
        }

        if (!m_DicTable.ContainsKey(seatPos))
        {
            m_DicTable.Add(seatPos, new List<MaJiangCtrl>());
        }
        m_DicTable[seatPos].Add(ctrl);

        if (ctrl.Poker == null || ctrl.Poker.color == 0)
        {
            AppDebug.ThrowError("生成了一张空牌");
        }
        return ctrl;
    }
    #endregion

    #region Sort 手牌排序
    /// <summary>
    /// 手牌排序
    /// </summary>
    /// <param name="lst"></param>
    public void Sort(List<MaJiangCtrl> lst, List<Poker> universal)
    {
        if (lst == null || lst.Count == 0 || lst[0].Poker.size == 0) return;

        string temp = "排序后的手牌为：";
        for (int i = 0; i < lst.Count; ++i)
        {
            temp += lst[i].Poker.ToString() + "  ";
        }

        List<Poker> pokers = new List<Poker>();
        for (int i = 0; i < lst.Count; ++i)
        {
            pokers.Add(lst[i].Poker);
        }
        MahJongHelper.Sort(pokers, universal, RoomMaJiangProxy.Instance.Rule.UniversalSortType);

        for (int i = 0; i < pokers.Count; ++i)
        {
            for (int j = lst.Count - 1; j >= 0; --j)
            {
                if (lst[j].Poker == pokers[i])
                {
                    MaJiangCtrl majiang = lst[j];
                    lst.Remove(majiang);
                    lst.Insert(i, majiang);
                    break;
                }
            }
        }
#if DEBUG_LOG
        string log = "排序后的手牌为：";
        for (int i = 0; i < lst.Count; ++i)
        {
            log += lst[i].Poker.ToString() + "  ";
        }
        Debug.Log(log);
#endif
    }
    #endregion

    #region GetIndex 获取牌的序号
    /// <summary>
    /// 获取牌的序号
    /// </summary>
    /// <param name="seatIndex"></param>
    /// <param name="majiang"></param>
    /// <returns></returns>
    public int GetIndex(int seatIndex, MaJiangCtrl majiang)
    {
        List<MaJiangCtrl> hand = m_DicHand[seatIndex];
        for (int i = 0; i < hand.Count; ++i)
        {
            if (majiang.Poker.index == hand[i].Poker.index)
            {
                Debug.Log("摸的牌放入第" + i + "个位置");
                return i;
            }
        }
        return 0;
    }
    #endregion

    #region GetHand 获取手牌
    /// <summary>
    /// 获取手牌
    /// </summary>
    /// <param name="seatPos"></param>
    /// <returns></returns>
    public List<MaJiangCtrl> GetHand(int seatPos)
    {
        if (!m_DicHand.ContainsKey(seatPos))
        {
            return null;
        }
        return m_DicHand[seatPos];
    }
    #endregion

    public MaJiangCtrl GetPoker(int index)
    {
        for (int i = 0; i < m_AllPoker.Count; ++i)
        {
            if (m_AllPoker[i].Poker == null) continue;
            if (m_AllPoker[i].Poker.index == index)
            {
                return m_AllPoker[i];
            }
        }
        return null;
    }

    #region Operate 吃碰杠
    /// <summary>
    /// 吃碰杠
    /// </summary>
    public Combination3D Operate(int seatPos, OperatorType operateId, int subTypeId, List<Poker> pokers)
    {
        List<MaJiangCtrl> lst = new List<MaJiangCtrl>();

        Combination3D combination = null;
        if (operateId == OperatorType.Gang)
        {
            if (m_DicPeng.ContainsKey(seatPos))
            {
                List<Combination3D> usedList = m_DicPeng[seatPos];
                for (int i = 0; i < usedList.Count; ++i)
                {
                    if (usedList[i].OperatorType == OperatorType.Peng || usedList[i].OperatorType == OperatorType.Kou)
                    {
                        for (int j = 0; j < pokers.Count; ++j)
                        {
                            if (usedList[i].PokerList[0].Poker.index == pokers[j].index)
                            {
                                combination = usedList[i];
                                for (int k = 0; k < combination.PokerList.Count; ++k)
                                {
                                    lst.Add(combination.PokerList[k]);
                                }
                                usedList.Remove(combination);
                                break;
                            }
                        }
                    }
                }
            }
        }


        List<MaJiangCtrl> handList = m_DicHand[seatPos];
        Dictionary<int, List<MaJiangCtrl>> deskDic = m_DicTable;
        for (int i = 0; i < pokers.Count; ++i)
        {
            foreach (var pair in deskDic)
            {
                for (int j = 0; j < pair.Value.Count; ++j)
                {
                    if (pair.Value[j].Poker.index == pokers[i].index)
                    {
                        lst.Add(pair.Value[j]);
                        pair.Value.RemoveAt(j);
                        break;
                    }
                }
            }

            foreach (var pair in m_DicHand)
            {
                if (pair.Value == null) continue;
                for (int j = 0; j < pair.Value.Count; ++j)
                {
                    if (pair.Value[j].Poker.index == pokers[i].index)
                    {
                        lst.Add(pair.Value[j]);
                        pair.Value.RemoveAt(j);
                        break;
                    }
                }
            }

        }
        if (operateId != OperatorType.Gang)
        {
            Sort(handList, RoomMaJiangProxy.Instance.GetSeatBySeatId(seatPos).UniversalList);
        }

        if (!m_DicPeng.ContainsKey(seatPos))
        {
            m_DicPeng.Add(seatPos, new List<Combination3D>());
        }

        if (lst.Count != pokers.Count)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("客户端找到牌的数量:{0},有", lst.Count);
            for (int i = 0; i < lst.Count; ++i)
            {
                sb.Append(lst[i].Poker.ToString(true, true));
                sb.Append(" ");
            }
            sb.AppendFormat(",服务器的数量:{0},有", pokers.Count);
            for (int i = 0; i < pokers.Count; ++i)
            {
                sb.Append(pokers[i].ToString(true, true));
                sb.Append(" ");
            }
            sb.AppendLine();
            sb.Append("客户端所有的手牌数据:");
            SeatEntity seat = RoomMaJiangProxy.Instance.GetSeatBySeatId(seatPos);
            if (seat != null)
            {
                for (int i = 0; i < seat.PokerList.Count; ++i)
                {
                    sb.Append(seat.PokerList[i].ToString(true, true));
                    sb.Append(" ");
                }
                sb.AppendLine();
                sb.Append("客户端所有的模型:");
            }
            for (int i = 0; i < handList.Count; ++i)
            {
                sb.Append(handList[i].Poker.ToString(true, true));
                sb.Append(" ");
            }
            AppDebug.ThrowError(sb.ToString());
        }

        List<MaJiangCtrl> newList = new List<MaJiangCtrl>();
        for (int i = 0; i < pokers.Count; ++i)
        {
            MaJiangCtrl ctrl = SpawnMaJiang(seatPos, pokers[i], "Table");
            if (pokers[i].pos != seatPos && (operateId == OperatorType.Chi || operateId == OperatorType.ChiTing))
            {
                ctrl.isGray = true;
            }

            if (operateId == OperatorType.LiangXi)
            {
                ctrl.isGray = true;
            }

            if (lst.Count == pokers.Count)
            {
                ctrl.transform.position = lst[i].transform.position;
                ctrl.transform.rotation = lst[i].transform.rotation;
            }
            newList.Add(ctrl);
        }

        if (combination == null)
        {
            combination = new Combination3D((int)operateId, subTypeId, newList);
        }
        else
        {
            combination.BuGang(newList);
        }

        for (int i = 0; i < lst.Count; ++i)
        {
            DespawnMaJiang(lst[i]);
        }
        m_DicPeng[seatPos].Add(combination);

        return combination;
    }
    #endregion

    #region Hu 胡牌
    /// <summary>
    /// 胡牌
    /// </summary>
    /// <param name="seatPos"></param>
    /// <param name="subTypeId"></param>
    /// <param name="hitPoker"></param>
    /// <param name="isPlayer"></param>
    /// <returns></returns>
    public MaJiangCtrl Hu(int seatPos, int subTypeId, Poker hitPoker, bool isPlayer)
    {
        MaJiangCtrl ctrl = null;
        List<MaJiangCtrl> hand = m_DicHand[seatPos];
        if (hitPoker != null)
        {
            MaJiangCtrl oldCtrl = null;
            for (int i = 0; i < hand.Count; ++i)
            {
                if (hand[i].Poker.index == hitPoker.index)
                {
                    oldCtrl = hand[i];
                    hand.RemoveAt(i);
                }
            }
            if (oldCtrl == null)
            {
                foreach (var pair in m_DicTable)
                {
                    bool isBreak = false;
                    if (pair.Value != null)
                    {
                        for (int i = 0; i < pair.Value.Count; ++i)
                        {
                            if (pair.Value[i].Poker.index == hitPoker.index)
                            {
                                Debug.Log("找到了牌");
                                oldCtrl = pair.Value[i];
                                pair.Value.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    if (isBreak) break;
                }
            }
#if IS_GONGXIAN
            if (oldCtrl == null)
            {
                foreach (var pair in m_DicPeng)
                {
                    if (pair.Value != null)
                    {
                        for (int i = 0; i < pair.Value.Count; ++i)
                        {
                            for (int j = 0; j < pair.Value[i].PokerList.Count; ++j)
                            {
                                if (pair.Value[i].PokerList[j].Poker.index == hitPoker.index)
                                {
                                    if (pair.Value[i].OperatorType == OperatorType.Gang)
                                    {
                                        pair.Value[i].OperatorType = OperatorType.Peng;
                                    }
                                    Debug.Log("找到了牌");
                                    oldCtrl = pair.Value[i].PokerList[j];
                                    pair.Value[i].PokerList.RemoveAt(j);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
#endif

            ctrl = SpawnMaJiang(seatPos, hitPoker, isPlayer ? "PlayerHand" : "Hand");
            if (oldCtrl != null)
            {
                DespawnMaJiang(oldCtrl);
            }
            hand.Add(ctrl);
        }
        return ctrl;
    }
    #endregion

    #region DingJiang 定将
    /// <summary>
    /// 定将
    /// </summary>
    /// <param name="seatPos"></param>
    /// <param name="pokers"></param>
    /// <returns></returns>
    public List<MaJiangCtrl> DingJiang(int seatPos, List<Poker> pokers)
    {
        List<MaJiangCtrl> lst = new List<MaJiangCtrl>();

        List<MaJiangCtrl> handList = m_DicHand[seatPos];
        Dictionary<int, List<MaJiangCtrl>> deskDic = m_DicTable;
        for (int i = 0; i < pokers.Count; ++i)
        {
            foreach (var pair in deskDic)
            {
                for (int j = 0; j < pair.Value.Count; ++j)
                {
                    if (pair.Value[j].Poker.index == pokers[i].index)
                    {
                        lst.Add(pair.Value[j]);
                        pair.Value.RemoveAt(j);
                        break;
                    }
                }
            }

            for (int j = 0; j < handList.Count; ++j)
            {
                if (handList[j].Poker.index == pokers[i].index)
                {
                    if (!lst.Contains(handList[j]))
                    {
                        lst.Add(handList[j]);
                    }
                    handList.RemoveAt(j);
                    break;
                }
            }
        }
        Sort(handList, RoomMaJiangProxy.Instance.GetSeatBySeatId(seatPos).UniversalList);

        List<MaJiangCtrl> newList = new List<MaJiangCtrl>();
        for (int i = 0; i < pokers.Count; ++i)
        {
            MaJiangCtrl ctrl = SpawnMaJiang(seatPos, pokers[i], "Table");

            if (lst.Count == pokers.Count)
            {
                ctrl.transform.position = lst[i].transform.position;
                ctrl.transform.rotation = lst[i].transform.rotation;
            }
            newList.Add(ctrl);
            handList.Add(ctrl);
        }

        for (int i = 0; i < lst.Count; ++i)
        {
            DespawnMaJiang(lst[i]);
        }

        return newList;
    }
    #endregion

    #region BuXi 补喜
    /// <summary>
    /// 补喜
    /// </summary>
    /// <param name="seatPos"></param>
    /// <param name="poker"></param>
    /// <returns></returns>
    public MaJiangCtrl BuXi(int seatPos, Poker poker)
    {
        List<MaJiangCtrl> handList = m_DicHand[seatPos];
        for (int i = 0; i < handList.Count; ++i)
        {
            if (handList[i].Poker.index == poker.index)
            {
                return handList[i];
            }
        }
        return null;
    }
    #endregion

    #region SetPokerColor 设置牌颜色
    /// <summary>
    /// 设置牌颜色
    /// </summary>
    /// <param name="color"></param>
    public void SetPokerColor(string color)
    {
        m_CurrentColor = color;
        for (int i = 0; i < m_AllPoker.Count; ++i)
        {
            if (m_AllPoker[i] == null) continue;
            m_AllPoker[i].Color = color;
        }
    }
    #endregion

    #region GetUsedPoker 获取吃碰杠组合
    /// <summary>
    /// 获取吃碰杠组合
    /// </summary>
    /// <param name="ctrl">其中一张牌</param>
    /// <returns></returns>
    public List<MaJiangCtrl> GetUsedPoker(MaJiangCtrl ctrl)
    {
        foreach (var pair in m_DicPeng)
        {
            if (pair.Value != null)
            {
                for (int i = 0; i < pair.Value.Count; ++i)
                {
                    if (pair.Value[i].PokerList.Contains(ctrl))
                    {
                        return pair.Value[i].PokerList;
                    }
                }
            }
        }
        return null;
    }
    #endregion
}
