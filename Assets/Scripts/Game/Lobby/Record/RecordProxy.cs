//===================================================
//Author      : DRB
//CreateTime  ：4/29/2017 3:19:18 PM
//Description ：
//===================================================
using System;
using System.Collections.Generic;
using com.oegame.mahjong.protobuf;
using UnityEngine;


public class RecordProxy  :Singleton<RecordProxy>
{

    public List<TestRecord> AllRecord;

    public RecordReplayEntity CurrentReplayEntity;



    public TestRecord GetRecord(int battleId)
    {
        if (AllRecord == null || AllRecord.Count == 0) return null;
        for (int i = 0; i < AllRecord.Count; ++i)
        {
            if (AllRecord[i].battleId == battleId) return AllRecord[i];
        }
        return null;
    }

    public TestRecordDetail GetRecordDetail(int battleId, int recordId)
    {
        if (AllRecord == null || AllRecord.Count == 0) return null;
        for (int i = 0; i < AllRecord.Count; ++i)
        {
            if (AllRecord[i].battleId == battleId)
            {
                for (int j = 0; j < AllRecord[i].detail.Count; ++j)
                {
                    if (AllRecord[i].detail[j].id == recordId)
                    {
                        return AllRecord[i].detail[j];
                    }
                }
            }
        }
        return null;
    }
}
