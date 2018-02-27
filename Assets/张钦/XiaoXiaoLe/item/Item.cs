using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item  {//物品基类
    public int ID { get; set; }
    public string Name { get; set; }
    public ItemType it { get; set; }//物品类型
    public Quality quality { get; set; }//品质
    public string Description { get; set; }//描述
    public int Capacity { get; set; }//容量
    public int BuyPrice { get; set; }//买价格
    public int SellPrice { get; set; }//卖价格
    public string sprite { get; set; }//图标地址

    public Item() { ID = -1; }
    public Item(int iD, string name, ItemType it, Quality quality, string description, int capacity, int buyPrice, int sellPrice,string sprite)
    {
        ID = iD;
        Name = name;
        this.it = it;
        this.quality = quality;
        Description = description;
        Capacity = capacity;
        BuyPrice = buyPrice;
        SellPrice = sellPrice;
        this.sprite = sprite;
    }

   public enum ItemType
    {
        Consumable,//消耗品
        Equipment,//装备
        Weapon,//武器
        Material//材料
    }

   public enum Quality//品质
    {
        Common,
        UnCommon,
        Rare,
        Epic,
        Legendary,
        Artifact
    }

    //返回在提示框上显示的内容（物品信息）
    public virtual string GetToolTipText()
    {
        string color = "";
        switch (quality)
        {
            case Quality.Common:
                color = "white";
                break;
            case Quality.UnCommon:
                color = "grey";
                break;
            case Quality.Rare:
                color = "navy";
                break;
            case Quality.Epic:
                color = "red";
                break;
            case Quality.Legendary:
                color = "yellow";
                break;
            case Quality.Artifact:
                color = "orange";
                break;
            default:
                break;
        }

        string text = string.Format("<color={0}>{1}</color>\n<color=green>购买：${2}</color>\n<color=yellow>{3}</color>",color,Name,BuyPrice,Description);
        return text;//"<color="+color+">"+Name+"</color>"+ "\n<color=green>购买：$" +BuyPrice+"</color>\n<color=yellow>" + Description+"</color>"
    }
}
