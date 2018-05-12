//===================================================
//Author      : WZQ
//CreateTime  ：7/4/2017 1:32:22 PM
//Description ：牌九Poker
//===================================================
using proto.paigow; 

namespace PaiJiu
{
    /// <summary>
    /// 牌九Poker
    /// </summary>
    public class Poker
    {
        //索引
        public int index { get; private set; }

        //权值 2-9
        public int size { get; private set; }

        //条筒
        public int type { get; private set; }

        //是否翻开
         public  PAIGOW_STATUS status = PAIGOW_STATUS.HIDE;

        public Poker() { }


        public Poker(int index, int size, int type, PAIGOW_STATUS status)
        {
            this.index = index;
            this.size = size;
            this.type = type;
            this.status = status;

        }

        public void DefaultValue( )
        {
            this.size = 0;
            this.type = 0;
            this.status = PAIGOW_STATUS.HIDE;
        }

        public Poker(PaiJiu.Poker poker)
        {
            this.index = poker.index;
            this.size = poker.size;
            this.type = poker.type;
            this.status = poker.status;


        }
        public void SetPoker(PaiJiu.Poker poker)
        {

            this.index = poker.index;
            this.size = poker.size;
            this.type = poker.type;
            this.status = poker.status;
        }

        public void SetPoker(PAIGOW_MAHJONG poker)
        {
            if (poker.hasIndex()) this.index = poker.index;
            if (poker.hasSize()) this.size = poker.size;
            if (poker.hasType()) this.type = poker.type;
            if (poker.hasMahjongStatus()) this.status = poker.mahjong_status;
        }

        public override string ToString()
        {
            return string.Format("{0}_{1}", type, size);
        }


        /// <summary>
        /// 转化为中文字符
        /// </summary>
        /// <returns></returns>
        public string ToChinese()
        {
            string chineseType = "";
            string stringSize = this.size.ToString();
            switch (this.type)
            {
                case 0:
                    chineseType = "错误：空牌";
                    break;
                case 1:
                    chineseType = "万";
                    break;
                case 2:
                    chineseType = "筒";
                    break;
                case 3:
                    chineseType = "条";
                    break;

            }

            return stringSize + chineseType;

        }





    }

   
}