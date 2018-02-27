using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour {

    public Level level;
    public Text remainingText;
    public Text remainingSubtext;
    public Text targetText;
    public Text targetSubtext;
    public Text scoreText;
    public Image[] stars;

    int starIdx = 0;//控制当前四张星级图片其中之一显示
    bool isGameOver = false;
	void Start () {
        for (int i = 0; i < stars.Length; i++)//把三张带星的图片隐藏，先显示不带星的
        {
            if (i == starIdx) { stars[i].enabled = true; }
            else { stars[i].enabled = false; }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetScore(int score)//显示分数星级
    {
        scoreText.text = score.ToString();
        int visibleStar = 0;//判断显示哪张星级图片 
        if (score >= level.score1Star && score < level.score2Star)
        {
            visibleStar = 1;
        }
        else if (score >= level.score2Star && score < level.score3Star)
        { visibleStar = 2; }
        else if (score >= level.score3Star) { visibleStar = 3 ; }

        for (int i = 0; i < stars.Length; i++)
        {
            if (i == visibleStar) { stars[i].enabled = true; }
            else { stars[i].enabled = false; }
        }

        starIdx = visibleStar;
    }

    public void SetTarget(int target)//显示目标分数
    {
        targetText.text = target.ToString();
    }
    public void SetRemaining(int remaining)//整数 显示整数分数
    {
        remainingText.text = remaining.ToString();
    }
    public void SetRemaining(string remaining)//字符串 显示时间字符
    {
        remainingText.text = remaining;
    }
    public void SetLevelType(Level.LevelType type)
    {
        if (type == Level.LevelType.MOVES)
        {
            remainingSubtext.text = "剩余步数";
            targetSubtext.text = "目标分数";
        }
        else if (type == Level.LevelType.OBSTACLE)
        {
            remainingSubtext.text = "剩余步数";
            targetSubtext.text = "剩余障碍物";
        }
        else if (type == Level.LevelType.TIMER)
        {
            remainingSubtext.text = "剩余时间";
            targetSubtext.text = "目标分数";
        }
    }

    public void OnGameWin(int score)
    {
        isGameOver = true;
    }
    public void OnGameLose()
    {
        isGameOver = true;
    }
}
