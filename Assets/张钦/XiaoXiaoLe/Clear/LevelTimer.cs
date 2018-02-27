using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : Level{

    public int timeInSeconds;//总时间
    public int targetScore;//目标分数
    private float timer;//已消耗的时间
    bool timeOut= false;//时间结束
	void Start () {
        Type = LevelType.TIMER;
        //hud.SetLevelType(Type);//设置显示文字
        //hud.SetTarget(targetScore);//设置目标时间
        //hud.SetScore(currentScore);//设置分数
        //hud.SetRemaining(string.Format("{0}:{1:00}",timeInSeconds/60,timeInSeconds%60));//设置剩余可走的步数
    }
	
	// Update is called once per frame
	void Update () {
        if (!timeOut)
        {
            timer += Time.deltaTime;
           // hud.SetRemaining(string.Format("{0}:{1:00}", (int)Mathf.Max((timeInSeconds - timer) / 60, 0), (int)Mathf.Max((timeInSeconds - timer) % 60, 0)));

            if (timeInSeconds - timer <= 0)
            {
              
                if (currentScore >= targetScore)
                {
                    GameWin();
                }
                else
                {
                    GameLose();
                }
                timeOut = true;
            }
        }
	}
}
