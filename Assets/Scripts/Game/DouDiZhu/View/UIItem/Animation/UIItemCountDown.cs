//===================================================
//Author      : DRB
//CreateTime  ：1/12/2018 1:47:45 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemCountDown : MonoBehaviour {
    [SerializeField]
    private int countDownTime;
    [SerializeField]
    private float timer;
    [SerializeField]
    private Text m_text;
    
	void Start () {
		
	}
	
	void Update () {
        if (countDownTime > 0)
        {
            if (timer > 1)
            {
                timer = 0;
                countDownTime--;
                m_text.SafeSetText(countDownTime.ToString());
            }
            else
            {
                timer+=Time.deltaTime;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
	}
    public void SetCountDown(int countDown)
    {
        //Debug.LogWarning(countDown);
        countDownTime = countDown;
        m_text.SafeSetText(countDown.ToString());
    }
}
