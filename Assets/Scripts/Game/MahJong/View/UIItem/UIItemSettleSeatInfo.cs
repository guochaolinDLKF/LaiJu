//===================================================
//Author      : DRB
//CreateTime  ：3/17/2017 10:00:21 AM
//Description ：结算UI项
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using DRB.MahJong;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSettleSeatInfo : MonoBehaviour
{
    [SerializeField]
    private Image m_Bg;//背景图
    [SerializeField]
    private RawImage m_Head; //头像
    [SerializeField]
    private Text m_TextNickName; //昵称
    [SerializeField]
    private Image m_ImageBanker; //庄家
    [SerializeField]
    private Transform m_HandPokerContainer; //手牌挂载点
    [SerializeField]
    private Transform m_PengPokerContainer; //碰的牌挂载点
    [SerializeField]
    private Transform m_ProbContainer;//抓马挂载点
    [SerializeField]
    private Text m_TextHuInfo; //胡牌类型
    [SerializeField]
    private Text m_GetGold;//当局获得的金币
    [SerializeField]
    private Sprite[] m_SpriteBG;//背景图
    [SerializeField]
    private Image m_ImgTing;
    [SerializeField]
    private Image m_ImgLaZi;

    [SerializeField]
    private Text m_HuType;
    [SerializeField]
    private Image[] m_ArrDirection;


    public void SetUI(int seatPos, bool isPlayer, string avatar, string nickName, int gold,
        int settle, bool isBanker, List<Poker> handList, Poker hitPoker,
        List<PokerCombinationEntity> pengList, List<IncomeDetailEntity> settleInfo,
        List<IncomeDetailEntity> huScoreInfo, bool isWiner, List<Poker> prob, int probTimes,
        bool isLoser, bool isZimo, List<Poker> universal, int totalHuScore, Poker luckPoker,
        bool isTing, List<List<Poker>> zhidui, int bankerPos, bool hasFeng, List<Poker> desktopPoker,
        int direction)
    {
        if (m_HuType != null)
        {
            if (isLoser)
            {
                m_HuType.SafeSetText("点炮");
            }
            if (isZimo && isWiner)
            {
                m_HuType.SafeSetText("自摸");
            }
            if (isWiner && !isZimo)
            {
                m_HuType.SafeSetText("胡");
            }
        }

        if (m_ImgTing != null)
        {
            m_ImgTing.gameObject.SetActive(isTing);
        }

        m_Bg.overrideSprite = m_SpriteBG[isWiner ? 1 : 0];
        TextureManager.Instance.LoadHead(avatar, OnAvatarLoadFinish);
        m_TextNickName.SafeSetText(nickName);
        m_GetGold.SafeSetText(settle.ToString(ConstDefine.STRING_FORMAT_SIGN));
        m_ImageBanker.gameObject.SetActive(isBanker);
        string strInfo = string.Empty;

        bool isLaZi = false;

        int taiCount = 0;
        if (settleInfo != null)
        {
            for (int i = 0; i < settleInfo.Count; ++i)
            {
                cfg_configEntity entity = cfg_configDBModel.Instance.Get(settleInfo[i].typeId);
                if (entity != null)
                {

#if IS_LONGGANG//龙港算台数
                    strInfo += entity.name + (settleInfo[i].poker == null? "": "(" + settleInfo[i].poker.ToChinese() + ")") + settleInfo[i].times.ToString(ConstDefine.STRING_FORMAT_SIGN);
                    strInfo += "台  ";
#elif IS_LEPING || IS_HONGHU || IS_TAILAI
                    strInfo += entity.name;
                    if (entity.name.Equals("打宝"))
                    {
                        strInfo += isWiner ? "+" : "-";
                        strInfo += settleInfo[i].times.ToString();
                    }
                    if (entity.name.Equals("亮喜"))
                    {
                        strInfo += "×";
                        strInfo += settleInfo[i].times.ToString();
                    }
                    strInfo += "  ";
#elif IS_WANGQUE
                    if (entity.name.Equals("辣子"))
                    {
                        isLaZi = true;
                        continue;
                    }

                    if (huScoreInfo != null && huScoreInfo.Count > 0)
                    {
                        strInfo += entity.name + (settleInfo[i].poker == null ? "" : "(" + settleInfo[i].poker.ToChinese() + ")") + settleInfo[i].times.ToString(ConstDefine.STRING_FORMAT_SIGN);
                        strInfo += "台  ";
                    }
                    else
                    {
                        strInfo += entity.name + (settleInfo[i].times == 0 ? "" : settleInfo[i].times.ToString(ConstDefine.STRING_FORMAT_SIGN));
                        strInfo += "  ";
                    }
#elif IS_LAOGUI
                    strInfo += entity.name + settleInfo[i].times.ToString(ConstDefine.STRING_FORMAT_SIGN);
                    strInfo += "  ";
#else
                    strInfo += entity.name + (settleInfo[i].times == 0 ? "" : settleInfo[i].times.ToString(ConstDefine.STRING_FORMAT_SIGN));
                    strInfo += "  ";
#endif
                }
                taiCount += settleInfo[i].times;
            }
        }
#if IS_LONGGANG
        if (taiCount > 3)
        {
            taiCount = 3;
        }
#endif

        int huScore = 0;
        if (huScoreInfo != null)
        {
            for (int i = 0; i < huScoreInfo.Count; ++i)
            {
                Debug.Log(huScoreInfo[i].typeId);
                cfg_configEntity entity = cfg_configDBModel.Instance.Get(huScoreInfo[i].typeId);
                if (entity != null)
                {
#if IS_LONGGANG//龙港算胡数
                    strInfo += entity.name + (huScoreInfo[i].poker == null ? "" : "(" + huScoreInfo[i].poker.ToChinese() + ")") + huScoreInfo[i].times.ToString(ConstDefine.STRING_FORMAT_SIGN);
                    strInfo += "胡  ";
#elif IS_WANGQUE
                    strInfo += entity.name + (huScoreInfo[i].poker == null ? "" : "(" + huScoreInfo[i].poker.ToChinese() + ")") + huScoreInfo[i].times.ToString(ConstDefine.STRING_FORMAT_SIGN);
                    strInfo += "胡  ";
#else
                    strInfo += entity.name + huScoreInfo[i].times.ToString(ConstDefine.STRING_FORMAT_SIGN);
                    strInfo += "  ";
#endif
                }
                huScore += huScoreInfo[i].times;
            }
        }
#if IS_LONGGANG//龙港算总胡数
        strInfo += string.Format("{0}台{1}胡={2}", taiCount, huScore, totalHuScore);
#elif IS_WANGQUE
        if (totalHuScore > 0)
        {
            strInfo += string.Format("{0}台{1}胡={2}", taiCount, huScore, totalHuScore);
        }
#endif
#if !IS_LAOGUI
        if (prob != null && probTimes != 0)
        {
            strInfo += "抓马" + probTimes.ToString(ConstDefine.STRING_FORMAT_SIGN);
        }
#endif


        m_TextHuInfo.SafeSetText(strInfo);

        if (hitPoker != null)
        {
            handList.Add(hitPoker);
        }

        MahJongHelper.Sort(handList, universal, RoomMaJiangProxy.Instance.Rule.UniversalSortType);

        Vector2 handCellSize = m_HandPokerContainer.GetComponent<GridLayoutGroup>().cellSize;

        UIViewManager.Instance.LoadItemAsync("UIItemMahjong", (GameObject prefab) =>
         {
             if (this == null) return;

             for (int i = 0; i < handList.Count; ++i)
             {
                 GameObject go = Instantiate(prefab);
                 UIItemMahjong item = go.GetComponent<UIItemMahjong>();
                 bool isDim = false;
                 bool isHu = false;
                 bool isBao = false;
                 bool isMa = false;
                 bool isUniversal = false;
                 bool isPeng = false;
                 for (int j = 0; j < zhidui.Count; ++j)
                 {
                     if (MahJongHelper.ContainPoker(handList[i], zhidui[j]))
                     {
                         isDim = true;
                         break;
                     }
                 }
                 if (handList[i] == hitPoker && isWiner)
                 {
                     isHu = true;
                 }
                 if (MahJongHelper.CheckUniversal(handList[i], universal))
                 {
                     isUniversal = true;
                 }
                 item.SetUI(handList[i], isHu, isBao, isMa, isUniversal, isPeng, isDim);
                 go.SetParent(m_HandPokerContainer);
             }

             float currentX = 0f;
             if (pengList != null)
             {
                 m_PengPokerContainer.transform.position = new Vector3(m_HandPokerContainer.transform.position.x + (handList.Count * (handCellSize.x / 2)), m_PengPokerContainer.transform.position.y, m_PengPokerContainer.transform.position.z);
                 for (int i = 0; i < pengList.Count; ++i)
                 {
                     if (pengList[i].CombinationType == OperatorType.BuXi) continue;
                     for (int j = 0; j < pengList[i].PokerList.Count; ++j)
                     {
                         GameObject go = Instantiate(prefab);
                         UIItemMahjong item = go.GetComponent<UIItemMahjong>();
                         bool isDim = false;
                         bool isHu = false;
                         bool isBao = false;
                         bool isMa = false;
                         bool isUniversal = false;
                         bool isPeng = true;
                         item.SetSize(handCellSize);
                         if (pengList[i].PokerList[j].pos != seatPos)
                         {
                             isDim = true;
                         }
                         go.SetParent(m_PengPokerContainer);
                         go.transform.localPosition = new Vector3(currentX, 0f, 0f);
                         currentX += handCellSize.x;
                         item.SetUI(pengList[i].PokerList[j], isHu, isBao, isMa, isUniversal, isPeng, isDim);
                     }
                     currentX += 10f;
                 }
             }

#if IS_GUGENG
             List<Poker> jin = new List<Poker>();
             for (int i = 0; i < desktopPoker.Count; ++i)
             {
                 if (MahJongHelper.CheckUniversal(desktopPoker[i], universal))
                 {
                     jin.Add(desktopPoker[i]);
                 }
             }

             for (int i = 0; i < jin.Count; ++i)
             {
                 GameObject go = Instantiate(prefab);
                 UIItemMahjong item = go.GetComponent<UIItemMahjong>();
                 bool isDim = false;
                 bool isHu = false;
                 bool isBao = false;
                 bool isMa = false;
                 bool isUniversal = true;
                 bool isPeng = true;
                 item.SetSize(handCellSize);
                 go.SetParent(m_PengPokerContainer);
                 go.transform.localPosition = new Vector3(currentX, 0f, 0f);
                 currentX += handCellSize.x;
                 item.SetUI(jin[i], isHu, isBao, isMa, isUniversal, isPeng, isDim);
             }
#endif

#if IS_HONGHU || IS_TAILAI//鸿鹄 赢家在抓马位置上显示宝牌
        if (isWiner && luckPoker != null && luckPoker.color != 0)
        {
            GameObject go = Instantiate(prefab);
            UIItemMahjong item = go.GetComponent<UIItemMahjong>();
            item.SetUI(luckPoker,false,true,false,false,false,false);
            go.SetParent(m_ProbContainer);
        }
#endif
             if (isWiner && prob != null)
             {
                 if (prob.Count == 1)
                 {
                     GameObject go = Instantiate(prefab);
                     UIItemMahjong item = go.GetComponent<UIItemMahjong>();
                     item.SetUI(prob[0], false, false, true, false, false, false);
                     go.SetParent(m_ProbContainer);
                 }
                 else if (prob.Count > 1)
                 {
                     for (int i = 0; i < prob.Count; ++i)
                     {
                         Poker p = prob[i];
#if IS_LAOGUI
                    
                    if ((hasFeng && (((seatPos == bankerPos && ((p.color < 4 && (p.size == 1 || p.size == 5 || p.size == 9)) || (p.color == 4 && p.size == 1) || (p.color == 5 && p.size == 1))))
                        || ((seatPos - bankerPos == 1 || seatPos - bankerPos == -3) && ((p.color < 4 && (p.size == 2 || p.size == 6)) || (p.color == 4 && p.size == 2) || (p.color == 5 && p.size == 2)))
                        || ((seatPos - bankerPos == 2 || seatPos - bankerPos == -2) && ((p.color < 4 && (p.size == 3 || p.size == 7)) || (p.color == 4 && p.size == 3) || (p.color == 5 && p.size == 3)))
                        || ((seatPos - bankerPos == 3 || seatPos - bankerPos == -1) && ((p.color < 4 && (p.size == 4 || p.size == 8)) || (p.color == 4 && p.size == 4)))))
                        || (!hasFeng && (p.color < 4 && ((p.size == 1 || p.size == 5 || p.size == 9)) || (p.color == 5 && p.size == 1))))
#else
                         if ((p.color < 4 && (p.size == 1 || p.size == 5 || p.size == 9)) || (p.color == 5 && p.size == 1))
#endif
                         {
                             GameObject go = Instantiate(prefab);
                             UIItemMahjong item = go.GetComponent<UIItemMahjong>();
                             item.SetUI(p, false, false, true, false, false, false);
                             go.SetParent(m_ProbContainer);
                         }
                     }
                 }
             }
         });


        if (m_ImgLaZi != null)
        {
            m_ImgLaZi.gameObject.SetActive(isLaZi);
        }

        if (m_ArrDirection != null && m_ArrDirection.Length > 0)
        {
            for (int i = 0; i < m_ArrDirection.Length; ++i)
            {
                m_ArrDirection[i].gameObject.SetActive(i + 1 == direction);
            }
        }
    }

    private void OnAvatarLoadFinish(Texture2D tex)
    {
        if (m_Head != null)
        {
            m_Head.texture = tex;
        }
    }
}
