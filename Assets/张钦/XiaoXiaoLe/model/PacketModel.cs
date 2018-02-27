using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PacketModel : Singleton<PacketModel>
{
   public List<ItemIndex> packetList = new List<ItemIndex>() { new ItemIndex() { itemId = 1,num=2} };//保存获得的物品的信息

    public void Save(int itemId,int num=1)
    {
        ItemIndex item = new ItemIndex() {itemId = itemId,num = num};
        packetList.Add(item);
    }
    public void delete(int itemId,int num)
    {
        ItemIndex item= new ItemIndex();
        for (int i = 0; i < packetList.Count; i++)
        {
            if (packetList[i].itemId == itemId)
            {
                item = packetList[i];
                break;
            }
        }
        if (item == new ItemIndex()) { }
        if (item.num < num)
        {
            packetList.Remove(item);
        }
        else
        {
            item.num -= num;
        }
       
    }
}

public class ItemIndex
{
   public int itemId;
   public int num;
}