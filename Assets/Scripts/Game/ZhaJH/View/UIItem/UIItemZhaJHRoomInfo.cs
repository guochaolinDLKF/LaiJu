//===================================================
//Author      : CZH
//CreateTime  ：6/14/2017 11:18:48 AM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZhaJh;
using zjh.proto;
using DG.Tweening;

public class UIItemZhaJHRoomInfo : UIItemRoomInfoBase
{
    public static UIItemZhaJHRoomInfo Instance;

    [SerializeField]
    protected Text baseScore;
    [SerializeField]
    protected Transform chipTran;
    [SerializeField]
    private GameObject beautyBG;
    [SerializeField]
    private GameObject[] tableBG;
    [SerializeField]
    private Transform btnZhaJHViewShareTran;
    [SerializeField]
    private GameObject pokerList;
    [SerializeField]
    private Transform pokerListTran;
    [SerializeField]
    private Text totalRoundText;//总轮数
    [SerializeField]
    private Text m_roomType;//房间模式
    [SerializeField]
    private Text m_Shopping;//是否血拼
    [SerializeField]
    private Text m_Stuffy;//必闷轮数
    [SerializeField]
    private Image ShoppingImage;//血拼图片
    private Tweener ShoppingImageTween;


    protected override void OnAwake()
    {
        base.OnAwake();
        Instance = this;
        ShoppingImageTween= ShoppingImage.rectTransform.DOMove(ShoppingImage.rectTransform.position + new Vector3(0, 100, 0), 0.7f).SetEase(Ease.Linear).SetAutoKill(false).Pause();
        // ModelDispatcher.Instance.AddEventListener("OnOverplusWallCountChange", OnOverplusWallCountChange);
        //ModelDispatcher.Instance.AddEventListener("OnRoomInfoChanged", OnRoomZJHInfoChanged);
        ModelDispatcher.Instance.AddEventListener(ZhaJHMethodname.OnZJHShoppingImageTweenMethod, ShoppingImageTweenMethod);
        ModelDispatcher.Instance.AddEventListener(ZhaJHMethodname.OnZJHRoomInfoChanged, OnRoomInfoChanged);


    }
    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        //ModelDispatcher.Instance.RemoveEventListener("OnRoomInfoChanged", OnRoomZJHInfoChanged);
        ModelDispatcher.Instance.RemoveEventListener(ZhaJHMethodname.OnZJHShoppingImageTweenMethod, ShoppingImageTweenMethod);
        ModelDispatcher.Instance.RemoveEventListener(ZhaJHMethodname.OnZJHRoomInfoChanged, OnRoomInfoChanged);
    }

#if IS_WANGPAI
    //private void OnRoomZJHInfoChanged(TransferData data)
    //{
    //    RoomEntity CurrentRoom = data.GetValue<RoomEntity>("Room");
           
    //}
#else
    // private void OnRoomZJHInfoChanged(TransferData data)
    //{
    //    RoomEntity CurrentRoom = data.GetValue<RoomEntity>("Room");
    //    if (CurrentRoom.roomSettingId==1)
    //    {
    //        beautyBG.SetActive(true);
    //    }
    //    else if (CurrentRoom.roomSettingId == 2)
    //    {
    //        beautyBG.SetActive(false);
    //        btnZhaJHViewShareTran.localPosition= new Vector3(0, btnZhaJHViewShareTran.localPosition.y,0);
    //    }

    //    if (RoomZhaJHProxy.Instance.PlayerSeat.pos==7)
    //    {
    //        tableBG[0].SetActive(false);
    //        tableBG[1].SetActive(true);
    //        pokerList.transform.localPosition = pokerListTran.localPosition;
    //    }
    //    else
    //    {
    //        tableBG[1].SetActive(false);
    //        tableBG[0].SetActive(true);
    //    }       
    //}
#endif


    public void SetBaseScoreUI(float score)
    {
        baseScore.SafeSetText(string.Format("总分:{0}", score.ToString()));        
    }

    public void SetTotalRound(int round ,int totalRound)
    {
        string roomTypeName = string.Empty;
        switch (RoomZhaJHProxy.Instance.CurrentRoom.roomSettingId)
        {
            case RoomMode.Ordinary:
                roomTypeName = "普通玩法";
                break;
            case RoomMode.Senior:
                roomTypeName = "高级玩法";
                break;
            case RoomMode.Passion:
                roomTypeName = "激情玩法";
                break;
            default:
                break;
        }       
        string m_ShoppingName = RoomZhaJHProxy.Instance.isShopping == 1&& RoomZhaJHProxy.Instance.CurrentRoom.roomSettingId!= RoomMode.Senior ? "是" : "否";
        m_Stuffy.SafeSetText(string.Format("必闷轮数:{0}轮",RoomZhaJHProxy.Instance.SureWheelNumber));
        m_roomType.SafeSetText(string.Format("房间模式:{0}", roomTypeName));
        m_Shopping.SafeSetText(string.Format("是否血拼:{0}", m_ShoppingName));

        //if (round==0&& RoomZhaJHProxy.Instance.CurrentRoom.roomStatus != ENUM_ROOM_STATUS.IDLE)
        //{
        //    round = 1;           
        //}
        Debug.Log(round+"                 轮数");
        totalRoundText.SafeSetText(string.Format("第{0}/{1}轮", round.ToString(), totalRound.ToString()));
    }





    /// <summary>
    /// 中间加入的时候实例化筹码
    /// </summary>
    /// <param name="betPoints"></param>
    public void SeatChip(float betPoints)
    {
        Remainder(betPoints);
        if (Twenty!=0)
        {
            InstantiationChip(Twenty, 20);
        }
        if (ten != 0)
        {
            InstantiationChip(ten, 10);
        }
        if (five != 0)
        {
            InstantiationChip(five, 5);
        }
        if (two != 0)
        {
            InstantiationChip(two, 2);
        }
        if (one != 0)
        {
            InstantiationChip(one, 1);
        }


    }

    /// <summary>
    /// 实例化筹码显示
    /// </summary>
    /// <param name="number"></param>
    /// <param name="score"></param>
    public void InstantiationChip(float number, int score)
    {
        for (int i = 0; i < number; i++)
        {
            int randomx = UnityEngine.Random.Range(-100, 100);
            int randomy = UnityEngine.Random.Range(-30, 30);
            GameObject go = Instantiate(ZJHPrefabManager.Instance.LoadChip(score), new Vector3(chipTran.position.x+randomx, chipTran.position.y+randomy,0) , Quaternion.identity);
            go.transform.SetParent(chipTran);
            go.transform.localScale = Vector3.one;
                     
        }
    }

    /// <summary>
    /// 计算各个分数筹码的个数
    /// </summary>
    float ten = 0;
    float five = 0;
    float two = 0;
    float one = 0;
    float Twenty = 0;//二十
    private float Remainder(float betPoints)
    {
        if (betPoints >= 20 )
        {
            float remainderTwenty = betPoints % 20;
            Twenty = (betPoints - remainderTwenty) / 20;
            return Remainder(remainderTwenty);
        }
        else if (betPoints >= 10&& betPoints < 20)
        {
            float remainderTen = betPoints % 10;
            ten = (betPoints - remainderTen) / 10;
            return Remainder(remainderTen);
        }
        else if (betPoints >= 5 && betPoints < 10)
        {
            float remainderFive = betPoints % 5;
            five = (betPoints - remainderFive) / 5;
            return Remainder(remainderFive);
        }
        else if (betPoints >= 2 && betPoints < 5)
        {
            float remaindertwo = betPoints % 2;
            two = (betPoints - remaindertwo) / 2;
            return Remainder(remaindertwo);
        }
        else
        {
            one = betPoints;
            return one;
        }
    }

    /// <summary>
    /// 显示血拼图片
    /// </summary>
    private void ShoppingImageTweenMethod(TransferData data)
    {
        if (RoomZhaJHProxy.Instance.isShopping==1)
        {
            ShoppingImage.gameObject.SetActive(true);
            if (ShoppingImageTween != null) ShoppingImageTween.Restart();
            StartCoroutine("ShoppingImageTweenTor");
        }       
    }
    IEnumerator ShoppingImageTweenTor()
    {
        yield return new  WaitForSeconds(2);
        ShoppingImage.gameObject.SetActive(false);
    }


    /// <summary>
    /// 房间信息变更
    /// </summary>
    /// <param name="obj"></param>
    private void OnRoomInfoChanged(TransferData data)
    {
        RoomEntity room = data.GetValue<RoomEntity>("Room");       
        base.ShowLoop(room.currentLoop, room.maxLoop, room.isQuan);
    }  

}
