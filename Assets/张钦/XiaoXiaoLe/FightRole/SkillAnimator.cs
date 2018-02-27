using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SkillAnimator : MonoBehaviour {

    static SkillAnimator instance;
    public static SkillAnimator Instance
    {
        get { return instance; }
    }
    void Awake() { instance = this; }

  
    public List<Animator> humanAnimator;//存储已调用动画的对象的动画机
    Dictionary<Human,Animator> human = new Dictionary<Human, Animator>();//存储对应的动画控制器    

    public void Init()
    {
        human.Clear();//清空字典
        foreach (Role item in FightMgr.Instance.rolelist)
        {
            Animator ani = item.gameObject.transform.Find("skill").GetComponent<Animator>();
            human.Add(item, ani);
        }
        foreach (Enemy item in FightMgr.Instance.enemyList)
        {
            Animator ani = item.gameObject.transform.Find("skill").GetComponent<Animator>();
            human.Add(item, ani);
        }
    }

    //int i = 0;
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    i++;
        //    AllroleSkill(i);
        //    AllenemySkill(i);
        //}
    }


    public void Skill(Human man,int number)//控制某个角色出现特效
    {
        human[man].SetInteger("skill",number);
        humanAnimator.Add(human[man]);
        Invoke("closeAnimator", 1.5f);
    }

    public void AllroleSkill(int number)//全体主角特效
    {
        foreach (Role item in FightMgr.Instance.rolelist)
        {
            if (human[item] != null)
            {
                human[item].SetInteger("skill", number);
                humanAnimator.Add(human[item]);
            }
        }
        Invoke("closeAnimator", 1.5f);
    }

    public void AllenemySkill(int number)//全体敌人
    {
        foreach (Enemy item in FightMgr.Instance.enemyList)
        {
            if (human[item] != null)
            {
                human[item].SetInteger("skill", number);
                humanAnimator.Add(human[item]);
            }
        }
        Invoke("closeAnimator", 1.5f);
    }

    public void closeAnimator()
    {
        if (humanAnimator.Count != 0)
        {
            foreach (Animator item in humanAnimator)
            {
                if (item != null)
                {
                    item.SetInteger("skill", 0);
                }
            }
        }
        humanAnimator.Clear();//清除保存的对象
    }
}
