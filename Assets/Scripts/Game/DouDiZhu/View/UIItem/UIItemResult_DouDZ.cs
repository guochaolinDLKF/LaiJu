//===================================================
//Author      : DRB
//CreateTime  ：12/18/2017 10:20:52 AM
//Description ：
//===================================================
using DRB.DouDiZhu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DRB.DouDiZhu
{
    public class UIItemResult_DouDZ : MonoBehaviour
    {
        [SerializeField]
        private RawImage headRawImage;
        [SerializeField]
        private Text playerIDText;
        [SerializeField]
        private Text playerNameText;
        [SerializeField]
        private Text scoreText;
        [SerializeField]
        private Image img_isWinner;
        [SerializeField]
        private Image img_isOwner;
        [SerializeField]
        private Image img_PlayerBg;
        [SerializeField]
        private Image img_isBg;

        public void SetUI(SeatEntity seatEntity,bool isWiner, bool isOwner, bool isPlayer)
        {
            if (isWiner)
            {
                img_isWinner.SafeSetActive(true);               
            }
            else
            {
                img_isWinner.SafeSetActive(false);
            }

            if (isOwner)
            {
                img_isOwner.SafeSetActive(true);
            }
            else
            {
                img_isOwner.SafeSetActive(false);
            }

            if (isPlayer)
            {
                img_PlayerBg.SafeSetActive(true);
                img_isBg.SafeSetActive(false);
                playerIDText.SafeSetText("<color=#FFFFFFFF>" + "ID" + seatEntity.PlayerId.ToString() + "</color>");
                playerNameText.SafeSetText("<color=#FFFFFFFF>" + seatEntity.Nickname.ToString() + "</color>");
                scoreText.SafeSetText("<color=#FFFFFFFF>" + seatEntity.totalScore.ToString() + "</color>");
            }
            else
            {
                img_PlayerBg.SafeSetActive(false);
                img_isBg.SafeSetActive(true);
                playerIDText.SafeSetText("<color=#AF5302FF>" + "ID" + seatEntity.PlayerId.ToString() + "</color>");
                playerNameText.SafeSetText("<color=#AF5302FF>" + seatEntity.Nickname.ToString() + "</color>");
                scoreText.SafeSetText("<color=#AF5302FF>" + seatEntity.totalScore.ToString() + "</color>");
            }

            TextureManager.Instance.LoadHead(seatEntity.Avatar, OnAvatarLoadFinish);
        }
        private void OnAvatarLoadFinish(Texture2D tex)
        {
            if (headRawImage != null)
            {
                headRawImage.texture = tex;
            }
        }
    }
}
