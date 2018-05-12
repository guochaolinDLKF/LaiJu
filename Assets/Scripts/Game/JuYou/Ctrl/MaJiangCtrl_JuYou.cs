//===================================================
//Author      : WZQ
//CreateTime  ：8/10/2017 3:36:20 PM
//Description ：聚友 麻将控制器
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace JuYou
{
    public class MaJiangCtrl_JuYou : MonoBehaviour
    {
        /// <summary>
        /// 数据
        /// </summary>
        [SerializeField]
        private Poker m_Poker;
        public Poker Poker
        {
            get { return m_Poker; }
            private set { m_Poker = value; }
        }

        private bool isBeenPlayed = false;
        public bool IsBeenPlayed
        {
            get{return isBeenPlayed;}
        }


        private Tweener tweener;
        private Vector3 UIPokerShow = new Vector3(180, 0, 0);
        private Vector3 UIPokerHide = Vector3.zero;

        public GameObject feng;

        private static readonly Vector3 UniversalPoint = new Vector3(-1.2f, 2.5f, -2f);
        //private static readonly Vector3 UniversalPoint = new Vector3(-1.2f, 2.5f, 2.3f);

        private static readonly Vector3 UniversalAngle = new Vector3(0f, 180f, 0f);

        private static readonly Vector3 UniversalScale = new Vector3(0.45f, 0.45f, 0.45f);

        private void Awake()
        {
            tweener = transform.DOLocalRotate(UIPokerShow, 0.7f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetAutoKill(false).Pause();
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.size = new Vector3(7f, 5f, 10f);

        }
        // Use this for initialization
        void Start()
        {
           //DRB.MahJong.MaJiangCtrl
        }

     

        public void Init(Poker poker)
        {
            isBeenPlayed = false;
            Poker = poker;
            if (poker != null && poker.color == 5 && feng == null) SetFeng();
            //SetUniversal(isUniversal);
            //CloseTip();
        }

        private void SetFeng()
        {

            const string modelName = "feng";
            string path = string.Format("download/{0}/prefab/model/{1}.drb", ConstDefine.GAME_NAME, modelName);
            AssetBundleManager.Instance.LoadOrDownload<GameObject>(path, modelName, (GameObject go) =>
            {
                feng = Instantiate(go);
                feng.transform.SetParent(transform);
                feng.transform.localPosition = UniversalPoint;
                feng.transform.localEulerAngles = UniversalAngle;
                feng.transform.localScale = UniversalScale;
                feng.layer = gameObject.layer;
            }, 0);
        }



        /// <summary>
        /// 设置牌状态
        /// </summary>
        /// <param name="isPlayAnimation">是否播放动画</param>
        /// <param name="pokerStatus">Poker状态</param>
        public void SetPokerStatus(bool isPlayAnimation,bool pokerStatus,System.Action OnComplete = null)
        {
            //是否已经播放过动画
            if (isBeenPlayed) return;

            if (isPlayAnimation)
            {
                if (pokerStatus)
                {

                    isBeenPlayed = true;
                    AppDebug.Log(string.Format("翻开牌{0}", m_Poker.ToChinese()));

                    tweener.OnComplete(

                        () =>
                        {
                            transform.localEulerAngles = pokerStatus ? UIPokerShow : UIPokerHide;
                            AppDebug.Log("Poker旋转动画播放完毕");
                            if (OnComplete != null) OnComplete();//兜中回调
                        }
                        ).Restart();
                }


            }
            else
            {
                transform.localEulerAngles = pokerStatus ? UIPokerShow : UIPokerHide;
                if (pokerStatus) isBeenPlayed = true;

            }

      



        }


    }
}