//===================================================
//Author      : DRB
//CreateTime  ：3/15/2017 9:37:14 AM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DRB.MahJong
{
    public class UIItemOperator : UIItemBase
    {
        public static UIItemOperator Instance;

        [SerializeField]
        private Image m_ImageChi;
        [SerializeField]
        private Image m_ImagePeng;
        [SerializeField]
        private Image m_ImageGang;
        [SerializeField]
        private Image m_ImageTing;
        [SerializeField]
        private Image m_ImageHu;
        [SerializeField]
        private Image m_ImageZiMo;
        [SerializeField]
        private Image m_ImagePass;
        [SerializeField]
        private Image m_ImageCancel;
        [SerializeField]
        private Image m_ImageZhiDui;
        [SerializeField]
        private Image m_ImageChiTing;
        [SerializeField]
        private Image m_ImagePengTing;
        [SerializeField]
        private Image m_ImageLiangXi;
        [SerializeField]
        private Image m_ImageOK;
        [SerializeField]
        private Image m_ImageDingZhang;
        [SerializeField]
        private Image m_ImageDingJiang;
        [SerializeField]
        private Image m_ImageKou;
        [SerializeField]
        private Image m_ImageBuXi;
        [SerializeField]
        private Image m_ImagePiaoTing;
        [SerializeField]
        private Image m_ImagePao;
        [SerializeField]
        private Image m_ImageJiao;
        [SerializeField]
        private Image m_ImageMingTi;
        [SerializeField]
        private Image m_ImageWan;
        [SerializeField]
        private Image m_ImageTong;
        [SerializeField]
        private Image m_ImageTiao;

        [SerializeField]
        private Transform m_DetailContainer;


        private OperatorType m_CurrentType;


        private List<UIItemOperateDetail> m_ListDetail = new List<UIItemOperateDetail>(2);

        private List<Poker> m_PengList;//碰列表
        private List<List<Poker>> m_GangList;//杠列表
        private List<List<Poker>> m_ChiList;//吃列表
        private List<List<Poker>> m_ZhiDuiList;//支对列表
        private List<List<Poker>> m_ChiTingList;//吃听列表
        private List<Poker> m_PengTingList;//碰听列表
        private List<List<Poker>> m_LiangXiList;//亮喜列表
        private List<List<Poker>> m_KouList;//扣牌列表
        private List<Poker> m_BuXiList;//补喜列表
        private bool m_isTing;//可以听
        private bool m_isZiMo;
        private bool m_isHu;
        private bool m_isDingZhang;
        private bool m_isDingJiang;
        private bool m_isPiaoTing;
        private bool m_isPao;
        private bool m_isJiao;
        private bool m_isMingTi;
        protected override void OnAwake()
        {
            base.OnAwake();
            Instance = this;

            Button[] arr = GetComponentsInChildren<Button>();
            for (int i = 0; i < arr.Length; ++i)
            {
                EventTriggerListener.Get(arr[i].gameObject).onClick = OnBtnClick;
            }
            Close();
        }

        public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
        {
            Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
            dic.Add(RoomMaJiangProxy.ON_ROOM_INFO_CHANGED, OnRoomInfoChanged);
            return dic;
        }

        private void OnRoomInfoChanged(TransferData data)
        {
            RoomEntity room = data.GetValue<RoomEntity>("Room");
            if (room == null) return;
            ShowColor(room.Status == RoomEntity.RoomStatus.LackColor && room.PlayerSeat.LackColor == 0);
        }

        public void Show(List<List<Poker>> chiList, List<Poker> pengList, List<List<Poker>> gangList, bool isHu, bool isZiMo,
                            List<List<Poker>> chiTingList, List<Poker> pengTingList, List<List<Poker>> liangxiList, bool isDingZhang,
                            bool isDingJiang, List<List<Poker>> kouList, List<Poker> buxiList, bool isPiaoTing, bool isPao, bool isJiao,
                            bool isMingTi)
        {
            m_ChiList = chiList;
            m_PengList = pengList;
            m_GangList = gangList;
            m_ChiTingList = chiTingList;
            m_PengTingList = pengTingList;
            m_LiangXiList = liangxiList;
            m_KouList = kouList;
            m_BuXiList = buxiList;
            m_isZiMo = isZiMo;
            m_isHu = isHu;
            m_isDingZhang = isDingZhang;
            m_isDingJiang = isDingJiang;
            m_isPiaoTing = isPiaoTing;
            m_isPao = isPao;
            m_isJiao = isJiao;
            m_isMingTi = isMingTi;

            Show();
            UIItemTingTip.Instance.Close();
        }

        private void Show()
        {
            m_ImageChi.SafeSetActive(m_ChiList != null && m_ChiList.Count > 0);
            m_ImagePeng.SafeSetActive(m_PengList != null && m_PengList.Count > 0);
            m_ImageGang.SafeSetActive(m_GangList != null && m_GangList.Count > 0);
            m_ImageZiMo.SafeSetActive(m_isZiMo && m_isHu);
            m_ImageHu.SafeSetActive(!m_isZiMo && m_isHu);
            m_ImagePass.SafeSetActive(true);
            m_ImageChiTing.SafeSetActive(m_ChiTingList != null && m_ChiTingList.Count > 0);
            m_ImagePengTing.SafeSetActive(m_PengTingList != null && m_PengTingList.Count > 0);
            m_ImageLiangXi.SafeSetActive(m_LiangXiList != null && m_LiangXiList.Count > 0);
            m_ImageDingZhang.SafeSetActive(m_isDingZhang);
            m_ImageDingJiang.SafeSetActive(m_isDingJiang);
            m_ImageKou.SafeSetActive(m_KouList != null && m_KouList.Count > 0);
            m_ImageBuXi.SafeSetActive(m_BuXiList != null && m_BuXiList.Count > 0);
            m_ImagePiaoTing.SafeSetActive(m_isPiaoTing);
            m_ImageTing.SafeSetActive(m_isTing);
            m_ImagePao.SafeSetActive(m_isPao);
            m_ImageJiao.SafeSetActive(m_isJiao);
            m_ImageMingTi.SafeSetActive(m_isMingTi);
        }

        public void ShowColor(bool isShow)
        {
            m_ImageWan.SafeSetActive(isShow);
            m_ImageTong.SafeSetActive(isShow);
            m_ImageTiao.SafeSetActive(isShow);
        }


        public void ShowTing(bool canTing)
        {
            m_isTing = canTing;
            m_ImageTing.SafeSetActive(m_isTing);
            m_ImagePass.SafeSetActive(canTing);
        }

        public void ShowOK(bool canOK)
        {
            if (m_ImageOK == null) return;

            m_ImageOK.SafeSetActive(canOK);
        }

        public void ShowZhiDui(List<List<Poker>> lstZhiDui, bool canPass)
        {
            m_ZhiDuiList = lstZhiDui;
            m_ImageZhiDui.SafeSetActive(true);
            if (canPass)
            {
                m_ImagePass.SafeSetActive(true);
            }
        }

        public void Close(bool isClear = true)
        {
            for (int i = 0; i < m_ListDetail.Count; ++i)
            {
                m_ListDetail[i].gameObject.SetActive(false);
            }
            m_ImageChi.SafeSetActive(false);
            m_ImagePeng.SafeSetActive(false);
            m_ImageGang.SafeSetActive(false);
            m_ImageHu.SafeSetActive(false);
            m_ImagePass.SafeSetActive(false);
            m_ImageTing.SafeSetActive(false);
            m_ImageCancel.SafeSetActive(false);
            m_ImageZhiDui.SafeSetActive(false);
            m_ImageChiTing.SafeSetActive(false);
            m_ImagePengTing.SafeSetActive(false);
            m_ImageLiangXi.SafeSetActive(false);
            m_ImageOK.SafeSetActive(false);
            m_ImageZiMo.SafeSetActive(false);
            m_ImageDingZhang.SafeSetActive(false);
            m_ImageDingJiang.SafeSetActive(false);
            m_ImageKou.SafeSetActive(false);
            m_ImageBuXi.SafeSetActive(false);
            m_ImagePiaoTing.SafeSetActive(false);
            m_ImagePao.SafeSetActive(false);
            m_ImageJiao.SafeSetActive(false);
            m_ImageMingTi.SafeSetActive(false);
            m_ImageWan.SafeSetActive(false);
            m_ImageTong.SafeSetActive(false);
            m_ImageTiao.SafeSetActive(false);
            if (isClear)
            {
                m_PengList = null;
                m_GangList = null;
                m_ChiList = null;
                m_ZhiDuiList = null;
                m_ChiTingList = null;
                m_PengTingList = null;
                m_LiangXiList = null;
                m_KouList = null;
                m_BuXiList = null;

                m_isTing = false;
                m_isZiMo = false;
                m_isHu = false;
                m_isDingZhang = false;
                m_isDingJiang = false;
                m_isPiaoTing = false;
                m_isPao = false;
                m_isJiao = false;
                m_isMingTi = false;
            }
        }

        private void OnBtnClick(GameObject go)
        {
            List<Poker> lst = null;
            if (go == m_ImageChi.gameObject)
            {
                m_CurrentType = OperatorType.Chi;
                lst = m_ChiList[0];
                if (m_ChiList.Count > 1)
                {
                    UIViewManager.Instance.LoadItemAsync("UIItemOperateDetail", (GameObject prefab) =>
                    {
                        m_DetailContainer.GetComponent<GridLayoutGroup>().spacing = new Vector2(400, 0);
                        for (int i = 0; i < m_ChiList.Count; ++i)
                        {
                            UIItemOperateDetail detail = null;
                            if (i >= m_ListDetail.Count)
                            {
                                GameObject obj = Instantiate(prefab);
                                obj.SetParent(m_DetailContainer);
                                detail = obj.GetComponent<UIItemOperateDetail>();
                                m_ListDetail.Add(detail);
                            }
                            else
                            {
                                detail = m_ListDetail[i];
                                detail.gameObject.SetActive(true);
                            }
                            detail.SetUI(m_ChiList[i], OnDetailClick);
                        }
                    });
                    return;
                }
            }
            else if (go == m_ImagePeng.gameObject)
            {
                m_CurrentType = OperatorType.Peng;
                lst = m_PengList;
            }
            else if (go == m_ImageGang.gameObject)
            {
                m_CurrentType = OperatorType.Gang;
                lst = m_GangList[0];
                if (m_GangList.Count > 1)
                {
                    UIViewManager.Instance.LoadItemAsync("UIItemOperateDetail", (GameObject prefab) =>
                    {
                        m_DetailContainer.GetComponent<GridLayoutGroup>().spacing = new Vector2(400, 0);
                        for (int i = 0; i < m_GangList.Count; ++i)
                        {
                            UIItemOperateDetail detail = null;
                            if (i >= m_ListDetail.Count)
                            {
                                GameObject obj = Instantiate(prefab);
                                obj.SetParent(m_DetailContainer);
                                detail = obj.GetComponent<UIItemOperateDetail>();
                                m_ListDetail.Add(detail);
                            }
                            else
                            {
                                detail = m_ListDetail[i];
                                detail.gameObject.SetActive(true);
                            }
                            detail.SetUI(m_GangList[i], OnDetailClick);
                        }
                    });
                    return;
                }
            }
            else if (m_ImageTing != null && go == m_ImageTing.gameObject)
            {
                m_CurrentType = OperatorType.Ting;
                Close(false);
                if (m_ImageCancel != null)
                {
                    m_ImageCancel.gameObject.SetActive(true);
                }
            }
            else if (go == m_ImageHu.gameObject)
            {
                m_CurrentType = OperatorType.Hu;
            }
            else if (go == m_ImagePass.gameObject)
            {
                m_CurrentType = OperatorType.Pass;
                Close();
            }
            else if (m_ImageZiMo != null && go == m_ImageZiMo.gameObject)
            {
                m_CurrentType = OperatorType.Hu;
            }
            else if (m_ImageCancel != null && go == m_ImageCancel.gameObject)
            {
                m_CurrentType = OperatorType.Cancel;
                Show();
                if (m_ImageOK != null)
                {
                    m_ImageOK.gameObject.SetActive(false);
                }
                m_ImageCancel.gameObject.SetActive(false);
            }
            else if (m_ImageZhiDui != null && go == m_ImageZhiDui.gameObject)
            {
                m_CurrentType = OperatorType.ZhiDui;
                if (m_ZhiDuiList.Count > 1)
                {
                    UIViewManager.Instance.LoadItemAsync("UIItemOperateDetail", (GameObject prefab) =>
                    {
                        m_DetailContainer.GetComponent<GridLayoutGroup>().spacing = new Vector2(300, 0);
                        for (int i = 0; i < m_ZhiDuiList.Count; ++i)
                        {
                            UIItemOperateDetail detail = null;
                            if (i >= m_ListDetail.Count)
                            {
                                GameObject obj = Instantiate(prefab);
                                obj.SetParent(m_DetailContainer);
                                detail = obj.GetComponent<UIItemOperateDetail>();
                                m_ListDetail.Add(detail);
                            }
                            else
                            {
                                detail = m_ListDetail[i];
                                detail.gameObject.SetActive(true);
                            }
                            detail.SetUI(m_ZhiDuiList[i], OnDetailClick);
                        }
                    });
                    return;
                }
            }
            else if (m_ImageChiTing != null && go == m_ImageChiTing.gameObject)
            {
                m_CurrentType = OperatorType.ChiTing;
                lst = m_ChiTingList[0];
                if (m_ChiTingList.Count > 1)
                {
                    UIViewManager.Instance.LoadItemAsync("UIItemOperateDetail", (GameObject prefab) =>
                    {
                        m_DetailContainer.GetComponent<GridLayoutGroup>().spacing = new Vector2(400, 0);
                        for (int i = 0; i < m_ChiTingList.Count; ++i)
                        {
                            UIItemOperateDetail detail = null;
                            if (i >= m_ListDetail.Count)
                            {
                                GameObject obj = Instantiate(prefab);
                                obj.SetParent(m_DetailContainer);
                                detail = obj.GetComponent<UIItemOperateDetail>();
                                m_ListDetail.Add(detail);
                            }
                            else
                            {
                                detail = m_ListDetail[i];
                                detail.gameObject.SetActive(true);
                            }
                            detail.SetUI(m_ChiTingList[i], OnDetailClick);
                        }
                    });
                    return;
                }
            }
            else if (m_ImagePengTing != null && go == m_ImagePengTing.gameObject)
            {
                m_CurrentType = OperatorType.PengTing;
                lst = m_PengTingList;
            }
            else if (m_ImageLiangXi != null && go == m_ImageLiangXi.gameObject)
            {
                m_CurrentType = OperatorType.LiangXi;
#if IS_LUALU
                lst = m_LiangXiList[0];
                if (m_LiangXiList.Count > 1)
                {
                    UIViewManager.Instance.LoadItemAsync("UIItemOperateDetail", (GameObject prefab) =>
                    {
                        m_DetailContainer.GetComponent<GridLayoutGroup>().spacing = new Vector2(400, 0);
                        for (int i = 0; i < m_LiangXiList.Count; ++i)
                        {
                            UIItemOperateDetail detail = null;
                            if (i >= m_ListDetail.Count)
                            {
                                GameObject obj = Instantiate(prefab);
                                obj.SetParent(m_DetailContainer);
                                detail = obj.GetComponent<UIItemOperateDetail>();
                                m_ListDetail.Add(detail);
                            }
                            else
                            {
                                detail = m_ListDetail[i];
                                detail.gameObject.SetActive(true);
                            }
                            detail.SetUI(m_LiangXiList[i], OnDetailClick);
                        }
                    });
                    return;
                }
#else
                Close(false);
                m_ImageCancel.gameObject.SetActive(true);
#endif

            }
            else if (m_ImageOK != null && go == m_ImageOK.gameObject)
            {
                m_CurrentType = OperatorType.Ok;
                m_ImageOK.gameObject.SetActive(false);
            }
            else if (m_ImageDingZhang != null && go == m_ImageDingZhang.gameObject)
            {
                m_CurrentType = OperatorType.DingZhang;
            }
            else if (m_ImageDingJiang != null && go == m_ImageDingJiang.gameObject)
            {
                m_CurrentType = OperatorType.DingJiang;
            }
            else if (m_ImageKou != null && go == m_ImageKou.gameObject)
            {
                m_CurrentType = OperatorType.Kou;
                lst = m_KouList[0];
                if (m_KouList.Count > 1)
                {
                    UIViewManager.Instance.LoadItemAsync("UIItemOperateDetail", (GameObject prefab) =>
                    {
                        m_DetailContainer.GetComponent<GridLayoutGroup>().spacing = new Vector2(350, 0);
                        for (int i = 0; i < m_KouList.Count; ++i)
                        {
                            UIItemOperateDetail detail = null;
                            if (i >= m_ListDetail.Count)
                            {
                                GameObject obj = Instantiate(prefab);
                                obj.SetParent(m_DetailContainer);
                                detail = obj.GetComponent<UIItemOperateDetail>();
                                m_ListDetail.Add(detail);
                            }
                            else
                            {
                                detail = m_ListDetail[i];
                                detail.gameObject.SetActive(true);
                            }
                            detail.SetUI(m_KouList[i], OnDetailClick);
                        }
                    });
                    return;
                }
            }
            else if (m_ImageBuXi != null && go == m_ImageBuXi.gameObject)
            {
                m_CurrentType = OperatorType.BuXi;
                lst = new List<Poker>() { m_BuXiList[0] };
                if (m_BuXiList.Count > 1)
                {
                    UIViewManager.Instance.LoadItemAsync("UIItemOperateDetail", (GameObject prefab) =>
                    {
                        m_DetailContainer.GetComponent<GridLayoutGroup>().spacing = new Vector2(180, 0);
                        for (int i = 0; i < m_BuXiList.Count; ++i)
                        {
                            UIItemOperateDetail detail = null;
                            if (i >= m_ListDetail.Count)
                            {
                                GameObject obj = Instantiate(prefab);
                                obj.SetParent(m_DetailContainer);
                                detail = obj.GetComponent<UIItemOperateDetail>();
                                m_ListDetail.Add(detail);
                            }
                            else
                            {
                                detail = m_ListDetail[i];
                                detail.gameObject.SetActive(true);
                            }
                            detail.SetUI(new List<Poker>() { m_BuXiList[i] }, OnDetailClick);
                        }
                    });
                    return;
                }
            }
            else if (m_ImagePiaoTing != null && go == m_ImagePiaoTing.gameObject)
            {
                m_CurrentType = OperatorType.PiaoTing;
            }
            else if (m_ImagePao != null && go == m_ImagePao.gameObject)
            {
                m_CurrentType = OperatorType.Pao;
            }
            else if (m_ImageJiao != null && go == m_ImageJiao.gameObject)
            {
                m_CurrentType = OperatorType.Jiao;
            }
            else if (m_ImageMingTi != null && go == m_ImageMingTi.gameObject)
            {
                m_CurrentType = OperatorType.MingTi;
            }
            else if (m_ImageWan != null && go == m_ImageWan.gameObject)
            {
                m_CurrentType = OperatorType.Wan;
            }
            else if (m_ImageTong != null && go == m_ImageTong.gameObject)
            {
                m_CurrentType = OperatorType.Tong;
            }
            else if (m_ImageTiao != null && go == m_ImageTiao.gameObject)
            {
                m_CurrentType = OperatorType.Tiao;
            }
            SendNotification("OnOperatorClick", m_CurrentType, lst);
        }

        private void OnDetailClick(List<Poker> lst)
        {
            SendNotification("OnOperatorClick", m_CurrentType, lst);
        }
    }
}
