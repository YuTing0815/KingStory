using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
public class InventoryManager:MonoBehaviour {
    //管理所有的物品，总管理器
    static InventoryManager _instance;
    public static InventoryManager Instance()
    {        
        return _instance;
    }
    void Awake() { _instance = this; }
    //单例
   
    List<Item> itemList;//保存所有物品

    public ToolTip tooltip;//获取提示框
    bool isToolTipTrue=false;

    public Canvas can;//ui画布

    Vector2 tooltipPositionOffSet = new Vector2(25,-25);//提示框偏移量

    
    void Start()
    {
        tooltip = GameObject.FindObjectOfType<ToolTip>();
        //tooltip.gameObject.SetActive(false);
        ParseItemJson(); //将所有json文件中的物品存入链表
        can = GameObject.Find("Canvas").GetComponent<Canvas>();
        pickItem = GameObject.Find("Pickitem").GetComponent<ItemUI>();
        pickItem.Hide();//隐藏拖拽框
        UpdateBag();
    }

    public void UpdateBag()//查找当前已有的物品并显示在背包中
    {
        if (PacketModel.Instance.packetList ==new List<ItemIndex>()) { return; }
        foreach (var item in PacketModel.Instance.packetList)
        {
            for (int i = 0; i < item.num; i++)
            {
                Knapsack.Instance.StoreItem(item.itemId);
            }
        }  ;
    }
    void Update()
    {
        if (isPickItem)//拖拽框跟随移动
        {
            Vector2 position;
            //获取鼠标在画布上的局部坐标
            RectTransformUtility.ScreenPointToLocalPointInRectangle(can.transform as RectTransform, Input.mousePosition, null, out position);
            pickItem.SetLocalPosition(position);
        }
        else if (isToolTipTrue)//提示框跟随鼠标
        {
            Vector2 position;
            //获取鼠标在画布上的局部坐标
            RectTransformUtility.ScreenPointToLocalPointInRectangle(can.transform as RectTransform, Input.mousePosition, null, out position);
            tooltip.SetLocalPosition(position+tooltipPositionOffSet);
        }

        //物品丢弃的处理
        if (isPickItem && Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1))//事件系统控制鼠标左键是否点击到物体
        {
            isPickItem = false;
            pickItem.Hide();
        }
    }
    #region  拖拽框方法调用

    public ItemUI pickItem;//拖拽框

    public bool isPickItem = false;//判断是否被选中（拖拽框上有无物体）

    public void PickItem(Item item,int amount)//更新拖拽框
    {
        pickItem.SetItem(item,amount);//拿出格子中所有物品更新鼠标拖拽物体的信息            
        isPickItem = true;
        pickItem.Show();//显示拖拽框
        tooltip.Hide();//隐藏提示框
    }

    public void RemoveItem(int number)//从拖拽框去掉一定数量放在格子中
    {
        pickItem.ReduceAmount(number);//减少拖拽框的数量
        if (pickItem.amount <= 0)
        {
            isPickItem = false;
            pickItem.Hide();
        }
    }
    //public bool IsPickItem
    //{
    //    get { return isPickItem; }
    //    set { IsPickItem = value; }
    //}
    #endregion
    #region 使用json解析物品信息,读取json中的物品信息
    void ParseItemJson()//从json文件中取出所有物品信息
    {
        itemList = new List<Item>();
        TextAsset itemText = Resources.Load<TextAsset>("Items");
      
        string itemJson = itemText.text;//物品信息的json格式
        
        JsonData jd= JsonMapper.ToObject(itemJson);   
        foreach (JsonData i in jd)
        {
           // print(i["type"]);
            //提取出类型，判断当前物品的类型
            Item.ItemType it =(Item.ItemType)  System.Enum.Parse(typeof(Item.ItemType), i["type"].ToString());//获取type属性字符串并将其转为枚举
            //共有属性先提取出来
            int id = int.Parse(i["id"].ToString());
           // print(id);
            string name = i["name"].ToString();
           // print(name);
            Item.Quality quality = (Item.Quality)System.Enum.Parse(typeof(Item.Quality), i["quality"].ToString());
           // print(quality);
            string description = i["Description"].ToString();
            //print(description);
            int capacity = int.Parse(i["Capacity"].ToString());
            //print(capacity);
            int buyprice = int.Parse(i["BuyPrice"].ToString());
            //print(buyprice);
            int sellprice = int.Parse(i["SellPrice"].ToString());
            //print(sellprice);
            string sprite = i["sprite"].ToString();
            //print(sprite);

            Item item = null;//创建item接收物体
            switch (it)
            {
                case Item.ItemType.Consumable:
                    int hp = int.Parse(i["hp"].ToString());
                    int mp = int.Parse(i["mp"].ToString());
                    item = new Consumable(hp,mp, id, name,it,quality,description,capacity,buyprice,sellprice,sprite);                  
                    break;

                case Item.ItemType.Equipment:
                  int stength = int.Parse(i["stength"].ToString());
                  int intellect = int.Parse(i["intellect"].ToString()); 
                  int agility = int.Parse(i["agility"].ToString());
                  int stamina = int.Parse(i["stamina"].ToString()); 
                  Equipment.EquipmentType equipType = (Equipment.EquipmentType)System.Enum.Parse(typeof(Equipment.EquipmentType), i["equipType"].ToString());
                  item = new Equipment(stength, intellect, agility, stamina, equipType, id, name, it, quality, description, capacity, buyprice, sellprice, sprite);
                    break;

                case Item.ItemType.Weapon:
                  int Damage = int.Parse(i["damage"].ToString()); ;
                  Weapon.WeaponType wpType = (Weapon.WeaponType)System.Enum.Parse(typeof(Weapon.WeaponType), i["wpType"].ToString());
                  item = new Weapon(Damage, wpType, id, name, it, quality, description, capacity, buyprice, sellprice, sprite);
                    break;

                case Item.ItemType.Material://材料类
                    item = new Material(id, name, it, quality, description, capacity, buyprice, sellprice, sprite);
                    break;
                default:
                    break;
            }
            itemList.Add(item);//将物品保存在链表中
        }
    }
    public Item GetItemById(int id)//传入id，返回物体信息
    {
        foreach (Item item in itemList)
        {
            if (item.ID == id)
            {
                return item;
            }
        }
        return null;
    }
    #endregion
    #region 提示框的显示隐藏
    //通过管理器控制提示框显示隐藏，并改变文字,item存有该方法。在slot中调用
    public void ShowTooltip(string content)
    {
        if (isPickItem) { return; }//如果当前拖拽框有物体，则不显示
        isToolTipTrue = true;
        tooltip.Show(content);//改变显示文字
       
    }

    public void HideTooltip()
    {
        isToolTipTrue = false;
        tooltip.Hide();
    }
    #endregion
}
