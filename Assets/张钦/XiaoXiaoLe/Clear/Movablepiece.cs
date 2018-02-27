using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movablepiece : MonoBehaviour {

    //使元素移动
    GamePiece piece;
    IEnumerator moveCoroutine;//使用协程来产生帧动画的效果
	void Awake () {
        piece = GetComponent<GamePiece>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Move(int newX, int newY,float time)//移动位置
    {
        //piece.transform.localPosition = piece.GridRef.GetWorldPosition(newX, newY);//当前物体通过grid 的方法获得新的坐标
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = MoveCoroutine(newX, newY,time);
        StartCoroutine(moveCoroutine);
    }

    IEnumerator MoveCoroutine(int newX,int newY,float time)//协程使用差值动画控制自身移动
    {
        piece.X = newX;
        piece.Y = newY;
        Vector3 startPos = transform.position;
        Vector3 endPos = piece.GridRef.GetWorldPosition(newX, newY);
        for (float t = 0; t < 1 * time; t += Time.deltaTime)
        {
            piece.transform.position = Vector3.Lerp(startPos, endPos, t / time);
            yield return 0;//返回0则为1帧
        }
        piece.transform.position = endPos;
    }
}
