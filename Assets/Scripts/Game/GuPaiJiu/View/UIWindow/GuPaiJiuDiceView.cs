//===================================================
//Author      : CZH
//CreateTime  ：9/7/2017 1:57:29 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuPaiJiu;
using System;


public class GuPaiJiuDiceView : UIViewBase
{
    [SerializeField]
    private Transform[] DiceTran;
    private List<GameObject> diceAnimaList;//加载的骰子动画   
    private GameObject DiceEntity1;//加载骰子1
    private GameObject DiceEntity2;//加载的骰子2
    private float diceAniTime = 1f;//骰子动画播放的时间
    private float diceTime = 2f;//骰子实体存在的时间



    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add(ConstantGuPaiJiu.RollDice, RollDice);
        dic.Add("DealRollDice", DealRollDice); //发牌摇筛子  
           
        return dic;
    }
    protected override void OnAwake()
    {
        base.OnAwake();
        diceAnimaList = new List<GameObject>();//加载的骰子动画 
    }


    /// <summary>
    /// 发牌摇骰子
    /// </summary>
    /// <param name="data"></param>
    private void DealRollDice(TransferData data)
    {
        RoomEntity room = data.GetValue<RoomEntity>("Room");
        //SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        StartCoroutine(RollDiceAniamtion(room, () =>
        {
            ModelDispatcher.Instance.Dispatch(ConstantGuPaiJiu.BigDealAni, data);
        }));
    }


    /// <summary>
    /// 抢庄摇筛子
    /// </summary>
    /// <param name="data"></param>
    private void RollDice(TransferData data)
    {
        RoomEntity room = data.GetValue<RoomEntity>("Room");       
        int isGrabBankerNum = data.GetValue<int>("isGrabBankerNum");
        if (isGrabBankerNum > 1)//如果多个人抢庄就摇筛子
        {
            StartCoroutine(RollDiceAniamtion(room, () =>
            {
                ModelDispatcher.Instance.Dispatch("OnPlayerInfo", data);
            }));
        }
        else //如果只有一个人抢庄就直接确认是庄
        {
            ModelDispatcher.Instance.Dispatch("OnPlayerInfo", data);
        }
    }

    IEnumerator RollDiceAniamtion(RoomEntity room, Action onComplete=null)
    {
        AudioEffectManager.Instance.Play("gp_yaoshaizi", Vector3.zero);//播放摇色子音效
        Debug.Log("```````````````````````````````````````````````````开始摇骰子````````````````````````");          
        //加载骰子动画
        string prefabName = "dice";
        string path = string.Format("download/{0}/prefab/uiprefab/uiitems/{1}.drb", ConstDefine.GAME_NAME, prefabName);
        AssetBundleManager.Instance.LoadOrDownload(path, prefabName, (GameObject go) =>
        {
            Debug.Log("```````````````````````开始加载骰子````````````````````````````");
            if (diceAnimaList.Count==2)
            {
                diceAnimaList[0].gameObject.SetActive(true);
                diceAnimaList[1].gameObject.SetActive(true);
            }
            else
            {
                if (DiceTran[0] != null)
                {
                    Debug.Log("```````````````````````开始加载骰子11````````````````````````````");
                    go = Instantiate(go);
                    go.SetActive(true);
                    go.SetParent(DiceTran[0]);
                    //UIAnimation ctrl = go.GetComponent<UIAnimation>();
                    diceAnimaList.Add(go);                    
                }
                if (DiceTran[1] != null)
                {
                    Debug.Log("```````````````````````开始加载骰子22````````````````````````````");
                    go = Instantiate(go);
                    go.SetActive(true);
                    go.SetParent(DiceTran[1]);
                   // UIAnimation ctrl2 = go.GetComponent<UIAnimation>();
                    diceAnimaList.Add(go);                   
                }
            }
            Debug.Log("```````````````````````加载骰子完毕````````````````````````````");
        });
        Debug.Log("````````````````````````````````AssetBundle 加载完毕``````````````````````````````````");  
        yield return new WaitForSeconds(diceAniTime);
        Debug.Log("`````````````````````````倒计时开始`````````````````````````````");
        //for (int i = 0; i < diceAnimaList.Count; i++)
        //{
        //    diceAnimaList[i].gameObject.SetActive(false);
        //}        
        Debug.Log("``````````````````````````````````````````摇骰子结束" + "点数1：" + room.FirstDice + "     点数2：" + room.SecondDice);
        ///加载真实的骰子
        GuPaiJiuPrefabManager.Instance.LoadPrefab(room.FirstDice.ToString(),(GameObject go)=> 
        {
            for (int i = 0; i < diceAnimaList.Count; i++)
            {
                diceAnimaList[i].gameObject.SetActive(false);
            }
            DiceEntity1 = go;
            go.SetParent(DiceTran[0]);
        });
        GuPaiJiuPrefabManager.Instance.LoadPrefab(room.SecondDice.ToString(), (GameObject go) =>
        {
            for (int i = 0; i < diceAnimaList.Count; i++)
            {
                diceAnimaList[i].gameObject.SetActive(false);
            }
            DiceEntity2 = go;
            go.SetParent(DiceTran[1]);
        });        
        yield return new WaitForSeconds(diceTime);
        GuPaiJiuPrefabManager.Instance.PushToPool(room.FirstDice.ToString(), DiceEntity1);
        GuPaiJiuPrefabManager.Instance.PushToPool(room.SecondDice.ToString(), DiceEntity2);
        yield return new WaitForSeconds(0.2f);
        if (onComplete != null) onComplete();           
        //ShuffleAnimation(room);
    }





    /// <summary>
    /// 洗牌动画
    /// </summary>
    /// <param name="room"></param>
    private void ShuffleAnimation(RoomEntity room)
    {
        Debug.Log("             ````````````````````````通知庄家洗牌喽`````````````````````````````       ");
        TransferData data = new TransferData();
        data.SetValue("Room",room);     
        ModelDispatcher.Instance.Dispatch(ConstantGuPaiJiu.ShuffleAnimation, data);
    }



}
