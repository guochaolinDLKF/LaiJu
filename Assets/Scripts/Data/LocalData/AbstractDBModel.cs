using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractDBModel<T,P> where T : class,new() where P:AbstractEntity
{
    protected List<P> m_List;
    protected Dictionary<int, P> m_Dic;
    public AbstractDBModel()
    {
        m_List = new List<P>();
        m_Dic = new Dictionary<int, P>();
        Load();
    }
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

    protected abstract string FileName { get; }


    protected void Load()
    {
#if UNITY_EDITOR
        string path = string.Format(Application.dataPath + "/Download/{0}/BinaryData/{1}",ConstDefine.GAME_NAME,FileName);
#else
        string path = string.Format("{0}{1}{2}", LocalFileManager.Instance.LocalFilePath,"download/binarydata/", FileName);
#endif
        using (GameDataTableParser parse = new GameDataTableParser(path))
        {
            while (!parse.Eof)
            {
                P p = MakeEntity(parse);
                m_List.Add(p);
                m_Dic[p.id] = p;
                parse.Next();
            }
        }
    }
    public List<P> GetList()
    {
        return m_List;
    }
    public P Get(int id)
    {
        if (m_Dic.ContainsKey(id))
        {
            return m_Dic[id];
        }
        return null;
    }
    protected abstract P MakeEntity(GameDataTableParser parse);

	
}
