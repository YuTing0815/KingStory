using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
//UI管理器，所有UI的操作都由此类完成
class UIManager : Singleton<UIManager>
{
    //根节点
    public GameObject _root;
    //游戏的UI事件系统
    EventSystem _eventSys;
    //存放所有的UI
    Dictionary<UILayer, GameObject> _allLayerRoot = new Dictionary<UILayer, GameObject>();

    public bool OpenEventSystem
    {
        get { return _eventSys.enabled; }
        set { _eventSys.enabled = value; }
    }
    //表示UI的层次结构
    public void Init()
    {
        if (_root != null)
        {
            return;
        }
        else
        {
            //加载UI层次结构预制体
            _root = ResMgr.Instance.GetGameObject("Prafbs/UI/UISystem");
            _eventSys = _root.FindComponent<EventSystem>("EventSystem");
            //保存层次根节点
            _allLayerRoot.Add(UILayer.RoleUI, _root.transform.Find("RoleUI").gameObject);
            _allLayerRoot.Add(UILayer.MenuPanel, _root.transform.Find("MenuPanel").gameObject);
            _allLayerRoot.Add(UILayer.Normal, _root.transform.Find("Normal").gameObject);
            GameObject.DontDestroyOnLoad(_root);
        }
    }

    //UI的加载(UI的预制体路径，UI所在的层次)
    //返回新生成的UI根节点
    public GameObject AddUI(string path, UILayer layer = UILayer.Normal)
    {
        var uiRoot = ResMgr.Instance.GetGameObject(path);
        GameObject layerRoot = findLayerRoot(layer);
        if (layerRoot != null)
        {
            uiRoot.transform.SetParent(layerRoot.transform, false);
            return uiRoot;
        }
        else
        {
            Debug.LogError("未找到UI层次节点：" + layer);
            return null;
        }
    }

    //替换UI
    public GameObject ReplaceUI(string path, UILayer layer = UILayer.Normal)
    {
        //删除之前的UI
        var layerRoot = findLayerRoot(layer);
        for (int i = 0; i < layerRoot.transform.childCount; i++)
        {
            Release(layerRoot.transform.GetChild(i).gameObject);
        }
        return AddUI(path, layer);
    }

    private GameObject findLayerRoot(UILayer layer)
    {
        GameObject layerRoot;
        _allLayerRoot.TryGetValue(layer, out layerRoot);
        return layerRoot;
    }

    //UI的删除
    public void Release(GameObject uiRoot)
    {
        ResMgr.Instance.Release(uiRoot);
    }


}

/// <summary>
/// UI层次
/// </summary>
public enum UILayer
{
    RoleUI,
    MenuPanel,
    Normal,
}
