//===================================================
//Author      : DRB
//CreateTime  ：12/18/2017 4:29:48 PM
//Description ：
//===================================================


public class ShareEntity 
{

    public int id { get; set; }
    /// <summary>
    /// 类型
    /// </summary>
    public int type { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string content { get; set; }

    /// <summary>
    /// 链接
    /// </summary>
    public string url { get; set; }

    /// <summary>
    /// 是否有奖励
    /// </summary>
    public int isReward { get; set; }
}
