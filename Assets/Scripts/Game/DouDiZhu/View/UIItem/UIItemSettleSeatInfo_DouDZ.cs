//===================================================
//Author      : DRB
//CreateTime  ：12/16/2017 1:59:21 PM
//Description ：
//===================================================
using DRB.DouDiZhu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DRB.DouDiZhu
{
    public class UIItemSettleSeatInfo_DouDZ : MonoBehaviour
    {
        [SerializeField]
        private RawImage headRawImage;
        [SerializeField]
        private Text playerIDText;
        [SerializeField]
        private Text playerNameText;
        [SerializeField]
        private Text betText;
        [SerializeField]
        private Text multipleText;
        [SerializeField]
        private Text scoreText;
        [SerializeField]
        private Image img_isWinner;
        [SerializeField]
        private GameObject isBankerObj;
        [SerializeField]
        private GameObject isOwnerObj;
        [SerializeField]
        private Image img_isPlayerWinerBg;
        [SerializeField]
        private Image img_isPlayerLoserBg;
        [SerializeField]
        private Image img_isBg;

        public void SetUI(SeatEntity seatEntity, int bet, bool isOwner, bool isPlayer,int Times)
        {
            playerIDText.SafeSetText(seatEntity.PlayerId.ToString());
            playerNameText.SafeSetText(seatEntity.Nickname.ToString());
            betText.SafeSetText(bet.ToString());
            scoreText.SafeSetText(seatEntity.score.ToString());

            if (seatEntity.isWiner)
            {
                img_isWinner.SafeSetActive(true);
            }
            else
            {
                img_isWinner.SafeSetActive(false);
            }

            if (seatEntity.IsBanker)
            {
                multipleText.SafeSetText((Times* 2).ToString());
                if (isBankerObj != null)
                {
                    isBankerObj.SetActive(true);
                }
            }
            else
            {
                multipleText.SafeSetText(Times.ToString());
                if (isBankerObj != null)
                {
                    isBankerObj.SetActive(false);
                }
            }

            if (isOwner)
            {
                if (isOwnerObj != null)
                {
                    isOwnerObj.SetActive(true);
                }
            }
            else
            {
                if (isOwnerObj != null)
                {
                    isOwnerObj.SetActive(false);
                }
            }
            if (isPlayer)
            {
                if (seatEntity.isWiner)
                {
                    img_isPlayerWinerBg.SafeSetActive(true);
                    img_isPlayerLoserBg.SafeSetActive(false);
                    img_isBg.SafeSetActive(false);
                }
                else
                {
                    img_isPlayerWinerBg.SafeSetActive(false);
                    img_isPlayerLoserBg.SafeSetActive(true);
                    img_isBg.SafeSetActive(false);
                }
            }
            else
            {
                img_isPlayerWinerBg.SafeSetActive(false);
                img_isPlayerLoserBg.SafeSetActive(false);
                img_isBg.SafeSetActive(true);
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
