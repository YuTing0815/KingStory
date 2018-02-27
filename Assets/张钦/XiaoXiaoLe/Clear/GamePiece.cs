using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour {

    //格子上的物体脚本
    //每个格子保存以下数据
    public int score;//在预制体上设置每个多少分

    int x;
    int y;
    public Grid.PieceType type;//自己的类型

    public ColorPiece.ColorType MaxSkill;//当为五连消时，记录与自己互换的物体
    Grid grid;
    Movablepiece movableComponent;//控制移动脚本
    ColorPiece colorComponent;
    ClearablePiece clearableComponent;//清除当前物体脚本

    public State state = State.normal;//该格子当前携带的负面状态
    #region  //封装 格子的坐标，每个格子的坐标不能改变
    public int X
    {
        get
        {
            return x;
        }
        set
        {
            if (IsMovable()) { x = value; }
        }
    }

    public int Y
    {
        get
        {
            return y;
        }
        set
        {
            if (IsMovable()) { y = value; }
        }
    }

    public Grid.PieceType Type
    {
        get
        {
            return type;
        }
    }

    public Grid GridRef
    {
        get
        {
            return grid;
        }
    }

    public Movablepiece MovableComponent
    {
        get
        {
            return movableComponent;
        }
    }

    public ColorPiece ColorComponent
    {
        get
        {
            return colorComponent;
        }
    }

    public ClearablePiece ClearableComponent
    {
        get
        {
            return clearableComponent;
        }
    }
    #endregion
    void Awake () {
        movableComponent = GetComponent<Movablepiece>();//通过查看该属性是否为空来判断当前物体是否能移动
        colorComponent = GetComponent<ColorPiece>();
        clearableComponent = GetComponent<ClearablePiece>();       
    }
		
    public void Init(int _x,int _y,Grid _grid,Grid.PieceType _type)
    {
        x = _x;
        y = _y;
        type = _type;
        grid = _grid;
    }
    public bool IsMovable()//判断当前物体是否能移动
    {
        //print(gameObject.name);
        return movableComponent != null;
    }

    public bool IsColored()//判断当前图片物体是否存在colorpiece脚本
    {
        return colorComponent != null;
    }
    public bool IsClearable()//判断是否存在clearablePiece脚本
    {
        return clearableComponent != null;
    }

    public ColorPiece.ColorType nowColor()//获取当前脚本的颜色（不同的图片）
    {
        if (IsColored())
        {
            return gameObject.GetComponent<ColorPiece>().Color;
        }
        return 0;
    }

    public void ChangeSprite(Sprite sprite)//修改当前物体的图片
    {
        colorComponent.sprite.sprite = sprite;
    }

   // 为当前格子添加鼠标事件
    void OnMouseEnter()//
    { grid.EnterPiece(this); }
    void OnMouseDown()
    { grid.PressPiece(this); }
    void OnMouseUp()
    { grid.ReleasePiece(); }
}
