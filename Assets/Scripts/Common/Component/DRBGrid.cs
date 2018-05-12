//===================================================
//Author      : DRB
//CreateTime  ：3/16/2017 2:14:47 PM
//Description ：
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum Aligment
{
    UpperLeft,
    UpperCenter,
    UpperRight,
    CenterLeft,
    Center,
    CenterRight,
    LowerLeft,
    LowerCenter,
    LowerRight,
}


public enum Constraint
{
    FixedRowCount,
    FixedColumnCount,
    FixedDeepCount,
}

[ExecuteInEditMode]
public class DRBGrid : MonoBehaviour
{
    /// <summary>
    /// 每个子物体大小
    /// </summary>
    [SerializeField]
    private Vector2 cellSize;

    /// <summary>
    /// 子物体间距
    /// </summary>
    [SerializeField]
    private Vector2 spacing;

    /// <summary>
    /// 物体偏移
    /// </summary>
    [SerializeField]
    private Vector2 offset;

    [SerializeField]
    private Aligment childAligment;
    [SerializeField]
    private Constraint constraint;
    [SerializeField]
    private int constraintCount;
    [SerializeField]
    private bool is3D;




#if UNITY_EDITOR
    private void Update()
    {
        if (Application.isPlaying) return;
        Sort();
    }
#endif

    public Vector2 GetNextPos()
    {
        int index = transform.childCount;
        return GetPos(index) + new Vector2(this.transform.position.x, this.transform.position.y);
    }

    public Vector2 GetPos(int index)
    {
        Vector2 ret = Vector2.zero;
        //int index = transform.childCount;

        int columnIndex = 0;
        int rowIndex = 0;
        switch (constraint)
        {
            case Constraint.FixedColumnCount:
                columnIndex = index % constraintCount;
                rowIndex = index / constraintCount;
                break;
            case Constraint.FixedRowCount:
                columnIndex = index / constraintCount;
                rowIndex = index % constraintCount;
                break;
        }

        switch (childAligment)
        {
            case Aligment.LowerLeft:
                if (is3D)
                {
                    ret = new Vector2((columnIndex * offset.x), columnIndex * spacing.x + rowIndex * spacing.y + (columnIndex * offset.y));
                }
                else
                {
                    ret = new Vector3(columnIndex * spacing.x + (rowIndex * offset.x), rowIndex * spacing.y + (columnIndex * offset.y));
                }
                break;
            case Aligment.LowerRight:
                if (is3D)
                {
                    ret = new Vector3((columnIndex * offset.x), columnIndex * -spacing.x + rowIndex * spacing.y + (columnIndex * offset.y));
                }
                else
                {
                    ret = new Vector3(columnIndex * -spacing.x + (rowIndex * offset.x), rowIndex * spacing.y + (columnIndex * offset.y));
                }
                break;
            case Aligment.UpperLeft:
                if (is3D)
                {
                    ret = new Vector3((columnIndex * offset.x), columnIndex * spacing.x + rowIndex * -spacing.y + (columnIndex * offset.y));
                }
                else
                {
                    ret = new Vector3(columnIndex * spacing.x + (rowIndex * offset.x), rowIndex * -spacing.y + (columnIndex * offset.y));
                }
                break;
            case Aligment.UpperRight:
                if (is3D)
                {
                    ret = new Vector3((columnIndex * offset.x), columnIndex * -spacing.x + rowIndex * -spacing.y + (columnIndex * offset.y));
                }
                else
                {
                    ret = new Vector3(columnIndex * -spacing.x + (rowIndex * offset.x), rowIndex * -spacing.y + (columnIndex * offset.y));
                }
                break;
        }
        return ret;
    }

    /// <summary>
    /// 排列
    /// </summary>
    public void Sort()
    {
        if (constraintCount == 0) return;

        int childCount = transform.childCount;
        List<RectTransform> lst = new List<RectTransform>();
        for (int i = 0; i < childCount; ++i)
        {
            lst.Add(transform.GetChild(i).GetComponent<RectTransform>());
        }
        for (int i = 0; i < lst.Count; ++i)
        {
            SetPos(lst[i], i);
        }
    }

    private void SetPos(RectTransform trans,int index)
    {
        trans.localPosition = GetPos(index);
       
        trans.sizeDelta = cellSize;
    }
}
