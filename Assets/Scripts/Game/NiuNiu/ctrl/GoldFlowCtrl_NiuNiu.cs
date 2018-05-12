//===================================================
//Author      : WZQ
//CreateTime  ：6/12/2017 6:19:46 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NiuNiu;

namespace NiuNiu
{
    public class GoldFlowCtrl_NiuNiu : MonoBehaviour//SingletonMono<GoldFlowCtrl_NiuNiu>
    {
        private static GoldFlowCtrl_NiuNiu instance;
        public static GoldFlowCtrl_NiuNiu Instance
        {
            get
            {

                return instance;
            }


        }

        public Transform m_goldParent; //金币物体挂载点
        public Transform[] m_playersPos;//6个点
        private Transform bankerTransform;//庄的位置
        public float winAndFailureInterval = 1f;//胜利和失败间隔
        public string m_goldName = NiuNiu.ConstDefine_NiuNiu.GoldName_NiuNiu;//金币预制体名字


        public int m_goldSum = 10;//每波生成金币数量
        public float m_createGoldTime = 0.05f;//生成间隔时间
        public Vector3 m_createPosOffset = Vector3.zero;//生成位置偏移量范围



        private List<NiuNiu.Seat> winList = new List<NiuNiu.Seat>(); //庄胜利列表
        private List<NiuNiu.Seat> failureList = new List<NiuNiu.Seat>();//庄失败列表


        void Awake()
        {
            instance = this;
            if (m_goldParent == null)
            {
                m_goldParent = transform;
            }

        }

        void OnDestroy()
        {
            ItemPool_NiuNiu.Instance.ClearItemObj(NiuNiu.ConstDefine_NiuNiu.GoldName_NiuNiu);

        }


        public void GoldFlowAni(List<NiuNiu.Seat> seatList)
        {
            winList.Clear();
            failureList.Clear();
            for (int i = 0; i < seatList.Count; i++)
            {
                if (seatList[i].IsBanker)
                {
                    Debug.Log(seatList[i].Index);

                    bankerTransform = m_playersPos[seatList[i].Index];

                    continue;
                }

                if (seatList[i].PockeType == 0)
                {
                    continue;
                }


                if (seatList[i].Winner)
                {
                    failureList.Add(seatList[i]);
                    //Debug.Log()
                }
                else
                {
                    winList.Add(seatList[i]);
                }

            }





            StartCoroutine("WinAndFailure");








        }

        IEnumerator WinAndFailure()
        {
            yield return 0;


            if (winList != null && winList.Count > 0)
            {
                AudioEffectManager.Instance.Play(NiuNiu.ConstDefine_NiuNiu.GoldMove_niuniu, Vector3.zero);

                //先播放庄胜利的金币动画
                for (int i = 0; i < winList.Count; i++)
                {
                    TransferData data = new TransferData();

                    int index = winList[i].Index;
                    data.SetValue<Vector3>("startPos", m_playersPos[index].position);//起点为闲家
                    data.SetValue<Vector3>("endPos", bankerTransform.position);//终点为庄

                    StartCoroutine("CreateGold", data);

                }
            }

            yield return new WaitForSeconds(winAndFailureInterval);
            if (failureList != null && failureList.Count > 0)
            {
                AudioEffectManager.Instance.Play(NiuNiu.ConstDefine_NiuNiu.GoldMove_niuniu, Vector3.zero);

                //再播放庄输的动画
                for (int i = 0; i < failureList.Count; i++)
                {

                    TransferData data = new TransferData();
                    int index = failureList[i].Index;
                    data.SetValue<Vector3>("startPos", bankerTransform.position);//起点为庄
                    data.SetValue<Vector3>("endPos", m_playersPos[index].position);//终点为闲家

                    StartCoroutine("CreateGold", data);

                }
            }







        }






        IEnumerator CreateGold(TransferData data)
        {
            Vector3 startPos = data.GetValue<Vector3>("startPos");
            Vector3 endPos = data.GetValue<Vector3>("endPos");

            yield return 0;


            for (int i = 0; i < m_goldSum; i++)
            {
                //m_createPosOffset = Mathf.Abs(m_createPosOffset);


                GameObject go = ItemPool_NiuNiu.Instance.GetObjectFromPool(m_goldName, m_goldParent, startPos);
                //go.transform.localPosition = m_createPosOffset;
                //Vector3 aaa = m_goldParent.position - go.transform.position;
                //startPos += aaa;
                //改变开始位置
                //startPos += new Vector3(Random.Range(-m_createPosOffset, m_createPosOffset), Random.Range(-m_createPosOffset, m_createPosOffset));

                //错开
                RectTransform rt = null;
                   rt= go.transform.FindChild(ConstDefine_NiuNiu.GoldImage_NiuNiu).GetComponent<RectTransform>();
                if (rt != null)
                {
                    float posx=Random.Range(-50,50);
                    float posy = Random.Range(-50, 50);
                    rt.anchoredPosition = new Vector2(posx, posy);

                //金币移动动画
                }

                NiuNiuWindWordAni.GoldFlowingAni(go.transform, rt, startPos, endPos);

                yield return new WaitForSeconds(m_createGoldTime);

            }





        }





    }
}