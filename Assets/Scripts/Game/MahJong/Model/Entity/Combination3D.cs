//===================================================
//Author      : DRB
//CreateTime  ：4/20/2017 5:20:24 PM
//Description ：
//===================================================
using System.Collections.Generic;

namespace DRB.MahJong
{
    public class Combination3D
    {
        public List<MaJiangCtrl> PokerList;

        public OperatorType OperatorType;

        public int SubTypeId;
        public Combination3D(int operatorId, int subTypeId, List<MaJiangCtrl> majiang)
        {
            OperatorType = (OperatorType)operatorId;
            SubTypeId = subTypeId;
            PokerList = majiang;
        }

        public void BuGang(List<MaJiangCtrl> majiang)
        {
            OperatorType = OperatorType.Gang;
            PokerList = majiang;
        }
    }
}
