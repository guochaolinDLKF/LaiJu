//===================================================
//Author      : DRB
//CreateTime  ：4/1/2017 2:53:13 PM
//Description ：麻将控制器
//===================================================
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum PokerType
{
    Hand,
    Draw,
    Desktop,
    Peng,
    Wall,
}

namespace DRB.MahJong
{
    public class MaJiangCtrl : MonoBehaviour
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

        private GameObject m_Tip;

        private GameObject m_Universal;

        private GameObject m_Model;

#if IS_LONGGANG || IS_WANGQUE
        private static readonly Vector3 UniversalPoint = new Vector3(-1.2f, 2.5f, -2f);
#else
        private static readonly Vector3 UniversalPoint = new Vector3(-1.2f, 2.5f, 2.3f);
#endif

#if IS_LONGGANG || IS_WANGQUE
        private const string MODEL_NAME = "isUniversal_lg";
#else
        private const string MODEL_NAME = "isUniversal";
#endif


        private static readonly Vector3 UniversalAngle = new Vector3(0f, 180f, 0f);

        private static readonly Vector3 UniversalScale = new Vector3(0.45f, 0.45f, 0.45f);


        private bool m_isHold = false;

        private bool m_isShow = false;

        private bool m_isPlayingAnimation;

        private void Awake()
        {
            m_Model = transform.GetChild(0).gameObject;
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.size = new Vector3(7f, 5f, 10f);
        }

        #region Init 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="poker"></param>
        /// <param name="isUniversal"></param>
        public void Init(Poker poker, bool isUniversal)
        {
            Reset();
            Poker = poker;
            SetUniversal(isUniversal, RoomMaJiangProxy.Instance.CurrentRoom.PokerTotalPerPlayer == 17);
        }
        #endregion

        #region SetUniversal 设置万能牌
        /// <summary>
        /// 设置万能牌
        /// </summary>
        /// <param name="isUniversal"></param>
        public void SetUniversal(bool isUniversal, bool isSpecialColor)
        {
            if (isUniversal)
            {
                if (m_Universal != null)
                {
                    m_Universal.SetActive(true);
                    m_Universal.SetLayer(gameObject.layer);
                }
                else
                {
                    string path = string.Format("download/{0}/prefab/model/{1}.drb", ConstDefine.GAME_NAME, MODEL_NAME);
                    GameObject go = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(path, MODEL_NAME);
                    m_Universal = Instantiate(go);
                    m_Universal.transform.SetParent(transform);
                    m_Universal.transform.localPosition = UniversalPoint;
                    m_Universal.transform.localEulerAngles = UniversalAngle;
                    m_Universal.transform.localScale = UniversalScale;

                    MeshCollider collider = m_Universal.GetComponent<MeshCollider>();
                    if (collider != null)
                    {
                        collider.enabled = false;
                    }
                    m_Universal.SetLayer(gameObject.layer);
                }
                if (isSpecialColor)
                {
                    Color = "universal";
                }
            }
            else
            {
                if (m_Universal != null)
                {
                    m_Universal.SetActive(false);
                }
            }
        }
        #endregion

        #region ShowTip 显示提示
        /// <summary>
        /// 显示提示
        /// </summary>
        /// <param name="isHandPoker"></param>
        public void ShowTip(bool isHandPoker)
        {
            if (m_Tip == null)
            {
                string prefabName = "tip";
                string path = string.Format("download/{0}/prefab/model/{1}.drb", ConstDefine.GAME_NAME, prefabName);
                AssetBundleManager.Instance.LoadOrDownload(path, prefabName, (GameObject go) =>
                {
                    m_Tip = Instantiate(go);
                    m_Tip.SetParent(transform);
                    if (isHandPoker)
                    {
                        m_Tip.transform.localPosition = new Vector3(0f, 0f, 8f);
                        m_Tip.transform.localScale = Vector3.one;
                    }
                    else
                    {
                        m_Tip.transform.position = transform.position + new Vector3(0f, 10f, 0f);
                        m_Tip.transform.localScale = new Vector3(3f, 3f, 3f);
                    }
                    m_Tip.SetLayer(gameObject.layer);
                    m_Tip.transform.rotation = Quaternion.identity;
                });
            }
            else
            {
                m_Tip.gameObject.SetActive(true);
                if (isHandPoker)
                {
                    m_Tip.transform.localPosition = new Vector3(0f, 0f, 8f);
                    m_Tip.transform.localScale = Vector3.one;
                }
                else
                {
                    m_Tip.transform.position = transform.position + new Vector3(0f, 10f, 0f);
                    m_Tip.transform.localScale = new Vector3(3f, 3f, 3f);
                }
                m_Tip.SetLayer(gameObject.layer);
                m_Tip.transform.rotation = Quaternion.identity;
            }
        }
        #endregion

        #region CloseTip 关闭提示
        /// <summary>
        /// 关闭提示
        /// </summary>
        public void CloseTip()
        {
            if (m_Tip != null)
            {
                m_Tip.SetActive(false);
            }
        }
        #endregion

        #region Hold 扣牌
        /// <summary>
        /// 扣牌
        /// </summary>
        public void Hold(bool isPlayer, bool isReplay)
        {
            if (m_isPlayingAnimation) return;
            m_isPlayingAnimation = true;

            if (isReplay)
            {
                m_Model.transform.localEulerAngles = new Vector3(180f, 0f, 0f);
            }
            else
            {
                if (isPlayer)
                {
                    if (!m_isHold)
                    {
                        m_Model.transform.DORotate(new Vector3(-150, 0, 0), 0.3f, RotateMode.LocalAxisAdd).OnComplete(() => { m_isPlayingAnimation = false; });
                    }
                    else
                    {
                        m_Model.transform.DORotate(new Vector3(150, 0, 0), 0.3f, RotateMode.LocalAxisAdd).OnComplete(() => { m_isPlayingAnimation = false; });
                    }
                    m_isHold = !m_isHold;
                }
                else
                {
                    m_Model.transform.eulerAngles = new Vector3(0f, -180f, 180f);
                    m_Model.transform.localPosition = new Vector3(0f, 0f, -3f);
                }
            }

        }
        #endregion

        #region Show 平摊牌
        /// <summary>
        /// 平摊牌
        /// </summary>
        /// <param name="isPlayer"></param>
        public void Show(bool isPlayer)
        {
            if (isPlayer)
            {
                m_Model.transform.localEulerAngles = new Vector3(60f, 0f, 0f);
                m_Model.transform.localPosition = Vector3.zero;
            }
            else
            {
                m_Model.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
                m_Model.transform.localPosition = new Vector3(0f, 0f, -2.7f);
            }
        }
        #endregion

        public void Reset()
        {
            Poker = null;
            CloseTip();
            SetUniversal(false, false);
            m_Model.transform.localEulerAngles = Vector3.zero;
            m_Model.transform.localPosition = Vector3.zero;
            m_Model.transform.localScale = Vector3.one;
            m_isHold = false;
            m_isShow = false;
            isGray = false;
            isSelect = false;
            m_isPlayingAnimation = false;
        }


        private bool m_isSelect;
        public bool isSelect
        {
            get { return m_isSelect; }
            set
            {
                if (m_isSelect == value) return;
                m_isSelect = value;
                if (m_isSelect)
                {
                    m_Model.transform.localPosition = new Vector3(0, 0, 2);
                }
                else
                {
                    m_Model.transform.localPosition = Vector3.zero;
                }
            }
        }

        private string m_Color;

        private bool m_isGray;
        public bool isGray
        {
            get { return m_isGray; }
            set
            {
                if (m_isGray == value) return;
                m_isGray = value;
                if (m_isGray)
                {
                    const string materialName = "mj_gray";
                    string materialPath = string.Format("download/{0}/source/modelsource/materials/{1}.drb", ConstDefine.GAME_NAME, materialName);
                    AssetBundleManager.Instance.LoadOrDownload<Material>(materialPath, materialName, (Material material) =>
                    {
                        GetComponentInChildren<Renderer>().material = material;
                    }, 0);
                }
                else
                {
                    Color = m_Color;
                }
            }
        }

        public string Color
        {
            get { return m_Color; }
            set
            {
                m_Color = value;
                string path = string.Format("download/{0}/source/modelsource/materials/mj_{1}.drb", ConstDefine.GAME_NAME, m_Color);
                string name = string.Format("mj_{0}", m_Color);
                AssetBundleManager.Instance.LoadOrDownload(path, name, (Material mat) =>
                {
                    if (mat != null)
                    {
                        GetComponentInChildren<Renderer>().material = mat;
                    }
                }, 0);
            }
        }
    }
}
