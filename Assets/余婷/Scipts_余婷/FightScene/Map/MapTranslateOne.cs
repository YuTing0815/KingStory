using UnityEngine;
using System.Collections;
using DG.Tweening;
public class MapTranslateOne : MonoBehaviour
{
    private static MapTranslateOne _instance;
    public static MapTranslateOne Instance
    { get { return _instance; } }
    void Awake()
    {
        _instance = this;
        modelPlaceObj = gameObject.FindComponent<Transform>("ModelPlace").gameObject;
        var mapPath = MapTableOne.Instance[SaveIndexMgr.Instance.CurindexMapOne + 1].MapPath;
        mapObj = GameObject.Instantiate(Resources.Load(mapPath)) as GameObject;
        mapObj.transform.SetParent(modelPlaceObj.transform);
        RoleInfoMgr.Instance.Init();
    }
    public bool isTranslate = false;
    AudioSource audioSource;
    GameObject modelPlaceObj;//摄影棚放置节点
    GameObject mapObj;//实例化的地图
    Vector3 pos;
    public void Start()
    {
        pos = modelPlaceObj.transform.position;
        //Destory();
        audioSource = mapObj.GetComponent<AudioSource>();
        audioSource.Play();
    }

    public void Translate()
    {
        modelPlaceObj.transform.DOMove(pos, 1.8f).SetEase(Ease.Linear).OnComplete(ControlMapMove);
    }

    private void ControlMapMove()
    {
        isTranslate = false;
    }

    void Update()
    {
        if (isTranslate == true)
        {
            Translate();
        }

    }
    public void MapRun()
    {
        pos = modelPlaceObj.transform.position - new Vector3(12, 0, 0);
        isTranslate = true;
    }

    //public void Destory()
    //{
    //    GameObject roleUI = GameObject.Find("RoleUI");
    //    GameObject menuPanel = GameObject.Find("MenuPanel");
    //    GameObject Normol = GameObject.Find("Normal");
    //    roleUI.DestroyAllChildren();
    //    menuPanel.DestroyAllChildren();
    //    Normol.DestroyAllChildren();
    //}
}
