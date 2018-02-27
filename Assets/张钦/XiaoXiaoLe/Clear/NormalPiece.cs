using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ClearMgr
{
    ClearMgr() { }
    static ClearMgr instance = null;
    public static ClearMgr Instance
    {
        get
        {
            if (instance == null) { instance = new ClearMgr(); }
            return instance;
        }
    }

    ArrayList outData = new ArrayList();
    public int magic = 0;
    public int heart = 0;
    public int shield = 0;
    public int sword = 0;
    //public List<int> Skill= new List<int>();//1 2 3 4分别为剑盾魔法爱心
    //public List<int> Rainbow = new List<int>();//大招，存入1，2，3，4同上
    public bool[] istrue = new bool[9];

    public void save(GamePiece piece)
    {
        if (piece.type == Grid.PieceType.Normal)
        {
            switch (piece.nowColor())
            {
                case ColorPiece.ColorType.SWORD:
                    sword += 1;
                    break;
                case ColorPiece.ColorType.SHIELD:
                    shield += 1;
                    break;
                case ColorPiece.ColorType.MAGIC:
                    magic += 1;
                    break;
                case ColorPiece.ColorType.HEART:
                    heart += 1;
                    break;
                default:
                    break;
            }
        }
        else if (piece.type == Grid.PieceType.Row_Clear)//小技能
        {
            // Debug.Log("小技能");
            switch (piece.nowColor())
            {
                case ColorPiece.ColorType.SWORD:

                    istrue[1] = true;

                    break;
                case ColorPiece.ColorType.SHIELD:

                    istrue[2] = true;

                    break;
                case ColorPiece.ColorType.MAGIC:

                    istrue[3] = true;

                    break;
                case ColorPiece.ColorType.HEART:

                    istrue[4] = true;

                    break;
                default:
                    Debug.Log("无小技能");
                    break;
            }
        }
        else if (piece.type == Grid.PieceType.Rainbow_Clear)//大招
        {
            //Debug.Log("大技能");
            if (piece.IsColored())
            {
                switch (piece.MaxSkill)
                {
                    case ColorPiece.ColorType.SWORD:
                        istrue[5] = true;
                        break;
                    case ColorPiece.ColorType.SHIELD:
                        istrue[6] = true;
                        break;
                    case ColorPiece.ColorType.MAGIC:
                        istrue[7] = true;
                        break;
                    case ColorPiece.ColorType.HEART:
                        istrue[8] = true;
                        break;
                    case ColorPiece.ColorType.ANY:
                        istrue[0] = true;
                        break;
                    default:
                        Debug.Log("无大技能");
                        break;
                }
            }
        }



    }
    //清除
    public void clear()//在下次移动时清除数据
    {
        magic = 0;
        heart = 0;
        shield = 0;
        sword = 0;
       
        for (int i = 0; i < istrue.Length; i++)
        {
            istrue[i] = false;
        }
        outData.Clear();//清空输出数据
    }

    public NormalPiece Out()//传出
    {
        //Debug.Log(magic + " " + heart + " " + shield + " " + sword);
        //foreach (var item in istrue)
        //{
        //    Debug.Log(item);
        //}
        NormalPiece nor = new NormalPiece();
        nor.magic = magic;
        nor.heart = heart;
        nor.shield = shield;
        nor.sword = sword;
        nor.istrue = istrue;
        //outData.Add(Skill);
        //outData.Add(Rainbow);
        //Debug.Log(magic+" "+heart+" "+shield+" "+sword);

        return nor;
    }
}

//返回的数据，显示本轮中消除了多少方块
public class NormalPiece
{
    public int magic;
    public int heart;
    public int shield;
    public int sword;
    public bool[] istrue = new bool[9];
}






