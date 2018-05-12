//===================================================
//Author      : DRB
//CreateTime  ：11/29/2017 4:58:36 PM
//Description ：申请解散指令
//===================================================

using UnityEngine;
using proto.pdk;
namespace PaoDeKuai
{
    public class ApplyDisbandCommand : IGameCommand
    {
        private PDK_APPLY_DISMISS m_DataEntity;

        public ApplyDisbandCommand(PDK_APPLY_DISMISS data)
        {
            m_DataEntity = data;
        }
        public void Execute()
        {
            Debug.Log("未实现申请解散指令");

        }

        public void Revoke()
        {

        }

    }
}