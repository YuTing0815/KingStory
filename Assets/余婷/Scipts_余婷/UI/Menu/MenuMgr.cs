using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMgr : Singleton<MenuMgr>
{
   SystemMenu systemMenu;
   public  GameObject coverImage;
    public void Init()
    {
        systemMenu = null;
        systemMenu = new SystemMenu();
        systemMenu.Init();
        Debug.Log("初始化");
    }

    public void CoverImage()
    {
        coverImage = UIManager.Instance.AddUI("Prafbs/UI/map/MainMap/Image",UILayer.MenuPanel);
    }
    public void DestoryImage()
    {
        UIManager.Instance.Release(coverImage);
    }

    public void ReleseMenu()
    {
        systemMenu.Release_UIRoot();
    }
}
