using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ShopTable
{
    public int ID;
    public string Name;
    public int ItemType;//物品类型
    public int Quality;//品质
    public string Description; //描述
    public int Price;//买价格
    public string Sprite; //图标地址
}

// 地图表
public class ShopTableData : ConfigTableBase<MapTableData, ShopTable>
{

}
