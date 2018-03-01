using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class WY : MonoBehaviour
{
    AudioSource ani;
    private static WY _instance;
    public static WY Instance
    { get { return _instance; } }
    Tishi tiShi;
    void Awake()
    { _instance = this; }

    public List<Button> AllPointList = new List<Button>();

    GameObject Zj;
    bool Shi = false;
    Button Bt;
    public int pointIndex = 0;
    // Use this for initialization
    void Start()
    {
        ani = gameObject.GetComponent<AudioSource>();
        ani.Play();
        UIManager.Instance.Init();
        MenuMgr.Instance.Init();
        tiShi = new Tishi();
        Zj = GameObject.FindGameObjectWithTag("zhuJue").gameObject;
        if (SaveIndexMgr.Instance.Curindex != -1)
        {
            Zj.transform.position = AllPointList[SaveIndexMgr.Instance.Curindex].transform.position;
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

    private void Yd(int Index)
    {
        int chaValue = Index - pointIndex;
        if (chaValue >= 0 && chaValue <= 1 || chaValue >= -1 && chaValue <= 0)
        {
            pointIndex = Index;
            SaveIndexMgr.Instance.Save(pointIndex);
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


