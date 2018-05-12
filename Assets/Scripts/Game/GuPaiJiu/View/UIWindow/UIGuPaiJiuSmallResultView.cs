//===================================================
//Author      : DRB
//CreateTime  ：10/10/2017 5:11:10 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GuPaiJiu;

public class UIGuPaiJiuSmallResultView : UIWindowViewBase
{
    [SerializeField]
    private GameObject m_smallResultObj;//加载人物头像
    [SerializeField]
    private Transform m_smallResultTran;//头像挂载点
    [SerializeField]
    private Text currPlayerScore;



    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);      
        switch (go.name)
        {
            case "btnjixu":                
                BtnOnclik();
                break;           
        }
    }

    public void SetUI(List<SeatEntity> seatList)
    {        
        for (int i = 0; i < seatList.Count; i++)
        {
            if (seatList[i].PlayerId == 0) continue;
            if (seatList[i] == RoomGuPaiJiuProxy.Instance.PlayerSeat)
            {
                string scoreText = string.Format(seatList[i].eamings >= 0 ? "+{0}" : "{1}", seatList[i].eamings, seatList[i].eamings);
                currPlayerScore.color = seatList[i].eamings >= 0 ? Color.white : Color.red;
                currPlayerScore.text = scoreText;
            }
            else
            {
                GameObject go = Instantiate(m_smallResultObj);
                go.SetActive(true);
                go.SetParent(m_smallResultTran);
                go.GetComponent<UIItemGuPaiJiuSmallResult>().SetUI(seatList[i]);
            }          
        }
        StartCoroutine(SetBtn());
    }

    IEnumerator SetBtn()
    {
        yield return new WaitForSeconds(5f);
        BtnOnclik();
    }

    private void BtnOnclik()
    {
        StopCoroutine("SetBtn");
        Debug.Log("是否点击  继续游戏=========================================");
        ModelDispatcher.Instance.Dispatch(ConstantGuPaiJiu.CloseHandContainer);
        SendNotification(ConstantGuPaiJiu.GuPaiJiuClientSendInformNext);
        Close();
       // Destroy(this.gameObject);
     
    }
}
