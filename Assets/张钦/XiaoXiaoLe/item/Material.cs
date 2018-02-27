using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material : Item
{
    //材料类
    public Material(int iD, string name, ItemType it, Quality quality, string description, int capacity, int buyPrice, int sellPrice, string sprite) : base(iD, name, it, quality, description, capacity, buyPrice, sellPrice,sprite)
    {
       
    }
}
