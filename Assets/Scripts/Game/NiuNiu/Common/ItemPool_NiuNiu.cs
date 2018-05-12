//===================================================
//Author      : WZQ
//CreateTime  ：6/12/2017 5:30:56 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NiuNiu
{

    public class ItemPool_NiuNiu : Singleton<ItemPool_NiuNiu>
    {

        private Dictionary<string, List<GameObject>> pool = new Dictionary<string, List<GameObject>>();

        private Vector3 ResetPos = new Vector3(5000, 5000, 0);

        /// <summary>
        /// 取出游戏对象 （物体名字 父物体 位置 角度）
        /// </summary>
        /// <param name="ObjName"></param>
        /// <param name="ObjParent"></param>
        /// <param name="pos"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public GameObject GetObjectFromPool(string ObjName, Transform ObjParent = null, Vector3 pos = default(Vector3), Quaternion direction = default(Quaternion))
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
                //const string prefabName = "TaurenAni";
                string prefabPath = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, ObjName);
                go = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(prefabPath, ObjName);
                go = UnityEngine.Object.Instantiate(go);

            }

            if (go != null)
            {
                go.SetActive(true);
                go.transform.position = pos;
                go.transform.rotation = direction;
                go.SetParent(ObjParent);
                if (ObjParent != null)
                {
                    go.transform.SetParent(ObjParent);
                }

            }

            return go;
        }


        /// <summary>
        /// 存放游戏对象 （对象 是否重置）
        /// </summary>
        /// <param name="go"></param>
        /// <param name="isReset"></param>
        public void PushToPool(GameObject go, bool isReset = true)
        {
            if (go == null)
            {
                return;
            }

            if (isReset)
            {
                go.transform.position = ResetPos;
                go.transform.rotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                //go.transform.localPosition = Vector3.zero;
            }


            go.SetActive(false);

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