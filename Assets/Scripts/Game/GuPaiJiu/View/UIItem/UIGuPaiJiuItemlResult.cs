//===================================================
//Author      : DRB
//CreateTime  ：12/7/2017 3:02:00 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace GuPaiJiu {
    public class UIGuPaiJiuItemlResult : MonoBehaviour {

        [SerializeField]
        private GameObject obj;
        [SerializeField]
        private RawImage m_ImageHead;
        [SerializeField]
        private Text playerName;
        [SerializeField]
        private Text playerId;
        [SerializeField]
        private Image pokerXing;
        [SerializeField]
        private Text fen;

        private List<int> list = new List<int>();

        void Awake()
        {
            if (obj != null) obj.SetActive(false);
        }

        public void SetUI(SeatEntity seat)
        {
            obj.SetActive(true);
            list.Clear();
            playerName.text = seat.Nickname;
            playerId.text = seat.PlayerId.ToString();
            string textGold = string.Format("{0}", seat.Gold >= 0 ? "+" : "-");
            fen.text = textGold + seat.Gold.ToString();
            TextureManager.Instance.LoadHead(seat.Avatar, (Texture2D tex) =>
            {
                m_ImageHead.texture = tex;
            });
            for (int i = 0; i < seat.pokerList.Count; i++)
            {
                list.Add(seat.pokerList[i].Type);
            }
            pokerXing.sprite = LoadSprite(list);

    }


        private Sprite LoadSprite(List<int> list)
        {
            PokerType pokerType = LookupPokerType.Instance.GetDicPokerType(list);

            Sprite sprite  = GuPaiJiuPrefabManager.Instance.LoadSprite(pokerType.ToString());
            return sprite;
        }

    }
}
