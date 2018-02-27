using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//资源管理器（预留接口，以支持：热更新，性能分析，对象池）
//游戏逻辑中的所有资源加载，都应该放在此类中完成
class ResMgr:Singleton<ResMgr>
{
    //获取一个游戏对象（动态实例化）
    public GameObject GetGameObject(string path)
    {
        return GameObject.Instantiate(Resources.Load<GameObject>(path));
    }

    //模板
    public T LoadTemplate<T>(string path) where T:UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }


    //释放已创建的对象
    public void Release(GameObject obj)
    {
        GameObject.Destroy(obj);
    }
}
