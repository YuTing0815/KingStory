using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//计时器管理器（作为封装Timer，管理Timer）
public class TimeMgr : Singleton<TimeMgr>
{
    //定时器驱动
    private Action TimerFixedLoop;

    //创建一个定时器
    public Timer CreateTimer(float duringTime, int repeateTime, Action callBack)
    {
        var timer = new Timer();
        timer.DuringTime = duringTime;
        timer.RepeatTimes = repeateTime;
        timer.CallBack = callBack;
        return timer;
    }

    //做一个简单的时间传递(驱动)
    public void FixedLoop()
    {
        if (TimerFixedLoop!=null)
        {
            TimerFixedLoop();
        }
    }

    //定时器   相当于多个闹钟
    public class Timer
    {
        //定时周期
        public float DuringTime;
        //定时重复次数  //小于零 表示无限循环
        public int RepeatTimes;
        //输出闹铃  （通知起床  委托）
        public Action CallBack;

        //计时开始时间
        private float _startTime;
        //已经重复了多少次
        private int _repeatedTimes;
        //开始计时
        public void Start()
        {
            _startTime = Time.time;
            _repeatedTimes = 0;
            TimeMgr.Instance.TimerFixedLoop += FixedLoop;
        }

        //此函数定时循环调用
        public void FixedLoop()
        {
            if (Time.time - _startTime > DuringTime)
            {
                //叮铃铃一下
                _startTime = Time.time;
                _repeatedTimes++;

                //记录重复次数
                if ((RepeatTimes > 0) && _repeatedTimes > RepeatTimes)
                {
                    StopLoop();
                    return;
                }
                if (CallBack != null) { CallBack(); }
            }
        }
        public void StopLoop()
        {
            TimeMgr.Instance.TimerFixedLoop -= FixedLoop;
        }
    }
}




