//===================================================
//Author      : DRB
//CreateTime  ：4/18/2017 5:11:50 PM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;

public class TestRecord
{
    public int battleId;
    public int roomId;
    public int gameId;
    public int clubId;
    public int maxLoop;
    public int roomType;
    public int ownerId;
    public string ownerName;
    public string time;

    public List<TestRecordDetail> detail = new List<TestRecordDetail>();


    private List<TestPlayer> m_Players;
    public List<TestPlayer> Players
    {
        get
        {
            if (m_Players != null) return m_Players;
            m_Players = new List<TestPlayer>();
            for (int i = 0; i < detail.Count; ++i)
            {
                for (int j = 0; j < detail[i].players.Count; ++j)
                {
                    TestPlayer player = null;
                    for (int k = 0; k < m_Players.Count; ++k)
                    {
                        if (m_Players[k].id == detail[i].players[j].id)
                        {
                            player = m_Players[k];
                            break;
                        }
                    }
                    if (player == null)
                    {
                        player = new TestPlayer();
                        player.nickname = detail[i].players[j].nickname;
                        player.id = detail[i].players[j].id;
                        player.avatar = detail[i].players[j].avatar;
                        player.poker = detail[i].players[j].poker;
                        m_Players.Add(player);
                    }

                    player.gold += detail[i].players[j].gold;
                }
            }

            return m_Players;
        }
    }
}

public class TestRecordDetail : IComparable<TestRecordDetail>
{
    public int id;
    public int loop;
    public string time;
    public List<TestPlayer> players;

    public int CompareTo(TestRecordDetail other)
    {
        if (other == null) return 0;

        return loop - other.loop;
    }
}

public class TestPlayer : IComparable<TestPlayer>
{
    public int id;
    public string nickname;
    public int gold;
    public string avatar;
    public List<TestPoker> poker;

    public int CompareTo(TestPlayer other)
    {
        if (other == null) return 0;

        return other.gold - gold;
    }
}

public class TestPoker
{
    public int color;
    public int size;
}
