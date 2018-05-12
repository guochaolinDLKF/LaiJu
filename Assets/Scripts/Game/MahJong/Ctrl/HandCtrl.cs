//===================================================
//Author      : DRB
//CreateTime  ：4/10/2017 11:19:09 AM
//Description ：
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DRB.MahJong
{
    public class HandCtrl : MonoBehaviour
    {

        private Animator Animator;


        private void Awake()
        {
            Animator = GetComponentInChildren<Animator>();
        }


        private void Update()
        {
            AnimatorStateInfo info = Animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Animation"))
            {
                if (info.normalizedTime >= 1f)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        public void Reset()
        {
            gameObject.SetActive(true);
            Animator.Play("Animation",0,0);
        }
    }
}
