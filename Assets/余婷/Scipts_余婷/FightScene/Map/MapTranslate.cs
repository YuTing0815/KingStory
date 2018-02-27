using UnityEngine;
using System.Collections;

public class MapTranslate : MonoBehaviour
{
    private static MapTranslate _instance;
    public static MapTranslate Instance
    { get { return _instance; } }
    void Awake()
    {
        _instance = this;
        modelPlaceObj = gameObject.FindComponent<Transform>("ModelPlace").gameObject;
        var mapPath = MapTable.Instance[SaveIndexMgr.Instance.Curindex + 1].MapPath;
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
        modelPlaceObj.transform.position = Vector3.Lerp(modelPlaceObj.transform.position, pos, 0.0165f);
        if (Vector3.Distance(modelPlaceObj.transform.position, pos) < 0.1f)
        {
            modelPlaceObj.transform.position = pos;
            isTranslate = false;
        }
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
