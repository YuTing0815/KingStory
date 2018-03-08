using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//战斗页面角色信息和回合数的控制
public class RoleInfoMgr : Singleton<RoleInfoMgr>
{
    GameObject _uiRoot;
    public Text timeRound;
    public Text maxTimeRound;
    public Text coinCounts;
    public Text diamondCounts;
    public Text userName;
    public Slider hpValue;
    public Slider shieldValue;
    Button btnBackMainMap;

    //人物信息、回合数初始化
    public void Init()
    {
        _uiRoot = UIManager.Instance.ReplaceUI("Prafbs/UI/PlayingMenu/role/roleInfo",UILayer.MenuPanel);
        hpValue = _uiRoot.gameObject.FindComponent<Slider>("hpValue");
        shieldValue = _uiRoot.gameObject.FindComponent<Slider>("shieldValue");
        timeRound = _uiRoot.gameObject.FindComponent<Text>("HuiHe/timeRound");
        maxTimeRound = _uiRoot.gameObject.FindComponent<Text>("HuiHe/maxTimeRound");
        coinCounts = _uiRoot.gameObject.FindComponent<Text>("Coin/counts");
        coinCounts.text = RoleMgr.Instance.Money.ToString();
        diamondCounts = _uiRoot.gameObject.FindComponent<Text>("Diamond/counts");
        diamondCounts.text = RoleMgr.Instance.Diamond.ToString();
        btnBackMainMap = _uiRoot.gameObject.FindComponent<Button>("btnBackMainMap");
        btnBackMainMap.onClick.AddListener(OnBtnBackMainMap);
        userName = _uiRoot.gameObject.FindComponent<Text>("userName");
        userName.text = LoginAndReigsterInfo.Instance.CurUserName;
    }

    private void OnBtnBackMainMap()
    {
        if (SaveIndexMgr.Instance.SaveSceneId == 1)
        {
            SceneManager.LoadScene("Scenes/MapOne");
        }
        if (SaveIndexMgr.Instance.SaveSceneId == 2)
        {
            SceneManager.LoadScene("Scenes/MapTwo");
        }
        if (SaveIndexMgr.Instance.SaveSceneId == 3)
        {
            SceneManager.LoadScene("Scenes/MapThree");
        }
        if (SaveIndexMgr.Instance.SaveSceneId == 4)
        {
            SceneManager.LoadScene("Scenes/MapFour");
        }
        UIManager.Instance.Release(_uiRoot);
    }
}
