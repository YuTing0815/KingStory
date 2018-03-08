using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class EquipMgr : MonoBehaviour
{
    private static EquipMgr _instance;
    public static EquipMgr Instance
    { get { return _instance; } }

    private RectTransform _equipImageRoot;
    private Image _toggleRoot;
    private Toggle toggleFaShi;
    private Toggle toggleZhanShi;
    private Toggle toggleMuShi;
    private Image imageFaShi;
    private Image imageZhanShi;
    private Image imageMuShi;
    private Button btnClose;
    public void Init()
    {
        _equipImageRoot = gameObject.FindComponent<RectTransform>("equipImageRoot");
        _toggleRoot = gameObject.FindComponent<Image>("ToggleCtrl");
        toggleFaShi = _toggleRoot.gameObject.FindComponent<Toggle>("Toggle1");
        toggleFaShi.onValueChanged.AddListener(ImageFaShiPanelShow);
        toggleMuShi = _toggleRoot.gameObject.FindComponent<Toggle>("Toggle3");
        toggleMuShi.onValueChanged.AddListener(ImageMuShiPanelShow);
        toggleZhanShi = _toggleRoot.gameObject.FindComponent<Toggle>("Toggle2");
        toggleZhanShi.onValueChanged.AddListener(ImageZhanShiPanelShow);
        imageFaShi = gameObject.FindComponent<Image>("faShi");
        imageMuShi = gameObject.FindComponent<Image>("muShi");
        imageMuShi.gameObject.SetActive(false);
        imageZhanShi = gameObject.FindComponent<Image>("zhanShi");
        imageZhanShi.gameObject.SetActive(false);
        btnClose = gameObject.FindComponent<Button>("btnClose");
        btnClose.onClick.AddListener(OnBtnCloseClick);
    }


    private void ShowCurEquip()
    {
        List<Image> equipImagesRoot=new List<Image>();
        for (int i = 0; i < _equipImageRoot.childCount; i++)
        {
            equipImagesRoot.Add(_equipImageRoot.transform.GetChild(i).GetComponent<Image>());
        }
        for (int i = 0; i < RoleMgr.Instance.CurEquip.Values.Count; i++)
        {
        }
    }

    private void OnBtnCloseClick()
    {
       gameObject.SetActive(false);
    }

    private void ImageMuShiPanelShow(bool isOn)
    {
        if (isOn)
        {
           imageMuShi.gameObject.SetActive(true);
            imageFaShi.gameObject.SetActive(false);
            imageZhanShi.gameObject.SetActive(false);
        }
    }

    private void ImageZhanShiPanelShow(bool isOn)
    {
        if (isOn)
        {
            imageMuShi.gameObject.SetActive(false);
            imageFaShi.gameObject.SetActive(false);
            imageZhanShi.gameObject.SetActive(true);
        }
    }

    private void ImageFaShiPanelShow(bool isOn)
    {
        if (isOn)
        {
           imageFaShi.gameObject.SetActive(true);
            imageMuShi.gameObject.SetActive(false);
            imageZhanShi.gameObject.SetActive(false);
        }
    }
}
