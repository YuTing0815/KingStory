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

// 地图表1
public class MapTableOne : ConfigTableBase<MapTableData, MapTableOne>
{
    
}
// 地图表1
public class MapTableTwo : ConfigTableBase<MapTableData, MapTableTwo>
{

}
// 地图表1
public class MapTableThree : ConfigTableBase<MapTableData, MapTableThree>
{

}
// 地图表1
public class MapTableFour : ConfigTableBase<MapTableData, MapTableFour>
{

}
