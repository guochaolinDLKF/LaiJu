//===================================================
//Author      : WZQ
//CreateTime  ：7/11/2017 11:00:57 AM
//Description ：
//===================================================


namespace NiuNiu
{
    using System.Collections.Generic;

    using UnityEngine;

    using com.oegame.niuniu.protobuf;

    using niuniu.proto;

    //ö�� ��������������
    public enum sortRule
    {
        name = 1,          //����
        score,             //����

    }

    #region
    #endregion


    public enum UIAniType
    {
        UIAnimation_TongSha,//通杀
        UIAnimation_TongPei,//通赔
    }

    //================================================================================================================
    /// <summary>
    /// �˿���
    /// </summary>
    public class Poker
    {
        /// <summary>
        /// �˿����� ����ÿ����
        /// </summary>
        public int index;

        /// <summary>
        /// �Ƶ�Ȩֵ
        /// </summary>
        public int size;

        /// <summary>
        /// �ƵĻ�ɫ
        /// </summary>
        public int color;

        /// <summary>
        /// �Ƿ񷭿���״̬
        /// </summary>
        public NN_ENUM_POKER_STATUS status;

        public Poker() { }

        public Poker(int index, int size, int color, NN_ENUM_POKER_STATUS status)
        {
            this.index = index;
            this.size = size;
            this.color = color;
            this.status = status;

        }

        public Poker(Poker poker)
        {
            index = poker.index;
            size = poker.size;
            color = poker.color;
            status = poker.status;
        }

   

        public void SetPoker(NN_POKER poker)
        {
            if (poker.hasIndex())        this.index = poker.index;
            if (poker.hasColor())        this.color = poker.color;
            if (poker.hasSize())         this.size = poker.size;
            if (poker.hasPokerStatus())  this.status = poker.pokerStatus;

        }




        public void SetPoker(Poker poker)
        {

            this.index = poker.index;
            this.color = poker.color;
            this.size = poker.size;
            this.status = poker.status;

        }







        public override string ToString()
        {
            return string.Format("{0}_{1}", size, color);
        }




    }


    //================================================================================================================

    /// <summary>
    /// ��λ��
    /// </summary>
    public class Seat:SeatEntityBase
    {
        public enum SeatStatus
        {
            Idle,      //����
            Gameing     //��Ϸ��
        }


        public enum SeatDissolve
        {
            AGREE=1,//ͬ����ɢ
            DISAGREE=2,//��ͬ��
            NOP=3,//δ����

        }

        /// <summary>
        /// ����ʵ��
        /// </summary>
        public PlayerInfoEntity player;



        /// <summary>
        /// ����
        /// </summary>
        public List<NiuNiu.Poker> PokerList;


        /// <summary>
        /// ��������
        /// </summary>
        public int PockeType;

        ///// <summary>
        ///// ����
        ///// </summary>
        //public int Index;

        /// <summary>
        /// �Ƿ�׼��
        /// </summary>
        public bool IsReady;

        /// <summary>
        /// ����ʱ
        /// </summary>
        public long Countdown; //����δʹ�ã�


        /// <summary>
        /// �Ƿ�ͬ����ɢ ��1 ͬ�� 2��ͬ�� 3δ������
        /// </summary>
        //public SeatDissolve Dissolve;
        public NN_ENUM_SEAT_DISSOLVE Dissolve;
   
        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public bool IsHomeowners;


        /// <summary>
        /// ����
        /// </summary>
        public int Earnings;


        /// <summary>
        /// �Ƿ���ʤ����
        /// </summary>
        public bool Winner;

        /// <summary>
        /// ������Ϸ��
        /// </summary>
        public int settle;


        /// <summary>
        /// ��ע����
        /// </summary>           
        public int Pour;
        /// <summary>
        /// ���Ƴ���
        /// </summary>
        private const int PokerCount=5;

        public  const int ROOM_POKERLIST_COUNT = 5;

        /// <summary>
        /// 是否已经抢庄 -1不能抢庄 0无响应  1 抢庄  2不抢
        /// </summary>
        public int isAlreadyHOG = 0;

        public Seat()
        {
            PokerList = new List<Poker>();
            //IncomesList = new List<PokerGroup>();
        }

    

        public void SetSeat(Seat seat)
        {
            //if (true)
            //{
            //player = seat.player;

            //}
            IsHomeowners = seat.IsHomeowners;

            PlayerId = seat.PlayerId;

            Nickname = seat.Nickname;

            Avatar = seat.Avatar;
            IsReady = seat.IsReady;
            Gender = seat.Gender;

            Gold = seat.Gold;
            Earnings = seat.Earnings;

           
             Winner = seat.Winner;
          

            PokerList = new List<NiuNiu.Poker>();
            if (seat.PokerList.Count > 0)
            {
                for (int i = 0; i < PokerCount; i++)
                {
                    NiuNiu.Poker poker = new Poker();
                    if (seat.PokerList[i] != null)
                    {
                        poker.SetPoker(seat.PokerList[i]);            
                    }

                        PokerList.Add(poker);

                }
            }

            if (seat.PokerList.Count==0)
            {
                for (int i = 0; i < PokerList.Count; i++)
                {
                    NiuNiu.Poker poker = new Poker();
                    PokerList[i].SetPoker(poker);
                }
            }


            PockeType = seat.PockeType;

            Pos = seat.Pos;

            //Countdown = pbSeat.Countdown;

          
                Dissolve = seat.Dissolve;
          
            IsBanker = seat.IsBanker;


        }


        #region SetSeat

        public void SetSeat(NN_SEAT prSeat)
        {


            //����
            if (prSeat.hasIsHomeowners())
            {
                IsHomeowners = prSeat.IsHomeowners;
            }
            //�Ƿ�ͬ����ɢ
            if (prSeat.hasDissolve())
            {
                Dissolve = prSeat.dissolve;
            }
            //ׯ
            if (prSeat.hasIsBanker())
            {
                IsBanker = prSeat.isBanker;
            }

            //����ID
            if (prSeat.hasPlayerId())
            {
                PlayerId = prSeat.playerId;
            }

            //�ǳ�
            if (prSeat.hasNickname())
            {
                Nickname = prSeat.nickname;

            }
            //ͷ��
            if (prSeat.hasAvatar())
            {
                Avatar = prSeat.avatar;
            }

            //�Ա�
            if (prSeat.hasGender())
            {
                Gender = prSeat.gender;

            }

            if (prSeat.hasPos())
            {
                Pos = prSeat.pos;
            }

            if (prSeat.hasReady())
            {
                IsReady = prSeat.ready;
            }


            //���н���
            if (prSeat.hasGold())
            {
                //if (pBRoom.SeatList[i].Gold != 0)
                //{

                Gold = prSeat.gold;
                //}
            }
            //��������
            if (prSeat.hasNnEarnings())
            {
                Earnings = prSeat.nn_earnings;
            }
            //�Ƿ���ʤ����
            if (prSeat.hasIsWiner())
            {
                Winner = prSeat.isWiner;
            }
            //��ע
            if (prSeat.hasPour())
            {
                Pour = prSeat.pour;
            }

            //��������
            if (prSeat.hasNnPokerType())
            {

                if (prSeat.nn_pokerType != 0)
                {
                    PockeType = prSeat.nn_pokerType;
                    //PockeType = (int)prSeat.nn_pokerType;

                }

            }

            //��������
            
            List<NN_POKER> prPokerList = prSeat.getNnPokerList();
            if (prPokerList != null && prPokerList.Count > 0)
            {



                for (int j = 0; j < prPokerList.Count; j++)
                {

                    if (prPokerList[j] != null && prPokerList[j].hasIndex() && prPokerList[j].index != 0)
                    {
                        PokerList[j].SetPoker(prPokerList[j]);

                    }

                }

            }

             //纬度
            if (prSeat.hasLatitude())
            {
                Latitude = prSeat.latitude;
            }
            //经度
            if (prSeat.hasLongitude())
            {
                Longitude = prSeat.longitude;
            }

        }
        #endregion



    }






    //================================================================================================================
    /// <summary>
    /// ������
    /// </summary>

    public class Room: RoomEntityBase
    {

   

        public enum RoomModel
        {

            AutoBanker=1,//服务器自动指定庄（没牛下庄 轮庄）
            AbdicateBanker = 2,//让庄抢庄 （固定庄）
            robBanker=3,//抢庄
            EveryTime=4,//每局必轮
            WinnerByBanker,//连庄（胜者坐庄）
        }
        public enum SuperModel
        {
            CommonRoom = 1,//普通场
            PassionRoom = 2,//激情场

        }

        private const int ROOM_SEAT_COUNT = 6;

        //����ģʽ

        public RoomModel roomModel=  RoomModel.AutoBanker;

        /// <summary>
        /// 普通 激情场
        /// </summary>
        public SuperModel superModel = SuperModel.CommonRoom;

        /// <summary>
        /// 座位长度
        /// </summary>
        public int SeatCount;



        ///  /// <summary>
        /// 房间状态
        /// </summary>
        public NN_ENUM_ROOM_STATUS roomStatus;

        ///// <summary>
        ///// 
        ///// </summary>
        //public int Loop;

        ///// <summary>
        ///// �ܾ���
        ///// </summary>
        //public int MaxLoop;

        /// <summary>
        /// 座位列表
        /// </summary>
        public List<NiuNiu.Seat> SeatList;

        /// <summary>
        /// ��ʼʱ��
        /// </summary>
        public long BeginTime;

        /// <summary>
        /// 时间戳
        /// </summary>
        public long serverTime;



        /// <summary>
        ///底分
        /// </summary> 
        public int BaseScore;

        /// <summary>
        /// 
        /// </summary>
        public List<NiuNiu.PokerGroup> Incomes;

        /// <summary>
        /// 当前抢庄座位
        /// </summary>
        public Seat RobBankerSeat = null;

        public Room() { }

       


        #region   SetRoom(PBRoom pBRoom)
        public void SetRoom(NN_ROOM prRoom)
        {
           
            if (prRoom.hasNnRoomStatus())
            {
                roomStatus = prRoom.nn_room_status;
            }

            if (prRoom.hasLoop())
            {
                currentLoop = prRoom.loop;
            }

            if (prRoom.hasMaxLoop())
            {
                maxLoop = prRoom.maxLoop;
            }

            if (prRoom.hasUnixtime())
            {
                serverTime = prRoom.unixtime;
            }


           
           
            List<NN_SEAT> prPokerList = prRoom.getNnSeatList();
            for (int i = 0; i < prPokerList.Count; i++)
            {


                if (prPokerList[i] != null && (prPokerList[i].pos > 0 || prPokerList[i].playerId > 0))//&& pBRoom.SeatList[i].Pos== CurrentRoom.SeatList[i].Pos)
                {

                    //--------------1-----------------
                    for (int j = 0; j < SeatList.Count; j++)
                    {
                        if (prPokerList[i].pos == SeatList[j].Pos)
                        {
                            SeatList[j].SetSeat(prPokerList[i]);
                            break;
                        }
                    }
                    //------------2-------------------
                    //Syn(pBRoom.SeatList[i]);

                }


            }


        



        }

 #endregion




    }

    //=============================================================================================================
    //player��
    public class PlayerInfoEntity
    {

        /// <summary>
        /// ����ID
        /// </summary>
        public int PlayerId;
        /// <summary>
        /// �Ƿ��Ƿ���
        /// </summary>
        public bool MatchId;


        /// <summary>
        /// �����ǳ�
        /// </summary>
        public string Nickname;

        /// <summary>
        /// �Ա�
        /// </summary>
        public int Gender;

        /// <summary>
        /// ͷ��
        /// </summary>
        public string Avatar;

        /// <summary>
        /// ����
        /// </summary>
        public int Gold;

        /// <summary>
        /// ��ע
        /// </summary>
        public int Pour;

        /// <summary>
        /// ʱ����
        /// </summary>
        public long Unixtime;


        /// <summary>
        /// ��������
        /// </summary>
        public int Cards;


        public PlayerInfoEntity() { }
        public PlayerInfoEntity(PBPlayer pbPlayer)
        {

            if (pbPlayer.HasPlayerId)
            {
                PlayerId = pbPlayer.PlayerId;
            }
            if (pbPlayer.HasMatchId)
            {
                MatchId = pbPlayer.MatchId;
            }


            if (pbPlayer.HasNickname)
            {
                Nickname = pbPlayer.Nickname;
            }

            if (pbPlayer.HasGender)
            {
                Gender = pbPlayer.Gender;
            }
            if (pbPlayer.HasAvatar)
            {
                Avatar = pbPlayer.Avatar;
            }

            if (pbPlayer.HasGold)
            {
                Gold = pbPlayer.Gold;
            }

            //---------------�ļ�δ����--------------------------
            //if (pbPlayer.HasPour)
            //{
            //    Pour = pbPlayer.Pour;
            //}
            //---------------�ļ�δ����--------------------------

            if (pbPlayer.HasUnixtime)
            {
                Unixtime = pbPlayer.Unixtime;
            }

            //����
            if (pbPlayer.HasCards)
            {
                Cards = pbPlayer.Cards;
            }


        }


    }
    //================================================================================================================
 
    //================================================================================================================
    public class Error
    {
        /// <summary>
        /// ����ID
        /// </summary>
        public int Code;

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public string Msg;
        public Error() { }

        public Error(PBError pBError)
        {
            if (pBError.HasCode)
            {
                Code = pBError.Code;
            }
            if (pBError.HasMsg)
            {
                Msg = pBError.Msg;
            }

        }


    }



    //================================================================================================================

    public class PokerGroup
    {

        public PokerGroup() { }


        //public PokerGroup(PBPokerGroup pBPokerGroup) { }


    }



}

