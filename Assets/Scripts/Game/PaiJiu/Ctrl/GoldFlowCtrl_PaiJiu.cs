//===================================================
//Author      : WZQ
//CreateTime  ：7/24/2017 12:05:44 PM
//Description ： 需要对此进行改造
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaiJiu
{
    public class GoldFlowCtrl_PaiJiu : MonoBehaviour {


        private static GoldFlowCtrl_PaiJiu instance;
        public static GoldFlowCtrl_PaiJiu Instance
        {
            get
            {
                return instance;
            }


        }

        public Transform m_goldParent; //金币物体挂载点
        public Transform[] m_playersPos;//4个点
        private Transform bankerTransform;//庄的位置
        public float aniTimeLength = 1f;//动画时长

        public int m_goldSum = 10;//每波生成金币数量
        public float m_createGoldTime = 0.05f;//生成间隔时间
       

        private List<PaiJiu.Seat> winList = new List<PaiJiu.Seat>(); //庄胜利列表
        private List<PaiJiu.Seat> failureList = new List<PaiJiu.Seat>();//庄失败列表


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
            Pool_PaiJiu.Instance.ClearItemObj(ConstDefine_PaiJiu.UIItemNameGold);

        }


        public void GoldFlowAni(List<PaiJiu.Seat> seatList)
        {
            winList.Clear();
            failureList.Clear();
            for (int i = 0; i < seatList.Count; i++)
            {
                if (seatList[i].PlayerId <= 0) continue;
                if (seatList[i].IsBanker)
                {
                    bankerTransform = m_playersPos[seatList[i].Index];
                    continue;
                }


                if (seatList[i].Winner)
                {
                    failureList.Add(seatList[i]);

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
                AudioEffectManager.Instance.Play(ConstDefine_PaiJiu.GoldMove_paijiu, Vector3.zero);

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

            yield return new WaitForSeconds(aniTimeLength);
            if (failureList != null && failureList.Count > 0)
            {
                AudioEffectManager.Instance.Play(ConstDefine_PaiJiu.GoldMove_paijiu, Vector3.zero);

                //再播放庄输的动画
                for (int i = 0; i < failureList.Count; i++)
                {

                    TransferData data = new TransferData();
                    int index = failureList[i].Index;
                    data.SetValue<Vector3>("startPos", bankerTransform.position);//起点为庄
                    data.SetValue<Vector3>("endPos", m_playersPos[index].position);//终点为闲家

                    StartCoroutine("CreateGold", data);
                    //yield return new WaitForSeconds((aniTimeLength*0.5f));
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
                string prefabPath = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, ConstDefine_PaiJiu.UIItemNameGold);//--------------------游戏名 暂时写死-------------------
                GameObject go = Pool_PaiJiu.Instance.GetObjectFromPool(ConstDefine_PaiJiu.UIItemNameGold, prefabPath, m_goldParent, startPos);
               

                //错开
                RectTransform rt = null;
                rt = go.transform.FindChild(ConstDefine_PaiJiu.GoldImage_NiuNiu).GetComponent<RectTransform>();
                if (rt != null)
                {
                    float posx = Random.Range(-50, 50);
                    float posy = Random.Range(-50, 50);
                    rt.anchoredPosition = new Vector2(posx, posy);

                    //金币移动动画
                }

                TweenAni_PaiJiu.GoldFlowingAni(go.transform, rt, startPos, endPos,

                    () => {

                        if (rt != null) rt.localPosition = Vector3.zero;
                        Pool_PaiJiu.Instance.PushToPool( go , m_goldParent);
                        


                    }


                    );

                yield return new WaitForSeconds(m_createGoldTime);

            }





        }


    }
}