//===================================================
//Author      : DRB
//CreateTime  ：9/27/2017 5:03:06 PM
//Description ：
//===================================================
using System.Collections.Generic;
using UnityEngine;


public class ChatGroupUnitTest : MonoBehaviour 
{


#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.O))
        {
            UIViewManager.Instance.OpenWindow(UIWindowType.ChatGroup);
        }

        //if (Input.GetKeyUp(KeyCode.Alpha2))
        //{
        //    ChatGroupProxy.Instance.AddGroup(new ChatGroupEntity(1, "哈哈", "", 1,5,20,0,true,0));
        //}
        //if (Input.GetKeyUp(KeyCode.Alpha3))
        //{
        //    ChatGroupProxy.Instance.RemoveGroup(1);
        //}

        //if (Input.GetKeyUp(KeyCode.Alpha4))
        //{
        //    ChatGroupProxy.Instance.AddMember(1,new PlayerEntity() { id = 1,nickname = "hehe",online = 1});
        //}

        //if (Input.GetKeyUp(KeyCode.Alpha5))
        //{
        //    ChatGroupProxy.Instance.AddRoom(1, new RoomEntityBase()
        //    {
        //        gameId = 22,maxLoop = 8, currentLoop = 2,roomId = 1
        //    },new List<int>() { 1,0,5,4,0,2});
        //}
    }
#endif
}
