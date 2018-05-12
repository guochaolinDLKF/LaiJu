//===================================================
//Author      : DRB
//CreateTime  ：10/9/2017 4:53:01 PM
//Description ：
//===================================================
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestGrid : MonoBehaviour 
{
    /// <summary>
    /// 排列顺序
    /// </summary>
    public enum GridOrder
    {
        X_Y_Z,
        X_Z_Y,
        Y_X_Z,
        Y_Z_X,
        Z_Y_X,
        Z_X_Y,
    }
    /// <summary>
    /// X排列准线
    /// </summary>
    public enum GridXAlignment
    {
        Left,
        Center,
        Right,
    }
    /// <summary>
    /// Y排列准线
    /// </summary>
    public enum GridYAlignment
    {
        Low,
        Center,
        High,
    }
    /// <summary>
    /// Z排列准线
    /// </summary>
    public enum GridZAlignment
    {
        Near,
        Center,
        Far,
    }


    /// <summary>
    /// 子物体间距
    /// </summary>
    public Vector3 spacing;

    /// <summary>
    /// 物体偏移
    /// </summary>
    public Vector3 offset;

    public GridOrder order = GridOrder.X_Z_Y;

    public int XConstraint = -1;

    public int YConstraint = -1;

    public int ZConstraint = -1;
    
    public GridXAlignment XAlignment = GridXAlignment.Left;

    public GridYAlignment YAlignment = GridYAlignment.Low;

    public GridZAlignment ZAlignment = GridZAlignment.Near;


#if UNITY_EDITOR
    private void Update()
    {
        if (Application.isPlaying) return;
        Sort();
    }
#endif

    /// <summary>
    /// 排列
    /// </summary>
    public void Sort()
    {
        if (XConstraint == 0) return;
        if (YConstraint == 0) return;
        if (ZConstraint == 0) return;

        int childCount = transform.childCount;
        List<Transform> lst = new List<Transform>();
        for (int i = 0; i < childCount; ++i)
        {
            lst.Add(transform.GetChild(i));
        }

        switch (order)
        {
            case GridOrder.X_Z_Y:
                for (int i = 0; i < lst.Count; ++i)
                {
                    float x = i % XConstraint * spacing.x;
                    float z = i % (XConstraint * ZConstraint) / XConstraint * spacing.z;
                    float y = (i / (XConstraint * ZConstraint)) * spacing.y;
                    lst[i].localPosition = new Vector3(x, y, z);
                }
                break;
            case GridOrder.X_Y_Z:
                for (int i = 0; i < lst.Count; ++i)
                {
                    float x = i % XConstraint * spacing.x;
                    float y = i % (XConstraint * YConstraint) / XConstraint * spacing.y;
                    float z = (i / (XConstraint * YConstraint)) * spacing.z;
                    lst[i].localPosition = new Vector3(x, y, z);
                }
                break;
            case GridOrder.Y_X_Z:
                for (int i = 0; i < lst.Count; ++i)
                {
                    float y = i % YConstraint * spacing.y;
                    float x = i % (XConstraint * YConstraint) / YConstraint * spacing.x;
                    float z = (i / (XConstraint * YConstraint)) * spacing.z;
                    lst[i].localPosition = new Vector3(x, y, z);
                }
                break;
            case GridOrder.Y_Z_X:
                for (int i = 0; i < lst.Count; ++i)
                {
                    float y = i % YConstraint * spacing.y;
                    float z = i % (YConstraint * ZConstraint) / YConstraint * spacing.z;
                    float x = (i / (YConstraint * ZConstraint)) * spacing.x;
                    lst[i].localPosition = new Vector3(x, y, z);
                }
                break;
            case GridOrder.Z_X_Y:
                for (int i = 0; i < lst.Count; ++i)
                {
                    float z = i % ZConstraint * spacing.z;
                    float x = i % (XConstraint * ZConstraint) / ZConstraint * spacing.x;
                    float y = (i / (XConstraint * ZConstraint)) * spacing.y;
                    lst[i].localPosition = new Vector3(x, y, z);
                }
                break;
            case GridOrder.Z_Y_X:
                for (int i = 0; i < lst.Count; ++i)
                {
                    float z = i % ZConstraint * spacing.z;
                    float y = i % (YConstraint * ZConstraint) / ZConstraint * spacing.y;
                    float x = (i / (YConstraint * ZConstraint)) * spacing.x;
                    lst[i].localPosition = new Vector3(x, y, z);
                }
                break;
        }
    }
}
