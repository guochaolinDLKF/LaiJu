//===================================================
//Author      : WZQ
//CreateTime  ：8/7/2017 8:04:15 PM
//Description ：牌数据实体
//===================================================
//using com.oegame.mahjong.protobuf;
using proto.jy;
namespace JuYou
{
    /// <summary>
    /// 麻将
    /// </summary>
    [System.Serializable]
    public class Poker
    {
        #region Public Members
        public int index { get; private set; }

        public int color { get; private set; }

        public int size { get; private set; }

        /// <summary>
        /// 所属座位
        /// </summary>
        //public int pos { get; set; }
        #endregion

        #region Constructor
        public Poker() { }

        public Poker(int color, int size)
        {
            this.index = 0;
            this.color = color;
            this.size = size;
            //this.pos = 0;
        }

        public Poker(int index, int color, int size)
        {
            this.index = index;
            this.color = color;
            this.size = size;
            //this.pos = 0;
        }

        public Poker(int index, int color, int size, int pos)
        {
            this.index = index;
            this.color = color;
            this.size = size;
            //this.pos = pos;
        }

        public Poker(Poker poker)
        {
            if (poker != null)
            {
                index = poker.index;
                color = poker.color;
                size = poker.size;
                //pos = poker.pos;
            }
        }
        #endregion


        #region SetPoker
        public void SetPoker(int index, int color, int size)
        {
            this.index = index;
            this.color = color;
            this.size = size;

        }

        public void SetPoker(JY_POKER jyPoker)
        {
            this.index = jyPoker. index;
            this.color = jyPoker.type;
            this.size = jyPoker.size;

        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return string.Format("{0}_{1}",color, size);
        }

        public string ToString(string format)
        {
            return string.Format(format, index, color, size);
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
                        size = "竹";
                        break;
                    case 4:
                        size = "菊";
                        break;
                }
            }
            return size + color;
        }
        #endregion

        #region ToValue 转化为兜值
        /// <summary>
        /// 转化为兜值
        /// </summary>
        /// <returns></returns>
        public int ToValue()
        {
            return color == 5 ? 1 : size;
        }
        #endregion
    }
}
