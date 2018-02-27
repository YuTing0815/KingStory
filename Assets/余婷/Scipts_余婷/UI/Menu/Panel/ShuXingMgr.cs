using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
//属性面板管理器
public class ShuXingMgr : MonoBehaviour
{
    private static ShuXingMgr _instance;
    public static ShuXingMgr Instance
    { get { return _instance; } }
    void Awake() { _instance = this; }
    //需要控制的外部变量
    public int Level;//玩家等级
    public string Name;//玩家姓名
    public int Hp;//玩家现有血量
    public int MaxHp;//玩家最大血量
    public int HpValue;//玩家血量条
    public int Shield;//玩家现有防御值
    public int MaxShield;//玩家最大防御值
    public int ShieldValue;//玩家防御条
    public int Exp;//玩家现有经验值
    public int MaxExp;//玩家最大经验值
    public int ExpValue;//玩家经验条
    public int Mg;//玩家属性法师现有魔法值
    public int MgValue;//玩家属性法师魔法条
    public int Phy;//玩家属性战士现有物攻值
    public int PhyValue;//玩家属性战士物攻条
    public int Belief;//玩家属性牧师现有信仰值
    public int BeliefValue;//玩家属性牧师信仰条

    public void ShuXing()
    {
        MainRole role = RoleMgr.Instance.Role;
        Debug.Log(123);
        Level = role.Level;
        Hp = role.Hp;
        HpValue = role.Hp;
        MaxHp= role.MaxHp;
        Shield = role.Shield;
        ShieldValue = role.Shield;
        MaxShield = role.MaxShield;
        Exp = role.Exp;
        ExpValue= role.Exp;
        MaxExp = role.MaxExp;
        Mg = role.MagAttack;
        MgValue = role.MagAttack;
        Phy = role.PhyAttck;
        PhyValue = role.PhyAttck;
        Belief = role.MagAttack;
        BeliefValue = role.MagAttack;
        Debug.Log(MgValue + " " + PhyValue + " " + BeliefValue);
    }
    //关闭页面按钮
    Button btnClose;

    //三个面板toggle
    Toggle faShiToggle;
    Toggle zhanShiToggle;
    Toggle muShiToggle;

    RectTransform faShiPanel;
    Toggle faShiSkill1;
    Toggle faShiSkill2;
    Toggle faShiSkill3;
    Text faShiSkill1Text;
    Text faShiSkill2Text;
    Text faShiSkill3Text;

    RectTransform zhanShiPanel;
    Toggle zhanShiSkill1;
    Toggle zhanShiSkill2;
    Toggle zhanShiSkill3;
    Text zhanShiSkill1Text;
    Text zhanShiSkill2Text;
    Text zhanShiSkill3Text;

    RectTransform muShiPanel;
    Toggle muShiSkill1;
    Toggle muShiSkill2;
    Toggle muShiSkill3;
    Text muShiSkill1Text;
    Text muShiSkill2Text;
    ScrollRect muShiSkill3Text;



    // Use this for initialization
    public void Init()
    {
        ShuXing();//为人物属性赋值
        #region  //面板上方属性值
        gameObject.FindComponent<Text>("other/LevelValue").text = Level.ToString();
        gameObject.FindComponent<Text>("other/HpBg/hpNum").text = Hp.ToString();
        gameObject.FindComponent<Text>("other/HpBg/MaxHp").text = MaxHp.ToString();
        gameObject.FindComponent<Slider>("other/HpBg").value = HpValue;
        gameObject.FindComponent<Text>("other/shieldBg/shieldNum").text = Shield.ToString();
        gameObject.FindComponent<Text>("other/shieldBg/MaxShield").text = MaxShield.ToString();
        gameObject.FindComponent<Slider>("other/shieldBg").value = ShieldValue;
        gameObject.FindComponent<Text>("other/ExBg/ExNum").text = Exp.ToString();
        gameObject.FindComponent<Text>("other/ExBg/MaxEx").text = MaxExp.ToString();
        gameObject.FindComponent<Slider>("other/ExBg").value = ExpValue;
        gameObject.FindComponent<Text>("FaShi/MgValue/MgBg/hpNum").text = Mg.ToString();
        gameObject.FindComponent<Slider>("FaShi/MgValue/MgBg").value = MgValue;
        gameObject.FindComponent<Text>("ZhanShi/PyValue/PyBg/pyNum").text = Phy.ToString();
        gameObject.FindComponent<Slider>("ZhanShi/PyValue/PyBg").value = PhyValue;
        gameObject.FindComponent<Text>("MuShi/ShValue/ShBg/shNum").text = Belief.ToString();
        gameObject.FindComponent<Slider>("MuShi/ShValue/ShBg").value = BeliefValue;
        #endregion

        #region  //toggle切换和事件响应
        faShiToggle = gameObject.FindComponent<Toggle>("ToggleCtrl/Toggle1");
        zhanShiToggle = gameObject.FindComponent<Toggle>("ToggleCtrl/Toggle2");
        muShiToggle = gameObject.FindComponent<Toggle>
            ("ToggleCtrl/Toggle3");
        #endregion

        #region  //三个面板的技能介绍
        //法师
        faShiPanel = gameObject.FindComponent<RectTransform>("FaShi");
        faShiSkill1 = faShiPanel.gameObject.FindComponent<Toggle>("Skill/skill1");
        faShiSkill2 = faShiPanel.gameObject.FindComponent<Toggle>
            ("Skill/skill2");
        faShiSkill3 = faShiPanel.gameObject.FindComponent<Toggle>("Skill/skill3");
        faShiSkill1Text = faShiPanel.gameObject.FindComponent<Text>("Skill/skill1/Text");
        faShiSkill2Text = faShiPanel.gameObject.FindComponent<Text>
        ("Skill/skill2/Text");
        faShiSkill3Text = faShiPanel.gameObject.FindComponent<Text>("Skill/skill3/Text");
        faShiSkill1.onValueChanged.AddListener(OnChangeFaShiSkill1);
        faShiSkill2.onValueChanged.AddListener
        (OnChangeFaShiSkill2);
        faShiSkill3.onValueChanged.AddListener(OnChangeFaShiSkill3);


        //战士
        zhanShiPanel = gameObject.FindComponent<RectTransform>("ZhanShi");
        zhanShiSkill1 = zhanShiPanel.gameObject.FindComponent<Toggle>("Skill/skill1");
        zhanShiSkill2 = zhanShiPanel.gameObject.FindComponent<Toggle>
            ("Skill/skill2");
        zhanShiSkill3 = zhanShiPanel.gameObject.FindComponent<Toggle>("Skill/skill3");
        zhanShiSkill1Text = zhanShiPanel.gameObject.FindComponent<Text>("Skill/skill1/Text");
        zhanShiSkill2Text = zhanShiPanel.gameObject.FindComponent<Text>
        ("Skill/skill2/Text");
        zhanShiSkill3Text = zhanShiPanel.gameObject.FindComponent<Text>("Skill/skill3/Text");
        zhanShiSkill1.onValueChanged.AddListener(OnChangeZhanShiSkill1);
        zhanShiSkill2.onValueChanged.AddListener
        (OnChangeZhanShiSkill2);
        zhanShiSkill3.onValueChanged.AddListener(OnChangeZhanShiSkill3);



        //牧师
        muShiPanel = gameObject.FindComponent<RectTransform>("MuShi");
        muShiSkill1 = muShiPanel.gameObject.FindComponent<Toggle>("Skill/skill1");
        muShiSkill2 = muShiPanel.gameObject.FindComponent<Toggle>
            ("Skill/skill2");
        muShiSkill3 = muShiPanel.gameObject.FindComponent<Toggle>("Skill/skill3");
        muShiSkill1Text = muShiPanel.gameObject.FindComponent<Text>("Skill/skill1/Text");
        muShiSkill2Text = muShiPanel.gameObject.FindComponent<Text>
        ("Skill/skill2/Text");
        muShiSkill3Text = muShiPanel.gameObject.FindComponent<ScrollRect>("Skill/skill3/Text");

        muShiSkill1.onValueChanged.AddListener(OnChangeMuShiSkill1);
        muShiSkill2.onValueChanged.AddListener
        (OnChangeMuShiSkill2);
        muShiSkill3.onValueChanged.AddListener(OnChangeMuShiSkill3);
        #endregion



        //三个面板切换事件响应
        faShiToggle.onValueChanged.AddListener(OnChangeFaShiToggle);
        zhanShiToggle.onValueChanged.AddListener(OnChangeZhanShiToggle);
        muShiToggle.onValueChanged.AddListener(OnChangeMuShiToggle);


        btnClose = gameObject.FindComponent<Button>("btnClose");
        btnClose.onClick.AddListener(OnBtnCloseClick);
    }
    //关闭按钮
    private void OnBtnCloseClick()
    {
        UIManager.Instance.Release(gameObject);
    }

    private void OnChangeMuShiSkill3(bool isOn)
    {
        muShiSkill3Text.gameObject.SetActive(isOn);
    }

    private void OnChangeMuShiSkill2(bool isOn)
    {
        muShiSkill2Text.gameObject.SetActive(isOn);
    }

    private void OnChangeMuShiSkill1(bool isOn)
    {
        muShiSkill1Text.gameObject.SetActive(isOn);
    }

    //战士面板技能文字介绍
    private void OnChangeZhanShiSkill3(bool isOn)
    {
        zhanShiSkill3Text.gameObject.SetActive(isOn);
    }

    private void OnChangeZhanShiSkill2(bool isOn)
    {
        zhanShiSkill2Text.gameObject.SetActive(isOn);
    }

    private void OnChangeZhanShiSkill1(bool isOn)
    {
        zhanShiSkill1Text.gameObject.SetActive(isOn);
    }

    //法师面板技能文字介绍
    private void OnChangeFaShiSkill3(bool isOn)
    {
        faShiSkill3Text.gameObject.SetActive(isOn);
    }

    private void OnChangeFaShiSkill2(bool isOn)
    {
        faShiSkill2Text.gameObject.SetActive(isOn);
    }

    private void OnChangeFaShiSkill1(bool isOn)
    {
        faShiSkill1Text.gameObject.SetActive(isOn);
    }

    //三个面板切换响应事件
    private void OnChangeMuShiToggle(bool isOn)
    {
        muShiPanel.gameObject.SetActive(isOn);
    }

    private void OnChangeZhanShiToggle(bool isOn)
    {
        zhanShiPanel.gameObject.SetActive(isOn);
    }

    private void OnChangeFaShiToggle(bool isOn)
    {
        faShiPanel.gameObject.SetActive(isOn);
    }
}
