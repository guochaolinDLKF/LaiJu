//===================================================
//Author      : DRB
//CreateTime  ：12/5/2017 4:38:59 PM
//Description ：重新发牌命令
//===================================================
using System;

namespace DRB.DouDiZhu
{
    public class ReDealCommand : IGameCommand
    {
        public void Execute()
        {
            LogSystem.LogWarning("执行重新发牌命令");
            //RoomProxy.Instance.ClearPoker();
        }

        public void Revoke()
        {
            throw new NotImplementedException();
        }
    }
}
