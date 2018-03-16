using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClosePackegeBtn:Singleton<ClosePackegeBtn>{

    //private static ClosePackegeBtn _instance;
    //public static ClosePackegeBtn Instance
    //{ get { return _instance; } }
    //void Awake()
    //{ _instance = this; }

    GameObject Package;
    Button btnClose;
    public void Init()
    {
        Package = UIManager.Instance.ReplaceUI("Packet", UILayer.Normal);
        btnClose = Package.gameObject.FindComponent<Button>("Canvas/btnClose");
        btnClose.onClick.AddListener(OnBtnClose);
        InventoryManager.Instance().UpdateBag();
    }

    private void OnBtnClose()
    {
        UIManager.Instance.Release(Package);
    }
}
