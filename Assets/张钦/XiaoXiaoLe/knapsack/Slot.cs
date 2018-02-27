using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    public GameObject ItemPrefab;
    //每个物品格子的方法
    public void storeItem(Item item)//将item存储在自身，若已存在item，使amount++
    {
        if (transform.childCount == 0)
        {
            GameObject itemgameobj = Instantiate(ItemPrefab);
            itemgameobj.transform.SetParent(transform);
            itemgameobj.transform.localPosition = Vector3.zero;
            itemgameobj.transform.localScale = Vector3.one;
            itemgameobj.GetComponent<ItemUI>().SetItem(item);//默认数值为1
        }
        else//如果存在子节点，则调用增加数量方法
        {
            transform.GetChild(0).GetComponent<ItemUI>().AddAmount();//默认添加一数量
        }
    }
    public int GetItemId()//得到当前物品槽存储的物体类型
    {
        return transform.GetChild(0).GetComponent<ItemUI>().item.ID;
    }

    public bool isFilled()//判断容量和amount的大小
    {
        ItemUI itemui = transform.GetChild(0).GetComponent<ItemUI>();
        return itemui.amount >= itemui.item.Capacity;//当前数量大于等于容量
    }

    public void OnPointerEnter(PointerEventData eventData)//当鼠标进入，判断格子是否有物体，有则显示内容(在item中返回物品内容)
    {
        if (transform.childCount > 0)
        {
            string tooltext = transform.GetChild(0).GetComponent<ItemUI>().item.GetToolTipText();
            InventoryManager.Instance().ShowTooltip(tooltext);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            InventoryManager.Instance().HideTooltip();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //自身是空的 
        //（pickitem!=null）已选中格子，
        //按下ctrl 放置当前鼠标上物品的一个 
        //不按下ctrl 放下当前鼠标物品的所有个数
        //未选中格子  不作处理 

        //自身不为空   
        //已选中格子 物品交换
        //如果自身id与选中格子上的物品的id相同 
        //可以全部放入
        //只能放入一部分，剩下的仍在鼠标上
        //id不同时，进行交换

        //未选中格子  把当前格子的物品放到鼠标上
        //按下ctrl 取出当前各自内物品的一半
        //没有按下 全部取出

        if (transform.childCount > 0)//点击的格子有物体
        {
            ItemUI currentItem = transform.GetChild(0).GetComponent<ItemUI>();
            if (!InventoryManager.Instance().isPickItem)//当前拖拽框为空
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    //按下ctrl,可取格子内一半的数量
                    int amountPicked = (currentItem.amount + 1) / 2;//一半的数值
                    InventoryManager.Instance().PickItem(currentItem.item, amountPicked);
                    int amountLast = currentItem.amount - amountPicked;
                    if (amountLast <= 0)//如果取完后格子内数量为0，则直接销毁
                    {
                        Destroy(currentItem.gameObject);
                    }
                    else
                    {
                        currentItem.SetAmount(amountLast);//设置数量
                    }
                }
                else
                {
                    InventoryManager.Instance().PickItem(currentItem.item, currentItem.amount);//直接取出格子中所有数量
                    Destroy(currentItem.gameObject);
                    //把当前物品的信息赋值给拖拽框
                }
            }
            else//当拖拽框不为空
            {
                //已选中格子 则物品交换
                //如果自身id与选中格子上的物品的id相同 
                //可以全部放入
                //只能放入一部分，剩下的仍在鼠标上
                //id不同时，进行交换

                if (currentItem.item.ID == InventoryManager.Instance().pickItem.item.ID)
                //当前拖拽框物体与物品槽中物体相同，则可进行合并
                {
                    if (Input.GetKey(KeyCode.LeftControl))//按下ctrl时，每次从拖拽框放下一个物体
                    {
                        if (currentItem.item.Capacity > currentItem.amount)//当前格子物品还有容量
                        {
                            currentItem.AddAmount();//每次增加一个数量
                            InventoryManager.Instance().RemoveItem(1);
                        }
                        else { return; }//无容量时不作处理
                    }
                    else//没按下ctrl，直接进行合并
                    {
                        if (currentItem.item.Capacity > currentItem.amount)//当前格子还有容量时
                        {
                            int amountmain = currentItem.item.Capacity - currentItem.amount; //当前格子的剩余容量
                            if (amountmain >= InventoryManager.Instance().pickItem.amount)//如果剩余容量大于拖拽框内物体的容量
                            {
                                currentItem.AddAmount(InventoryManager.Instance().pickItem.amount);//当前格子数量增加
                                InventoryManager.Instance().RemoveItem(InventoryManager.Instance().pickItem.amount);//拖拽框数量减少
                            }
                            else//若剩余容量小于拖拽框物体的容量
                            {
                                currentItem.AddAmount(amountmain);
                                InventoryManager.Instance().RemoveItem(InventoryManager.Instance().pickItem.amount - amountmain);
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else//拖拽物体和格子物品不同 ,交换两者物品
                {
                    Item item = currentItem.item;
                    int amount = currentItem.amount;
                    currentItem.SetItem(InventoryManager.Instance().pickItem.item, InventoryManager.Instance().pickItem.amount);
                    InventoryManager.Instance().pickItem.SetItem(item, amount);
                }
            }
        }
        else//点击的格子无物体时
        {
            if (InventoryManager.Instance().isPickItem)//当拖拽框不为空时
            {
                if (Input.GetKey(KeyCode.LeftControl))//按下ctrl时，每次从拖拽框放下一个物体
                {
                    storeItem(InventoryManager.Instance().pickItem.item);//存入一个物体
                    InventoryManager.Instance().RemoveItem(1);
                }
               else//把拖拽框所有物体放到格子中
                {
                    for (int i = 0; i < InventoryManager.Instance().pickItem.amount; i++)//多次调用存入多个
                    {
                        storeItem(InventoryManager.Instance().pickItem.item);
                    }
                    InventoryManager.Instance().RemoveItem(InventoryManager.Instance().pickItem.amount);//拖拽框物体清空
                }             
            }
            else
            {
                return;
            }
        }
    }
}
