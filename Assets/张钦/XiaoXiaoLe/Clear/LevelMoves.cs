using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//有限步数
public class LevelMoves : Level {

    public int numMoves;//总步数
    public int targetScore;//目标分数

    int movesUsed = 0;//已经使用的步数
	void Start () {
        Type = LevelType.MOVES;//设置当前的类型为步数赛
        //hud.SetLevelType(Type);//设置显示文字
        //hud.SetTarget(targetScore);//设置目标分数
        //hud.SetScore(currentScore);//设置分数
        //hud.SetRemaining(numMoves);//设置可走的步数
	}

    public override void OnMove()
    {
        movesUsed++;
       // hud.SetRemaining(numMoves - movesUsed);
        if (numMoves - movesUsed == 0)
        {
            if (currentScore >= targetScore)
            {
                GameWin();
            }
            else
            {
                GameLose();
            }
        }
    }
}
