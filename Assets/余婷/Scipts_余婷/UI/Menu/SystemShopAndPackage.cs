using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEditor;

//商城、背包、属性、装备系统
public class SystemShopAndPackage
{
    public GameObject _uiRoot;
    Button package;
    Button equip;
    Button shop;
    Button shuXing;
    Vector3 startPos;
    public Action PackageCallBack;
    public Action EquipCallBack;
    public Action ShopCallBack;
    public Action ShuXingCallBack;
    public void Init()
    {
        _uiRoot = UIManager.Instance.AddUI("Prafbs/UI/PlayingMenu/system", UILayer.MenuPanel);
        startPos = _uiRoot.transform.position;
        package = _uiRoot.FindComponent<Button>("Package");
        equip = _uiRoot.FindComponent<Button>("Equip");
        shop = _uiRoot.FindComponent<Button>("Shop");
        shuXing = _uiRoot.FindComponent<Button>("ShuXing");
        package.onClick.AddListener(OnBtnPackageClick);
        equip.onClick.AddListener(OnBtnEquipClick);
        shop.onClick.AddListener(OnBtnShopClick);
        shuXing.onClick.AddListener(OnBtnShuXingClick);
        ComeDoTweem();
        //var autoClose = _uiRoot.AddComponent<TouchOutSideEx>();
        //autoClose.OutSideCallback = BackDoTweem;
    }

    private void OnBtnShuXingClick()
    {
        if (ShuXingCallBack!=null)
        {
            ShuXingCallBack();
        }
        BackDoTweem();
      var shuXing=  UIManager.Instance.AddUI("Prafbs/UI/PlayingMenu/role/ShuXing",UILayer.Normal);
        if (shuXing.GetComponent<ShuXingMgr>()==null)
        {
            shuXing.AddComponent<ShuXingMgr>().Init();
        }
        else
        {
            shuXing.GetComponent<ShuXingMgr>().Init();
        }
    }

    private void OnBtnShopClick()
    {
        if (ShopCallBack != null)
        {
            ShopCallBack();
        }
        BackDoTweem();
        var shopUi = UIManager.Instance.AddUI("Prafbs/UI/Shop/shop", UILayer.Normal);
        if (shuXing.GetComponent<ShopMgr>() == null)
        {
            shopUi.AddComponent<ShopMgr>().Init();
        }
        else
        {
            shopUi.GetComponent<ShopMgr>().Init();
        }
    }

    private void OnBtnEquipClick()
    {
        if (EquipCallBack != null)
        {
            EquipCallBack();
        }
        BackDoTweem();
        var shopUi = UIManager.Instance.AddUI("Prafbs/UI/Equip/Equip", UILayer.Normal);
        if (shuXing.GetComponent<EquipMgr>() == null)
        {
            shopUi.AddComponent<EquipMgr>().Init();
        }
        else
        {
            shopUi.GetComponent<EquipMgr>().Init();
        }
    }

    private void OnBtnPackageClick()
    {
        if (PackageCallBack != null)
        {
            PackageCallBack();
        }
        BackDoTweem();
        ClosePackegeBtn.Instance.Init();
    }
    public void ComeDoTweem()
    {
        Tweening.Instance.ComeDoTweenAnimationDOLocalMove(_uiRoot,new Vector3(0,-310f,0));
        Debug.Log("SystemComeTo");
        Debug.Log(Time.timeScale);
    }

    public void BackDoTweem()
    {
        Tweening.Instance.BackDoTweenAnimationDOMove(_uiRoot, startPos);
    }
}