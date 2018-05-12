//===================================================
//Author      : WZQ
//CreateTime  ：6/14/2017 10:21:26 AM
//Description ：挂载到牛头动画预制体上  提供动画事件
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NiuNiu
{
    public class UIItemTauren_NiuNiu : MonoBehaviour
    {

        public GameObject ItemTauren;//动画预制体
        public Transform m_aniRoot;//动画根物体
        public Image m_tauren;//牛头
        public Image m_brand;//牌子
        public Image m_halo;//强光光环 



        public string[] BrandName;
        private int BrandSpriteIndex = 0;



        void Start()
        {
            AudioEffectManager.Instance.Play(NiuNiu.ConstDefine_NiuNiu.StartGame_niuniu, Vector3.zero);

        }


        //开始牛头动画  更换开始牌子
        public void ReplaceBrand()
        {

            BrandSpriteIndex++;
            BrandSpriteIndex = BrandSpriteIndex % BrandName.Length;

            //更换图片
            string spriteName = BrandName[BrandSpriteIndex];
            Sprite currSprite = null;
            string pathK = string.Format("download/{0}/source/uisource/niuniu.drb", ConstDefine.GAME_NAME);
            currSprite = AssetBundleManager.Instance.LoadSprite(pathK, spriteName);

            if (currSprite != null)
            {
                m_brand.sprite = currSprite;
            }




        }



        //动画全部播放完成后
        public void PlayOver()
        {
            Destroy(ItemTauren);

        }











    }
}