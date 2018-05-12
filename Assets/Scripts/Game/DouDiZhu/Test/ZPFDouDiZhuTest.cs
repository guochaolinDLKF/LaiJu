//===================================================
//Author      : DRB
//CreateTime  ：12/29/2017 10:29:10 AM
//Description ：
//===================================================
using DRB.DouDiZhu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZPFDouDiZhuTest : MonoBehaviour
{
    [SerializeField]
    private List<Poker> lst;
    private int posY = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnGUI()
    {

        posY = 0;
        if (GUI.Button(new Rect(1, posY, 80, 30), "牌型测试"))
        {
            DeckTest();
        }

        posY += 30;

        if (GUI.Button(new Rect(1, posY, 80, 30), "牌型测试2"))
        {
            DeckTest2();
        }
        posY += 30;

        if (GUI.Button(new Rect(1, posY, 80, 30), "所有牌型"))
        {
            AllDect();
        }

        posY += 30;

        if (GUI.Button(new Rect(1, posY, 120, 30), "获取最长的顺子"))
        {
            LonestABCDE();
        }

        posY += 30;

        if (GUI.Button(new Rect(1, posY, 100, 30), "更大的组合"))
        {
            GetStrongDeck();
        }

        posY += 30;

        if (GUI.Button(new Rect(1, posY, 100, 30), "A是否大于B"))
        {
            isBigerDeck();
        }

    }
    #region InitPoker 初始化牌
    /// <summary>
    /// 初始化牌
    /// </summary>
    /// <returns></returns>
    private List<Poker> InitPoker()
    {
        List<Poker> pokers = new List<Poker>();

        #region Old
        //2 2 1 12 12 11 10 9 9 8 7 7 7 6 6 6 6 4 3 3 
        //pokers.Add(new Poker(0, 1, 3));
        //pokers.Add(new Poker(0, 2, 3));
        //pokers.Add(new Poker(0, 4, 4));
        //pokers.Add(new Poker(0, 3, 6));
        //pokers.Add(new Poker(0, 2, 6));
        //pokers.Add(new Poker(0, 2, 6));
        //pokers.Add(new Poker(0, 2, 6));
        //pokers.Add(new Poker(0, 2, 7));
        //pokers.Add(new Poker(0, 2, 7));
        //pokers.Add(new Poker(0, 1, 7));
        //pokers.Add(new Poker(0, 2, 8));
        //pokers.Add(new Poker(0, 4, 9));
        //pokers.Add(new Poker(0, 3, 9));
        //pokers.Add(new Poker(0, 2, 10));
        //pokers.Add(new Poker(0, 2, 11));
        //pokers.Add(new Poker(0, 2, 12));
        //pokers.Add(new Poker(0, 2, 12));
        //pokers.Add(new Poker(0, 2, 1));
        //pokers.Add(new Poker(0, 2, 2));
        //pokers.Add(new Poker(0, 2, 2));

        //87776664
        //pokers.Add(new Poker(0, 1, 8));
        //pokers.Add(new Poker(0, 2, 7));
        //pokers.Add(new Poker(0, 4, 7));
        //pokers.Add(new Poker(0, 3, 7));
        //pokers.Add(new Poker(0, 2, 6));
        //pokers.Add(new Poker(0, 2, 6));
        //pokers.Add(new Poker(0, 2, 6));
        //pokers.Add(new Poker(0, 2, 4));

        //JJQQKKAA
        //pokers.Add(new Poker(0, 1, 11));
        //pokers.Add(new Poker(0, 2, 11));
        //pokers.Add(new Poker(0, 4, 12));
        //pokers.Add(new Poker(0, 3, 12));
        //pokers.Add(new Poker(0, 2, 13));
        //pokers.Add(new Poker(0, 2, 13));
        //pokers.Add(new Poker(0, 2, 1));
        //pokers.Add(new Poker(0, 2, 1));

        //333 4444 555
        //pokers.Add(new Poker(0, 1, 3));
        //pokers.Add(new Poker(0, 2, 3));
        //pokers.Add(new Poker(0, 4, 3));
        //pokers.Add(new Poker(0, 3, 4));
        //pokers.Add(new Poker(0, 2, 4));
        //pokers.Add(new Poker(0, 2, 4));
        //pokers.Add(new Poker(0, 2, 4));
        //pokers.Add(new Poker(0, 2, 5));
        //pokers.Add(new Poker(0, 2, 5));
        //pokers.Add(new Poker(0, 2, 5));

        //333 4444 5
        //pokers.Add(new Poker(0, 1, 3));
        //pokers.Add(new Poker(0, 2, 3));
        //pokers.Add(new Poker(0, 4, 3));
        //pokers.Add(new Poker(0, 3, 4));
        //pokers.Add(new Poker(0, 2, 4));
        //pokers.Add(new Poker(0, 2, 4));
        //pokers.Add(new Poker(0, 2, 4));
        //pokers.Add(new Poker(0, 2, 5));

        //KKK QQQ 99
        //pokers.Add(new Poker(0, 2, 9));
        //pokers.Add(new Poker(0, 2, 9));
        //pokers.Add(new Poker(0, 3, 12));
        //pokers.Add(new Poker(0, 2, 12));
        //pokers.Add(new Poker(0, 2, 12));
        //pokers.Add(new Poker(0, 1, 13));
        //pokers.Add(new Poker(0, 2, 13));
        //pokers.Add(new Poker(0, 4, 13));

        // 小王 22AKKKQJJJ109876543
        //pokers.Add(new Poker(0, 1, 13));
        //pokers.Add(new Poker(0, 2, 2));
        //pokers.Add(new Poker(0, 4, 2));
        //pokers.Add(new Poker(0, 3, 1));
        //pokers.Add(new Poker(0, 2, 13));
        //pokers.Add(new Poker(0, 2, 13));
        //pokers.Add(new Poker(0, 2, 13));
        //pokers.Add(new Poker(0, 2, 12));
        //pokers.Add(new Poker(0, 2, 11));
        //pokers.Add(new Poker(0, 2, 11));
        //pokers.Add(new Poker(0, 2, 11));
        //pokers.Add(new Poker(0, 2, 10));
        //pokers.Add(new Poker(0, 2, 10));
        //pokers.Add(new Poker(0, 2, 9));
        //pokers.Add(new Poker(0, 2, 8));
        //pokers.Add(new Poker(0, 2, 7));
        //pokers.Add(new Poker(0, 2, 6));
        //pokers.Add(new Poker(0, 2, 5));
        //pokers.Add(new Poker(0, 2, 4));
        //pokers.Add(new Poker(0, 2, 3)); 

        //pokers.Add(new Poker(3, 3));
        //pokers.Add(new Poker(1, 4));
        //pokers.Add(new Poker(2, 4));
        //pokers.Add(new Poker(3, 4));
        //pokers.Add(new Poker(1, 6));
        //pokers.Add(new Poker(2, 6));
        //pokers.Add(new Poker(1, 7));
        //pokers.Add(new Poker(2, 7));
        #endregion

        //pokers.Add(new Poker(0, 1, 3));
        //pokers.Add(new Poker(0, 3, 4));


        pokers.Add(new Poker(0, 1, 1));
        pokers.Add(new Poker(0, 1, 13));


        //pokers.Add(new Poker(0, 3, 5));
        pokers.Add(new Poker(0, 4, 8, true));
        pokers.Add(new Poker(0, 3, 8, true));
        pokers.Add(new Poker(0, 2, 8, true));
        pokers.Add(new Poker(0, 1, 8, true));
        //pokers.Add(new Poker(0, 4, 8, true));

        //pokers.Add(new Poker(0, 3, 10));
        //pokers.Add(new Poker(0, 3, 10));
        //pokers.Add(new Poker(0, 3, 10));
        //pokers.Add(new Poker(0, 3, 11));
        //pokers.Add(new Poker(0, 3, 12));
        if (lst.Count != 0)
        {
            return lst;
        }

        return pokers;
    }
    #endregion

    #region DeckTest 牌型测试
    /// <summary>
    /// 牌型测试
    /// </summary>
    private void DeckTest()
    {
        Deck deck = DouDiZhuHelper.Check(InitPoker());
        if (deck == null)
        {
            Debug.Log("空");
        }
        else
        {
            Debug.Log(deck.type);
            for (int i = 0; i < deck.pokers.Count; i++)
            {
                Debug.Log(deck.pokers[i].ToString());
            }
            Debug.Log(string.Format("main:{0}", deck.mainPoker.ToString()));
        }
    }
    #endregion

    #region DeckTest 牌型测试
    /// <summary>
    /// 牌型测试
    /// </summary>
    private void DeckTest2()
    {
       
        List<Deck> lstDeck = DouDiZhuHelper.Check2(InitPoker());
        if (lstDeck.Count == 0)
        {
            Debug.Log("空");
        }
        else
        {
            for (int i = 0; i < lstDeck.Count; i++)
            {
                Debug.Log(lstDeck[i].type);
                for (int j = 0; j < lstDeck[i].pokers.Count; j++)
                {
                    Debug.Log(lstDeck[i].pokers[j].ToString());
                }
            }
        }
    }
    #endregion

    #region AllDect 获取所有牌型测试
    /// <summary>
    /// 获取所有牌型测试
    /// </summary>
    private void AllDect()
    {
        List<Deck> decks = DouDiZhuHelper.GetAllDeck(InitPoker());
        for (int i = 0; i < decks.Count; i++)
        {
            //if (decks[i].type == DeckType.AA)
            //{
            //    Debug.Log("type:" + decks[i].type);
            //    for (int j = 0; j < decks[i].pokers.Count; j++)
            //    {
            //        Debug.Log(decks[i].pokers[j].ToString());
            //    }
            //    Debug.Log("==================================");
            //}
            if (decks[i].type == DeckType.AABBCC)
            {
                Debug.Log("type:" + decks[i].type);
                for (int j = 0; j < decks[i].pokers.Count; j++)
                {
                    Debug.Log(decks[i].pokers[j].ToString());
                }
                Debug.Log("main:" + decks[i].mainPoker.ToString());
                Debug.Log("==================================");
            }
            //Debug.Log("type:" + decks[i].type);
        }
    }
    #endregion

    #region LonestABCDE 最长的顺子
    /// <summary>
    /// 最长的顺子
    /// </summary>
    private void LonestABCDE()
    {
        Deck deck = DouDiZhuHelper.GetLongestABCED(InitPoker());
        for (int i = 0; i < deck.pokers.Count; i++)
        {
            Debug.Log(deck.pokers[i].ToString());
        }
    }
    #endregion

    private void GetStrongDeck()
    {
        List<Poker> lstPrePoker = new List<Poker>();
        lstPrePoker.Add(new Poker(0, 4));
        lstPrePoker.Add(new Poker(0, 4));
        lstPrePoker.Add(new Poker(0, 5));
        lstPrePoker.Add(new Poker(0, 5));
        lstPrePoker.Add(new Poker(0, 6));
        lstPrePoker.Add(new Poker(0, 6));
        lstPrePoker.Add(new Poker(0, 7));
        lstPrePoker.Add(new Poker(0, 7));

        Deck preDeck = new Deck(DeckType.AABBCC, lstPrePoker, new Poker(0, 7));

        List<Poker> lstHandPoker = new List<Poker>();
        lstHandPoker.Add(new Poker(0, 7));
        lstHandPoker.Add(new Poker(0, 7));
        lstHandPoker.Add(new Poker(0, 8));
        lstHandPoker.Add(new Poker(0, 8));
        lstHandPoker.Add(new Poker(0, 9));
        lstHandPoker.Add(new Poker(0, 9));
        //lstHandPoker.Add(new Poker(0, 10));
        //lstHandPoker.Add(new Poker(0, 10));
        List<Deck> lstDeck = DouDiZhuHelper.GetStrongerDeck(preDeck, lstHandPoker);
        Debug.LogWarning(lstDeck.Count);
    }
    private void isBigerDeck()
    {
        List<Poker> lstPrePoker = new List<Poker>();
        lstPrePoker.Add(new Poker(0, 5));
        lstPrePoker.Add(new Poker(0, 5));
        lstPrePoker.Add(new Poker(0, 5));
        lstPrePoker.Add(new Poker(0, 6));
        lstPrePoker.Add(new Poker(0, 6));
        lstPrePoker.Add(new Poker(0, 6));
        lstPrePoker.Add(new Poker(0, 6));
        lstPrePoker.Add(new Poker(0, 7));

        Deck preDeck = DouDiZhuHelper.Check(lstPrePoker);

        List<Poker> lstHandPoker = new List<Poker>();
        lstHandPoker.Add(new Poker(0, 9));
        lstHandPoker.Add(new Poker(0, 9));
        lstHandPoker.Add(new Poker(0, 12));
        lstHandPoker.Add(new Poker(0, 12));
        lstHandPoker.Add(new Poker(0, 12));
        lstHandPoker.Add(new Poker(0, 13));
        lstHandPoker.Add(new Poker(0, 13));
        lstHandPoker.Add(new Poker(0, 13));

        Deck newDeck = DouDiZhuHelper.Check(lstHandPoker);

        if (preDeck <= newDeck)
        {
            Debug.LogWarning("大于等于");
        }
        else
        {
            Debug.LogWarning("小于");
        }

    }
}
