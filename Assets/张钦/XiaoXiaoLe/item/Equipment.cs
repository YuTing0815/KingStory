using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
   
    public int Stength { get; set; }//力量
    public int Intellect { get; set; }//智力

    public int Agility { get; set; }//敏捷
    public int Stamina { get; set; }//体力
    public EquipmentType EquipType { get; set; }

    public Equipment(int stength, int intellect, int agility, int stamina, EquipmentType equipType,int iD, string name, ItemType it, Quality quality, string description, int capacity, int buyPrice, int sellPrice,string sprite) : base(iD, name, it, quality, description, capacity, buyPrice, sellPrice,sprite)
    {
        Stength = stength;
        Intellect = intellect;
        Agility = agility;
        Stamina = stamina;
        EquipType = equipType;
    }
    public enum EquipmentType
    {
        Chest,//胸甲
        Head,//头部
        Neck,//脖子
        Ring,//戒指
        Leg,//腿
        Bracer,//护腕
        Boots,//靴子
        Shoulder,//肩膀
        Belt,//腰带
        OffHand//副手

    }

    public override string GetToolTipText()
    {
        string text = base.GetToolTipText();
        string EquiptypeName = "";
        switch (EquipType)
        {
            case EquipmentType.Chest:
                EquiptypeName = "胸甲";
                break;
            case EquipmentType.Head:
                EquiptypeName = "头部";
                break;
            case EquipmentType.Neck:
                EquiptypeName = "项链";
                break;
            case EquipmentType.Ring:
                EquiptypeName = "戒指";
                break;
            case EquipmentType.Leg:
                EquiptypeName = "腿部";
                break;
            case EquipmentType.Bracer:
                EquiptypeName = "护腕";
                break;
            case EquipmentType.Boots:
                EquiptypeName = "鞋";
                break;
            case EquipmentType.Shoulder:
                EquiptypeName = "肩膀";
                break;
            case EquipmentType.Belt:
                EquiptypeName = "腰带";
                break;
            case EquipmentType.OffHand:
                EquiptypeName = "副手";
                break;
            default:
                break;
        }
        string newtext = string.Format("{0}\n\n<color=clear>装备类型：{5}\n力量+{1}\n智力+{2}\n敏捷+{3}\n体力+{4}</color>", text, Stength, Intellect,Agility, Stamina, EquiptypeName);
        return newtext;
    }
}
