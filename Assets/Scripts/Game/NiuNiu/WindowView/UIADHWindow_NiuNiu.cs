//===================================================
//Author      : WZQ
//CreateTime  ：6/14/2017 5:59:38 PM
//Description ：AgreeDissolveHint_NiuNiu 同意解散提示 显示同意人数
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using NiuNiu;
using UnityEngine.UI;

namespace NiuNiu
{
    public class UIADHWindow_NiuNiu : UIWindowViewBase
    {

        public GameObject m_ADHWindow_NiuNiu; //整个预制体
        public Transform m_CountdownParent;//倒计时物体


        public Image m_countdownImage;//倒计时

        public Text m_hintTextSum;//提示文字

        [SerializeField]
        private float countdownTimeMax = 15f;//设置倒计时最大读秒
        private float countdownTime = 15;//倒计时剩余时间
        private int intCountdownTime = 0;//倒计时图片数值
        private bool countdownNoOff = false;//是否开启倒计时
        private float countdownUIAdjust = 0f;//倒计时UI调整 补偿

        public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
        {
            Dictionary<string, ModelDispatcher.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
            dic.Add(ConstDefine_NiuNiu.ObKey_SetADHWindowSum, SetADHWindowSum);//SetADHWindowSum
            return dic;
        }



        void Update()
        {
            if (countdownNoOff)
            {

                countdownTime -= Time.deltaTime;
                if ((int)(countdownTime + countdownUIAdjust) != intCountdownTime)
                {
                    SetCountdownSprite((int)countdownTime);

                }
                if (countdownTime <= 0)
                {

                    InitCountdownUI();


                }

            }


        }

        public void SetUI(TransferData data)
        {
            //int sum= data.GetValue<int>("SetADHWindowSum");//人数

            //显示人数
            SetADHWindowSum(data);



            countdownNoOff = data.GetValue<bool>("IsCountdownNoOff");//是否显示倒计时

            if (countdownNoOff)
            {
                long time = data.GetValue<long>("SetSvrTime");//倒计时SetSvrTime

                if (time < 0)
                {
                    Close();
                    return;
                }
                //计算倒计时
                long currTime = TimeUtil.GetTimestamp();
                countdownTime = (time - currTime) + GlobalInit.Instance.TimeDistance * 0.001f;
                countdownTime = Mathf.Clamp(countdownTime, 0, countdownTimeMax);

                countdownNoOff = true;
                intCountdownTime = (int)countdownTime;
                SetCountdownSprite(intCountdownTime);
            }


        }

        /// <summary>
        /// 设置同意解散的人数
        /// </summary>
        /// <param name="data"></param>
        public void SetADHWindowSum(TransferData data)
        {
            int sum = data.GetValue<int>("SetADHWindowSum");
            if (m_hintTextSum != null)
            {
                m_hintTextSum.text = sum.ToString();

            }
        }







        void SetCountdownSprite(int time)
        {
            //if (time == intCountdownTime)
            //{
            //    return;
            //}
            intCountdownTime = time;


            //更换图片
            string spriteName = "img_daoshu" + time;
            Sprite currSprite = null;
            string pathK = string.Format("download/{0}/source/uisource/niuniu.drb", ConstDefine.GAME_NAME);
            currSprite = AssetBundleManager.Instance.LoadSprite(pathK, spriteName);

            if (currSprite != null)
            {
                m_countdownImage.sprite = currSprite;
            }


        }


        void InitCountdownUI()
        {

            //countdownTime = countdownTimeMax;
            //intCountdownTime = (int)countdownTimeMax;

            //SetCountdownSprite(intCountdownTime);

            countdownNoOff = false;
            //m_CountdownParent.gameObject.SetActive(false);

        }

        void HintOver()
        {
            Close();

        }

    }
}