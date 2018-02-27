using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//处理动画
public class Tweening : Singleton<Tweening>
{

    public void ComeDoTweenAnimationDOLocalMove(GameObject _uiRoot,Vector3 pos)
    {
     _uiRoot.GetComponent<RectTransform>().DOLocalMove(pos,0.3f);

    }
    public void BackDoTweenAnimationDOLocalMove(GameObject _uiRoot, Vector3 pos)
    {
        _uiRoot.GetComponent<RectTransform>().DOMove(pos, 0.3f);
    }
    public void BackDoTweenAnimationDOMove(GameObject _uiRoot, Vector3 pos)
    {
        _uiRoot.GetComponent<RectTransform>().DOMove(pos, 0.3f);
    }

    public void ComeDoTweenAnimationDoScale(GameObject _uiRoot, Vector3 pos)
    {
        _uiRoot.GetComponent<RectTransform>().DOScale(pos, 0.3f);
    }

    public void BackDoTweenAnimationDoScale(GameObject _uiRoot, Vector3 pos)
    {
        _uiRoot.GetComponent<RectTransform>().DOScale(pos, 0.3f);
    }
}
