using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{

    public int Hp { get; set; }
    public int Mp { get; set; }

    public Consumable(int hp, int mp, int iD, string name, ItemType it, Quality quality, string description, int capacity, int buyPrice, int sellPrice,string sprite) : base(iD, name, it, quality, description, capacity, buyPrice, sellPrice,sprite)
    {
        this.Hp = hp;
        this.Mp = mp;
    }

    public override string GetToolTipText()
    {
        string text = base.GetToolTipText();
        string newtext = string.Format("{0}\n\n<color=clear>Hp恢复{1}\nMp恢复{2}</color>", text,Hp,Mp);
        return newtext;
    }
}