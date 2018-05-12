//===================================================
//Author      :聚友 牌墙管理
//CreateTime  ：8/12/2017 11:10:18 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using proto.jy;

namespace JuYou
{
    public class WallCtrl_JuYou : MonoBehaviour
    {
        private static WallCtrl_JuYou instance;
        public static WallCtrl_JuYou Instance { get { return instance; } }

        [SerializeField]
        private Grid3D[] m_WallContainers;//4个墙挂载点
        [SerializeField]
        private int m_EachWallcount = 8;//每个挂载点挂载数量

        [SerializeField]
        private Transform m_Wall;//墙模型（出麻将的横板）
        private Tweener m_WallTweener;//横板下移



        private void Awake()
        {
            instance = this;
            m_WallTweener = m_Wall.DOMove(m_Wall.transform.position + new Vector3(0, -20, 0), 0.7f).SetEase(Ease.Linear).SetAutoKill(false).Pause();
        }
        // Use this for initialization
        void Start()
        {

        }

   

        /// <summary>
        /// 初始化墙
        /// </summary>
        /// <param name="wall"></param>
        /// <param name="isPlayAnimation"></param>
        public void InitWall(List<MaJiangCtrl_JuYou> wall, bool isPlayAnimation)
        {
            int tableMaJiangCount = wall.Count / m_WallContainers.Length;
            int index = 0;
            for (int i = 0; i < m_WallContainers.Length; ++i)
            {
               
                int endIndex = tableMaJiangCount + index;
                for (int j = index; j < endIndex; ++j, ++index)
                {
                    wall[j].gameObject.SetParent(m_WallContainers[i].transform);
                }
             
            }

            //Debug.Log("===================m_WallContainer.gameObject.name============================" + m_WallContainer.gameObject.name);

            for (int j = 0; j < m_WallContainers.Length; j++)
            {
                m_WallContainers[j].Sort();
            }
            //m_WallContainer.Sort();
            if (isPlayAnimation)
            {
                StartCoroutine(InitWallCoroutine(wall));
            }
           



        }



        /// <summary>
        /// 初始化墙动画
        /// </summary>
        /// <param name="wall"></param>
        /// <returns></returns>
        private IEnumerator InitWallCoroutine(List<MaJiangCtrl_JuYou> wall)
        {

            for (int i = 0; i < wall.Count; ++i)
            {
                wall[i].gameObject.SetActive(false);
            }
            Debug.Log("-------------------初始化墙动画------------------------------------");
            yield return null;

            m_WallTweener.OnComplete(() =>
            {
                for (int i = 0; i < wall.Count; ++i)
                {
                    wall[i].gameObject.SetActive(true);
                }

                m_WallTweener.PlayBackwards();


            }).Restart();
        }






    }
}
