using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ToolTip : MonoBehaviour {
    //用于控制提示框的显示与否
    //被InventoryManager调用
    Text tooltipText;
    Text ContentText;//使用canvasGroup组件来控制提示框的显示和隐藏
    CanvasGroup canvasGroup;
    float targetAlpha = 0;//透明度

    void Start()
    {
        tooltipText = GetComponent<Text>();
        ContentText = transform.Find("Text").GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    void Update()
    {
        if (canvasGroup.alpha != targetAlpha)//若当前提示框透明度不为目标透明度，则渐变到目标透明度
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha,targetAlpha,Time.deltaTime*3f);
            if (Mathf.Abs(canvasGroup.alpha - targetAlpha) < 0.01f)
            {
                canvasGroup.alpha = targetAlpha;
            }
        }
    }
    public void Show(string text)
    {
        // canvasGroup.alpha = targetAlpha;
        tooltipText.text = text;
        ContentText.text = text;
        targetAlpha = 1;
    }

    public void Hide()
    {
        //canvasGroup.alpha = targetAlpha;
        targetAlpha = 0;
    }

    public void SetLocalPosition(Vector3 position)
    {
        transform.localPosition = position;
    }
}
