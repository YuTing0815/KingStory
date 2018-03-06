using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FailOrSuccess : Singleton<FailOrSuccess>
{
    GameObject _successPanel;
    Text _coinAddCount;
    Text _expAddValue;
    Button backToMainMapSuccess;
    GameObject _failPanel;
    Button backToMainMapFail;

    //游戏战斗成功  面板显示
    public void InitSuccess()
    {
        _successPanel = UIManager.Instance.AddUI("Prafbs/UI/PlayingMenu/successPanel",UILayer.Normal);
        _coinAddCount = _successPanel.gameObject.FindComponent<Text>("coin/coinAddCounts");
        _expAddValue = _successPanel.gameObject.FindComponent<Text>("exp/expAddValue");
        backToMainMapSuccess = _successPanel.gameObject.FindComponent<Button>("btnBackToMainMap");
        backToMainMapSuccess.onClick.AddListener(OnBtnBackToMainMapSuccess);
        int[] value = RewardModel.Instance.VectorReward();
        _coinAddCount.text = value[0].ToString();
        _expAddValue.text = value[1].ToString();
        RoleMgr.Instance.Money += value[0];
        RoleMgr.Instance.Role.Exp += value[1];
        RoleMgr.Instance.Role.Levelup();
    }

    //游戏战斗失败  面板显示
    public void InitFail()
    {
        _failPanel = UIManager.Instance.AddUI("Prafbs/UI/PlayingMenu/failPanel", UILayer.Normal);
        backToMainMapFail = _failPanel.gameObject.FindComponent<Button>("btnBackToMainMap");
        backToMainMapFail.onClick.AddListener(OnBtnBackMainMapFail);
    }

    //游戏战斗失败  按钮事件响应
    private void OnBtnBackMainMapFail()
    {
        UIManager.Instance.Release(_failPanel);
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

    }

    //游戏战斗成功 按钮事件响应
    private void OnBtnBackToMainMapSuccess()
    {
        UIManager.Instance.Release(_successPanel);
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
    }



}

