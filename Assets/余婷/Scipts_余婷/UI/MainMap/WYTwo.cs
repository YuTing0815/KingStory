using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WYTwo : MonoBehaviour
{
    AudioSource ani;
    private static WYTwo _instance;
    public static WYTwo Instance
    { get { return _instance; } }
    TishiTwo tiShi;
    void Awake()
    { _instance = this; }

    public List<Button> AllPointList = new List<Button>();

    GameObject Zj;
    bool Shi = false;
    Button Bt;
    public int pointIndex = 0;
    private Button _returnScene;
    // Use this for initialization
    void Start()
    {
        SaveIndexMgr.Instance.SaveSceneId = 2;
        ani = gameObject.GetComponent<AudioSource>();
        ani.Play();
        UIManager.Instance.Init();
        MenuMgr.Instance.Init();
        tiShi = new TishiTwo();
        Zj = GameObject.FindGameObjectWithTag("zhuJue").gameObject;
        _returnScene = gameObject.FindComponent<Button>("ReturnScene");
        _returnScene.onClick.AddListener(ReturnScenePanel);
        if (SaveIndexMgr.Instance.CurindexMapOne != -1)
        {
            Zj.transform.position = AllPointList[SaveIndexMgr.Instance.CurindexMapTwo].transform.position;
        }
        AllPointList[0].onClick.AddListener(() => Yd(0));
        AllPointList[1].onClick.AddListener(() => Yd(1));
        AllPointList[2].onClick.AddListener(() => Yd(2));
        AllPointList[3].onClick.AddListener(() => Yd(3));
        AllPointList[4].onClick.AddListener(() => Yd(4));
        AllPointList[5].onClick.AddListener(() => Yd(5));
        AllPointList[6].onClick.AddListener(() => Yd(6));
        AllPointList[7].onClick.AddListener(() => Yd(7));
        AllPointList[8].onClick.AddListener(() => Yd(8));
        AllPointList[9].onClick.AddListener(() => Yd(9));
    }
    private void ReturnScenePanel()
    {
        MenuMgr.Instance.ReleseMenu();
        SceneManager.LoadScene("Scenes/Login");
    }

    private void Yd(int Index)
    {
        int chaValue = Index - pointIndex;
        if (chaValue >= 0 && chaValue <= 1 || chaValue >= -1 && chaValue <= 0)
        {
            pointIndex = Index;
            SaveIndexMgr.Instance.SaveMapTwo(pointIndex);
            Debug.Log(pointIndex);
            Shi = true;
            if (Shi == true)
            {
                MenuMgr.Instance.CoverImage();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Shi)
        {
            Zj.transform.position = Vector3.Lerp(Zj.transform.position, AllPointList[pointIndex].gameObject.transform.position, 0.05f);
            if (Vector3.Distance(Zj.transform.position, AllPointList[pointIndex].gameObject.transform.position) < 0.5f)
            {
                Shi = false;
                Zj.transform.position = AllPointList[pointIndex].gameObject.transform.position;
                tiShi.Init(pointIndex);
            }
        }
    }
}


