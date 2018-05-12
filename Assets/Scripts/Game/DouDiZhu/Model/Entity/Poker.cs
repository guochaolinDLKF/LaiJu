//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 3:40:04 PM
//Description ：
//===================================================
using System;

namespace DRB.DouDiZhu
{
    [Serializable]
    public class Poker : PokerBase
    {

        public int power
        {
            get
            {
                int ret = size;
                if (ret == 1) ret = 14;
                else if (ret == 2) ret = 15;
                else if (ret == 14) ret = 16;
                else if (ret == 15) ret = 17;
                return ret;
            }
        }

        public Poker(int color, int size) : base(color, size)
        {
            m_Index = 0;
            m_Color = color;
            m_Size = size;
        }

        public Poker(int index, int color, int size) : base(index, color, size)
        {
            m_Index = index;
            m_Color = color;
            m_Size = size;
        }
        public Poker(int index, int color, int size,bool isUniversal) : base(index, color, size,isUniversal)
        {
            m_Index = index;
            m_Color = color;
            m_Size = size;
            m_IsUniversal = isUniversal;
        }

        public override int CompareTo(object other)
        {
            if (other == null) return -1;
            if (!(other is Poker)) return -1;
            Poker otherPoker = other as Poker;
            if (power - otherPoker.power == 0)
            {
                return color - otherPoker.color;
            }
            return power - otherPoker.power;
        }
    }
}
