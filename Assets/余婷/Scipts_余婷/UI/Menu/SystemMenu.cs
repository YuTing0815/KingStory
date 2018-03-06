using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

//商城系统
public class SystemMenu
{
    SystemShopAndPackage systemShopAndPackage;
    GameObject _uiRoot;
    Button btnMenu;
    public void Init()
    {
        _uiRoot = UIManager.Instance.ReplaceUI("Prafbs/UI/PlayingMenu/Menu", UILayer.MenuPanel);
        btnMenu = _uiRoot.FindComponent<Button>("btnMenu");
        btnMenu.onClick.AddListener(BtnMenuClick);
        Debug.Log("Menu");
    }

    private void BtnMenuClick()
    {
        if (systemShopAndPackage != null)
        {
            GameObject.Destroy(systemShopAndPackage._uiRoot);
        }
        systemShopAndPackage = new SystemShopAndPackage();
        systemShopAndPackage.Init();
    }

    public void Release_UIRoot()
    {
        UIManager.Instance.Release(_uiRoot);
    }
}

