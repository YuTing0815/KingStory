using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//进入场景提示
public class Tishi
{
    Button btnGoTo;
    Button btnCancle;
    Text mapName;
    GameObject _uiRoot;

    GameObject _uiRootObj1;
    Button btnContinue1;
    GameObject _uiRootObj2;
    Button btnContinue2;
    GameObject _uiRootObj3;
    Button btnContinue3;
    public void Init(int index)
    {
        //DontUse();
        int num = UnityEngine.Random.Range(0, 2);
        if (num == 0)
        {
            if (_uiRoot != null)
            {
                ResMgr.Instance.Release(_uiRoot);
            }
            else
            {
                _uiRoot = UIManager.Instance.AddUI("Prafbs/UI/map/MainMap/TiShi", UILayer.Normal);
                btnCancle = _uiRoot.gameObject.FindComponent<Button>("btnCancel");
                btnGoTo = _uiRoot.gameObject.FindComponent<Button>("btnGoTo");
                mapName = _uiRoot.FindComponent<Text>("mapName");
                mapName.text = MapTable.Instance[index + 1].Name;
                btnCancle.onClick.AddListener(OnBtnCancleClick);
                btnGoTo.onClick.AddListener(() => OnBtnGoToClick(index,_uiRoot));
                //Time.timeScale = 0;
            }
        }
        if (num == 1)
        {
            int num1 = UnityEngine.Random.Range(1, 4);
            if (num1 == 1)
            {
                ShowTiShi(_uiRootObj1, 1, btnContinue1);
            }
            if (num1 == 2)
            {
                ShowTiShi(_uiRootObj2, 2, btnContinue2);
            }
            if (num1 == 3)
            {
                ShowTiShi(_uiRootObj3, 3, btnContinue3);
            }
        }
    }


    public void DontUse()
    {
        foreach (var item in WY.Instance.AllPointList)
        {
            item.interactable = false;
        }
    }
    public void CanUse()
    {
        foreach (var item in WY.Instance.AllPointList)
        {
            item.interactable = true;
        }
    }

    public void ShowTiShi(GameObject obj, int index, Button btn)
    {
        if (obj != null)
        {
            ResMgr.Instance.Release(obj);
        }
        else
        {
            obj = UIManager.Instance.AddUI("Prafbs/UI/map/MainMap/TiShi" + index, UILayer.Normal);
            btn = obj.gameObject.FindComponent<Button>("btnGoBack");
            btn.onClick.AddListener(() => OnBtnGoBackClick(obj));
            //Time.timeScale = 0;
        }
    }

    private void OnBtnGoBackClick(GameObject obj)
    {
        ResMgr.Instance.Release(obj);
        //Time.timeScale = 1;
        //CanUse();
        MenuMgr.Instance.DestoryImage();

    }

    private void OnBtnGoToClick(int mapIndex,GameObject obj)
    {
       // CanUse();
        ResMgr.Instance.Release(obj);
        //Time.timeScale = 1;
        SaveIndexMgr.Instance.Save(mapIndex);
        SceneManager.LoadScene("Scenes/GamePlaying");
        //MapTranslate.Instance.Init();
        MenuMgr.Instance.DestoryImage();

    }

    private void OnBtnCancleClick()
    {
        ResMgr.Instance.Release(_uiRoot);
        //Time.timeScale = 1;
        // CanUse();
        MenuMgr.Instance.DestoryImage();
    }
}