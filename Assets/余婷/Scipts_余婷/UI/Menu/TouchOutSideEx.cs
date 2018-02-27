using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 点击到范围外，关闭
/// 挂在Menu上
/// 要在所有子节点都添加之后再添加此脚本
/// </summary>
public class TouchOutSideEx : MonoBehaviour
{
    Transform[] _allTransform;
    TimeMgr.Timer _timer;

    public Action OutSideCallback;
    void Awake()
    {
        _allTransform = gameObject.GetComponentsInChildren<Transform>();
        EventSystem.current.SetSelectedGameObject(gameObject);

        _timer = TimeMgr.Instance.CreateTimer(0.1f, -1, CheckOutSide);
        _timer.Start();
    }

    void OnDestroy()
    {
        _timer.StopLoop();
    }

    void CheckOutSide()
    {
        bool bOutSide = true;
        foreach (var tr in _allTransform)
        {
            if (tr.gameObject == EventSystem.current.currentSelectedGameObject)
            {
                bOutSide = false;
                break;
            }
        }

        if (bOutSide)
        {
            if (OutSideCallback != null) { OutSideCallback.Invoke(); }
            _timer.StopLoop();
        }
    }
}
