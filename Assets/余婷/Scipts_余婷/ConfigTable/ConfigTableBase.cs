using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

// 所有表的父类。
// 泛型参数含义<数据项结构>
public class ConfigTableBase<TDatabase, Ttable> : Singleton<Ttable> where TDatabase : new() where Ttable : new()
{
    List<TDatabase> _cache = new List<TDatabase>();

    protected ConfigTableBase()
    {
        read("Config/" + typeof(Ttable).ToString() + ".csv");
    }

    protected void read(string tablePath)
    {
        var tableAssetText = ResMgr.Instance.LoadTemplate<TextAsset>(tablePath);

        Stream tableStream = new MemoryStream(tableAssetText.bytes);

        using (var reader = new StreamReader(tableStream, Encoding.GetEncoding("gb2312")))
        {
            // 第一行表头，读取字段信息
            var line = reader.ReadLine();
            var fieldStr = line.Split(',');
            List<FieldInfo> fieldInfoList = new List<FieldInfo>();
            foreach (var field in fieldStr)
            {
                var fieldInfo = typeof(TDatabase).GetField(field);
                if (fieldInfo == null)
                {
                    Debug.LogError("未找到表中字段：" + field);
                    continue;
                }
                fieldInfoList.Add(fieldInfo);
            }

            // 后面是数据
            line = reader.ReadLine();
            while (line != null)
            {
                // 解析一行
                string[] itemStr = line.Split(',');

                var data = doParse(fieldInfoList, itemStr);
                _cache.Add(data);

                line = reader.ReadLine();
            }
        }

    }

    private TDatabase doParse(List<FieldInfo> fieldInfoList, string[] itemStr)
    {
        TDatabase data = new TDatabase();

        // 对每一个值(字段值)
        //int index = 0;
        //foreach (var item in itemStr)
        for (int index = 0; index < fieldInfoList.Count; ++index)
        {
            var fieldInfo = fieldInfoList[index];
            var item = itemStr[index];

            // 赋值
            if (fieldInfo.FieldType == typeof(int))
            {
                fieldInfo.SetValue(data, int.Parse(item));
            }
            else if (fieldInfo.FieldType == typeof(float))
            {
                fieldInfo.SetValue(data, float.Parse(item));
            }
            else if (fieldInfo.FieldType == typeof(bool))
            {
                fieldInfo.SetValue(data, bool.Parse(item));
            }
            else if (fieldInfo.FieldType == typeof(string))
            {
                fieldInfo.SetValue(data, item);
            }
            else if (fieldInfo.FieldType == typeof(List<int>))
            {
                var subItem = item.Split('$'); // item的值，是以下结构:"1$2$3$4"

                List<int> list = new List<int>();
                foreach (var subItemValue in subItem)
                {
                    list.Add(int.Parse(subItemValue));
                }

                fieldInfo.SetValue(data, list);
            }
            else if (fieldInfo.FieldType == typeof(List<float>))
            {
                var subItem = item.Split('$'); // item的值，是以下结构:"1$2$3$4"

                List<float> list = new List<float>();
                foreach (var subItemValue in subItem)
                {
                    list.Add(float.Parse(subItemValue));
                }

                fieldInfo.SetValue(data, list);
            }
            else if (fieldInfo.FieldType == typeof(List<string>))
            {
                var subItem = item.Split('$'); // item的值，是以下结构:"1$2$3$4"

                List<string> list = new List<string>();
                foreach (var subItemValue in subItem)
                {
                    list.Add(subItemValue);
                }

                fieldInfo.SetValue(data, list);
            }
        }

        return data;
    }

    public TDatabase this[int id]
    {
        get
        {
            return _cache[id - 1];
        }
    }
}
