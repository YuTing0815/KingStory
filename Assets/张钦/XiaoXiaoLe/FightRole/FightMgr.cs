
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Hero
{
    saber,
    caster,
    shield
}
public class FightMgr : MonoBehaviour
{
    public MainRole role;//主角小队血量及属性
    public float fillTime = 2f;

    public Dictionary<Hero, Role> roleDic = new Dictionary<Hero, Role>();//查找不同的主角
    public List<Role> rolelist;//方便对主角进行统一操作
    public List<Enemy> enemyList = new List<Enemy>();//生成的敌人列表
    public int Levelnum = 2;//控制敌人波次

    public Text hurt;
    public Text health;
    public Text shield;
    
    public GameObject[] vec = new GameObject[6];//记录主角和敌人生成的位置
    bool[] isvec = new bool[6];//记录主角和敌人位置是否被占用

    public Enemy curEnemy;//目标敌人
    public List<GameObject> prefab = new List<GameObject>();//主角模型
    public GameObject[] enemyPrefab;//敌人模型

    NormalPiece nor;//获取的本回合方块消除数据

    public Grid grid;
    public StateMgr stateMgr;

    static FightMgr fightmgr;
    public static FightMgr Instance
    {
        get { return fightmgr; }
    }
    void Awake()
    {
        fightmgr = this;
    }
    void Start()
    {
        Levelnum = Random.Range(2, 4);
        enableText();
        stateMgr = grid.gameObject.GetComponent<StateMgr>();
        role = RoleMgr.Instance.Role;//获取控制器上的人物属性
        CreateRole();
        //与UI中进行交互
        Debug.Log(RoleInfoMgr.Instance.hpValue.name);
        RoleInfoMgr.Instance.hpValue.maxValue = role.MaxHp;
        RoleInfoMgr.Instance.hpValue.value = role.Hp;
        RoleInfoMgr.Instance.shieldValue.maxValue = role.MaxShield;
        RoleInfoMgr.Instance.shieldValue.value = role.Shield;
        RoleInfoMgr.Instance.maxTimeRound.text = Levelnum.ToString();
        RoleInfoMgr.Instance.coinCounts.text = role.Money.ToString();
        RoleInfoMgr.Instance.timeRound.text = "1";


        OnceAgain();//创建敌人并进行初始化
                    //CreateEnemy(1, enemyPrefab[1]);
                    //CreateEnemy(1, enemyPrefab[0]);

        //SkillAnimator.Instance.Init();
    }

    public void CreateRole()//创建主角
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject hero = GameObject.Instantiate(prefab[i]);
            hero.transform.position = vec[i].transform.position;
            Role ro = hero.AddComponent<Role>();

            rolelist.Add(ro);
            if (prefab[i].name == "Saber")
            {
                roleDic.Add(Hero.saber, ro);
            }
            else if (prefab[i].name == "Caster")
            {
                roleDic.Add(Hero.caster, ro);
            }
            else { roleDic.Add(Hero.shield, ro); }
        }
    }
    public void CurEnemy(Enemy enemy)//为三个主角指定敌人
    {
        foreach (Role item in rolelist)
        {
            item.SetCurEnemy(enemy);
        }
        curEnemy = enemy;
    }
    public void CreateEnemy(int Level, GameObject game, int isboss = 0)//创造一个敌人
    {
        GameObject enemy = GameObject.Instantiate(game);
        for (int i = 3; i < isvec.Length; i++)
        {
            if (!isvec[i])//如果为false则该位置没人
            {
                isvec[i] = true;
                enemy.GetComponent<SpriteRenderer>().sortingOrder = i;//显示图层为当前值
                enemy.transform.position = vec[i].transform.position;
                break;
            }
        }
        // enemy.transform.position = vec[1].transform.position;
        enemy.transform.localScale = new Vector3(enemy.transform.localScale.x * -1, 1, 1);
        Enemy ene = enemy.AddComponent<Enemy>();
        ene.isBoss = isboss;
        ene.Init(Level);//初始化敌人属性脚本
        Debug.Log("加入敌人队列");
        Debug.Log(ene.name);
        enemyList.Add(ene);
    }

    public void Init(NormalPiece nor)//在Grid中调用，回合初始化主函数
    {
        this.nor = nor;//获取并解析本轮中消除的格子数量

        for (int i = 0; i < enemyList.Count; i++)//查找一个当前敌人
        {
            if (enemyList[i] != null)
            {
                Debug.Log("找到了一个敌人");
                CurEnemy(enemyList[i]);//设置敌人目标
                break;
            }
        }

        StartCoroutine(Fight());
        //协程控制人物动画
    }


    IEnumerator Fight() //主角攻击回合
    {
        //查看场上是否存在状态，存在则执行对应操作
        bool ismagic = true;
        bool isheart = true;
        bool isshield = true;
        bool issword = true;
        for (int i = 0; i < nor.istrue.Length; i++)
        {
            if (nor.istrue[i])//使用了技能
            {
                //rolelist[0].goMove();
                //yield return new WaitForSeconds(fillTime);
                switch (i)
                {
                    case 0:
                        //大招
                        Debug.Log(0);
                        yield return new WaitForSeconds(fillTime);
                        break;
                    case 1:
                        //剑
                        //roleDic[Hero.saber].isSkill = true;                       
                        roleDic[Hero.saber].goMove();
                        yield return new WaitForSeconds(1);
                        PhyAttack(1);//计算伤害
                        roleDic[Hero.saber].Back();
                        issword = false;
                        break;
                    case 2:
                        //盾
                        roleDic[Hero.shield].isSkill = true;
                        roleDic[Hero.shield].goMove();
                        yield return new WaitForSeconds(1);
                        shieldUp(1);//计算伤害
                        roleDic[Hero.shield].Back();
                        isshield = false;
                        break;
                    case 3:
                        //魔法
                        roleDic[Hero.caster].isSkill = true;
                        roleDic[Hero.caster].goMove();
                        yield return new WaitForSeconds(1);
                        MagAttack(1); //计算伤害             
                        roleDic[Hero.caster].Back();
                        ismagic = false;
                        break;
                    case 4:
                        //爱心
                        roleDic[Hero.shield].isSkill = true;
                        roleDic[Hero.shield].goMove();
                        isheart = false;
                        yield return new WaitForSeconds(1);
                        HpUp(1);//计算伤害
                        roleDic[Hero.shield].Back();
                        break;
                    case 5:
                        //剑技
                        roleDic[Hero.saber].isMaxSkill = true;
                        roleDic[Hero.saber].goMove();
                        yield return new WaitForSeconds(1);
                        PhyAttack(2);//计算伤害
                        roleDic[Hero.saber].Back();
                        issword = false;
                        break;
                    case 6:
                        //盾技
                        roleDic[Hero.shield].isMaxSkill = true;
                        roleDic[Hero.shield].goMove();
                        yield return new WaitForSeconds(1);
                        shieldUp(2);//计算伤害
                        roleDic[Hero.shield].Back();
                        isshield = false;
                        break;
                    case 7:
                        //魔法
                        roleDic[Hero.caster].isMaxSkill = true;
                        roleDic[Hero.caster].goMove();
                        yield return new WaitForSeconds(1);
                        MagAttack(2);//计算伤害
                        roleDic[Hero.caster].Back();
                        ismagic = false;
                        break;
                    case 8:
                        //爱心
                        roleDic[Hero.shield].isMaxSkill = true;
                        roleDic[Hero.shield].goMove();
                        isheart = false;
                        yield return new WaitForSeconds(1);
                        HpUp(2);//计算伤害
                        roleDic[Hero.shield].Back();
                        break;
                    default:
                        break;
                }
                //rolelist[0].Back();
                yield return new WaitForSeconds(fillTime);
            }

        }

        if (issword && nor.sword != 0)
        {
            roleDic[Hero.saber].goMove();

            yield return new WaitForSeconds(1);
            //执行攻击方法
            PhyAttack();
            roleDic[Hero.saber].Back();
            yield return new WaitForSeconds(fillTime);
        }

        if (isshield && nor.shield != 0)
        {
            if (role.Shield != role.MaxShield) { roleDic[Hero.shield].isSkill = true; }//如果不等于，调用恢复技能动画
            roleDic[Hero.shield].goMove();
            yield return new WaitForSeconds(1);
            shieldUp();//计算伤害
            roleDic[Hero.shield].Back();
            yield return new WaitForSeconds(fillTime);

        }
        if (ismagic && nor.magic != 0)//执行魔法方法
        {
            roleDic[Hero.caster].goMove();

            yield return new WaitForSeconds(1);
            MagAttack();//计算伤害
            roleDic[Hero.caster].Back();
            yield return new WaitForSeconds(fillTime);

        }
        if (isheart && nor.heart != 0) //执行恢复方法
        {
            roleDic[Hero.shield].isSkill = true;
            roleDic[Hero.shield].goMove();
            yield return new WaitForSeconds(1);
            HpUp();//恢复方法
            roleDic[Hero.shield].Back();
            yield return new WaitForSeconds(fillTime);

        }

        //协程控制敌人动画
        StartCoroutine(EnemyFight());
    }

    IEnumerator EnemyFight()//敌人攻击回合
    {
        //遍历，若有敌人死亡则销毁敌人
        DestoryEnemy();
        if (enemyList.Count != 0)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (enemyList[i].subRounds())//回合数减少，0时攻击
                {
                    enemyList[i].role = rolelist[UnityEngine.Random.Range(0, 3)];
                    enemyList[i].goMove();
                    yield return new WaitForSeconds(1);
                    Hurt(Random.Range(Mathf.CeilToInt(enemyList[i].enemy.PhyAttck * 0.8f), enemyList[i].enemy.PhyAttck * 2));//敌人攻击方法,伤害随机计算

                    switch (enemyList[i].isBoss)
                    {
                        case 2:
                            if (Random.Range(0, 2) == 0)
                            {
                                stateMgr.randomBuddle(Random.Range(2, 5));//随机生成障碍
                            }
                            break;
                        case 3:
                            if (Random.Range(0, 2) == 0)
                            {
                                stateMgr.randomState(State.burn, Random.Range(1, 3));//随机生成燃烧状态
                            }
                            break;
                        case 4:
                            if (Random.Range(0, 2) == 0)
                            {
                                stateMgr.randomState(State.poison, Random.Range(1, 3));//随机生成中毒状态
                            }
                            break;
                        default:
                            break;
                    }

                    enemyList[i].Back();
                    yield return new WaitForSeconds(fillTime);
                }
            }
        }

        if (enemyList.Count == 0 && Levelnum > 1)//如果没有敌人,则再次生成,共三波敌人
        {
            MapTranslate.Instance.MapRun();//地图移动
            roleMove();//人物移动动画
            yield return new WaitForSeconds(2.4f);
            OnceAgain();
            roleStop();//人物停止动画
            int i = int.Parse(RoleInfoMgr.Instance.timeRound.text);
            i++;
            RoleInfoMgr.Instance.timeRound.text = i.ToString();//显示回合数
            Levelnum -= 1;

        }

        if (enemyList.Count == 0 && Levelnum == 1)
        {
            Debug.Log("Success");
            Vectory();
        }

        CheckState();//检查消消乐棋盘是否存在debuff
        grid.Start();//人物动画执行完毕后打开消消乐界面格子拖动操作
    }

    public void Hurt(int hurt)//主角受伤方法
    {
        ableHurt(hurt);
        Invoke("enableText", 1);//延迟关闭text函数
        role.Hurt(hurt);
        if (role.Hp <= 0)
        {
            end();
        }
        RoleInfoMgr.Instance.hpValue.maxValue = role.MaxHp;
        RoleInfoMgr.Instance.hpValue.value = role.Hp;
        RoleInfoMgr.Instance.shieldValue.maxValue = role.MaxShield;
        RoleInfoMgr.Instance.shieldValue.value = role.Shield;
    }
    public void bloodHurt(int hurt)//直接扣除血量
    {
        ableHurt(hurt);
        Invoke("enableText", 1);//延迟关闭text函数
        role.Hp -= hurt;
        if (role.Hp <= 0) { role.Hp = 0; end(); }
        RoleInfoMgr.Instance.hpValue.maxValue = role.MaxHp;
        RoleInfoMgr.Instance.hpValue.value = role.Hp;

    }
    #region 显示主角受到的伤害及回复值
    public void ableHealth(int hea)//修改text显示的人物恢复值
    {
        health.text = "+" + hea; health.enabled = true;
        Invoke("enableText", 1);
    }
    public void ableHurt(int hur)//修改显示的人物被伤害值
    {
        hurt.text = "-" + hur; hurt.enabled = true;
        Invoke("enableText", 1);
    }
    public void ableShield(int shi)//修改显示的人物盾牌值
    {
        shield.text = "+" + shi; shield.enabled = true;
        Invoke("enableText", 1);
    }
    public void enableText()//关闭text窗口
    {
        hurt.enabled = false;
        health.enabled = false;
        shield.enabled = false;
    }
    #endregion

    public void CheckState()//检查当前是否存在debuff状态，若存在，则执行对应操作
    {
        int[] statenum = grid.CheckState();
        if (statenum[0] != 0)
        {
            Hurt(statenum[0] * 3);//根据数量造成伤害
            stateMgr.randomState(State.burn, Random.Range(1, 3));//额外生成两个
        }
        if (statenum[1] != 0)
        {
            bloodHurt(statenum[0] * 3);//直接扣除血量
        }
    }
    public void end()//主角死亡方法
    {
        print("主角死了");
        grid.GameOver();
        FailOrSuccess.Instance.InitFail();
    }
    public void Vectory()
    {
        Debug.Log("胜利");
        grid.GameOver();
        FailOrSuccess.Instance.InitSuccess();
        //打开胜利Ui界面
    }

    #region 主角技能控制
    public void PhyAttack(int i = 0)//剑士技能伤害
    {
        switch (i)
        {
            case 1://小技能
                SkillAnimator.Instance.Skill(curEnemy, 7);
                curEnemy.Hurt((int)(role.PhyAttck * 1f * (nor.sword)));//重攻击并恢复血量
                role.HpUp((int)(role.PhyAttck * 1f * (nor.sword) * 0.4f));
                break;
            case 2:
                SkillAnimator.Instance.Skill(curEnemy, 6);
                curEnemy.Hurt((int)(role.PhyAttck * 2.5f * (nor.sword) + role.Level * 50));//对单体造成巨大伤害                
                break;
            default:
                //SkillAnimator.Instance.Skill(curEnemy, 6);
                curEnemy.Hurt((int)(role.PhyAttck * 0.8f * (nor.sword)));//普通攻击
                break;
        }
    }

    public void MagAttack(int i = 0)//魔法技能伤害
    {
        switch (i)
        {
            case 1://小技能
                SkillAnimator.Instance.Skill(curEnemy, 5);
                curEnemy.Hurt((int)(role.MagAttack * 1.2f * (nor.magic)));//重攻击              
                break;
            case 2:
                SkillAnimator.Instance.AllenemySkill(3);
                foreach (Enemy item in enemyList)
                {
                    if (item != null)
                    {
                        item.Hurt((int)(role.PhyAttck * 2f * (nor.magic)) + role.Level * 50);//对敌人全体造成巨大伤害                
                    }
                }

                break;
            default:
                SkillAnimator.Instance.Skill(curEnemy, 4);
                curEnemy.Hurt((int)(role.PhyAttck * 1f * (nor.magic)));//普通攻击
                break;
        }
    }

    public void shieldUp(int i = 0)//盾的技能伤害
    {
        switch (i)
        {
            case 1://小技能
                SkillAnimator.Instance.AllroleSkill(2);
                role.ShieldUp((int)(role.PhyAttck * 1.5f * (nor.shield)) + role.Level * 10);//根据数量恢复盾牌，并攻击       
                curEnemy.Hurt((int)(role.PhyAttck * 1f * (nor.shield)));
                break;
            case 2:
                SkillAnimator.Instance.AllroleSkill(2);
                role.ShieldUp((int)(role.PhyAttck * 3f * (nor.shield)) + role.Level * 15);//恢复大量盾牌值
                ////下回合无敌
                break;
            default:
                if (role.Shield == role.MaxShield)//若盾牌满则攻击
                {
                    curEnemy.Hurt((int)(role.PhyAttck * 0.6f * (nor.shield)));
                }
                else
                {
                    SkillAnimator.Instance.AllroleSkill(8);
                    roleDic[Hero.shield].isSkill = true;//播放技能动作                    
                    role.ShieldUp((int)(role.PhyAttck * 1f * (nor.shield)) + role.Level * 5);//根据数量恢复盾牌
                }
                break;
        }
    }

    public void HpUp(int i = 0)//血量技能
    {
        switch (i)
        {
            case 1:
                SkillAnimator.Instance.AllroleSkill(1);
                role.HpUp((int)(role.MagAttack * 1f * (nor.heart)) + role.Level * 20);//根据数量恢复生命
                stateMgr.randomHuifu(Random.Range(2, 4));//清除Debuff
                break;
            case 2:
                SkillAnimator.Instance.AllroleSkill(1);
                role.HpUp((int)(role.MagAttack * 1.4f * (nor.heart)) + role.Level * 50);//根据数量恢复生命，死亡时复活
                stateMgr.randomHuifu(5);//清除Debuff
                break;
            default:
                SkillAnimator.Instance.AllroleSkill(8);
                role.HpUp((int)(role.MagAttack * 0.8f * (nor.heart)) + role.Level * 10);//根据数量恢复生命
                break;
        }
    }
    #endregion

    public void DestoryEnemy()//删除isDead的敌人
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i].isDead)
            {
                Destroy(enemyList[i].gameObject);
                enemyList.Remove(enemyList[i]);
            }
        }
    }

    public void OnceAgain()//敌人全部被消灭后再次创建敌人
    {
        enemyList.Clear();
        for (int i = 3; i < 6; i++)//清空格子的bool值
        {
            isvec[i] = false;
        }
        for (int i = 0; i < Random.Range(1, 4); i++)//随机生成多个敌人
        {
            int j = Random.Range(0, enemyPrefab.Length);

            CreateEnemy(role.Level, enemyPrefab[j], j);//随机生成一个敌人
        }

        SkillAnimator.Instance.Init();//初始化获取场上敌人和主角的动画机


        //判断是否有剩余关卡
        //有则继续移动（场景移动）
        //roleMove();
        //无则本次战斗胜利
    }

    public void roleMove()//播放人物移动动画
    {
        foreach (Role item in rolelist)
        {
            item.Run();
        }
    }

    public void roleStop()//播放人物站立动画
    {
        foreach (Role item in rolelist)
        {
            item.Idle();
        }
    }
}

