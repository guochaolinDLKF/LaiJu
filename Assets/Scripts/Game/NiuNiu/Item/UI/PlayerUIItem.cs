//===================================================
//Author      : WZQ
//CreateTime  ：5/10/2017 9:33:05 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using niuniu.proto;
/// <summary>
/// 控制单个玩家ItemUI显示
///         脚本挂载到每个玩家Item上
/// </summary>

namespace NiuNiu
{
    public class PlayerUIItem : MonoBehaviour
    {
        private int m_SeatPos;
        [SerializeField]
        protected int m_nSeatIndex = -1;
        public Image _avatar;                    //玩家头像
        public RawImage _avatarTexture;          //玩家头像
        private Texture _defaultAvatarTexture;         //默认玩家头像
        private int _gender;                        //性别
        public Text _nickname;                  //昵称

        public Image _isReadyTo;               //准备好了

        [SerializeField]
        private Image[] m_isRobBanker;           //是否抢庄

        public Image _banker;                  //庄家标识图片父(呼吸显示)

        public Image _banker_zhuang;           //庄小图

        public Transform m_UIHintLight;           //提示灯
        private bool isUIHintLight = false;//提示灯是否开启
        private float HintLightInterval = 0.2f;//提示灯闪烁间隔

        public Image _baseScore;               //下注分数

        public Image _pockeType;               //开牌手牌型

        public PokerCtrl[] _pokersList;            //5张手牌

        public Text _gold;                   //积分 金币

        public NiuNiuEarningsUI _earningsAni;  //收益动画
        //------------------------------------------------------------------------
        [SerializeField]
        private Transform m_drawPokerPos;//发牌点
        [SerializeField]
        private Transform m_inflexionPos;//发牌拐点
        private Tweener[] m_drawPokerAni = new Tweener[5];//发牌到拐点
        private Tweener[] m_returnPokerAni = new Tweener[5];//还原位
        //private Tweener[] m_ShowPokerAni = new Tweener[5];//翻开Poker
        private Vector3 UIPokerShow = new Vector3(180, 0, 0);
        private Vector3 UIPokerHide = Vector3.zero;
        private bool isPlayDrawPokerAni = false;//是否在播放发牌动画

        //-----------------------------------------------------------------------
        public RectTransform[] _haveNiuPos;     //有牛时位置 

        private Vector3[] _initPokerPos;       //初始5张牌位置

        private bool _seatWhetherNiu = false;    //该座位是否已经有牛       

        //public RectTransform DealPokerParent;       //发牌动画 牌的父物体
        //public GameObject DealPokerTemplate;       //发牌动画牌模板

        private Tween myPokerTypeSequence;//牌型动画
        [SerializeField]
        private Tween mySequenceConfirmBankerAni;//换庄动画
        
        public Room.SuperModel superModel = Room.SuperModel.CommonRoom;
        void Awake()
        {
            if (_avatarTexture != null) _defaultAvatarTexture = _avatarTexture.texture;
            ModelDispatcher.Instance.AddEventListener(ConstDefine_NiuNiu.ObKey_SeatInfoChanged, OnSeatInfoChanged);

            //MahJongManager
            Button btn = _avatarTexture.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(OnHeadClick);
            }

            //Poker位置
            if (_initPokerPos == null)
            {
                _initPokerPos = new Vector3[_pokersList.Length];
                for (int i = 0; i < _pokersList.Length; i++)
                {
                    _initPokerPos[i] = _pokersList[i].transform.position;
                }
            }


            myPokerTypeSequence = _pockeType.rectTransform.DOScale(1f, 0.5f).SetAutoKill(false).Pause();
            mySequenceConfirmBankerAni = _banker_zhuang.rectTransform.DOScale(3f, 0.2f).SetLoops(4,LoopType.Yoyo).SetAutoKill(false).Pause();
            _banker_zhuang.rectTransform.localScale = Vector3.one ;
            _pockeType.rectTransform.localScale = Vector3.one ;

            m_UIHintLight.gameObject.SetActive(false);
            for (int i = 0; i < _pokersList.Length; i++)
            {
                m_drawPokerAni[i] = _pokersList[i].transform.DOMove(m_inflexionPos.position, 0.25f).SetEase(Ease.Linear).SetAutoKill(false).Pause();
                m_returnPokerAni[i] = _pokersList[i].transform.DOMove(_pokersList[i].transform.position, 0.2f).SetEase(Ease.Linear).SetAutoKill(false).Pause();
                //m_ShowPokerAni[i] = _pokersList[i].transform.DOLocalRotate(UIPokerShow, 0.5f).SetEase(Ease.Linear).SetAutoKill(false).Pause();
            }


        }
 
        #region

        //private void Awake()
        //{
        //    tweener = m_UIEarningsText.transform.DOLocalMoveY(m_UIEarningsText.transform.localPosition.y + 200, 1.5f).SetEase(Ease.Flash).SetAutoKill(false).Pause();
        //    ModelDispatcher.Instance.AddEventListener(ConstDefine_JuYou.ObKey_SeatInfoChanged, OnSeatInfoChanged);
        //    gameObject.SetActive(false);
        //    m_UIHintLight.gameObject.SetActive(false);
        //}

        private void OnDestroy()
        {
            ModelDispatcher.Instance.RemoveEventListener(ConstDefine_NiuNiu.ObKey_SeatInfoChanged, OnSeatInfoChanged);
        }

        private void OnHeadClick()
        {       
            AudioEffectManager.Instance.Play("btnclick", Vector3.zero, false);

            Debug.Log(string.Format("点击的玩家Pos:{0}", m_SeatPos));
            UIDispatcher.Instance.Dispatch(ConstDefine_NiuNiu.ObKey_OnNiuNiuViewHeadClick,new object[] { m_SeatPos } );
            //SendNotification("OnMahjongViewHeadClick", m_SeatPos);
        }


        private void OnSeatInfoChanged(TransferData data)
        {
            //PaiJiu.UIItemSeat_PaiJiu
            Seat seat = data.GetValue<Seat>("Seat");
             NN_ENUM_ROOM_STATUS roomStatus = data.GetValue<NN_ENUM_ROOM_STATUS>("RoomStatus");
            Seat BankerSeat = data.GetValue<Seat>("BankerSeat");//庄家座位
            Room currRoom= data.GetValue<Room>("CurrentRoom");//当前房间
            // PAIGOW_ENUM_ROOM_STATUS roomStatus = data.GetValue<RoomEntity.RoomStatus>("RoomStatus");
            bool IsPlayer = data.GetValue<bool>("IsPlayer");

            if (m_nSeatIndex != seat.Index) return;
            if (!gameObject.activeSelf && seat.PlayerId <= 0) return;
            SetSeatInfo(seat, roomStatus);

            //设置走马灯
            SetChooseBankerHint(seat.IsBanker);
            //SetChooseBankerHint( (roomStatus != NN_ENUM_ROOM_STATUS.HOG && seat.IsBanker) || (roomStatus == NN_ENUM_ROOM_STATUS.HOG && currRoom.RobBankerSeat != null && seat == currRoom.RobBankerSeat));
            //设置已下注
            SetBaseScoreUI(seat.Pour);
            //设置庄
            SetBanker((seat.IsBanker && (currRoom.roomModel != Room.RoomModel.robBanker || currRoom.roomStatus != NN_ENUM_ROOM_STATUS.HOG)));

            //设置是否抢庄标识
            if(currRoom.roomModel == Room.RoomModel.robBanker) SetRobBanker(seat, roomStatus);
            ////设置牌型
            //SetPockeTypeUI(seat.PockeType, roomStatus);
            //设置牌
            //SetAllPokerUI(seat.PokerList);


            //m_Pour.transform.parent.gameObject.SetActive(seat.Pour > 0);
            //m_Pour.SafeSetText(seat.Pour.ToString());
            //SetChooseBankerHint(seat.seatStatus == SEAT_STATUS.POUR || seat.seatStatus == SEAT_STATUS.SETTLE);//走马灯
            //下注
        }



        public void SetSeatInfo(Seat seat, NN_ENUM_ROOM_STATUS roomStatus)
        {
             m_SeatPos = seat.Pos;
            
             if(!gameObject.activeSelf && seat.PlayerId != 0) SetGold(seat.Gold);//从新开启 则刷新
            gameObject.SetActive(seat.PlayerId != 0);
            //m_PlayerInfo.SetUI(seat);
            RefreshBasicInfo(seat);
            _isReadyTo.gameObject.SetActive(RoomNiuNiuProxy.Instance.CurrentRoom.currentLoop == 0 && RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus == NN_ENUM_ROOM_STATUS.IDLE && seat.IsReady);
          
        }


        #endregion


        #region 设置是否是庄
        /// <summary>
        /// 设置是否是庄
        /// </summary>
        /// <param name="isBanker"></param>
        public void SetBanker(bool isBanker)
        {
            
            bool isBankerActive = _banker.gameObject.activeSelf;
            _banker.gameObject.SetActive(isBanker);

            if (isBanker && isBankerActive != isBanker)
            {
                //_banker_zhuang.rectTransform.localScale = Vector3.one * 3;
                //mySequenceConfirmBankerAni.Restart();
                
                SetBankerUI(true, true);
            }

        }
        #endregion



        #region 设置是否抢庄
        /// <summary>
        /// 设置是否抢庄
        /// </summary>
        /// <param name="isBanker"></param>
        public void SetRobBanker(Seat seat, NN_ENUM_ROOM_STATUS roomStatus)
        {

            m_isRobBanker[0].gameObject.SetActive(seat.isAlreadyHOG == 1);//抢
            m_isRobBanker[1].gameObject.SetActive(seat.isAlreadyHOG == 2);//不抢
            m_isRobBanker[0].transform.parent.gameObject.SetActive(roomStatus == NN_ENUM_ROOM_STATUS.HOG);

        }
        #endregion

        #region 刷新基本信息
        /// <summary>
        /// 刷新基本信息
        /// </summary>
        /// <param name="seat"></param>
        public void RefreshBasicInfo(NiuNiu.Seat seat)
        {
         
            //设置头像
            SetAvatarUI(seat.Avatar);
            ////设置是否是庄标识
            //SetBankerUI(seat.IsBanker);
            //名字
            SetNicknameUI(seat.Nickname);
            ////金币
            //_gold.text = seat.Gold.ToString();
            //等等。。。。
            _gender = seat.Gender;
        }
        #endregion


        public void PlayeDealAni(NiuNiu.Seat seat ,System.Action onComplete = null)
        {
            
            isPlayDrawPokerAni = true;
            StartCoroutine(DealAni(seat, onComplete));
            //StartCoroutine("DealShowPoker", seat);
        }

        //发牌动画
        IEnumerator DealAni(NiuNiu.Seat seat, System.Action onComplete)
        {
            yield return 0;
            
            string pokerTypeAudio = string.Format("DealPoker_niuniu");
            AudioEffectManager.Instance.Play(pokerTypeAudio, Vector3.zero);


            //=========新动画==========================

            for (int i = 0; i < _pokersList.Length; i++)
            {
                _pokersList[i].transform.position = m_drawPokerPos.position;
                _pokersList[i].InitFlipCardsAni();
            }
            ShowPokers(true);
            for (int i = 0; i < m_drawPokerAni.Length; i++)
            {
               
                //_pokersList[i].gameObject.SetActive(true);
                m_drawPokerAni[i].Restart();
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.3f);

            for (int i = 0; i < m_returnPokerAni.Length; i++)
            {
             if(i == (m_returnPokerAni.Length-1))   m_returnPokerAni[i].OnComplete( 
                 ()=> {
                     if (onComplete != null) onComplete();
                     isPlayDrawPokerAni = false;
                     DealShowPoker(seat);
                 });
                m_returnPokerAni[i].Restart();
            }
       
        }
        //显示Poker 牌
        private void DealShowPoker(NiuNiu.Seat seat)
        {

            SetAllPokerUI(seat.PokerList);
           SetPokerPos(seat.PokerList);


        }



        //-------------以下 处理单个项目-------------------------------------------------------------------------------------------------------



        #region 设置头像
        /// <summary>
        /// 设置头像  
        /// </summary>
        /// <param name="avatar"></param>
        private void SetAvatarUI(string avatar)
        {

            TextureManager.Instance.LoadHead(avatar, OnAvatarLoadCallBack);

           
        }

        private void OnAvatarLoadCallBack(Texture avater)
        {
            if (_avatarTexture != null)
            {
                if (avater != null)
                {
                    _avatarTexture.texture = avater;
                }
                else
                {
                    if(_defaultAvatarTexture!=null) _avatarTexture.texture =  _defaultAvatarTexture;
                }
            }

        }
      

        #endregion

        #region 设置昵称
        /// <summary>
        /// 设置昵称  
        /// </summary>
        /// <param name="avatar"></param>
        private void SetNicknameUI(string nickname)
        {
            if (nickname != null)
            {

                _nickname.text = nickname;
            }
         
        }
        #endregion

        #region 设置准备
        /// <summary>
        /// 设置 准备好了图片显影
        /// </summary>
        /// <param name="avatar"></param>
        public void SetIsReadyToUI(bool noOff)
        {

            if (RoomNiuNiuProxy.Instance.CurrentRoom.currentLoop != 0 || RoomNiuNiuProxy.Instance.CurrentRoom.roomStatus != NN_ENUM_ROOM_STATUS.IDLE)
            {
                noOff = false;
            }
          
            if (_isReadyTo != null)
            {
                if (_isReadyTo.gameObject.activeSelf != noOff)
                {
                    _isReadyTo.gameObject.SetActive(noOff);

                }
            }



        }
        #endregion




        bool isPlayConfirmBankerAni = false;
       Vector3 _banker_zhuangLocalPosition = Vector3.zero;
        #region 设置是否是庄
        /// <summary>
        /// 设置是否是庄 标识
        /// </summary>
        /// <param name="isBanker"></param>
        public void SetBankerUI(bool isBanker ,bool isAni=false)
        {
            _banker.gameObject.SetActive(isBanker);

            if (isBanker && isAni)
            {
               
                if (isPlayConfirmBankerAni) return;
                if (_banker_zhuang != null)
                {
                    if (mySequenceConfirmBankerAni != null)
                    {
                        
                        _banker_zhuangLocalPosition = _banker_zhuang.rectTransform.localPosition;
                        isPlayConfirmBankerAni = true;
                        _banker_zhuang.rectTransform.localPosition = Vector3.zero;
                        //_banker_zhuang.rectTransform.localScale = Vector3.one * 3;

                        //播放声音
                        AudioEffectManager.Instance.Play(ConstDefine_NiuNiu.PromotionToBanker_niuniu, Vector3.zero);
                        mySequenceConfirmBankerAni.OnComplete(() => {
                            isPlayConfirmBankerAni = false;
                            _banker_zhuang.rectTransform.localPosition = _banker_zhuangLocalPosition;
                        }
                        ).Restart();
                    }
                    //NiuNiu.NiuNiuWindWordAni.ConfirmBankerAni(_banker_zhuang.transform);
                }

            }

        }
        #endregion

        #region 设置该家已有积分
        /// <summary>
        /// 设置该家已有积分
        /// </summary>
        /// <param name="gold"></param>
        public void SetGold(int gold)
        {

            _gold.text = gold.ToString();

        }
        #endregion


        #region 显示下注分数
        /// <summary>
        /// 设置显示 下注分数 (参数分数资源索引)
        /// </summary>
        /// <param name="seat"></param>
        public void SetBaseScoreUI(int baseScore)
        {
            if (baseScore > 0)
            {
                _baseScore.gameObject.SetActive(true);
              
            }
            if (baseScore == 0)
            {
                _baseScore.gameObject.SetActive(false);
            }

            if (_baseScore != null)
            {
                string onesStrName = "img_x" + baseScore;
                string pathK = string.Format("download/{0}/source/uisource/niuniu.drb", ConstDefine.GAME_NAME);
                Sprite currSprite = null;
                currSprite = AssetBundleManager.Instance.LoadSprite(pathK, onesStrName);

                if (currSprite != null)
                {
                    _baseScore.sprite = currSprite;
                }

            }

        }
        #endregion

        #region  设置手牌牌型显示
        /// <summary>
        /// 设置手牌牌型显示  (参数牌型 或 牌型资源索引 )
        /// </summary>
        /// <param name="pokersType"></param>
        public void SetPockeTypeUI(int pokersType, NN_ENUM_ROOM_STATUS roomStatus)
        {

            if (pokersType >= -1 && pokersType <= 17)
            {


                string pokerName = "niuniu_pokertype0";
                if (pokersType == -1)
                {
                    pokerName = "niuniu_pokertype0";
                    _pockeType.gameObject.SetActive(true);
                   
                }
                if (pokersType > 0)
                {
                    pokerName = "niuniu_pokertype" + pokersType;
                    _pockeType.gameObject.SetActive(true);
                   
                }
                if (pokersType == 0 ||roomStatus !=  NN_ENUM_ROOM_STATUS.RSETTLE)
                {
                    _pockeType.gameObject.SetActive(false);
                }


                if (_pockeType.gameObject.activeSelf)
                {

                    Sprite currSprite = null;
                    string pathK = string.Format("download/{0}/source/uisource/niuniu.drb", ConstDefine.GAME_NAME);
                    currSprite = AssetBundleManager.Instance.LoadSprite(pathK, pokerName);

                    if (currSprite != null)
                    {
                        _pockeType.sprite = currSprite;
                    }


                    if (myPokerTypeSequence != null)
                    {
                        _pockeType.rectTransform.localScale = Vector3.one * 3;
                        myPokerTypeSequence.Restart();
                    }
                    //NiuNiuWindWordAni.PokerTypeAni(_pockeType.rectTransform);
                    //播放声音
                    string pokerTypeAudio = string.Format("{0}_PokerType{1}_niuniu", _gender, pokersType);
                    AudioEffectManager.Instance.Play(pokerTypeAudio, Vector3.zero);

                }


            }


        }
        #endregion

        #region 播放收益动画 参数本局收益
        /// <summary>
        ///  播放收益动画 参数本局收益
        /// </summary>
        /// <param name="earnings"></param>
        public void PlayEarningsAni(int earnings)
        {
            _earningsAni.AgainPlay(earnings);
        }
        #endregion

        #region  设置5张扑克UI 
        /// <summary>
        /// 设置5张扑克UI  (由poker列表 过滤null 显示包含信息)
        /// </summary>
        public void SetAllPokerUI(List<NiuNiu.Poker> pokers)
        {
            //是否要设置关闭 5张牌父物体  ???
            ShowPokers(pokers[0].index != 0);
        
            for (int i = 0; i < pokers.Count; i++)
            {
                if (pokers[i] != null)
                {
                    SetSinglePokerUI(pokers[i], i);

                }

            }
        }
        #endregion

        #region  设置单张扑克UI (参数 大小 花色   扑克位置索引)
        /// <summary>
        /// 设置单张扑克UI (参数 大小 花色   扑克位置索引)
        /// </summary>
        public void SetSinglePokerUI(NiuNiu.Poker poker, int pokerIndex)
        {

            if (poker.status == NN_ENUM_POKER_STATUS.POKER_STATUS_UPWARD)
            {
                _pokersList[pokerIndex].SetPokerSprite(poker);
                //_pokersList[pokerIndex].transform.parent.GetComponent<PokerCtrl>().FlipCardsForward();
                //m_ShowPokerAni[pokerIndex].Restart();
            }
     
        }
        #endregion

        #region 设置Poker位置
        /// <summary>
        /// 设置 poker位置
        /// </summary>
        /// <param name="pokers"></param>
        public void SetPokerPos(List<NiuNiu.Poker> pokers)
        {
            if (!isPlayDrawPokerAni && gameObject.activeSelf)//等待
            {
               if(superModel== Room.SuperModel.CommonRoom) StartCoroutine("SetCalculationPokerPos", pokers);
                if (superModel == Room.SuperModel.PassionRoom) StartCoroutine("SetPos", pokers);

            }
        }

       
        /// <summary>
        ///  计算最后序列
        /// </summary>
        /// <param name="pokers"></param>
        /// <returns></returns>
        IEnumerator SetCalculationPokerPos(List<NiuNiu.Poker> pokers)
        {
            yield return 0;
            if (_seatWhetherNiu)
            {
                yield break;
            }

            //分析有没有牛
            bool whetherNiu = false;
           
            int[] pokerSubscript = Algorithm_NiuNiu.Instance.Calculate(pokers, out whetherNiu);

            //---------设置位置的形式-----------------------
            if (whetherNiu == false)
            {
                yield break;
            }
            else
            {
                _seatWhetherNiu = true;

                yield return new WaitForSeconds(0.2f);
                for (int i = 0; i < _pokersList.Length; i++)
                {
                    if (pokerSubscript[i] >= 0 && pokerSubscript[i] < _pokersList.Length)
                    {

                        //_pokersList[pokerSubscript[i]].rectTransform.position = _haveNiuPos[i].position;
                        Tweener tweener = _pokersList[pokerSubscript[i]].transform.DOMove(_haveNiuPos[i].position, 0.5f).SetEase(Ease.Linear);
                      
                        _pokersList[pokerSubscript[i]].transform.SetSiblingIndex(i);
                    }

                }

            }

    
        }



        private int CurrCount = 0;//当前计算的长度
        private int CurrType = 0;//当前计算出的牌型
        IEnumerator SetPos(List<NiuNiu.Poker> pokers)
        {

            yield return 0;

            List<Poker> positiveList = new List<Poker>();
            for (int i = 0; i < pokers.Count; i++)
            {
              if(pokers[i].status == NN_ENUM_POKER_STATUS.POKER_STATUS_UPWARD)   positiveList.Add(pokers[i]);

            }

            if(positiveList.Count <= CurrCount) yield break;
            if (positiveList.Count < 3) yield break;

            CurrCount = positiveList.Count;

            //返回牌型及牌顺序
            int currType = 0;
           Algorithm_NiuNiu.Instance.Calculate(positiveList, out _seatWhetherNiu, out currType);

            if(currType <= CurrType) yield break;
            CurrType = currType;

            for (int i = 0; i < pokers.Count; i++)
            {
                if (pokers[i].status == NN_ENUM_POKER_STATUS.POKER_STATUS_BACK) positiveList.Add(pokers[i]);

            }


            //改变位置
            yield return new WaitForSeconds(0.2f);
            for (int j = 0; j < positiveList.Count; j++)
            {
                for (int i = 0; i < _pokersList.Length; i++)
                {


                    if (pokers[i].index == positiveList[j].index)
                    {
                        Tweener tweener = _pokersList[i].transform.DOMove(_seatWhetherNiu ? _haveNiuPos[j].position : _initPokerPos[j], 0.5f).SetEase(Ease.Linear);
                        _pokersList[i].transform.SetSiblingIndex(j);
                        break;
                    }


                }
            }


        }












        #endregion

        #region 初始化 5张Poker位置
        /// <summary>
        ///  初始化 5张Poker位置
        /// </summary>
        public void InitPokerPos()
        {

            _seatWhetherNiu = false;
            CurrCount = 0;//当前计算的长度
            CurrType = 0;//当前计算出的牌型


            for (int i = 0; i < _pokersList.Length; i++)
            {
                //_pokersList[i].transform.localPosition = _initPokerPos[i];
                _pokersList[i].transform.position = _initPokerPos[i];
                _pokersList[i].transform.SetSiblingIndex(i);


            }

        }
        #endregion


        #region  设置5张牌 父物体显影
        /// <summary>
        /// 设置5张牌 父物体显影
        /// </summary>
        private void ShowPokers(bool noOff)
        {
            _pokersList[0].transform.parent.gameObject.SetActive(noOff);
        }
        #endregion


        //-------------以上 处理单个项目--------------------------------------------------------------------------------------------------------








        #region  初始化玩家Item  
        /// <summary>
        /// 初始化这个Item  全部默认值 全部关闭
        /// </summary>
        public void InfoItem()
        {
       
            //已准备
            _isReadyTo.gameObject.SetActive(false);
            //下注显示
            _banker.gameObject.SetActive(false);
            SetBaseScoreUI(0);
            //是否是庄
            SetBankerUI(false);
            //牌型
            _pockeType.gameObject.SetActive(false);
            SetPockeTypeUI(0, NN_ENUM_ROOM_STATUS.IDLE);

            ShowPokers(false);
          
            //关闭此Item
            gameObject.SetActive(false);

        }

        #endregion

        #region 根据信息打开这个Item 完整信息

        /// <summary>
        /// 根据信息打开这个Item 完整信息
        /// </summary>
        /// <param name="seat"></param>
        public void ShowWholeItem(NiuNiu.Seat seat, NN_ENUM_ROOM_STATUS roomStatus)
        {
            //头像
            SetAvatarUI(seat.Avatar);
            //昵称
            SetNicknameUI(seat.Nickname);
            //玩家是否准备
            SetIsReadyToUI(seat.IsReady);
            //是否是庄
            SetBankerUI(seat.IsBanker);


            //当前分数
            SetGold(seat.Gold);

            //下注显示
            SetBaseScoreUI(seat.Pour);
            //_banker.gameObject.SetActive(false);


            //牌型
            SetPockeTypeUI(seat.PockeType, roomStatus);

            //牌
            SetAllPokerUI(seat.PokerList);

            //PokerPos
            if (roomStatus == NN_ENUM_ROOM_STATUS.RSETTLE)
            {
                if (seat.PokerList[0].index > 0) SetPokerPos(seat.PokerList);



            }
            Debug.Log("位置" + seat.Pos + "的玩家是否胜利" + seat.Winner);

        }
        #endregion




        /// <summary>
        /// 控制对应玩家Item显影
        /// </summary>
        /// <param name="ooc"></param>
        public void OpenOrClosed(bool ooc)
        {
            gameObject.SetActive(ooc);

        }


        
        #region  设置提示灯
        /// <summary>
        /// 设置提示灯（开启提示 选择是否坐庄）
        /// </summary>
        public void SetChooseBankerHint(bool OnOff)
        {
            if (OnOff == isUIHintLight) return;
            m_UIHintLight.gameObject.SetActive(OnOff);
            isUIHintLight = OnOff;
            //开关提示 （灯旋转）
            if (OnOff)
            {
                StartCoroutine("SetUIHintLight");
            }
            else
            {
                StopCoroutine("SetUIHintLight");

            }

        }

        IEnumerator SetUIHintLight()
        {

            while (isUIHintLight)
            {
                m_UIHintLight.Rotate(new Vector3(0, 0, 90));
                yield return new WaitForSeconds(HintLightInterval);
            }

        }

        #endregion

    }
}