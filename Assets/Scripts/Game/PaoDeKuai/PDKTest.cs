//===================================================
//Author      : DRB
//CreateTime  ：12/1/2017 11:00:33 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PaoDeKuai
{
    public class PDKTest : MonoBehaviour
    {

        List<Poker> pokerList = new List<Poker>();//一副牌
        List<Poker> m_HandPoksers;//手牌


        HintPokersEntity hint;//提示实体


        // Use this for initialization
        void Start()
        {
            CreatePoker();

            List<Poker> othersPokers = CreateOthers();

            m_HandPoksers =CreateHandPoksers();

            CombinationPokersEntity m_others=new CombinationPokersEntity ( 3, othersPokers, PokersType.None,0);//上家出的牌
            PaoDeKuaiHelper.CheckPokerType(m_others);
            Debug.Log(string.Format("测试的上家牌型为{0} 大小为{1}",m_others.PokersType,m_others.CurrSize));


            hint = new HintPokersEntity(m_others);

     




        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.A))
            {
                PaoDeKuaiHelper.HintPoker(hint, m_HandPoksers);
                Debug.Log("当前提示级别"+ hint.CurrHintLevel.ToString());
                Debug.Log("当前期望提示级别"+ hint.ExpectHintLevel.ToString());
                Debug.Log("提示牌型："+hint.CurrHint.PokersType.ToString());
                Debug.Log("提示大小："+ hint.CurrHint.CurrSize);
                Debug.Log("提示牌：" );
                for (int i = 0; i < hint.CurrHint.Pokers.Count; i++)
                {
                   Debug.Log(hint.CurrHint.Pokers[i].ToChinese());

                }

            }

        }

        //创建上家出牌
        private List<Poker> CreateOthers()
        {
            List<Poker> othersPokers = new List<Poker>();

            othersPokers.Add(new Poker(1, 3, 1));
            othersPokers.Add(new Poker(1, 3, 2));
            othersPokers.Add(new Poker(1, 4, 1));
            othersPokers.Add(new Poker(1, 4, 2));
            //othersPokers.Add(new Poker(1, 4, 3));
            //othersPokers.Add(new Poker(1, 5, 1));
            //othersPokers.Add(new Poker(1, 5, 2));
            //othersPokers.Add(new Poker(1, 5, 3));
            //othersPokers.Add(new Poker(1, 5, 4));
            //othersPokers.Add(new Poker(1, 6, 3));
            //othersPokers.Add(new Poker(1, 7, 3));
            //othersPokers.Add(new Poker(1, 7, 4));
            //othersPokers.Add(new Poker(1, 9, 4));
            //othersPokers.Add(new Poker(1, 10, 1));
            //othersPokers.Add(new Poker(1, 11, 2));
            ////othersPokers.Add(new Poker(1, 11, 3));
            //othersPokers.Add(new Poker(1, 12, 1));

            //othersPokers.Add(new Poker(1, 13, 3));
            return othersPokers;
        }


        //创建自己手牌
        private List<Poker> CreateHandPoksers()
        {
            List<Poker> handPoksers = new List<Poker>();

            handPoksers.Add(new Poker(1, 3, 1));
            handPoksers.Add(new Poker(1, 3, 2));
            handPoksers.Add(new Poker(1, 4, 1));
            handPoksers.Add(new Poker(1, 4, 2));
            handPoksers.Add(new Poker(1, 5, 3));
            handPoksers.Add(new Poker(1, 5, 4));
            handPoksers.Add(new Poker(1, 6, 1));
            handPoksers.Add(new Poker(1, 6, 2));
            handPoksers.Add(new Poker(1, 6, 3));
            handPoksers.Add(new Poker(1, 9, 1));
            handPoksers.Add(new Poker(1, 9, 4));
            handPoksers.Add(new Poker(1, 10, 2));
            handPoksers.Add(new Poker(1, 10, 3));
            handPoksers.Add(new Poker(1, 11, 3));
            handPoksers.Add(new Poker(1, 12, 1));
 
            //handPoksers.Add(new Poker(1, 11, 1));
            //handPoksers.Add(new Poker(1, 11, 2));
            //handPoksers.Add(new Poker(1, 11, 3));
            //handPoksers.Add(new Poker(1, 12, 2));
            //handPoksers.Add(new Poker(1, 12, 3));
            //handPoksers.Add(new Poker(1, 12, 4));
            handPoksers.Add(new Poker(1, 13, 1));

            handPoksers.Add(new Poker(1, 14, 3));
            return handPoksers;

        }





        private List<Poker> CreatePoker()
        {
            pokerList.Clear();

            int index = 0;
            for (int i = 3; i < 18; ++i)
            {
                for (int j = 0; j < 5; ++j)
                {
                    if (i != 0 && j == 0) continue;
                    if (i > 15 && j != 1) continue;
                    ++index;
                    Poker poker = new Poker(index,i, j);
                    pokerList.Add(poker);
                    if (i == 0) break;
                }
            }
            return pokerList;
        }


    }
}