using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;

/// <summary>
/// 窗口UI管理器
/// </summary>
public class UIViewUtil : Singleton<UIViewUtil>
{
    private Dictionary<string, UIWindowViewBase> m_DicWindow = new Dictionary<string, UIWindowViewBase>();

    private AnimationCurve m_DefaultAnimationCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0,0),new Keyframe(1,1)});

    public string NextOpenViewName;
    /// <summary>
    /// 已经打开的窗口数量
    /// </summary>
    public int OpenWindowCount
    {
        get
        {
            return m_DicWindow.Count;
        }
    }

    #region Public Function

    #region OpenWindow 打开窗口(Resources)
    /// <summary>
    /// 打开窗口(Resources)
    /// </summary>
    /// <param name="type">窗口类型</param>
    /// <returns>窗口对象</returns>
    public GameObject OpenWindow(UIWindowType type)
    {
        if (type == UIWindowType.None) return null;

        return OpenWindow(type.ToString());
    }

    /// <summary>
    /// 打开窗口(Resources)
    /// </summary>
    /// <param name="name">窗口名称</param>
    /// <returns></returns>
    public GameObject OpenWindow(string name)
    {
        if (string.IsNullOrEmpty(name)) return null;
        name = name.ToLower();
        GameObject obj = null;
        //如果窗口不存在 则
        if (!m_DicWindow.ContainsKey(name) || m_DicWindow[name] == null)
        {
            //枚举的名称要和预设的名称对应
            obj = ResourcesManager.Instance.Load(ResourceType.UIWindow, string.Format("pan_{0}", name), true);
            if (obj == null) return null;
            UIWindowViewBase windowBase = obj.GetComponent<UIWindowViewBase>();
            if (windowBase == null) return null;

            m_DicWindow[name] = windowBase;

            windowBase.WindowName = name;
            Transform transParent = null;

            switch (windowBase.containerType)
            {
                case ContainerType.Center:
                    transParent = UIViewManager.Instance.CurrentUIScene.Container_Center;
                    break;
                case ContainerType.TopRight:
                    transParent = UIViewManager.Instance.CurrentUIScene.Container_TopRight;
                    break;
            }

            obj.transform.SetParent(transParent);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            obj.gameObject.SetActive(false);

            StartShowWindow(windowBase, true);
        }
        else
        {
            obj = m_DicWindow[name].gameObject;
        }
        //层级管理
        LayerUIManager.Instance.SetLayer(obj);

        return obj;
    }
    #endregion

    #region LoadWindow 同步加载窗口
    /// <summary>
    /// 同步加载窗口
    /// </summary>
    /// <returns>窗口类型</returns>
    public GameObject LoadWindow(UIWindowType type)
    {
        if (type == UIWindowType.None) return null;
        return LoadWindow(type.ToString());
    }

    /// <summary>
    /// 同步加载窗口
    /// </summary>
    /// <param name="name">窗口名称</param>
    /// <returns></returns>
    public GameObject LoadWindow(string name)
    {
        if (string.IsNullOrEmpty(name)) return null;
        name = name.ToLower();
        GameObject go = null;
        UIWindowViewBase windowBase = null;
        //如果窗口不存在 则
        if (!m_DicWindow.ContainsKey(name) || m_DicWindow[name] == null)
        {
            string windowName = string.Format("pan_{0}", name);
            string path = string.Format("download/{0}/prefab/uiprefab/uiwindows/{1}.drb", ConstDefine.GAME_NAME, windowName);
            go = AssetBundleManager.Instance.LoadAssetBundle<GameObject>(path, windowName);
            go = UnityEngine.Object.Instantiate(go);
            windowBase = go.GetComponent<UIWindowViewBase>();
            m_DicWindow[name] = windowBase;
            windowBase.WindowName = name;
            Transform transParent = null;
            switch (windowBase.containerType)
            {
                case ContainerType.Center:
                    transParent = UIViewManager.Instance.CurrentUIScene.Container_Center;
                    break;
                case ContainerType.TopRight:
                    transParent = UIViewManager.Instance.CurrentUIScene.Container_TopRight;
                    break;
                case ContainerType.Left:
                    transParent = UIViewManager.Instance.CurrentUIScene.Container_Left;
                    break;
            }
            go.SetParent(transParent);
            go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        }
        else
        {
            windowBase = m_DicWindow[name];
            go = windowBase.gameObject;
            windowBase.Show();
        }
        LayerUIManager.Instance.SetLayer(go);
        StartShowWindow(windowBase, true);
        return go;
    }
    #endregion

    #region LoadWindowAsync 异步加载窗口
    /// <summary>
    /// 异步加载窗口
    /// </summary>
    /// <param name="type">窗口类型</param>
    /// <param name="onComplete">加载完毕回调</param>
    public void LoadWindowAsync(UIWindowType type,Action<GameObject> onComplete)
    {
        if (type == UIWindowType.None) return;
        LoadWindowAsync(type.ToString(), onComplete);
    }

    /// <summary>
    /// 加载窗口
    /// </summary>
    /// <param name="name">窗口名称</param>
    /// <param name="onComplete">加载完毕回调</param>
    public void LoadWindowAsync(string name, Action<GameObject> onComplete)
    {
        if (string.IsNullOrEmpty(name)) return;
        name = name.ToLower();
        if (!m_DicWindow.ContainsKey(name) || m_DicWindow[name] == null)
        {
            string windowName = string.Format("pan_{0}", name);
            string path = string.Format("download/{0}/prefab/uiprefab/uiwindows/{1}.drb", ConstDefine.GAME_NAME, windowName);
            AssetBundleManager.Instance.LoadOrDownload(path, windowName, (GameObject go) =>
            {
                go = UnityEngine.Object.Instantiate(go);

                UIWindowViewBase windowBase = go.GetComponent<UIWindowViewBase>();
                if (windowBase == null) return;

                m_DicWindow[name] = windowBase;

                windowBase.WindowName = name;
                Transform transParent = null;

                switch (windowBase.containerType)
                {
                    case ContainerType.Center:
                        transParent = UIViewManager.Instance.CurrentUIScene.Container_Center;
                        break;
                    case ContainerType.TopRight:
                        transParent = UIViewManager.Instance.CurrentUIScene.Container_TopRight;
                        break;
                    case ContainerType.Left:
                        transParent = UIViewManager.Instance.CurrentUIScene.Container_Left;
                        break;
                }

                go.SetParent(transParent);
                go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                //层级管理
                LayerUIManager.Instance.SetLayer(go);
                go.gameObject.SetActive(false);
                StartShowWindow(windowBase, true);
                if (onComplete != null)
                {
                    onComplete(go);
                }
            });

        }
        else
        {
            UIWindowViewBase windowBase = m_DicWindow[name];
            GameObject go = windowBase.gameObject;
            windowBase.Show();
            LayerUIManager.Instance.SetLayer(go);
            StartShowWindow(windowBase, true);
            if (onComplete != null)
            {
                onComplete(go);
            }
        }
    }
    #endregion

    #region CloseWindow 关闭窗口
    /// <summary>
    /// 关闭窗口
    /// </summary>
    /// <param name="type"></param>
    public void CloseWindow(string name)
    {
        if (m_DicWindow.ContainsKey(name))
        {
            StartShowWindow(m_DicWindow[name], false);
        }
    }
    #endregion

    #region ClearWindows 清空窗口
    /// <summary>
    /// 清空窗口
    /// </summary>
    public void ClearWindows()
    {
        List<string> lst = new List<string>();
        foreach (var pair in m_DicWindow)
        {
            if (pair.Value != null && pair.Value.persistenceType < UIWindowPersistenceType.LoadSceneHide)
            {
                lst.Add(pair.Key);
                UnityEngine.Object.Destroy(pair.Value.gameObject);
            }
            if (pair.Value != null && pair.Value.persistenceType == UIWindowPersistenceType.LoadSceneHide)
            {
                pair.Value.Hide();
            }
        }
        for (int i = 0; i < lst.Count; ++i)
        {
            m_DicWindow.Remove(lst[i]);
        }
        if (m_DicWindow.Count == 0)
        {
            LayerUIManager.Instance.Reset();
        }
    }
    #endregion

    #endregion

    #region Private Function

    #region StartShowWindow 开始打开窗口
    /// <summary>
    /// 开始打开窗口
    /// </summary>
    /// <param name="windowBase"></param>
    /// <param name="isOpen">是否打开</param>
    private void StartShowWindow(UIWindowViewBase windowBase, bool isOpen)
    {
        switch (windowBase.showStyle)
        {
            case UIWindowShowStyleType.Normal:
                ShowNormal(windowBase, isOpen);
                break;
            case UIWindowShowStyleType.CenterToBig:
                ShowNormal(windowBase, isOpen);
                break;
            case UIWindowShowStyleType.FromTop:
                ShowFromDir(windowBase, 0, isOpen);
                break;
            case UIWindowShowStyleType.FromDown:
                ShowFromDir(windowBase, 1, isOpen);
                break;
            case UIWindowShowStyleType.FromLeft:
                ShowFromDir(windowBase, 2, isOpen);
                break;
            case UIWindowShowStyleType.FromRight:
                ShowFromDir(windowBase, 3, isOpen);
                break;
        }
    }
    #endregion

    #region 各种打开效果
    /// <summary>
    /// 正常打开
    /// </summary>
    /// <param name="windowBase"></param>
    /// <param name="isOpen"></param>
    private void ShowNormal(UIWindowViewBase windowBase, bool isOpen)
    {
        if (isOpen)
        {
            windowBase.gameObject.SetActive(true);
        }
        else
        {
            DestroyWindow(windowBase);
        }
    }

    /// <summary>
    /// 中间变大
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="isOpen"></param>
    private void ShowCenterToBig(UIWindowViewBase windowBase, bool isOpen)
    {
        windowBase.gameObject.SetActive(true);
        windowBase.transform.localScale = Vector3.zero;
        windowBase.transform.DOScale(Vector3.one, windowBase.duration)
            .SetAutoKill(false)
            .SetEase(m_DefaultAnimationCurve)
            .Pause().OnRewind(() =>
        {
            DestroyWindow(windowBase);
        });

        if (isOpen)
            windowBase.transform.DOPlayForward();
        else
            windowBase.transform.DOPlayBackwards();
    }

    /// <summary>
    /// 从不同的方向加载
    /// </summary>
    /// <param name="windowBase"></param>
    /// <param name="dirType">0=从上 1=从下 2=从左 3=从右</param>
    /// <param name="isOpen"></param>
    private void ShowFromDir(UIWindowViewBase windowBase, int dirType, bool isOpen)
    {
        windowBase.gameObject.SetActive(true);
        Vector3 from = Vector3.zero;
        switch (dirType)
        {
            case 0:
                from = new Vector3(0, 1000, 0);
                break;
            case 1:
                from = new Vector3(0, -1000, 0);
                break;
            case 2:
                from = new Vector3(-1400, 0, 0);
                break;
            case 3:
                from = new Vector3(1400, 0, 0);
                break;
        }
        windowBase.transform.localPosition = from;

        windowBase.transform.DOLocalMove(Vector3.zero, windowBase.duration)
            .SetAutoKill(false)
            .SetEase(m_DefaultAnimationCurve)
            .Pause().OnRewind(() =>
        {
            DestroyWindow(windowBase);
        });
        if (isOpen)
            windowBase.transform.DOPlayForward();
        else
            windowBase.transform.DOPlayBackwards();
    }

    #endregion

    #region DestroyWindow 销毁窗口
    /// <summary>
    /// 销毁窗口
    /// </summary>
    /// <param name="obj"></param>
    private void DestroyWindow(UIWindowViewBase windowBase)
    {
        switch (windowBase.persistenceType)
        {
            case UIWindowPersistenceType.Once:
                m_DicWindow.Remove(windowBase.WindowName);
                UnityEngine.Object.Destroy(windowBase.gameObject);
                break;
            case UIWindowPersistenceType.LoadSceneDestroy:
                windowBase.Hide();
                break;
            case UIWindowPersistenceType.LoadSceneHide:
                windowBase.Hide();
                break;
            case UIWindowPersistenceType.LongStay:
                windowBase.Hide();
                break;
        }

        if (UIViewManager.Instance.OnWindowClose != null)
        {
            UIViewManager.Instance.OnWindowClose(windowBase.WindowName);
        }

        LayerUIManager.Instance.CheckOpenWindow();

        if (!string.IsNullOrEmpty(NextOpenViewName))
        {
            UIViewManager.Instance.OpenWindow((UIWindowType)Enum.Parse(typeof(UIWindowType),NextOpenViewName,true));
            NextOpenViewName = string.Empty;
        }
    }
    #endregion

    #endregion
}