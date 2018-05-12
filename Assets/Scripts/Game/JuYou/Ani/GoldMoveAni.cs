//===================================================
//Author      : WZQ
//CreateTime  ：8/24/2017 3:55:14 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
namespace JuYou
{
    public class GoldMoveAni : MonoBehaviour
    {
        [SerializeField]
        private Transform[] gold;
        public int m_sum = 5;

        //private List<KeyValuePair<int, int>> m_PairChipSnumber; //筹码数量Pair
        //private Dictionary<int, int> m_DicChipSnumber;//筹码数量Dic
        private ChipSnumberList m_ChipSnumberList;//筹码数量
        public void PlayAni(Vector3 startPos, Vector3 endValue, int goldValue, bool autoDestruction, Action OnComplete)
        {

            StartCoroutine(goldMove(startPos, endValue, goldValue, autoDestruction, OnComplete));

        }

        IEnumerator goldMove(Vector3 startPos, Vector3 endValue, int goldValue, bool autoDestruction, Action OnComLete)
        {
            Debug.Log("-----------------------------------1");
            //yield return null;

            //for (int i = 0; i < m_sum; ++i)
            //{
            //    int goldIndex = UnityEngine.Random.Range(0, gold.Length);
            //    GameObject go = Instantiate(gold[goldIndex].gameObject);
            //    go.SetParent(transform, true);
            //    go.transform.position = startPos;
            //    go.transform.GetChild(0).localPosition = new Vector3(UnityEngine.Random.Range(-150, 100), UnityEngine.Random.Range(-100, 100), 0);
            //    go.SetActive(true);

            //    go.transform.DOMove(endValue, 0.8f).OnComplete(
            //        () =>
            //        {
            //            AudioEffectManager.Instance.Play(ConstDefine_JuYou.AudioGoldMove, Vector3.zero, false);
            //            go.SetActive(!autoDestruction);
            //        }

            //        );

            //    yield return null;

            //}




            //展示实际筹码数--------------------------------------------------

            if (m_ChipSnumberList == null)
            {
                m_ChipSnumberList = new ChipSnumberList();
                m_ChipSnumberList.AddPair(new ChipSnumber<int, int>(50, 0));
                m_ChipSnumberList.AddPair(new ChipSnumber<int, int>(20, 0));
                m_ChipSnumberList.AddPair(new ChipSnumber<int, int>(10, 0));
            }
            m_ChipSnumberList.InitValue();

            GetChipSnumber(Mathf.Abs(goldValue), 0, m_ChipSnumberList);

            for (int i = 0; i < m_ChipSnumberList.m_ChipSnumberList.Count; i++)
            {
                Debug.Log("m_ChipSnumberList.m_ChipSnumberList.Count:" + m_ChipSnumberList.m_ChipSnumberList[i].Value);
            }


            Debug.Log("-----------------------------------goldValue" + goldValue);
            for (int i = 0; i < m_ChipSnumberList.m_ChipSnumberList.Count; i++)
            {

                int goldIndex = 0;

                for (int j = 0; j < gold.Length; j++)
                {
                    if (gold[j].name == m_ChipSnumberList.m_ChipSnumberList[i].Key.ToString()) goldIndex = j;
                }


                Debug.Log("-----------------------------------goldIndex" + goldIndex);
                for (int j = 0; j < m_ChipSnumberList.m_ChipSnumberList[i].Value; j++)
                {

                    Debug.Log("-----------------------------------m_ChipSnumberList.m_ChipSnumberList[i].Value" + m_ChipSnumberList.m_ChipSnumberList[i].Value);
                    GameObject go = Instantiate(gold[goldIndex].gameObject);
                    go.SetParent(transform, true);
                    //go.transform.position = startPos;

                    //go.transform.GetChild(0).localPosition = new Vector3(UnityEngine.Random.Range(-150, 100), UnityEngine.Random.Range(-100, 100), 0);
                    go.transform.localPosition = new Vector3(UnityEngine.Random.Range(-150, 100), UnityEngine.Random.Range(-100, 100), 0);
                    go.SetActive(true);

                    //go.transform.DOMove(endValue, 0.8f).OnComplete(
                    //    () =>
                    //    {
                    //        AudioEffectManager.Instance.Play(ConstDefine_JuYou.AudioGoldMove, Vector3.zero, false);
                    //        go.SetActive(!autoDestruction);
                    //    }

                    //    );
                }
            }

            //yield return new WaitForSeconds(0.8f);

            //if (OnComLete != null) OnComLete();

            //if (autoDestruction) Destroy(gameObject);

            AudioEffectManager.Instance.Play(ConstDefine_JuYou.AudioGoldMove, Vector3.zero, false);
            transform.DOMove(endValue, 0.8f).OnComplete(
                () =>
                {
                  
                    if (OnComLete != null) OnComLete();
                    //gameObject.SetActive(!autoDestruction);
                    //if (autoDestruction) Destroy(gameObject);
                    Debug.Log("金币动画移动结束");
                    Destroy(gameObject);
                }

                );
            yield return null;

        }



        //计算各筹码个数
        private List<ChipSnumber<int, int>> GetChipSnumber(int chipValue, int index, ChipSnumberList chipSnumberList)
        {
            if (index >= chipSnumberList.m_ChipSnumberList.Count) return m_ChipSnumberList.m_ChipSnumberList;
            Debug.Log("chipSnumberList.m_ChipSnumberList[index].Key" + chipSnumberList.m_ChipSnumberList[index].Key);
            int x = chipSnumberList.m_ChipSnumberList[index].Key;
            if (chipValue >= x)
            {
                Debug.Log("-----------------------------------chipValue / x" + (chipValue / x));
                chipSnumberList.m_ChipSnumberList[index].Value = chipValue / x;
            }
                GetChipSnumber((chipValue % x), ++index, chipSnumberList);

            //筹码 10 20 50
            return m_ChipSnumberList.m_ChipSnumberList;
        }

        //private float Remainder(float betPoints)
        //{



        //}


    }
}