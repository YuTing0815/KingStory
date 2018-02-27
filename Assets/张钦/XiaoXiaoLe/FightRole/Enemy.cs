using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface Human { }


public class Enemy : MonoBehaviour,Human {
    public Role role;
    public EnemyMan enemy;//自身属性
    public bool isDead;
    Vector3 pos;//自身位置
    Animator ani;
    bool isGo;
    bool isBack;
    public Slider Hp;
    public int isBoss = 0;
    public Text Round;//控制显示回合数
    public Text hurt;
    void Awake ()
    {
        role = FightMgr.Instance.rolelist[0];
        pos = transform.localPosition;
        ani = GetComponent<Animator>();
        Hp = transform.Find("Canvas").transform.Find("Slider").GetComponent<Slider>();
        Round = transform.Find("Canvas").transform.Find("Image").transform.Find("text").GetComponent<Text>();
        hurt = transform.Find("Canvas").transform.Find("hurt").GetComponent<Text>();
        FEnable();

    }
	
	
	void Update () {
        if (isGo)
        {
            Debug.Log("移动");
            transform.localPosition = Vector2.Lerp(transform.localPosition, role.transform.localPosition, 0.1f);
            if (Vector2.Distance(role.transform.localPosition, transform.localPosition) < 1.2)
            {
                //ani.SetInteger("labi", 3);
                ani.SetInteger("role", 3);
                isGo = false;
            }
        }

        if (isBack)
        {
            Debug.Log("返回位置");
            transform.localPosition = Vector2.Lerp(transform.localPosition, pos, 0.1f);
            if (Vector2.Distance(transform.localPosition,pos) < 0.1)
            {
               // ani.SetInteger("labi", 0);
                transform.position =pos;
                isBack = false;
            }
        }
    }

    public void goMove()//发动攻击移动到主角面前
    {       
        ani.SetInteger("role", 2);
        isGo = true;
    }

    public void Back()
    {
        ani.SetInteger("role", 1);     
        isBack = true;
    }

    public void Init(int Level)
    {
        enemy = new EnemyMan(Level);
        SoliderHp();
        Round.text = enemy.Rounds.ToString();
    }
    public bool subRounds()
    {
        if (enemy.Rounds >= 1)
        {
            enemy.Rounds--;
            Round.text = enemy.Rounds.ToString();
        }
        if (enemy.Rounds == 0)
        {
            enemy.Rounds = enemy.MaxRounds;
            Round.text = enemy.Rounds.ToString();
            return true;
        }
        return false;
    }
    public void Hurt(int hurt)
    {
        this.hurt.text = "-"+hurt;
        TEnable();
        Invoke("FEnable", 1);
        enemy.Hurt(hurt);
        if (enemy.Hp <= 0)
        {
            isDead = true;
        }
        SoliderHp();
    }

    public void SoliderHp()
    {
        Hp.maxValue = enemy.MaxHp;
        Hp.value = enemy.Hp;
    }

    public void TEnable()
    {
        hurt.enabled = true;
    }
    public void FEnable()
    {
        hurt.enabled = false;
    }
}

public class EnemyMan
{
    public int MaxHp { get; set; }
    public int Hp { get; set; }
    public int PhyAttck { get; set; }
    public int MagAttack { get; set; }
    public int MaxRounds { get; set; }
    public int Rounds { get; set; }
   
    public EnemyMan(int Level)
    {
        MaxHp = 100;// + (Level) * 100;
        Hp = MaxHp;
        PhyAttck = 10 + (Level - 1) * 6;
        MagAttack = PhyAttck;
        MaxRounds = UnityEngine.Random.Range(1, 4);
        Rounds = MaxRounds;
    }

    public EnemyMan(int maxHp, int hp, int phyAttck, int magAttack, int maxRounds, int rounds)
    {
        MaxHp = maxHp;
        Hp = hp;
        PhyAttck = phyAttck;
        MagAttack = magAttack;
        MaxRounds = maxRounds;
        Rounds = rounds;
    }

    public void Hurt(int hurt)
    {
        //Debug.Log("对敌人造成伤害" + hurt);
        if (Hp > 0 && hurt >= 0)
        {
            Hp -= hurt;
        }
        
    }
}
