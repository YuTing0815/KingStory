using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleMgr:Singleton<RoleMgr>//全局保存人物属性
{
    MainRole role = new MainRole();
    public MainRole Role//全局的人物属性
    {
        set { role = value; }
        get { return role; }
    }

    public int Money //全局的金币
    {
        get { return role.Money; }
        set { role.Money = value; }
    }
    public List<ItemIndex> BagItem//背包里的物品,在PacketModel控制物品存储和物品的删除
    {
        get {return PacketModel.Instance.packetList; }  
    }
    

    public bool isClear = false;
   
}
