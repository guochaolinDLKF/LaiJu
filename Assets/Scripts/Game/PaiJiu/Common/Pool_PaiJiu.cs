//===================================================
//Author      : WZQ
//CreateTime  ：7/7/2017 8:05:59 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaiJiu
{
    public class Pool_PaiJiu  :MonoBehaviour//: Singleton<Pool_PaiJiu>
    {

        private static Pool_PaiJiu instance;

        public static Pool_PaiJiu Instance { get { return instance; } }
        void Awake()
        {
            instance = this;
        }

        private Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>();

        private Vector3 ResetPos = new Vector3(5000, 5000, 0);

        void OnDestroy()
        {
            //清空池
            ClearPool();

        }




        // 取出游戏对象 （物体名字 父物体 位置 角度）

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ObjName 名字"></param>
        /// <param name="prefabPath 路径"></param>
        /// <param name="ObjParent"></param>
        /// <param name="pos"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public GameObject GetObjectFromPool(string ObjName,string prefabPath, Transform ObjParent = null, Vector3 pos = default(Vector3), Quaternion direction = default(Quaternion))
        {
            GameObject go = null;



            if (pool.ContainsKey(ObjName) && pool[ObjName].Count > 0)
            {
                go = pool[ObjName][0];
                pool[ObjName].Remove(go);

            }
            else
            {
                //加载物体
                
                //string prefabPath = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, ObjName);
                go = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(prefabPath, ObjName);
                go = UnityEngine.Object.Instantiate(go);

            }

            if (go != null)
            {
                go.SetActive(true);

                if(ObjParent!=null) go.SetParent(ObjParent);
                go.transform.position = pos;
                go.transform.rotation = direction;
                //if (ObjParent != null)
                //{
                //    go.transform.SetParent(ObjParent);
                //}

            }

            return go;
        }


        /// <summary>
        /// 存放游戏对象 （对象 是否重置）
        /// </summary>
        /// <param name="go"></param>
        /// <param name="isReset"></param>
        public void PushToPool(GameObject go, Transform panent, bool isReset = true)
        {
            if (go == null) return;

            go.SetActive(false);

            //if (panent != null) go.transform.SetParent(panent);
            go.transform.SetParent(panent == null ? this.transform : panent);

            if (isReset)
            {
                go.transform.position = ResetPos;
                go.transform.rotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                //go.transform.localPosition = Vector3.zero;
            }



            string objName = go.name.Split('(')[0];

            if (pool.ContainsKey(objName))
            {
                pool[objName].Add(go);
            }
            else
            {
                pool[objName] = new List<GameObject> { go };
            }



        }


        //清空某物体
        public void ClearItemObj(string ObjName)
        {
            if (pool.ContainsKey(ObjName))
            {
                pool[ObjName].Clear();
                pool.Remove(ObjName);
            }

        }




        public void ClearPool()
        {
            pool.Clear();
        }
    }
}