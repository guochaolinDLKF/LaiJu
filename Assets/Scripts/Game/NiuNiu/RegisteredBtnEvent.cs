//===================================================
//Author      : WZQ
//CreateTime  ：5/11/2017 3:57:58 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using niuniu.proto;
/// <summary>
/// 注册各种 按钮的点击事件
/// </summary>

namespace NiuNiu
{
    public class RegisteredBtnEvent : MonoBehaviour
    {
        private static RegisteredBtnEvent instance;

        public static RegisteredBtnEvent Instance { get { return instance; } }


        public RectTransform _ReadyBtn;                         //准备按钮

        public RectTransform _StartBtn;                      //开始本局游戏Btn

        public RectTransform _AbdicateBankerBtn;              //让庄Btn

        public RectTransform _RobBankerBtn;                  //抢庄Btn

        public RectTransform _NoRobBankerBtn;                //不抢庄Btn

        [SerializeField]
        private RectTransform[] _BetScoreBtn;               //下注选分按钮


        public RectTransform _OpenPokerBtn;                   //开牌Btn

        public RectTransform _RubPokerBtn;                   //搓牌Btn//SendRubPokerNiuNiu

        public RectTransform[] _NeedOpenPoker;               //需要开启的两张牌

        public RectTransform _DropDown;                     //下拉菜单Btn





        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            EventTriggerListener.Get(_ReadyBtn.gameObject).onClick += ready; //准备

            EventTriggerListener.Get(_StartBtn.gameObject).onClick += StartGame; //开始游戏

            EventTriggerListener.Get(_AbdicateBankerBtn.gameObject).onClick += AbdicateBanker;//让庄

            EventTriggerListener.Get(_RobBankerBtn.gameObject).onClick += RobBanker; //抢庄
            EventTriggerListener.Get(_NoRobBankerBtn.gameObject).onClick += NoRobBanker; //不抢庄


            for (int i = 0; i < _BetScoreBtn.Length; i++)
            {
                EventTriggerListener.Get(_BetScoreBtn[i].gameObject).onClick += BetScoreBtn;//下注选分5个按钮
            }



            EventTriggerListener.Get(_OpenPokerBtn.gameObject).onClick += OpenPokerBtn;//开牌Btn

           if(_RubPokerBtn != null) EventTriggerListener.Get(_RubPokerBtn.gameObject).onClick += RubPokerBtn;//搓牌Btn

            for (int i = 0; i < _NeedOpenPoker.Length; i++)
            {
                EventTriggerListener.Get(_NeedOpenPoker[i].gameObject).onClick += NeedOpenPoker;//需要开启的两张牌
            }

            EventTriggerListener.Get(_DropDown.gameObject).onClick += DropDown;//下拉监听



        }


        //准备按钮监听
        void ready(GameObject go)
        {

            NiuNiuEventDispatcher.Instance.Dispatch("applyReadyNiuNiu");

        }



        //庄家开始游戏 按钮监听 
        void StartGame(GameObject go)
        {

          
            object[] obj = new object[] { };
            NiuNiuEventDispatcher.Instance.Dispatch("applyForStartGameNiuNiu", obj);

        }

        //让庄
        void AbdicateBanker(GameObject go)
        {
            

            object[] obj = new object[] { };
            NiuNiuEventDispatcher.Instance.Dispatch("AbdicateBankerNiuNiu", obj);

        }

        //抢庄
        void RobBanker(GameObject go)
        {
           
            object[] obj = new object[] { 1 };
            NiuNiuEventDispatcher.Instance.Dispatch("RobBankerNiuNiu", obj);

        }

        //不抢庄
        void NoRobBanker(GameObject go)
        {
            object[] obj = new object[] { 2 };
            NiuNiuEventDispatcher.Instance.Dispatch("RobBankerNiuNiu", obj);
            ////关闭抢庄
            //UIInteraction_NiuNiu.Instance.SwitchNoBankerBtnParentBtn(false);

        }



        //下注选分5个按钮监听
        void BetScoreBtn(GameObject go)
        {
            Debug.Log("选择下注分数为：" + go.name);
            int betScore = int.Parse(go.name);
            //GameStateManager.Instance.SendBetScore(betScore);     ||||||
            object[] obj = new object[] { betScore };
            NiuNiuEventDispatcher.Instance.Dispatch("SendBetScoreNiuNiu", obj);


        }




        ///开牌对比 Btn监听   //功能为开启自己两张牌
        void OpenPokerBtn(GameObject go)
        {

            object[] obj = new object[_NeedOpenPoker.Length];
            for (int i = 0; i < _NeedOpenPoker.Length; i++)
            {
                int index = _NeedOpenPoker[i].GetSiblingIndex();
                obj[i] = index;

            }
            NiuNiuEventDispatcher.Instance.Dispatch("NeedOpenPokerNiuNiu", obj);

        }

        ///搓牌
        void RubPokerBtn(GameObject go)
        {
            NiuNiuEventDispatcher.Instance.Dispatch("SendRubPokerNiuNiu");

        }



        //需要被开启的两张牌 Btn监听
        void NeedOpenPoker(GameObject go)
        {
            int index = go.GetComponent<RectTransform>().GetSiblingIndex();


            //GameStateManager.Instance.NeedOpenPoker(index);      ||||||
            object[] obj = new object[] { index };
            NiuNiuEventDispatcher.Instance.Dispatch("NeedOpenPokerNiuNiu", obj);

        }


        //下拉菜单 Btn监听
        void DropDown(GameObject go)
        {
            //下拉 开启菜单窗口       
            UIViewManager.Instance.OpenWindow(UIWindowType.Setting);

        }








    }
}