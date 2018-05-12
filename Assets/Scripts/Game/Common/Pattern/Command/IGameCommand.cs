//===================================================
//Author      : DRB
//CreateTime  ：7/6/2017 7:17:41 PM
//Description ：游戏命令接口
//===================================================


public interface IGameCommand 
{
    /// <summary>
    /// 执行
    /// </summary>
    void Execute();
    /// <summary>
    /// 撤销
    /// </summary>
    void Revoke();
}
