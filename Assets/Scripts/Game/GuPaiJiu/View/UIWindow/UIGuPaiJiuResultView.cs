//===================================================
//Author      : DRB
//CreateTime  ：9/14/2017 1:59:14 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using proto.gp;
using GuPaiJiu;
using UnityEngine.UI;

public class UIGuPaiJiuResultView : UIWindowViewBase
{
    [SerializeField]
    private GameObject RankingObjH;//黄条
    [SerializeField]
    private GameObject RankingObjLan;//蓝条
    [SerializeField]
    private Transform Tran;//挂在点
    [SerializeField]
    private Sprite[] spriteS;//排行图片
    [SerializeField]
    private UIGuPaiJiuItemlResult[] results;



    private List<SeatEntity> seatList;


    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnfanhui":
                SendNotification("ReturnHall");
                break;
            case "btnXuanYao":
                SendNotification(ConstantGuPaiJiu.OnBtnResultViewGuPaiJiuShareClick);
                break;
        }
    }



    /// <summary>
    /// 解散房间的时候结算
    /// </summary>
    /// <param name="proto"></param>
    public void SetUI(GP_ROOM_TOTALSETTLE proto)
    {
        seatList = new List<SeatEntity>();
        for (int i = 0; i < proto.room.seatListCount(); i++)
        {
            GP_SEAT op_seat = proto.room.getSeatList(i);
            if (op_seat.playerId == 0) continue;           
            SeatEntity seat = new SeatEntity();
            seat.PlayerId = op_seat.playerId;//玩家ID            
            seat.Nickname = op_seat.nickname;//玩家名字            
            seat.Avatar = op_seat.avatar;//玩家头像
            seat.Gender = op_seat.gender;//玩家性别            
            seat.Gold = op_seat.gold;//底分                     
            seat.Pos = op_seat.pos;//座位位置 
            seat.pokerList = new List<Poker>();
            for (int j = 0; j < op_seat.maxPokerListCount(); j++)
            {
                GP_POKER op_Poker = op_seat.getMaxPokerList(i);     
                seat.pokerList.Add(new Poker()
                {
                    Index = op_Poker.index,//索引
                    Type = op_Poker.type,//花色
                    Size = op_Poker.size,//大小                                     
                });
            }                                      
            seatList.Add(seat);
        }
        SeatSort(seatList);
#if IS_CHUANTONGPAIJIU
         LoadRanking(seatList);
#else
        SetResult(seatList);
#endif

    }


    /// <summary>
    /// 正常游戏下设置排行
    /// </summary>
    /// <param name="proto"></param>
    public void SetUI(GP_ROOM_GAMEOVER proto)
    {
        seatList = new List<SeatEntity>();       
        for (int i = 0; i < proto.room.seatListCount(); i++)
        {
            GP_SEAT op_seat = proto.room.getSeatList(i);
            if (op_seat.playerId == 0) continue;
            SeatEntity seat = new SeatEntity();
            seat.PlayerId = op_seat.playerId;//玩家ID            
            seat.Nickname = op_seat.nickname;//玩家名字
            seat.Pos = op_seat.pos;            
            seat.Avatar = op_seat.avatar;//玩家头像
            seat.Gender = op_seat.gender;//玩家性别            
            seat.Gold = op_seat.gold;//底分                     
            seat.Pos = op_seat.pos;//座位位置   
            seat.pokerList = new List<Poker>();         
            for (int j = 0; j < op_seat.maxPokerListCount(); j++)
            {
                GP_POKER op_Poker = op_seat.getMaxPokerList(j);         
                seat.pokerList.Add(new Poker()
                {
                    Index = op_Poker.index,//索引
                    Type = op_Poker.type,//花色
                    Size = op_Poker.size,//大小                                     
                });
            }
            seatList.Add(seat);
        }
        SeatSort(seatList);
#if IS_CHUANTONGPAIJIU
         LoadRanking(seatList);
#else
        SetResult(seatList);
#endif

    }


    private void SetResult(List<SeatEntity> seatList)
    {
        for (int i = 0; i < seatList.Count; i++)
        {
            results[i].SetUI(seatList[i]);
        }
    }



   /// <summary>
   /// 加载排行条
   /// </summary>
   /// <param name="seatList"></param>
    private void LoadRanking(List<SeatEntity> seatList)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i%2==0)
            {
                GameObject go = Instantiate(RankingObjH);
                go.SetActive(true);
                if (i>seatList.Count-1) go.GetComponent<UIItemGuPaiJiuRanking>().SetUI(null, null);
                else go.GetComponent<UIItemGuPaiJiuRanking>().SetUI(seatList[i], spriteS[i]);
                go.SetParent(Tran);
            }
            else
            {
                GameObject go = Instantiate(RankingObjLan);
                go.SetActive(true);
                if (i > seatList.Count - 1) go.GetComponent<UIItemGuPaiJiuRanking>().SetUI(null, null);
                else go.GetComponent<UIItemGuPaiJiuRanking>().SetUI(seatList[i], spriteS[i]);
                go.SetParent(Tran);
            }            
        }
    }


    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="seatList"></param>
    private void SeatSort(List<SeatEntity> seatList)
    {
        seatList.Sort((SeatEntity seat1,SeatEntity seat2)=> 
        {
            if (seat1.Gold > seat2.Gold)
            {
                return -1;
            }
            else return 1;
        });
    }
}
