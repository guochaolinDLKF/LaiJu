//===================================================
//Author      : DRB
//CreateTime  ：4/7/2017 3:40:21 PM
//Description ：规则模块控制器
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleCtrl : SystemCtrlBase<RuleCtrl>, ISystemCtrl
{

    private UIRuleView m_UIRuleView;
    public void OpenView(UIWindowType type)
    {
        switch (type)
        {
            case UIWindowType.Rule:
                OpenRuleView();
                break;
        }
    }

    private void OpenRuleView()
    {
      
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Rule, (GameObject go) =>
        {
          
            if (m_UIRuleView == null)
            {
                List<cfg_gameEntity> lst = cfg_gameDBModel.Instance.GetList();
                string gameType = lst.Count > 0 ? lst[0].GameType : string.Empty;
                UIRuleView view2 = go.GetComponent<UIRuleView>();
                view2.SetGame(gameType);
                return;
            }
           List<cfg_gameEntity> lstGame = cfg_gameDBModel.Instance.GetList();
            
        });
    }
    public void OpenRuleView(int gameId)
    {
       
        UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Rule, (GameObject go) =>
        {
           
            if (m_UIRuleView == null)
            {
                UIRuleView view2 = go.GetComponent<UIRuleView>();
                if (gameId == 0)
                {
                    view2.SetGame(string.Empty);
                }
                else
                {
                    view2.SetGame(cfg_gameDBModel.Instance.Get(gameId).GameType);
                }
                return;
            }
          
        });
    }



    //private void OpenRuleView()
    //{
    //    UIViewUtil.Instance.LoadWindowAsync(UIWindowType.Rule, (GameObject go) =>
    //    {
    //        m_UIRuleView = go.GetComponent<UIRuleView>();

    //        if (SceneMgr.Instance.CurrentSceneType != SceneType.Main)
    //        {
    //            List<cfg_gameEntity> lst = cfg_gameDBModel.Instance.GetList();
    //            for (int i = 0; i < lst.Count; ++i)
    //            {
    //                if (lst[i].id == GameCtrl.Instance.CurrentGameId)
    //                {
    //                    m_UIRuleView.SetUI(i);
    //                    break;
    //                }
    //            }
    //        }
    //    });
    //}
}
