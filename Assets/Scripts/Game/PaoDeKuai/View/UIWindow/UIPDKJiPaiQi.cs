//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 3:22:35 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace PaoDeKuai
{
    public class UIPDKJiPaiQi : UIWindowViewBase
    {
        [SerializeField]
        private Text[] m_PokersText;
        //[SerializeField]
        //private EventTriggerListener m_BtnClose;


        private Dictionary<int, int> dic;

        private Tweener moveTweener;



        public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
        {
            Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
            dic.Add(ConstDefine_PaoDeKuai.ON_HistoryPoker_CHANGED, HistoryPokerChanged);//历史出牌变更

            return dic;
        }


        public void SetUI(List<Poker> previousPoker)
        {
            if (dic == null)
            {
                dic = new Dictionary<int, int>();
            }
            for (int i = 3; i <= 17; ++i)
            {
                dic[i] = 0;
            }

            for (int i = 0; i < previousPoker.Count; ++i)
            {
                dic[previousPoker[i].size]++;
            }

            for (int i = 0; i < m_PokersText.Length; ++i)
            {
                int sum = -1;
                if (int.TryParse(m_PokersText[i].gameObject.name, out sum))
                {
                    if (dic.ContainsKey(sum))
                    {
                        m_PokersText[i].text =dic[sum].ToString();
                    }
                }
             
            }

            //moveTweener.PlayForward();
        }


        private void HistoryPokerChanged(TransferData data)
        {
            List<Poker> pokers = data.GetValue<List<Poker>>("HistoryPoker");
            if (pokers != null)
            {
                SetUI(pokers);
            }
        }
        //public void Close(GameObject go)
        //{
        //    moveTweener.PlayBackwards();
        //}

    }
}