using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleMgr : Singleton<RoleMgr>//全局保存人物属性
{
    MainRole role = new MainRole();
    public Dictionary<EquipType, Item> CurEquip;//当前的装备
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
    public int Diamond //全局的钻石
    {
        get { return role.Diamond; }
        set { role.Diamond = value; }
    }
    public List<ItemIndex> BagItem//背包里的物品,在PacketModel控制物品存储和物品的删除
    {
        get { return PacketModel.Instance.packetList; }
    }

    public bool isClear = false;

    public void EquipItem(EquipType type, int itemid)//装备物品
    {
        Item it = InventoryManager.Instance().GetItemById(itemid);
        if (CurEquip[type] != null)//把原有装备放入背包里
        {
            PacketModel.Instance.Save(CurEquip[type].ID);
        }
        CurEquip.Add(type, it);
    }

    public string CheckEquip(EquipType type)//传入装备部位，若存在装备则返回图片的加载地址
    {
        if (CurEquip[type] == null) { return ""; }
        return CurEquip[type].sprite;
    }


}
public enum EquipType
{
    头盔, 武器, 鞋, 盔甲
}