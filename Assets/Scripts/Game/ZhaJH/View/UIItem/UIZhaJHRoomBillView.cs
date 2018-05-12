//===================================================
//Author      : DRB
//CreateTime  ：7/27/2017 11:51:21 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhaJh;

public class UIZhaJHRoomBillView : UIViewBase
{
    [SerializeField]
    private GameObject billGrid;//小屏幕显示退出玩家信息
    [SerializeField]
    private GameObject billDiDa;
    [SerializeField]
    private GameObject mask;

    [SerializeField]
    private GameObject billDida;//大图的挂载点

    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add("BillView", BillView);//高级房加载账单 
        dic.Add("OnRoomZJHInfoChanged", OnRoomZJHInfoChanged);
        return dic;
    }

    //protected override void OnAwake()
    //{
    //    billGrid.SetActive(false);
    //}


    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "billDi":
                billDiDa.SetActive(true);
                mask.SetActive(true);
                break;
            case "mask":
                billDiDa.SetActive(false);
                mask.SetActive(false);
                break;
        }
    }



    private void BillView(TransferData data)
    {
        billGrid.SetActive(true);
        if (billGrid.transform.childCount>=6)
        {
            Destroy(billGrid.transform.GetChild(0).gameObject);
            SeatEntity seat = data.GetValue<SeatEntity>("Seat");
            BillViewSmall(seat);//加载小账单
            BillViewLarge(seat);//加载大账单
        }
        else
        {
            SeatEntity seat = data.GetValue<SeatEntity>("Seat");
            BillViewSmall(seat);
            BillViewLarge(seat);
        }        
    }

    private void BillViewSmall(SeatEntity seat)
    {
        ZJHPrefabManager.Instance.LoadSendBill(seat.PlayerId, seat.gold, seat.Avatar, (GameObject go) =>
        {
            if (billGrid!=null)
            {
                go.transform.SetParent(billGrid.transform);
                go.transform.localScale = Vector3.one;
            }           
        }, "uizjhitemplayerbill");
    }

    private void BillViewLarge(SeatEntity seat)
    {       
        ZJHPrefabManager.Instance.LoadSendBill(seat.PlayerId, seat.gold,seat.Avatar, (GameObject go) =>
        {
            if (billDida != null)
            {
                go.transform.SetParent(billDida.transform);
                go.transform.localScale = Vector3.one;
            }            
        }, "uizjhitemplayerbillda"); 
    }

    private void OnRoomZJHInfoChanged(TransferData data)
    {        
        RoomEntity CurrentRoom = data.GetValue<RoomEntity>("Room");       
        if (CurrentRoom.roomSettingId == RoomMode.Senior && CurrentRoom.billList.Count > 0)
        {
            billGrid.SetActive(true);
        }
        else
        {
            billGrid.SetActive(false);
        }

        if (CurrentRoom.billList.Count>0)
        {
            if (CurrentRoom.billList.Count > 6)
            {
                for (int i = 0; i < CurrentRoom.billList.Count; i++)
                {                   
                    ZJHPrefabManager.Instance.LoadSendBill(CurrentRoom.billList[i].playerIDBill, CurrentRoom.billList[i].pourBill, CurrentRoom.billList[i].avatarBill, (GameObject go) =>
                    {
                        if (billDida != null)
                        {
                            go.transform.SetParent(billDida.transform);
                            go.transform.localScale = Vector3.one;
                        }                        
                    }, "uizjhitemplayerbillda");
                }
                int cout = CurrentRoom.billList.Count - 6;                
                for (int j = 0; j < cout; j++)
                {
                    CurrentRoom.billList.RemoveAt(0);
                }
                for (int i = 0; i < CurrentRoom.billList.Count; i++)
                {
                    ZJHPrefabManager.Instance.LoadSendBill(CurrentRoom.billList[i].playerIDBill, CurrentRoom.billList[i].pourBill, CurrentRoom.billList[i].avatarBill, (GameObject go) =>
                    {
                        if (billGrid != null)
                        {
                            go.transform.SetParent(billGrid.transform);
                            go.transform.localScale = Vector3.one;
                        }                       
                    }, "uizjhitemplayerbill");
                }
            }
            else
            {
                if (CurrentRoom.billList.Count <= 6)
                {
                    for (int i = 0; i < CurrentRoom.billList.Count; i++)
                    {
                        ZJHPrefabManager.Instance.LoadSendBill(CurrentRoom.billList[i].playerIDBill, CurrentRoom.billList[i].pourBill, CurrentRoom.billList[i].avatarBill, (GameObject go) =>
                        {
                            if (billGrid!=null)
                            {
                                go.transform.SetParent(billGrid.transform);
                                go.transform.localScale = Vector3.one;
                            }                            
                        }, "uizjhitemplayerbill");
                        ZJHPrefabManager.Instance.LoadSendBill(CurrentRoom.billList[i].playerIDBill, CurrentRoom.billList[i].pourBill, CurrentRoom.billList[i].avatarBill, (GameObject go) =>
                        {
                            if (billDida!=null)
                            {
                                go.transform.SetParent(billDida.transform);
                                go.transform.localScale = Vector3.one;
                            }                           
                        }, "uizjhitemplayerbillda");
                    }
                }
            }          
            RoomZhaJHProxy.Instance.CurrentRoom.billList.Clear();
        }
       
    }
}
