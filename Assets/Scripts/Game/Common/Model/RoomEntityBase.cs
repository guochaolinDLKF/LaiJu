//===================================================
//Author      : DRB
//CreateTime  ：6/29/2017 3:21:01 PM
//Description ：房间实体基类
//===================================================
using System.Collections.Generic;

/// <summary>
/// 房间实体基类
/// </summary>
public class RoomEntityBase 
{
    public enum RoomStatus
    {
        /// <summary>
        /// 等待中
        /// </summary>
        Waiting,
        /// <summary>
        /// 游戏中
        /// </summary>
        Gaming,
    }


    /// <summary>
    /// 房间Id
    /// </summary>
    public int roomId;
    /// <summary>
    /// 比赛Id
    /// </summary>
    public int matchId;
    /// <summary>
    /// 游戏Id
    /// </summary>
    public int gameId;
    /// <summary>
    /// 群Id
    /// </summary>
    public int groupId;
    /// <summary>
    /// 房间规则配置
    /// </summary>
    public List<cfg_settingEntity> Config = new List<cfg_settingEntity>();
    /// <summary>
    /// 当前局数
    /// </summary>
    public int currentLoop;
    /// <summary>
    /// 总局数
    /// </summary>
    public int maxLoop;
    /// <summary>
    /// true:圈数,false:局数
    /// </summary>
    public bool isQuan;
    /// <summary>
    /// 解散时间
    /// </summary>
    public long DisbandTime;
    /// <summary>
    /// 解散总计时间
    /// </summary>
    public int DisbandTimeMax;
    /// <summary>
    /// 回放
    /// </summary>
    public bool isReplay;
    /// <summary>
    /// 玩家列表
    /// </summary>
    public List<PlayerEntity> players = new List<PlayerEntity>();

    public string GameName
    {
        get
        {
            string ret = string.Empty;
            cfg_gameEntity entity = cfg_gameDBModel.Instance.Get(gameId);
            if (entity != null)
            {
                ret = entity.GameName;
            }
            return ret;
        }
    }

    public RoomStatus roomStatus;
}
