//===================================================
//Author      : 牌九 牌墙管理
//CreateTime  ：8/1/2017 11:10:18 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using proto.paigow;

namespace PaiJiu
{
    public class WallManager_PaiJiu : MonoBehaviour
    {
        private static WallManager_PaiJiu instance;
        public static WallManager_PaiJiu Instance{get{return instance;}}

        [SerializeField]
        private Grid3D[] m_WallContainers;//4个墙挂载点
        [SerializeField]
        private int m_EachWallcount = 8;//每个挂载点挂载数量

        [SerializeField]
        private Transform m_WallModel;//墙模型（出麻将的横板）
        private Tweener m_WallTweener;//横板下移

        

        private void Awake()
        {
            instance = this;
            m_WallTweener = m_WallModel.DOMove(m_WallModel.transform.position + new Vector3(0, -20, 0), 0.7f).SetEase(Ease.Linear).SetAutoKill(false).Pause();
        }
        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }



        /// <summary>
        /// 初始化墙
        /// </summary>
        /// <param name="wall"></param>
        /// <param name="isPlayAnimation"></param>
        public void InitWall(List<MaJiangCtrl_PaiJiu> wall, bool isPlayAnimation)
        {


            //Debug.Log("===================m_WallContainer.gameObject.name============================" + m_WallContainer.gameObject.name);

            for (int i = 0; i < wall.Count; ++i)
            {
                for (int j = 0; j < m_WallContainers.Length; j++)
                {

                    if (m_WallContainers[j].transform.childCount >= m_EachWallcount) continue;
                    wall[i].gameObject.SetParent(m_WallContainers[j].transform);
                    break;

                }
            }
            for (int j = 0; j < m_WallContainers.Length; j++)
            {
                m_WallContainers[j].Sort();
            }

            //m_WallContainer.Sort();
            if (isPlayAnimation)
            {

                StartCoroutine(InitWallCoroutine(wall));
            }
            else
            {
                //m_WallContainer.transform.position = wallModelInitPos + m_WallContainer.transform.forward * 20;
            }



        }



        /// <summary>
        /// 初始化墙动画
        /// </summary>
        /// <param name="wall"></param>
        /// <returns></returns>
        private IEnumerator InitWallCoroutine(List<MaJiangCtrl_PaiJiu> wall)
        {

            for (int i = 0; i < wall.Count; ++i)
            {
                wall[i].gameObject.SetActive(false);
            }

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