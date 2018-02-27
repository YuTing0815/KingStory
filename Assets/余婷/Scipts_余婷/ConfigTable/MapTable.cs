using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// 地图表的数据结构
public class MapTableData
{
    public int ID;
    public string Name;
    public string MapPath;
}

// 地图表
public class MapTable : ConfigTableBase<MapTableData, MapTable>
{
    
}
