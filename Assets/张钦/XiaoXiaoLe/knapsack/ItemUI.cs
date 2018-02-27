using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemUI : MonoBehaviour {
    //格子上的物品图片
	public Item item { set; get; }
    public int amount { get; set; }

    float TargetScale = 1f;//设置目标大小 当选中物品时物品会变大并逐渐恢复原大小
    Vector3 scale = new Vector3(1.15f, 1.15f, 1.15f);    
    Image itemimage;
    Text amountText;

    void Awake()
    {
        TargetScale = transform.localScale.x;
        itemimage = GetComponent<Image>();
        amountText = GetComponentInChildren<Text>();
    }
    void Update()
    {
        if (transform.localScale.x != TargetScale)//当选中物品时物品会变大并逐渐恢复原大小
        {
            float scale = Mathf.Lerp(transform.localScale.x, TargetScale,3*Time.deltaTime);
            transform.localScale = new Vector3(scale, scale, scale);
            if (Mathf.Abs(transform.localScale.x - TargetScale) < 0.02f)
            {
                transform.localScale = new Vector3(TargetScale, TargetScale, TargetScale);
            }
        }
    }
    public void SetItem(Item item,int amount=1)
    {
        transform.localScale = scale;
        this.item = item;
        this.amount = amount;
        //更新ui
        itemimage.sprite= Resources.Load<Sprite>(item.sprite);//使用resource加载图片
        amountText.text = amount.ToString();
    }

    public void SetAmount(int amount)
    {
        transform.localScale = scale;
        this.amount = amount;
        amountText.text = this.amount.ToString();
    }
    public void AddAmount(int amount=1)//增加数量
    {
        transform.localScale = scale;
        this.amount += amount;
        amountText.text = this.amount.ToString();
        //更新ui
    }
    public void ReduceAmount(int amount = 1)//减少数量
    {
        transform.localScale = scale;
        this.amount -= amount;
        amountText.text = this.amount.ToString();
    }
    public void SetLocalPosition(Vector3 position)//用于拖拽跟随鼠标
    {
        transform.localPosition = position;
    }

    public void Show()//显示
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
