//===================================================
//Author      : DRB
//CreateTime  ：4/8/2017 4:46:49 PM
//Description ：听牌提示界面
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DRB.MahJong
{
    public class UIItemTingTip : MonoBehaviour
    {

        public static UIItemTingTip Instance;
        [SerializeField]
        private Transform m_Container;
        [SerializeField]
        private Image m_BG;
        [SerializeField]
        private Image m_ImageAllHu;
        [SerializeField]
        private Image m_ImageHu;


        private const int SHOW_ALL_HU = 27;


        private List<GameObject> m_HuList = new List<GameObject>();

        private void Awake()
        {
            Instance = this;
            gameObject.SetActive(false);
        }

        public void Show(MaJiangCtrl ctrl, List<Poker> lst)
        {
#if IS_HONGHU || IS_LEPING
            return;
#endif
#if IS_TAILAI
            if (!RoomMaJiangProxy.Instance.PlayerSeat.IsTing) return;
#endif
            if (RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Ready) return;
            if (RoomMaJiangProxy.Instance.CurrentRoom.isReplay) return;
            if (RoomMaJiangProxy.Instance.CurrentRoom.Status == RoomEntity.RoomStatus.Settle) return;
            if (RoomMaJiangProxy.Instance.CurrentRoom.CurrentOperator != RoomMaJiangProxy.Instance.PlayerSeat && ctrl != null) return;
            if (RoomMaJiangProxy.Instance.CurrentState == MahjongGameState.PlayPoker && ctrl != null) return;
            if (lst == null || lst.Count == 0)
            {
                Close();
                return;
            }
            gameObject.SetActive(true);
            MahJongHelper.SimpleSort(lst);
            for (int i = 0; i < m_HuList.Count; ++i)
            {
                m_HuList[i].SetActive(false);
            }
            if (lst.Count < SHOW_ALL_HU)
            {
                m_ImageAllHu.gameObject.SetActive(false);
                m_ImageHu.gameObject.SetActive(true);
                for (int i = 0; i < lst.Count; ++i)
                {
                    GameObject go = null;

                    if (i < m_HuList.Count)
                    {
                        go = m_HuList[i];
                        go.SetActive(true);
                    }
                    else
                    {
                        go = new GameObject();
                        m_HuList.Add(go);
                        go.SetParent(m_Container);
                    }
                    Image img = go.GetOrCreatComponent<Image>();
                    img.overrideSprite = MahJongManager.Instance.LoadPokerSprite(lst[i], false);
                    img.SetNativeSize();

                    //AssetBundleManager.Instance.LoadSpriteAsync(path, imgName,(Sprite sprite)=> 
                    //{
                    //    img.overrideSprite = sprite;
                    //    img.SetNativeSize();
                    //});
                }
                m_BG.SafeSetActive(true);
                m_BG.rectTransform.sizeDelta = new Vector2(lst.Count * 100 + 100, m_BG.rectTransform.sizeDelta.y);
            }
            else
            {
                m_ImageAllHu.gameObject.SetActive(true);
                m_ImageHu.gameObject.SetActive(false);
                m_BG.SafeSetActive(false);
            }



        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
