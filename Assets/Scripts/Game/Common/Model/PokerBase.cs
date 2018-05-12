//===================================================
//Author      : DRB
//CreateTime  ：12/4/2017 9:46:12 AM
//Description ：
//===================================================
using System;

public class PokerBase : IComparable
{

    [UnityEngine.SerializeField]
    protected int m_Index;
    [UnityEngine.SerializeField]
    protected int m_Color;
    [UnityEngine.SerializeField]
    protected int m_Size;
    [UnityEngine.SerializeField]
    protected bool m_IsUniversal;

    public const string DefaultName = "0_0";


    public int index { get { return m_Index; } set { m_Index = value; } }

    public int color { get { return m_Color; } set { m_Color = value; } }

    public int size { get { return m_Size; } set { m_Size = value; } }

    public bool isUniversal { get { return m_IsUniversal; } set { isUniversal = value; } }

    public PokerBase() { }

    public PokerBase(int color, int size)
    {
        m_Index = 0;
        m_Color = color;
        m_Size = size;
        m_IsUniversal = false;
    }

    public PokerBase(int index, int color, int size)
    {
        m_Index = index;
        m_Color = color;
        m_Size = size;
        m_IsUniversal = false;
    }
    public PokerBase(int index, int color, int size,bool isUniversal)
    {
        m_Index = index;
        m_Color = color;
        m_Size = size;
        m_IsUniversal = isUniversal;
    }

    public override string ToString()
    {
        return string.Format("{0}_{1}", color.ToString(), size.ToString());
    }

    public string ToLog()
    {
        return string.Format("{0}_{1}_{2}", index.ToString(), color.ToString(), size.ToString());
    }

    public virtual int CompareTo(object other)
    {
        if (other == null) return -1;
        if (!(other is PokerBase)) return -1;
        PokerBase otherPoker = other as PokerBase;
        if (color != otherPoker.color) return color - otherPoker.color;
        if (size != otherPoker.size) return size - otherPoker.size;
        return index - otherPoker.index;
    }
}
