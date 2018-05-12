//===================================================
//Author      : DRB
//CreateTime  ：12/18/2017 6:33:02 PM
//Description ：
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace DRB.DouDiZhu
{
    public enum AnimationType
    {
        None,
        //Bomb,
        //SSBomb,
        SSBombSmoke,
        Spring,
    }
    public class UIItemAnimation : MonoBehaviour
    {
        ///// <summary>
        /// 动画移动时间
        /// </summary>
        private const float INTERACTIVE_EXPRESSION_DURATION = 0.6f;

        /// <summary>
        ///  动画移动时的alpha值
        /// </summary>
        private const float INTERACTIVE_EXPRESSION_ALPHA = 0.5f;

        //public static UIItemAnimation Instance;

        //private void Awake()
        //{
        //    Instance = this;
        //}
        public void LoadAnimation(string animation, int index = 0, Transform parent = null, Transform statTra = null, Transform targetTra = null)
        {
            AssetBundleManager.Instance.LoadOrDownload(string.Format("download/{0}/prefab/uiprefab/uianimations/{1}.drb", ConstDefine.GAME_NAME, animation), animation, (GameObject go) =>
           {
               if (go != null)
               {
                   go = Instantiate(go);
                   if (parent != null)
                   {
                       go.SetParent(parent);
                   }
                   else
                   {
                       go.SetParent(this.transform);
                   }

                   go.transform.position = Vector3.zero;
                   //go.transform.DOMoveZ(1, 5).OnComplete(() => { Destroy(go); });
                   if (animation == "DouDiZhu/UI_BombAnimation")
                   {
                       //go.GetComponent<UIItemBombAnimation>().SetBomb(index);
                   }
                   if (animation == "uiicon/UI_IconABCDE")
                   {
                       if (targetTra == null)
                       {
                           Debug.LogWarning("kkkkkkkkkkkkkk");
                       }
                       else
                       {
                           Debug.LogWarning(targetTra.name);
                       }
                       //go.GetComponent<UIItemIconAnimation>().SetTargetVector3(targetTra.position + new Vector3(5, 0, 0));
                   }
               }
           });
        }

        #region ShowPokersAnimation 显示牌的动画
        /// <summary>
        /// 显示牌的动画
        /// </summary>
        public void ShowPokersAnimation(string animation, Vector3 startPos, Vector3 targetPos, AnimationType animationType = AnimationType.None,
            float animationDuration = INTERACTIVE_EXPRESSION_DURATION, Ease ease = Ease.Linear, System.Action complete = null)
        {
            AssetBundleManager.Instance.LoadOrDownload(string.Format("download/{0}/prefab/uiprefab/uianimations/{1}.drb", ConstDefine.GAME_NAME, animation), animation, (GameObject go) =>
            {
                if (go != null)
                {
                    go = Instantiate(go);

                    go.SetParent(this.transform);

                    go.transform.position = startPos;

                    if (go.GetComponent<UIAnimation>() != null)
                    {
                        go.GetComponent<UIAnimation>().enabled = false;
                    }

                    if (animationType == AnimationType.SSBombSmoke || animationType == AnimationType.Spring)
                    {
                        go.transform.localScale = new Vector3(2, 2, 2);
                    }
                    else
                    {
                        go.transform.localScale = Vector3.one;

                    }

                    Image img = go.GetComponent<Image>();

                    if (go.GetComponent<UIAnimation>() != null)
                    {
                        go.GetComponent<UIAnimation>().enabled = true;
                    }

                    img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
                    //EndVector3 = transform.TransformPoint(EndVector3);
                    
                    go.transform.DOMove(targetPos, animationDuration).SetEase(ease).OnComplete(() =>
                    {
                        Debug.LogWarning(animation + "???");
                        if (animation == "DouDiZhu/UI_SSBombSmoke_DouDiZhu")
                        {
                            ShowPokersAnimation("uiicon/UI_IconSS", targetPos, targetPos, AnimationType.None, INTERACTIVE_EXPRESSION_DURATION, Ease.InOutCubic);
                            AudioEffectManager.Instance.Play("", Vector3.zero, false);
                        }
                        if (complete != null)
                        {
                            complete();
                        }

                        Destroy(go);
                    });

                    if (animationType == AnimationType.Spring)
                    {
                        go.GetComponent<UIItemSpringAnimation>().FlowerAnimation();
                    }
                }
            });
        }
        #endregion
        public void ShowPokersIconAnimation(string animation, Vector3 StartVector3, Vector3 EndVector3, bool isMove, float animationDurTime = 0, Ease ease = Ease.Linear, string audioName = "")
        {
            AssetBundleManager.Instance.LoadOrDownload(string.Format("download/{0}/prefab/uiprefab/uianimations/uiicon/{1}.drb", ConstDefine.GAME_NAME, animation), animation, (GameObject go) =>
            {
                if (go != null)
                {
                    go = Instantiate(go);
                    go.SetParent(this.transform);
                    go.transform.position = StartVector3;
                    //go.transform.localScale = new Vector3(2, 2, 2);
                    go.transform.localScale = Vector3.one;

                    float animationDuration;
                    if (animationDurTime != 0)
                    {
                        animationDuration = animationDurTime;
                    }
                    else
                    {
                        animationDuration = INTERACTIVE_EXPRESSION_DURATION;
                    }

                    Image img = go.GetComponent<Image>(); if (!string.IsNullOrEmpty(audioName))
                    {
                        AudioEffectManager.Instance.Play(string.Format("{0}", audioName), Vector3.zero, false);
                    }
                    img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
                    //EndVector3 = transform.TransformPoint(EndVector3);
                    if (isMove)
                    {
                        go.transform.DOMove(EndVector3, animationDuration).SetEase(ease).OnComplete(() =>
                        {
                            Destroy(go);
                        });
                    }
                    else
                    {
                        go.transform.GetComponent<Image>().DOColor(Color.white, animationDuration).OnComplete(() => { Destroy(go); });
                    }
                }
            });
        }
    }
}