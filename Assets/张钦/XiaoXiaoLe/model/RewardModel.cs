using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RewardModel : Singleton<RewardModel>
{
    public int[] VectorReward()
    {
        int[] reward = new int[2];
        reward[0] = UnityEngine.Random.Range(50,100);
        reward[1] = UnityEngine.Random.Range(30,50);
        return reward;
    }
}

