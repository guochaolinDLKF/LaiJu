using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameUtil
{
    public static Sprite LoadSprite(SpriteType type,string picName)
    {
        string path = string.Format("UI/{0}/{1}",type.ToString(), picName);
        return Resources.Load<Sprite>(path);
    }

    public static Sprite[] LoadSprites(SpriteType type)
    {
        string path = string.Format("UI/{0}/", type.ToString());
        return Resources.LoadAll<Sprite>(path);
    }

    public static Texture LoadTexture(SpriteType type, string picName)
    {
        string path = string.Format("UI/{0}/{1}", type.ToString(), picName);
        return Resources.Load<Texture>(path);
    }

    public static AudioClip LoadAudio(string audioName,AudioType type)
    {
        string path = string.Format("Sound/{0}/{1}", type.ToString(),audioName);
        return Resources.Load<AudioClip>(path);
    }

    public static Vector3 GetRandomPos(Vector3 targetPos,float distance)
    {
        Vector3 v = new Vector3(0f,0f,1f);
        v = Quaternion.Euler(0f,Random.Range(0f,360f),0f) * v;

        Vector3 pos = v * distance * Random.Range(0.8f,1f);
        return targetPos + pos;
    }

    public static Vector3 GetRandomPos(Vector3 currentPos, Vector3 targetPos, float distance)
    {
        Vector3 v = (currentPos - targetPos).normalized;
        v = Quaternion.Euler(0f, Random.Range(-90f,90f), 0f) * v;

        Vector3 pos = v * distance * Random.Range(0.8f, 1f);
        return targetPos + pos;
    }

    public static void Shake()
    {
        if (SystemProxy.Instance.IsShake)
        {
            Handheld.Vibrate();
        }
    }
}
