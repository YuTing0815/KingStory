using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPiece : MonoBehaviour {
    //控制不同图片和颜色
    public enum ColorType
    {
        SWORD,
        SHIELD,
        MAGIC,
        HEART,
        ANY,
        COUNT
    };
    [System.Serializable]
    public struct ColorSprite
    {
        public ColorType color;
        public Sprite sprite;
    }
    public ColorSprite[] colorSprites;//保存其他物品图片，用于匹配
    private ColorType color;//修改不同颜色
    public ColorType Color
    {
        get
        {
            return color;
        }

        set
        {
            SetColor(value);//使用该方法修改颜色
        }
    }
    public int NumColors
    {
        get { return colorSprites.Length; }
    }

   public SpriteRenderer sprite;

    Dictionary<ColorType, Sprite> colorSpriteDict;//键为枚举，值为不同物品图片

    void Awake () {
        sprite = transform.GetComponent<SpriteRenderer>();//获取格子背景的渲染器
        colorSpriteDict = new Dictionary<ColorType, Sprite>();
               
            for (int i = 0; i < colorSprites.Length; i++)
            {
                if (!colorSpriteDict.ContainsKey(colorSprites[i].color))
                {
                    colorSpriteDict.Add(colorSprites[i].color, colorSprites[i].sprite);
                }
            }
        }
	
   public void SetColor(ColorType newColor)//设置颜色（图片）
    {
        color = newColor;
        if (colorSpriteDict.ContainsKey(newColor))//如果字典中存在这种颜色，则为其赋值sprite
        {
            sprite.sprite = colorSpriteDict[newColor];
        }
    }

}
