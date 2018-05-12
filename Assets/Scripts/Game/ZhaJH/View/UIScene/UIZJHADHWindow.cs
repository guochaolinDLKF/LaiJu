//===================================================
//Author      : DRB
//CreateTime  ：7/24/2017 10:34:40 AM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIZJHADHWindow : UIWindowViewBase
{
    [SerializeField]
    private Text peopleNumber;

    public  void DisplayPeopleNumber(int number)
    {
        if(peopleNumber!=null)
        peopleNumber.text = number.ToString();
    }

}
