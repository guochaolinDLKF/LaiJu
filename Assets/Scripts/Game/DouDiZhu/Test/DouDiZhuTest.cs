//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 2:37:26 PM
//Description ：
//===================================================
using UnityEngine;
using DRB.DouDiZhu;
using System.Collections.Generic;

public class DouDiZhuTest : MonoBehaviour
{
    private int posY = 0;

    private string playerId = "玩家Id";

    private string gold = "金币";

    private string pos = "座位号";

    private string nickname = "昵称";

    private string pokerCount = "牌数量";

    private List<Poker> lstPoker = new List<Poker>();

    private void Awake()
    {
        //for (int i = 0; i < lstPokerIndex.Length; ++i)
        //{
        //    lstPokerIndex[i] = string.Empty;
        //    lstPokerColor[i] = string.Empty;
        //    lstPokerSize[i] = string.Empty;
        //}


    }

    private void OnGUI()
    {
        posY = 0;

        playerId = GUI.TextField(new Rect(100, posY, 80, 30), playerId);
        gold = GUI.TextField(new Rect(200, posY, 80, 30), gold);
        nickname = GUI.TextField(new Rect(300, posY, 80, 30), nickname);
        pos = GUI.TextField(new Rect(400, posY, 80, 30), pos);
        pokerCount = GUI.TextField(new Rect(500, posY, 80, 30), pokerCount);

        for (int i = 0; i < pokerCount.ToInt(); ++i)
        {
            if (i >= lstPoker.Count)
            {
                Poker poker = new Poker(0,0,0);
                lstPoker.Add(poker);
            }
            lstPoker[i].index = GUI.TextField(new Rect(600, posY + i * 30, 80, 30), lstPoker[i].index.ToString()).ToInt();
            lstPoker[i].color = GUI.TextField(new Rect(700, posY + i * 30, 80, 30), lstPoker[i].color.ToString()).ToInt();
            lstPoker[i].size = GUI.TextField(new Rect(800, posY + i * 30, 80, 30), lstPoker[i].size.ToString()).ToInt();
        }
        for (int i = lstPoker.Count - 1; i >= pokerCount.ToInt(); --i)
        {
            lstPoker.RemoveAt(i);
        }

        if (GUI.Button(new Rect(1, posY, 80, 30), "创建房间"))
        {
            GameCtrl.Instance.CreateRoom(4);
        }

        posY += 30;

        if (GUI.Button(new Rect(1, posY, 80, 30), "进入房间"))
        {
            RoomProxy.Instance.EnterRoom(playerId.ToInt(), gold.ToInt(), string.Empty, 0, nickname, pos.ToInt());
        }

        posY += 30;

        if (GUI.Button(new Rect(1, posY, 80, 30), "离开房间"))
        {
            RoomProxy.Instance.ExitRoom(playerId.ToInt());
        }

        posY += 30;

        if (GUI.Button(new Rect(1, posY, 80, 30), "准备"))
        {
            RoomProxy.Instance.Ready(playerId.ToInt(), true);
        }

        posY += 30;

        if (GUI.Button(new Rect(1, posY, 80, 30), "取消准备"))
        {
            RoomProxy.Instance.Ready(playerId.ToInt(), false);
        }

        posY += 30;

        if (GUI.Button(new Rect(1, posY, 80, 30), "询问下注"))
        {
            new AskBetCommand(playerId.ToInt()).Execute();
        }

        posY += 30;

        if (GUI.Button(new Rect(1, posY, 80, 30), "下注"))
        {
            new BetCommand(playerId.ToInt(), gold.ToInt()).Execute();
        }

        posY += 30;

        if (GUI.Button(new Rect(1, posY, 80, 30), "发牌"))
        {
            //new BeginCommand(playerId.ToInt(), CopyPoker(lstPoker)).Execute();
        }

        posY += 30;

        //if (GUI.Button(new Rect(1, posY, 80, 30), "明牌"))
        //{
        //    RoomProxy.Instance.ShowPoker(playerId.ToInt(), CopyPoker(lstPoker));
        //}

        //posY += 30;

        if (GUI.Button(new Rect(1, posY, 80, 30), "询问出牌"))
        {
            //new AskPlayPokerCommand(playerId.ToInt()).Execute();
        }

        posY += 30;

        if (GUI.Button(new Rect(1, posY, 80, 30), "出牌"))
        {
            Deck deck = DouDiZhuHelper.Check(CopyPoker(lstPoker));
            new PlayPokerCommand(playerId.ToInt(), deck).Execute();
        }

        posY += 30;

        if (GUI.Button(new Rect(1, posY, 80, 30), "过"))
        {
            new PassCommand(playerId.ToInt()).Execute();
        }

        posY += 30;
    }

    #region DeckTest 牌型测试
    /// <summary>
    /// 牌型测试
    /// </summary>
    private void DeckTest()
    {
        List<Poker> pokers = new List<Poker>();
        pokers.Add(new Poker(1, 3));
        pokers.Add(new Poker(2, 3));
        pokers.Add(new Poker(3, 3));
        pokers.Add(new Poker(1, 4));
        pokers.Add(new Poker(2, 4));
        pokers.Add(new Poker(3, 4));
        pokers.Add(new Poker(1, 6));
        //pokers.Add(new Poker(2, 6));
        pokers.Add(new Poker(1, 7));
        //pokers.Add(new Poker(2, 7));
        Deck deck = DouDiZhuHelper.Check(pokers);
        Debug.Log(deck.type);
    }
    #endregion

    #region CreateRoomUnitTest 创建房间单元测试
    /// <summary>
    /// 创建房间单元测试
    /// </summary>
    public void CreateRoomUnitTest()
    {
        //    RoomEntity room = new RoomEntity()
        //    {
        //        roomId = 123456,
        //        currentLoop = 1,
        //        Times = 1,
        //        SeatCount = 3,
        //        baseScore = 1,
        //        gameId = GameCtrl.Instance.CurrentGameId,
        //        groupId = 0,
        //        maxLoop = 8,
        //        matchId = 0,
        //        SeatList = new List<SeatEntity>(),
        //    };
        //    for (int i = 1; i <= room.SeatCount; ++i)
        //    {
        //        SeatEntity seat = new SeatEntity();
        //        seat.PlayerId = i;//玩家ID      
        //        if (i == 1) seat.IsPlayer = true;
        //        seat.Nickname = "玩家" + i.ToString();
        //        seat.Avatar = string.Empty;
        //        seat.Gender = i % 2;
        //        seat.Gold = i;
        //        seat.Pos = i;
        //        seat.bet = 0;
        //        seat.pokerList = new List<Poker>();
        //        room.SeatList.Add(seat);
        //    }

        //    IGameCommand command = new CreateRoomCommand(room);
        //    DouDiZhuGameCtrl.Instance.CommandQueue.Enqueue(command);

        //    SceneMgr.Instance.LoadScene(SceneType.DouDZ);
    }
    #endregion

    private List<Poker> CopyPoker(List<Poker> pokers)
    {
        List<Poker> lst = new List<Poker>();
        for (int i = 0; i < lstPoker.Count; ++i)
        {
            Poker poker = new Poker(lstPoker[i].index, lstPoker[i].color, lstPoker[i].size);
            lst.Add(poker);
        }
        return lst;
    }
}
