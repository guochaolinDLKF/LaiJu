//===================================================
//Author      : DRB
//CreateTime  ：12/5/2017 10:27:15 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


namespace ShiSanZhang
{
    public class UITipPokerShiSanZhangView : UIViewBase
    {
        public static UITipPokerShiSanZhangView Instance;
        [SerializeField]
        private UIItemSeat_ShiSanZhang m_MySeat;
        [SerializeField]
        private GameObject m_TipObj;
        [SerializeField]
        private GameObject m_TipDuiZihui;
        [SerializeField]
        private GameObject m_TipLiangDuihui;
        [SerializeField]
        private GameObject m_TipSanTiaohui;
        [SerializeField]
        private GameObject m_TipShunZihui;
        [SerializeField]
        private GameObject m_TipTongHuahui;
        [SerializeField]
        private GameObject m_TipHuLuhui;
        [SerializeField]
        private GameObject m_TipTieZhihui;
        [SerializeField]
        private GameObject m_TipTongHuaShunhui;
        /// <summary>
        /// button
        /// </summary>
        [SerializeField]
        private GameObject m_Kuang3;
        [SerializeField]
        private GameObject m_Kuang5_1;
        [SerializeField]
        private GameObject m_Kuang5_2;
        [SerializeField]
        private GameObject m_touBtnHui;
        [SerializeField]
        private GameObject m_zhongBtnHui;
        [SerializeField]
        private GameObject m_weiBtnHui;

        /// <summary>
        /// Poker挂载点
        /// </summary>
        [SerializeField]
        private Transform Tou_Container;
        [SerializeField]
        private Transform Zhong_Container;
        [SerializeField]
        private Transform Wei_Container;

        [SerializeField]
        private GameObject m_ChuPai;//出牌按钮
        [SerializeField]
        private GameObject[] m_BtnSort;//排序按钮
        [SerializeField]
        private Image m_CuoWu;//出牌时错误牌型提示

        //List<Poker> Tou_List = new List<Poker>();//头
        //List<Poker> Zhong_List = new List<Poker>();//中
        //List<Poker> Wei_List = new List<Poker>();//尾

        
        private Tweener ImageTween;        



        public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
        {
            Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
            //dic.Add(RoomShiSanZhangProxy.ON_SEAT_INFO_CHANGED, OnSeatInfoChanged);
            dic.Add(ShiSanZhangConstant.OnShiSanZhangGroupPoker, OnShiSanZhangGroupPoker);//组合牌
            dic.Add(ShiSanZhangConstant.OnShiSanZhangGroupPokerShow, GroupPokerShow);//组合牌结束
            return dic;
        }



        protected override void OnAwake()
        {
            base.OnAwake();
            Instance = this;
            if (m_TipObj != null) m_TipObj.SetActive(false);
            ImageTween = m_CuoWu.DOColor(new Color(255f,255f,255f,255f),1).SetEase(Ease.Linear).SetAutoKill(false).Pause();           
        }


        protected override void OnBtnClick(GameObject go)
        {
            base.OnBtnClick(go);
            switch (go.name)
            {
                case "ShiSanZhangTipDuiZi":
                    TipPlayPoker(DeckType.DUI_ZI);
                    break;
                case "ShiSanZhangTipLiangDui":
                    TipPlayPoker(DeckType.LIANG_DUI);
                    break;
                case "ShiSanZhangTipSanTiao":
                    TipPlayPoker(DeckType.SAN_TIAO);
                    break;
                case "ShiSanZhangTipShunZi":
                    TipPlayPoker(DeckType.SHUN_ZI);
                    break;
                case "ShiSanZhangTipTongHua":
                    TipPlayPoker(DeckType.TONG_HUA);
                    break;
                case "ShiSanZhangTipHuLu":
                    TipPlayPoker(DeckType.HU_LU);
                    break;
                case "ShiSanZhangTipTieZhi":
                    TipPlayPoker(DeckType.TIE_ZHI);
                    break;
                case "ShiSanZhangTipTongHuaShun":
                    TipPlayPoker(DeckType.TONG_HUA_SHUN);
                    break;
                case "kuang":
                    ChoicePoker(LevelType.TOU_DAO);
                    break;
                case "kuang1":
                    ChoicePoker(LevelType.ZHONG_DAO);
                    break;
                case "kuang2":
                    ChoicePoker(LevelType.WEI_DAO);
                    break;
                case "touBtnHui":
                    ChoicePokerHui(LevelType.TOU_DAO);
                    break;
                case "zhongBtnHui":
                    ChoicePokerHui(LevelType.ZHONG_DAO);
                    break;
                case "weiBtnHui":
                    ChoicePokerHui(LevelType.WEI_DAO);
                    break;
                case "btnShiSanZhangViewPlayPoker"://出牌
                    ShiSanZhangGameCtrl.Instance.PlayPoker();
                    break;
                case "ShiSanZhangbtnSortBigSmall":
                    SortHandPoker(EnumSortPoker.BIG_SMALL);
                    break;
                case "ShiSanZhangbtnSortColor":
                    SortHandPoker(EnumSortPoker.POKER_COLOR);
                    break;                    
            }
        }
       

        #region 提示
        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="deckType"></param>
        private void TipPlayPoker(DeckType deckType)
        {
            SendNotification("OnShiSanZhangTipPlayPoker",deckType);
        }
        #endregion

        #region 组合牌结束
        /// <summary>
        /// 组合牌结束
        /// </summary>
        /// <param name="data"></param>
        private void GroupPokerShow(TransferData data)
        {
            m_TipObj.SetActive(false);
        }
        #endregion

        #region 确认选择的牌
        /// <summary>
        /// 选择牌
        /// </summary>
        private void ChoicePoker(LevelType lecelType)
        {
            List<UIItemPoker_ShiSanZhang> pokerList = m_MySeat.HandList;            
            if (lecelType== LevelType.TOU_DAO)
            {
                for (int i = 0; i <pokerList.Count ; i++)
                {
                    if (pokerList[i].isSelect)
                    {
                        pokerList[i].isSelect = !pokerList[i].isSelect;
                        m_MySeat.HandPokerTou.Add(pokerList[i]);                                                          
                        SetPokerContainer(pokerList[i].gameObject.transform, Tou_Container,0.75f);
                        SendNotification(ShiSanZhangConstant.OnShiSanZhangRemoveHandPoker, pokerList[i].Poker,LevelType.TOU_DAO);
                        //pokerList.Remove(pokerList[i]);
                    }
                }
                pokerList.RemoveAll(UIItemPoker_ShiSanZhang => m_MySeat.HandPokerTou.Contains(UIItemPoker_ShiSanZhang));
                SetTipButtonShowAndHid(RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList);
                m_touBtnHui.SetActive(true);
                m_Kuang3.SetActive(false);
            }
            else if (lecelType== LevelType.ZHONG_DAO)
            {
                for (int i = 0; i < pokerList.Count; i++)
                {
                    if (pokerList[i].isSelect)
                    {
                        pokerList[i].isSelect = !pokerList[i].isSelect;
                        m_MySeat.HandPokerZhong.Add(pokerList[i]);                        
                        SetPokerContainer(pokerList[i].gameObject.transform, Zhong_Container, 0.75f);
                        SendNotification(ShiSanZhangConstant.OnShiSanZhangRemoveHandPoker, pokerList[i].Poker, LevelType.ZHONG_DAO);
                        //pokerList.Remove(pokerList[i]);
                    }
                }
                pokerList.RemoveAll(UIItemPoker_ShiSanZhang => m_MySeat.HandPokerZhong.Contains(UIItemPoker_ShiSanZhang));
                SetTipButtonShowAndHid(RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList);
                m_zhongBtnHui.SetActive(true);
                m_Kuang5_1.SetActive(false);
            }
            else if (lecelType == LevelType.WEI_DAO)
            {
                for (int i = 0; i < pokerList.Count; i++)
                {
                    if (pokerList[i].isSelect)
                    {
                        pokerList[i].isSelect = !pokerList[i].isSelect;
                        m_MySeat.HandPokerWei.Add(pokerList[i]);                        
                        SetPokerContainer(pokerList[i].gameObject.transform, Wei_Container, 0.75f);
                        SendNotification(ShiSanZhangConstant.OnShiSanZhangRemoveHandPoker, pokerList[i].Poker, LevelType.WEI_DAO);
                        //pokerList.Remove(pokerList[i]);
                    }
                }
                pokerList.RemoveAll(UIItemPoker_ShiSanZhang => m_MySeat.HandPokerWei.Contains(UIItemPoker_ShiSanZhang));
                SetTipButtonShowAndHid(RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList);
                m_weiBtnHui.SetActive(true);
                m_Kuang5_2.SetActive(false);
            }

            if (m_MySeat.HandPokerTou.Count==3&& m_MySeat.HandPokerZhong.Count==5)
            {
                for (int i = 0; i < pokerList.Count; i++)
                {
                    m_MySeat.HandPokerWei.Add(pokerList[i]);
                    pokerList[i].isSelect = false;                   
                    SetPokerContainer(pokerList[i].gameObject.transform, Wei_Container, 0.75f);
                    SendNotification(ShiSanZhangConstant.OnShiSanZhangRemoveHandPoker, pokerList[i].Poker, LevelType.WEI_DAO);
                    //pokerList.Remove(pokerList[i]);
                }
                pokerList.RemoveAll(UIItemPoker_ShiSanZhang => m_MySeat.HandPokerWei.Contains(UIItemPoker_ShiSanZhang));
                pokerList.Clear();                             
                m_weiBtnHui.SetActive(true);
                m_Kuang5_2.SetActive(false);
            }
            else if (m_MySeat.HandPokerTou.Count == 3 && m_MySeat.HandPokerWei.Count == 5)
            {
                for (int i = 0; i < pokerList.Count; i++)
                {
                    m_MySeat.HandPokerZhong.Add(pokerList[i]);
                    pokerList[i].isSelect = false;                  
                    SetPokerContainer(pokerList[i].gameObject.transform, Zhong_Container, 0.75f);
                    SendNotification(ShiSanZhangConstant.OnShiSanZhangRemoveHandPoker, pokerList[i].Poker, LevelType.ZHONG_DAO);
                   // pokerList.Remove(pokerList[i]);
                }
                pokerList.RemoveAll(UIItemPoker_ShiSanZhang => m_MySeat.HandPokerZhong.Contains(UIItemPoker_ShiSanZhang));
                m_zhongBtnHui.SetActive(true);
                m_Kuang5_1.SetActive(false);
            }
            else if (m_MySeat.HandPokerZhong.Count == 5 && m_MySeat.HandPokerWei.Count == 5)
            {

                for (int i = 0; i < pokerList.Count; i++)
                {
                    m_MySeat.HandPokerTou.Add(pokerList[i]);
                    pokerList[i].isSelect = false;                    
                    SetPokerContainer(pokerList[i].gameObject.transform, Tou_Container, 0.75f);
                    SendNotification(ShiSanZhangConstant.OnShiSanZhangRemoveHandPoker, pokerList[i].Poker, LevelType.TOU_DAO);
                   // pokerList.Remove(pokerList[i]);
                }
                pokerList.RemoveAll(UIItemPoker_ShiSanZhang => m_MySeat.HandPokerTou.Contains(UIItemPoker_ShiSanZhang));
                m_touBtnHui.SetActive(true);
                m_Kuang3.SetActive(false);
            }
            if (m_MySeat.HandPokerTou.Count == 3 && m_MySeat.HandPokerZhong.Count == 5 && m_MySeat.HandPokerWei.Count == 5)
            {
                m_ChuPai.SetActive(true);
                m_BtnSort[0].SetActive(false);
                m_BtnSort[1].SetActive(false);
            }
            TipKuang();
        }
        #endregion

        #region 选择牌回去
        /// <summary>
        /// 选择牌回去
        /// </summary>
        /// <param name="lecelType"></param>
        private void ChoicePokerHui(LevelType lecelType)
        {
            List<UIItemPoker_ShiSanZhang> pokerList = m_MySeat.HandList;
            if (lecelType == LevelType.TOU_DAO)
            {
                for (int i = 0; i < m_MySeat.HandPokerTou.Count; i++)
                {
                    m_MySeat.HandList.Add(m_MySeat.HandPokerTou[i]);
                    SetPokerContainer(m_MySeat.HandPokerTou[i].gameObject.transform, m_MySeat.HandContainer);
                    SendNotification(ShiSanZhangConstant.OnShiSanZhangAddHandPoker, m_MySeat.HandPokerTou[i].Poker, LevelType.TOU_DAO);
                }
                m_MySeat.HandPokerTou.Clear();             
                SetTipButtonShowAndHid(RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList);//重新检测牌型              
                m_touBtnHui.SetActive(false);                
            }
            else if (lecelType == LevelType.ZHONG_DAO)
            {
                for (int i = 0; i < m_MySeat.HandPokerZhong.Count; i++)
                {
                    m_MySeat.HandList.Add(m_MySeat.HandPokerZhong[i]);
                    SetPokerContainer(m_MySeat.HandPokerZhong[i].gameObject.transform, m_MySeat.HandContainer);
                    SendNotification(ShiSanZhangConstant.OnShiSanZhangAddHandPoker, m_MySeat.HandPokerZhong[i].Poker, LevelType.ZHONG_DAO);
                }
                m_MySeat.HandPokerZhong.Clear();
                SetTipButtonShowAndHid(RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList);//重新检测牌型      
                m_zhongBtnHui.SetActive(false);              
            }
            else if (lecelType == LevelType.WEI_DAO)
            {
                for (int i = 0; i < m_MySeat.HandPokerWei.Count; i++)
                {
                    m_MySeat.HandList.Add(m_MySeat.HandPokerWei[i]);
                    SetPokerContainer(m_MySeat.HandPokerWei[i].gameObject.transform, m_MySeat.HandContainer);
                    SendNotification(ShiSanZhangConstant.OnShiSanZhangAddHandPoker, m_MySeat.HandPokerWei[i].Poker, LevelType.WEI_DAO);
                }
                m_MySeat.HandPokerWei.Clear();
                SetTipButtonShowAndHid(RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList);
                m_weiBtnHui.SetActive(false);                        
            }
            SortHandPoker(EnumSortPoker.BIG_SMALL);
            m_ChuPai.SetActive(false);
        }
        #endregion

        #region 设置牌的位置
        /// <summary>
        /// 设置牌的位置
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="ParentTran"></param>
        private void SetPokerContainer(Transform tran,Transform ParentTran,float scale=1)
        {
            tran.SetParent(ParentTran);
            tran.localScale = new Vector3(scale, scale, scale);
            tran.localPosition = Vector3.zero;
        }
        #endregion

        #region 通知组合牌
        /// <summary>
        /// 通知组合牌
        /// </summary>
        /// <param name="data"></param>
        private void OnShiSanZhangGroupPoker(TransferData data)
        {       
            List<Poker> pokerList = data.GetValue<List<Poker>>("PokerList");
            if (pokerList == null)
            {
                Debug.Log("我日，要检测的牌怎么是空呢！！！！！！！！！！！！！！！！！！！！！！！！！！！");
                return;
            }
            ClearList();
            m_TipObj.SetActive(true);
            SetTipButtonShowAndHid(pokerList);
        }
        #endregion

        #region 检测牌型，显示按钮
        public void SetTipButtonShowAndHid(List<Poker> pokerList)
        {
            m_TipDuiZihui.SetActive(DeckRules.GetAllDuiZi(pokerList).Count==0);
            m_TipLiangDuihui.SetActive(DeckRules.GetAllLiangDui(pokerList).Count==0);
            m_TipSanTiaohui.SetActive(DeckRules.GetAllSanTiao(pokerList).Count==0);
            m_TipShunZihui.SetActive(DeckRules.GetAllShunZi(pokerList).Count==0);
            m_TipTongHuahui.SetActive(DeckRules.GetAllTongHua(pokerList).Count==0);
            m_TipHuLuhui.SetActive(DeckRules.GetAllHuLu(pokerList).Count==0);
            m_TipTieZhihui.SetActive(DeckRules.GetAllTieZhi(pokerList).Count==0);
            m_TipTongHuaShunhui.SetActive(DeckRules.GetAllTongHuaShun(pokerList).Count==0);
        }
        #endregion

        #region 显示三张或者5张牌的框
        /// <summary>
        /// 显示或隐藏提示框
        /// </summary>
        public void TipKuang()
        {
            List<UIItemPoker_ShiSanZhang> pokerList = m_MySeat.HandList;
            int index = 0;
            for (int i = 0; i < pokerList.Count; i++)
            {
                if (pokerList[i].isSelect)
                {
                    index++;
                }
            }
            if (index==3)
            {                
                if(m_MySeat.HandPokerTou.Count==0)
                 m_Kuang3.SetActive(true);
            }
            else if (index==5)
            {
                if(m_MySeat.HandPokerZhong.Count==0|| m_MySeat.HandPokerWei.Count == 0)
                {
                    m_Kuang5_1.SetActive(true);
                    m_Kuang5_2.SetActive(true);
                }                  
            }
            else
            {
                m_Kuang3.SetActive(false);
                m_Kuang5_1.SetActive(false);
                m_Kuang5_2.SetActive(false);
            }
        }
        #endregion

        #region 初始化按钮和列表
        /// <summary>
        /// 初始化
        /// </summary>
        private void ClearList()
        {           
            m_Kuang3.SetActive(false);
            m_Kuang5_1.SetActive(false);
            m_Kuang5_2.SetActive(false);
            m_touBtnHui.SetActive(false);
            m_zhongBtnHui.SetActive(false);
            m_weiBtnHui.SetActive(false);
            m_CuoWu.gameObject.SetActive(false);
            m_BtnSort[0].SetActive(false);//大小排序
            m_BtnSort[1].SetActive(true);//花色排序
            m_ChuPai.SetActive(false);
        }
        #endregion

        #region OnSelectPoker 选择牌
        /// <summary>
        /// 选择牌
        /// </summary>
        /// <param name="obj"></param>
        public void SelectPoker(List<Poker> pokers)
        {
            List<UIItemPoker_ShiSanZhang> pokerList = m_MySeat.HandList;
            for (int i = 0; i < pokerList.Count; ++i)
            {
                bool isExists = false;
                for (int j = 0; j < pokers.Count; ++j)
                {
                    if (pokerList[i].Poker.Index == pokers[j].Index)
                    {
                        isExists = true;
                        break;
                    }
                }
                pokerList[i].isSelect = isExists;
            }
        }
        #endregion

        #region 排序手牌（按照大小或者花色）
        /// <summary>
        /// 排序手牌（按照大小或者花色）
        /// </summary>
        private void SortHandPoker(EnumSortPoker sortHandPoker)
        {
            if (sortHandPoker==EnumSortPoker.BIG_SMALL)
            {
                RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList.Sort(Comparator);             
                for (int i = 0; i < RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList.Count; i++)
                {
                    m_MySeat.HandList[i].Init(RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList[i]);
                }
                m_BtnSort[0].SetActive(false);
                m_BtnSort[1].SetActive(true);
            }
            else
            {
                RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList.Sort(Comparator1);
                for (int i = 0; i < RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList.Count; i++)
                {
                    m_MySeat.HandList[i].Init(RoomShiSanZhangProxy.Instance.PlayerSeat.handPokerList[i]);
                }
                m_BtnSort[1].SetActive(false);
                m_BtnSort[0].SetActive(true);
            }
        }
        #endregion

        #region 出牌错误提示
        // 错误提示
        public void FalseHints()
        {
            m_CuoWu.gameObject.SetActive(true);
            ImageTween.OnComplete(() =>
            {
               StartCoroutine("FalseHide");
            }).Restart();
        }
        IEnumerator FalseHide()
        {
            yield return new WaitForSeconds(3);
            ImageTween.PlayBackwards();
            m_CuoWu.gameObject.SetActive(false);
        }
        #endregion

        #region 排序
        /// <summary>
        /// 按照大小排序(从大到小排序)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>  
        public  int Comparator(Poker x, Poker y)
        {
            if (x.Size <= y.Size)
                return 1;
            else
                return -1;
        }
        /// <summary>
        /// 按照花色大小排序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public   int Comparator1(Poker x, Poker y)
        {
            if (x.Color < y.Color)
            {
                return 1;
            }
            else if (x.Color == y.Color)
            {
                if (x.Size <= y.Size)
                    return 1;
                else
                    return -1;
            }
            else
            {
                return -1;
            }                          
        }
        #endregion


    }
}
