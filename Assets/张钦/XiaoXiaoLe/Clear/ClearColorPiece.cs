using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearColorPiece :ClearablePiece {
    //用于消除所有同色的方块
    ColorPiece.ColorType color;

    public ColorPiece.ColorType Color
    {
        get { return color; }
        set {color = value; }
    }

    public override void Clear()
    {
        base.Clear();
        if (RoleMgr.Instance.isClear)
        {
            piece.GridRef.ClearColor(color);//清除某种颜色的元素
        }
    }
}
