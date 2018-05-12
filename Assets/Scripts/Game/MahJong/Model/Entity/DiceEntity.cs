//===================================================
//Author      : DRB
//CreateTime  ：6/16/2017 6:49:10 PM
//Description ：骰子数据实体
//===================================================

public class DiceEntity
{
    public int seatPos;

    public int diceA;

    public int diceB;

    public int diceTotal { get { return diceA + diceB; } }
}
