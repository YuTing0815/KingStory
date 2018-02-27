using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Role : MonoBehaviour,Human {

    public Vector3 Pos;
    bool isGo;
    bool isBack;
    public bool isSkill;//触发技能
    public bool isMaxSkill;//触发必杀技
    Animator ani;
    
    public Enemy curEnemy;//目标敌人
    

    void Awake () {
        Pos = transform.position;
        ani = GetComponent<Animator>();
       
    }

    void Update()
    {
        if (isGo)
        {
           // Debug.Log("移动");
            transform.localPosition = Vector2.Lerp(transform.localPosition, curEnemy.transform.localPosition, 0.1f);
            if (Vector2.Distance(transform.localPosition, curEnemy.transform.localPosition) < 1.2)
            {
                if (isSkill)//小技能动作
                {
                    //Debug.Log(123);
                    Skill();
                    isSkill = false;
                }
                else if (isMaxSkill)//大技能动作
                {
                    //Debug.Log(234);

                    MaxSkill();
                    isMaxSkill = false;
                }
                else
                {                   
                    Attack();
                }//普通攻击动作
                isGo = false;
            }
        }
        if (isBack)
        {
           // Debug.Log("返回位置");
            transform.localPosition = Vector2.Lerp(transform.localPosition, Pos, 0.1f);
            if (Vector2.Distance(transform.localPosition, Pos) < 0.3)
            {                             
                transform.position = Pos;
                isBack = false;
            }
        }
    }
    public void SetCurEnemy(Enemy enemy)//设置目标敌人
    {
        curEnemy = enemy;
    }
    public void goMove()//发动攻击移动到敌人面前
    {     
        ani.SetInteger("role", 2);       
        isGo = true;
    }
    public void Back()
    {
        Idle();
        //transform.localScale = new Vector3(-1, 1, 1);
        isGo = false;
       // ani.SetInteger("labi", 2);
        isBack = true;
    }

    public void Skill()
    {
        ani.SetInteger("role", 4);
    }
    public void MaxSkill()
    {
        ani.SetInteger("role", 4);
    }   
    public void Idle()
    {
        ani.SetInteger("role", 1);
    }
    public void Attack()
    {
        ani.SetInteger("role", 3);
    }
    public void Run() { ani.SetInteger("role", 2); }
}   

public class MainRole
{
    public int MaxHp { get; set; }
    public int Hp { get; set; }
    public int PhyAttck { get; set; }
    public int MagAttack { get; set; }
    public int Level { get; set; }
    public int MaxExp { get; set; }
    public int Exp { get; set; }
    public int MaxShield { get; set; }
    public int Shield { get; set; }
    public int Money { get; set; }

    public MainRole()
    {
        MaxHp = 100;
        Hp = 100;
        PhyAttck = 10;
        MagAttack = 10;
        Level = 1;
        MaxExp = 100;
        Exp = 0;
        MaxShield = 50;
        Shield = 50;
        Money = 0;
    }

    public void Hurt(int hurt)
    {
        if (Shield > hurt) { Shield -= hurt; }
        if (Shield < hurt) { Hp -= hurt - Shield; Shield = 0; }
        if (Hp <= 0) { Hp = 0; }
        RoleInfoMgr.Instance.hpValue.maxValue = MaxHp;
        RoleInfoMgr.Instance.hpValue.value =Hp;
        RoleInfoMgr.Instance.shieldValue.maxValue = MaxShield;
        RoleInfoMgr.Instance.shieldValue.value =Shield;
    }

    public  void Levelup()//升级方法
    {
        if (Exp >= MaxExp)
        {
            Debug.Log("升级");
            Level++;
            if (Level < 5)
            {
                MaxHp += 50;
                MaxShield += 30;
                Hp = MaxHp;
                Shield = MaxShield;
                Exp = 0;
                MaxExp += 100;
            }
            else
            {
                MaxHp += 80;
                MaxShield += 50;
                Hp = MaxHp;
                Shield = MaxShield;
                Exp = 0;
                MaxExp += 200;
            }
        }
    }

    public void ShieldUp(int number)//恢复防御值方法
    {
        if (number < MaxShield - Shield && number >0)
        {
            Shield += number;
            FightMgr.Instance.ableShield(number);
        }
        else 
        {
            FightMgr.Instance.ableShield(MaxShield - Shield);
            Shield = MaxShield;      
        }
        RoleInfoMgr.Instance.shieldValue.maxValue = MaxShield;
        RoleInfoMgr.Instance.shieldValue.value = Shield;
    } 

    public void HpUp(int number)//恢复血量方法
    {
        if (number < MaxHp - Hp && number > 0)
        {
            FightMgr.Instance.ableHealth(number);
            Hp += number;
        }
        else
        {
            FightMgr.Instance.ableHealth(MaxHp - Hp);
            Hp = MaxHp;
        }
        RoleInfoMgr.Instance.hpValue.maxValue = MaxHp;
        RoleInfoMgr.Instance.hpValue.value = Hp;
    }
}