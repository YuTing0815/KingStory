using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObstacles : Level{
    //障碍赛

    public int numMoves;//移动步数
    public Grid.PieceType[] obstacleTypes;//手动设置只存入遮挡物类型
    int movesUsed = 0;//已用的步数
    int numObstaclesLeft;//剩余障碍物数量
	void Start () {
        Type = LevelType.OBSTACLE;
        for (int i = 0; i < obstacleTypes.Length; i++)//查找所有遮挡物类型物品
        {
            numObstaclesLeft += grid.GetPiecesOfType(obstacleTypes[i]).Count;
        }
        //hud.SetLevelType(Type);//设置显示文字
        //hud.SetTarget(numObstaclesLeft);//设置剩余障碍物
        //hud.SetScore(currentScore);//设置分数
        //hud.SetRemaining(numMoves);//设置剩余可走的步数
    }
	
	
	void Update () {
		
	}
    public override void OnMove()
    {
        movesUsed++;//已走步数+1
        //hud.SetRemaining(numMoves - movesUsed);//显示剩余个数
        if (numMoves - movesUsed == 0 && numObstaclesLeft > 0)//当步数为0且障碍物没被清除时
        {
            GameLose();
        }
    }

    public override void OnPieceCleared(GamePiece piece)//判断障碍物是否就减少
    {
        base.OnPieceCleared(piece);
        for (int i = 0; i < obstacleTypes.Length; i++)
        {
            if (obstacleTypes[i] == piece.Type)
            {
                numObstaclesLeft--;
                //hud.SetTarget(numObstaclesLeft);//减少障碍物
                if (numObstaclesLeft == 0)
                {
                    currentScore += 100 * (numMoves - movesUsed);
                   // hud.SetScore(currentScore);
                    GameWin();
                }
            }
        }
    }
}
