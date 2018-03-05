using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PacketModel : Singleton<PacketModel>
{
    //保存背包中的物品
   public List<ItemIndex> packetList = new List<ItemIndex>() ;//保存获得的物品的信息

    public void Save(int itemId,int num=1)//传入id将num个物品存入背包
    {
        ItemIndex item = new ItemIndex() {itemId = itemId,num = num};
        packetList.Add(item);
    }
    public void Delete(int itemId,int num)//删除num个物品
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

    public List<Items> GetAllType(Item.ItemType itemType)//获取当前背包里的某种类型的全部物品(消耗，装备，武器，材料)
    {
        if (packetList == new List<ItemIndex>()) { return null; }
        List<Items> it = new List<Items>();
        for (int i = 0; i < packetList.Count; i++)
        {
            if (InventoryManager.Instance().GetItemById(packetList[i].itemId).it == itemType)
            {
                Items items = new Items() { itemId = packetList[i].itemId, item = InventoryManager.Instance().GetItemById(packetList[i].itemId), num = packetList[i].num };
                it.Add(items);
            }
        }
        return it;
    }

}

public class ItemIndex
{
   public int itemId;
   public int num;
}
public class Items
{
    public int itemId;
    public Item item;
    public int num;
}