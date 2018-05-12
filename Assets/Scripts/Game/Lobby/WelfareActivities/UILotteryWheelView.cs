//===================================================
//Author      : DRB
//CreateTime  ：12/2/2017 10:15:45 AM
//Description ：
//===================================================
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILotteryWheelView : MonoBehaviour
{
    #region Variable
    [SerializeField]
    UIWelfareActivitiesWindow m_UIWelfareActivitiesWindow;

    [SerializeField]
    private Text surPlusTimeText;
    [SerializeField]
    private Text surPlusTotalTimeText;
    [SerializeField]
    private RawImage[] giftRawImage;

    //从服务器中接受的抽中的号码
    [SerializeField]
    private int choiceTargetNum;
    private int choiceTargetIndex;

    //当客户端代码运行时正在变化的号码
    [SerializeField]
    private int choiceNum;

    //从一个号码到下一个号码所需要的时间
    private float oncetimer;
    [SerializeField]
    private float onceChoiceTimer;
    private float onceMinChoiceTimer;
    private float onceMaxChoiceTimer;

    //抽奖转了多少圈
    private int choiceTotalCircle;
    private int totalCircle;
    private int reduceAndAddCircle;

    //是否开始转盘
    private bool isStartTurn;

    public bool GetIsStartTrun()
    {
        return isStartTurn;
    }

    [SerializeField]
    private RawImage bgRawImage;
    [SerializeField]
    private RawImage buttonRawImage;



    [SerializeField]
    private float lightChangeTimer;
    [SerializeField]
    private float lightTargetChangeTimer;
    [SerializeField]
    private Image lightImage;
    [SerializeField]
    private DOTweenAnimation rotateAnimation;
    [SerializeField]
    private DOTweenAnimation moveAnimation;
    [SerializeField]
    private RawImage giftImage;
    [SerializeField]
    private Image giftShaddingImage;
    #endregion

    #region MonoBehaviour

    void Awake()
    {
        onceChoiceTimer = 0.1f;
        onceMinChoiceTimer = 0.45f;
        onceMaxChoiceTimer = 0.03f;
        totalCircle = 5;
        reduceAndAddCircle = 2;
        isStartTurn = false;
        for (int i = 0; i < giftRawImage.Length; i++)
        {
            giftRawImage[i].transform.GetChild(0).GetComponent<Image>().color = Color.clear;
        }
    }

    void Update()
    {
        if (isStartTurn)
        {
            LotteryWheel();
        }
        if (lightChangeTimer > lightTargetChangeTimer)
        {
            lightChangeTimer = 0;
            lightImage.SafeSetActive(!lightImage.IsActive());
        }
        else
        {
            lightChangeTimer += Time.deltaTime;
        }

    }
    #endregion

    //public System.Action OnLotteryComplete;


    #region DeleteShadowEffect 删除幻影特效
    /// <summary>
    /// 删除幻影特效
    /// </summary>
    private void DeleteShadowEffect()
    {
        for (int i = 0; i < giftRawImage.Length; i++)
        {
            if (i != choiceTargetNum - 1)
            {
                if (giftRawImage[i].transform.GetChild(0).GetComponent<Image>().color != Color.clear)
                {
                    giftRawImage[i].transform.GetChild(0).GetComponent<Image>().color = Color.clear;
                }
            }
            else
            {
                //giftRawImage[i].transform.GetChild(0).GetComponent<Image>().color = Color.white;
            }
        }
    }
    #endregion

    #region StartTurn 打开大转盘旋转开关
    /// <summary>
    /// 打开大转盘旋转开关
    /// </summary>
    /// <param name="data"></param>
    public void StartTurn(TransferData data)
    {
        int giftIndex = data.GetValue<int>("giftIndex");
        int surPlusTime = data.GetValue<int>("time");
        int totalTime = data.GetValue<int>("timeTotal");
        //string message = data.GetValue<string>("message");
        //int giftCallBackNum = data.GetValue<int>("giftCallBackNum");
        //GiftType giftType = data.GetValue<GiftType>("giftType");

        //GetComponent<UILotteryWheelInfo>().SetGiftCallBackData(giftIndex, message, surPlusTime, totalTime, giftCallBackNum, giftType);
        //GetComponent<UILotteryWheelInfo>().SetGiftCallBackData(giftIndex, message, surPlusTime, totalTime);
        GetComponent<UILotteryWheelInfo>().SetGiftCallBackData(giftIndex, surPlusTime, totalTime);

        //surPlusTimeText.SafeSetText(surPlusTime.ToString());
        //surPlusTotalTimeText.SafeSetText(totalTime.ToString());

        for (int i = 0; i < giftRawImage.Length; ++i)
        {
            UIGiftInfo info = giftRawImage[i].gameObject.GetComponent<UIGiftInfo>();
            if (info.GetUIGiftIndex() == giftIndex)
            {
                choiceTargetIndex = info.GetUIGiftIndex();
                choiceTargetNum = i + 1;
                if (choiceTargetNum == 12)
                {
                    choiceTargetNum = 0;
                }
            }
        }
        if (!isStartTurn)
        {
            isStartTurn = true;
        }
    }

    #endregion

    #region 大转盘 LotteryWheel
    /// <summary>
    /// 大转盘
    /// </summary>
    private void LotteryWheel()
    {
        if (choiceTotalCircle >= totalCircle && choiceNum == choiceTargetNum)
        {
            oncetimer = 0;
            choiceTotalCircle = 0;
            DeleteShadowEffect();
            m_UIWelfareActivitiesWindow.OnLotteryWheelComplete();
            isStartTurn = false;
            onceChoiceTimer = 0.1f;

            Debug.LogWarning(choiceTargetNum - 1);
            int rotateAnimationIndex;

            if ((choiceTargetNum - 1) == -1)
            {
                rotateAnimationIndex = 11;
                //rotateAnimation.transform.SetParent(giftRawImage[11].transform);
            }
            else
            {
                rotateAnimationIndex = choiceTargetNum - 1;
                  //rotateAnimation.transform.SetParent(giftRawImage[choiceTargetNum - 1].transform);
            }

            rotateAnimation.transform.SetParent(giftRawImage[rotateAnimationIndex].transform);
            rotateAnimation.transform.SetAsLastSibling();
            rotateAnimation.transform.localPosition = Vector2.zero;
            rotateAnimation.SafeSetActive(true);
            moveAnimation.transform.position = giftRawImage[rotateAnimationIndex].transform.position;
            UIGiftInfo uiGiftInfo = giftRawImage[rotateAnimationIndex].transform.GetComponent<UIGiftInfo>();

            if (uiGiftInfo.GetUIGiftType() == GiftType.Null)
            {
                AudioEffectManager.Instance.Play("notGetPrize");
            }
            else
            {
                AudioEffectManager.Instance.Play("getPrize");
            }

            TextureManager.Instance.LoadHead(uiGiftInfo.GetURL(), (Texture2D texture2d) =>
            {
                moveAnimation.transform.GetComponent<RawImage>().texture = texture2d;
                giftImage.texture = texture2d;
            }, true);


            rotateAnimation.DORestart();
            giftShaddingImage.SafeSetActive(true);
            giftShaddingImage.color = Color.clear;
        }
        else
        {
            AddShadowEffect();
            if (choiceTotalCircle < reduceAndAddCircle)
            {
                if (onceChoiceTimer > onceMaxChoiceTimer)
                {
                    onceChoiceTimer = Mathf.Lerp(onceChoiceTimer, onceMaxChoiceTimer, 1 * Time.deltaTime);
                }
            }
            else if (choiceTotalCircle > totalCircle - reduceAndAddCircle)
            {
                if (onceChoiceTimer < onceMinChoiceTimer)
                {
                    onceChoiceTimer = Mathf.Lerp(onceChoiceTimer, onceMinChoiceTimer, 0.5f * Time.deltaTime);
                }
            }

            if (oncetimer > onceChoiceTimer)
            {
                AudioEffectManager.Instance.Play("dudu");
                choiceNum++;
                choiceNum %= giftRawImage.Length;
                oncetimer = 0;
                if (choiceNum == choiceTargetNum)
                {
                    choiceTotalCircle++;
                }
            }
        }
        oncetimer += Time.deltaTime;
    }

    #region AddShadowEffect 增加幻影特效 
    /// <summary>
    /// 增加幻影特效
    /// </summary>
    private void AddShadowEffect()
    {
        int choiceChanged = choiceNum;
        giftRawImage[choiceChanged].transform.GetChild(0).GetComponent<Image>().color = Color.white;
        choiceChanged = (choiceChanged + giftRawImage.Length - 1) % giftRawImage.Length;
        giftRawImage[choiceChanged].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.75f);
        choiceChanged = (choiceChanged + giftRawImage.Length - 1) % giftRawImage.Length;
        giftRawImage[choiceChanged].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        choiceChanged = (choiceChanged + giftRawImage.Length - 1) % giftRawImage.Length;
        giftRawImage[choiceChanged].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
        choiceChanged = (choiceChanged + giftRawImage.Length - 1) % giftRawImage.Length;
        giftRawImage[choiceChanged].transform.GetChild(0).GetComponent<Image>().color = Color.clear;
    }
    #endregion

    #endregion

    #region SetLotteryWheelUI  设置大转盘UI
    /// <summary>
    /// 设置大转盘UI
    /// </summary>
    /// <param name="data"></param>
    public void SetLotteryWheelUI(TransferData data)
    {
        AccountCtrl.Instance.RequestCards();

        int surPlusTimeCount = data.GetValue<int>("surPlusTimeCount");
        int timeTotalNum = data.GetValue<int>("totalTimeCount");
        
        string bgUrl = data.GetValue<string>("BgURL");
        string buttonUrl = data.GetValue<string>("buttonURL");

        TextureManager.Instance.LoadHead(bgUrl, (Texture2D texture2d) =>
        {
            if (bgRawImage != null)
            {
                bgRawImage.texture = texture2d;
                //bgRawImage.SetNativeSize();
            }
        });

        TextureManager.Instance.LoadHead(buttonUrl, (Texture2D texture2d) =>
        {
            if (buttonRawImage != null)
            {
                buttonRawImage.texture = texture2d;
            }
        }, true);

        surPlusTimeText.SafeSetText(surPlusTimeCount.ToString());
        surPlusTotalTimeText.SafeSetText(timeTotalNum.ToString());

        GetComponent<UILotteryWheelInfo>().SetTime(surPlusTimeCount, timeTotalNum);

        List<lotteryWheelEntity> lstLotteryWheelGiftEntity = data.GetValue<List<lotteryWheelEntity>>("lstLotteryWheelEntity");

        for (int i = 0; i < lstLotteryWheelGiftEntity.Count; i++)
        {
            giftRawImage[i].gameObject.GetComponent<UIGiftInfo>().SetUIGiftIndex(lstLotteryWheelGiftEntity[i].id, lstLotteryWheelGiftEntity[i].name, lstLotteryWheelGiftEntity[i].type, lstLotteryWheelGiftEntity[i].img_url);

            RawImage rawImageTemp = giftRawImage[i];

            TextureManager.Instance.LoadHead(lstLotteryWheelGiftEntity[i].img_url, (Texture2D texture2d) =>
            {
                if (rawImageTemp != null)
                {
                    rawImageTemp.texture = texture2d;
                }
            }, true);
        }
    }
    #endregion

    public void SetSurPlusTimeText(int surPlusTime)
    {
        surPlusTimeText.SafeSetText(surPlusTime.ToString());
    }
    public GiftType GetTargetGiftType()
    {
        for (int i = 0; i < giftRawImage.Length; ++i)
        {
            UIGiftInfo info = giftRawImage[i].gameObject.GetComponent<UIGiftInfo>();
            if (info.GetUIGiftIndex() == choiceTargetIndex)
            {
                return info.GetUIGiftType();
            }
        }
        return GiftType.Null;
    }
    public string GetTargetGiftName()
    {
        for (int i = 0; i < giftRawImage.Length; ++i)
        {
            UIGiftInfo info = giftRawImage[i].gameObject.GetComponent<UIGiftInfo>();
            if (info.GetUIGiftIndex() == choiceTargetIndex)
            {
                return info.GetName();
            }
        }
        return null;
    }
}
