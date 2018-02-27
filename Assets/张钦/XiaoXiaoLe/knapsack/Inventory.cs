using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {      
    //背包和箱子的父类
  
    Slot[] slotlist;//保存所有的物品槽
  
    public virtual void Awake () {
        slotlist = GetComponentsInChildren<Slot>();//存储所有格子
        canvasgroup = GetComponent<CanvasGroup>();
	}
    #region  向格子中存储物品
    public bool StoreItem(int id)//通过id存储物品,bool表示存储是否成功
    {
        Item item = InventoryManager.Instance().GetItemById(id);
        return StoreItem(item);//此处进行存储
    }

    public bool StoreItem(Item item)//通过传入item存储物品 
    {
        if (item == null)
        {
            Debug.Log("不存在");
            return false;
        }
        if (item.Capacity == 1)//如果物品容量只为1，则查找空闲格子放入
        {
            Slot slot = FindEmptySlot();
            if (slot == null)
            {
                Debug.Log("没有空的物品槽");
                return false;
            }
            else
            {
                slot.storeItem(item);//把物品item放在这个空物品槽中
            }
        }
        else//若容量不为1，则查找和该物品类型相同的格子，查不到时查找空的物品槽
        {
            Slot slot = FindSameTypeSlot(item);
            if (slot != null)
            {
                slot.storeItem(item);
               
            }
            else
            {
                Slot sl = FindEmptySlot();
                if (sl != null)
                {
                    sl.storeItem(item);
                }
                else
                {
                    Debug.Log("没有空的物品槽");
                    return false;
                }
            }
        }
        return true;
    }

    Slot FindEmptySlot()//查找当前空的格子slot
    {  
        foreach (Slot slot in slotlist)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return null;
    }

    Slot FindSameTypeSlot(Item item)//判断是否有相同的格子
    {
       
        foreach (Slot slot in slotlist)
        {
            if (slot.transform.childCount >= 1&&slot.GetItemId()==item.ID&&slot.isFilled()==false)
                //当前格子只有一个子物体，当前格子的类型与物品一致，格子未达到最大容量
            {                
                return slot;
            }
        }
        return null;
    }
    #endregion

    #region 显示或隐藏背包或者箱子界面
    public float targetAlpha = 1;//表示目标透明度
    float smoothing = 4;
    CanvasGroup canvasgroup;//在awake中获取
    void Update () {
        if (canvasgroup.alpha != targetAlpha)
        {
            canvasgroup.alpha = Mathf.Lerp(canvasgroup.alpha, targetAlpha, smoothing * Time.deltaTime);
            if (Mathf.Abs(canvasgroup.alpha - targetAlpha) < 0.01f)//绝对值
            {
                canvasgroup.alpha = targetAlpha;
            }
        }   
	}

    public void Show() { targetAlpha = 1; canvasgroup.blocksRaycasts = true; }
    public void Hide() { targetAlpha = 0; canvasgroup.blocksRaycasts = false; }
    #endregion
}
