//===================================================
//Author      : WZQ
//CreateTime  ：5/30/2017 2:32:53 PM
//Description ：退出房间投票详情
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NiuNiu
{
    public class UIVoteWindow_NiuNiu : UIWindowViewBase
    {
        [SerializeField]
        private Text _AgreedSum;           //同意人数
        [SerializeField]
        private Text _AgainstSum;         //反对人数

        //进度条



        void SetUI(int agreedSum, int againstSum)
        {
            _AgreedSum.text = agreedSum.ToString();
            _AgainstSum.text = againstSum.ToString();



        }






    }
}