//===================================================
//Author      : DRB
//CreateTime  ：11/28/2017 4:18:59 PM
//Description ：图片数字
//===================================================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(GridLayoutGroup))]
public class UITextureNumber : MonoBehaviour 
{
    [SerializeField]
    private List<Image> m_ImgNumber;

    public Sprite[] numberSprite = new Sprite[10];

    [SerializeField]
    private string m_Number;


    private void Start()
    {
        if (m_ImgNumber == null)
        {
            m_ImgNumber = new List<Image>();
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        SetNumber(m_Number);
    }
#endif

    public void SetNumber(int number)
    {
        SetNumber(number.ToString());
    }


    public void SetNumber(string number)
    {
        m_Number = number;

        for (int i = m_ImgNumber.Count - 1; i >= 0; --i)
        {
            //if (m_ImgNumber[i] == null)
            //{
            //    m_ImgNumber.RemoveAt(i);
            //    continue;
            //}
            if (m_ImgNumber[i] != null)
            {
                m_ImgNumber[i].gameObject.SetActive(false);
            }
        }

        if (string.IsNullOrEmpty(number)) return;

        for (int i = 0; i < number.Length; ++i)
        {
            int num = new string(number[i], 1).ToInt();

            Image img = null;
            if (i < m_ImgNumber.Count)
            {
                img = m_ImgNumber[i];
                img.gameObject.SetActive(true);
            }
            else
            {
                GameObject go = new GameObject();
                img = go.AddComponent<Image>();
                go.SetParent(transform);
                m_ImgNumber.Add(img);
            }

            img.overrideSprite = numberSprite[num];
        }
    }
}
