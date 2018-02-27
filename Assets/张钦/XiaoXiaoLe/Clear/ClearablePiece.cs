using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//消除的基类
public class ClearablePiece : MonoBehaviour {

    public AnimationClip clearAnimation;

    bool isBeingCleared = false;

    public bool IsBeingCleared
    {
        get
        {
            return isBeingCleared;
        }
    }

    protected GamePiece piece;//后期需扩展类，方便驱动类调用
    void Awake () {
        piece = GetComponent<GamePiece>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public virtual void Clear()
    {
        piece.GridRef.level.OnPieceCleared(piece);//消除时更新分数
        ClearMgr.Instance.save(piece);
        isBeingCleared = true;
        StartCoroutine(ClearCoroutine());
    }
    private IEnumerator ClearCoroutine()
    {
        Animator animator = GetComponent<Animator>();
        if (animator)
        {
            animator.Play(clearAnimation.name);
            yield return new WaitForSeconds(clearAnimation.length);//播放结束动画，播放完删除
            Destroy(gameObject);
        }
    }
}
