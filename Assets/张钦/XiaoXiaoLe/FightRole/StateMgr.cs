using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State//负面状态
{
    normal,//正常
    burn,//燃烧
    poison,//毒
    buddle,//障碍物
}
public class StateMgr : MonoBehaviour
{
    public Grid grid;

    void Awake()
    {
        grid = gameObject.GetComponent<Grid>();
    }

    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Q))
        {
            randomState(State.burn, 1);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            randomState(State.poison, 1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
           grid.HuifuState(2);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            RoleMgr.Instance.isClear = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            RoleMgr.Instance.isClear = false;
        }

    }

    public void randomState(State state,int num)//随机生成多个同一状态的物体(火，毒)
    {
        int i = 0;       
        while (i <num)
        {
            if (grid.ChangeState(Random.Range(0, grid.xDim), Random.Range(0, grid.yDim), state))
            {
                i++;
            }
        }
    }
    public void randomBuddle(int num)//随机生成多个障碍物
    {
        int i = 0;
        while (i < num)
        {
            if (grid.InstanceBubble(Random.Range(0, grid.xDim), Random.Range(0, grid.yDim)))
            {
                i++;
            }
        }
    }
    public void randomHuifu(int num)//恢复状态
    {
        grid.HuifuState(num);
    }
}
