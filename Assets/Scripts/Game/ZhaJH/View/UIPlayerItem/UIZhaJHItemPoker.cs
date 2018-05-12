//===================================================
//Author      : CZH
//CreateTime  ：6/16/2017 2:40:01 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhaJh;
using UnityEngine.UI;
using zjh.proto;
using DG.Tweening;

public class UIZhaJHItemPoker : UIItemBase
{
    [SerializeField]
    private UIZhaJHItemSeat[] m_itemSeat;
    [SerializeField]
    private Transform pokerAniTran;
    [SerializeField]
    private GameObject pokerAniObj;

    //public RuntimeAnimatorController[] DealPokerAniCtrls;//发牌动画

    public GameObject chipMount;//筹码挂载点。用来清除筹码
    [SerializeField]
    private Transform pokerFP;
    [SerializeField]
    private GameObject pokerCln;//生成牌预制体的挂载点
    [SerializeField]
    private GameObject pokerPrefab; //生成牌的预制体

    public override Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> DicNotificationInterests()
    {
        Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler> dic = new Dictionary<string, DispatcherBase<ModelDispatcher, TransferData, string>.Handler>();
        dic.Add(ZhaJHMethodname.OnZJHHairPoker, HairPoker);
        dic.Add(ZhaJHMethodname.OnZJHLookPoker, LookPoker);          
        dic.Add(ZhaJHMethodname.OnZJHInfoSettlement, InfoSettlement);
        dic.Add(ZhaJHMethodname.OnZJHInfoSettlement1, InfoSettlement1);
        dic.Add(ZhaJHMethodname.OnZJHHairPokerAni, HairPokerAni);
        return dic;
    }





   
    /// <summary>
    /// 播放发牌动画
    /// </summary>
    /// <param name="data"></param>
    private void HairPokerAni(TransferData data)
    {
        List<SeatEntity> seatList = data.GetValue<List<SeatEntity>>("SeatList");
        //StartCoroutine("HairPokerAni11"); 
        Debug.Log(seatList.Count+"                    长度");
        StartCoroutine(HairPokerAni11(seatList));
    }
    IEnumerator HairPokerAni11(List<SeatEntity> seatList)
    {        
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 21; i++)
        {
            GameObject go = Instantiate(pokerPrefab, pokerCln.transform);
            go.name = i.ToString();
            go.transform.localPosition = Vector3.zero;
            go.SetActive(true);

        }       
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 3; i++)
        {            
            for (int j = 0; j < seatList.Count; j++)
            {               
                for (int z = 0; z < m_itemSeat.Length; z++)
                {
                    if (seatList[j].Index == m_itemSeat[z].m_nSeatIndex)
                    {
                        GameObject go = pokerCln.transform.GetChild(pokerCln.transform.childCount - 1).gameObject;
                        go.transform.SetParent(m_itemSeat[z].pokerMounts[i].transform);
                        Tweener magnify1 = go.transform.DOMove(new Vector3(m_itemSeat[z].pokerMounts[i].transform.position.x, m_itemSeat[z].pokerMounts[i].transform.position.y, 0), 0.2f);
                        Tweener magnify2 = go.transform.DORotate(Vector3.zero, 0.2f);
                        yield return new WaitForSeconds(0.2f);
                        go.transform.localPosition = Vector3.zero;
                        if (m_itemSeat[z].m_nSeatIndex == 0)
                        {
                            go.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                        }
                        else
                        {
                            go.transform.localScale = new Vector3(1f, 1f, 1f);
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < pokerCln.transform.childCount; i++)
        {
            if (pokerCln.transform.GetChild(i).gameObject.activeSelf)
            {
                Destroy(pokerCln.transform.GetChild(i).gameObject);
            }
        }      
         SendNotification(ZhaJHMethodname.OnZJHDealLookEnd);
    












        //yield return new WaitForSeconds(0.5f);
        //AudioEffectManager.Instance.Play("zjh_fapai", Vector3.zero);
        //for (int z = 0; z < 3; z++)
        //{
        //    for (int j = 0; j < m_itemSeat.Length; j++)
        //    {
        //        if (seat.Index == m_itemSeat[j].m_nSeatIndex)
        //        {
        //            GameObject go = pokerCln.transform.GetChild(pokerCln.transform.childCount - 1).gameObject;
        //            go.transform.SetParent(m_itemSeat[j].pokerMounts[z].transform);
        //            Tweener magnify1 = go.transform.DOMove(new Vector3(m_itemSeat[j].pokerMounts[z].transform.position.x, m_itemSeat[j].pokerMounts[z].transform.position.y, 0), 3f);
        //            Tweener magnify2 = go.transform.DORotate(Vector3.zero, 3f);
        //            yield return new WaitForSeconds(2f);
        //            go.transform.localPosition = Vector3.zero;
        //            go.transform.localScale = new Vector3(1f, 1f, 1f);
        //        }               
        //    }
        //}
        

    }
 




    private void Licensing()
    {
        for (int i = 0; i < 21; i++)
        {
            GameObject go = Instantiate(pokerPrefab, pokerCln.transform);
            go.name = i.ToString();            
            go.transform.localPosition = Vector3.zero;
            go.SetActive(true);

        }
    }

     IEnumerator InitializationPoker()
    {       
       int a = 0;
        if (RoomZhaJHProxy.Instance.PlayerSeat.pos==7)
        {
            for (int i = 0; i < 21; i++)
            {
                GameObject go = Instantiate(pokerPrefab, pokerCln.transform);
                go.name = i.ToString();
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = new Vector3(-348 + a, 0, 0);
                go.SetActive(true);
                a += 30;
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            for (int i = 0; i < 21; i++)
            {
                GameObject go = Instantiate(pokerPrefab, pokerCln.transform);
                go.name = i.ToString();
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = new Vector3(348 - a, 0, 0);
                go.SetActive(true);
                a += 30;
                yield return new WaitForSeconds(0.01f);
            }
        }
       
       // yield return new WaitForSeconds(0.2f);
    }



    /// <summary>
    /// 中途加入实例化牌
    /// </summary>
    /// <param name="data"></param>
    private void HairPoker(TransferData data)
    {
        SeatEntity seat = data.GetValue<SeatEntity>("Seat");
        //int index = data.GetValue<int>("index");
        int index = seat.Index;
        string spriteName = data.GetValue<string>("spriteName");        
        for (int j = 0; j < m_itemSeat.Length; j++)
        {
            if (index == m_itemSeat[j].m_nSeatIndex)
            {
                if (m_itemSeat[j].gameObject.activeSelf)
                {
                    for (int z = 0; z < m_itemSeat[j].pokerMounts.Length; z++)
                    {
                        if (m_itemSeat[j].pokerMounts[z].transform.childCount>0)
                        {
                            Destroy(m_itemSeat[j].pokerMounts[z].transform.GetChild(0).gameObject);
                        }                        
                    }
                }
                Vector3 pokerScale = m_itemSeat[j].m_nSeatIndex == 0 ? new Vector3(0.8f, 0.8f, 0.8f) : new Vector3(0.7f, 0.7f, 0.7f);
                for (int k = 0; k < 3; k++)
                {
                    GameObject go = ZJHPrefabManager.Instance.LoadPoker(null, null, spriteName);
                    go.transform.SetParent(m_itemSeat[j].pokerMounts[k].transform);                  
                    go.transform.localPosition = Vector3.zero;                  
                    go.transform.localScale = pokerScale;
                }                
            }
        }        
    }

    


    private void LookPoker(TransferData data)
    {
        List<Poker> pokerList = data.GetValue<List<Poker>>("pokerList");
        int index = data.GetValue<int>("index");
        string spriteName = data.GetValue<string>("spriteName");
        for (int j = 0; j < m_itemSeat.Length; j++)
        {
            if (index == m_itemSeat[j].m_nSeatIndex)
            {
                if (pokerList.Count != 0)
                {
                    if (m_itemSeat[j].gameObject.activeSelf)
                    {
                        for (int z = 0; z < m_itemSeat[j].pokerMounts.Length; z++)
                        {
                            for (int k = 0; k < m_itemSeat[j].pokerMounts[z].transform.childCount; k++)
                            {
                                Destroy(m_itemSeat[j].pokerMounts[z].transform.GetChild(k).gameObject);
                            }
                        }
                    }
                    Vector3 pokerScale = m_itemSeat[j].m_nSeatIndex == 0 ? new Vector3(0.8f,0.8f,0.8f):new Vector3(0.7f, 0.7f, 0.7f);
                    for (int i = 0; i < pokerList.Count; i++)
                    {                        
                        GameObject go = ZJHPrefabManager.Instance.LoadPoker(pokerList[i], null, spriteName);
                        go.transform.SetParent(m_itemSeat[j].pokerMounts[i].transform);
                        go.transform.localPosition = Vector3.zero;
                        go.transform.localScale = pokerScale;
                    }
                }
            }
        }
    }

  


    /// <summary>
    /// 结算后清空牌和失败和看牌标记
    /// </summary>
    private void InfoSettlement(TransferData data)
    {       
        for (int i = 0; i < m_itemSeat.Length; i++)
        {
            if (m_itemSeat[i].gameObject.activeSelf)
            {
                m_itemSeat[i].m_zhuangtai.enabled = false;//清空所有的标记
                if (m_itemSeat[i].display != null)
                    m_itemSeat[i].display.enabled = false;
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < m_itemSeat[i].pokerMounts[j].transform.childCount; k++)
                    {
                        Destroy(m_itemSeat[i].pokerMounts[j].transform.GetChild(k).gameObject);
                    }                         
                }
            }           
        }
        ///清除所有的筹码
        for (int i = 0; i < chipMount.transform.childCount; i++)
        {                 
            Destroy(chipMount.transform.GetChild(i).gameObject);
        }        
    }

    private void InfoSettlement1(TransferData data)
    {
        int index = data.GetValue<int>("Index");
        for (int i = 0; i < m_itemSeat.Length; i++)
        {
            if (m_itemSeat[i].m_nSeatIndex == index)
            {
                if (m_itemSeat[i].m_zhuangtai.enabled==true)
                {
                    m_itemSeat[i].m_zhuangtai.enabled = false;//清空所有的标记                                        
                }
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < m_itemSeat[i].pokerMounts[j].transform.childCount; k++)
                    {                       
                        Destroy(m_itemSeat[i].pokerMounts[j].transform.GetChild(k).gameObject);
                    }
                }
            }
        }
    }


}
