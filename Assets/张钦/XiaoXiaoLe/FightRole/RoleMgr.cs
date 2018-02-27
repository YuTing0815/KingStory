using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleMgr:Singleton<RoleMgr>//全局保存人物属性
{
    MainRole role = new MainRole();
    public bool isClear = false;
    public MainRole Role
    {
        get { return role; }
    }
	
}
