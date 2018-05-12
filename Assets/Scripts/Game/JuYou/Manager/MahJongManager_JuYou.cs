//===================================================
//Author      : WZQ
//CreateTime  ：8/11/2017 4:33:50 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DRBPool;
namespace JuYou
{
    public class MahJongManager_JuYou : Singleton<MahJongManager_JuYou>
    {
        /// <summary>
        /// 麻将墙池
        /// </summary>
        private SpawnPool m_WallPool;

        /// <summary>
        /// 手牌
        /// key:座位序号,value:所有手牌
        /// </summary>
        private Dictionary<int, List<MaJiangCtrl_JuYou>> m_DicHand;

        /// <summary>
        /// 牌墙
        /// </summary>
        private List<MaJiangCtrl_JuYou> m_ListWall;
        
        /// <summary>
        /// 已使用过的牌
        ///  key:座位序号,value:所有已打出的牌
        /// </summary>
        private Dictionary<int, List<MaJiangCtrl_JuYou>> m_DicTable /*= new Dictionary<int, List<MaJiangCtrl_JuYou>>()*/;

        /// <summary>
        /// 所有牌
        /// </summary>
        private List<MaJiangCtrl_JuYou> m_AllPoker;

        /// <summary>
        /// 当前抓第几张
        /// </summary>
        private int CurrentWallIndex;

        /// <summary>
        /// 麻将总数
        /// </summary>
        private int m_nMaJiangCount;
        public int MaJiangCount
        {
            get { return m_nMaJiangCount; }
        }


        private int m_OverplusCount;
        public int OverplusWallCount
        {
            get { return m_OverplusCount; }
            set
            {
                m_OverplusCount = value;
                TransferData data = new TransferData();
                data.SetValue("OverplusWallCount", m_OverplusCount);
                ModelDispatcher.Instance.Dispatch("OnOverplusWallCountChange", data);
            }
        }
        string PrefabName = "2_1";//默认加载该麻将

        public MahJongManager_JuYou()
        {
            //MahJongManager
            m_DicHand = new Dictionary<int, List<MaJiangCtrl_JuYou>>();
            m_ListWall = new List<MaJiangCtrl_JuYou>();
            m_DicTable = new Dictionary<int, List<MaJiangCtrl_JuYou>>();
            //m_DicPeng = new Dictionary<int, List<Combination3D>>();
            m_AllPoker = new List<MaJiangCtrl_JuYou>();
            //m_DicMaterial = new Dictionary<string, Material>();
        }
        public override void Dispose()
        {
            base.Dispose();
            m_DicHand.Clear();
            m_ListWall.Clear();
            m_DicTable.Clear();
            m_AllPoker.Clear();
        }
        public void LoadPrefab(Poker poker, Action<GameObject> onComplete)
        {
            string pokerName = poker == null || poker.size == 0 ? "1_1" : string.Format("{0}_{1}", poker.color, poker.size);
            string path = string.Format("download/{0}/prefab/model/{1}.drb", ConstDefine.GAME_NAME, pokerName);
            AssetBundleManager.Instance.LoadOrDownload(path, pokerName, (GameObject go) =>
            {
                if (onComplete != null)
                {
                    onComplete(go);
                }
            });
        }

        public void Init(Action onComplete)
        {
           if(m_WallPool == null) m_WallPool = PoolManager.Pools.Create("MaJiang");
            m_WallPool.Group.parent = null;
            m_WallPool.Group.position = new Vector3(0f, 5000f, 0f);


            InitPoker(1, 1, onComplete);
            //if (onComplete != null) onComplete();



        }

        private void InitPoker(int color, int size, Action onComplete)
        {
            if (size > 9)
            {
                ++color;
                size = 1;
            }
            LoadPrefab(new Poker(0, color, size), (GameObject go) =>
            {
                GameObject prefab = go;
                PrefabPool prefabPool = new PrefabPool(prefab.transform);
                if (color == 1 && size == 1)
                {
                    prefabPool.PreloadAmount = 136;
                }
                else
                {
                    prefabPool.PreloadAmount = 4;
                }
                m_WallPool.CreatePrefabPool(prefabPool);

                if ((color == 4 && size == 4) || (color == 5 && size == 3) || (color == 6 && size == 4))
                {
                    ++color;
                    size = 1;
                    InitPoker(color, size, onComplete);
                }
                else if (color == 7 && size == 4)
                {
                    //InitHandDice(onComplete);//无需加载手 骰子 直接回调
                    if (onComplete != null) onComplete();
                }
                else
                {
                    InitPoker(color, size + 1, onComplete);
                }
            });
        }



        public List<MaJiangCtrl_JuYou> Rebuild(int mahJongSum)
        {
            //MahJongManager
            m_DicHand.Clear();
            m_ListWall.Clear();
            m_DicTable.Clear();
            m_AllPoker.Clear();
            DespawnAll();

             m_nMaJiangCount = mahJongSum;
            CurrentWallIndex = 0;
            CreateWall();
            return m_ListWall;
        }

        /// <summary>
        /// 清空桌面 牌墙
        /// </summary>
        public void DespawnAll()
        {
            m_WallPool.DespawnAll();
        }

        //public void CreateWall()
        //{
        //    for (int i = 0; i < m_nMaJiangCount; ++i)
        //    {
        //        MaJiangCtrl_JuYou majiang = SpawnMaJiang(1, null, "Hand");
        //        m_ListWall.Add(majiang);
        //    }
        //}

        public void CreateWall()
        {
            for (int i = 0; i < m_nMaJiangCount; ++i)
            {
                MaJiangCtrl_JuYou majiang = SpawnMaJiang(1, null, "Hand");
                m_ListWall.Add(majiang);
            }
        }

        public MaJiangCtrl_JuYou SpawnMaJiang(int seatPos, Poker poker, string layer)
        {
           
            MaJiangCtrl_JuYou ctrl = m_WallPool.Spawn((poker == null || poker.color == 0) ? "1_1" : poker.ToString()).gameObject.GetOrCreatComponent<MaJiangCtrl_JuYou>();

            //====================麻将颜色==============================================
            //if (string.IsNullOrEmpty(m_CurrentColor))
            //{
            //    AppDebug.LogError("颜色是空的？？？");
            //    string materialName = "mj_dif";
            //    string materialPath = string.Format("download/{0}/source/modelsource/materials/{1}.drb", ConstDefine.GAME_NAME, materialName);
            //    AssetBundleManager.Instance.LoadOrDownload<Material>(materialPath, materialName, (Material go) =>
            //    {
            //        if (go != null)
            //        {
            //            ctrl.GetComponent<Renderer>().material = go;
            //        }
            //    }, 1);
            //}
            //else
            //{
            //    string materialName = string.Format("mj_{0}", m_CurrentColor);
            //    string materialPath = string.Format("download/{0}/source/modelsource/materials/{1}.drb", ConstDefine.GAME_NAME, materialName);
            //    AssetBundleManager.Instance.LoadOrDownload<Material>(materialPath, materialName, (Material go) =>
            //    {
            //        if (go != null)
            //        {
            //            ctrl.GetComponent<Renderer>().material = go;
            //        }
            //        else
            //        {
            //            string materialName1 = "mj_dif";
            //            string materialPath1 = string.Format("download/{0}/source/modelsource/materials/{1}.drb", ConstDefine.GAME_NAME, materialName1);
            //            AssetBundleManager.Instance.LoadOrDownload<Material>(materialPath1, materialName1, (Material go1) =>
            //            {
            //                if (go1 != null)
            //                {
            //                    ctrl.GetComponent<Renderer>().material = go1;
            //                }
            //            }, 1);
            //        }
            //    }, 1);
            //}
            // ============================================================

            //bool isUniversal = MahJongHelper.CheckUniversal(poker, RoomJuYouProxy.Instance.GetSeatBySeatId(seatPos).UniversalList);
            ctrl.Init(poker);
            ctrl.gameObject.SetLayer(LayerMask.NameToLayer(layer));

            m_AllPoker.Add(ctrl);
            return ctrl;
        }


        #region Clear
        public void ClearWall()
        {
            ClearPokerList(m_ListWall);
        }
        public void ClearDicTable()
        {
            for (int i = 0; i < m_DicTable.Count; i++)
            {
                ClearPokerList(m_DicTable[i]);
            }

        }
        public void ClearDicTable(int seatIndex)
        {
            if(m_DicTable.ContainsKey(seatIndex)) ClearPokerList(m_DicTable[seatIndex]);
        }


        #endregion

        #region DrawMaJiang 摸牌
        /// <summary>
        /// 摸牌
        /// </summary>
        /// <param name="fromSeatIndex">从哪摸</param>
        /// <param name="toSeatIndex">摸到哪</param>
        /// <param name="poker">摸啥牌</param>
        /// <returns></returns>
        public MaJiangCtrl_JuYou DrawMaJiang(int toSeatIndex, Poker poker)
        {
          
            if (CurrentWallIndex >= m_ListWall.Count)
            {
                CurrentWallIndex = 0;
            }

            if (m_ListWall == null || m_ListWall.Count <= CurrentWallIndex)
            {
                AppDebug.ThrowError("摸的牌墙木有牌了阿！！！！");
                return null;
            }
            MaJiangCtrl_JuYou majiang = null;
            //MaJiangCtrl majiang = null;


            majiang = m_ListWall[CurrentWallIndex++];


            MaJiangCtrl_JuYou ctrl = SpawnMaJiang(toSeatIndex, poker, toSeatIndex == 0? "PlayerHand" : "Hand");
            ctrl.transform.SetParent(majiang.transform.parent);
            ctrl.transform.position = majiang.transform.position;
            ctrl.transform.rotation = majiang.transform.rotation;
            //m_ListWall.Remove(majiang);
            m_WallPool.Despawn(majiang.transform);
            if (!m_DicHand.ContainsKey(toSeatIndex))
            {
              
                m_DicHand.Add(toSeatIndex, new List<MaJiangCtrl_JuYou>());
            }

            m_DicHand[toSeatIndex].Add(ctrl);
            --OverplusWallCount;
            return ctrl;
        }
        #endregion

        #region
        /// <summary>
        /// 丢弃手牌
        /// </summary>
        /// <param name="seatIndex"></param>
        public MaJiangCtrl_JuYou ClearHandPoker(int seatIndex, Poker poker)
        {
         
            List<MaJiangCtrl_JuYou> lst = m_DicHand[seatIndex];

            //从手牌里找牌
            MaJiangCtrl_JuYou majiang = null;
            for (int i = 0; i < lst.Count; ++i)
            {
                if (lst[i].Poker.index == poker.index)
                {

                    majiang = lst[i];
                    lst.Remove(lst[i]);
                    //lst.RemoveAt(i);
                    break;
                }
            }

            if (majiang == null)
            {
                AppDebug.ThrowError("没找到要打的牌!!!!!!" + poker.ToString());
            }

            MaJiangCtrl_JuYou ctrl = SpawnMaJiang(seatIndex, poker, "Table");
            if (majiang != null)
            {
                ctrl.transform.position = majiang.transform.position;
                ctrl.transform.rotation = majiang.transform.rotation;
                m_WallPool.Despawn(majiang.transform);//删除
            }

            //把牌放桌面上
            if (!m_DicTable.ContainsKey(seatIndex))
            {
                m_DicTable.Add(seatIndex, new List<MaJiangCtrl_JuYou>());
            }
            m_DicTable[seatIndex].Add(ctrl);
            return ctrl;

        }
        #endregion

        private void ClearPokerList(List<MaJiangCtrl_JuYou> lst)
        {
            MaJiangCtrl_JuYou majiang = null;
            for (int i = lst.Count - 1; i >= 0; --i)
            {
                majiang = lst[i];
                lst.Remove(majiang);
                if (majiang != null)
                {
                    m_WallPool.Despawn(majiang.transform);
                }
                else
                {
                    AppDebug.ThrowError("没找到要丢弃的牌!!!!!!" + lst[i].ToString());
                }
            }

        }


        #region GetHand 获取手牌
        /// <summary>
        /// 由座位Index获取手牌
        /// </summary>
        /// <param name="seatPos"></param>
        /// <returns></returns>
        public List<MaJiangCtrl_JuYou> GetHand(int seatIndex)//seatPos
        {
            if (!m_DicHand.ContainsKey(seatIndex))
            {
                //AppDebug.LogError(string.Format("没有座位{0}的牌", seatIndex));
                return null;
            }
            return m_DicHand[seatIndex];
        }
        #endregion

        #region GetDicTable 获取桌面牌
        /// <summary>
        /// 由座位Index获取桌面牌
        /// </summary>
        /// <param name="seatPos"></param>
        /// <returns></returns>
        public List<MaJiangCtrl_JuYou> GetDicTable(int seatIndex)//seatPos
        {
            if (!m_DicTable.ContainsKey(seatIndex))
            {
                //AppDebug.LogError(string.Format("没有座位{0}的牌", seatIndex));
                return null;
            }
            return m_DicTable[seatIndex];
        }
        #endregion


    }
}