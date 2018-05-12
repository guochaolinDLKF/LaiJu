//===================================================
//Author      : WZQ
//CreateTime  ：7/7/2017 6:10:10 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaiJiu
{
    public class MahJongManager_PaiJiu : /*MonoBehaviour*/Singleton<MahJongManager_PaiJiu>
    {

        //[SerializeField]
        //private Transform m_PoolMaJiangContainer;//存储关闭的麻将

        string PrefabName = "2_1";//默认加载该麻将


        /// <summary>
        /// 手牌
        /// key:座位序号,value:所有手牌
        /// </summary>
        private Dictionary<int, List<MaJiangCtrl_PaiJiu>> m_DicHand=new Dictionary<int, List<MaJiangCtrl_PaiJiu>> ();

        /// <summary>
        /// 牌墙
        /// </summary>
        private List<MaJiangCtrl_PaiJiu> m_ListWall=new List<MaJiangCtrl_PaiJiu> ();

        /// <summary>
        /// 已使用过的牌
        ///  key:座位序号,value:所有已打出的牌
        /// </summary>
        private Dictionary<int, List<MaJiangCtrl_PaiJiu>> m_DicTable=new Dictionary<int, List<MaJiangCtrl_PaiJiu>> ();


        /// <summary>
        /// 麻将总数
        /// </summary>
        private int m_nMaJiangCount;
        public int MaJiangCount
        {
            get { return m_nMaJiangCount; }
        }


        //private static MahJongManager_PaiJiu instance;
        //public static MahJongManager_PaiJiu Instance { get { return instance; } }
        //void Awake()
        //{
        //    instance = this;
        //}


        public void Init(Action onComplete)
        {
            m_DicHand.Clear();
            m_ListWall.Clear();
            m_DicTable.Clear();
            if (onComplete != null) onComplete();

            //m_WallPool = PoolManager.Pools.Create("MaJiang");
            //m_WallPool.Group.parent = null;
            //m_WallPool.Group.position = new Vector3(0f, 5000f, 0f);

            //InitPoker(1, 1, onComplete);
        }



        //加载麻将预制体  
        public  GameObject LoadMaJiang(string ObjName,Transform parent=null)
        {

            //设置加载路径
            string prefabPath = string.Format(ConstDefine_PaiJiu.MaJiangPrefabPath, ConstDefine.GAME_NAME, ObjName);
           GameObject go=   Pool_PaiJiu.Instance.GetObjectFromPool(ObjName, prefabPath, parent);

            //设置信息
            if (go == null) return null;
            return go;

        }

        



        //存入麻将
        public void  PushMaJiangToPool(GameObject go)
        {
            Pool_PaiJiu.Instance.PushToPool(go, null, true);
        }

        /// <summary>
        /// 初始化 //增加:   改为创建墙 sceneCtrl 初始牌墙调用此方法
        /// </summary>
        /// <param name="majiangCount">牌墙长度</param>
        /// <param name="diceSeatPos2">骰子位置</param>
        /// <param name="firstDice">骰子值</param>
        /// <param name="secondDice">骰子值</param>
        /// <param name="playerCount">玩家长度</param>
        /// <param name="bankerPos">庄位置</param>
        /// <returns></returns>
        public List<MaJiangCtrl_PaiJiu> Rebuild(int majiangCount/*, int diceSeatPos2, int firstDice, int secondDice, int playerCount, int bankerPos*/)
        {
            m_DicHand.Clear();
            m_ListWall.Clear();
            m_DicTable.Clear();

            m_nMaJiangCount = majiangCount;


            CreateWall();
            return m_ListWall;

        }

        public void CreateWall()
        {
            for (int i = 0; i < m_nMaJiangCount; ++i)
            {
                MaJiangCtrl_PaiJiu majiang = SpawnMaJiang(1, null, "Table");
                m_ListWall.Add(majiang);
            }
        }


        /// <summary>
        ///  加载麻将
        /// </summary>
        /// <param name="seatPos"></param>
        /// <param name="poker"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public MaJiangCtrl_PaiJiu SpawnMaJiang(int seatPos, Poker poker, string layer)
        {
            //MaJiangCtrl_PaiJiu ctrl = m_WallPool.Spawn((poker == null || poker.color == 0) ? "1_1" : poker.ToString()).gameObject.GetOrCreatComponent<MaJiangCtrl>();

           
            MaJiangCtrl_PaiJiu ctrl = LoadMaJiang((poker == null || poker.type == 0) ? PrefabName : poker.ToString()).GetOrCreatComponent<MaJiangCtrl_PaiJiu>();
            string materialName = "mj_green";
            string materialPath = string.Format("download/{0}/source/modelsource/materials/{1}.drb", ConstDefine.GAME_NAME, materialName);
            AssetBundleManager.Instance.LoadOrDownload<Material>(materialPath, materialName, (Material go) =>
            {
                ctrl.GetComponentInChildren<Renderer>().material = go;
            }, 1);
            //bool isUniversal = MahJongHelper.CheckUniversal(poker, RoomMaJiangProxy.Instance.GetSeatBySeatId(seatPos).UniversalList);

            ctrl.Init(poker);
            ctrl.gameObject.SetLayer(LayerMask.NameToLayer(layer));
            return ctrl;
        }



        //摸牌
        public MaJiangCtrl_PaiJiu DrawMaJiang(int toSeatPos, Poker poker/*, bool isInitPoker, bool isLast = false*/)
        {
           
            if (m_ListWall == null || m_ListWall.Count <= 0)
            {
                AppDebug.ThrowError("摸的牌墙木有牌了阿！！！！");
                return null;
            }
            //MaJiangCtrl_PaiJiu majiang = null;


            MaJiangCtrl_PaiJiu ctrl = SpawnMaJiang(toSeatPos, poker, "Hand");
            //ctrl.transform.SetParent(majiang.transform.parent);
            //ctrl.transform.position = majiang.transform.position;
            //ctrl.transform.rotation = majiang.transform.rotation;
            //m_WallPool.Despawn(majiang.transform);
            if (!m_DicHand.ContainsKey(toSeatPos))
            {
                m_DicHand.Add(toSeatPos, new List<MaJiangCtrl_PaiJiu>());
            }

            m_DicHand[toSeatPos].Add(ctrl);
            //--OverplusWallCount;
            //m_ListWall[0].transform.GetComponent<MeshRenderer>().enabled = false;
            PushMaJiangToPool(m_ListWall[0].gameObject);
            m_ListWall.RemoveAt(0);
            return ctrl;
        }



        public List<MaJiangCtrl_PaiJiu> DrawMaJiangWall(List<Poker> pokermWall)
        {

            for (int i = 0; i < pokermWall.Count; i++)
            {
                if (i > m_ListWall.Count - 1 ) break;

                MaJiangCtrl_PaiJiu ctrl = SpawnMaJiang(1, pokermWall[i], "Table");

                GameObject majiang = m_ListWall[i].gameObject;
                ctrl.transform.SetParent(majiang.transform.parent);
                ctrl.transform.SetSiblingIndex(majiang.transform.GetSiblingIndex());
                ctrl.transform.position = majiang.transform.position;
                ctrl.transform.rotation = majiang.transform.rotation;

                m_ListWall[i] = ctrl;
                PushMaJiangToPool(majiang);
            }

            return m_ListWall;
        }



        //设置手牌 （空手牌 赋值）

        public void SetHand(int seatPos, Poker poker)
        {

            //判断是否需要加载
            List<MaJiangCtrl_PaiJiu> handPokerList = GetHand(seatPos);
            if (handPokerList == null) return;

            for (int i = 0; i < handPokerList.Count; i++)
            {
                if (handPokerList[i].Poker.index == poker.index)
                {
                    AppDebug.Log(string.Format("加载有数据的牌"));
                    MaJiangCtrl_PaiJiu ctrl=   SpawnMaJiang(seatPos, poker, "Hand");

                    ctrl.transform.SetParent(handPokerList[i].transform.parent);
                    ctrl.transform.localScale = handPokerList[i].transform.localScale;
                    ctrl.transform.localPosition = handPokerList[i].transform.localPosition;
                    ctrl.transform.localEulerAngles= handPokerList[i].transform.localEulerAngles;
                    PushMaJiangToPool(handPokerList[i].gameObject);                  
                    handPokerList[i] = ctrl;
                }
            }

        }


        //弃牌 由座位一个一个弃牌
        public MaJiangCtrl_PaiJiu ClearHandPoker(int seatPos, Poker poker)
        {
     
            List<MaJiangCtrl_PaiJiu> lst = m_DicHand[seatPos];

            //从手牌里找牌
            MaJiangCtrl_PaiJiu majiang = null;
            for (int i = 0; i < lst.Count; ++i)
            {
                if (lst[i].Poker.size == poker.size && lst[i].Poker.type == poker.type)
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

            MaJiangCtrl_PaiJiu ctrl = SpawnMaJiang(seatPos, poker, "Table");
            if (majiang != null)
            {
                ctrl.transform.position = majiang.transform.position;
                ctrl.transform.rotation = majiang.transform.rotation;
                PushMaJiangToPool(majiang.gameObject);//删除
                //m_WallPool.Despawn(majiang.transform);//删除
            }

            //把牌放桌面上
            if (!m_DicTable.ContainsKey(seatPos))
            {
                m_DicTable.Add(seatPos, new List<MaJiangCtrl_PaiJiu>());
            }
            m_DicTable[seatPos].Add(ctrl);
            return ctrl;


        }


        /// <summary>
        ///  清空牌墙
        /// </summary>
        public void ClearWall()
        {
            for (int i = m_ListWall.Count - 1; i >= 0; i--)
            {
                MaJiangCtrl_PaiJiu deletePoker = m_ListWall[i];
                m_ListWall.Remove(deletePoker);
                //存入对象池
                PushMaJiangToPool(deletePoker.gameObject);
            }
            m_ListWall.Clear();

        }





        //获得手牌
        public List<MaJiangCtrl_PaiJiu> GetHand(int seatPos)
        {
            if (!m_DicHand.ContainsKey(seatPos))
            {
                //m_DicHand.Add(seatPos, new List<MaJiangCtrl_PaiJiu>());

                //AppDebug.LogError(string.Format("没有座位{0}的牌", seatPos));
                return null;
            }
            return m_DicHand[seatPos];
        }

       //获得打过的牌
        public List<MaJiangCtrl_PaiJiu> GetDicTable(int seatIndex)
        {
            if (!m_DicTable.ContainsKey(seatIndex))
            {
                m_DicTable.Add(seatIndex, new List<MaJiangCtrl_PaiJiu>());
                //AppDebug.LogError(string.Format("没有座位{0}的牌", seatPos));
                return null;
            }
            return m_DicTable[seatIndex];

         


        }
    }
}