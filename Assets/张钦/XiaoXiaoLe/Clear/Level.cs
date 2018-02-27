using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    //关卡
    public enum LevelType
    {
        TIMER,
        OBSTACLE,
        MOVES,
    };
    public Grid grid;
   // public HUD hud;//获取界面脚本
    //分数显示星级
    public int score1Star;//一星
    public int score2Star;//二星
    public int score3Star;//三星

    private LevelType type;

    protected LevelType Type
    {
        get
        {
            return type;
        }
        set { type = value; }
    }

    protected int currentScore;

    void Start()
    {
       // hud.SetScore(currentScore);
    }

    public virtual void GameWin()
    {
        Debug.Log("you win");
       // hud.OnGameWin(currentScore);
        grid.GameOver();
    }
    public virtual void GameLose()
    {
        Debug.Log("lose");
      //  hud.OnGameLose();
        grid.GameOver();

    }

    public virtual void OnMove()//用户的操作和清除效果
    {
      //  Debug.Log("移动");
    }

    public virtual void OnPieceCleared(GamePiece piece)//更新最新分数
    {
        currentScore += piece.score;
        // Debug.Log(currentScore);
      //  hud.SetScore(currentScore);
    }
}
