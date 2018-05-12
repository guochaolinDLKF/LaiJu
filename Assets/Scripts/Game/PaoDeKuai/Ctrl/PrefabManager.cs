//===================================================
//Author      : WZQ
//CreateTime  ：11/22/2017 6:51:00 PM
//Description ： poker用SpawnPool管理  其他Item使用UIPoolManager管理
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DRBPool;


namespace PaoDeKuai
{
    public class PrefabManager : Singleton<PrefabManager>
    {
        /// <summary>
        /// 手牌
        /// key:座位序号,value:所有手牌
        /// </summary>
        private Dictionary<int, List<PokerCtrl>> m_DicHand;

        /// <summary>
        /// 牌墙
        /// </summary>
        private List<PokerCtrl> m_ListWall;


        /// <summary>
        /// 已打出的牌
        ///  key:座位序号,value:所有已打出的牌
        /// </summary>
        private Dictionary<int, List<PokerCtrl>> m_DicTable;

        /// <summary>
        /// 当前出的牌
        /// </summary>
        private List<PokerCtrl> m_CurrPlayPoker;


        /// <summary>
        /// 所有牌
        /// </summary>
        private List<PokerCtrl> m_AllPoker;

        private string m_CurrentColor;

        /// <summary>
        /// Poker总数
        /// </summary>
        private int m_nPokerCount;
        public int PokerCount
        {
            get { return m_nPokerCount; }
        }




        /// <summary>
        /// Pker墙池
        /// </summary>
        //private SpawnPool m_WallPool;

        /// <summary>
        /// Poker池
        /// </summary>
        private Dictionary<string, List<GameObject>> m_PokerPool;

        private Transform m_PokerPoolParent;


        /// <summary>
        /// 翻的牌  (可改为黑桃3)
        /// </summary>
        private PokerCtrl m_LuckPoker;

        private List<int> m_TaiLaiLuckPokerIndexList = new List<int>();

        public int GetHandCount(int seatIndex)
        {
            if (!m_DicHand.ContainsKey(seatIndex)) return 0;
            return m_DicHand[seatIndex].Count;
        }


        public PrefabManager()
        {
            m_DicHand = new Dictionary<int, List<PokerCtrl>>();
            m_ListWall = new List<PokerCtrl>();
            m_DicTable = new Dictionary<int, List<PokerCtrl>>();
            m_AllPoker = new List<PokerCtrl>();
            m_PokerPool = new Dictionary<string, List<GameObject>>();
            m_CurrPlayPoker = new List<PokerCtrl>();
        }

        public override void Dispose()
        {
            base.Dispose();
            m_DicHand.Clear();
            m_ListWall.Clear();
            m_DicTable.Clear();
            m_AllPoker.Clear();
            m_PokerPool.Clear();
            m_CurrPlayPoker.Clear();
        }



        #region LoadPrefab 加载Poker预制体
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefabName"></param>
        /// <returns></returns>
        public GameObject LoadPrefab(string prefabName)
        {
            //string pokerName = (poker == null || poker.size == 0) ? Poker.DefaultName : string.Format("{0}_{1}", poker.color, poker.size);
            string path = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, prefabName);
            GameObject go = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(path, prefabName);

            //if(go!=null) go.name = (poker == null || poker.size == 0) ? Poker.DefaultName : string.Format("{0}_{1}", poker.size, poker.color);
            return go;
        }
        #endregion

        #region Init 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="onComplete"></param>
        public void Init()
        {
            if (m_PokerPoolParent == null) m_PokerPoolParent = new GameObject("PDKPoker").transform;
            m_PokerPoolParent.parent = null;
            m_PokerPoolParent.position = new Vector3(0f, 5000f, 0f);

            //m_WallPool = PoolManager.Pools.Create("PDKPoker");
            //m_WallPool.Group.parent = null;
            //m_WallPool.Group.position = new Vector3(0f, 5000f, 0f);

            //InitPoker(0, 0, onComplete);

            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

            for (int i = 3; i < 18; ++i)
            {
                for (int j = 0; j < 5; ++j)
                {
                    if (i != 0 && j == 0) continue;
                    if (i > 15 && j != 1) continue;
                    Poker poker = new Poker(i, j);
                    GameObject go = LoadPrefabPoker(poker.ToString());
                    go.GetOrCreatComponent<PokerCtrl>().Init(poker);
                    DespawnPoker(go);
                    if (i == 0) break;
                }
            }
            stopWatch.Stop();
            Debug.Log("初始化一副牌耗时： " + stopWatch.Elapsed.TotalSeconds.ToString());

        }
        #endregion


        /// <summary>
        /// 开局初始化
        /// </summary>
        /// <param name="pokerCount"></param>
        /// <param name="playerCount"></param>
        /// <param name="bankerPos"></param>
        /// <returns></returns>
        public List<PokerCtrl> Rebuild(int pokerCount)
        {
            m_DicHand.Clear();
            m_ListWall.Clear();
            m_DicTable.Clear();
            for (int i = 0; i < m_AllPoker.Count; ++i)
            {
                m_AllPoker[i].Reset();
            }
            m_AllPoker.Clear();
            m_nPokerCount = pokerCount;

            List<PokerCtrl> tempWall = new List<PokerCtrl>();
            for (int i = 0; i < m_nPokerCount; ++i)
            {
                PokerCtrl majiang = SpawnPoker(1, null, "Hand");
                tempWall.Add(majiang);
            }


            return tempWall;
        }




        /// <summary>
        /// 排序手牌
        /// </summary>
        /// <param name="seatPos"></param>
        public void SortHandPokers(int seatPos)
        {
            List<PokerCtrl> lst = GetHand(seatPos);
            //MahJongManager

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
            PaoDeKuaiHelper.Sort(pokers);

            for (int i = 0; i < pokers.Count; ++i)
            {
                for (int j = lst.Count - 1; j >= 0; --j)
                {
                    if (lst[j].Poker == pokers[i])
                    {
                        PokerCtrl poker = lst[j];
                        lst.Remove(poker);
                        lst.Insert(i, poker);
                        break;
                    }
                }
            }




        }

        #region GetIndex 获取牌的序号
        /// <summary>
        /// 获取牌的序号
        /// </summary>
        /// <param name="seatPos"></param>
        /// <param name="majiang"></param>
        /// <returns></returns>
        public int GetIndex(int seatPos, PokerCtrl poker)
        {
            List<PokerCtrl> hand = m_DicHand[seatPos];
            for (int i = 0; i < hand.Count; ++i)
            {
                if (poker.Poker.index == hand[i].Poker.index)
                {
                    Debug.Log("摸的牌放入第" + i + "个位置");
                    return i;
                }
            }
            return 0;
        }
        #endregion



        #region DrawMaJiang 摸牌


        /// <summary>
        /// 发牌（生成牌）
        /// </summary>
        /// <param name="toSeatPos"></param>
        /// <param name="poker"></param>
        /// <param name="isPlayer"></param>
        /// <param name="isSort"></param>
        /// <returns></returns>
        public PokerCtrl DrawPoker(int toSeatPos, Poker poker, bool isPlayer, bool isSort)
        {
            if (isPlayer && poker.color == 0)
            {
                AppDebug.ThrowError("生成了一张空牌 1");
            }
            PokerCtrl ctrl = SpawnPoker(toSeatPos, poker, (isPlayer && (RoomPaoDeKuaiProxy.Instance.CurrentRoom.Status != RoomEntity.RoomStatus.Replay)) ? "PlayerHand" : "Hand");

            if (!m_DicHand.ContainsKey(toSeatPos))
            {
                m_DicHand.Add(toSeatPos, new List<PokerCtrl>());
            }
            m_DicHand[toSeatPos].Add(ctrl);

            //if (isSort)
            //{
            //    Sort(m_DicHand[toSeatPos], RoomMaJiangProxy.Instance.GetSeatBySeatId(toSeatPos).UniversalList);
            //}

            if (isPlayer && (ctrl.Poker == null || ctrl.Poker.color == 0))
            {
                AppDebug.ThrowError("生成了一张空牌 2");
            }
            return ctrl;

        }


        /// <summary>
        /// 摸牌动作（替换牌）
        /// </summary>
        /// <param name="fromSeatIndex">从哪摸</param>
        /// <param name="toSeatPos">摸到哪</param>
        /// <param name="poker">摸啥牌</param>
        /// <returns></returns>
        public PokerCtrl DrawPoker(int toSeatPos, PokerCtrl poker, bool isPlayer)
        {
            if (isPlayer && poker == null)
            {
                AppDebug.ThrowError("摸牌动作是一张空牌");
            }
            PokerCtrl pokerInWall = null;

            pokerInWall = m_ListWall[0];

            m_ListWall.Remove(pokerInWall);
            poker.transform.SetParent(pokerInWall.transform.parent);
            poker.transform.position = pokerInWall.transform.position;
            poker.transform.rotation = pokerInWall.transform.rotation;
            DespawnPoker(pokerInWall);
            return poker;
        }
        #endregion



        #region PlayPokers 出牌
        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="pokers"></param>
        /// <returns></returns>
        public List<PokerCtrl> PlayPokers(int pos, List<Poker> pokers,bool isPlayer)
        {
            if (!m_DicHand.ContainsKey(pos)) return null;

            Debug.Log("--------------------------出牌 ");
            ClearCurrPlayPoker();

            m_CurrPlayPoker.Clear();
            List<PokerCtrl> handPoker = m_DicHand[pos];

            for (int i = 0; i < pokers.Count; ++i)
            {
                for (int j = 0; j < handPoker.Count; ++j)
                {
                    if (pokers[i].index == handPoker[j].Poker.index)
                    {
                        Debug.Log(string.Format("出牌:index:{0}  size:{1} color:{2}", pokers[i].index, pokers[i].size, pokers[i].color));
                        PokerCtrl oldCtrl= handPoker[j];
                        PokerCtrl ctrl = SpawnPoker(pos, pokers[i], "Hand");
                        m_CurrPlayPoker.Add(ctrl);
                        handPoker.Remove(oldCtrl);
                        DespawnPoker(oldCtrl);
                        break;
                    }
                }
            }
            return m_CurrPlayPoker;
        }
        #endregion

        #region  清空牌
        /// <summary>
        /// 清空当前出的桌面牌
        /// </summary>
        public void ClearCurrPlayPoker()
        {
            for (int i = m_CurrPlayPoker.Count - 1; i >= 0; --i)
            {
              PokerCtrl poker=  m_CurrPlayPoker[i];
                poker.Reset();
                m_CurrPlayPoker.Remove(poker);
                DespawnPoker(poker.gameObject);
            }
        }

        /// <summary>
        /// 清空当桌面
        /// </summary>
        public void ClearTable()
        {
            ClearCurrPlayPoker();

            foreach (KeyValuePair<int, List<PokerCtrl>> item in m_DicHand)
            {
                for (int i = item.Value.Count-1; i >=0 ; --i)
                {
                    PokerCtrl poker = item.Value[i];
                    item.Value.RemoveAt(i);
                    DespawnPoker(poker.gameObject);
                }
           }



        }
        #endregion














        #region SpawnMaJiang 生成牌
        /// <summary>
        /// 生成牌
        /// </summary>
        /// <param name="seatPos">座位号</param>
        /// <param name="poker">牌</param>
        /// <param name="layer">层级</param>
        /// <returns></returns>
        public PokerCtrl SpawnPoker(int seatPos, Poker poker, string layer)
        {
            PokerCtrl ctrl = LoadPrefabPoker((poker == null || poker.color == 0) ? Poker.DefaultName : poker.ToString()).GetOrCreatComponent<PokerCtrl>();
            //ctrl.Color = m_CurrentColor;

            //SeatEntity seat = RoomMaJiangProxy.Instance.GetSeatBySeatId(seatPos);
            //bool isUniversal = false;
            //if (seat != null)
            //{
            //    isUniversal = MahJongHelper.CheckUniversal(poker, seat.UniversalList);
            //}
            ctrl.Init(poker);
            ctrl.gameObject.SetLayer(LayerMask.NameToLayer(layer));

            m_AllPoker.Add(ctrl);
            return ctrl;
        }
        #endregion


        #region LoadPokerSprite 加载牌图片
        /// <summary>
        /// 加载牌图片
        /// </summary>
        /// <param name="poker"></param>
        /// <returns></returns>
        public Sprite LoadPokerSprite(Poker poker)
        {
            string spriteName = (poker == null ||poker.size == 0) ?ConstDefine_PaoDeKuai.SpriteNameDefaultPoker:poker.ToString();
    
            string path = string.Format("download/{0}/source/uisource/gameuisource/poker.drb", ConstDefine.GAME_NAME);
            Sprite sprite = AssetBundleManager.Instance.LoadSprite(path, spriteName);
            return sprite;
        }
        #endregion



        #region DespawnPoker 回收Poker
        /// <summary>
        /// 回收Poker
        /// </summary>
        /// <param name="poker"></param>
        public void DespawnPoker(PokerCtrl poker)
        {
            Debug.Log("------回收Poker---------"+poker.Poker.ToChinese());
            poker.Reset();
            DespawnPoker(poker.gameObject);
        }
        #endregion

        #region GetHand 获取手牌
        /// <summary>
        /// 获取手牌
        /// </summary>
        /// <param name="seatPos"></param>
        /// <returns></returns>
        public List<PokerCtrl> GetHand(int seatPos)
        {
            if (!m_DicHand.ContainsKey(seatPos))
            {
                return null;
            }
            return m_DicHand[seatPos];
        }
        #endregion

        #region SpawnPoker 生产Poker游戏对象
        /// <summary>
        /// 生产Poker游戏对象
        /// </summary>
        /// <param name="ObjName"></param>
        /// <param name="ObjParent"></param>
        /// <param name="pos"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private  GameObject LoadPrefabPoker(string ObjName)
        {
            GameObject go = null;

            if (m_PokerPool.ContainsKey(ObjName) && m_PokerPool[ObjName].Count > 0)
            {
                go = m_PokerPool[ObjName][0];
                m_PokerPool[ObjName].Remove(go);

            }
            else
            {
                //加载物体
                go = LoadPrefab(ConstDefine_PaoDeKuai.UIItemNamePoker);
                go = UnityEngine.Object.Instantiate(go);
                //SeiUI
                go.name = ObjName;
                go.SetParent(m_PokerPoolParent);
            }

            return go;
        }
        #endregion

        #region DespawnPoker 回收Poker游戏对象
        /// <summary>
        /// 回收Poker游戏对象
        /// </summary>
        /// <param name="go"></param>
        /// <param name="isReset"></param>
        private void DespawnPoker(GameObject go, bool isReset = true)
        {
            if (go == null)
            {
                return;
            }

            if (isReset)
            {
                //go.transform.position = ResetPos;
                //go.transform.rotation = Quaternion.identity;
                //go.transform.localScale = Vector3.one;



                //go.transform.localPosition = Vector3.zero;
            }

            go.SetActive(false);

            string objName = go.name.Split('(')[0];

            if (m_PokerPool.ContainsKey(objName))
            {
                m_PokerPool[objName].Add(go);
            }
            else
            {
                m_PokerPool[objName] = new List<GameObject> { go };
            }

            go.SetParent(m_PokerPoolParent);
        }
        #endregion




    }
}