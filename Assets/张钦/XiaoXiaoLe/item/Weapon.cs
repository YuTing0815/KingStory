using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {

    public int Damage { get; set; }

    public WeaponType wpType { get; set; }

    public enum WeaponType
    {
        OffHand,
        MainHand
    }

    public Weapon(int damage,WeaponType wea, int iD, string name, ItemType it, Quality quality, string description, int capacity, int buyPrice, int sellPrice,string sprite) : base(iD, name, it, quality, description, capacity, buyPrice, sellPrice,sprite)
    {
        Damage = damage;
        wpType = wea;
    }

    public override string GetToolTipText()
    {
        string text = base.GetToolTipText();
        string WeaponTypeName = "";
        switch (wpType)
        {
            case WeaponType.OffHand:
                WeaponTypeName = "副手武器";
                break;
            case WeaponType.MainHand:
                WeaponTypeName = "主武器";
                break;
            default:
                break;
        }
        string newtext = string.Format("{0}\n\n<color=clear>{1}\n攻击力:{2}</color>", text,WeaponTypeName,Damage );
        return newtext;
    }
}
