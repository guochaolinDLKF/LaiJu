//===================================================
//Author      : DRB
//CreateTime  ：7/25/2017 2:26:30 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DRB.DouDiZhu
{
    public class DouDZSceneCtrl : SceneCtrlBase
    {

        public static DouDZSceneCtrl Instance;
        private UISceneDouDZView m_UISceneDouDZView;

        protected override void OnAwake()
        {
            AudioBackGroundManager.Instance.Play("bgm_doudizhu");
            base.OnAwake();
        }


        protected override void OnStart()
        {
            base.OnStart();
            if (DelegateDefine.Instance.OnSceneLoadComplete != null)
            {
                DelegateDefine.Instance.OnSceneLoadComplete();
            }

            m_AI = new DouDiZhuGameAI();

            GameObject go = UIViewManager.Instance.LoadSceneUIFromAssetBundle(UIViewManager.SceneUIType.DouDiZhu);
            m_UISceneDouDZView = go.GetComponent<UISceneDouDZView>();
            m_UISceneDouDZView.Reset();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (DouDiZhuGameCtrl.Instance.CommandQueue.Count > 0)
            {
                IGameCommand command = DouDiZhuGameCtrl.Instance.CommandQueue.Dequeue();
                command.Execute();
            }
        }
    }
}
