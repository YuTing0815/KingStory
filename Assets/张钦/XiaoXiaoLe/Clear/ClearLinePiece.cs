using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearLinePiece :ClearablePiece {
    //清除整条线 
    public bool isRow;//为true则可以使用

    public override void Clear()
    {
        base.Clear();

        if (RoleMgr.Instance.isClear)
        {
            if (isRow)
            {
                piece.GridRef.ClearColumn(piece.X);
                piece.GridRef.ClearRow(piece.Y);
            }
            else
            {
                piece.GridRef.ClearRow(piece.Y);
                piece.GridRef.ClearColumn(piece.X);
            }
        }
    }
}
