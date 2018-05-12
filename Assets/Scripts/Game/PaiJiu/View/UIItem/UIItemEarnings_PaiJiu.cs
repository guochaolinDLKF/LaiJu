//===================================================
//Author      : WZQ
//CreateTime  ：7/20/2017 5:25:36 PM
//Description ：每次结算 飘分动画
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PaiJiu {
    public class UIItemEarnings_PaiJiu : MonoBehaviour {

        [SerializeField]
        private Transform m_scoreImages;//分数父物体

        [SerializeField]
        private Image[] _symbol;//  _symbol[0] 正 _symbol[1] 负

        [SerializeField]
        private Image place;//个位图片（也是图片模板）

        private List<Image> imageList = new List<Image>();//图片List
        private List<int> intList = new List<int>();// 每位数List


        


        //播放完成后
        public void InitAni()
        {
            if(imageList.Count <= 0) imageList.Add(place);
            gameObject.SetActive(false);
            transform.localScale = Vector3.zero;
            transform.localPosition = Vector3.zero;
            for (int i = 1; i < imageList.Count; i++)
            {
                imageList[i].gameObject.SetActive(false);
            }


        }

        public void SetUI(int earningsValue)
        {
            //设置符号

            _symbol[0].gameObject.SetActive(earningsValue >= 0);
            _symbol[1].gameObject.SetActive(earningsValue < 0);


            intList.Clear();
            for (int i = 0; i < imageList.Count; i++)
            {
                imageList[i].gameObject.SetActive(false);
            }

            int earningsABSValue = Mathf.Abs(earningsValue);

            SetFindString(earningsABSValue);

            //根据intlist值设置ImageList

            for (int i = 0; i < intList.Count; i++)
            {
                if ((i+1)> imageList.Count)
                {
                    GameObject imageClone = Instantiate(place.gameObject)as GameObject;
                    imageList.Add(imageClone.GetComponent<Image>());
                    imageClone.SetParent(m_scoreImages);
                    imageClone.transform.SetSiblingIndex(1);
                }

                SetSprite(imageList[i], earningsValue, intList[i].ToString());

                imageList[i].gameObject.SetActive(true);
            }
            //设置分数
            //SetSprite(place, earningsValue, (earningsABSValue % 10).ToString());//个位


           
            TweenAni_PaiJiu.FlyTo(transform, 

                () => {
                InitAni();


            }
            );




        }


        //找到每一位数
        private void SetFindString(int sum)
        {
            int x = sum % 10;

            intList.Add(x);
            int shang = sum / 10;//商
            if (sum >= 10)
            {
                SetFindString(shang);
            }
            else
            {
                return;
            }

        }



        //设置图片
        private void SetSprite(Image setSprite, int earningsValue, string strChar)
        {

            if (setSprite != null)
            {

                string onesStrName = earningsValue >= 0 ? "img_fenshu" + strChar : "jian" + strChar;
                Sprite currSprite = null;
                //设置加载路径
                string prefabPath = string.Format(ConstDefine_PaiJiu.UISpritePath, ConstDefine.GAME_NAME);
                currSprite = AssetBundleManager.Instance.LoadSprite(prefabPath, onesStrName);

                if (currSprite != null)
                {
                    setSprite.sprite = currSprite;
                }

            }
        }

    }
}