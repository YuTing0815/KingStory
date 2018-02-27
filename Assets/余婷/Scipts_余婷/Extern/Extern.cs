using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public static class Extern
{
    public static T FindComponent<T>(this GameObject parent, string path)
    {
        return parent.transform.Find(path).GetComponent<T>();
    }
    //删除所有子节点信息
    public static void DestroyAllChildren(this GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; ++i)
        {
            GameObject.Destroy(parent.transform.GetChild(i).gameObject);
        }
    }
    public static void BtnAudioPlay(this Button btn)
    {
        AudioSource audio;
        audio = btn.gameObject.GetComponent<AudioSource>();
        audio.Play();
    }

    //public static void PlayAnim(this GameObject go)
    //{
    //    DOTweenAnimation dtanim;
    //    dtanim = go.GetComponent<DOTweenAnimation>();
    //    dtanim.autoPlay = true;       
    //}
}
