//===================================================
//Author      : DRB
//CreateTime  ：4/25/2017 8:04:15 PM
//Description ：麻将牌数据实体
//===================================================

namespace DRB.MahJong
{
    /// <summary>
    /// 麻将
    /// </summary>
    [System.Serializable]
    public class Poker : PokerBase
    {
        [UnityEngine.SerializeField]
        private int m_Pos;

        public int pos { get { return m_Pos; } set { m_Pos = value; } }

        public Poker() : base() { }

        public Poker(int color, int size) : base(color, size)
        {
            m_Pos = 0;
        }

        public Poker(int index, int color, int size) : base(index, color, size)
        {
            m_Pos = 0;
        }

        public Poker(int index, int color, int size, int pos):base(index,color,size)
        {
            m_Pos = pos;
        }

        public Poker(Poker poker)
        {
            if (poker != null)
            {
                m_Index = poker.index;
                m_Color = poker.color;
                m_Size = poker.size;
                pos = poker.pos;
            }
        }

        #region ToString

        public string ToString(string format)
        {
            return string.Format(format, index.ToString(), color.ToString(), size.ToString(), pos.ToString());
        }

        public string ToString(bool containIndex, bool containPos)
        {
            return string.Format("{0}{1}_{2}{3}", containIndex ? index.ToString() + "_" : string.Empty, color.ToString(), size.ToString(), containPos ? "_" + pos.ToString() : string.Empty);
        }
        #endregion

        #region ToChinese 转换成中文字符串
        /// <summary>
        /// 转换成中文字符串
        /// </summary>
        /// <returns></returns>
        public string ToChinese()
        {
            string color = string.Empty;
            string size = this.size.ToString();
            switch (this.color)
            {
                case 1:
                    color = "万";
                    break;
                case 2:
                    color = "筒";
                    break;
                case 3:
                    color = "条";
                    break;
                case 4: //东西南北
                    size = "风";
                    break;
                case 5: //中发白
                    color = "";
                    break;
                case 6: //春夏秋冬
                    color = "";
                    break;
                case 7: //梅兰竹菊
                    color = "";
                    break;
            }

            if (this.color == 4)
            {
                switch (this.size)
                {
                    case 1:
                        size = "东";
                        break;
                    case 2:
                        size = "南";
                        break;
                    case 3:
                        size = "西";
                        break;
                    case 4:
                        size = "北";
                        break;
                }
            }
            if (this.color == 5)
            {
                switch (this.size)
                {
                    case 1:
                        size = "红中";
                        break;
                    case 2:
                        size = "发财";
                        break;
                    case 3:
                        size = "白板";
                        break;
                }
            }
            if (this.color == 6)
            {
                switch (this.size)
                {
                    case 1:
                        size = "春";
                        break;
                    case 2:
                        size = "夏";
                        break;
                    case 3:
                        size = "秋";
                        break;
                    case 4:
                        size = "冬";
                        break;
                }
            }
            if (this.color == 7)
            {
                switch (this.size)
                {
                    case 1:
                        size = "梅";
                        break;
                    case 2:
                        size = "兰";
                        break;
                    case 3:
                        size = "菊";
                        break;
                    case 4:
                        size = "竹";
                        break;
                }
            }
            return size + color;
        }
        #endregion

        public override int GetHashCode()
        {
            return string.Format("{0}_{1}", m_Color, m_Size).GetHashCode();
        }
    }
}
